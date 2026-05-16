using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.Algorithm.Enums;
using CryptoTool.Algorithm.Utils;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace CryptoTool.Win.Helpers;

internal static class RsaUiHelper
{
    private static readonly Regex PemBlockRegex = new(
        "-----BEGIN[\\s\\S]+?-----END[\\s\\S]+?-----",
        RegexOptions.Compiled);

    public static AsymmetricPaddingMode ParsePadding(string? paddingText)
    {
        return paddingText?.Trim().ToUpperInvariant() switch
        {
            "PKCS1" => AsymmetricPaddingMode.PKCS1,
            "OAEP" => AsymmetricPaddingMode.OAEP,
            "NOPADDING" => throw new NotSupportedException("RSA NoPadding 当前未实现，请使用 PKCS1 或 OAEP"),
            "NONE" => throw new NotSupportedException("RSA NoPadding 当前未实现，请使用 PKCS1 或 OAEP"),
            _ => AsymmetricPaddingMode.PKCS1
        };
    }

    public static SignatureAlgorithm ParseSignatureAlgorithm(string? algorithmText)
    {
        var normalized = algorithmText?.Trim().ToUpperInvariant() ?? string.Empty;
        return normalized switch
        {
            var text when text.StartsWith("SHA1WITHRSA") => SignatureAlgorithm.SHA1withRSA,
            var text when text.StartsWith("SHA256WITHRSA") => SignatureAlgorithm.SHA256withRSA,
            var text when text.StartsWith("SHA384WITHRSA") => SignatureAlgorithm.SHA384withRSA,
            var text when text.StartsWith("SHA512WITHRSA") => SignatureAlgorithm.SHA512withRSA,
            var text when text.StartsWith("MD5WITHRSA") => SignatureAlgorithm.MD5withRSA,
            _ => SignatureAlgorithm.SHA256withRSA
        };
    }

    public static byte[] ReadKeyBytes(string keyText, string? formatText, bool isPrivateKey, string? keyTypeText)
    {
        if (string.IsNullOrWhiteSpace(keyText))
            throw new ArgumentException("密钥内容不能为空", nameof(keyText));

        var format = NormalizeKeyFormat(formatText);
        var normalizedKeyType = NormalizeKeyType(keyTypeText);

        return format switch
        {
            "pem" => isPrivateKey
                ? new RsaCrypto(2048, normalizedKeyType).ImportPrivateKeyFromPem(keyText)
                : new RsaCrypto(2048, normalizedKeyType).ImportPublicKeyFromPem(keyText),
            "base64" => Convert.FromBase64String(RemoveWhitespace(keyText)),
            "hex" => StringUtil.HexToBytes(RemoveWhitespace(keyText)),
            _ => throw new ArgumentException($"不支持的RSA密钥格式: {formatText}")
        };
    }

    public static string WriteKeyText(byte[] keyBytes, string? formatText, bool isPrivateKey, string? keyTypeText)
    {
        var format = NormalizeKeyFormat(formatText);
        var normalizedKeyType = NormalizeKeyType(keyTypeText);

        return format switch
        {
            "pem" => isPrivateKey
                ? new RsaCrypto(2048, normalizedKeyType).ExportPrivateKeyToPem(keyBytes)
                : new RsaCrypto(2048, normalizedKeyType).ExportPublicKeyToPem(keyBytes),
            "base64" => Convert.ToBase64String(keyBytes),
            "hex" => StringUtil.BytesToHex(keyBytes),
            _ => throw new ArgumentException($"不支持的RSA密钥格式: {formatText}")
        };
    }

    public static byte[] ReadBinaryText(string text, string? formatText)
    {
        if (string.IsNullOrWhiteSpace(text))
            throw new ArgumentException("内容不能为空", nameof(text));

        var normalized = formatText?.Trim().ToUpperInvariant();
        return normalized switch
        {
            "BASE64" => Convert.FromBase64String(RemoveWhitespace(text)),
            "HEX" => StringUtil.HexToBytes(RemoveWhitespace(text)),
            _ => Encoding.UTF8.GetBytes(text)
        };
    }

    public static string WriteBinaryText(byte[] bytes, string? formatText)
    {
        var normalized = formatText?.Trim().ToUpperInvariant();
        return normalized switch
        {
            "HEX" => StringUtil.BytesToHex(bytes),
            _ => Convert.ToBase64String(bytes)
        };
    }

