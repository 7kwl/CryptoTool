using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.UnitTests.TestHelpers;

namespace CryptoTool.UnitTests;

public class RsaConvertTabControlTests
{
    [StaFact]
    public void ConvertingPkcs1PublicKeyToPkcs8ShouldChangeKeyBytes()
    {
        using var control = new CryptoTool.Win.RSAConvertTabControl();
        var rsa = new RsaCrypto(2048, "pkcs1");
        var (pkcs1PublicKey, _) = rsa.GenerateKeyPair();
        var expectedPkcs8PublicKey = rsa.ConvertPublicKeyFromPKCS1ToPKCS8(pkcs1PublicKey);

        var radioPublicKey = WinFormsTestHelper.GetPrivateField<RadioButton>(control, "radioPublicKey");
        var inputFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboInputFormat");
        var inputKeyTypeCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboInputKeyType");
        var outputFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboOutputFormat");
        var outputKeyTypeCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboOutputKeyType");
        var inputTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textInputKey");
        var outputTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textOutputKey");

        radioPublicKey.Checked = true;
        inputFormatCombo.SelectedItem = "Base64";
        inputKeyTypeCombo.SelectedItem = "PKCS1";
        outputFormatCombo.SelectedItem = "Base64";
        outputKeyTypeCombo.SelectedItem = "PKCS8";
        inputTextBox.Text = Convert.ToBase64String(pkcs1PublicKey);

        WinFormsTestHelper.ClickButton(control, "btnConvert");

        Assert.Equal(Convert.ToBase64String(expectedPkcs8PublicKey), outputTextBox.Text);
    }

    [StaFact]
    public void ValidatingMismatchedKeyPairShouldReportMismatch()
    {
        using var control = new CryptoTool.Win.RSAConvertTabControl();
        var firstRsa = new RsaCrypto(2048, "pkcs8");
        var secondRsa = new RsaCrypto(2048, "pkcs8");
        var (firstPublicKey, _) = firstRsa.GenerateKeyPair();
        var (_, secondPrivateKey) = secondRsa.GenerateKeyPair();

        WinFormsTestHelper.SetPrivateField(control, "_publicKeyForValidation", Convert.ToBase64String(firstPublicKey));
        WinFormsTestHelper.SetPrivateField(control, "_privateKeyForValidation", Convert.ToBase64String(secondPrivateKey));

        var validationLabel = WinFormsTestHelper.GetPrivateField<Label>(control, "labelValidationResult");

        WinFormsTestHelper.ClickButton(control, "btnValidateKeyPair");

        Assert.Contains("不匹配", validationLabel.Text);
    }
}
