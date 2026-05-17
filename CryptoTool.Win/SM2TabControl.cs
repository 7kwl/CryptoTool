using CryptoTool.Algorithm.Algorithms.SM2;
using CryptoTool.Algorithm.Utils;
using CryptoTool.Win.Helpers;
using System.Text;

namespace CryptoTool.Win
{
    public partial class SM2TabControl : UserControl
    {
        public event Action<string>? StatusChanged;

        public SM2TabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            comboSM2KeyFormat.SelectedIndex = 0; // Base64
            comboSM2CipherFormat.SelectedIndex = 0; // C1C3C2
            comboSM2SignFormat.SelectedIndex = 0; // ASN1
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region SM2功能

        private void btnGenerateSM2Key_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成SM2密钥对...");

                var sm2Crypto = new Sm2Crypto();
                var keyPair = sm2Crypto.GenerateKeyPair();

                string? formatText = comboSM2KeyFormat.SelectedItem?.ToString();

                if (formatText == "Base64")
                {
                    textSM2PublicKey.Text = Convert.ToBase64String(keyPair.PublicKey);
                    textSM2PrivateKey.Text = Convert.ToBase64String(keyPair.PrivateKey);
                }
                else // Hex
                {
                    textSM2PublicKey.Text = StringUtil.BytesToHex(keyPair.PublicKey, false);
                    textSM2PrivateKey.Text = StringUtil.BytesToHex(keyPair.PrivateKey, false);
                }

