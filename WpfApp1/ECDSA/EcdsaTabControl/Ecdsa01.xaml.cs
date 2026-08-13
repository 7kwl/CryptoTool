using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;

namespace WpfApp1.ECDSA.EcdsaTabControl
{
    /// <summary>
    /// ECDSA01 - ECDH 密钥交换页面逻辑（由 CryptoTool.Win\ECDSA\EcdsaTabControl.cs 移植）
    /// 密钥对生成 / ECIES 与 8gwifi.org 加解密 / 存储标准转换 / 曲线级联
    /// </summary>
    public partial class Ecdsa01 : UserControl
    {
        /// <summary>状态栏消息通知（供宿主订阅，可选）</summary>
        public event Action<string>? StatusChanged;

        /// <summary>
        /// 将 ECDH 操作结果同步推送到顶部"运行结果"框。
        /// 由 EcdsaTabPage.ResultAppender 注入（最终指向 EcdsaMainPage.AppendValidationResult）。
        /// </summary>
        public Action<string, SolidColorBrush>? AppendToHost { get; set; }

        /// <summary>
        /// 将曲线/密钥检测结果同步推送到顶部"计算结果"框。
        /// 由 EcdsaTabPage.KeyResultAppender 注入（最终指向 EcdsaMainPage.AppendKeyResult）。
        /// </summary>
        public Action<string, SolidColorBrush>? AppendKeyToHost { get; set; }

        /// <summary>曲线分类数据：分类Key → (图标, 曲线列表)</summary>
        private Dictionary<string, (string Icon, List<KeyValuePair<string, string>> Curves)> _allCurveData = [];

        /// <summary>最近一次 ECDH 加密/解密使用的 IV</summary>
        private byte[]? _ecdhLastIV = null;

        // 密钥存储标准常量（与 WinForms 源版 EcdsaTabControl.CurveAndStandard.cs 一致）
        private const string PrivateKeyStandardNamedCurve = "SEC1 / RFC 5915（短编码 / namedCurve）";
        private const string PrivateKeyStandardSec1 = "SEC1/RFC 5915（长编码 / specifiedCurve）";
        private const string PublicKeyStandardNamedCurve = "RFC 5480/namedCurve";
        private const string PublicKeyStandardSpecifiedCurve = "RFC 5480/specifiedCurve";

        public Ecdsa01()
        {
            InitializeComponent();
            WireEvents();
            InitializeDefaults();
        }

        #region 初始化

