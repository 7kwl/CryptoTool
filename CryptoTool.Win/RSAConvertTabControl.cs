using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using System.Text;

namespace CryptoTool.Win
{
    public partial class RSAConvertTabControl : UserControl
    {
        public event Action<string> StatusChanged;

        // 存储导入的公钥和私钥用于验证
        private string _publicKeyForValidation = string.Empty;
        private string _privateKeyForValidation = string.Empty;

        public RSAConvertTabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            // 设置默认选项
            comboInputFormat.SelectedIndex = 0; // PEM
            comboInputKeyType.SelectedIndex = 1; // PKCS8
            comboOutputFormat.SelectedIndex = 1; // Base64
            comboOutputKeyType.SelectedIndex = 1; // PKCS8
            radioPrivateKey.Checked = true;

            // 初始化验证结果
            labelValidationResult.Text = "验证结果: 未验证";
            labelValidationResult.ForeColor = Color.Gray;
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region 事件处理

        private void btnValidateKeyPair_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(_publicKeyForValidation) || string.IsNullOrEmpty(_privateKeyForValidation))
                {
                    MessageBox.Show("请先转换密钥或从私钥提取公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在验证密钥对...");

                bool isValid = RsaKeyConversionService.ValidateKeyPair(_publicKeyForValidation, _privateKeyForValidation);
                labelValidationResult.Text = isValid ? "验证结果: 密钥对匹配" : "验证结果: 密钥对不匹配";
                labelValidationResult.ForeColor = isValid ? Color.Green : Color.Red;

                SetStatus($"密钥对验证完成 - 结果：{(isValid ? "匹配" : "不匹配")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"验证密钥对失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelValidationResult.Text = "验证结果: 验证异常";
                labelValidationResult.ForeColor = Color.Red;
                SetStatus("验证密钥对失败");
            }
        }

