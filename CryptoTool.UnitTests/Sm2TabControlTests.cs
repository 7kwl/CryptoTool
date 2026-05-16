using CryptoTool.Algorithm.Algorithms.SM2;
using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class Sm2TabControlTests
{
    [StaFact]
    public void EncryptingWithDifferentCipherFormatSelections_ShouldProduceRequestedFormats()
    {
        using var control = new CryptoTool.Win.SM2TabControl();
        var sm2 = new Sm2Crypto();
        var (publicKey, privateKey) = sm2.GenerateKeyPair();

        var cipherFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSM2CipherFormat");
        var keyFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSM2KeyFormat");
        var plaintextTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2PlainText");
        var publicKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2PublicKey");
        var privateKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2PrivateKey");
        var cipherTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2CipherText");

        keyFormatCombo.SelectedItem = "Base64";
        publicKeyTextBox.Text = Convert.ToBase64String(publicKey);
        privateKeyTextBox.Text = Convert.ToBase64String(privateKey);
        plaintextTextBox.Text = "SM2 cipher format";

        cipherFormatCombo.SelectedItem = "C1C2C3";
        WinFormsTestHelper.ClickButton(control, "btnSM2Encrypt");
        var c1c2c3Cipher = Convert.FromBase64String(cipherTextBox.Text);

        cipherFormatCombo.SelectedItem = "C1C3C2";
        WinFormsTestHelper.ClickButton(control, "btnSM2Encrypt");
        var c1c3c2Cipher = Convert.FromBase64String(cipherTextBox.Text);

        var c1c2c3PlainBytes = sm2.Decrypt(c1c2c3Cipher, privateKey, SM2CipherFormat.C1C2C3);
        var c1c3c2PlainBytes = sm2.Decrypt(c1c3c2Cipher, privateKey, SM2CipherFormat.C1C3C2);

        Assert.Equal("SM2 cipher format", System.Text.Encoding.UTF8.GetString(c1c2c3PlainBytes));
        Assert.Equal("SM2 cipher format", System.Text.Encoding.UTF8.GetString(c1c3c2PlainBytes));
        Assert.NotEqual(Convert.ToBase64String(c1c2c3Cipher), Convert.ToBase64String(c1c3c2Cipher));
    }

    [StaFact]
    public void SigningWithAsn1Selection_ShouldProduceVerifiableDerSignature()
    {
        using var control = new CryptoTool.Win.SM2TabControl();
        var sm2 = new Sm2Crypto();
        var (publicKey, privateKey) = sm2.GenerateKeyPair();

        var keyFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSM2KeyFormat");
        var signFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSM2SignFormat");
        var publicKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2PublicKey");
        var privateKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2PrivateKey");
        var signDataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2SignData");
        var signatureTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSM2Signature");
        var verifyResultLabel = WinFormsTestHelper.GetPrivateField<Label>(control, "labelSM2VerifyResult");

        keyFormatCombo.SelectedItem = "Base64";
        signFormatCombo.SelectedItem = "ASN1";
        publicKeyTextBox.Text = Convert.ToBase64String(publicKey);
        privateKeyTextBox.Text = Convert.ToBase64String(privateKey);
        signDataTextBox.Text = "SM2 ASN1 signature";

        WinFormsTestHelper.ClickButton(control, "btnSM2Sign");

        var signatureBytes = Convert.FromBase64String(signatureTextBox.Text);
        Assert.NotEqual(64, signatureBytes.Length);

        WinFormsTestHelper.ClickButton(control, "btnSM2Verify");
        Assert.Contains("验证成功", verifyResultLabel.Text);
    }
}
