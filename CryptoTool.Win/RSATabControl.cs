using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.Algorithm.Enums;
using CryptoTool.Win.Helpers;
using System.Text;

namespace CryptoTool.Win
{
    public partial class RSATabControl : UserControl
    {
        public event Action<string>? StatusChanged;

        public RSATabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            // 初始化默认值
            comboRSAKeySize.SelectedIndex = 1; // 2048
            comboRSAKeyFormat.SelectedIndex = 1; // pkcs#8
            comboRSAKeyPadding.SelectedIndex = 0; // PKCS1
            comboRSAKeyOutputFormat.SelectedIndex = 1; // base64
            comboRSAEncryptOutputFormat.SelectedIndex = 0; // base64
            comboRSASignAlgmFormat.SelectedIndex = 1; // SHA256withRSA(RSA2)
            comboRSASignOutputFormat.SelectedIndex = 0; // base64
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region RSA功能

        private void btnGenerateRSAKey_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成RSA密钥对...");

                string keySizeStr = comboRSAKeySize.SelectedItem?.ToString() ?? "2048";
                var keyFormat = comboRSAKeyFormat.SelectedItem?.ToString() ?? "PKCS8";
                var keyOutputFormat = comboRSAKeyOutputFormat.SelectedItem?.ToString() ?? "Base64";
                int keySize = int.Parse(keySizeStr);

                var rsaCrypto = new RsaCrypto(keySize, keyFormat.ToLowerInvariant());
                var keyPair = rsaCrypto.GenerateKeyPair();

                textRSAPublicKey.Text = RsaUiHelper.WriteKeyText(keyPair.PublicKey, keyOutputFormat, false, keyFormat);
                textRSAPrivateKey.Text = RsaUiHelper.WriteKeyText(keyPair.PrivateKey, keyOutputFormat, true, keyFormat);