    public static (string? PublicKey, string? PrivateKey) ParseKeyFileContent(string content)
    {
        if (string.IsNullOrWhiteSpace(content))
            return (null, null);

        var pemBlocks = PemBlockRegex.Matches(content)
            .Select(match => match.Value.Trim())
            .ToList();

        if (pemBlocks.Count > 0)
        {
            string? publicKey = pemBlocks.FirstOrDefault(block => block.Contains("PUBLIC KEY", StringComparison.OrdinalIgnoreCase));
            string? privateKey = pemBlocks.FirstOrDefault(block => block.Contains("PRIVATE KEY", StringComparison.OrdinalIgnoreCase));
            return (publicKey, privateKey);
        }

        var lines = content
            .Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries)
            .Select(line => line.Trim())
            .Where(line => !line.StartsWith("#", StringComparison.Ordinal))
            .ToList();

        if (lines.Count == 0)
            return (null, null);

        if (lines.Count == 1)
        {
            return lines[0].Contains("PRIVATE", StringComparison.OrdinalIgnoreCase)
                ? (null, lines[0])
                : (lines[0], null);
        }

        return (lines[0], lines[1]);
    }

    public static bool LooksLikePem(string text)
    {
        return !string.IsNullOrWhiteSpace(text) && text.Contains("-----BEGIN", StringComparison.OrdinalIgnoreCase);
    }

    public static byte[] ReadKeyBytesAuto(string keyText, bool isPrivateKey)
    {
        if (LooksLikePem(keyText))
        {
            var keyType = DetectPemKeyType(keyText);
            return ReadKeyBytes(keyText, "PEM", isPrivateKey, keyType);
        }

        var format = DetectTextFormat(keyText);
        var normalizedText = RemoveWhitespace(keyText);
        var rawBytes = format == "hex"
            ? StringUtil.HexToBytes(normalizedText)
            : Convert.FromBase64String(normalizedText);
        var keyTypeText = RsaCrypto.DetectRsaKeyFormatByDer(rawBytes);
        return ReadKeyBytes(keyText, format, isPrivateKey, keyTypeText);
    }

    public static void ImportPublicKey(RSA rsa, byte[] keyBytes, string keyTypeText)
    {
        if (NormalizeKeyType(keyTypeText) == "pkcs1")
        {
            rsa.ImportRSAPublicKey(keyBytes, out _);
            return;
        }

        rsa.ImportSubjectPublicKeyInfo(keyBytes, out _);
    }

    public static void ImportPrivateKey(RSA rsa, byte[] keyBytes, string keyTypeText)
    {
        if (NormalizeKeyType(keyTypeText) == "pkcs1")
        {
            rsa.ImportRSAPrivateKey(keyBytes, out _);
            return;
        }

        rsa.ImportPkcs8PrivateKey(keyBytes, out _);
    }

    public static string DetectKeyTypeFromDer(byte[] keyBytes)
    {
        return RsaCrypto.DetectRsaKeyFormatByDer(keyBytes);
    }

    private static string NormalizeKeyFormat(string? formatText)
    {
        return formatText?.Trim().ToLowerInvariant() switch
        {
            "pem" => "pem",
            "base64" => "base64",
            "hex" => "hex",
            _ => throw new ArgumentException($"不支持的RSA密钥文本格式: {formatText}")
        };
    }

    private static string NormalizeKeyType(string? keyTypeText)
    {
        return keyTypeText?.Trim().ToLowerInvariant() switch
        {
            "pkcs1" => "pkcs1",
            "pkcs8" => "pkcs8",
            _ => throw new ArgumentException($"不支持的RSA密钥类型: {keyTypeText}")
        };
    }

    private static string DetectPemKeyType(string pemKey)
    {
        return RsaCrypto.DetectRsaKeyFormatFromPem(pemKey);
    }

    private static string DetectTextFormat(string keyText)
    {
        var normalized = RemoveWhitespace(keyText);
        if (normalized.Length % 2 == 0 && Regex.IsMatch(normalized, "^[0-9A-Fa-f]+$"))
            return "hex";

        return "base64";
    }

    private static string RemoveWhitespace(string text)
    {
        return Regex.Replace(text, "\\s+", string.Empty);
    }
}
