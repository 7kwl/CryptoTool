using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Threading;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;

namespace WpfApp1.ECDSA.EcdsaTopPanel
{
    /// <summary>
    /// ECDSA 主页逻辑（由 CryptoTool.Win\ECDSA\EcdsaTabControl.cs 移植）
    /// 密钥生成 / 验证 / 提取 / 曲线检测 / 格式与存储标准转换 / 导入导出
    /// </summary>
    public partial class EcdsaTopPanel : UserControl
    {
        /// <summary>状态栏消息通知（供宿主订阅，可选）</summary>
        public event Action<string>? StatusChanged;

        /// <summary>缓存的标准 PEM 私钥（文本框可能显示为 Base64/Hex）</summary>
        private string _privateKeyPem = string.Empty;

        /// <summary>缓存的标准 PEM 公钥（文本框可能显示为 Base64/Hex）</summary>
        private string _publicKeyPem = string.Empty;

        /// <summary>返回当前标准 PEM 私钥（供子页面使用，可能为空）</summary>
        public string GetCurrentPrivateKeyPem() => _privateKeyPem ?? string.Empty;

        /// <summary>返回当前标准 PEM 公钥（供子页面使用，可能为空）</summary>
        public string GetCurrentPublicKeyPem() => _publicKeyPem ?? string.Empty;

        /// <summary>曲线分类数据：分类Key → (图标, 曲线列表)</summary>
        private Dictionary<string, (string Icon, List<KeyValuePair<string, string>> Curves)> _allCurveData = [];

        public EcdsaTopPanel()
        {
            InitializeComponent();
            WireEvents();       // 挂接所有控件事件
            InitializeDefaults(); // 初始化下拉框与结果占位文本
        }

        #region 初始化默认值

        /// <summary>
        /// 初始化页面默认状态：曲线级联、存储标准、输出格式、占位结果
        /// </summary>
        private void InitializeDefaults()
        {
            InitializeEcdsaCurveAndStandardControls();

            // 输出格式默认 PEM
            comboOutputFormat.SelectedIndex = 0;

            radioPrivateKey.IsChecked = true;

            ResetValidationResult("未验证", Brushes.Gray);
            SetKeyResultPlaceholder();
        }

        /// <summary>
        /// 计算结果框写入灰色占位文本
        /// </summary>
        private void SetKeyResultPlaceholder()
        {
            textKeyResult.Document.Blocks.Clear();
            var p = new Paragraph(new Run("从私钥提取/曲线检测：\n等待操作..."))
            {
                Foreground = Brushes.Gray,
                Margin = new Thickness(0)
            };
            textKeyResult.Document.Blocks.Add(p);
        }

        #endregion

        #region 事件挂接