        private void InitializeDefaults()
        {
            InitializeEcdhCurveList();
            InitializeEcdhKeyStandards();

            if (comboEcdhMode.Items.Count > 0) comboEcdhMode.SelectedIndex = 0;
            if (comboEcdhEncoding.Items.Count > 0) comboEcdhEncoding.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化 ECDH 私钥/公钥存储标准下拉框
        /// </summary>
        private void InitializeEcdhKeyStandards()
        {
            comboEcdhPrivateKeyStandard.Items.Clear();
            comboEcdhPrivateKeyStandard.Items.Add(PrivateKeyStandardNamedCurve);
            comboEcdhPrivateKeyStandard.Items.Add(PrivateKeyStandardSec1);
            comboEcdhPrivateKeyStandard.SelectedIndex = 0;

            comboEcdhPublicKeyStandard.Items.Clear();
            comboEcdhPublicKeyStandard.Items.Add(PublicKeyStandardNamedCurve);
            comboEcdhPublicKeyStandard.Items.Add(PublicKeyStandardSpecifiedCurve);
            comboEcdhPublicKeyStandard.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化 ECDH 曲线分类级联下拉框（分类 → 曲线）
        /// </summary>
        private void InitializeEcdhCurveList()
        {
            if (_allCurveData.Count == 0)
                _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

            comboEcdhCategory.DisplayMemberPath = "Text";
            comboEcdhCategory.SelectedValuePath = "Value";
            comboEcdhCategory.Items.Clear();
            foreach (var cat in _allCurveData)
            {
                comboEcdhCategory.Items.Add(new
                {
                    Text = $"{cat.Value.Icon} {cat.Key}",
                    Value = cat.Key
                });
            }

            comboEcdhCurve.DisplayMemberPath = "Value";
            comboEcdhCurve.SelectedValuePath = "Key";

            if (comboEcdhCategory.Items.Count > 0)
                comboEcdhCategory.SelectedIndex = 0;
        }

        #endregion

        #region 事件挂接

        private void WireEvents()
        {
            btnGenerateEcdhKeys.Click += BtnGenerateEcdhKeys_Click;
            btnEcdhEncrypt.Click += BtnEcdhEncrypt_Click;
            btnEcdhDecrypt.Click += BtnEcdhDecrypt_Click;
            btnEcdhCopyResult.Click += BtnEcdhCopyResult_Click;
            btnEcdhPasteInput.Click += BtnEcdhPasteInput_Click;
            btnEcdhClear.Click += BtnEcdhClear_Click;
            btnEcdhAliceCurve.Click += BtnEcdhAliceCurve_Click;
            btnEcdhBobCurve.Click += BtnEcdhBobCurve_Click;
            btnConvertEcdhPrivateKeyStandard.Click += BtnConvertEcdhPrivateKeyStandard_Click;
            btnConvertEcdhPublicKeyStandard.Click += BtnConvertEcdhPublicKeyStandard_Click;

            comboEcdhCategory.SelectionChanged += ComboEcdhCategory_SelectedIndexChanged;
            comboEcdhPrivateKeyStandard.SelectionChanged += ComboEcdhPrivateKeyStandard_SelectedIndexChanged;
            comboEcdhPublicKeyStandard.SelectionChanged += ComboEcdhPublicKeyStandard_SelectedIndexChanged;

            // 下拉框支持滚轮直接切换选中项（复刻 WinForms ComboBox 行为）
            AttachComboBoxWheel(comboEcdhCategory);
            AttachComboBoxWheel(comboEcdhCurve);
            AttachComboBoxWheel(comboEcdhMode);
            AttachComboBoxWheel(comboEcdhEncoding);
            AttachComboBoxWheel(comboEcdhPrivateKeyStandard);
            AttachComboBoxWheel(comboEcdhPublicKeyStandard);
        }

        /// <summary>
        /// 让下拉框支持鼠标滚轮切换选中项：上滚选上一个、下滚选下一个。
        /// </summary>
        private static void AttachComboBoxWheel(ComboBox combo)
        {
            combo.PreviewMouseWheel += (s, e) =>
            {
                // 下拉已展开时不做处理，避免与列表滚动冲突
                if (combo.Items.Count <= 0 || combo.IsDropDownOpen)
                    return;

                int target = combo.SelectedIndex + (e.Delta > 0 ? -1 : 1);
                if (target >= 0 && target < combo.Items.Count)
                {
                    combo.SelectedIndex = target;
                    e.Handled = true;
                }
            };
        }

        #endregion

        #region 公共状态通知

        private void SetStatus(string message) => StatusChanged?.Invoke(message);

        /// <summary>
        /// 记录操作结果：写入顶部"运行结果"框 + 推送到状态栏；含 ❌/⚠️ 时弹窗提示
        /// </summary>
        private void AppendValidationResult(string message, SolidColorBrush color)
        {
            // 1. 优先写入顶部"运行结果"框（由 EcdsaTabPage.ResultAppender 注入的回调）
            AppendToHost?.Invoke(message, color);

            // 2. 推状态栏事件
            SetStatus(message);

            // 3. 含错误/警告时弹窗提示
            if (message.Contains('\u274C') || message.Contains('\u26A0'))
            {
                MessageBox.Show(message, "提示", MessageBoxButton.OK,
                    message.Contains('\u274C') ? MessageBoxImage.Warning : MessageBoxImage.Information);
            }
        }

        #endregion

        #region ECDH 业务逻辑（由 EcdsaTabControl.cs 移植）

        private void ComboEcdhCategory_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (comboEcdhCategory.SelectedItem is not { } selected)
                return;

            dynamic d = selected;
            string categoryKey = (string)d.Value;

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

        private void BtnGenerateEcdhKeys_Click(object? sender, RoutedEventArgs e)
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

                textEcdhAlicePrivate.Text = ExportPrivateKeyByStandard(alicePriv, comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardNamedCurve);
                textEcdhAlicePublic.Text = ExportPublicKeyByStandard(alicePub, comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve);
                textEcdhBobPrivate.Text = ExportPrivateKeyByStandard(bobPriv, comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardNamedCurve);
                textEcdhBobPublic.Text = ExportPublicKeyByStandard(bobPub, comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve);

                byte[] shared = EcdhAlgorithm.DeriveSharedSecret(alicePriv, bobPub);
                textEcdhSharedKey.Text = Convert.ToBase64String(shared);

                // 生成新密钥对后，旧密文/IV/输入与新的密钥不再匹配，清空避免误用
                textEcdhInput.Clear();
                textEcdhOutput.Clear();
                textEcdhIV.Clear();
                _ecdhLastIV = null;

                AppendValidationResult($"✅ ECDH 密钥对已生成\n曲线: {EcdsaCurveNames.GetDisplayName(curve)}\n共享密钥长度: {shared.Length} 字节", Brushes.Green);
                SetStatus("ECDH 密钥对生成完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 生成 ECDH 密钥对失败: {ex.Message}", Brushes.Red);
                SetStatus("生成 ECDH 密钥对失败");
            }
        }

