using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.Algorithm.Enums;
using CryptoTool.UnitTests.TestHelpers;
using System.Text;

namespace CryptoTool.UnitTests;

public class RsaTabControlTests
{
    [StaFact]
    public void SigningShouldUseSelectedSignatureAlgorithm()
    {
        using var control = new CryptoTool.Win.RSATabControl();
        var rsa = new RsaCrypto(2048, "pkcs8");
        var (publicKey, privateKey) = rsa.GenerateKeyPair();

        var algorithmCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboRSASignAlgmFormat");
        var dataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textRSASignData");
        var privateKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textRSAPrivateKey");
        var signatureTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textRSASignature");

        algorithmCombo.SelectedItem = "SHA1withRSA(RSA1)";
        dataTextBox.Text = "signature payload";
        privateKeyTextBox.Text = Convert.ToBase64String(privateKey);

        WinFormsTestHelper.ClickButton(control, "btnRSASign");

        var signatureBytes = Convert.FromBase64String(signatureTextBox.Text);
        var isValid = rsa.VerifySign(
            Encoding.UTF8.GetBytes(dataTextBox.Text),
            signatureBytes,
            publicKey,
            SignatureAlgorithm.SHA1withRSA);

        Assert.True(isValid);
    }

    [StaFact]
    public void EncryptionShouldUseSelectedPadding()
    {
        using var control = new CryptoTool.Win.RSATabControl();
        var rsa = new RsaCrypto(2048, "pkcs8");
        var (publicKey, privateKey) = rsa.GenerateKeyPair();

        var paddingCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboRSAKeyPadding");
        var plaintextTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textRSAPlainText");
        var publicKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textRSAPublicKey");
        var cipherTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textRSACipherText");

        paddingCombo.SelectedItem = "OAEP";
        plaintextTextBox.Text = "oaep payload";
        publicKeyTextBox.Text = Convert.ToBase64String(publicKey);

        WinFormsTestHelper.ClickButton(control, "btnRSAEncrypt");

        var cipherBytes = Convert.FromBase64String(cipherTextBox.Text);
        var decryptedBytes = rsa.Decrypt(cipherBytes, privateKey, AsymmetricPaddingMode.OAEP);

        Assert.Equal(plaintextTextBox.Text, Encoding.UTF8.GetString(decryptedBytes));
    }

    [StaFact]
    public void PaddingOptionsShouldNotExposeUnsupportedNoPadding()
    {
        using var control = new CryptoTool.Win.RSATabControl();

        var paddingCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboRSAKeyPadding");
        var items = paddingCombo.Items.Cast<object>().Select(item => item.ToString()).ToArray();

        Assert.DoesNotContain("NoPadding", items);
    }
}
