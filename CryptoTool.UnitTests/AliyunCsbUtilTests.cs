using CryptoTool.Algorithm.Utils;
using System.Security.Cryptography;
using System.Text;

namespace CryptoTool.UnitTests;

public class AliyunCsbUtilTests
{
    [Fact]
    public void SignShouldSortParametersAndIncludeRequiredHeaders()
    {
        const string apiName = "queryPatient";
        const string apiVersion = "1.0";
        const long timestamp = 1710000000123;
        const string accessKey = "access-key";
        const string secretKey = "secret-key";

        var formParameters = new Dictionary<string, object[]>
        {
            ["z"] = new object[] { "last", 2 },
            ["a"] = new object[] { "first" }
        };

        var signature = AliyunCSBUtil.Sign(apiName, apiVersion, timestamp, accessKey, secretKey, formParameters, body: string.Empty);
        var signingText = "_api_access_key=access-key&_api_name=queryPatient&_api_timestamp=1710000000123&_api_version=1.0&a=first&z=last&z=2";

        using var hmac = new HMACSHA1(Encoding.UTF8.GetBytes(secretKey));
        var expectedSignature = Convert.ToBase64String(hmac.ComputeHash(Encoding.UTF8.GetBytes(signingText)));

        Assert.Equal(expectedSignature, signature);
    }
}