                SetStatus($"SM2密钥对生成完成 - {formatText}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成SM2密钥对失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成SM2密钥对失败");
            }
        }

        private void btnImportSM2Key_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "密钥文件 (*.txt;*.key)|*.txt;*.key|所有文件 (*.*)|*.*";
                    openFileDialog.Title = "导入SM2密钥文件";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        var (publicKey, privateKey) = Sm2UiHelper.ParseKeyFileContent(content);

                        if (!string.IsNullOrWhiteSpace(publicKey) && !string.IsNullOrWhiteSpace(privateKey))
                        {
                            textSM2PublicKey.Text = publicKey;
                            textSM2PrivateKey.Text = privateKey;
                            SetStatus("SM2密钥导入成功");
                        }
                        else
                        {
                            MessageBox.Show("密钥文件格式错误！应包含公钥和私钥两行。", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入SM2密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导入SM2密钥失败");
            }
        }

        private void btnExportSM2Key_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM2PublicKey.Text) || string.IsNullOrEmpty(textSM2PrivateKey.Text))
                {
                    MessageBox.Show("请先生成或输入SM2密钥对！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "密钥文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                    saveFileDialog.Title = "导出SM2密钥文件";
                    saveFileDialog.FileName = "sm2_keys.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string content = Sm2UiHelper.BuildKeyFileContent(textSM2PublicKey.Text, textSM2PrivateKey.Text);
                        File.WriteAllText(saveFileDialog.FileName, content, Encoding.UTF8);
                        SetStatus("SM2密钥导出成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出SM2密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导出SM2密钥失败");
            }
        }

        private void btnSM2Encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM2PlainText.Text))
                {
                    MessageBox.Show("请输入明文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM2PublicKey.Text))
                {
                    MessageBox.Show("请先生成或输入SM2公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行SM2加密...");

                string formatText = comboSM2KeyFormat.SelectedItem?.ToString() ?? "";

                var sm2Crypto = new Sm2Crypto();
                byte[] publicKeyBytes;
                byte[] dataBytes = Encoding.UTF8.GetBytes(textSM2PlainText.Text);

                if (formatText == "Base64")
                {
                    publicKeyBytes = Convert.FromBase64String(textSM2PublicKey.Text);
                }
                else // Hex
                {
                    publicKeyBytes = StringUtil.HexToBytes(textSM2PublicKey.Text);
                }

                var cipherFormat = Sm2UiHelper.ParseCipherFormatSelection(
                    comboSM2CipherFormat.SelectedItem?.ToString());
                byte[] encryptedBytes = sm2Crypto.Encrypt(dataBytes, publicKeyBytes, cipherFormat);
                string cipherText = Convert.ToBase64String(encryptedBytes);

                textSM2CipherText.Text = cipherText;
                SetStatus($"SM2加密完成 - 密钥{formatText}格式，密文{comboSM2CipherFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM2加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("SM2加密失败");
            }
        }

        private void btnSM2Decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM2CipherText.Text))
                {
                    MessageBox.Show("请输入密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM2PrivateKey.Text))
                {
                    MessageBox.Show("请先生成或输入SM2私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行SM2解密...");

                string formatText = comboSM2KeyFormat.SelectedItem?.ToString() ?? "";

                var sm2Crypto = new Sm2Crypto();
                byte[] privateKeyBytes;
                byte[] cipherBytes = Convert.FromBase64String(textSM2CipherText.Text);

                if (formatText == "Base64")
                {
                    privateKeyBytes = Convert.FromBase64String(textSM2PrivateKey.Text);
                }
                else // Hex
                {
                    privateKeyBytes = StringUtil.HexToBytes(textSM2PrivateKey.Text);
                }

                var cipherFormat = Sm2UiHelper.ParseCipherFormatSelection(
                    comboSM2CipherFormat.SelectedItem?.ToString());
                byte[] decryptedBytes = sm2Crypto.Decrypt(cipherBytes, privateKeyBytes, cipherFormat);
                string plainText = Encoding.UTF8.GetString(decryptedBytes);

                textSM2PlainText.Text = plainText;
                SetStatus($"SM2解密完成 - 密钥{formatText}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM2解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("SM2解密失败");
            }
        }

        private void btnSM2Sign_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM2SignData.Text))
                {
                    MessageBox.Show("请输入要签名的原文数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM2PrivateKey.Text))
                {
                    MessageBox.Show("请先生成或输入SM2私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行SM2签名...");

                string formatText = comboSM2KeyFormat.SelectedItem?.ToString() ?? "";

                var sm2Crypto = new Sm2Crypto();
                byte[] privateKeyBytes;
                byte[] dataBytes = Encoding.UTF8.GetBytes(textSM2SignData.Text);

                if (formatText == "Base64")
                {
                    privateKeyBytes = Convert.FromBase64String(textSM2PrivateKey.Text);
                }
                else // Hex
                {
                    privateKeyBytes = StringUtil.HexToBytes(textSM2PrivateKey.Text);
                }

                byte[] signatureBytes = sm2Crypto.Sign(dataBytes, privateKeyBytes);
                signatureBytes = Sm2UiHelper.ConvertSignatureForOutput(
                    signatureBytes,
                    comboSM2SignFormat.SelectedItem?.ToString());
                string signature = Convert.ToBase64String(signatureBytes);

                textSM2Signature.Text = signature;
                SetStatus($"SM2签名完成 - 密钥{formatText}格式，签名{comboSM2SignFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM2签名失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("SM2签名失败");
            }
        }

        private void btnSM2Verify_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM2SignData.Text))
                {
                    MessageBox.Show("请输入原文数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM2Signature.Text))
                {
                    MessageBox.Show("请输入签名数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM2PublicKey.Text))
                {
                    MessageBox.Show("请先生成或输入SM2公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行SM2验签...");

                string formatText = comboSM2KeyFormat.SelectedItem?.ToString() ?? "";

                var sm2Crypto = new Sm2Crypto();
                byte[] publicKeyBytes;
                byte[] dataBytes = Encoding.UTF8.GetBytes(textSM2SignData.Text);
                byte[] signatureBytes = Convert.FromBase64String(textSM2Signature.Text);
                signatureBytes = Sm2UiHelper.NormalizeSignatureForVerify(
                    signatureBytes,
                    comboSM2SignFormat.SelectedItem?.ToString());

                if (formatText == "Base64")
                {
                    publicKeyBytes = Convert.FromBase64String(textSM2PublicKey.Text);
                }
                else // Hex
                {
                    publicKeyBytes = StringUtil.HexToBytes(textSM2PublicKey.Text);
                }

                bool verifyResult = sm2Crypto.VerifySign(dataBytes, signatureBytes, publicKeyBytes);

                labelSM2VerifyResult.Text = $"验签结果: {(verifyResult ? "验证成功" : "验证失败")}";
                labelSM2VerifyResult.ForeColor = verifyResult ? Color.Green : Color.Red;

                SetStatus($"SM2验签完成 - {(verifyResult ? "验证成功" : "验证失败")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM2验签失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelSM2VerifyResult.Text = "验签结果: 验证异常";
                labelSM2VerifyResult.ForeColor = Color.Red;
                SetStatus("SM2验签失败");
            }
        }

        #endregion
    }
}
