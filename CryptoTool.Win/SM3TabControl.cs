using CryptoTool.Algorithm.Algorithms.SM3;
using CryptoTool.Algorithm.Utils;
using System.Text;

namespace CryptoTool.Win
{
    public partial class SM3TabControl : UserControl
    {
        public event Action<string>? StatusChanged;

        public SM3TabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            // 设置默认选项
            comboSM3DataFormat.SelectedIndex = 0; // Text
            comboSM3OutputFormat.SelectedIndex = 0; // Hex
            comboSM3FileHashFormat.SelectedIndex = 0; // Hex
            comboSM3VerifyDataFormat.SelectedIndex = 0; // Text
            comboSM3VerifyHashFormat.SelectedIndex = 0; // Hex
            comboSM3HMACDataFormat.SelectedIndex = 0; // Text
            comboSM3HMACOutputFormat.SelectedIndex = 0; // Hex

            // 设置示例数据
            textSM3Input.Text = "Hello SM3!";
            textSM3HMACData.Text = "Hello HMAC-SM3!";
            textSM3HMACKey.Text = "mySecretKey";
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region SM3哈希计算

        private void btnSM3Hash_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM3Input.Text))
                {
                    MessageBox.Show("请输入要计算哈希的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在计算SM3哈希...");

                string inputData = textSM3Input.Text;
                string dataFormat = comboSM3DataFormat.SelectedItem?.ToString() ?? "";
                string outputFormat = comboSM3OutputFormat.SelectedItem?.ToString() ?? "";

                byte[] dataBytes = ConvertInputData(inputData, dataFormat);

                var sm3Hash = new Sm3Hash();
                byte[] hashBytes = sm3Hash.ComputeHash(dataBytes);

                string result = outputFormat switch
                {
                    "Hex" => StringUtil.BytesToHex(hashBytes, false),
                    "Base64" => Convert.ToBase64String(hashBytes),
                    _ => StringUtil.BytesToHex(hashBytes, false)
                };

                textSM3Output.Text = result;
                SetStatus($"SM3哈希计算完毕 - 输入格式：{dataFormat}，输出格式：{outputFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM3哈希计算失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("SM3哈希计算失败");
            }
        }

        private void btnSM3Clear_Click(object sender, EventArgs e)
        {
            textSM3Input.Clear();
            textSM3Output.Clear();
            SetStatus("已清空输入和输出");
        }

        #endregion

        #region SM3文件哈希

        private void btnSM3SelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "选择要计算哈希的文件";
                    openFileDialog.Filter = "所有文件 (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textSM3FilePath.Text = openFileDialog.FileName;
                        SetStatus($"已选择文件: {Path.GetFileName(openFileDialog.FileName)}");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"选择文件失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("选择文件失败");
            }
        }

        private void btnSM3ComputeFileHash_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM3FilePath.Text) || !File.Exists(textSM3FilePath.Text))
                {
                    MessageBox.Show("请先选择一个有效的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在计算文件SM3哈希...");

                string outputFormat = comboSM3FileHashFormat.SelectedItem?.ToString() ?? "";

                var sm3Hash = new Sm3Hash();
                byte[] hashBytes = sm3Hash.ComputeFileHash(textSM3FilePath.Text);

                string result = outputFormat switch
                {
                    "Hex" => StringUtil.BytesToHex(hashBytes, false),
                    "Base64" => Convert.ToBase64String(hashBytes),
                    _ => StringUtil.BytesToHex(hashBytes, false)
                };

                textSM3FileHash.Text = result;
                SetStatus($"文件SM3哈希计算完成 - 输出格式：{outputFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算文件哈希失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("计算文件哈希失败");
            }
        }

        #endregion

        #region SM3哈希验证

        private void btnSM3Verify_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM3VerifyData.Text))
                {
                    MessageBox.Show("请输入要验证的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM3VerifyHash.Text))
                {
                    MessageBox.Show("请输入期望的哈希值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在验证SM3哈希...");

                string data = textSM3VerifyData.Text;
                string expectedHash = textSM3VerifyHash.Text;
                string dataFormat = comboSM3VerifyDataFormat.SelectedItem?.ToString() ?? "";
                string hashFormat = comboSM3VerifyHashFormat.SelectedItem?.ToString() ?? "";

                byte[] dataBytes = ConvertInputData(data, dataFormat);

                var sm3Hash = new Sm3Hash();
                bool isValid = sm3Hash.VerifyHash(dataBytes, expectedHash);

                labelSM3VerifyResult.Text = isValid ? "验证通过" : "验证失败";
                labelSM3VerifyResult.ForeColor = isValid ? Color.Green : Color.Red;

                SetStatus($"SM3哈希验证完成 - 结果：{(isValid ? "通过" : "失败")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"SM3哈希验证失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelSM3VerifyResult.Text = "验证异常";
                labelSM3VerifyResult.ForeColor = Color.Red;
                SetStatus("SM3哈希验证失败");
            }
        }

        #endregion

        #region HMAC-SM3

        private void btnSM3ComputeHMAC_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textSM3HMACData.Text))
                {
                    MessageBox.Show("请输入要计算HMAC的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textSM3HMACKey.Text))
                {
                    MessageBox.Show("请输入HMAC密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在计算HMAC-SM3...");

                string data = textSM3HMACData.Text;
                string key = textSM3HMACKey.Text;
                string dataFormat = comboSM3HMACDataFormat.SelectedItem?.ToString() ?? "";
                string outputFormat = comboSM3HMACOutputFormat.SelectedItem?.ToString() ?? "";

                byte[] dataBytes = ConvertInputData(data, dataFormat);
                byte[] keyBytes = Encoding.UTF8.GetBytes(key);

                var sm3Hash = new Sm3Hash();
                byte[] hmacBytes = sm3Hash.ComputeHmac(dataBytes, keyBytes);

                string result = outputFormat switch
                {
                    "Hex" => StringUtil.BytesToHex(hmacBytes, false),
                    "Base64" => Convert.ToBase64String(hmacBytes),
                    _ => StringUtil.BytesToHex(hmacBytes, false)
                };

                textSM3HMACOutput.Text = result;
                SetStatus($"HMAC-SM3计算完成 - 输出格式：{outputFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"HMAC-SM3计算失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("HMAC-SM3计算失败");
            }
        }

        #endregion

        #region 辅助方法

        private byte[] ConvertInputData(string data, string format)
        {
            return format switch
            {
                "Text" => Encoding.UTF8.GetBytes(data),
                "Base64" => Convert.FromBase64String(data),
                "Hex" => StringUtil.HexToBytes(data),
                _ => Encoding.UTF8.GetBytes(data)
            };
        }

        #endregion
    }
}
