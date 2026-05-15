using CryptoTool.Algorithm.Algorithms.AES;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using System.Text;

namespace CryptoTool.Win
{
    public partial class AESTabControl : UserControl
    {
        public event Action<string> StatusChanged;

        public AESTabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            comboAESMode.SelectedIndex = 0; // CBC
            comboAESPadding.SelectedIndex = 0; // PKCS7
            comboAESKeyFormat.SelectedIndex = 0; // Base64
            comboAESIVFormat.SelectedIndex = 0; // Base64
            comboAESPlaintextFormat.SelectedIndex = 0; // Text
            comboAESCiphertextFormat.SelectedIndex = 0; // Base64
            comboAESKeySize.SelectedIndex = 2; // AES256
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region AES功能

        private void btnGenerateAESKey_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成AES密钥...");

                string keySizeText = comboAESKeySize.SelectedItem?.ToString() ?? "";
                int keySize = int.Parse(keySizeText.Replace("AES", "").Replace("位", ""));
                UIOutputFormat keyFormat = FormatConversionHelper.ParseOutputFormat(comboAESKeyFormat.SelectedItem?.ToString() ?? "");

                var aesCrypto = new AesCrypto(keySize);
                byte[] keyBytes = aesCrypto.GenerateKey();

                // 转换为用户指定的格式
                string key = FormatConversionHelper.BytesToString(keyBytes, keyFormat);

