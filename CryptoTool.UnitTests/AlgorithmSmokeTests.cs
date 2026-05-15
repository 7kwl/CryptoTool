using CryptoTool.Algorithm.Enums;
using CryptoTool.Algorithm.Factory;
using CryptoTool.Algorithm.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace CryptoTool.UnitTests;

public class AlgorithmSmokeTests
{
    [Fact]
    public void CryptoFactoryShouldExposeAllSupportedAlgorithms()
    {
        var supportedAlgorithms = CryptoFactory.GetSupportedAlgorithms().OrderBy(name => name).ToArray();

        Assert.Equal(new[] { "AES", "DES", "MD5", "RSA", "SM2", "SM3", "SM4" }, supportedAlgorithms);
        Assert.Equal(CryptoAlgorithmType.Symmetric, CryptoFactory.GetAlgorithmType("AES"));
        Assert.Equal(CryptoAlgorithmType.Asymmetric, CryptoFactory.GetAlgorithmType("RSA"));
        Assert.Equal(CryptoAlgorithmType.Hash, CryptoFactory.GetAlgorithmType("SM3"));
    }

    [Fact]
    public void AesAndDesShouldRoundTripRepresentativePayloads()
    {
        var payload = Encoding.UTF8.GetBytes("对称算法回归 Smoke Test");

        var aes = CryptoFactory.CreateAes(256, CipherMode.CBC, PaddingMode.PKCS7);
        var aesKey = aes.GenerateKey();
        var aesIv = aes.GenerateIV();
        var aesCipherBytes = aes.Encrypt(payload, aesKey, aesIv);
        var aesPlainBytes = aes.Decrypt(aesCipherBytes, aesKey, aesIv);

        var des = CryptoFactory.CreateDes(CipherMode.CBC, PaddingMode.PKCS7);
        var desKey = des.GenerateKey();
        var desIv = des.GenerateIV();
        var desCipherBytes = des.Encrypt(payload, desKey, desIv);
        var desPlainBytes = des.Decrypt(desCipherBytes, desKey, desIv);

        Assert.Equal(payload, aesPlainBytes);
        Assert.Equal(payload, desPlainBytes);
    }

    [Fact]
    public void Sm2ShouldSupportEncryptionAndSignatureRoundTrips()
    {
        var sm2 = CryptoFactory.CreateSm2();
        var (publicKey, privateKey) = sm2.GenerateKeyPair();
        var payload = Encoding.UTF8.GetBytes("SM2 roundtrip payload");
        var cipherBytes = sm2.Encrypt(payload, publicKey);
        var plainBytes = sm2.Decrypt(cipherBytes, privateKey);
        var signatureBytes = sm2.Sign(payload, privateKey);

        Assert.Equal(payload, plainBytes);
        Assert.True(sm2.VerifySign(payload, signatureBytes, publicKey));
    }

    [Fact]
    public void Md5AndSm3ShouldProduceVerifiableHashes()
    {
        var payload = Encoding.UTF8.GetBytes("hash regression payload");

        var md5 = CryptoFactory.CreateMd5();
        var md5Hash = md5.ComputeHash(payload);

        var sm3 = CryptoFactory.CreateSm3();
        var sm3Hash = sm3.ComputeHash(payload);

        Assert.Equal(16, md5Hash.Length);
        Assert.True(md5.VerifyHash(payload, md5Hash));
        Assert.Equal(32, sm3Hash.Length);
        Assert.True(sm3.VerifyHash(payload, sm3Hash));
    }
}