        private void BtnEcdhEncrypt_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEcdhInput.Text))
                { MessageBox.Show("请输入要加密的信息", "提示", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

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
                        AppendValidationResult($"❌ IV 格式无效: {ex.Message}", Brushes.Red);
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
                    { MessageBox.Show("请输入 Bob 公钥（接收方公钥）", "提示", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                    var bobPub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhBobPublic.Text.Trim());
                    byte[] combined = EcdhAlgorithm.EciesEncrypt(plain, bobPub, curve, mode, out shared, out iv, userIv);
                    output = Convert.ToBase64String(combined);
                    AppendValidationResult($"✅ ECIES 加密成功\n模式: {GetComboSelectedText(comboEcdhMode)}\n曲线: {EcdsaCurveNames.GetDisplayName(curve)}\n临时公钥: {EcdhAlgorithm.GetEphemeralPubKeyLength(curve)} 字节\n明文: {plain.Length} 字节 → 密文: {combined.Length} 字节", Brushes.Green);
                }
                else
                {
                    // 8gwifi.org 兼容：静态 ECDH，需要 Alice 私钥 + Bob 公钥
                    if (string.IsNullOrWhiteSpace(textEcdhAlicePrivate.Text) || string.IsNullOrWhiteSpace(textEcdhBobPublic.Text))
                    { MessageBox.Show("8gwifi.org 模式加密需要 Alice 私钥和 Bob 公钥", "提示", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                    var alicePriv = EcdsaKeyHelper.ImportPrivateKeyPem(textEcdhAlicePrivate.Text.Trim());
                    var bobPub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhBobPublic.Text.Trim());
                    output = EcdhAlgorithm.StaticEcdhEncrypt(plain, alicePriv, bobPub, mode, out shared, out iv, userIv);
                    AppendValidationResult($"✅ 8gwifi.org 加密成功\n模式: {GetComboSelectedText(comboEcdhMode)}\n曲线: {EcdsaCurveNames.GetDisplayName(curve)}\n明文: {plain.Length} 字节\n密文（不含 IV）: {Convert.FromBase64String(output).Length} 字节", Brushes.Green);
                }

                textEcdhOutput.Text = output;
                textEcdhSharedKey.Text = Convert.ToBase64String(shared);
                textEcdhIV.Text = EcdhAlgorithm.FormatIv(iv, mode);
                _ecdhLastIV = iv;
                SetStatus($"{GetComboSelectedText(comboEcdhMode)} 加密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ {GetComboSelectedText(comboEcdhMode)} 加密失败: {ex.Message}", Brushes.Red);
                SetStatus($"{GetComboSelectedText(comboEcdhMode)} 加密失败");
            }
        }

        private void BtnEcdhDecrypt_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                // 解密固定从"密文（可编辑）"框读取，不再回退到"明文"框
                // 避免用户误将明文当密文解密
                string cipherSource = textEcdhOutput.Text.Trim();
                if (string.IsNullOrWhiteSpace(cipherSource))
                {
                    AppendValidationResult("⚠️ 请先加密生成密文，或将 Base64 密文粘贴到 密文（可编辑） 框", Brushes.Orange);
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
                    { MessageBox.Show("请输入 Bob 私钥（接收方私钥）来解密", "提示", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                    var bobPriv = EcdsaKeyHelper.ImportPrivateKeyPem(textEcdhBobPrivate.Text.Trim());
                    byte[] combined;
                    try
                    {
                        combined = Convert.FromBase64String(cipherSource);
                    }
                    catch (FormatException)
                    {
                        AppendValidationResult("❌ 密文格式无效: 请输入 Base64 编码的密文，不是原始数字或字符串", Brushes.Red);
                        SetStatus("Base64 格式错误");
                        return;
                    }
                    try
                    {
                        plain = EcdhAlgorithm.EciesDecrypt(combined, bobPriv, curve, mode, out shared, out iv);
                    }
                    catch (InvalidCipherTextException)
                    {
                        AppendValidationResult($"❌ ECIES 解密失败: MAC 校验不通过，密钥或密文不匹配", Brushes.Red);
                        SetStatus("ECIES 解密失败");
                        return;
                    }
                    catch (ArgumentException argEx) when (argEx.Message.Contains("Specified argument"))
                    {
                        AppendValidationResult($"❌ 密文结构无效: {argEx.Message}。请检查密文是否完整、曲线是否匹配", Brushes.Red);
                        SetStatus("密文结构错误");
                        return;
                    }
                    textEcdhSharedKey.Text = Convert.ToBase64String(shared);
                    textEcdhIV.Text = EcdhAlgorithm.FormatIv(iv, mode);
                    _ecdhLastIV = iv;
                    AppendValidationResult($"✅ ECIES 解密成功\n模式: {GetComboSelectedText(comboEcdhMode)}\n明文长度: {plain.Length} 字节", Brushes.Green);
                }
                else
                {
                    // 8gwifi.org 兼容：静态 ECDH 解密，需要 Bob 私钥 + Alice 公钥
                    if (string.IsNullOrWhiteSpace(textEcdhBobPrivate.Text))
                    { MessageBox.Show("8gwifi.org 模式解密需要 Bob 私钥", "提示", MessageBoxButton.OK, MessageBoxImage.Warning); return; }
                    if (string.IsNullOrWhiteSpace(textEcdhAlicePublic.Text))
                    { MessageBox.Show("8gwifi.org 模式解密需要 Alice 公钥", "提示", MessageBoxButton.OK, MessageBoxImage.Warning); return; }

                    var bobPriv = EcdsaKeyHelper.ImportPrivateKeyPem(textEcdhBobPrivate.Text.Trim());
                    var alicePub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhAlicePublic.Text.Trim());
                    try
                    {
                        plain = EcdhAlgorithm.StaticEcdhDecrypt(cipherSource, bobPriv, alicePub, mode, out shared, out iv);
                    }
                    catch (InvalidCipherTextException)
                    {
                        AppendValidationResult($"❌ 8gwifi.org 解密失败: MAC 校验不通过，密钥或密文不匹配", Brushes.Red);
                        SetStatus("8gwifi.org 解密失败");
                        return;
                    }
                    textEcdhSharedKey.Text = Convert.ToBase64String(shared);
                    textEcdhIV.Text = EcdhAlgorithm.FormatIv(iv, mode);
                    _ecdhLastIV = iv;
                    AppendValidationResult($"✅ 8gwifi.org 解密成功\n模式: {GetComboSelectedText(comboEcdhMode)}\n明文长度: {plain.Length} 字节", Brushes.Green);
                }

                textEcdhInput.Text = GetEcdhEncoding().GetString(plain);
                SetStatus($"{GetComboSelectedText(comboEcdhMode)} 解密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ {GetComboSelectedText(comboEcdhMode)} 解密失败: {ex.Message}", Brushes.Red);
                SetStatus($"{GetComboSelectedText(comboEcdhMode)} 解密失败");
            }
        }

        private EcdhMode GetEcdhMode()
        {
            var selected = GetComboSelectedText(comboEcdhMode);
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
            var selected = GetComboSelectedText(comboEcdhEncoding);
            return selected switch
            {
                "GBK (GB2312)" => Encoding.GetEncoding("GBK"),
                "Unicode (UTF-16 LE)" => Encoding.Unicode,
                _ => Encoding.UTF8
            };
        }

        private void BtnEcdhCopyResult_Click(object? sender, RoutedEventArgs e)
        {
            TrySetClipboardText(textEcdhOutput.Text, "ECDH 结果已复制");
        }

        private void BtnEcdhPasteInput_Click(object? sender, RoutedEventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textEcdhOutput.Text = Clipboard.GetText().Trim();
                SetStatus("密文已粘贴到 ECDH 密文框");
            }
        }

        private void BtnEcdhClear_Click(object? sender, RoutedEventArgs e)
        {
            textEcdhInput.Clear();
            textEcdhOutput.Clear();
            textEcdhSharedKey.Clear();
            textEcdhIV.Clear();
            _ecdhLastIV = null;
            SetStatus("ECDH 输入/输出已清空");
        }

        private void BtnEcdhAliceCurve_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEcdhAlicePrivate.Text))
                {
                    AppendKeyToHost?.Invoke("⚠️ Alice 私钥为空，无法检测曲线", Brushes.DarkOrange);
                    SetStatus("检测 Alice 私钥曲线失败：私钥为空");
                    return;
                }

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textEcdhAlicePrivate.Text.Trim(), true));
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                AppendKeyToHost?.Invoke($"✅ Alice 私钥曲线: {curveName}\n显示名称: {EcdsaCurveNames.GetDisplayName(curveName)}", Brushes.Green);
                SetStatus("Alice 私钥曲线检测完成 - " + curveName);
            }
            catch (Exception ex)
            {
                AppendKeyToHost?.Invoke($"❌ 检测 Alice 私钥曲线失败: {ex.Message}", Brushes.Red);
                SetStatus("检测 Alice 私钥曲线失败");
            }
        }

        private void BtnEcdhBobCurve_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEcdhBobPrivate.Text))
                {
                    AppendKeyToHost?.Invoke("⚠️ Bob 私钥为空，无法检测曲线", Brushes.DarkOrange);
                    SetStatus("检测 Bob 私钥曲线失败：私钥为空");
                    return;
                }

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textEcdhBobPrivate.Text.Trim(), true));
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                AppendKeyToHost?.Invoke($"✅ Bob 私钥曲线: {curveName}\n显示名称: {EcdsaCurveNames.GetDisplayName(curveName)}", Brushes.Green);
                SetStatus("Bob 私钥曲线检测完成 - " + curveName);
            }
            catch (Exception ex)
            {
                AppendKeyToHost?.Invoke($"❌ 检测 Bob 私钥曲线失败: {ex.Message}", Brushes.Red);
                SetStatus("检测 Bob 私钥曲线失败");
            }
        }

        #endregion

        #region 密钥导出（按存储标准）

        /// <summary>
        /// 按存储标准导出私钥 PEM（namedCurve 短编码 或 specifiedCurve 长编码）
        /// </summary>
        private static string ExportPrivateKeyByStandard(ECPrivateKeyParameters priv, string standard)
        {
            if (standard == PrivateKeyStandardNamedCurve)
                return EcdsaKeyHelper.ExportPrivateKeyPemNamedCurve(priv);

            // SEC1/RFC 5915: 强制使用显式参数 (specifiedCurve)，避免 namedCurve 私钥被输出为短编码
            var explicitParams = new ECDomainParameters(
                priv.Parameters.Curve, priv.Parameters.G, priv.Parameters.N,
                priv.Parameters.H, priv.Parameters.GetSeed());
            var explicitPriv = new ECPrivateKeyParameters(priv.D, explicitParams);
            return EcdsaKeyHelper.ExportPrivateKeyPem(explicitPriv);
        }

        /// <summary>
        /// 按存储标准导出公钥 PEM（namedCurve 或 specifiedCurve）
        /// </summary>
        private static string ExportPublicKeyByStandard(ECPublicKeyParameters pub, string standard)
        {
            if (standard == PublicKeyStandardNamedCurve)
                return EcdsaKeyHelper.ExportPublicKeyPemNamedCurve(pub);

            // specifiedCurve: 强制使用显式参数，避免 namedCurve 导入后又被输出为 OID 格式
            var explicitParams = new ECDomainParameters(
                pub.Parameters.Curve, pub.Parameters.G, pub.Parameters.N,
                pub.Parameters.H, pub.Parameters.GetSeed());
            var explicitPub = new ECPublicKeyParameters(pub.Q, explicitParams);
            return EcdsaKeyHelper.ExportPublicKeyPem(explicitPub);
        }

        #endregion

        #region ECDH 密钥存储标准转换

        private void BtnConvertEcdhPrivateKeyStandard_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                string standard = comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardNamedCurve;
                int convertedCount = 0;
                if (ConvertEcdhPrivateKey(textEcdhAlicePrivate, standard)) convertedCount++;
                if (ConvertEcdhPrivateKey(textEcdhBobPrivate, standard)) convertedCount++;

                if (convertedCount == 0)
                {
                    MessageBox.Show("当前没有 Alice 或 Bob 的私钥内容可转换，请先生成或导入私钥。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                AppendValidationResult($"✅ {convertedCount} 组私钥已转换为 {standard}", Brushes.Green);
                SetStatus($"Alice/Bob 私钥存储标准转换完成 - {standard}");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ ECDH 私钥标准转换失败: {ex.Message}", Brushes.Red);
                SetStatus("ECDH 私钥标准转换失败");
            }
        }

        private static bool ConvertEcdhPrivateKey(TextBox textBox, string standard)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return false;

            string pem = ConvertDisplayToPem(textBox.Text.Trim(), true);
            var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
            string converted = ExportPrivateKeyByStandard(priv, standard);
            textBox.Text = FormatKeyForDisplay(converted, UIOutputFormat.PEM);
            return true;
        }

        private void BtnConvertEcdhPublicKeyStandard_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                string standard = comboEcdhPublicKeyStandard.SelectedItem?.ToString() ?? PublicKeyStandardNamedCurve;
                int convertedCount = 0;
                if (ConvertEcdhPublicKey(textEcdhAlicePublic, standard)) convertedCount++;
                if (ConvertEcdhPublicKey(textEcdhBobPublic, standard)) convertedCount++;

                if (convertedCount == 0)
                {
                    MessageBox.Show("当前没有 Alice 或 Bob 的公钥内容可转换，请先生成或导入公钥。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                AppendValidationResult($"✅ {convertedCount} 组公钥已转换为 {standard}", Brushes.Green);
                SetStatus($"Alice/Bob 公钥存储标准转换完成 - {standard}");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ ECDH 公钥标准转换失败: {ex.Message}", Brushes.Red);
                SetStatus("ECDH 公钥标准转换失败");
            }
        }

        private static bool ConvertEcdhPublicKey(TextBox textBox, string standard)
        {
            if (string.IsNullOrWhiteSpace(textBox.Text))
                return false;

            string pem = ConvertDisplayToPem(textBox.Text.Trim(), false);
            var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
            string converted = ExportPublicKeyByStandard(pub, standard);
            textBox.Text = FormatKeyForDisplay(converted, UIOutputFormat.PEM);
            return true;
        }

        private void ComboEcdhPrivateKeyStandard_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
        {
            try
            {
                string standard = comboEcdhPrivateKeyStandard.SelectedItem?.ToString() ?? PrivateKeyStandardNamedCurve;
                ConvertEcdhPrivateKey(textEcdhAlicePrivate, standard);
                ConvertEcdhPrivateKey(textEcdhBobPrivate, standard);
            }
            catch
            {
                // 下拉框选项变化时仅静默转换已存在的密钥，避免弹窗干扰操作
            }
        }

        private void ComboEcdhPublicKeyStandard_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
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

        #region 辅助方法

        /// <summary>
        /// 安全地将文本复制到剪贴板，自动重试并在失败时给出友好提示。
        /// </summary>
        private bool TrySetClipboardText(string text, string successStatus, string? emptyMessage = null)
        {
            if (string.IsNullOrEmpty(text))
            {
                if (!string.IsNullOrEmpty(emptyMessage))
                {
                    MessageBox.Show(emptyMessage, "提示", MessageBoxButton.OK, MessageBoxImage.Information);
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
                catch (System.Runtime.InteropServices.ExternalException ex)
                {
                    lastEx = ex;
                    // 剪贴板可能被其他进程临时锁定，短暂等待后重试
                    System.Threading.Thread.Sleep(50);
                }
            }

            MessageBox.Show(
                $"剪贴板操作失败: {lastEx?.Message}\n请稍后再试或关闭可能占用剪贴板的程序。",
                "复制失败",
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            SetStatus("复制失败：剪贴板被占用");
            return false;
        }

        /// <summary>
        /// 读取 ComboBox 选中项文本（兼容 ComboBoxItem 与 string 两种 Item 类型）
        /// </summary>
        private static string GetComboSelectedText(ComboBox combo)
        {
            if (combo.SelectedItem is ComboBoxItem item && item.Content != null)
                return item.Content.ToString() ?? string.Empty;
            return combo.SelectedItem?.ToString() ?? string.Empty;
        }

        /// <summary>
        /// 将标准 PEM 密钥按输出格式转为文本框显示文本
        /// </summary>
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

        /// <summary>
        /// 用 Base64 内容拼装 PEM 外壳
        /// </summary>
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
    }
}
