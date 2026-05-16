using CryptoTool.Win.Helpers;

namespace CryptoTool.UnitTests;

public class Sm2UiHelperTests
{
    [Fact]
    public void ExportedSm2KeyContentShouldRoundTripThroughParser()
    {
        const string publicKey = "public-key-content";
        const string privateKey = "private-key-content";

        var content = Sm2UiHelper.BuildKeyFileContent(publicKey, privateKey);
        var parsed = Sm2UiHelper.ParseKeyFileContent(content);

        Assert.Equal(publicKey, parsed.PublicKey);
        Assert.Equal(privateKey, parsed.PrivateKey);
    }
}
