using CryptoTool.Algorithm.Algorithms.MD5;
using CryptoTool.UnitTests.TestHelpers;
using CryptoTool.Algorithm.Utils;
using System.Text;

namespace CryptoTool.UnitTests;

public class Md5TabControlTests
{
    [StaFact]
    public void VerifyingMatchingHash_ShouldShowReadableSuccessMessage()
    {
        using var control = new CryptoTool.Win.MD5TabControl();

        var dataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textMD5VerifyData");
        var hashTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textMD5VerifyHash");
        var resultLabel = WinFormsTestHelper.GetPrivateField<Label>(control, "labelMD5VerifyResult");

        var dataBytes = Encoding.UTF8.GetBytes("md5 verification");
        var hashBytes = new Md5Hash().ComputeHash(dataBytes);

        dataTextBox.Text = "md5 verification";
        hashTextBox.Text = StringUtil.BytesToHex(hashBytes);

        WinFormsTestHelper.ClickButton(control, "btnMD5Verify");

        Assert.Equal("验证通过", resultLabel.Text);
        Assert.Equal(Color.Green, resultLabel.ForeColor);
    }
}
