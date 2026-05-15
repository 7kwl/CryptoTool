using CryptoTool.Algorithm.Algorithms.SM2;
using CryptoTool.Algorithm.Utils;

namespace CryptoTool.UnitTests;

public class MedicareUtilTests
{
    [Fact]
    public void BuildSignatureBaseStringShouldExcludeIgnoredKeysAndCanonicalizeObjects()
    {
        var parameters = new Dictionary<string, object>
        {
            ["b"] = 2,
            ["a"] = new { z = 3, m = "value" },
            ["signData"] = "ignored",
            ["encData"] = "ignored",
            ["empty"] = ""
        };

        var baseString = MedicareUtil.BuildSignatureBaseString(parameters, "app-secret");

        Assert.Equal("a={\"m\":\"value\",\"z\":3}&b=2&key=app-secret", baseString);
    }

    [Fact]
    public void SigningParametersShouldRoundTripAndRejectTampering()
    {
        var sm2 = new Sm2Crypto();
        var (publicKey, privateKey) = sm2.GenerateKeyPair();
        var parameters = new Dictionary<string, object>
        {
            ["appId"] = "demo-app-id",
            ["timestamp"] = 1710000000123L,
            ["payload"] = new { code = "A01", amount = 123.45m }
        };

        var signature = MedicareUtil.SignParameters(parameters, privateKey, "medicare-secret");

        Assert.True(MedicareUtil.VerifyParametersSignature(parameters, signature, publicKey, "medicare-secret"));

        parameters["payload"] = new { code = "A01", amount = 999.99m };

        Assert.False(MedicareUtil.VerifyParametersSignature(parameters, signature, publicKey, "medicare-secret"));
    }

    [Fact]
    public void EncryptingAndDecryptingEncDataShouldRoundTripCanonicalJson()
    {
        var payload = new
        {
            z = "tail",
            a = new { name = "Alice", count = 2 }
        };

        var encrypted = MedicareUtil.EncryptData(payload, "ABCDEFGHIJKLMNOP1234", "医保联调Secret123");
        var decrypted = MedicareUtil.DecryptEncData(encrypted, "ABCDEFGHIJKLMNOP1234", "医保联调Secret123");

        Assert.Equal("{\"a\":{\"count\":2,\"name\":\"Alice\"},\"z\":\"tail\"}", decrypted);
    }
}
