using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.Algorithm.Enums;
using System.Text;

namespace CryptoTool.UnitTests;

public class RsaAlgorithmTests
{
    [Theory]
    [InlineData("pkcs1", "pkcs1")]
    [InlineData("pkcs8", "pkcs8")]
    public void PemKeyPairShouldRoundTripAndRemainUsable(string keyFormat, string expectedFormat)
    {
        var rsa = new RsaCrypto(2048, keyFormat);
        var (publicKeyPem, privateKeyPem) = rsa.GenerateKeyPairPem();
        var payload = Encoding.UTF8.GetBytes("pem roundtrip payload");

        var importedPublicKey = rsa.ImportPublicKeyFromPem(publicKeyPem);
        var importedPrivateKey = rsa.ImportPrivateKeyFromPem(privateKeyPem);
        var cipherBytes = rsa.Encrypt(payload, importedPublicKey);
        var plainBytes = rsa.Decrypt(cipherBytes, importedPrivateKey);
        var signatureBytes = rsa.Sign(payload, importedPrivateKey);

        Assert.Equal(expectedFormat, RsaCrypto.DetectRsaKeyFormatFromPem(publicKeyPem));
        Assert.Equal(expectedFormat, RsaCrypto.DetectRsaKeyFormatFromPem(privateKeyPem));
        Assert.Equal(payload, plainBytes);
        Assert.True(rsa.VerifySign(payload, signatureBytes, importedPublicKey));
    }

    [Fact]
    public void PkcsConversionShouldPreserveEncryptionAndSignatureCapabilities()
    {
        var rsaPkcs1 = new RsaCrypto(2048, "pkcs1");
        var (pkcs1PublicKey, pkcs1PrivateKey) = rsaPkcs1.GenerateKeyPair();
        var pkcs8PublicKey = rsaPkcs1.ConvertPublicKeyFromPKCS1ToPKCS8(pkcs1PublicKey);
        var pkcs8PrivateKey = rsaPkcs1.ConvertPrivateKeyFromPKCS1ToPKCS8(pkcs1PrivateKey);
        var rsaPkcs8 = new RsaCrypto(2048, "pkcs8");
        var payload = Encoding.UTF8.GetBytes("pkcs conversion payload");

        var cipherBytes = rsaPkcs8.Encrypt(payload, pkcs8PublicKey, AsymmetricPaddingMode.OAEP);
        var plainBytes = rsaPkcs8.Decrypt(cipherBytes, pkcs8PrivateKey, AsymmetricPaddingMode.OAEP);
        var signatureBytes = rsaPkcs8.Sign(payload, pkcs8PrivateKey, SignatureAlgorithm.SHA512withRSA);

        Assert.Equal(payload, plainBytes);
        Assert.True(rsaPkcs8.VerifySign(payload, signatureBytes, pkcs8PublicKey, SignatureAlgorithm.SHA512withRSA));
    }
}
