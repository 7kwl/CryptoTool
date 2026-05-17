using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class MedicareTabControlTests
{
    [StaFact]
    public void GeneratedKeysShouldBeUsableForSigning()
    {
        using var control = new CryptoTool.Win.MedicareTabControl();

        var signDataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textMedicareSignData");

        WinFormsTestHelper.ClickButton(control, "btnGenerateMedicareKey");
        WinFormsTestHelper.ClickButton(control, "btnMedicareSign");

        Assert.False(string.IsNullOrWhiteSpace(signDataTextBox.Text));
    }
}
