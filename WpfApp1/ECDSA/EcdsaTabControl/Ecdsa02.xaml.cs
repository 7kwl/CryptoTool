using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;

namespace WpfApp1.ECDSA.EcdsaTabControl
{
    /// <summary>
    /// ECDSA 签名验签视图。
    /// 业务逻辑 1:1 复刻自 CryptoTool.Win 的 EcdsaTabControl（ECDSA02 签名/验签功能区）。
    /// 签名/验签所需的私钥/公钥取自顶部 ECDSA 面板（通过宿主注入的委托），或自动回退到顶部面板。
    /// </summary>
    public partial class Ecdsa02 : UserControl
    {
        #region 注入

        public Func<string>? PrivateKeyProvider { get; set; }
        public Func<string>? PublicKeyProvider { get; set; }
        public Action<string, SolidColorBrush>? AppendToHost { get; set; }

        #endregion

        public Ecdsa02()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                // 默认选中：Hash=SHA-256，签名格式=Base64
                if (comboHashAlgorithm != null && comboHashAlgorithm.Items.Count > 0 && comboHashAlgorithm.SelectedIndex < 0)
                    comboHashAlgorithm.SelectedIndex = 1; // SHA-256
                if (comboSignatureFormat != null && comboSignatureFormat.Items.Count > 0 && comboSignatureFormat.SelectedIndex < 0)
                    comboSignatureFormat.SelectedIndex = 0; // Base64

                // 自动从顶部 ECDSA 面板拿密钥（如果尚未由宿主注入）。
                // 注意：EcdsaTopPanel 与 EcdsaTabPage 是兄弟节点，FindAncestor 走不到，
                // 所以这里只在 AppendToHost 没注入时做"日志桥接"检查，密钥委托由 EcdsaTabPage 在 ShowSubPage 阶段直接传入。
                if (PrivateKeyProvider == null || PublicKeyProvider == null)
                {
                    var top = FindAncestor<EcdsaTopPanel.EcdsaTopPanel>(this);
                    if (top != null)
                    {
                        PrivateKeyProvider ??= top.GetCurrentPrivateKeyPem;
                        PublicKeyProvider ??= top.GetCurrentPublicKeyPem;
                        var hasPriv = !string.IsNullOrEmpty(top.GetCurrentPrivateKeyPem());
                        var hasPub = !string.IsNullOrEmpty(top.GetCurrentPublicKeyPem());
                        LogInfo($"已自动绑定顶部 ECDSA 面板：私钥{(hasPriv ? "OK" : "空")}，公钥{(hasPub ? "OK" : "空")}");
                    }
                    else
                    {
                        LogInfo("未发现顶部 ECDSA 面板，请通过 PrivateKeyProvider/PublicKeyProvider 属性注入密钥。");
                    }
                }

                // ===== 按钮点击事件 =====
                btnSign.Click += BtnSign_Click;
                btnVerify.Click += BtnVerify_Click;
                btnCopySignature.Click += BtnCopySignature_Click;

                // 中间"复制/粘贴/清空"图标按钮
                imgCopyPlainData.MouseLeftButtonDown += (s, e) => BtnCopyPlainData_Click(s!, e!);
                imgPastePlainData.MouseLeftButtonDown += (s, e) => BtnPastePlainData_Click(s!, e!);
                imgClearPlainData.MouseLeftButtonDown += (s, e) => BtnClearPlainData_Click(s!, e!);
                imgCopySignatureData.MouseLeftButtonDown += (s, e) => BtnCopySignatureData_Click(s!, e!);
                imgPasteSignatureData.MouseLeftButtonDown += (s, e) => BtnPasteSignatureData_Click(s!, e!);
                imgClearSignatureData.MouseLeftButtonDown += (s, e) => BtnClearSignatureData_Click(s!, e!);