                SetStatus($"RSA密钥对生成完成 - {keySize}位 {comboRSAKeyFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成RSA密钥对失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成RSA密钥对失败");
            }
        }

        private void btnImportRSAKey_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "密钥文件 (*.pem;*.key;*.txt)|*.pem;*.key;*.txt|所有文件 (*.*)|*.*";
                    openFileDialog.Title = "导入RSA密钥文件";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        var (publicKey, privateKey) = RsaUiHelper.ParseKeyFileContent(content);

                        if (!string.IsNullOrWhiteSpace(publicKey))
                            textRSAPublicKey.Text = publicKey;

                        if (!string.IsNullOrWhiteSpace(privateKey))
                            textRSAPrivateKey.Text = privateKey;

                        if ((publicKey != null && RsaUiHelper.LooksLikePem(publicKey))
                            || (privateKey != null && RsaUiHelper.LooksLikePem(privateKey)))
                        {
                            comboRSAKeyOutputFormat.SelectedItem = "PEM";
                            var pemText = publicKey ?? privateKey ?? string.Empty;
                            comboRSAKeyFormat.SelectedItem = RsaCrypto.DetectRsaKeyFormatFromPem(pemText).ToUpperInvariant();
                        }

                        SetStatus("RSA密钥文件导入完成");
                        MessageBox.Show("RSA密钥文件导入完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入RSA密钥文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导入RSA密钥文件失败");
            }
        }

        private void btnExportRSAKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textRSAPublicKey.Text) && string.IsNullOrEmpty(textRSAPrivateKey.Text))
                {
                    MessageBox.Show("没有可导出的密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "密钥文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                    saveFileDialog.Title = "导出RSA密钥文件";
                    saveFileDialog.FileName = "rsa_keypair.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        StringBuilder content = new StringBuilder();
                        if (!string.IsNullOrEmpty(textRSAPublicKey.Text))
                        {
                            if (RsaUiHelper.LooksLikePem(textRSAPublicKey.Text))
                            {
                                content.AppendLine(textRSAPublicKey.Text.Trim());
                                content.AppendLine();
                            }
                            else
                            {
                                content.AppendLine("# RSA公钥");
                                content.AppendLine(textRSAPublicKey.Text);
                                content.AppendLine();
                            }
                        }
                        if (!string.IsNullOrEmpty(textRSAPrivateKey.Text))
                        {
                            if (RsaUiHelper.LooksLikePem(textRSAPrivateKey.Text))
                            {
                                content.AppendLine(textRSAPrivateKey.Text.Trim());
                            }
                            else
                            {
                                content.AppendLine("# RSA私钥");
                                content.AppendLine(textRSAPrivateKey.Text);
                            }
                        }

                        File.WriteAllText(saveFileDialog.FileName, content.ToString(), Encoding.UTF8);
                        SetStatus("RSA密钥文件导出完成");
                        MessageBox.Show("RSA密钥文件导出完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出RSA密钥文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导出RSA密钥文件失败");
            }
        }

        private void btnRSAEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textRSAPlainText.Text))
                {
                    MessageBox.Show("请输入要加密的明文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textRSAPublicKey.Text))
                {
                    MessageBox.Show("请先生成或输入RSA公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行RSA加密...");

                var rsaCrypto = new RsaCrypto();
                byte[] publicKeyBytes = RsaUiHelper.ReadKeyBytes(
                    textRSAPublicKey.Text,
                    comboRSAKeyOutputFormat.SelectedItem?.ToString(),
                    false,
                    comboRSAKeyFormat.SelectedItem?.ToString());
                byte[] dataBytes = Encoding.UTF8.GetBytes(textRSAPlainText.Text);
                byte[] encryptedBytes = rsaCrypto.Encrypt(
                    dataBytes,
                    publicKeyBytes,
                    RsaUiHelper.ParsePadding(comboRSAKeyPadding.SelectedItem?.ToString()));

                textRSACipherText.Text = RsaUiHelper.WriteBinaryText(
                    encryptedBytes,
                    comboRSAEncryptOutputFormat.SelectedItem?.ToString());

                SetStatus($"RSA加密完成 - 使用{comboRSAKeyPadding.SelectedItem}填充，输出{comboRSAEncryptOutputFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RSA加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("RSA加密失败");
            }
        }

        private void btnRSADecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textRSACipherText.Text))
                {
                    MessageBox.Show("请输入要解密的密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textRSAPrivateKey.Text))
                {
                    MessageBox.Show("请先生成或输入RSA私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行RSA解密...");

                var rsaCrypto = new RsaCrypto();
                byte[] privateKeyBytes = RsaUiHelper.ReadKeyBytes(
                    textRSAPrivateKey.Text,
                    comboRSAKeyOutputFormat.SelectedItem?.ToString(),
                    true,
                    comboRSAKeyFormat.SelectedItem?.ToString());
                byte[] cipherBytes = RsaUiHelper.ReadBinaryText(
                    textRSACipherText.Text,
                    comboRSAEncryptOutputFormat.SelectedItem?.ToString());
                byte[] decryptedBytes = rsaCrypto.Decrypt(
                    cipherBytes,
                    privateKeyBytes,
                    RsaUiHelper.ParsePadding(comboRSAKeyPadding.SelectedItem?.ToString()));

                textRSAPlainText.Text = Encoding.UTF8.GetString(decryptedBytes);

                SetStatus($"RSA解密完成 - 使用{comboRSAKeyPadding.SelectedItem}填充，输入{comboRSAEncryptOutputFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RSA解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("RSA解密失败");
            }
        }

        private void btnRSASign_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textRSASignData.Text))
                {
                    MessageBox.Show("请输入要签名的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textRSAPrivateKey.Text))
                {
                    MessageBox.Show("请先生成或输入RSA私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行RSA签名...");

                var rsaCrypto = new RsaCrypto();
                byte[] privateKeyBytes = RsaUiHelper.ReadKeyBytes(
                    textRSAPrivateKey.Text,
                    comboRSAKeyOutputFormat.SelectedItem?.ToString(),
                    true,
                    comboRSAKeyFormat.SelectedItem?.ToString());
                byte[] dataBytes = Encoding.UTF8.GetBytes(textRSASignData.Text);
                byte[] signatureBytes = rsaCrypto.Sign(
                    dataBytes,
                    privateKeyBytes,
                    RsaUiHelper.ParseSignatureAlgorithm(comboRSASignAlgmFormat.SelectedItem?.ToString()));

                textRSASignature.Text = RsaUiHelper.WriteBinaryText(
                    signatureBytes,
                    comboRSASignOutputFormat.SelectedItem?.ToString());

                SetStatus($"RSA签名完成 - 使用{comboRSASignAlgmFormat.SelectedItem}算法，输出{comboRSASignOutputFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RSA签名失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("RSA签名失败");
            }
        }

        private void btnRSAVerify_Click(object sender, EventArgs e)
        {
            try
            {
                string verifyData = textRSASignData.Text;
                string verifySignature = textRSASignature.Text;

                if (string.IsNullOrEmpty(verifyData))
                {
                    MessageBox.Show("请输入要验证的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(verifySignature))
                {
                    MessageBox.Show("请输入要验证的签名！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textRSAPublicKey.Text))
                {
                    MessageBox.Show("请先生成或输入RSA公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行RSA验签...");

                var rsaCrypto = new RsaCrypto();
                byte[] publicKeyBytes = RsaUiHelper.ReadKeyBytes(
                    textRSAPublicKey.Text,
                    comboRSAKeyOutputFormat.SelectedItem?.ToString(),
                    false,
                    comboRSAKeyFormat.SelectedItem?.ToString());
                byte[] dataBytes = Encoding.UTF8.GetBytes(verifyData);
                byte[] signatureBytes = RsaUiHelper.ReadBinaryText(
                    verifySignature,
                    comboRSASignOutputFormat.SelectedItem?.ToString());
                bool isValid = rsaCrypto.VerifySign(
                    dataBytes,
                    signatureBytes,
                    publicKeyBytes,
                    RsaUiHelper.ParseSignatureAlgorithm(comboRSASignAlgmFormat.SelectedItem?.ToString()));

                labelRSAVerifyResult.Text = isValid ? "验证通过" : "验证失败";
                labelRSAVerifyResult.ForeColor = isValid ? Color.Green : Color.Red;

                SetStatus($"RSA验签完成 - 结果：{(isValid ? "通过" : "失败")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"RSA验签失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelRSAVerifyResult.Text = "验证异常";
                labelRSAVerifyResult.ForeColor = Color.Red;
                SetStatus("RSA验签失败");
            }
        }

        #endregion

    }
}
