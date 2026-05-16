using CryptoTool.Algorithm.Algorithms.DES;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using System.Text;

namespace CryptoTool.Win
{
    public partial class DESTabControl : UserControl
    {
        public event Action<string> StatusChanged;

        public DESTabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            comboDESMode.SelectedIndex = 0; // CBC
            comboDESPadding.SelectedIndex = 0; // PKCS7
            comboDESKeyFormat.SelectedIndex = 0; // UTF8
            comboDESIVFormat.SelectedIndex = 0; // UTF8
            comboDESPlaintextFormat.SelectedIndex = 0; // UTF8
            comboDESCiphertextFormat.SelectedIndex = 0; // Base64
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region DES功能

        private void btnGenerateDESKey_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成DES密钥...");

                UIOutputFormat keyFormat = FormatConversionHelper.ParseOutputFormat(comboDESKeyFormat.SelectedItem?.ToString() ?? "");

                var desCrypto = new DesCrypto();
                byte[] keyBytes = desCrypto.GenerateKey();

                // 转换为用户指定的格式
                string key = FormatConversionHelper.BytesToString(keyBytes, keyFormat);

                textDESKey.Text = key;
                SetStatus($"DES密钥生成完成 - {comboDESKeyFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成DES密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成DES密钥失败");
            }
        }

        private void btnGenerateDESIV_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成DES初始向量...");

                UIOutputFormat ivFormat = FormatConversionHelper.ParseOutputFormat(comboDESIVFormat.SelectedItem?.ToString() ?? "");

                var desCrypto = new DesCrypto();
                byte[] ivBytes = desCrypto.GenerateIV();

                // 转换为用户指定的格式
                string iv = FormatConversionHelper.BytesToString(ivBytes, ivFormat);

                textDESIV.Text = iv;
                SetStatus($"DES初始向量生成完成 - {comboDESIVFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成DES初始向量失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成DES初始向量失败");
            }
        }

        private void btnDESEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textDESPlainText.Text))
                {
                    MessageBox.Show("请输入明文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textDESKey.Text))
                {
                    MessageBox.Show("请先生成或输入DES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboDESMode.SelectedItem?.ToString() ?? "";
                if (mode != "ECB" && string.IsNullOrEmpty(textDESIV.Text))
                {
                    MessageBox.Show($"{mode}模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行DES加密...");

                // 解析参数
                UIInputFormat plaintextFormat = FormatConversionHelper.ParseInputFormat(comboDESPlaintextFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboDESKeyFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboDESIVFormat.SelectedItem?.ToString() ?? "");
                UIOutputFormat outputFormat = FormatConversionHelper.ParseOutputFormat(comboDESCiphertextFormat.SelectedItem?.ToString() ?? "");

                // 处理输入数据
                string plaintext = GetPlaintextFromFormat(plaintextFormat);
                byte[] keyBytes = FormatConversionHelper.StringToBytes(textDESKey.Text, keyFormat);
                byte[]? ivBytes = string.IsNullOrEmpty(textDESIV.Text) ? null : FormatConversionHelper.StringToBytes(textDESIV.Text, ivFormat);

                // 创建DES加密器
                var desCrypto = SymmetricCryptoOptionHelper.CreateDesCrypto(
                    comboDESMode.SelectedItem?.ToString(),
                    comboDESPadding.SelectedItem?.ToString());

                // 执行加密
                byte[] dataBytes = Encoding.UTF8.GetBytes(plaintext);
                byte[] encryptedBytes = desCrypto.Encrypt(dataBytes, keyBytes, ivBytes);

                // 转换输出格式
                string cipherText = FormatConversionHelper.BytesToString(encryptedBytes, outputFormat);

                textDESCipherText.Text = cipherText;

                SetStatus($"DES加密完成 - 使用{mode}模式，输出{comboDESCiphertextFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DES加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("DES加密失败");
            }
        }

        private void btnDESDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textDESCipherText.Text))
                {
                    MessageBox.Show("请输入密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textDESKey.Text))
                {
                    MessageBox.Show("请先生成或输入DES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboDESMode.SelectedItem?.ToString() ?? "";
                if (mode != "ECB" && string.IsNullOrEmpty(textDESIV.Text))
                {
                    MessageBox.Show($"{mode}模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行DES解密...");

                // 解析参数
                UIInputFormat ciphertextFormat = FormatConversionHelper.ParseInputFormat(comboDESCiphertextFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboDESKeyFormat.SelectedItem?.ToString() ?? "");
                UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboDESIVFormat.SelectedItem?.ToString() ?? "");
                UIOutputFormat plaintextFormat = FormatConversionHelper.ParseOutputFormat(comboDESPlaintextFormat.SelectedItem?.ToString() ?? "");

                // 处理输入数据
                byte[] cipherBytes = FormatConversionHelper.StringToBytes(textDESCipherText.Text, ciphertextFormat);
                byte[] keyBytes = FormatConversionHelper.StringToBytes(textDESKey.Text, keyFormat);
                byte[]? ivBytes = string.IsNullOrEmpty(textDESIV.Text) ? null : FormatConversionHelper.StringToBytes(textDESIV.Text, ivFormat);

                // 创建DES加密器
                var desCrypto = SymmetricCryptoOptionHelper.CreateDesCrypto(
                    comboDESMode.SelectedItem?.ToString(),
                    comboDESPadding.SelectedItem?.ToString());

                // 执行解密
                byte[] decryptedBytes = desCrypto.Decrypt(cipherBytes, keyBytes, ivBytes);
                string plainText = Encoding.UTF8.GetString(decryptedBytes);

                // 设置解密结果
                SetPlaintextFromFormat(plainText, plaintextFormat);

                SetStatus($"DES解密完成 - 使用{mode}模式，输入{comboDESCiphertextFormat.SelectedItem}格式");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"DES解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("DES解密失败");
            }
        }

        private void comboDESMode_SelectedIndexChanged(object sender, EventArgs e)
        {
            // 当选择ECB模式时，禁用初始向量相关控件
            bool needsIV = comboDESMode.SelectedItem?.ToString() != "ECB";
            textDESIV.Enabled = needsIV;
            btnGenerateDESIV.Enabled = needsIV;
            btnConvertDESIV.Enabled = needsIV;
            comboDESIVFormat.Enabled = needsIV;
            labelDESIVFormat.Enabled = needsIV;

            if (!needsIV)
            {
                textDESIV.Text = "";
            }
        }

        private void btnConvertDESKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textDESKey.Text))
                {
                    MessageBox.Show("请先输入密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在转换密钥格式...");

                string? currentFormat = comboDESKeyFormat.SelectedItem?.ToString() ?? "";
                string newFormat;
                if (currentFormat == "UTF8") newFormat = "Base64";
                else if (currentFormat == "Base64") newFormat = "Hex";
                else newFormat = "UTF8";

                UIInputFormat fromFormat = FormatConversionHelper.ParseInputFormat(currentFormat);
                UIOutputFormat toFormat = FormatConversionHelper.ParseOutputFormat(newFormat);

                string convertedKey = FormatConversionHelper.ConvertStringFormat(textDESKey.Text, fromFormat, toFormat);
                textDESKey.Text = convertedKey;
                comboDESKeyFormat.SelectedItem = newFormat;

                SetStatus($"密钥格式转换完成 - 从{currentFormat}转换为{newFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"密钥格式转换失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("密钥格式转换失败");
            }
        }

        private void btnConvertDESIV_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textDESIV.Text))
                {
                    MessageBox.Show("请先输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在转换IV格式...");

                string? currentFormat = comboDESIVFormat.SelectedItem?.ToString() ?? "";
                string newFormat;
                if (currentFormat == "UTF8") newFormat = "Base64";
                else if (currentFormat == "Base64") newFormat = "Hex";
                else newFormat = "UTF8";

                UIInputFormat fromFormat = FormatConversionHelper.ParseInputFormat(currentFormat);
                UIOutputFormat toFormat = FormatConversionHelper.ParseOutputFormat(newFormat);

                string convertedIV = FormatConversionHelper.ConvertStringFormat(textDESIV.Text, fromFormat, toFormat);
                textDESIV.Text = convertedIV;
                comboDESIVFormat.SelectedItem = newFormat;

                SetStatus($"IV格式转换完成 - 从{currentFormat}转换为{newFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"IV格式转换失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("IV格式转换失败");
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

                        if (string.IsNullOrEmpty(textDESKey.Text))
                        {
                            MessageBox.Show("请先生成或输入DES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string? mode = comboDESMode.SelectedItem?.ToString();
                        if (mode != "ECB" && string.IsNullOrEmpty(textDESIV.Text))
                        {
                            MessageBox.Show($"{mode}模式需要初始向量，请先生成或输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        SetStatus("正在加密文件...");

                        // 解析参数
                        UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboDESKeyFormat.SelectedItem?.ToString() ?? "");
                        UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboDESIVFormat.SelectedItem?.ToString() ?? "");

                        // 处理输入数据
                        byte[] keyBytes = FormatConversionHelper.StringToBytes(textDESKey.Text, keyFormat);
                        byte[]? ivBytes = string.IsNullOrEmpty(textDESIV.Text) ? null : FormatConversionHelper.StringToBytes(textDESIV.Text, ivFormat);

                        var desCrypto = SymmetricCryptoOptionHelper.CreateDesCrypto(
                            comboDESMode.SelectedItem?.ToString(),
                            comboDESPadding.SelectedItem?.ToString());
                        byte[] fileData = File.ReadAllBytes(openDialog.FileName);
                        byte[] encryptedData = desCrypto.Encrypt(fileData, keyBytes, ivBytes);
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

                        if (string.IsNullOrEmpty(textDESKey.Text))
                        {
                            MessageBox.Show("请先输入DES密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        string? mode = comboDESMode.SelectedItem?.ToString();
                        if (mode != "ECB" && string.IsNullOrEmpty(textDESIV.Text))
                        {
                            MessageBox.Show($"{mode}模式需要初始向量，请先输入初始向量！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                            return;
                        }

                        SetStatus("正在解密文件...");

                        // 解析参数
                        UIInputFormat keyFormat = FormatConversionHelper.ParseInputFormat(comboDESKeyFormat.SelectedItem?.ToString() ?? "");
                        UIInputFormat ivFormat = FormatConversionHelper.ParseInputFormat(comboDESIVFormat.SelectedItem?.ToString() ?? "");

                        // 处理输入数据
                        byte[] keyBytes = FormatConversionHelper.StringToBytes(textDESKey.Text, keyFormat);
                        byte[]? ivBytes = string.IsNullOrEmpty(textDESIV.Text) ? null : FormatConversionHelper.StringToBytes(textDESIV.Text, ivFormat);

                        var desCrypto = SymmetricCryptoOptionHelper.CreateDesCrypto(
                            comboDESMode.SelectedItem?.ToString(),
                            comboDESPadding.SelectedItem?.ToString());
                        byte[] encryptedData = File.ReadAllBytes(openDialog.FileName);
                        byte[] decryptedData = desCrypto.Decrypt(encryptedData, keyBytes, ivBytes);
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
            string plaintext = textDESPlainText.Text;

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
                textDESPlainText.Text = decryptedText;
            }
            else
            {
                try
                {
                    byte[] bytes = Encoding.UTF8.GetBytes(decryptedText);
                    textDESPlainText.Text = FormatConversionHelper.BytesToString(bytes, format);
                }
                catch
                {
                    textDESPlainText.Text = decryptedText; // 如果转换失败，直接显示
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
