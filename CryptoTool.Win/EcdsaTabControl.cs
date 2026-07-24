using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace CryptoTool.Win
{
    public partial class EcdsaTabControl : UserControl
    {
        public event Action<string>? StatusChanged;

        private string _privateKeyPem = string.Empty;
        private string _publicKeyPem = string.Empty;
        private string _lastSignHashAlgorithm = string.Empty;

        private const string PrivateKeyStandardSec1 = "SEC1/RFC 5915（长编码 / specifiedCurve）";
        private const string PrivateKeyStandardPkcs8 = "PKCS#8 / RFC 5958（短编码 / namedCurve）";

        private const string PublicKeyStandardNamedCurve = "RFC 5480/namedCurve";
        private const string PublicKeyStandardSpecifiedCurve = "RFC 5480/specifiedCurve";

        private Dictionary<string, (string Icon, List<KeyValuePair<string, string>> Curves)> _allCurveData = [];


        private System.Windows.Forms.Timer _resizeTimer = null!;
        private int _lastWidth = -1;
        private int _lastHeight = -1;

        // 视图切换相关字段
        private int _currentViewIndex = 0;

        // ECDH 视图控件已在 Designer.cs 中声明
        private byte[]? _ecdhLastIV = null;

        public EcdsaTabControl()
        {
            InitializeComponent();
            InitializeEncryptControlDefaults();
            InitializeEncryptLayout();
            InitializeViewSwitcher();
            InitializeSignLayoutLabels();
            InitializeEcdhLayout();
            InitializeDefaults();
            InitializeResizeTimer();
            EnableDoubleBuffering();

            this.Load += (_, __) => ApplySplitterRatios();
            this.SizeChanged += OnResizeTimerRestart;
        }

        #region ECDH 业务逻辑
        private void ComboEcdhCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender == null || comboEcdhCategory.SelectedItem == null)
                return;

            dynamic selectedItem = comboEcdhCategory.SelectedItem;
            string categoryKey = selectedItem.Value;

            if (!_allCurveData.TryGetValue(categoryKey, out var categoryData))
                return;

            comboEcdhCurve.Items.Clear();
            foreach (var c in categoryData.Curves)
                comboEcdhCurve.Items.Add(c);

            if (comboEcdhCurve.Items.Count > 0)
                comboEcdhCurve.SelectedIndex = 0;
        }

        private string GetEcdhSelectedCurve()
        {
            if (comboEcdhCurve.SelectedItem is KeyValuePair<string, string> sel && !string.IsNullOrEmpty(sel.Key))
                return sel.Key;
            return "prime256v1";
        }

        private void BtnGenerateEcdhKeys_Click(object? sender, EventArgs e)
        {
            try
            {
                string curve = GetEcdhSelectedCurve();
                SetStatus($"正在生成 {EcdsaCurveNames.GetDisplayName(curve)} ECDH 密钥对...");

                var alice = EcdhAlgorithm.GenerateKeyPair(curve);
                var bob = EcdhAlgorithm.GenerateKeyPair(curve);

                var alicePriv = (ECPrivateKeyParameters)alice.Private;
                var alicePub = (ECPublicKeyParameters)alice.Public;
                var bobPriv = (ECPrivateKeyParameters)bob.Private;
                var bobPub = (ECPublicKeyParameters)bob.Public;

                textEcdhAlicePrivate.Text = ExportPrivateKeyByStandard(alicePriv, comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardPkcs8);
                textEcdhAlicePublic.Text = ExportPublicKeyByStandard(alicePub, comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve);
                textEcdhBobPrivate.Text = ExportPrivateKeyByStandard(bobPriv, comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardPkcs8);
                textEcdhBobPublic.Text = ExportPublicKeyByStandard(bobPub, comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve);

                byte[] shared = EcdhAlgorithm.DeriveSharedSecret(alicePriv, bobPub);
                textEcdhSharedKey.Text = Convert.ToBase64String(shared);

                // 生成新密钥对后，旧密文/IV/输入与新的密钥不再匹配，清空避免误用
                textEcdhInput.Clear();
                textEcdhOutput.Clear();
                textEcdhIV.Clear();
                _ecdhLastIV = null;

                AppendValidationResult($"✅ ECDH 密钥对已生成\n曲线: {EcdsaCurveNames.GetDisplayName(curve)}\n共享密钥长度: {shared.Length} 字节", Color.Green);
                SetStatus("ECDH 密钥对生成完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 生成 ECDH 密钥对失败: {ex.Message}", Color.Red);
                SetStatus("生成 ECDH 密钥对失败");
            }
        }

        private void BtnEcdhEncrypt_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEcdhInput.Text))
                { MessageBox.Show("请输入要加密的信息", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                var mode = GetEcdhMode();
                string curve = GetEcdhSelectedCurve();
                byte[] plain = GetEcdhEncoding().GetBytes(textEcdhInput.Text);
                textEcdhIV.Clear();
                byte[]? userIv = null;
                if (!string.IsNullOrWhiteSpace(textEcdhIV.Text))
                {
                    try { userIv = EcdhAlgorithm.ParseIv(textEcdhIV.Text.Trim(), mode); }
                    catch (FormatException ex)
                    {
                        AppendValidationResult($"❌ IV 格式无效: {ex.Message}", Color.Red);
                        SetStatus("IV 格式错误");
                        return;
                    }
                }
                byte[] shared, iv;
                string output;

                if (mode != EcdhMode.GwifiOrg)
                {
                    // ECIES：发送方生成临时密钥对，只需接收方公钥
                    if (string.IsNullOrWhiteSpace(textEcdhBobPublic.Text))
                    { MessageBox.Show("请输入 Bob 公钥（接收方公钥）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                    var bobPub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhBobPublic.Text.Trim());
                    byte[] combined = EcdhAlgorithm.EciesEncrypt(plain, bobPub, curve, mode, out shared, out iv, userIv);
                    output = Convert.ToBase64String(combined);
                    AppendValidationResult($"✅ ECIES 加密成功\n模式: {comboEcdhMode.SelectedItem}\n曲线: {EcdsaCurveNames.GetDisplayName(curve)}\n临时公钥: {EcdhAlgorithm.GetEphemeralPubKeyLength(curve)} 字节\n明文: {plain.Length} 字节 → 密文: {combined.Length} 字节", Color.Green);
                }
                else
                {
                    // 8gwifi.org 兼容：静态 ECDH，需要 Alice 私钥 + Bob 公钥
                    if (string.IsNullOrWhiteSpace(textEcdhAlicePrivate.Text) || string.IsNullOrWhiteSpace(textEcdhBobPublic.Text))
                    { MessageBox.Show("8gwifi.org 模式加密需要 Alice 私钥和 Bob 公钥", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                    var alicePriv = EcdsaKeyHelper.ImportPrivateKeyPem(textEcdhAlicePrivate.Text.Trim());
                    var bobPub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhBobPublic.Text.Trim());
                    output = EcdhAlgorithm.StaticEcdhEncrypt(plain, alicePriv, bobPub, mode, out shared, out iv, userIv);
                    AppendValidationResult($"✅ 8gwifi.org 加密成功\n模式: {comboEcdhMode.SelectedItem}\n曲线: {EcdsaCurveNames.GetDisplayName(curve)}\n明文: {plain.Length} 字节\n密文（不含 IV）: {Convert.FromBase64String(output).Length} 字节", Color.Green);
                }

                textEcdhOutput.Text = output;
                textEcdhSharedKey.Text = Convert.ToBase64String(shared);
                textEcdhIV.Text = EcdhAlgorithm.FormatIv(iv, mode);
                _ecdhLastIV = iv;
                SetStatus($"{comboEcdhMode.SelectedItem} 加密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ {comboEcdhMode.SelectedItem} 加密失败: {ex.Message}", Color.Red);
                SetStatus($"{comboEcdhMode.SelectedItem} 加密失败");
            }
        }

        private void BtnEcdhDecrypt_Click(object? sender, EventArgs e)
        {
            try
            {
                // 解密固定从"密文（可编辑）"框读取，不再回退到"明文"框
                // 避免用户误将明文当密文解密
                string cipherSource = textEcdhOutput.Text.Trim();
                if (string.IsNullOrWhiteSpace(cipherSource))
                {
                    AppendValidationResult("⚠️ 请先加密生成密文，或将 Base64 密文粘贴到 密文（可编辑） 框", Color.Orange);
                    SetStatus("无密文输入");
                    return;
                }

                var mode = GetEcdhMode();
                string curve = GetEcdhSelectedCurve();

                byte[] plain;
                byte[] shared;
                byte[] iv;

                if (mode != EcdhMode.GwifiOrg)
                {
                    // ECIES 解密：必须有接收方私钥（Bob 私钥）
                    if (string.IsNullOrWhiteSpace(textEcdhBobPrivate.Text))
                    { MessageBox.Show("请输入 Bob 私钥（接收方私钥）来解密", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                    var bobPriv = EcdsaKeyHelper.ImportPrivateKeyPem(textEcdhBobPrivate.Text.Trim());
                    byte[] combined;
                    try
                    {
                        combined = Convert.FromBase64String(cipherSource);
                    }
                    catch (FormatException)
                    {
                        AppendValidationResult("❌ 密文格式无效: 请输入 Base64 编码的密文，不是原始数字或字符串", Color.Red);
                        SetStatus("Base64 格式错误");
                        return;
                    }
                    try
                    {
                        plain = EcdhAlgorithm.EciesDecrypt(combined, bobPriv, curve, mode, out shared, out iv);
                    }
                    catch (InvalidCipherTextException)
                    {
                        AppendValidationResult($"❌ ECIES 解密失败: MAC 校验不通过，密钥或密文不匹配", Color.Red);
                        SetStatus("ECIES 解密失败");
                        return;
                    }
                    catch (ArgumentException argEx) when (argEx.Message.Contains("Specified argument"))
                    {
                        AppendValidationResult($"❌ 密文结构无效: {argEx.Message}。请检查密文是否完整、曲线是否匹配", Color.Red);
                        SetStatus("密文结构错误");
                        return;
                    }
                    textEcdhSharedKey.Text = Convert.ToBase64String(shared);
                    textEcdhIV.Text = EcdhAlgorithm.FormatIv(iv, mode);
                    _ecdhLastIV = iv;
                    AppendValidationResult($"✅ ECIES 解密成功\n模式: {comboEcdhMode.SelectedItem}\n明文长度: {plain.Length} 字节", Color.Green);
                }
                else
                {
                    // 8gwifi.org 兼容：静态 ECDH 解密，需要 Bob 私钥 + Alice 公钥
                    if (string.IsNullOrWhiteSpace(textEcdhBobPrivate.Text))
                    { MessageBox.Show("8gwifi.org 模式解密需要 Bob 私钥", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
                    if (string.IsNullOrWhiteSpace(textEcdhAlicePublic.Text))
                    { MessageBox.Show("8gwifi.org 模式解密需要 Alice 公钥", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                    var bobPriv = EcdsaKeyHelper.ImportPrivateKeyPem(textEcdhBobPrivate.Text.Trim());
                    var alicePub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhAlicePublic.Text.Trim());
                    try
                    {
                        plain = EcdhAlgorithm.StaticEcdhDecrypt(cipherSource, bobPriv, alicePub, mode, out shared, out iv);
                    }
                    catch (InvalidCipherTextException)
                    {
                        AppendValidationResult($"❌ 8gwifi.org 解密失败: MAC 校验不通过，密钥或密文不匹配", Color.Red);
                        SetStatus("8gwifi.org 解密失败");
                        return;
                    }
                    textEcdhSharedKey.Text = Convert.ToBase64String(shared);
                    textEcdhIV.Text = EcdhAlgorithm.FormatIv(iv, mode);
                    _ecdhLastIV = iv;
                    AppendValidationResult($"✅ 8gwifi.org 解密成功\n模式: {comboEcdhMode.SelectedItem}\n明文长度: {plain.Length} 字节", Color.Green);
                }

                textEcdhInput.Text = GetEcdhEncoding().GetString(plain);
                SetStatus($"{comboEcdhMode.SelectedItem} 解密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ {comboEcdhMode.SelectedItem} 解密失败: {ex.Message}", Color.Red);
                SetStatus($"{comboEcdhMode.SelectedItem} 解密失败");
            }
        }

        private EcdhMode GetEcdhMode()
        {
            var selected = comboEcdhMode.SelectedItem?.ToString();
            return selected switch
            {
                "8gwifi.org" => EcdhMode.GwifiOrg,
                "ANSI X9.63" => EcdhMode.AnsiX963,
                "IEEE 1363a" => EcdhMode.Ieee1363a,
                "ISO/IEC 18033-2" => EcdhMode.Iso180332,
                "SECG SEC 1" => EcdhMode.SecgSec1,
                _ => EcdhMode.CryptoTool
            };
        }

        private Encoding GetEcdhEncoding()
        {
            var selected = comboEcdhEncoding.SelectedItem?.ToString();
            return selected switch
            {
                "GBK (GB2312)" => Encoding.GetEncoding("GBK"),
                "Unicode (UTF-16 LE)" => Encoding.Unicode,
                _ => Encoding.UTF8
            };
        }

        private static bool TryParseHex(string? input, out byte[] bytes)
        {
            bytes = [];
            if (string.IsNullOrWhiteSpace(input))
                return false;
            try
            {
                bytes = Convert.FromHexString(input.Trim().Replace("0x", "").Replace("0X", "").Replace(" ", ""));
                return bytes.Length > 0;
            }
            catch
            {
                return false;
            }
        }

        private void BtnEcdhCopyResult_Click(object? sender, EventArgs e)
        {
            TrySetClipboardText(textEcdhOutput.Text, "ECDH 结果已复制");
        }

        private void BtnEcdhPasteInput_Click(object? sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textEcdhOutput.Text = Clipboard.GetText().Trim();
                SetStatus("密文已粘贴到 ECDH 密文框");
            }
        }

        private void BtnEcdhClear_Click(object? sender, EventArgs e)
        {
            textEcdhInput.Clear();
            textEcdhOutput.Clear();
            textEcdhSharedKey.Clear();
            textEcdhIV.Clear();
            _ecdhLastIV = null;
            SetStatus("ECDH 输入/输出已清空");
        }

        private void BtnConvertEcdhPrivateKeyStandard_Click(object? sender, EventArgs e)
        {
            try
            {
                string standard = comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardPkcs8;
                int convertedCount = 0;
                if (ConvertEcdhPrivateKey(textEcdhAlicePrivate, standard)) convertedCount++;
                if (ConvertEcdhPrivateKey(textEcdhBobPrivate, standard)) convertedCount++;

                if (convertedCount == 0)
                {
                    MessageBox.Show("当前没有 Alice 或 Bob 的私钥内容可转换，请先生成或导入私钥。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                AppendValidationResult($"✅ {convertedCount} 组私钥已转换为 {standard}", Color.Gray);
                SetStatus($"Alice/Bob 私钥存储标准转换完成 - {standard}");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ ECDH 私钥标准转换失败: {ex.Message}", Color.Red);
                SetStatus("ECDH 私钥标准转换失败");
            }
        }

        private bool ConvertEcdhPrivateKey(TextBox textBox, string standard)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return false;

            string pem = ConvertDisplayToPem(textBox.Text.Trim(), true);
            var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
            string converted = ExportPrivateKeyByStandard(priv, standard);
            textBox.Text = FormatKeyForDisplay(converted, UIOutputFormat.PEM);
            return true;
        }

        private void BtnConvertEcdhPublicKeyStandard_Click(object? sender, EventArgs e)
        {
            try
            {
                string standard = comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve;
                int convertedCount = 0;
                if (ConvertEcdhPublicKey(textEcdhAlicePublic, standard)) convertedCount++;
                if (ConvertEcdhPublicKey(textEcdhBobPublic, standard)) convertedCount++;

                if (convertedCount == 0)
                {
                    MessageBox.Show("当前没有 Alice 或 Bob 的公钥内容可转换，请先生成或导入公钥。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                AppendValidationResult($"✅ {convertedCount} 组公钥已转换为 {standard}", Color.Gray);
                SetStatus($"Alice/Bob 公钥存储标准转换完成 - {standard}");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ ECDH 公钥标准转换失败: {ex.Message}", Color.Red);
                SetStatus("ECDH 公钥标准转换失败");
            }
        }

        private bool ConvertEcdhPublicKey(TextBox textBox, string standard)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return false;

            string pem = ConvertDisplayToPem(textBox.Text.Trim(), false);
            var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
            string converted = ExportPublicKeyByStandard(pub, standard);
            textBox.Text = FormatKeyForDisplay(converted, UIOutputFormat.PEM);
            return true;
        }

        private void ComboEcdhPrivateKeyStandard_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                string standard = comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardPkcs8;
                ConvertEcdhPrivateKey(textEcdhAlicePrivate, standard);
                ConvertEcdhPrivateKey(textEcdhBobPrivate, standard);
            }
            catch
            {
                // 下拉框选项变化时仅静默转换已存在的密钥，避免弹窗干扰操作
            }
        }

        private void ComboEcdhPublicKeyStandard_SelectedIndexChanged(object? sender, EventArgs e)
        {
            try
            {
                string standard = comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve;
                ConvertEcdhPublicKey(textEcdhAlicePublic, standard);
                ConvertEcdhPublicKey(textEcdhBobPublic, standard);
            }
            catch
            {
                // 下拉框选项变化时仅静默转换已存在的密钥，避免弹窗干扰操作
            }
        }

        #endregion


        #region 初始化与级联下拉框逻辑
        private void InitializeDefaults()
        {
            _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

            comboCategory.DisplayMember = "Text";
            comboCategory.ValueMember = "Value";
            foreach (var cat in _allCurveData)
            {
                comboCategory.Items.Add(new
                {
                    Text = $"{cat.Value.Icon} {cat.Key}",
                    Value = cat.Key
                });
            }

            if (comboCategory.Items.Count > 0)
                comboCategory.SelectedIndex = 0;

            comboHashAlgorithm.Items.Clear();
            comboHashAlgorithm.Items.AddRange(["SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512"]);
            comboHashAlgorithm.SelectedIndex = 0;

            comboOutputFormat.Items.Clear();
            comboOutputFormat.Items.AddRange(["PEM", "Base64", "Hex大写", "Hex小写"]);
            comboOutputFormat.SelectedIndex = 0;

            comboSignatureFormat.Items.Clear();
            comboSignatureFormat.Items.AddRange(["Base64", "Hex"]);
            comboSignatureFormat.SelectedIndex = 0;

            comboPrivateKeyStandard.Items.Clear();
            comboPrivateKeyStandard.Items.AddRange([PrivateKeyStandardPkcs8, PrivateKeyStandardSec1]);
            comboPrivateKeyStandard.SelectedIndex = 0;

            comboPublicKeyStandard.Items.Clear();
            comboPublicKeyStandard.Items.AddRange([PublicKeyStandardNamedCurve, PublicKeyStandardSpecifiedCurve]);
            comboPublicKeyStandard.SelectedIndex = 0;

            radioPrivateKey.Checked = true;

            InitializeEncryptDefaults();

            ResetValidationResult("未验证", Color.Gray);
            textKeyResult.Text = "从私钥提取/曲线检测：\n等待操作...";
            textKeyResult.ForeColor = Color.Gray;
        }

        private void ComboCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender == null || comboCategory.SelectedItem == null)
                return;

            dynamic selectedItem = comboCategory.SelectedItem;
            string categoryKey = selectedItem.Value;

            if (!_allCurveData.TryGetValue(categoryKey, out var categoryData))
                return;

            comboCurve.Items.Clear();
            foreach (var c in categoryData.Curves)
                comboCurve.Items.Add(c);

            if (comboCurve.Items.Count > 0)
                comboCurve.SelectedIndex = 0;
        }

        private string GetSelectedCurve()
        {
            if (comboCurve.SelectedItem is KeyValuePair<string, string> sel && !string.IsNullOrEmpty(sel.Key))
                return sel.Key;
            return "prime256v1";
        }

        /// <summary>
        /// 根据下拉框选项将 EC 私钥导出为 SEC1 或 PKCS#8
        /// </summary>
        private string ExportPrivateKeyByStandard(ECPrivateKeyParameters priv)
        {
            string standard = comboPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardSec1;
            return ExportPrivateKeyByStandard(priv, standard);
        }

        private string ExportPrivateKeyByStandard(ECPrivateKeyParameters priv, string standard)
        {
            if (standard == PrivateKeyStandardPkcs8)
                return EcdsaKeyHelper.ExportPrivateKeyPemPkcs8(priv);

            // SEC1/RFC 5915: 强制使用显式参数 (specifiedCurve)，避免 namedCurve 私钥被输出为短编码
            var explicitParams = new ECDomainParameters(
                priv.Parameters.Curve, priv.Parameters.G, priv.Parameters.N, priv.Parameters.H, priv.Parameters.GetSeed());
            var explicitPriv = new ECPrivateKeyParameters(priv.D, explicitParams);
            return EcdsaKeyHelper.ExportPrivateKeyPem(explicitPriv);
        }

        /// <summary>
        /// 根据下拉框选项将 EC 公钥导出为 namedCurve 或 specifiedCurve
        /// </summary>
        private string ExportPublicKeyByStandard(ECPublicKeyParameters pub)
        {
            string standard = comboPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardSpecifiedCurve;
            return ExportPublicKeyByStandard(pub, standard);
        }

        private string ExportPublicKeyByStandard(ECPublicKeyParameters pub, string standard)
        {
            if (standard == PublicKeyStandardNamedCurve)
                return EcdsaKeyHelper.ExportPublicKeyPemNamedCurve(pub);

            // specifiedCurve: 强制使用显式参数，避免 namedCurve 导入后又被输出为 OID 格式
            var explicitParams = new ECDomainParameters(
                pub.Parameters.Curve, pub.Parameters.G, pub.Parameters.N, pub.Parameters.H, pub.Parameters.GetSeed());
            var explicitPub = new ECPublicKeyParameters(pub.Q, explicitParams);
            return EcdsaKeyHelper.ExportPublicKeyPem(explicitPub);
        }

        #endregion

        #region 公共状态通知
        private void SetStatus(string message) => StatusChanged?.Invoke(message);
        #endregion

        #region 哈希算法名称转换
        private static string GetSignerAlgorithm(string uiHash) => uiHash switch
        {
            "SHA-224" => "SHA-224withECDSA",
            "SHA-256" => "SHA-256withECDSA",
            "SHA-384" => "SHA-384withECDSA",
            "SHA-512" => "SHA-512withECDSA",
            "SHA3-224" => "SHA3-224withECDSA",
            "SHA3-256" => "SHA3-256withECDSA",
            "SHA3-384" => "SHA3-384withECDSA",
            "SHA3-512" => "SHA3-512withECDSA",
            _ => "SHA-256withECDSA"
        };

        private string GetSelectedHash() => comboHashAlgorithm.SelectedItem?.ToString() ?? "SHA-256";
        #endregion

        #region 密钥生成
        private void BtnGenerateKeyPair_Click(object? sender, EventArgs e)
        {
            try
            {
                string curveName = GetSelectedCurve();
                string displayName = EcdsaCurveNames.GetDisplayName(curveName);
                SetStatus($"正在生成 {displayName} 密钥对...");

                var kp = EcdsaAlgorithm.GenerateKeyPair(curveName);
                var priv = (ECPrivateKeyParameters)kp.Private;
                var pub = (ECPublicKeyParameters)kp.Public;

                _privateKeyPem = ExportPrivateKeyByStandard(priv);
                _publicKeyPem = ExportPublicKeyByStandard(pub);

                RefreshKeyDisplay();
                SetGenerateResult(curveName, "密钥对匹配（新生成）");

                _lastSignHashAlgorithm = string.Empty;
                SetStatus($"{displayName} 密钥对生成完成");
            }
            catch (Exception ex)
            {
                SetGenerateResult(GetSelectedCurve(), $"❌ 生成密钥对失败: {ex.Message}");
                SetStatus("生成密钥对失败");
            }
        }
        #endregion

        #region 从私钥提取公钥
        private void BtnGetPublicKeyFromPrivate_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPrivateKey.Text))
                {
                    AppendKeyResult("❌ 私钥为空，无法提取公钥", Color.Red);
                    SetStatus("私钥为空，无法提取公钥");
                    return;
                }

                SetStatus("正在从私钥提取公钥...");

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textPrivateKey.Text.Trim(), true));

                // 提取公钥前先判断私钥曲线是否可识别
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                if (curveName == "未知曲线")
                {
                    AppendKeyResult("❌ 私钥生成逻辑非法，不符合标准 OpenSSL", Color.Red);
                    SetStatus("私钥曲线无法识别，不符合标准 OpenSSL");
                    return;
                }

                var pub = EcdsaAlgorithm.GetPublicKey(priv);

                _privateKeyPem = ExportPrivateKeyByStandard(priv);
                _publicKeyPem = ExportPublicKeyByStandard(pub);

                RefreshKeyDisplay();
                AppendKeyResult("✅ 密钥对匹配（从私钥提取）", Color.Green, GetSelectedCurve());
                SetStatus("从私钥提取公钥完成");
            }
            catch (Exception ex)
            {
                AppendKeyResult($"❌ 提取公钥失败: {ex.Message}", Color.Red, GetSelectedCurve());
                SetStatus("提取公钥失败");
            }
        }
        #endregion

        #region 获取私钥曲线类型
        private void BtnGetCurveType_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPrivateKey.Text))
                {
                    AppendKeyResult("❌ 私钥为空，无法获取曲线类型", Color.Red);
                    SetStatus("获取曲线类型失败：私钥为空");
                    return;
                }

                SetStatus("正在检测私钥曲线类型...");

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textPrivateKey.Text.Trim(), true));
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                string displayName = EcdsaCurveNames.GetDisplayName(curveName);

                AppendKeyResult($"曲线名称: {curveName}\n显示名称: {displayName}", Color.Green);
                SetStatus("获取曲线类型完成 - " + displayName);
            }
            catch (Exception ex)
            {
                AppendKeyResult($"❌ 获取曲线类型失败: {ex.Message}", Color.Red);
                SetStatus("获取曲线类型失败");
            }
        }
        #endregion

        #region 密钥对验证
        private void BtnValidateKeyPair_Click(object? sender, EventArgs e)
        {
            try
            {
                string privPem = textPrivateKey.Text?.Trim() ?? string.Empty;
                string pubPem = textPublicKey.Text?.Trim() ?? string.Empty;

                if (string.IsNullOrWhiteSpace(privPem) || string.IsNullOrWhiteSpace(pubPem))
                {
                    SetGenerateResult(GetSelectedCurve(), "❌ 请先生成或导入密钥对");
                    return;
                }

                string testHash = GetSelectedHash();
                string signerAlg = GetSignerAlgorithm(testHash);

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(privPem, true));
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(ConvertDisplayToPem(pubPem, false));

                var testData = Encoding.UTF8.GetBytes("ECDSA_KeyPair_Validation");
                ISigner signer = SignerUtilities.GetSigner(signerAlg);
                signer.Init(true, priv);
                signer.BlockUpdate(testData, 0, testData.Length);
                byte[] signature = signer.GenerateSignature();

                ISigner verifier = SignerUtilities.GetSigner(signerAlg);
                verifier.Init(false, pub);
                verifier.BlockUpdate(testData, 0, testData.Length);
                bool isValid = verifier.VerifySignature(signature);

                if (isValid)
                {
                    SetGenerateResult(GetSelectedCurve(), $"密钥对完全匹配（{testHash}）");
                    SetStatus("密钥对验证完成 - 完全匹配");
                }
                else
                {
                    SetGenerateResult(GetSelectedCurve(), "❌ 密钥对不匹配");
                    SetStatus("密钥对验证完成 - 不匹配");
                }
            }
            catch (Exception ex)
            {
                SetGenerateResult(GetSelectedCurve(), $"❌ 验证异常: {ex.Message}");
                SetStatus("验证密钥对失败");
            }
        }
        #endregion

        #region 签名
        private void BtnSign_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPrivateKey.Text))
                { SetSignResult("❌ 私钥为空，无法签名", Color.Red); return; }

                if (string.IsNullOrWhiteSpace(textPublicKey.Text))
                { SetSignResult("❌ 公钥为空，无法签名", Color.Red); return; }

                if (string.IsNullOrWhiteSpace(textPlainData.Text))
                { SetSignResult("❌ 原始数据为空，无法签名", Color.Red); return; }

                string hashAlg = GetSelectedHash();
                string signerAlg = GetSignerAlgorithm(hashAlg);

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textPrivateKey.Text.Trim(), true));
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(ConvertDisplayToPem(textPublicKey.Text.Trim(), false));

                // 探针验证密钥对匹配
                byte[] probe = Encoding.UTF8.GetBytes("__ECDSA_INTERNAL_PROBE__");
                ISigner ps = SignerUtilities.GetSigner(signerAlg); ps.Init(true, priv); ps.BlockUpdate(probe, 0, probe.Length);
                byte[] psig = ps.GenerateSignature();
                ISigner pv = SignerUtilities.GetSigner(signerAlg); pv.Init(false, pub); pv.BlockUpdate(probe, 0, probe.Length);
                if (!pv.VerifySignature(psig))
                { SetSignResult("❌ 密钥对不匹配，无法签名", Color.Red); SetStatus("签名失败 - 密钥对不匹配"); return; }

                var data = Encoding.UTF8.GetBytes(textPlainData.Text);
                ISigner s = SignerUtilities.GetSigner(signerAlg); s.Init(true, priv); s.BlockUpdate(data, 0, data.Length);
                byte[] signature = s.GenerateSignature();

                textSignature.Text = (comboSignatureFormat.SelectedItem?.ToString() == "Hex")
                    ? Convert.ToHexString(signature).ToLowerInvariant()
                    : Convert.ToBase64String(signature);

                _lastSignHashAlgorithm = hashAlg;
                SetSignResult("签名成功", Color.Green);
                SetStatus($"ECDSA 签名完成 - {signerAlg}");
            }
            catch (Exception ex)
            {
                SetSignResult($"❌ 签名失败: {ex.Message}", Color.Red);
                SetStatus("签名失败");
            }
        }
        #endregion

        #region 验签
        private void BtnVerify_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPublicKey.Text))
                { SetSignResult("❌ 公钥为空，无法验签", Color.Red); return; }

                if (string.IsNullOrWhiteSpace(textPlainData.Text) || string.IsNullOrWhiteSpace(textSignature.Text))
                { SetSignResult("❌ 原始数据或签名为空", Color.Red); return; }

                string hashAlg = GetSelectedHash();
                string signerAlg = GetSignerAlgorithm(hashAlg);

                var pub = EcdsaKeyHelper.ImportPublicKeyPem(ConvertDisplayToPem(textPublicKey.Text.Trim(), false));
                byte[] sig = (comboSignatureFormat.SelectedItem?.ToString() == "Hex")
                    ? Convert.FromHexString(textSignature.Text.Trim())
                    : Convert.FromBase64String(textSignature.Text.Trim());

                ISigner s = SignerUtilities.GetSigner(signerAlg);
                s.Init(false, pub);
                s.BlockUpdate(Encoding.UTF8.GetBytes(textPlainData.Text), 0, Encoding.UTF8.GetByteCount(textPlainData.Text));

                if (s.VerifySignature(sig))
                    SetSignResult($"签名验证通过（{hashAlg}）", Color.Green);
                else
                    SetSignResult("❌ 签名验证失败", Color.Red);
            }
            catch (Exception ex)
            {
                SetSignResult($"❌ 验签异常: {ex.Message}", Color.Red);
            }
        }
        #endregion

        #region 格式转换
        private void BtnConvertKey_Click(object? sender, EventArgs e)
        {
            try
            {
                TextBox targetBox = radioPrivateKey.Checked ? textPrivateKey : textPublicKey;
                if (string.IsNullOrWhiteSpace(targetBox.Text))
                { MessageBox.Show("请先输入要转换的密钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                string input = targetBox.Text.Trim();
                string inputFormat = DetectKeyFormat(input);
                string tf = comboOutputFormat.SelectedItem?.ToString() ?? "Base64";
                if (inputFormat == tf)
                { MessageBox.Show($"输入格式和输出格式相同（{inputFormat}），无需转换！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }

                targetBox.Text = FormatConversionHelper.ConvertStringFormat(
                    input,
                    FormatConversionHelper.ParseInputFormat(inputFormat),
                    FormatConversionHelper.ParseOutputFormat(tf));

                if (radioPrivateKey.Checked) _privateKeyPem = targetBox.Text;
                else _publicKeyPem = targetBox.Text;

                AppendValidationResult("格式已转换", Color.Gray);
                SetStatus($"格式转换完成 - {inputFormat} → {tf}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"格式转换失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("格式转换失败");
            }
        }

        private void BtnConvertPrivateKeyStandard_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_privateKeyPem))
                {
                    MessageBox.Show("当前没有私钥内容可转换，请先生成或导入私钥。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string pem = ConvertDisplayToPem(_privateKeyPem, true);
                ECPrivateKeyParameters priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                _privateKeyPem = ExportPrivateKeyByStandard(priv);
                textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, GetCurrentOutputFormat());

                string standard = comboPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardSec1;
                AppendValidationResult($"私钥已转换为 {standard}", Color.Gray);
                SetStatus($"私钥存储标准转换完成 - {standard}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换私钥存储标准失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("私钥存储标准转换失败");
            }
        }

        private void BtnConvertPublicKeyStandard_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_publicKeyPem))
                {
                    MessageBox.Show("当前没有公钥内容可转换，请先生成或导入公钥。", "提示",
                        MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                string pem = ConvertDisplayToPem(_publicKeyPem, false);
                ECPublicKeyParameters pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
                _publicKeyPem = ExportPublicKeyByStandard(pub);
                textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, GetCurrentOutputFormat());

                string standard = comboPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardSpecifiedCurve;
                AppendValidationResult($"公钥已转换为 {standard}", Color.Gray);
                SetStatus($"公钥存储标准转换完成 - {standard}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换公钥存储标准失败：{ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                SetStatus("公钥存储标准转换失败");
            }
        }

        private static string DetectKeyFormat(string input)
        {
            if (input.Contains("BEGIN", StringComparison.OrdinalIgnoreCase) || input.Contains("END", StringComparison.OrdinalIgnoreCase))
                return "PEM";

            string stripped = input.Replace("\r", "").Replace("\n", "").Replace(" ", "");
            if (string.IsNullOrEmpty(stripped)) return "PEM";

            bool isHex = stripped.All(c => "0123456789abcdefABCDEF".Contains(c));
            if (isHex) return "Hex";

            bool isBase64 = stripped.All(c => "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=".Contains(c));
            if (isBase64) return "Base64";

            return "PEM";
        }
        #endregion

        #region 文件签名/验签
        private void BtnSignFile_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_privateKeyPem))
                { MessageBox.Show("请先生成或导入私钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                using OpenFileDialog od = new() { Title = "选择要签名的文件", Filter = "所有文件|*.*" };
                if (od.ShowDialog() != DialogResult.OK) return;
                using SaveFileDialog sd = new() { Title = "保存签名文件", Filter = "签名文件|*.sig", FileName = Path.GetFileNameWithoutExtension(od.FileName) + ".sig" };
                if (sd.ShowDialog() != DialogResult.OK) return;
                string ha = GetSelectedHash(), sa = GetSignerAlgorithm(ha);
                ISigner s = SignerUtilities.GetSigner(sa); s.Init(true, EcdsaKeyHelper.ImportPrivateKeyPem(_privateKeyPem));
                byte[] fd = File.ReadAllBytes(od.FileName); s.BlockUpdate(fd, 0, fd.Length);
                byte[] sig = s.GenerateSignature();
                File.WriteAllText(sd.FileName, (comboSignatureFormat.SelectedItem?.ToString() == "Hex") ? Convert.ToHexString(sig).ToLowerInvariant() : Convert.ToBase64String(sig), Encoding.UTF8);
                _lastSignHashAlgorithm = ha;
                SetStatus("文件签名完成");
            }
            catch (Exception ex) { MessageBox.Show($"文件签名失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnVerifyFile_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(_publicKeyPem))
                { MessageBox.Show("请先生成或导入公钥！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                using OpenFileDialog od = new() { Title = "选择原始文件", Filter = "所有文件|*.*" };
                if (od.ShowDialog() != DialogResult.OK) return;
                using OpenFileDialog sd = new() { Title = "选择签名文件", Filter = "签名文件|*.sig" };
                if (sd.ShowDialog() != DialogResult.OK) return;
                string ha = GetSelectedHash(), sa = GetSignerAlgorithm(ha);
                ISigner s = SignerUtilities.GetSigner(sa); s.Init(false, EcdsaKeyHelper.ImportPublicKeyPem(_publicKeyPem));
                byte[] fd = File.ReadAllBytes(od.FileName); s.BlockUpdate(fd, 0, fd.Length);
                string sigText = File.ReadAllText(sd.FileName).Trim();
                byte[] sig = (comboSignatureFormat.SelectedItem?.ToString() == "Hex") ? Convert.FromHexString(sigText) : Convert.FromBase64String(sigText);
                if (s.VerifySignature(sig)) AppendValidationResult($"✅ 文件签名验证通过（{ha}）", Color.Green);
                else AppendValidationResult("❌ 文件签名验证失败", Color.Red);
            }
            catch (Exception ex) { MessageBox.Show($"文件验签失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); AppendValidationResult("❌ 文件验签异常", Color.Red); }
        }
        #endregion

        #region 导入/保存/粘贴/清空/复制
        private void BtnImportPrivateKey_Click(object? s, EventArgs e) => ImportKey(true);
        private void BtnImportPublicKey_Click(object? s, EventArgs e) => ImportKey(false);

        private void ImportKey(bool isPrivate)
        {
            try
            {
                using OpenFileDialog d = new() { Title = isPrivate ? "导入私钥文件" : "导入公钥文件", Filter = "密钥文件|*.pem;*.key;*.txt|所有文件|*.*" };
                if (d.ShowDialog() != DialogResult.OK) return;
                string c = File.ReadAllText(d.FileName).Trim();
                string pem = ConvertDisplayToPem(c, isPrivate);
                if (isPrivate) EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                else EcdsaKeyHelper.ImportPublicKeyPem(pem);
                UIOutputFormat fmt = GetCurrentOutputFormat();
                if (isPrivate)
                {
                    var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                    _privateKeyPem = ExportPrivateKeyByStandard(priv);
                    textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, fmt);
                }
                else
                {
                    var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
                    _publicKeyPem = ExportPublicKeyByStandard(pub);
                    textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, fmt);
                }
                AppendValidationResult("新密钥已导入", Color.Gray);
                SetStatus($"{(isPrivate ? "私钥" : "公钥")}导入完成");
            }
            catch (Exception ex) { MessageBox.Show($"导入失败：{ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error); }
        }

        private void BtnSavePrivateKey_Click(object? s, EventArgs e) => SaveKeyToFile(_privateKeyPem, "私钥", "private");
        private void BtnSavePublicKey_Click(object? s, EventArgs e) => SaveKeyToFile(_publicKeyPem, "公钥", "public");

        private void SaveKeyToFile(string keyContent, string keyTypeName, string fileSuffix)
        {
            if (string.IsNullOrEmpty(keyContent))
            { MessageBox.Show($"没有可保存的{keyTypeName}！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }
            using SaveFileDialog d = new() { Title = $"保存{keyTypeName}", Filter = "PEM文件|*.pem|文本文件|*.txt", FileName = $"ecdsa_{fileSuffix}_key.pem" };
            if (d.ShowDialog() == DialogResult.OK) { File.WriteAllText(d.FileName, keyContent, Encoding.UTF8); SetStatus($"{keyTypeName}保存成功"); }
        }

        private void BtnCopySignature_Click(object? s, EventArgs e)
        {
            TrySetClipboardText(textSignature.Text, "签名已复制到剪贴板");
        }

        private void BtnCopyPlainData_Click(object? s, EventArgs e)
        {
            TrySetClipboardText(textPlainData.Text, "原始数据已复制到剪贴板");
        }

        private void BtnPastePlainData_Click(object? s, EventArgs e)
        {
            if (Clipboard.ContainsText()) { textPlainData.Text = Clipboard.GetText(); SetStatus("原始数据已粘贴"); }
        }

        private void BtnClearPlainData_Click(object? s, EventArgs e)
        {
            textPlainData.Clear();
            SetStatus("原始数据已清空");
        }

        private void BtnCopySignatureData_Click(object? s, EventArgs e)
        {
            TrySetClipboardText(textSignature.Text, "签名已复制到剪贴板");
        }

        private void BtnPasteSignatureData_Click(object? s, EventArgs e)
        {
            if (Clipboard.ContainsText()) { textSignature.Text = Clipboard.GetText(); SetStatus("签名已粘贴"); }
        }

        private void BtnClearSignatureData_Click(object? s, EventArgs e)
        {
            textSignature.Clear();
            SetStatus("签名已清空");
        }

        private void BtnClearAll_Click(object? s, EventArgs e)
        {
            textPrivateKey.Clear(); textPublicKey.Clear(); textPlainData.Clear(); textSignature.Clear();
            _privateKeyPem = _publicKeyPem = _lastSignHashAlgorithm = string.Empty;
            ResetValidationResult("未验证", Color.Gray);
            textKeyResult.Clear();
            textKeyResult.Text = "从私钥提取/曲线检测：\n等待操作...";
            textKeyResult.ForeColor = Color.Gray;
            SetStatus("已清空所有内容");
        }

        private void BtnPastePrivateKey_Click(object? s, EventArgs e)
        {
            if (!Clipboard.ContainsText()) { MessageBox.Show("剪贴板中没有文本内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            string c = Clipboard.GetText().Trim();
            try
            {
                string pem = ConvertDisplayToPem(c, true);
                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                _privateKeyPem = ExportPrivateKeyByStandard(priv);
                textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, GetCurrentOutputFormat());
                SetStatus("私钥已从剪贴板粘贴");
            }
            catch
            {
                _privateKeyPem = c;
                textPrivateKey.Text = c;
                SetStatus("私钥已从剪贴板粘贴");
            }
        }

        private void BtnClearPrivateKey_Click(object? s, EventArgs e) { textPrivateKey.Clear(); _privateKeyPem = string.Empty; SetStatus("私钥已清空"); }

        private void BtnPastePublicKey_Click(object? s, EventArgs e)
        {
            if (!Clipboard.ContainsText()) { MessageBox.Show("剪贴板中没有文本内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information); return; }
            string c = Clipboard.GetText().Trim();
            try
            {
                string pem = ConvertDisplayToPem(c, false);
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
                _publicKeyPem = ExportPublicKeyByStandard(pub);
                textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, GetCurrentOutputFormat());
                SetStatus("公钥已从剪贴板粘贴");
            }
            catch
            {
                _publicKeyPem = c;
                textPublicKey.Text = c;
                SetStatus("公钥已从剪贴板粘贴");
            }
        }

        private void BtnClearPublicKey_Click(object? s, EventArgs e) { textPublicKey.Clear(); _publicKeyPem = string.Empty; SetStatus("公钥已清空"); }

        /// <summary>
        /// 安全地将文本复制到剪贴板，自动重试并在失败时给出友好提示。
        /// 避免剪贴板被其他进程临时占用时直接崩溃。
        /// </summary>
        private bool TrySetClipboardText(string text, string successStatus, string? emptyMessage = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (!string.IsNullOrEmpty(emptyMessage))
                {
                    MessageBox.Show(emptyMessage, "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                SetStatus(string.IsNullOrEmpty(emptyMessage) ? "复制失败：内容为空" : $"复制失败：{emptyMessage}");
                return false;
            }

            const int maxAttempts = 3;
            Exception? lastEx = null;
            for (int i = 0; i < maxAttempts; i++)
            {
                try
                {
                    Clipboard.SetText(text);
                    if (!string.IsNullOrEmpty(successStatus))
                        SetStatus(successStatus);
                    return true;
                }
                catch (ExternalException ex)
                {
                    lastEx = ex;
                    // 剪贴板可能被其他进程临时锁定，短暂等待后重试
                    System.Threading.Thread.Sleep(50);
                }
            }

            MessageBox.Show(
                $"剪贴板操作失败: {lastEx?.Message}\n请稍后再试或关闭可能占用剪贴板的程序。",
                "复制失败",
                MessageBoxButtons.OK,
                MessageBoxIcon.Error);
            SetStatus("复制失败：剪贴板被占用");
            return false;
        }

        private void BtnCopyPrivateKey_Click(object? sender, EventArgs e)
        {
            TrySetClipboardText(textPrivateKey.Text, "私钥已复制到剪贴板", "私钥为空，无法复制！");
        }

        private void BtnCopyPublicKey_Click(object? sender, EventArgs e)
        {
            TrySetClipboardText(textPublicKey.Text, "公钥已复制到剪贴板", "公钥为空，无法复制！");
        }
        #endregion

        #region 控件事件
        private void ComboOutputFormat_SelectedIndexChanged(object? s, EventArgs e)
        {
            // 如果文本框当前内容不是 PEM，先反向推导为 PEM 并更新缓存，
            // 确保切换格式后显示的是正确的规范化内容
            if (!string.IsNullOrEmpty(textPrivateKey.Text))
            {
                try { _privateKeyPem = ConvertDisplayToPem(textPrivateKey.Text.Trim(), true); }
                catch { }
            }
            if (!string.IsNullOrEmpty(textPublicKey.Text))
            {
                try { _publicKeyPem = ConvertDisplayToPem(textPublicKey.Text.Trim(), false); }
                catch { }
            }

            RefreshKeyDisplay();
        }

        private void TextPrivateKey_TextChanged(object? s, EventArgs e)
        {
            // 不再自动清空左侧运行结果历史，保证生成/验证等记录持续累积
        }
        #endregion

        #region 辅助方法
        private void SetGenerateResult(string curveName, string status)
        {
            Color color = status.Contains('❌') ? Color.Red : Color.Green;
            AppendValidationResult($"{status}\n使用曲线: {EcdsaCurveNames.GetDisplayName(curveName)}", color);
        }

        private void SetSignResult(string status, Color color)
        {
            AppendValidationResult($"{status}\n使用曲线: {EcdsaCurveNames.GetDisplayName(GetSelectedCurve())}（{GetSelectedHash()}）", color);
        }

        private void ResetValidationResult(string text, Color color)
        {
            labelValidationResult.Clear();
            labelValidationResult.SelectionStart = 0;
            labelValidationResult.SelectionLength = 0;
            labelValidationResult.SelectionColor = color;
            labelValidationResult.SelectedText = "验证结果: " + text;
        }

        private void AppendValidationResult(string message, Color color)
        {
            if (labelValidationResult.Text.Contains("未验证") || labelValidationResult.Text.Contains("等待操作"))
                labelValidationResult.Clear();

            string entry = $"运行时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}此次任务: {message}{Environment.NewLine}{Environment.NewLine}";

            // 与右侧运行结果一致：上面新、下面旧
            labelValidationResult.Select(0, 0);
            labelValidationResult.SelectionColor = color;
            labelValidationResult.SelectedText = entry;

            labelValidationResult.SelectionStart = 0;
            labelValidationResult.ScrollToCaret();
        }

        private void AppendKeyResult(string message, Color color, string? curveName = null)
        {
            if (textKeyResult.Text.Contains("等待操作"))
                textKeyResult.Clear();

            string detail = message;
            if (!string.IsNullOrEmpty(curveName))
            {
                detail += $"\n使用曲线: {EcdsaCurveNames.GetDisplayName(curveName)}";
            }

            string entry = $"运行时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}此次任务: {detail}{Environment.NewLine}{Environment.NewLine}";

            textKeyResult.Select(0, 0);
            textKeyResult.SelectionColor = color;
            textKeyResult.SelectedText = entry;

            textKeyResult.SelectionStart = 0;
            textKeyResult.ScrollToCaret();
        }

        private void RefreshKeyDisplay()
        {
            UIOutputFormat fmt = GetCurrentOutputFormat();

            // 确保缓存的密钥始终是标准 PEM 格式
            if (!string.IsNullOrEmpty(_privateKeyPem) && !_privateKeyPem.Contains("-----BEGIN"))
            {
                try { _privateKeyPem = ConvertDisplayToPem(_privateKeyPem, true); }
                catch { }
            }
            if (!string.IsNullOrEmpty(_publicKeyPem) && !_publicKeyPem.Contains("-----BEGIN"))
            {
                try { _publicKeyPem = ConvertDisplayToPem(_publicKeyPem, false); }
                catch { }
            }

            if (!string.IsNullOrEmpty(_privateKeyPem)) textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, fmt);
            if (!string.IsNullOrEmpty(_publicKeyPem)) textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, fmt);
        }

        private UIOutputFormat GetCurrentOutputFormat() => comboOutputFormat.SelectedItem?.ToString() switch
        {
            "Base64" => UIOutputFormat.Base64,
            "Hex大写" => UIOutputFormat.HexUpper,
            "Hex小写" => UIOutputFormat.HexLower,
            _ => UIOutputFormat.PEM
        };

        private static string FormatKeyForDisplay(string pem, UIOutputFormat format)
        {
            if (format == UIOutputFormat.PEM) return pem;
            var sb = new StringBuilder();
            foreach (var line in pem.Split('\n'))
                if (!line.StartsWith("-----")) sb.Append(line.Trim());
            string b64 = sb.ToString();
            if (format == UIOutputFormat.Base64) return b64;

            string hex = Convert.ToHexString(Convert.FromBase64String(b64));
            return format == UIOutputFormat.HexUpper ? hex : hex.ToLowerInvariant();
        }

        /// <summary>
        /// 将文本框中显示的内容（PEM / Base64 / Hex）统一转换为标准 PEM 格式，便于导入操作。
        /// </summary>
        private static string ConvertDisplayToPem(string keyText, bool isPrivate)
        {
            string t = keyText.Trim();
            if (t.Contains("-----BEGIN")) return t;

            string stripped = string.Concat(t.Where(c => !char.IsWhiteSpace(c)));

            // 尝试 Hex 解码
            if (stripped.Length % 2 == 0 && stripped.All(c => "0123456789abcdefABCDEF".Contains(c)))
            {
                try
                {
                    byte[] bytes = Convert.FromHexString(stripped);
                    string b64 = Convert.ToBase64String(bytes);
                    return WrapInPem(b64, isPrivate);
                }
                catch { }
            }

            // 尝试 Base64 解码
            try
            {
                Convert.FromBase64String(stripped);
                return WrapInPem(stripped, isPrivate);
            }
            catch { }

            return t;
        }

        private static string WrapInPem(string base64Content, bool isPrivate)
        {
            var sb = new StringBuilder();
            string header = isPrivate ? "-----BEGIN EC PRIVATE KEY-----" : "-----BEGIN PUBLIC KEY-----";
            string footer = isPrivate ? "-----END EC PRIVATE KEY-----" : "-----END PUBLIC KEY-----";
            sb.AppendLine(header);
            for (int i = 0; i < base64Content.Length; i += 64)
            {
                sb.AppendLine(base64Content.Substring(i, Math.Min(64, base64Content.Length - i)));
            }
            sb.AppendLine(footer);
            return sb.ToString();
        }
        #endregion


        private void LabelCurveHeader_Click(object sender, EventArgs e)
        {

        }

        private void LabelCurve_Click(object sender, EventArgs e)
        {

        }
    }
}
