using CryptoTool.Algorithm.Algorithms.MD5;
using CryptoTool.Algorithm.Utils;
using System.Text;

namespace CryptoTool.Win
{
    public partial class MD5TabControl : UserControl
    {
        public event Action<string>? StatusChanged;

        public MD5TabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            // 设置默认选项
            comboMD5DataFormat.SelectedIndex = 0; // Text
            comboMD5OutputFormat.SelectedIndex = 0; // Hex
            comboMD5FileHashFormat.SelectedIndex = 0; // Hex
            comboMD5VerifyDataFormat.SelectedIndex = 0; // Text
            comboMD5VerifyHashFormat.SelectedIndex = 0; // Hex

            // 设置示例数据
            textMD5Input.Text = "Hello MD5!";
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        # region MD5哈希计算

        private void btnMD5Hash_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textMD5Input.Text))
                {
                    MessageBox.Show("请输入要计算哈希的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在计算MD5哈希...");

                string inputData = textMD5Input.Text;
                string dataFormat = comboMD5DataFormat.SelectedItem?.ToString() ?? "Text";
                string outputFormat = comboMD5OutputFormat.SelectedItem?.ToString() ?? "Hex";

                byte[] dataBytes = ConvertInputData(inputData, dataFormat);

                var md5Hash = new Md5Hash();
                byte[] hashBytes = md5Hash.ComputeHash(dataBytes);
                string result = ConvertHashToFormat(hashBytes, outputFormat);

                textMD5Output.Text = result;
                SetStatus($"MD5哈希计算完毕 - 输入格式：{dataFormat}，输出格式：{outputFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MD5哈希计算失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("MD5哈希计算失败");
            }
        }

        private void btnMD5Clear_Click(object sender, EventArgs e)
        {
            textMD5Input.Clear();
            textMD5Output.Clear();
            SetStatus("已清空输入和输出");
        }

        #endregion

        #region MD5文件哈希

        private void btnMD5SelectFile_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "选择要计算哈希的文件";
                    openFileDialog.Filter = "所有文件 (*.*)|*.*";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        textMD5FilePath.Text = openFileDialog.FileName;
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

        private void btnMD5ComputeFileHash_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textMD5FilePath.Text) || !File.Exists(textMD5FilePath.Text))
                {
                    MessageBox.Show("请先选择一个有效的文件！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在计算文件MD5哈希...");

                string outputFormat = comboMD5FileHashFormat.SelectedItem?.ToString() ?? "Hex";

                var md5Hash = new Md5Hash();
                byte[] hashBytes = md5Hash.ComputeFileHash(textMD5FilePath.Text);
                string result = ConvertHashToFormat(hashBytes, outputFormat);

                textMD5FileHash.Text = result;
                SetStatus($"文件MD5哈希计算完成 - 输出格式：{outputFormat}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"计算文件哈希失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("计算文件哈希失败");
            }
        }

        #endregion

        #region MD5哈希验证

        private void btnMD5Verify_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textMD5VerifyData.Text))
                {
                    MessageBox.Show("请输入要验证的数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrEmpty(textMD5VerifyHash.Text))
                {
                    MessageBox.Show("请输入期望的哈希值！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在验证MD5哈希...");

                string data = textMD5VerifyData.Text;
                string expectedHash = textMD5VerifyHash.Text;
                string dataFormat = comboMD5VerifyDataFormat.SelectedItem?.ToString() ?? "Text";
                string hashFormat = comboMD5VerifyHashFormat.SelectedItem?.ToString() ?? "Hex";

                byte[] dataBytes = ConvertInputData(data, dataFormat);
                byte[] expectedHashBytes = ConvertHashFromFormat(expectedHash, hashFormat);

                var md5Hash = new Md5Hash();
                bool isValid = md5Hash.VerifyHash(dataBytes, expectedHashBytes);

                labelMD5VerifyResult.Text = isValid ? "验证通过" : "验证失败";
                labelMD5VerifyResult.ForeColor = isValid ? Color.Green : Color.Red;

                SetStatus($"MD5哈希验证完成 - 结果：{(isValid ? "通过" : "失败")}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"MD5哈希验证失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                labelMD5VerifyResult.Text = "验证异常";
                labelMD5VerifyResult.ForeColor = Color.Red;
                SetStatus("MD5哈希验证失败");
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

        private string ConvertHashToFormat(byte[] hashBytes, string format)
        {
            return format switch
            {
                "Hex" => StringUtil.BytesToHex(hashBytes),
                "Base64" => Convert.ToBase64String(hashBytes),
                _ => StringUtil.BytesToHex(hashBytes)
            };
        }

        private byte[] ConvertHashFromFormat(string hashString, string format)
        {
            return format switch
            {
                "Hex" => StringUtil.HexToBytes(hashString),
                "Base64" => Convert.FromBase64String(hashString),
                _ => StringUtil.HexToBytes(hashString)
            };
        }

        #endregion
    }
}
