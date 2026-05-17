using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class Sm3TabControlTests
{
    [StaFact]
    public void ComputingFileHash_ShouldWriteResultToFileHashTextBox()
    {
        using var control = new CryptoTool.Win.SM3TabControl();
        var tempFilePath = Path.GetTempFileName();

        try
        {
            File.WriteAllText(tempFilePath, "sm3 file hash");

            var filePathTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM3FilePath");
            var fileHashTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM3FileHash");

            filePathTextBox.Text = tempFilePath;

            WinFormsTestHelper.ClickButton(control, "btnSM3ComputeFileHash");

            Assert.False(string.IsNullOrWhiteSpace(fileHashTextBox.Text));
        }
        finally
        {
            File.Delete(tempFilePath);
        }
    }

    [StaFact]
    public void ClickingHmacButton_ShouldPopulateHmacOutput()
    {
        using var control = new CryptoTool.Win.SM3TabControl();

        var dataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM3HMACData");
        var keyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM3HMACKey");
        var outputTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM3HMACOutput");

        dataTextBox.Text = "sm3 hmac data";
        keyTextBox.Text = "sm3 hmac key";

        WinFormsTestHelper.ClickButton(control, "btnSM3HMAC");

        Assert.False(string.IsNullOrWhiteSpace(outputTextBox.Text));
    }
}