                textAESKey.Text = key;
                SetStatus($"AES密钥生成完成 - {keySizeText}位，{comboAESKeyFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成AES密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成AES密钥失败");
            }
        }

        private void btnGenerateAESIV_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成AES初始向量...");

                UIOutputFormat ivFormat = FormatConversionHelper.ParseOutputFormat(comboAESIVFormat.SelectedItem?.ToString() ?? "");

                string keySizeText = comboAESKeySize.SelectedItem?.ToString() ?? "AES256";
                int keySize = int.Parse(keySizeText.Replace("AES", "").Replace("位", ""));
                var aesCrypto = new AesCrypto(keySize);
                byte[] ivBytes = aesCrypto.GenerateIV();

                // 转换为用户指定的格式
                string iv = FormatConversionHelper.BytesToString(ivBytes, ivFormat);

                textAESIV.Text = iv;
                SetStatus($"AES初始向量生成完成 - {comboAESIVFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成AES初始向量失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成AES初始向量失败");
            }
        }

        private void btnAESEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textAESPlainText.Text))
                {
                    MessageBox.Show("请输入明文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textAESKey.Text))
                {
                    MessageBox.Show("请先生成或输入AES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboAESMode.SelectedItem?.ToString() ?? "";
                if (mode != "ECB" && string.IsNullOrEmpty(textAESIV.Text))
                {
                    MessageBox.Show($"{mode}模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行AES加密...");

                // 解析参数
                UIInputFormat plaintextFormat = FormatConversionHelper.ParseInputFormat(comboAESPlaintextFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboAESKeyFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboAESIVFormat.SelectedItem?.ToString() ?? "");
                UIOutputFormat outputFormat = FormatConversionHelper.ParseOutputFormat(comboAESCiphertextFormat.SelectedItem?.ToString() ?? "");

                // 处理输入数据
                string plaintext = GetPlaintextFromFormat(plaintextFormat);
                byte[] keyBytes = FormatConversionHelper.StringToBytes(textAESKey.Text, keyFormat);
                byte[]? ivBytes = string.IsNullOrEmpty(textAESIV.Text) ? null : FormatConversionHelper.StringToBytes(textAESIV.Text, ivFormat);

                // 创建AES加密器
                var aesCrypto = SymmetricCryptoOptionHelper.CreateAesCrypto(
                    comboAESKeySize.SelectedItem?.ToString(),
                    keyBytes,
                    comboAESMode.SelectedItem?.ToString(),
                    comboAESPadding.SelectedItem?.ToString());

                // 执行加密
                byte[] dataBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = aesCrypto.Encrypt(dataBytes, keyBytes, ivBytes);

                // 转换输出格式
                string cipherText = FormatConversionHelper.BytesToString(encryptedBytes, outputFormat);

                textAESCipherText.Text = cipherText;

                SetStatus($"AES加密完成 - 使用{mode}模式，输出{comboAESCiphertextFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AES加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("AES加密失败");
            }
        }

        private void btnAESDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textAESCipherText.Text))
                {
                    MessageBox.Show("请输入密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textAESKey.Text))
                {
                    MessageBox.Show("请先生成或输入AES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboAESMode.SelectedItem?.ToString() ?? "";
                if (mode != "ECB" && string.IsNullOrEmpty(textAESIV.Text))
                {
                    MessageBox.Show($"{mode}模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行AES解密...");

                // 解析参数
                UIInputFormat ciphertextFormat = FormatConversionHelper.ParseInputFormat(comboAESCiphertextFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboAESKeyFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboAESIVFormat.SelectedItem?.ToString() ?? "");
                UIOutputFormat plaintextFormat = FormatConversionHelper.ParseOutputFormat(comboAESPlaintextFormat.SelectedItem?.ToString() ?? "");

                // 处理输入数据
                byte[] cipherBytes = FormatConversionHelper.StringToBytes(textAESCipherText.Text, ciphertextFormat);
                byte[] keyBytes = FormatConversionHelper.StringToBytes(textAESKey.Text, keyFormat);
                byte[]? ivBytes = string.IsNullOrEmpty(textAESIV.Text) ? null : FormatConversionHelper.StringToBytes(textAESIV.Text, ivFormat);

                // 创建AES加密器
                var aesCrypto = new AesCrypto();

                // 执行解密
                byte[] decryptedBytes = aesCrypto.Decrypt(cipherBytes, keyBytes, ivBytes);
                string plainText = Encoding.UTF8.GetString(decryptedBytes);

                // 设置解密结果
                SetPlaintextFromFormat(plainText, plaintextFormat);

                SetStatus($"AES解密完成 - 使用{mode}模式，输入{comboAESCiphertextFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"AES解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("AES解密失败");
            }
        }

        private void comboAESMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选择ECB模式时，禁用初始向量相关控件
            bool needsIV = comboAESMode.SelectedItem?.ToString() != "ECB";
            textAESIV.Enabled = needsIV;
            btnGenerateAESIV.Enabled = needsIV;
            comboAESIVFormat.Enabled = needsIV;
            labelAESIVFormat.Enabled = needsIV;

            if (!needsIV)
            {
                textAESIV.Text = "";
            }
        }

        #endregion

        #region 文件操作

        private void btnEncryptFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openDialog = new OpenFileDialog())
                {
                    openDialog.Title = "选择要加密的文件";
                    openDialog.Filter = "所有文件|*.*";

                    if (openDialog.ShowDialog() != DialogResult.OK)
                        return;

                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Title = "保存加密文件";
                        saveDialog.Filter = "加密文件|*.enc|所有文件|*.*";
                        saveDialog.FileName = Path.GetFileNameWithoutExtension(openDialog.FileName) + ".enc";

                        if (saveDialog.ShowDialog() != DialogResult.OK)
                            return;

                        if (string.IsNullOrEmpty(textAESKey.Text))
                        {
                            MessageBox.Show("请先生成或输入AES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string mode = comboAESMode.SelectedItem?.ToString() ?? "";
                        if (mode != "ECB" && string.IsNullOrEmpty(textAESIV.Text))
                        {
                            MessageBox.Show($"{mode}模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        SetStatus("正在加密文件...");

                        // 解析参数
                        UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboAESKeyFormat.SelectedItem?.ToString() ?? "");
                        UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboAESIVFormat.SelectedItem?.ToString() ?? "");

                        // 处理输入数据
                        byte[] keyBytes = FormatConversionHelper.StringToBytes(textAESKey.Text, keyFormat);
                        byte[]? ivBytes = string.IsNullOrEmpty(textAESIV.Text) ? null : FormatConversionHelper.StringToBytes(textAESIV.Text, ivFormat);

                        var aesCrypto = SymmetricCryptoOptionHelper.CreateAesCrypto(
                            comboAESKeySize.SelectedItem?.ToString(),
                            keyBytes,
                            comboAESMode.SelectedItem?.ToString(),
                            comboAESPadding.SelectedItem?.ToString());
                        byte[] fileData = File.ReadAllBytes(openDialog.FileName);
                        byte[] encryptedData = aesCrypto.Encrypt(fileData, keyBytes, ivBytes);
                        File.WriteAllBytes(saveDialog.FileName, encryptedData);

                        SetStatus($"文件加密完成：{saveDialog.FileName}");
                        MessageBox.Show("文件加密完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"文件加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("文件加密失败");
            }
        }

        private void btnDecryptFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openDialog = new OpenFileDialog())
                {
                    openDialog.Title = "选择要解密的文件";
                    openDialog.Filter = "加密文件|*.enc|所有文件|*.*";

                    if (openDialog.ShowDialog() != DialogResult.OK)
                        return;

                    using (SaveFileDialog saveDialog = new SaveFileDialog())
                    {
                        saveDialog.Title = "保存解密文件";
                        saveDialog.Filter = "所有文件|*.*";
                        string originalName = Path.GetFileNameWithoutExtension(openDialog.FileName);
                        if (originalName.EndsWith(".enc"))
                            originalName = originalName.Substring(0, originalName.Length - 4);
                        saveDialog.FileName = originalName;

                        if (saveDialog.ShowDialog() != DialogResult.OK)
                            return;

                        if (string.IsNullOrEmpty(textAESKey.Text))
                        {
                            MessageBox.Show("请先输入AES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string mode = comboAESMode.SelectedItem?.ToString() ?? "";
                        if (mode != "ECB" && string.IsNullOrEmpty(textAESIV.Text))
                        {
                            MessageBox.Show($"{mode}模式需要初始向量，请先输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        SetStatus("正在解密文件...");

                        // 解析参数
                        UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboAESKeyFormat.SelectedItem?.ToString() ?? "");
                        UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboAESIVFormat.SelectedItem?.ToString() ?? "");

                        // 处理输入数据
                        byte[] keyBytes = FormatConversionHelper.StringToBytes(textAESKey.Text, keyFormat);
                        byte[]? ivBytes = string.IsNullOrEmpty(textAESIV.Text) ? null : FormatConversionHelper.StringToBytes(textAESIV.Text, ivFormat);

                        var aesCrypto = SymmetricCryptoOptionHelper.CreateAesCrypto(
                            comboAESKeySize.SelectedItem?.ToString(),
                            keyBytes,
                            comboAESMode.SelectedItem?.ToString(),
                            comboAESPadding.SelectedItem?.ToString());
                        byte[] encryptedData = File.ReadAllBytes(openDialog.FileName);
                        byte[] decryptedData = aesCrypto.Decrypt(encryptedData, keyBytes, ivBytes);
                        File.WriteAllBytes(saveDialog.FileName, decryptedData);

                        SetStatus($"文件解密完成：{saveDialog.FileName}");
                        MessageBox.Show("文件解密完成！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"文件解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("文件解密失败");
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 根据格式获取明文内容
        /// </summary>
        private string GetPlaintextFromFormat(UIInputFormat format)
        {
            string plaintext = textAESPlainText.Text;

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
                textAESPlainText.Text = decryptedText;
            }
            else
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(decryptedText);
                    textAESPlainText.Text = FormatConversionHelper.BytesToString(bytes, format);
                }
                catch
                {
                    textAESPlainText.Text = decryptedText; // 如果转换失败，直接显示
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