        /// <summary>
        /// 在代码中集中挂接控件事件（XAML 保持纯布局排版）
        /// </summary>
        private void WireEvents()
        {
            btnGenerateKeyPair.Click += BtnGenerateKeyPair_Click;
            btnValidateKeyPair.Click += BtnValidateKeyPair_Click;
            btnGetPublicKeyFromPrivate.Click += BtnGetPublicKeyFromPrivate_Click;
            btnGetCurveType.Click += BtnGetCurveType_Click;
            btnClearAll.Click += BtnClearAll_Click;
            btnConvertKey.Click += BtnConvertKey_Click;
            btnConvertPrivateKeyStandard.Click += BtnConvertPrivateKeyStandard_Click;
            btnConvertPublicKeyStandard.Click += BtnConvertPublicKeyStandard_Click;

            btnCopyPrivateKey.Click += BtnCopyPrivateKey_Click;
            btnPastePrivateKey.Click += BtnPastePrivateKey_Click;
            btnImportPrivateKey.Click += BtnImportPrivateKey_Click;
            btnSavePrivateKey.Click += BtnSavePrivateKey_Click;
            btnClearPrivateKey.Click += BtnClearPrivateKey_Click;

            btnCopyPublicKey.Click += BtnCopyPublicKey_Click;
            btnPastePublicKey.Click += BtnPastePublicKey_Click;
            btnImportPublicKey.Click += BtnImportPublicKey_Click;
            btnSavePublicKey.Click += BtnSavePublicKey_Click;
            btnClearPublicKey.Click += BtnClearPublicKey_Click;

            comboOutputFormat.SelectionChanged += ComboOutputFormat_SelectedIndexChanged;
            comboPrivateKeyStandard.SelectionChanged += ComboPrivateKeyStandard_SelectedIndexChanged;
            comboPublicKeyStandard.SelectionChanged += ComboPublicKeyStandard_SelectedIndexChanged;
            comboCategory.SelectionChanged += ComboCategory_SelectedIndexChanged;

            // 下拉框支持滚轮直接切换选中项（复刻 WinForms ComboBox 行为）
            AttachComboBoxWheel(comboOutputFormat);
            AttachComboBoxWheel(comboPrivateKeyStandard);
            AttachComboBoxWheel(comboPublicKeyStandard);
            AttachComboBoxWheel(comboCategory);
AttachComboBoxWheel(comboCurve);

            // ============== 中列图标：复用相邻按钮的逻辑 ==============

            // ---- 私钥图标 ----
            imgCopyPrivateKey.MouseLeftButtonDown += (s, e) => BtnCopyPrivateKey_Click(s!, e!);
            imgPastePrivateKey.MouseLeftButtonDown += (s, e) => BtnPastePrivateKey_Click(s!, e!);
            imgClearPrivateKey.MouseLeftButtonDown += (s, e) => BtnClearPrivateKey_Click(s!, e!);
            imgImportPrivateKey.MouseLeftButtonDown += (s, e) => BtnImportPrivateKey_Click(s!, e!);
            imgExportPrivateKey.MouseLeftButtonDown += (s, e) => BtnSavePrivateKey_Click(s!, e!);

            // ---- 公钥图标 ----
            imgCopyPublicKey.MouseLeftButtonDown += (s, e) => BtnCopyPublicKey_Click(s!, e!);
            imgPastePublicKey.MouseLeftButtonDown += (s, e) => BtnPastePublicKey_Click(s!, e!);
            imgClearPublicKey.MouseLeftButtonDown += (s, e) => BtnClearPublicKey_Click(s!, e!);
            imgImportPublicKey.MouseLeftButtonDown += (s, e) => BtnImportPublicKey_Click(s!, e!);
            imgExportPublicKey.MouseLeftButtonDown += (s, e) => BtnSavePublicKey_Click(s!, e!);

            // ---- 悬停提示（Popup + 250ms 延迟关闭，规避 ToolTip 吞点击 / Popup 闪烁） ----
            // 私钥
            SetIconToolTip(imgCopyPrivateKey, "复制私钥");
            SetIconToolTip(imgPastePrivateKey, "粘贴私钥");
            SetIconToolTip(imgClearPrivateKey, "清空私钥");
            SetIconToolTip(imgImportPrivateKey, "导入私钥");
            SetIconToolTip(imgExportPrivateKey, "导出私钥（保存到文件）");
            // 公钥
            SetIconToolTip(imgCopyPublicKey, "复制公钥");
            SetIconToolTip(imgPastePublicKey, "粘贴公钥");
            SetIconToolTip(imgClearPublicKey, "清空公钥");
            SetIconToolTip(imgImportPublicKey, "导入公钥");
            SetIconToolTip(imgExportPublicKey, "导出公钥（保存到文件）");
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

        #endregion

        #region 曲线级联下拉框（分类 → 曲线）

        /// <summary>
        /// 初始化曲线分类级联下拉框，并确保存储标准下拉框有默认选中项
        /// </summary>
        private void InitializeEcdsaCurveAndStandardControls()
        {
            _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

            comboCategory.DisplayMemberPath = "Text";
            comboCategory.SelectedValuePath = "Value";
            comboCategory.Items.Clear();
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

            // 存储标准与曲线下拉框：XAML 已定义选项，这里仅兜底设置默认选中
            if (comboPrivateKeyStandard.SelectedIndex < 0) comboPrivateKeyStandard.SelectedIndex = 0;
            if (comboPublicKeyStandard.SelectedIndex < 0) comboPublicKeyStandard.SelectedIndex = 0;
        }

        /// <summary>
        /// 分类切换时联动刷新曲线列表
        /// </summary>
        private void ComboCategory_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
        {
            if (comboCategory.SelectedItem is not { } selected)
                return;

            // 匿名对象 { Text, Value }，用反射/动态取 Value
            dynamic d = selected;
            string categoryKey = (string)d.Value;

            if (!_allCurveData.TryGetValue(categoryKey, out var categoryData))
                return;

            comboCurve.Items.Clear();
            foreach (var c in categoryData.Curves)
                comboCurve.Items.Add(c);

            if (comboCurve.Items.Count > 0)
                comboCurve.SelectedIndex = 0;
        }

        /// <summary>
        /// 获取当前选中的曲线 Key（缺省 prime256v1）
        /// </summary>
        private string GetSelectedCurve()
        {
            if (comboCurve.SelectedItem is KeyValuePair<string, string> sel && !string.IsNullOrEmpty(sel.Key))
                return sel.Key;
            return "prime256v1";
        }

        #endregion

        #region 密钥生成

        /// <summary>
        /// 生成密钥对：按选中曲线生成，并同步缓存与显示
        /// </summary>
        private void BtnGenerateKeyPair_Click(object? sender, RoutedEventArgs e)
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

        /// <summary>
        /// 从文本框中的私钥提取公钥并显示
        /// </summary>
        private void BtnGetPublicKeyFromPrivate_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPrivateKey.Text))
                {
                    AppendKeyResult("❌ 私钥为空，无法提取公钥", Brushes.Red);
                    SetStatus("私钥为空，无法提取公钥");
                    return;
                }

