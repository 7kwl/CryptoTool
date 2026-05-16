using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class DesTabControlTests
{
    [StaFact]
    public void EncryptingWithEcbAndNonePaddingSelection_ShouldRespectUiOptions()
    {
        using var control = new CryptoTool.Win.DESTabControl();

        var modeCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboDESMode");
        var paddingCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboDESPadding");
        var keyFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboDESKeyFormat");
        var plaintextTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textDESPlainText");
        var keyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textDESKey");
        var cipherTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textDESCipherText");

        modeCombo.SelectedItem = "ECB";
        paddingCombo.SelectedItem = "None";
        keyFormatCombo.SelectedItem = "Base64";
        plaintextTextBox.Text = "12345678";
        keyTextBox.Text = Convert.ToBase64String(Enumerable.Range(1, 8).Select(i => (byte)i).ToArray());

        WinFormsTestHelper.ClickButton(control, "btnDESEncrypt");

        var ciphertextBytes = Convert.FromBase64String(cipherTextBox.Text);
        Assert.Equal(8, ciphertextBytes.Length);
    }
}