                SetIconToolTip(imgCopyPlainData, "复制原始数据");
                SetIconToolTip(imgPastePlainData, "粘贴原始数据");
                SetIconToolTip(imgClearPlainData, "清空原始数据");
                SetIconToolTip(imgCopySignatureData, "复制签名");
                SetIconToolTip(imgPasteSignatureData, "粘贴签名");
                SetIconToolTip(imgClearSignatureData, "清空签名");
            };
        }

        /// <summary>
        /// 沿视觉树向上查找指定类型的祖先。
        /// </summary>
        private static T? FindAncestor<T>(DependencyObject start) where T : DependencyObject
        {
            var d = start;
            while (d != null)
            {
                if (d is T match) return match;
                d = VisualTreeHelper.GetParent(d);
            }
            return null;
        }

        #region 日志

        private void Log(string msg, SolidColorBrush color) => AppendToHost?.Invoke(msg, color);
        private void LogInfo(string msg) => Log("[ECDSA] " + msg, Brushes.Black);
        private void LogOk(string msg) => Log("[ECDSA] " + msg, Brushes.Green);
        private void LogErr(string msg) => Log("[ECDSA] " + msg, Brushes.Red);

        #endregion

        #region ComboBox 取值

        private static string SelectedText(ComboBox? cb, string fallback)
        {
            if (cb == null) return fallback;
            if (cb.SelectedItem is ComboBoxItem ci)
                return ci.Content?.ToString() ?? fallback;
            return cb.SelectedValue?.ToString() ?? fallback;
        }

        private string GetSelectedHash() => SelectedText(comboHashAlgorithm, "SHA-256");

        private string GetSignatureFormat() => SelectedText(comboSignatureFormat, "Base64");

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

        #endregion

        #region 签名

        private void BtnSign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var privPem = PrivateKeyProvider?.Invoke()?.Trim() ?? string.Empty;
                var pubPem = PublicKeyProvider?.Invoke()?.Trim() ?? string.Empty;

                if (string.IsNullOrEmpty(privPem))
                { LogErr("❌ 私钥为空，无法签名"); return; }

                if (string.IsNullOrEmpty(pubPem))
                { LogErr("❌ 公钥为空，无法签名"); return; }

                if (string.IsNullOrWhiteSpace(textPlainData.Text))
                { LogErr("❌ 原始数据为空，无法签名"); return; }

                string hashAlg = GetSelectedHash();
                string signerAlg = GetSignerAlgorithm(hashAlg);

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(privPem);
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);

                // 探针验证密钥对匹配
                byte[] probe = Encoding.UTF8.GetBytes("__ECDSA_INTERNAL_PROBE__");
                ISigner ps = SignerUtilities.GetSigner(signerAlg); ps.Init(true, priv); ps.BlockUpdate(probe, 0, probe.Length);
                byte[] psig = ps.GenerateSignature();
                ISigner pv = SignerUtilities.GetSigner(signerAlg); pv.Init(false, pub); pv.BlockUpdate(probe, 0, probe.Length);
                if (!pv.VerifySignature(psig))
                { LogErr("❌ 密钥对不匹配，无法签名"); return; }

                var data = Encoding.UTF8.GetBytes(textPlainData.Text);
                ISigner s = SignerUtilities.GetSigner(signerAlg); s.Init(true, priv); s.BlockUpdate(data, 0, data.Length);
                byte[] signature = s.GenerateSignature();

                textSignature.Text = GetSignatureFormat() == "Hex"
                    ? Convert.ToHexString(signature).ToLowerInvariant()
                    : Convert.ToBase64String(signature);

                LogOk($"签名成功（{signerAlg}），签名已写入下方签名框");
            }
            catch (Exception ex)
            {
                LogErr($"❌ 签名失败: {ex.Message}");
            }
        }

        #endregion

        #region 验签

        private void BtnVerify_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var pubPem = PublicKeyProvider?.Invoke()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(pubPem))
                { LogErr("❌ 公钥为空，无法验签"); return; }

                if (string.IsNullOrWhiteSpace(textPlainData.Text) || string.IsNullOrWhiteSpace(textSignature.Text))
                { LogErr("❌ 原始数据或签名为空"); return; }

                string hashAlg = GetSelectedHash();
                string signerAlg = GetSignerAlgorithm(hashAlg);

                var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);
                byte[] sig = GetSignatureFormat() == "Hex"
                    ? Convert.FromHexString(textSignature.Text.Trim())
                    : Convert.FromBase64String(textSignature.Text.Trim());

                ISigner s = SignerUtilities.GetSigner(signerAlg);
                s.Init(false, pub);
                byte[] data = Encoding.UTF8.GetBytes(textPlainData.Text);
                s.BlockUpdate(data, 0, data.Length);

                if (s.VerifySignature(sig))
                    LogOk($"签名验证通过（{hashAlg}）");
                else
                    LogErr("❌ 签名验证失败");
            }
            catch (Exception ex)
            {
                LogErr($"❌ 验签异常: {ex.Message}");
            }
        }

        #endregion

        #region 复制 / 粘贴 / 清空

        private void BtnCopySignature_Click(object sender, RoutedEventArgs e) => TryCopy(textSignature.Text, "签名");

        private void BtnCopyPlainData_Click(object sender, RoutedEventArgs e) => TryCopy(textPlainData.Text, "原始数据");

        private void BtnPastePlainData_Click(object sender, RoutedEventArgs e) => TryPaste(textPlainData, "原始数据");

        private void BtnClearPlainData_Click(object sender, RoutedEventArgs e) => TryClear(textPlainData, "原始数据");

        private void BtnCopySignatureData_Click(object sender, RoutedEventArgs e) => TryCopy(textSignature.Text, "签名");

        private void BtnPasteSignatureData_Click(object sender, RoutedEventArgs e) => TryPaste(textSignature, "签名");

        private void BtnClearSignatureData_Click(object sender, RoutedEventArgs e) => TryClear(textSignature, "签名");

        private void TryCopy(string? text, string label)
        {
            try
            {
                text ??= string.Empty;
                if (text.Length == 0) { LogInfo($"{label}为空，无需复制"); return; }
                Clipboard.SetText(text);
                LogOk($"{label}已复制到剪贴板");
            }
            catch (Exception ex) { LogErr($"{label}复制失败: {ex.Message}"); }
        }

        private void TryPaste(TextBox? target, string label)
        {
            try
            {
                if (target == null) return;
                if (!Clipboard.ContainsText()) { LogInfo("剪贴板无文本"); return; }
                target.Text = Clipboard.GetText().Trim();
                LogOk($"{label}已粘贴");
            }
            catch (Exception ex) { LogErr($"粘贴失败: {ex.Message}"); }
        }

        private void TryClear(TextBox? target, string label)
        {
            try
            {
                if (target == null) return;
                if (string.IsNullOrEmpty(target.Text)) { LogInfo($"{label}已为空，无需清空"); return; }
                target.Clear();
                LogOk($"{label}已清空");
            }
            catch (Exception ex) { LogErr($"清空{label}失败: {ex.Message}"); }
        }

        #endregion

        /// <summary>
        /// 为图标按钮绑定红框白底悬停提示（与 Ecdsa03 保持一致风格）。
        /// - 鼠标从图标移到气泡上时不会立即关闭（延迟 250ms 内移入气泡即取消关闭），避免闪烁；
        /// - Popup 不拦截图标的点击事件，复制/粘贴可正常触发。
        /// </summary>
        private static void SetIconToolTip(Image img, string text)
        {
            var popup = new Popup
            {
                PlacementTarget = img,
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

            img.MouseEnter += (s, e) => { closeTimer.Stop(); popup.IsOpen = true; };
            img.MouseLeave += (s, e) => { closeTimer.Stop(); closeTimer.Start(); };
            popup.MouseEnter += (s, e) => { closeTimer.Stop(); };
            popup.MouseLeave += (s, e) => { closeTimer.Stop(); closeTimer.Start(); };
            img.MouseLeftButtonDown += (s, e) => { closeTimer.Stop(); popup.IsOpen = false; };
        }
    }
}
