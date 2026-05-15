using CryptoTool.Algorithm.Algorithms.RSA;
using System.Security.Cryptography;

namespace CryptoTool.Win.Helpers;

internal static class RsaKeyConversionService
{
    public static string ConvertKey(
        string keyText,
        bool isPrivateKey,
        string inputKeyTypeText,
        string inputFormatText,
        string outputKeyTypeText,
        string outputFormatText)
    {
        var inputBytes = RsaUiHelper.ReadKeyBytes(keyText, inputFormatText, isPrivateKey, inputKeyTypeText);
        var convertedBytes = ConvertKeyType(inputBytes, isPrivateKey, inputKeyTypeText, outputKeyTypeText);
        return RsaUiHelper.WriteKeyText(convertedBytes, outputFormatText, isPrivateKey, outputKeyTypeText);
    }

    public static string ExtractPublicKeyFromPrivate(
        string privateKeyText,
        string inputKeyTypeText,
        string inputFormatText,
        string outputKeyTypeText,
        string outputFormatText)
    {
        var privateKeyBytes = RsaUiHelper.ReadKeyBytes(privateKeyText, inputFormatText, true, inputKeyTypeText);

        using var rsa = RSA.Create();
        RsaUiHelper.ImportPrivateKey(rsa, privateKeyBytes, inputKeyTypeText);

        var publicKeyBytes = outputKeyTypeText.Trim().Equals("PKCS1", StringComparison.OrdinalIgnoreCase)
            ? rsa.ExportRSAPublicKey()
            : rsa.ExportSubjectPublicKeyInfo();

        return RsaUiHelper.WriteKeyText(publicKeyBytes, outputFormatText, false, outputKeyTypeText);
    }

    public static bool ValidateKeyPair(string publicKeyText, string privateKeyText)
    {
        var publicKeyBytes = RsaUiHelper.ReadKeyBytesAuto(publicKeyText, false);
        var privateKeyBytes = RsaUiHelper.ReadKeyBytesAuto(privateKeyText, true);

        using var publicRsa = RSA.Create();
        using var privateRsa = RSA.Create();

        var publicKeyType = RsaUiHelper.DetectKeyTypeFromDer(publicKeyBytes);
        var privateKeyType = RsaUiHelper.DetectKeyTypeFromDer(privateKeyBytes);

        RsaUiHelper.ImportPublicKey(publicRsa, publicKeyBytes, publicKeyType);
        RsaUiHelper.ImportPrivateKey(privateRsa, privateKeyBytes, privateKeyType);

        var publicParameters = publicRsa.ExportParameters(false);
        var privateParameters = privateRsa.ExportParameters(false);

        return publicParameters.Modulus.AsSpan().SequenceEqual(privateParameters.Modulus)
            && publicParameters.Exponent.AsSpan().SequenceEqual(privateParameters.Exponent);
    }

    private static byte[] ConvertKeyType(byte[] keyBytes, bool isPrivateKey, string inputKeyTypeText, string outputKeyTypeText)
    {
        if (inputKeyTypeText.Equals(outputKeyTypeText, StringComparison.OrdinalIgnoreCase))
            return keyBytes;

        var rsa = new RsaCrypto(2048, inputKeyTypeText.Trim().ToLowerInvariant());

        if (isPrivateKey)
        {
            return inputKeyTypeText.Equals("PKCS1", StringComparison.OrdinalIgnoreCase)
                ? rsa.ConvertPrivateKeyFromPKCS1ToPKCS8(keyBytes)
                : rsa.ConvertPrivateKeyFromPKCS8ToPKCS1(keyBytes);
        }

        return inputKeyTypeText.Equals("PKCS1", StringComparison.OrdinalIgnoreCase)
            ? rsa.ConvertPublicKeyFromPKCS1ToPKCS8(keyBytes)
            : rsa.ConvertPublicKeyFromPKCS8ToPKCS1(keyBytes);
    }
}
