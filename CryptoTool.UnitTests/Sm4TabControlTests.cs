using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class Sm4TabControlTests
{
    [StaFact]
    public void EncryptingWithNoPaddingSelection_ShouldNotAddPaddingBlock()
    {
        using var control = new CryptoTool.Win.SM4TabControl();

        var modeCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSM4Mode");
        var paddingCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSM4Padding");
        var plaintextTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM4PlainText");
        var keyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM4Key");
        var cipherTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM4CipherText");

        modeCombo.SelectedItem = "ECB";
        paddingCombo.SelectedItem = "NoPadding";
        plaintextTextBox.Text = "1234567890ABCDEF";
        keyTextBox.Text = Convert.ToBase64String(Enumerable.Range(1, 16).Select(i => (byte)i).ToArray());

        WinFormsTestHelper.ClickButton(control, "btnSM4Encrypt");

        var ciphertextBytes = Convert.FromBase64String(cipherTextBox.Text);
        Assert.Equal(16, ciphertextBytes.Length);
    }
}