                SetStatus("正在从私钥提取公钥...");

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textPrivateKey.Text.Trim(), true));

                // 提取公钥前先判断私钥曲线是否可识别
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                if (curveName == "未知曲线")
                {
                    AppendKeyResult("❌ 私钥生成逻辑非法，不符合标准 OpenSSL", Brushes.Red);
                    SetStatus("私钥曲线无法识别，不符合标准 OpenSSL");
                    return;
                }

                var pub = EcdsaAlgorithm.GetPublicKey(priv);

                _privateKeyPem = ExportPrivateKeyByStandard(priv);
                _publicKeyPem = ExportPublicKeyByStandard(pub);

                RefreshKeyDisplay();
                AppendKeyResult("✅ 密钥对匹配（从私钥提取）", Brushes.Green, GetSelectedCurve());
                SetStatus("从私钥提取公钥完成");
            }
            catch (Exception ex)
            {
                AppendKeyResult($"❌ 提取公钥失败: {ex.Message}", Brushes.Red, GetSelectedCurve());
                SetStatus("提取公钥失败");
            }
        }

        #endregion

        #region 获取私钥曲线类型

        /// <summary>
        /// 检测私钥使用的曲线类型
        /// </summary>
        private void BtnGetCurveType_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textPrivateKey.Text))
                {
                    AppendKeyResult("❌ 私钥为空，无法获取曲线类型", Brushes.Red);
                    SetStatus("获取曲线类型失败：私钥为空");
                    return;
                }

                SetStatus("正在检测私钥曲线类型...");

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(ConvertDisplayToPem(textPrivateKey.Text.Trim(), true));
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                string displayName = EcdsaCurveNames.GetDisplayName(curveName);

                AppendKeyResult($"曲线名称: {curveName}\n显示名称: {displayName}", Brushes.Green);
                SetStatus("获取曲线类型完成 - " + displayName);
            }
            catch (Exception ex)
            {
                AppendKeyResult($"❌ 获取曲线类型失败: {ex.Message}", Brushes.Red);
                SetStatus("获取曲线类型失败");
            }
        }

        #endregion

        #region 密钥对验证

        /// <summary>
        /// 验证私钥/公钥是否匹配：用固定 SHA-256 做签名探针验证
        /// </summary>
        private void BtnValidateKeyPair_Click(object? sender, RoutedEventArgs e)
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

                const string testHash = "SHA-256";
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

        /// <summary>
        /// UI 哈希算法名 → BouncyCastle Signer 算法名
        /// </summary>
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

        #endregion

        #region 格式转换

        /// <summary>
        /// 密钥格式转换（PEM / Base64 / Hex），作用于当前选中的密钥类型（私钥/公钥）
        /// </summary>
        private void BtnConvertKey_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                TextBox targetBox = radioPrivateKey.IsChecked == true ? textPrivateKey : textPublicKey;
                if (string.IsNullOrWhiteSpace(targetBox.Text))
                {
                    MessageBox.Show("请先输入要转换的密钥！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                string input = targetBox.Text.Trim();
                string inputFormat = DetectKeyFormat(input);
                string tf = GetComboSelectedText(comboOutputFormat);
                if (inputFormat == tf)
                {
                    MessageBox.Show($"输入格式和输出格式相同（{inputFormat}），无需转换！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                targetBox.Text = FormatConversionHelper.ConvertStringFormat(
                    input,
                    FormatConversionHelper.ParseInputFormat(inputFormat),
                    FormatConversionHelper.ParseOutputFormat(tf));

                if (radioPrivateKey.IsChecked == true) _privateKeyPem = targetBox.Text;
                else _publicKeyPem = targetBox.Text;

                AppendValidationResult("格式已转换", Brushes.Green);
                SetStatus($"格式转换完成 - {inputFormat} → {tf}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"格式转换失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                SetStatus("格式转换失败");
            }
        }

        /// <summary>
        /// 探测输入文本的密钥格式（PEM / Base64 / Hex）
        /// </summary>
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

        #region 导入/保存/粘贴/清空/复制

        private void BtnImportPrivateKey_Click(object? sender, RoutedEventArgs e) => ImportKey(true);
        private void BtnImportPublicKey_Click(object? sender, RoutedEventArgs e) => ImportKey(false);

        /// <summary>
        /// 从文件导入密钥（私钥/公钥），导入后缓存标准 PEM 并按输出格式显示
        /// </summary>
        private void ImportKey(bool isPrivate)
        {
            try
            {
                var d = new Microsoft.Win32.OpenFileDialog
                {
                    Title = isPrivate ? "导入私钥文件" : "导入公钥文件",
                    Filter = "密钥文件|*.pem;*.key;*.txt|所有文件|*.*"
                };
                if (d.ShowDialog() != true) return;

                string c = File.ReadAllText(d.FileName).Trim();
                string pem = ConvertDisplayToPem(c, isPrivate);

                UIOutputFormat fmt = GetCurrentOutputFormat();
                if (isPrivate)
                {
                    EcdsaKeyHelper.ImportPrivateKeyPem(pem); // 校验合法性
                    var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                    _privateKeyPem = ExportPrivateKeyByStandard(priv);
                    textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, fmt);
                    AppendHistory($"导入私钥成功 → {Path.GetFileName(d.FileName)}");
                }
                else
                {
                    EcdsaKeyHelper.ImportPublicKeyPem(pem); // 校验合法性
                    var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
                    _publicKeyPem = ExportPublicKeyByStandard(pub);
                    textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, fmt);
                    AppendHistory($"导入公钥成功 → {Path.GetFileName(d.FileName)}");
                }

                AppendValidationResult("新密钥已导入", Brushes.Green);
                SetStatus($"{(isPrivate ? "私钥" : "公钥")}导入完成");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"导入失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void BtnSavePrivateKey_Click(object? sender, RoutedEventArgs e) => SaveKeyToFile(_privateKeyPem, "私钥", "private");
        private void BtnSavePublicKey_Click(object? sender, RoutedEventArgs e) => SaveKeyToFile(_publicKeyPem, "公钥", "public");

        /// <summary>
        /// 将缓存的标准 PEM 密钥保存到文件
        /// </summary>
        private void SaveKeyToFile(string keyContent, string keyTypeName, string fileSuffix)
        {
            if (string.IsNullOrEmpty(keyContent))
            {
                MessageBox.Show($"没有可保存的{keyTypeName}！", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var d = new Microsoft.Win32.SaveFileDialog
            {
                Title = $"保存{keyTypeName}",
                Filter = "PEM文件|*.pem|文本文件|*.txt",
                FileName = $"ecdsa_{fileSuffix}_key.pem"
            };
            if (d.ShowDialog() == true)
            {
                File.WriteAllText(d.FileName, keyContent, Encoding.UTF8);
                SetStatus($"{keyTypeName}保存成功");
                AppendHistory($"保存{keyTypeName}成功 → {Path.GetFileName(d.FileName)}");
            }
        }

        private void BtnClearAll_Click(object? sender, RoutedEventArgs e)
        {
            textPrivateKey.Clear();
            textPublicKey.Clear();
            _privateKeyPem = string.Empty;
            _publicKeyPem = string.Empty;
            ResetValidationResult("未验证", Brushes.Gray);
            SetKeyResultPlaceholder();
            SetStatus("已清空所有内容");
            AppendHistory("清空全部");
        }

        private void BtnPastePrivateKey_Click(object? sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                MessageBox.Show("剪贴板中没有文本内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string c = Clipboard.GetText().Trim();
            try
            {
                string pem = ConvertDisplayToPem(c, true);
                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                _privateKeyPem = ExportPrivateKeyByStandard(priv);
                textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, GetCurrentOutputFormat());
                SetStatus("私钥已从剪贴板粘贴");
                AppendHistory("粘贴私钥成功");
            }
            catch
            {
                _privateKeyPem = c;
                textPrivateKey.Text = c;
                SetStatus("私钥已从剪贴板粘贴");
                AppendHistory("粘贴私钥成功（原文）");
            }
        }

        private void BtnClearPrivateKey_Click(object? sender, RoutedEventArgs e)
        {
            textPrivateKey.Clear();
            _privateKeyPem = string.Empty;
            SetStatus("私钥已清空");
            AppendHistory("清空私钥");
        }

        private void BtnPastePublicKey_Click(object? sender, RoutedEventArgs e)
        {
            if (!Clipboard.ContainsText())
            {
                MessageBox.Show("剪贴板中没有文本内容！", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            string c = Clipboard.GetText().Trim();
            try
            {
                string pem = ConvertDisplayToPem(c, false);
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
                _publicKeyPem = ExportPublicKeyByStandard(pub);
                textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, GetCurrentOutputFormat());
                SetStatus("公钥已从剪贴板粘贴");
                AppendHistory("粘贴公钥成功");
            }
            catch
            {
                _publicKeyPem = c;
                textPublicKey.Text = c;
                SetStatus("公钥已从剪贴板粘贴");
                AppendHistory("粘贴公钥成功（原文）");
            }
        }

        private void BtnClearPublicKey_Click(object? sender, RoutedEventArgs e)
        {
            textPublicKey.Clear();
            _publicKeyPem = string.Empty;
            SetStatus("公钥已清空");
            AppendHistory("清空公钥");
        }

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
                MessageBoxButton.OK,
                MessageBoxImage.Error);
            SetStatus("复制失败：剪贴板被占用");
            return false;
        }

        private void BtnCopyPrivateKey_Click(object? sender, RoutedEventArgs e)
        {
            if (TrySetClipboardText(textPrivateKey.Text, "私钥已复制到剪贴板", "私钥为空，无法复制！"))
                AppendHistory("复制私钥成功");
        }

        private void BtnCopyPublicKey_Click(object? sender, RoutedEventArgs e)
        {
            if (TrySetClipboardText(textPublicKey.Text, "公钥已复制到剪贴板", "公钥为空，无法复制！"))
                AppendHistory("复制公钥成功");
        }

        #endregion

        #region 控件事件

        /// <summary>
        /// 输出格式切换时：先把文本框内容反向推导为 PEM 缓存，再按新格式刷新显示
        /// </summary>
        private void ComboOutputFormat_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
        {
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

        #endregion

        #region 辅助方法

        /// <summary>
        /// 在运行结果框追加"生成/验证"类结果（自动带曲线信息，❌ 红色，否则绿色）
        /// </summary>
        private void SetGenerateResult(string curveName, string status)
        {
            SolidColorBrush color = status.Contains('\u274C') ? Brushes.Red : Brushes.Green;
            AppendValidationResult($"{status}\n使用曲线: {EcdsaCurveNames.GetDisplayName(curveName)}", color);
        }

        /// <summary>
        /// 重置运行结果框为单行文本（如"验证结果: 未验证"）
        /// </summary>
        private void ResetValidationResult(string text, SolidColorBrush color)
        {
            labelValidationResult.Document.Blocks.Clear();
            var p = new Paragraph(new Run("验证结果: " + text))
            {
                Foreground = color,
                Margin = new Thickness(0)
            };
            labelValidationResult.Document.Blocks.Add(p);
        }

        /// <summary>
        /// 运行结果框顶部插入一条彩色日志（上面新、下面旧），首个条目保留
        /// </summary>
        /// <summary>
        /// 内部写入"运行结果"框，供子页面（ECDH/ECIES/文件签名）通过 TabHost.ResultAppender 桥接写入
        /// </summary>
        internal void AppendValidationResult(string message, SolidColorBrush color)
        {
            // 若当前是占位文本（未验证/等待操作），先清空
            string current = new TextRange(labelValidationResult.Document.ContentStart, labelValidationResult.Document.ContentEnd).Text;
            if (current.Contains("未验证") || current.Contains("等待操作"))
                labelValidationResult.Document.Blocks.Clear();

            string entry = $"运行时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}此次任务: {message}{Environment.NewLine}{Environment.NewLine}";

            var p = new Paragraph(new Run(entry))
            {
                Foreground = color,
                Margin = new Thickness(0)
            };
            // 顶部插入：与右侧结果框一致，上面新、下面旧
            var blocks = labelValidationResult.Document.Blocks;
            if (blocks.FirstBlock != null)
                blocks.InsertBefore(blocks.FirstBlock, p);
            else
                blocks.Add(p);

            labelValidationResult.ScrollToHome();
        }

        /// <summary>
        /// 计算结果框顶部插入一条彩色日志（上面新、下面旧），可附带曲线名。
        /// internal 供子页面（ECDH/ECIES）通过 TabHost.KeyResultAppender 桥接写入
        /// </summary>
        internal void AppendKeyResult(string message, SolidColorBrush color, string? curveName = null)
        {
            string current = new TextRange(textKeyResult.Document.ContentStart, textKeyResult.Document.ContentEnd).Text;
            if (current.Contains("等待操作"))
                textKeyResult.Document.Blocks.Clear();

            string detail = message;
            if (!string.IsNullOrEmpty(curveName))
            {
                detail += $"\n使用曲线: {EcdsaCurveNames.GetDisplayName(curveName)}";
            }

            string entry = $"运行时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}{Environment.NewLine}此次任务: {detail}{Environment.NewLine}{Environment.NewLine}";

            var p = new Paragraph(new Run(entry))
            {
                Foreground = color,
                Margin = new Thickness(0)
            };
            var blocks = textKeyResult.Document.Blocks;
            if (blocks.FirstBlock != null)
                blocks.InsertBefore(blocks.FirstBlock, p);
            else
                blocks.Add(p);

            textKeyResult.ScrollToHome();
        }

        /// <summary>
        /// 操作历史顶部插入一条紫色日志（上面新、下面旧），统一并入"计算结果"框。
        /// 用于记录复制/粘贴/导入/保存等关键操作的成功路径。
        /// </summary>
        private void AppendHistory(string message)
        {
            string line = $"[{DateTime.Now:HH:mm:ss}] {message}";

            // 如果当前还停留在"等待操作..."灰色占位，先清掉它
            string current = new TextRange(textKeyResult.Document.ContentStart, textKeyResult.Document.ContentEnd).Text;
            if (current.Contains("等待操作"))
                textKeyResult.Document.Blocks.Clear();

            var p = new Paragraph(new Run(line))
            {
                Foreground = Brushes.Purple,
                Margin = new Thickness(0)
            };
            var blocks = textKeyResult.Document.Blocks;
            if (blocks.FirstBlock != null)
                blocks.InsertBefore(blocks.FirstBlock, p);
            else
                blocks.Add(p);

            textKeyResult.ScrollToHome();
        }

        /// <summary>
        /// 根据当前输出格式刷新两个文本框显示（缓存始终是标准 PEM）
        /// </summary>
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
        /// 当前输出格式枚举
        /// </summary>
        private UIOutputFormat GetCurrentOutputFormat() => GetComboSelectedText(comboOutputFormat) switch
        {
            "Base64" => UIOutputFormat.Base64,
            "Hex大写" => UIOutputFormat.HexUpper,
            "Hex小写" => UIOutputFormat.HexLower,
            _ => UIOutputFormat.PEM
        };

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

        /// <summary>
        /// 为图标设置悬停提示：鼠标悬停时在图标右侧显示红色文字，移走自动消失。
        /// 使用 Popup + 延迟关闭实现：
        ///  - 鼠标从图标移到气泡上时不会立即关闭（延迟 250ms 内移入气泡即取消关闭），避免闪烁；
        ///  - Popup 不拦截图标的点击事件，复制/粘贴/清除可正常触发。
        /// </summary>
        private static void SetIconToolTip(FrameworkElement icon, string text)
        {
            var popup = new Popup
            {
                PlacementTarget = icon,
                Placement = PlacementMode.Right,
                HorizontalOffset = 6,
                AllowsTransparency = true,
                StaysOpen = true,
                IsOpen = false
            };
            popup.Child = new Border
            {
                BorderBrush = Brushes.Red,
                BorderThickness = new Thickness(1),
                Child = new TextBlock
                {
                    Text = text,
                    Foreground = Brushes.Red,
                    Background = Brushes.White,
                    Padding = new Thickness(6, 2, 6, 2)
                }
            };

            var closeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
            closeTimer.Tick += (s, e) =>
            {
                closeTimer.Stop();
                popup.IsOpen = false;
            };

            icon.MouseEnter += (s, e) => { closeTimer.Stop(); popup.IsOpen = true; };
            icon.MouseLeave += (s, e) => { closeTimer.Stop(); closeTimer.Start(); };
            popup.MouseEnter += (s, e) => { closeTimer.Stop(); };
            popup.MouseLeave += (s, e) => { closeTimer.Stop(); closeTimer.Start(); };
            // 点击图标时立即关闭气泡，避免点击操作后残留
            icon.MouseLeftButtonDown += (s, e) => { closeTimer.Stop(); popup.IsOpen = false; };
        }
    }
}
