using CryptoTool.Algorithm.Algorithms.AES;
using CryptoTool.Algorithm.Algorithms.DES;
using CryptoTool.Algorithm.Algorithms.SM4;
using CryptoTool.Algorithm.Enums;
using System.Security.Cryptography;

namespace CryptoTool.Win.Helpers;

internal static class SymmetricCryptoOptionHelper
{
    public static AesCrypto CreateAesCrypto(string? keySizeText, byte[] keyBytes, string? modeText, string? paddingText)
    {
        var keySize = ParseAesKeySize(keySizeText, keyBytes);
        return new AesCrypto(keySize, ParseAesMode(modeText), ParseAesPadding(paddingText));
    }

    public static DesCrypto CreateDesCrypto(string? modeText, string? paddingText)
    {
        return new DesCrypto(ParseDesMode(modeText), ParseDesPadding(paddingText));
    }

    public static Sm4Crypto CreateSm4Crypto(string? modeText, string? paddingText)
    {
        return new Sm4Crypto(ParseSm4Mode(modeText), ParseSm4Padding(paddingText));
    }

    private static int ParseAesKeySize(string? keySizeText, byte[] keyBytes)
    {
        var normalized = keySizeText?.Trim().ToUpperInvariant();
        return normalized switch
        {
            "AES128" => 128,
            "AES192" => 192,
            "AES256" => 256,
            _ when keyBytes.Length == 16 => 128,
            _ when keyBytes.Length == 24 => 192,
            _ when keyBytes.Length == 32 => 256,
            _ => throw new ArgumentException("无法识别AES密钥长度")
        };
    }

    private static CipherMode ParseAesMode(string? modeText)
    {
        return modeText?.Trim().ToUpperInvariant() switch
        {
            "ECB" => CipherMode.ECB,
            "CBC" => CipherMode.CBC,
            "CFB" => CipherMode.CFB,
            "OFB" => CipherMode.OFB,
            _ => CipherMode.CBC
        };
    }

    private static PaddingMode ParseAesPadding(string? paddingText)
    {
        return paddingText?.Trim().ToUpperInvariant() switch
        {
            "PKCS7" => PaddingMode.PKCS7,
            "PKCS5" => PaddingMode.PKCS7,
            "ZEROS" => PaddingMode.Zeros,
            "ISO10126" => PaddingMode.ISO10126,
            "ANSIX923" => PaddingMode.ANSIX923,
            "NONE" => PaddingMode.None,
            _ => PaddingMode.PKCS7
        };
    }

    private static CipherMode ParseDesMode(string? modeText)
    {
        return modeText?.Trim().ToUpperInvariant() switch
        {
            "ECB" => CipherMode.ECB,
            "CBC" => CipherMode.CBC,
            "CFB" => CipherMode.CFB,
            _ => CipherMode.CBC
        };
    }

    private static PaddingMode ParseDesPadding(string? paddingText)
    {
        return paddingText?.Trim().ToUpperInvariant() switch
        {
            "PKCS7" => PaddingMode.PKCS7,
            "PKCS5" => PaddingMode.PKCS7,
            "ZEROS" => PaddingMode.Zeros,
            "ISO10126" => PaddingMode.ISO10126,
            "ANSIX923" => PaddingMode.ANSIX923,
            "NONE" => PaddingMode.None,
            _ => PaddingMode.PKCS7
        };
    }

    private static SymmetricCipherMode ParseSm4Mode(string? modeText)
    {
        return modeText?.Trim().ToUpperInvariant() switch
        {
            "ECB" => SymmetricCipherMode.ECB,
            "CBC" => SymmetricCipherMode.CBC,
            "CFB" => SymmetricCipherMode.CFB,
            "OFB" => SymmetricCipherMode.OFB,
            "CTR" => SymmetricCipherMode.CTR,
            _ => SymmetricCipherMode.CBC
        };
    }

    private static SymmetricPaddingMode ParseSm4Padding(string? paddingText)
    {
        return paddingText?.Trim().ToUpperInvariant() switch
        {
            "PKCS7" => SymmetricPaddingMode.PKCS7,
            "PKCS5" => SymmetricPaddingMode.PKCS5,
            "NOPADDING" => SymmetricPaddingMode.None,
            "NONE" => SymmetricPaddingMode.None,
            "ZEROS" => SymmetricPaddingMode.Zeros,
            _ => SymmetricPaddingMode.PKCS7
        };
    }
}
