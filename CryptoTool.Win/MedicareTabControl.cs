using CryptoTool.Algorithm.Algorithms.SM2;
using CryptoTool.Algorithm.Algorithms.SM4;
using CryptoTool.Algorithm.Enums;
using CryptoTool.Algorithm.Utils;
using Newtonsoft.Json;
using System.Text;
using CryptoTool.Win.Helpers;

namespace CryptoTool.Win
{
    public partial class MedicareTabControl : UserControl
    {
        public event Action<string>? StatusChanged;
        public event Action<string>? SM4KeyGenerated;

        public MedicareTabControl()
        {
            InitializeComponent();
            InitializeDefaults();
        }

        private void InitializeDefaults()
        {
            // 初始化医保默认值
            textMedicareAppId.Text = "43AF047BBA47FC8A1AE8EFB2XXXXXXXX";
            textMedicareAppSecret.Text = "4117E877F5FA0A0188891283E4B617D5";
            textMedicareEncType.Text = "SM4";
            textMedicareSignType.Text = "SM2";
            textMedicareVersion.Text = "2.0.1";

            // 设置默认的业务数据示例
            var defaultData = new
            {
                appId = "43AF047BBA47FC8A1AE8EFB2XXXXXXXX",
                appUserId = "o8z4C5avQXqC0aWFPf1Mzu6D7xxxx",
                idNo = "350582xxxxxxxx3519",
                idType = "01",
                phoneNumber = "137xxxxx033",
                userName = "测试"
            };
            textMedicareData.Text = JsonConvert.SerializeObject(defaultData, Formatting.Indented);

            // 设置当前时间戳
            textMedicareTimestamp.Text = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        }

        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #region 医保功能

        private void btnGenerateMedicareKey_Click(object sender, EventArgs e)
        {
            try
            {
                SetStatus("正在生成医保SM2密钥对...");
                var sm2Crypto = new Sm2Crypto();
                var keyPair = sm2Crypto.GenerateKeyPair();
                var publicKey = keyPair.PublicKey;
                var privateKey = keyPair.PrivateKey;

                textMedicarePublicKey.Text = StringUtil.BytesToBase64(publicKey);
                textMedicarePrivateKey.Text = StringUtil.BytesToBase64(privateKey);

                SetStatus("医保SM2密钥对生成完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成医保SM2密钥对失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成医保SM2密钥对失败");
            }
        }

        private void btnImportMedicareKey_Click(object sender, EventArgs e)
        {
            try
            {
                using (OpenFileDialog openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Filter = "密钥文件 (*.txt;*.key)|*.txt;*.key|所有文件 (*.*)|*.*";
                    openFileDialog.Title = "导入医保密钥文件";

                    if (openFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string content = File.ReadAllText(openFileDialog.FileName, Encoding.UTF8);
                        var (publicKey, privateKey) = Sm2UiHelper.ParseKeyFileContent(content);

                        if (!string.IsNullOrWhiteSpace(publicKey) && !string.IsNullOrWhiteSpace(privateKey))
                        {
                            textMedicarePublicKey.Text = publicKey;
                            textMedicarePrivateKey.Text = privateKey;
                            SetStatus("医保密钥导入成功");
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
                MessageBox.Show($"导入医保密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导入医保密钥失败");
            }
        }

        private void btnExportMedicareKey_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrEmpty(textMedicarePublicKey.Text) || string.IsNullOrEmpty(textMedicarePrivateKey.Text))
                {
                    MessageBox.Show("请先生成或输入医保密钥对！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "密钥文件 (*.txt)|*.txt|所有文件 (*.*)|*.*";
                    saveFileDialog.Title = "导出医保密钥文件";
                    saveFileDialog.FileName = "medicare_keys.txt";

                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    {
                        string content = Sm2UiHelper.BuildKeyFileContent(textMedicarePublicKey.Text, textMedicarePrivateKey.Text);
                        File.WriteAllText(saveFileDialog.FileName, content, Encoding.UTF8);
                        SetStatus("医保密钥导出成功");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导出医保密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("导出医保密钥失败");
            }
        }

        private void btnMedicareSign_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateMedicareInputs(false))
                {
                    SetStatus("正在进行医保签名...");

                    // 构造请求参数
                    var parameters = BuildMedicareParameters();

                    // 解析私钥
                    var privateKey = ParseMedicareKeyBytes(textMedicarePrivateKey.Text);
                    string appSecret = textMedicareAppSecret.Text.Trim();

                    // 构造签名字符串
                    string signatureString = MedicareUtil.BuildSignatureBaseString(parameters, appSecret);
                    textMedicareSignatureString.Text = signatureString;

                    // 生成签名
                    string signData = MedicareUtil.SignParameters(parameters, privateKey, appSecret);
                    textMedicareSignData.Text = signData;

                    SetStatus("医保签名完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"医保签名失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("医保签名失败");
            }
        }

        private void btnMedicareVerify_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateMedicareInputs(true))
                {
                    SetStatus("正在进行医保验签...");

                    // 构造参数（不包含signData进行验签）
                    var parameters = BuildMedicareParameters();

                    // 解析公钥
                    var publicKey = ParseMedicareKeyBytes(textMedicarePublicKey.Text);
                    string appSecret = textMedicareAppSecret.Text.Trim();
                    string signData = textMedicareSignData.Text.Trim();

                    // 验签
                    bool verifyResult = MedicareUtil.VerifyParametersSignature(parameters, signData, publicKey, appSecret);

                    MessageBox.Show($"验签结果：{(verifyResult ? "验证成功" : "验证失败")}",
                        "验签结果", MessageBoxButtons.OK,
                        verifyResult ? MessageBoxIcon.Information : MessageBoxIcon.Warning);

                    SetStatus($"医保验签完成 - {(verifyResult ? "验证成功" : "验证失败")}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"医保验签失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("医保验签失败");
            }
        }

        private void btnMedicareEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (ValidateMedicareEncryptedInputs(false))
                {
                    SetStatus("正在进行医保数据加密...");

                    string appId = textMedicareAppId.Text.Trim();
                    string appSecret = textMedicareAppSecret.Text.Trim();

                    // 解析JSON数据
                    object? dataObject;
                    try
                    {
                        dataObject = JsonConvert.DeserializeObject(textMedicareData.Text);
                    }
                    catch (JsonException)
                    {
                        // 如果不是有效JSON，使用原始字符串
                        dataObject = textMedicareData.Text;
                    }

                    // 加密数据
                    string encData = MedicareUtil.EncryptData(dataObject ?? textMedicareData.Text, appId, appSecret);
                    textMedicareEncData.Text = encData;

                    SetStatus("医保数据加密完成");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"医保数据加密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("医保数据加密失败");
            }
        }

