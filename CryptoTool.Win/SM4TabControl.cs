using CryptoTool.Algorithm.Algorithms.SM4;
using CryptoTool.Algorithm.Enums;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using System.Text;

namespace CryptoTool.Win
{
    public partial class SM4TabControl : UserControl
    {
        public event Action<string> StatusChanged;

        public SM4TabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            comboSM4Mode.SelectedIndex = 0; // ECB
            comboSM4Padding.SelectedIndex = 0; // PKCS7
            comboSM4KeyFormat.SelectedIndex = 0; // Base64
            comboSM4IVFormat.SelectedIndex = 0; // Base64
            comboSM4PlaintextFormat.SelectedIndex = 0; // Text
            comboSM4CiphertextFormat.SelectedIndex = 0; // Base64
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        public void UpdateKeyFromMedicare(string key)
        {
            textSM4Key.Text = key;
            comboSM4KeyFormat.SelectedItem = "Hex";
        }

        #region SM4功能

        private void btnGenerateSM4Key_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成SM4密钥...");

                UIOutputFormat keyFormat = FormatConversionHelper.ParseOutputFormat(comboSM4KeyFormat.SelectedItem?.ToString() ?? "");

                var sm4Crypto = new Sm4Crypto();
                byte[] keyBytes = sm4Crypto.GenerateKey();

                // 转换为用户指定的格式
                string key = FormatConversionHelper.BytesToString(keyBytes, keyFormat);