        private void btnGetPublicKeyFromPrivate_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textInputKey.Text))
                {
                    MessageBox.Show("请先输入私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (radioPublicKey.Checked)
                {
                    MessageBox.Show("请选择私钥类型才能提取公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在从私钥提取公钥...");

                string publicKeyString = RsaKeyConversionService.ExtractPublicKeyFromPrivate(
                    textInputKey.Text,
                    comboInputKeyType.SelectedItem?.ToString() ?? "PKCS8",
                    comboInputFormat.SelectedItem?.ToString() ?? "PEM",
                    comboOutputKeyType.SelectedItem?.ToString() ?? "PKCS8",
                    comboOutputFormat.SelectedItem?.ToString() ?? "Base64");

                textOutputKey.Text = publicKeyString;

                // 存储密钥用于验证
                _privateKeyForValidation = textInputKey.Text;
                _publicKeyForValidation = publicKeyString;

                // 自动验证密钥对
                labelValidationResult.Text = "验证结果: 密钥对匹配（从私钥提取）";
                labelValidationResult.ForeColor = Color.Green;

                SetStatus("从私钥提取公钥完成");
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show($"从私钥提取公钥失败：密钥类型不匹配\n\n详细信息：{ex.Message}\n\n请确认输入的是私钥而不是公钥", "类型错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("从私钥提取公钥失败 - 类型不匹配");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"从私钥提取公钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("从私钥提取公钥失败");
            }
        }

        private void btnConvert_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textInputKey.Text))
                {
                    MessageBox.Show("请先输入密钥内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行格式转换...");

                bool isPrivateKey = radioPrivateKey.Checked;
                string convertedKey = RsaKeyConversionService.ConvertKey(
                    textInputKey.Text,
                    isPrivateKey,
                    comboInputKeyType.SelectedItem?.ToString() ?? "PKCS8",
                    comboInputFormat.SelectedItem?.ToString() ?? "PEM",
                    comboOutputKeyType.SelectedItem?.ToString() ?? "PKCS8",
                    comboOutputFormat.SelectedItem?.ToString() ?? "Base64");

                textOutputKey.Text = convertedKey;
                if (isPrivateKey)
                {
                    _privateKeyForValidation = convertedKey;
                }
                else
                {
                    _publicKeyForValidation = convertedKey;
                }

                SetStatus($"格式转换完成 - {comboInputKeyType.SelectedItem}/{comboInputFormat.SelectedItem} -> {comboOutputKeyType.SelectedItem}/{comboOutputFormat.SelectedItem}");
            }
            catch (InvalidCastException ex)
            {
                MessageBox.Show($"密钥格式转换失败：密钥类型不匹配\n\n详细信息：{ex.Message}\n\n请检查：\n1. 选择的密钥类型（公钥/私钥）是否正确\n2. 输入格式和密钥类型选择是否匹配\n3. 尝试使用自动检测功能", "格式转换错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("格式转换失败 - 密钥类型不匹配");
            }
            catch (ArgumentException ex)
            {
                MessageBox.Show($"格式转换失败：{ex.Message}\n\n建议：\n1. 检查密钥内容是否完整\n2. 尝试不同的输入格式组合\n3. 使用自动检测功能", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("格式转换失败");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"格式转换失败：{ex.Message}\n\n如果问题持续存在：\n1. 请尝试清空内容重新输入\n2. 检查密钥是否来自可靠来源\n3. 尝试使用文件导入功能", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("格式转换失败");
            }
        }

        private void btnSaveToFile_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textOutputKey.Text))
                {
                    MessageBox.Show("没有可保存的内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    var outputFormat = FormatConversionHelper.ParseOutputFormat(comboOutputFormat.SelectedItem.ToString());
                    var keyType = radioPrivateKey.Checked ? "private" : "public";

                    string extension = outputFormat switch
                    {
                        UIOutputFormat.PEM => ".pem",
                        UIOutputFormat.Base64 => ".txt",
                        UIOutputFormat.Hex => ".txt",
                        _ => ".txt"
                    };

                    saveFileDialog.Filter = GetFileFilter(outputFormat);
                    saveFileDialog.Title = "保存转换后的密钥";
                    saveFileDialog.FileName = $"rsa_{keyType}_key{extension}";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        File.WriteAllText(saveFileDialog.FileName, textOutputKey.Text, Encoding.UTF8);
                        SetStatus("密钥文件保存成功");
                        MessageBox.Show("密钥文件保存成功！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"保存文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("保存文件失败");
            }
        }

        #endregion

        #region 缺失的事件处理方法

        private void btnImportFromFile_Click(object sender, EventArgs e)
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
                        textInputKey.Text = content;
                        SetStatus("密钥文件导入完成");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入密钥文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导入密钥文件失败");
            }
        }

        private void textInputKey_TextChanged(object sender, EventArgs e)
        {
            // 清空验证结果
            labelValidationResult.Text = "验证结果: 未验证";
            labelValidationResult.ForeColor = Color.Gray;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            textInputKey.Clear();
            textOutputKey.Clear();
            labelValidationResult.Text = "验证结果: 未验证";
            labelValidationResult.ForeColor = Color.Gray;
            SetStatus("已清空内容");
        }

        private void btnCopyToClipboard_Click(object sender, EventArgs e)
        {
            try
            {
                if (!string.IsNullOrEmpty(textOutputKey.Text))
                {
                    Clipboard.SetText(textOutputKey.Text);
                    SetStatus("密钥已复制到剪贴板");
                    MessageBox.Show("密钥已复制到剪贴板！", "成功", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("没有可复制的内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"复制失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("复制失败");
            }
        }

        #endregion

        #region 辅助方法

        private static string GetFileFilter(UIOutputFormat format)
        {
            return format switch
            {
                UIOutputFormat.PEM => "PEM文件 (*.pem)|*.pem|所有文件 (*.*)|*.*",
                UIOutputFormat.Base64 => "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
                UIOutputFormat.Hex => "文本文件 (*.txt)|*.txt|所有文件 (*.*)|*.*",
                _ => "所有文件 (*.*)|*.*"
            };
        }

        #endregion
    }
}
