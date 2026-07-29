using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.UnitTests.TestHelpers;
using Org.BouncyCastle.Crypto.Parameters;

namespace CryptoTool.UnitTests;

/// <summary>
/// ECDSA TabControl UI 层级测试：模拟用户操作，验证签名/验签 UI 交互
/// </summary>
public class EcdsaTabControlTests
{
    [StaFact]
    public void SignWithDefaultSettings_ShouldProduceSignatureInTextBox()
    {
        using var control = new Win.EcdsaTabControl();

        // 获取私有控件
        var plainDataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPlainData");
        var signatureTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSignature");
        var hashCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboHashAlgorithm");
        var signatureFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSignatureFormat");

        // 设置测试数据
        plainDataTextBox.Text = "ECDSA UI 签名测试";

        // 确保选择有效格式
        if (hashCombo.Items.Count > 0) hashCombo.SelectedIndex = 0;
        if (signatureFormatCombo.Items.Count > 0) signatureFormatCombo.SelectedIndex = 0;

        // 触发签名按钮
        WinFormsTestHelper.ClickButton(control, "btnSign");

        // 验证签名文本框不为空
        Assert.False(string.IsNullOrEmpty(signatureTextBox.Text), "签名后文本框不应为空");
    }

    [StaFact]
    public void SignAndVerify_WithSameKey_ShouldShowVerifySuccess()
    {
        using var control = new Win.EcdsaTabControl();

        // 生成密钥对并使用 PEM 设置到控件
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var privateKeyPem = EcdsaKeyHelper.ExportPrivateKeyPem(privateKey);
        var publicKeyPem = EcdsaKeyHelper.ExportPublicKeyPem(publicKey);

        // 获取私有控件
        var privateKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPrivateKey");
        var publicKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPublicKey");
        var plainDataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPlainData");
        var signatureTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textSignature");
        var hashCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboHashAlgorithm");
        var signatureFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSignatureFormat");

        // 设置密钥和明文
        privateKeyTextBox.Text = privateKeyPem;
        publicKeyTextBox.Text = publicKeyPem;
        plainDataTextBox.Text = "签名验签往返测试";

        if (hashCombo.Items.Count > 0) hashCombo.SelectedIndex = 0;
        if (signatureFormatCombo.Items.Count > 0) signatureFormatCombo.SelectedIndex = 0;

        // 签名
        WinFormsTestHelper.ClickButton(control, "btnSign");
        Assert.False(string.IsNullOrEmpty(signatureTextBox.Text), "签名不应为空");

        // 验签
        WinFormsTestHelper.ClickButton(control, "btnVerify");

        // 验签结果写入 labelValidationResult（RichTextBox）
        var verifyResultBox = WinFormsTestHelper.GetPrivateField<RichTextBox>(control, "labelValidationResult");
        Assert.Contains("签名验证通过", verifyResultBox.Text);
    }

    [StaFact]
    public void Verify_TamperedData_ShouldShowVerifyFailed()
    {
        using var control = new Win.EcdsaTabControl();

        // 生成密钥对
        var keyPair = EcdsaAlgorithm.GenerateKeyPair("secp256r1");
        var privateKey = (ECPrivateKeyParameters)keyPair.Private;
        var publicKey = (ECPublicKeyParameters)keyPair.Public;

        var privateKeyPem = EcdsaKeyHelper.ExportPrivateKeyPem(privateKey);
        var publicKeyPem = EcdsaKeyHelper.ExportPublicKeyPem(publicKey);

        var privateKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPrivateKey");
        var publicKeyTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPublicKey");
        var plainDataTextBox = WinFormsTestHelper.GetPrivateField<TextBox>(control, "textPlainData");
        var hashCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboHashAlgorithm");
        var signatureFormatCombo = WinFormsTestHelper.GetPrivateField<ComboBox>(control, "comboSignatureFormat");

        // 签名
        privateKeyTextBox.Text = privateKeyPem;
        publicKeyTextBox.Text = publicKeyPem;
        plainDataTextBox.Text = "原始数据";

        if (hashCombo.Items.Count > 0) hashCombo.SelectedIndex = 0;
        if (signatureFormatCombo.Items.Count > 0) signatureFormatCombo.SelectedIndex = 0;

        WinFormsTestHelper.ClickButton(control, "btnSign");

        // 篡改明文
        plainDataTextBox.Text = "被篡改的数据";

        // 验签
        WinFormsTestHelper.ClickButton(control, "btnVerify");

        var verifyResultBox = WinFormsTestHelper.GetPrivateField<RichTextBox>(control, "labelValidationResult");
        Assert.Contains("签名验证失败", verifyResultBox.Text);
    }
}