                textSM4Key.Text = key;
                SetStatus($"SM4密钥生成完成 - {comboSM4KeyFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成SM4密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成SM4密钥失败");
            }
        }

        private void btnGenerateSM4IV_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成SM4初始向量...");

                UIOutputFormat ivFormat = FormatConversionHelper.ParseOutputFormat(comboSM4IVFormat.SelectedItem?.ToString() ?? "");

                var sm4Crypto = new Sm4Crypto();
                byte[] ivBytes = sm4Crypto.GenerateIV();

                // 转换为用户指定的格式
                string iv = FormatConversionHelper.BytesToString(ivBytes, ivFormat);

                textSM4IV.Text = iv;
                SetStatus($"SM4初始向量生成完成 - {comboSM4IVFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成SM4初始向量失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成SM4初始向量失败");
            }
        }

        private void btnSM4Encrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM4PlainText.Text))
                {
                    MessageBox.Show("请输入明文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM4Key.Text))
                {
                    MessageBox.Show("请先生成或输入SM4密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboSM4Mode.SelectedItem?.ToString() ?? "";
                if (mode == "CBC" && string.IsNullOrEmpty(textSM4IV.Text))
                {
                    MessageBox.Show("CBC模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行SM4加密...");

                // 解析参数
                UIInputFormat plaintextFormat = FormatConversionHelper.ParseInputFormat(comboSM4PlaintextFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboSM4KeyFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboSM4IVFormat.SelectedItem?.ToString() ?? "");
                UIOutputFormat outputFormat = FormatConversionHelper.ParseOutputFormat(comboSM4CiphertextFormat.SelectedItem?.ToString() ?? "");

                // 处理输入数据
                string plaintext = GetPlaintextFromFormat(plaintextFormat);
                byte[] keyBytes = FormatConversionHelper.StringToBytes(textSM4Key.Text, keyFormat);
                byte[]? ivBytes = string.IsNullOrEmpty(textSM4IV.Text) ? null : FormatConversionHelper.StringToBytes(textSM4IV.Text, ivFormat);

                var sm4Crypto = SymmetricCryptoOptionHelper.CreateSm4Crypto(
                    comboSM4Mode.SelectedItem?.ToString(),
                    comboSM4Padding.SelectedItem?.ToString());

                // 执行加密
                byte[] dataBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = sm4Crypto.Encrypt(dataBytes, keyBytes, ivBytes);

                // 转换输出格式
                string cipherText = FormatConversionHelper.BytesToString(encryptedBytes, outputFormat);

                textSM4CipherText.Text = cipherText;

                SetStatus($"SM4加密完成 - 使用{mode}模式，输出{comboSM4CiphertextFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM4加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("SM4加密失败");
            }
        }

        private void btnSM4Decrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM4CipherText.Text))
                {
                    MessageBox.Show("请输入密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM4Key.Text))
                {
                    MessageBox.Show("请先生成或输入SM4密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboSM4Mode.SelectedItem?.ToString() ?? "";
                if (mode == "CBC" && string.IsNullOrEmpty(textSM4IV.Text))
                {
                    MessageBox.Show("CBC模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行SM4解密...");

                // 解析参数
                UIInputFormat ciphertextFormat = FormatConversionHelper.ParseInputFormat(comboSM4CiphertextFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboSM4KeyFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboSM4IVFormat.SelectedItem?.ToString() ?? "");
                UIOutputFormat plaintextFormat = FormatConversionHelper.ParseOutputFormat(comboSM4PlaintextFormat.SelectedItem?.ToString() ?? "");

                // 处理输入数据
                byte[] cipherBytes = FormatConversionHelper.StringToBytes(textSM4CipherText.Text, ciphertextFormat);
                byte[] keyBytes = FormatConversionHelper.StringToBytes(textSM4Key.Text, keyFormat);
                byte[]? ivBytes = string.IsNullOrEmpty(textSM4IV.Text) ? null : FormatConversionHelper.StringToBytes(textSM4IV.Text, ivFormat);

                // 创建SM4加密器
                var sm4Crypto = SymmetricCryptoOptionHelper.CreateSm4Crypto(
                    comboSM4Mode.SelectedItem?.ToString(),
                    comboSM4Padding.SelectedItem?.ToString());

                // 执行解密
                byte[] decryptedBytes = sm4Crypto.Decrypt(cipherBytes, keyBytes, ivBytes);
                string plainText = Encoding.UTF8.GetString(decryptedBytes);

                // 设置解密结果
                SetPlaintextFromFormat(plainText, plaintextFormat);

                SetStatus($"SM4解密完成 - 使用{mode}模式，输入{comboSM4CiphertextFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM4解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("SM4解密失败");
            }
        }

        private void comboSM4Mode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选择ECB模式时，禁用初始向量相关控件
            bool isCBC = comboSM4Mode.SelectedItem?.ToString() == "CBC";
            textSM4IV.Enabled = isCBC;
            btnGenerateSM4IV.Enabled = isCBC;
            comboSM4IVFormat.Enabled = isCBC;

            if (!isCBC)
            {
                textSM4IV.Text = "";
            }
        }

        private void comboSM4IVFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当格式变化时，如果当前有IV内容，尝试转换格式
            if (string.IsNullOrEmpty(textSM4IV.Text)) return;

            try
            {
                // 暂不实现自动格式转换，避免错误转换
                // 用户需要重新生成IV或手动输入正确格式的IV
            }
            catch
            {
                // 格式转换失败，忽略
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 根据格式获取明文内容
        /// </summary>
        private string GetPlaintextFromFormat(UIInputFormat format)
        {
            string plaintext = textSM4PlainText.Text;

            // 如果是UTF8格式，直接返回
            if (format == UIInputFormat.UTF8)
                return plaintext;

            // 其他格式需要先转换为字节数组再转换为UTF8字符串
            try
            {
                byte[] bytes = FormatConversionHelper.StringToBytes(plaintext, format);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                // 如果转换失败，直接返回原文本
                return plaintext;
            }
        }

        /// <summary>
        /// 根据格式设置明文内容
        /// </summary>
        private void SetPlaintextFromFormat(string decryptedText, UIOutputFormat format)
        {
            if (format == UIOutputFormat.UTF8)
            {
                textSM4PlainText.Text = decryptedText;
            }
            else
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(decryptedText);
                    textSM4PlainText.Text = FormatConversionHelper.BytesToString(bytes, format);
                }
                catch
                {
                    textSM4PlainText.Text = decryptedText; // 如果转换失败，直接显示
                }
            }
        }

        /// <summary>
        /// 将UI格式的数据转换为Provider需要的格式（UTF8字符串）
        /// </summary>
        private string ConvertToProviderFormat(string input, UIInputFormat format)
        {
            if (format == UIInputFormat.UTF8)
                return input;

            try
            {
                byte[] bytes = FormatConversionHelper.StringToBytes(input, format);
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                // 如果转换失败，直接返回原始输入
                return input;
            }
        }

        #endregion
    }
}
