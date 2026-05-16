using CryptoTool.Algorithm.Algorithms.SM2;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Math;
using System.Text;

namespace CryptoTool.Win.Helpers;

internal static class Sm2UiHelper
{
    private const int Sm2SignatureComponentLength = 32;
    private const int Sm2RsSignatureLength = Sm2SignatureComponentLength * 2;

    public static (string? PublicKey, string? PrivateKey) ParseKeyFileContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return (null, null);

        var lines = content
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !string.IsNullOrWhiteSpace(line))
            .ToList();

        string? publicKey = null;
        string? privateKey = null;

        for (var i = 0; i < lines.Count; i++)
        {
            if (TryReadLabeledValue(lines, ref i, "公钥", "publickey", out var parsedPublicKey))
            {
                publicKey ??= parsedPublicKey;
                continue;
            }

            if (TryReadLabeledValue(lines, ref i, "私钥", "privatekey", out var parsedPrivateKey))
            {
                privateKey ??= parsedPrivateKey;
            }
        }

        var unlabeledLines = lines
            .Where(line => !IsLabelLine(line, "公钥", "publickey") && !IsLabelLine(line, "私钥", "privatekey"))
            .ToList();

        publicKey ??= unlabeledLines.ElementAtOrDefault(0);
        privateKey ??= unlabeledLines.ElementAtOrDefault(1);

        return (publicKey, privateKey);
    }

    public static string BuildKeyFileContent(string publicKey, string privateKey)
    {
        if (string.IsNullOrWhiteSpace(publicKey))
            throw new ArgumentException("公钥不能为空", nameof(publicKey));

        if (string.IsNullOrWhiteSpace(privateKey))
            throw new ArgumentException("私钥不能为空", nameof(privateKey));

        var builder = new StringBuilder();
        builder.AppendLine("公钥：");
        builder.AppendLine(publicKey.Trim());
        builder.AppendLine();
        builder.AppendLine("私钥：");
        builder.AppendLine(privateKey.Trim());
        return builder.ToString();
    }

    public static SM2CipherFormat ParseCipherFormatSelection(string? formatText)
    {
        return formatText?.Trim().ToUpperInvariant() switch
        {
            "C1C2C3" => SM2CipherFormat.C1C2C3,
            "C1C3C2" => SM2CipherFormat.C1C3C2,
            _ => throw new NotSupportedException($"不支持的SM2密文格式: {formatText}")
        };
    }

    public static byte[] ConvertSignatureForOutput(byte[] rsSignature, string? formatText)
    {
        if (rsSignature == null || rsSignature.Length == 0)
            throw new ArgumentException("签名不能为空", nameof(rsSignature));

        return NormalizeSignatureFormat(formatText) switch
        {
            "ASN1" => ConvertRsToDer(rsSignature),
            "RS" => rsSignature,
            _ => rsSignature
        };
    }

    public static byte[] NormalizeSignatureForVerify(byte[] signatureBytes, string? formatText)
    {
        if (signatureBytes == null || signatureBytes.Length == 0)
            throw new ArgumentException("签名不能为空", nameof(signatureBytes));

        return NormalizeSignatureFormat(formatText) switch
        {
            "ASN1" => ConvertDerToRs(signatureBytes),
            "RS" => signatureBytes,
            _ => signatureBytes
        };
    }

    private static string NormalizeSignatureFormat(string? formatText)
    {
        return formatText?.Trim().ToUpperInvariant() switch
        {
            "ASN1" => "ASN1",
            "RS" => "RS",
            _ => throw new NotSupportedException($"不支持的SM2签名格式: {formatText}")
        };
    }

    private static byte[] ConvertRsToDer(byte[] rsSignature)
    {
        if (rsSignature.Length != Sm2RsSignatureLength)
            throw new ArgumentException("RS格式签名必须为64字节", nameof(rsSignature));

        var rBytes = new byte[Sm2SignatureComponentLength];
        var sBytes = new byte[Sm2SignatureComponentLength];
        Buffer.BlockCopy(rsSignature, 0, rBytes, 0, Sm2SignatureComponentLength);
        Buffer.BlockCopy(rsSignature, Sm2SignatureComponentLength, sBytes, 0, Sm2SignatureComponentLength);

        var sequence = new DerSequence(
            new DerInteger(new BigInteger(1, rBytes)),
            new DerInteger(new BigInteger(1, sBytes)));

        return sequence.GetEncoded();
    }

    private static byte[] ConvertDerToRs(byte[] derSignature)
    {
        var derSequence = Asn1Object.FromByteArray(derSignature) as DerSequence
            ?? throw new ArgumentException("ASN1签名格式无效", nameof(derSignature));

        if (derSequence.Count != 2)
            throw new ArgumentException("ASN1签名必须包含r和s两个分量", nameof(derSignature));

        var r = ((DerInteger)derSequence[0]).PositiveValue.ToByteArrayUnsigned();
        var s = ((DerInteger)derSequence[1]).PositiveValue.ToByteArrayUnsigned();
        var result = new byte[Sm2RsSignatureLength];

        Buffer.BlockCopy(PadToLength(r, Sm2SignatureComponentLength), 0, result, 0, Sm2SignatureComponentLength);
        Buffer.BlockCopy(PadToLength(s, Sm2SignatureComponentLength), 0, result, Sm2SignatureComponentLength, Sm2SignatureComponentLength);

        return result;
    }

    private static byte[] PadToLength(byte[] input, int length)
    {
        if (input.Length == length)
            return input;

        if (input.Length > length)
            return input[^length..];

        var result = new byte[length];
        Buffer.BlockCopy(input, 0, result, length - input.Length, input.Length);
        return result;
    }

    private static bool TryReadLabeledValue(IReadOnlyList<string> lines, ref int index, string zhLabel, string enLabel, out string? value)
    {
        value = null;
        var line = lines[index];
        if (!TryStripLabel(line, zhLabel, enLabel, out var inlineValue))
            return false;

        if (!string.IsNullOrWhiteSpace(inlineValue))
        {
            value = inlineValue;
            return true;
        }

        if (index + 1 < lines.Count && !IsPotentialLabel(lines[index + 1]))
        {
            value = lines[index + 1].Trim();
            index++;
            return true;
        }

        return true;
    }

    private static bool TryStripLabel(string line, string zhLabel, string enLabel, out string? value)
    {
        value = null;
        var normalized = line.Trim();

        foreach (var prefix in GetLabelPrefixes(zhLabel, enLabel))
        {
            if (!normalized.StartsWith(prefix, StringComparison.OrdinalIgnoreCase))
                continue;

            value = normalized[prefix.Length..].Trim();
            if (value.StartsWith("：", StringComparison.Ordinal) || value.StartsWith(":", StringComparison.Ordinal) || value.StartsWith("=", StringComparison.Ordinal))
            {
                value = value[1..].Trim();
            }

            return true;
        }

        return false;
    }

    private static bool IsLabelLine(string line, string zhLabel, string enLabel)
    {
        return TryStripLabel(line, zhLabel, enLabel, out _);
    }

    private static bool IsPotentialLabel(string line)
    {
        return IsLabelLine(line, "公钥", "publickey") || IsLabelLine(line, "私钥", "privatekey");
    }

    private static IEnumerable<string> GetLabelPrefixes(string zhLabel, string enLabel)
    {
        yield return zhLabel;
        yield return enLabel;
    }
}
