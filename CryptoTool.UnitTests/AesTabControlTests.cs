using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class AesTabControlTests
{
    [StaFact]
    public void EncryptingWithEcbSelection_ShouldProduceDeterministicCiphertext()
    {
        using var control = new CryptoTool.Win.AESTabControl();

        var modeCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboAESMode");
        var plaintextTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESPlainText");
        var keyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESKey");
        var cipherTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESCipherText");

        modeCombo.SelectedItem = "ECB";
        plaintextTextBox.Text = "ECB mode text";
        keyTextBox.Text = Convert.ToBase64String(Enumerable.Range(1, 32).Select(i => (byte)i).ToArray());

        WinFormsTestHelper.ClickButton(control, "btnAESEncrypt");
        var firstCiphertext = cipherTextBox.Text;

        WinFormsTestHelper.ClickButton(control, "btnAESEncrypt");
        var secondCiphertext = cipherTextBox.Text;

        Assert.Equal(firstCiphertext, secondCiphertext);
    }

    [StaFact]
    public void EncryptingWithNonePaddingSelection_ShouldNotAddPaddingBlock()
    {
        using var control = new CryptoTool.Win.AESTabControl();

        var modeCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboAESMode");
        var paddingCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboAESPadding");
        var plaintextTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESPlainText");
        var keyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESKey");
        var ivTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESIV");
        var cipherTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textAESCipherText");

        modeCombo.SelectedItem = "CBC";
        paddingCombo.SelectedItem = "None";
        plaintextTextBox.Text = "1234567890ABCDEF";
        keyTextBox.Text = Convert.ToBase64String(Enumerable.Range(1, 32).Select(i => (byte)i).ToArray());
        ivTextBox.Text = Convert.ToBase64String(Enumerable.Range(65, 16).Select(i => (byte)i).ToArray());

        WinFormsTestHelper.ClickButton(control, "btnAESEncrypt");

        var ciphertextBytes = Convert.FromBase64String(cipherTextBox.Text);
        Assert.Equal(16, ciphertextBytes.Length);
    }
}