        private void btnMedicareDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textMedicareEncData.Text))
                {
                    MessageBox.Show("请输入要解密的encData！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(textMedicareAppId.Text) || string.IsNullOrWhiteSpace(textMedicareAppSecret.Text))
                {
                    MessageBox.Show("请输入AppId和AppSecret！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在进行医保数据解密...");

                string appId = textMedicareAppId.Text.Trim();
                string appSecret = textMedicareAppSecret.Text.Trim();
                string encData = textMedicareEncData.Text.Trim();

                // 解密数据
                string decryptedData = MedicareUtil.DecryptEncData(encData, appId, appSecret);

                // 尝试格式化JSON显示
                try
                {
                    var jsonObject = JsonConvert.DeserializeObject(decryptedData);
                    textMedicareDecData.Text = JsonConvert.SerializeObject(jsonObject, Formatting.Indented);
                }
                catch
                {
                    // 如果不是有效JSON，直接显示原始数据
                    textMedicareDecData.Text = decryptedData;
                }

                SetStatus("医保数据解密完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"医保数据解密失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("医保数据解密失败");
            }
        }

        /// <summary>
        /// 医保加密功能输入校验
        /// </summary>
        /// <param name="includeSignData"></param>
        /// <returns></returns>
        private bool ValidateMedicareEncryptedInputs(bool includeSignData)
        {
            if (string.IsNullOrWhiteSpace(textMedicareAppId.Text))
            {
                MessageBox.Show("请输入AppId！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textMedicareAppSecret.Text))
            {
                MessageBox.Show("请输入AppSecret！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textMedicareData.Text))
            {
                MessageBox.Show("请输入业务数据！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        /// <summary>
        /// 医保签名验签功能输入校验
        /// </summary>
        /// <param name="includeSignData"></param>
        /// <returns></returns>
        private bool ValidateMedicareInputs(bool includeSignData)
        {
            if (string.IsNullOrWhiteSpace(textMedicareAppId.Text))
            {
                MessageBox.Show("请输入AppId！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textMedicareAppSecret.Text))
            {
                MessageBox.Show("请输入AppSecret！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textMedicareTimestamp.Text))
            {
                MessageBox.Show("请输入时间戳！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textMedicarePublicKey.Text))
            {
                MessageBox.Show("请先生成或输入公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (string.IsNullOrWhiteSpace(textMedicarePrivateKey.Text))
            {
                MessageBox.Show("请先生成或输入私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            if (includeSignData && string.IsNullOrWhiteSpace(textMedicareSignData.Text))
            {
                MessageBox.Show("请先进行签名操作获取SignData！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return false;
            }

            return true;
        }

        private Dictionary<string, object> BuildMedicareParameters()
        {
            var parameters = new Dictionary<string, object>();

            parameters["appId"] = textMedicareAppId.Text.Trim();
            parameters["encType"] = textMedicareEncType.Text.Trim();
            parameters["signType"] = textMedicareSignType.Text.Trim();
            parameters["timestamp"] = textMedicareTimestamp.Text.Trim();
            parameters["version"] = textMedicareVersion.Text.Trim();

            // 如果有数据内容，添加data字段
            if (!string.IsNullOrWhiteSpace(textMedicareData.Text))
            {
                try
                {
                    // 尝试解析为JSON对象
                    var dataObject = JsonConvert.DeserializeObject(textMedicareData.Text);
                    parameters["data"] = dataObject ?? textMedicareData.Text.Trim();
                }
                catch (JsonException)
                {
                    // 如果不是有效JSON，使用原始字符串
                    parameters["data"] = textMedicareData.Text.Trim();
                }
            }

            return parameters;
        }

        #endregion

        #region 医保SM4密钥生成功能

        private void btnGenerateMedicareSM4Key_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textMedicareAppId.Text))
                {
                    MessageBox.Show("请输入医保AppId！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(textMedicareAppSecret.Text))
                {
                    MessageBox.Show("请输入医保AppSecret！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string appId = textMedicareAppId.Text.Trim();
                string appSecret = textMedicareAppSecret.Text.Trim();

                if (appId.Length < 16)
                {
                    MessageBox.Show("AppId长度不足16字节，无法派生SM4密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                SetStatus("正在根据医保AppId和AppSecret生成SM4密钥...");

                // 使用MedicareUtil中的逻辑生成SM4密钥
                string derivedKey = GetMedicareSM4Key(appId, appSecret);
                textMedicareSM4Key.Text = derivedKey;

                // 通知外部更新SM4密钥
                SM4KeyGenerated?.Invoke(derivedKey);

                SetStatus($"医保SM4密钥生成完成 - 基于AppId和AppSecret派生");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"生成医保SM4密钥失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("生成医保SM4密钥失败");
            }
        }

        /// <summary>
        /// 根据医保规范，从AppId和AppSecret派生SM4密钥
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        private string GetMedicareSM4Key(string appId, string appSecret)
        {
            // 实现医保规范的SM4密钥派生算法
            // 以appId(渠道id)作为Key，对appSecret加密，得到新秘钥串，取前16字节作为SM4密钥

            if (appId.Length < 16)
            {
                throw new ArgumentException("appId长度不足16字节，无法派生SM4密钥", nameof(appId));
            }

            // 取appId的前16字节作为SM4密钥来加密appSecret
            string keyString = appId.Substring(0, 16);

            // 使用SM4-ECB模式，appId前16字符作为密钥，对appSecret进行加密
            var sm4Crypto = new Sm4Crypto(SymmetricCipherMode.ECB, SymmetricPaddingMode.PKCS7);

            //结果转换为字节数组，再转换为Hex字符串，取前16个字符（8字节）作为最终的SM4密钥
            byte[] encryptedBytes = sm4Crypto.Encrypt(Encoding.UTF8.GetBytes(appSecret), Encoding.UTF8.GetBytes(keyString));
            string hexResult = StringUtil.BytesToHex(encryptedBytes, true);

            // 取前16个字符作为最终的SM4密钥（Hex格式，实际对应8字节）
            // 但SM4需要16字节密钥，所以取前32个字符（对应16字节）
            string finalKey = hexResult.Substring(0, Math.Min(32, hexResult.Length));

            return finalKey;
        }

        private static byte[] ParseMedicareKeyBytes(string keyText)
        {
            if (string.IsNullOrWhiteSpace(keyText))
                throw new ArgumentException("密钥不能为空", nameof(keyText));

            var normalized = keyText.Trim();
            if (LooksLikeHex(normalized))
                return StringUtil.HexToBytes(normalized);

            return Convert.FromBase64String(normalized);
        }

        private static bool LooksLikeHex(string value)
        {
            return value.Length % 2 == 0 && value.All(Uri.IsHexDigit);
        }

        #endregion

        /// <summary>
        /// 当时间戳文本框失去焦点且未填充内容时，自动更新为当前时间戳
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextMedicareTimestamp_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textMedicareTimestamp.Text))
            {
                long timeStamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
                textMedicareTimestamp.Text = timeStamp.ToString();
            }
        }
    }
}
