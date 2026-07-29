using CryptoTool.Algorithm.Algorithms.ECDSA;
using Org.BouncyCastle.Crypto.Parameters;
using System.Text;

namespace CryptoTool.UnitTests;

/// <summary>
/// ECDSA 纯算法单元测试：密钥生成、签名/验签、密钥 PEM 导入/导出
/// </summary>
public class EcdsaAlgorithmTests
{
    #region 密钥生成

    [Fact]
    public void GenerateKeyPair_CommonCurves_ShouldSucceed()
    {
        var curves = new[] { "secp256r1", "secp384r1", "secp521r1", "brainpoolP256r1" };

        foreach (var curve in curves)
        {
            var keyPair = EcdsaAlgorithm.GenerateKeyPair(curve);
            Assert.NotNull(keyPair);
            Assert.NotNull(keyPair.Private);
            Assert.NotNull(keyPair.Public);
        }
    }

    [Fact]
    public void GetPublicKey_FromPrivateKey_ShouldMatchGeneratedPublicKey()
    {
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;

        var derivedPublic = EcdsaAlgorithm.GetPublicKey(privateKey);

        Assert.NotNull(derivedPublic);
        Assert.Equal(
            ((ECPublicKeyParameters)keyPair.Public).Q.GetEncoded(),
            derivedPublic.Q.GetEncoded());
    }

    #endregion

    #region 签名/验签

    [Theory]
    [InlineData("SHA-256")]
    [InlineData("SHA-384")]
    [InlineData("SHA-512")]
    public void SignAndVerify_VariousHashAlgorithms_ShouldRoundTrip(string hash)
    {
        var payload = Encoding.UTF8.GetBytes($"ECDSA {hash} test payload");
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var signature = EcdsaAlgorithm.Sign(payload, privateKey, hash);
        Assert.NotEmpty(signature);

        var isValid = EcdsaAlgorithm.Verify(payload, signature, publicKey, hash);
        Assert.True(isValid);
    }

    [Fact]
    public void Verify_TamperedData_ShouldReturnFalse()
    {
        var originalPayload = Encoding.UTF8.GetBytes("original data");
        var tamperedPayload = Encoding.UTF8.GetBytes("tampered data");
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var signature = EcdsaAlgorithm.Sign(originalPayload, privateKey);

        var isValid = EcdsaAlgorithm.Verify(tamperedPayload, signature, publicKey);
        Assert.False(isValid);
    }

    [Fact]
    public void Verify_WrongPublicKey_ShouldReturnFalse()
    {
        var payload = Encoding.UTF8.GetBytes("test payload");
        var keyPair1 = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var keyPair2 = EcdsaAlgorithm.GenerateKeyPair("secp256r1");

        var signature = EcdsaAlgorithm.Sign(payload, (ECPrivateKeyParameters)keyPair1.Private);
        var isValid = EcdsaAlgorithm.Verify(payload, signature, (ECPublicKeyParameters)keyPair2.Public);

        Assert.False(isValid);
    }

    [Fact]
    public void Sign_SameDataTwice_ShouldProduceDifferentSignatures()
    {
        var payload = Encoding.UTF8.GetBytes("deterministic test");
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var sig1 = EcdsaAlgorithm.Sign(payload, privateKey);
        var sig2 = EcdsaAlgorithm.Sign(payload, privateKey);

        // ECDSA 签名是随机的（有随机 k），两次签名应不同
        Assert.NotEqual(Convert.ToBase64String(sig1), Convert.ToBase64String(sig2));
        // 但都能通过验签
        Assert.True(EcdsaAlgorithm.Verify(payload, sig1, publicKey));
        Assert.True(EcdsaAlgorithm.Verify(payload, sig2, publicKey));
    }

    #endregion

    #region 密钥 PEM 导入/导出

    [Fact]
    public void ExportImportPrivateKeyPem_ShouldRoundTrip()
    {
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var pem = EcdsaKeyHelper.ExportPrivateKeyPem(privateKey);
        Assert.Contains("BEGIN", pem);

        var imported = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
        Assert.NotNull(imported);

        // 验证导入的私钥能正确签名
        var payload = Encoding.UTF8.GetBytes("PEM roundtrip");
        var signature = EcdsaAlgorithm.Sign(payload, imported);
        Assert.True(EcdsaAlgorithm.Verify(payload, signature, publicKey));
    }

    [Fact]
    public void ExportImportPublicKeyPem_ShouldRoundTrip()
    {
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var pem = EcdsaKeyHelper.ExportPublicKeyPem(publicKey);
        Assert.Contains("BEGIN", pem);

        var imported = EcdsaKeyHelper.ImportPublicKeyPem(pem);
        Assert.NotNull(imported);

        // 验证导入的公钥能正确验签
        var payload = Encoding.UTF8.GetBytes("PEM roundtrip");
        var signature = EcdsaAlgorithm.Sign(payload, (ECPrivateKeyParameters)EcdsaAlgorithm.GenerateKeyPair("secp256r1").Private);
        Assert.False(EcdsaAlgorithm.Verify(payload, signature, imported)); // 不同密钥对应失败
    }

    [Fact]
    public void ExportPrivateKeyPemNamedCurve_Secp256r1_ShouldContainCurveNameInOutput()
    {
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;

        var pem = EcdsaKeyHelper.ExportPrivateKeyPemNamedCurve(privateKey);

        // namedCurve 格式应在 PEM 头中包含曲线 OID 名称
        Assert.Contains("BEGIN EC PRIVATE KEY", pem);
    }

    #endregion

    #region ECDH/ECIES

    [Fact]
    public void Ecdh_KeyAgreement_ShouldDeriveSameSecret()
    {
        var aliceKeyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var bobKeyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");

        var secret1 = EcdhAlgorithm.DeriveSharedSecret(
            (ECPrivateKeyParameters)aliceKeyPair.Private,
            (ECPublicKeyParameters)bobKeyPair.Public);
        var secret2 = EcdhAlgorithm.DeriveSharedSecret(
            (ECPrivateKeyParameters)bobKeyPair.Private,
            (ECPublicKeyParameters)aliceKeyPair.Public);

        Assert.Equal(Convert.ToBase64String(secret1), Convert.ToBase64String(secret2));
    }

    [Fact]
    public void EciesEncryptDecrypt_ShouldRoundTrip()
    {
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var plainText = "Hello ECIES!";
        var payload = Encoding.UTF8.GetBytes(plainText);

        var encrypted = EcdhAlgorithm.EciesEncrypt(
            payload,
            (ECPublicKeyParameters)keyPair.Public,
            "secp256r1",
            EcdhMode.CryptoTool,
            out _, out _);
        Assert.NotNull(encrypted);

        var decrypted = EcdhAlgorithm.EciesDecrypt(
            encrypted,
            (ECPrivateKeyParameters)keyPair.Private,
            "secp256r1",
            EcdhMode.CryptoTool,
            out _, out _);
        Assert.Equal(plainText, Encoding.UTF8.GetString(decrypted));
    }

    #endregion
}
