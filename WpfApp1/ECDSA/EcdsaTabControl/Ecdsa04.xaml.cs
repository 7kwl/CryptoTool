using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Algorithm.Utils;
using Microsoft.Win32;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using WpfApp1.IconLogic;

namespace WpfApp1.ECDSA.EcdsaTabControl
{
    /// <summary>
    /// ECDSA 文件签名/验签视图。
    /// 行业流程（Hash-then-Sign，同 OpenSSL dgst）：对文件内容流式计算摘要 → 私钥签名 → 公钥验签。
    /// 签名/验签所需的私钥/公钥取自顶部 ECDSA 面板（通过宿主注入的委托），或自动回退到顶部面板。
    /// 大文件采用 64KB 分块流式处理，不一次性读入内存。
    /// </summary>
    public partial class Ecdsa04 : UserControl
    {
        #region 注入

        public Func<string>? PrivateKeyProvider { get; set; }
        public Func<string>? PublicKeyProvider { get; set; }
        public Action<string, SolidColorBrush>? AppendToHost { get; set; }

        #endregion

        #region 私有状态

        /// <summary>待签名文件路径</summary>
        private string? _signFilePath;

        /// <summary>流式处理分块大小（64KB）</summary>
        private const int BufferSize = 64 * 1024;

        /// <summary>最近一次签名得到的原始 DER 字节，用于导出 .sig</summary>
        private byte[]? _lastSignatureBytes;

        /// <summary>左侧“文件验签”行所选原文件路径</summary>
        private string? _leftVerifyFilePath;

        #endregion

        public Ecdsa04()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                // 默认选中：Hash=SHA-256，签名格式=Base64，k=混合熵
                if (comboHashAlgorithm != null && comboHashAlgorithm.Items.Count > 0 && comboHashAlgorithm.SelectedIndex < 0)
                    comboHashAlgorithm.SelectedIndex = 1; // SHA-256
                if (comboSignatureFormat != null && comboSignatureFormat.Items.Count > 0 && comboSignatureFormat.SelectedIndex < 0)
                    comboSignatureFormat.SelectedIndex = 0; // Base64
                if (comboKGeneration != null && comboKGeneration.Items.Count > 0 && comboKGeneration.SelectedIndex < 0)
                    comboKGeneration.SelectedIndex = 0; // 混合熵随机 k

                // 自动从顶部 ECDSA 面板拿密钥（如果尚未由宿主注入）。
                // 注意：EcdsaTopPanel 与 EcdsaTabPage 是兄弟节点，FindAncestor 走不到，
                // 密钥委托由 EcdsaTabPage 在 ShowSubPage 阶段直接传入。
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
                btnSelectSignFile.Click += (_, _) => SelectSignFile();
                btnSignFile.Click += async (_, _) => await SignFileAsync();
                btnSelectVerifyFileForHash.Click += (_, _) => SelectVerifyFileForHash();
                btnSelectSigFileForInfo.Click += (_, _) => SelectSigFileForInfo();
                btnVerifyHashAndSig.Click += async (_, _) => await VerifyLeftHashAndSigAsync();
                btnExportSignature.Click += (_, _) => ExportSignatureFile();

                // ===== 图标按钮 =====
                imgCopyVerifyHash.MouseLeftButtonDown += (s, e) => TryCopy(textVerifyHash.Text, "文件验签");
                imgExportVerifyHash.MouseLeftButtonDown += (s, e) => ExportHashFile();
                imgCopyVerifySigInfo.MouseLeftButtonDown += (s, e) => TryCopy(textVerifySigInfo.Text, "签名信息");
                imgCopyVerifyDetail.MouseLeftButtonDown += (s, e) => TryCopy(textVerifyDetail.Text, "验签详情");

                IconToolTipHelper.SetIconToolTip(imgCopyVerifyHash, "复制文件验签信息");
                IconToolTipHelper.SetIconToolTip(imgExportVerifyHash, "导出文件验签信息");
                IconToolTipHelper.SetIconToolTip(imgCopyVerifySigInfo, "复制签名信息");
                IconToolTipHelper.SetIconToolTip(imgCopyVerifyDetail, "复制验签详情");
                IconToolTipHelper.SetIconToolTip(btnVerifyHashAndSig, "用顶部公钥和右侧 Hash 算法验签左侧文件");
                IconToolTipHelper.SetIconToolTip(btnExportSignature, "将下方签名结果导出为 .sig 文件");
                IconToolTipHelper.SetIconToolTip(btnIndustryFlow, "行业流程说明");

                btnIndustryFlow.MouseEnter += (_, _) => textIndustryFlow.Foreground = Brushes.Black;
                btnIndustryFlow.MouseLeave += (_, _) => textIndustryFlow.Foreground = Brushes.Transparent;
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

        #endregion

        #region 签名流程

        /// <summary>选择待签名文件并把文件信息显示到第 0 行文本框</summary>
        private void SelectSignFile()
        {
            var dlg = new OpenFileDialog { Title = "选择要签名的文件", Filter = "所有文件|*.*" };
            if (dlg.ShowDialog() != true) return;

            _signFilePath = dlg.FileName;
            textVerifyResult.Text = DescribeFile(_signFilePath);
            LogInfo($"已选择待签名文件：{_signFilePath}");
        }

        /// <summary>对选中的文件生成 ECDSA 签名，并将签名结果输出到第 4 行（验签详情）文本框</summary>
        private async Task SignFileAsync()
        {
            if (string.IsNullOrEmpty(_signFilePath) || !File.Exists(_signFilePath))
            {
                LogInfo("⚠️ 请先选择要签名的文件。");
                return;
            }
            string privPem = PrivateKeyProvider?.Invoke() ?? "";
            if (string.IsNullOrEmpty(privPem))
            {
                LogInfo("⚠️ 请先在顶部 ECDSA 面板生成或导入私钥。");
                return;
            }

            string hashAlg = GetSelectedHash();
            string kMode = GetSelectedKMode();
            btnSignFile.IsEnabled = false;
            try
            {
                LogInfo($"正在对文件签名：{Path.GetFileName(_signFilePath)}（{hashAlg} / {kMode}）...");
                byte[] sig = await Task.Run(() =>
                {
                    var priv = EcdsaKeyHelper.ImportPrivateKeyPem(privPem);
                    ISigner signer = EcdsaKGenerator.CreateSigner(kMode, hashAlg);
                    signer.Init(true, priv);
                    StreamThrough(signer.BlockUpdate, _signFilePath!);
                    return signer.GenerateSignature();
                });

                _lastSignatureBytes = sig;
                textVerifyDetail.Text = FormatSignatureForDisplay(sig);
                LogInfo($"✅ 文件签名完成（{sig.Length} 字节 DER），结果已显示在下方文本框。");
            }
            catch (Exception ex)
            {
                textVerifyDetail.Text = $"签名失败：{ex.Message}";
                LogInfo($"❌ 文件签名失败：{ex.Message}");
            }
            finally
            {
                btnSignFile.IsEnabled = true;
            }
        }

        /// <summary>当前所选 k 生成方式（前缀匹配 EcdsaKGenerator.CreateSigner）</summary>
        private string GetSelectedKMode() => GetComboText(comboKGeneration) ?? "混合熵随机 k（默认）";

        /// <summary>按当前签名格式将 DER 签名格式化为显示文本</summary>
        private string FormatSignatureForDisplay(byte[] sig)
        {
            string fmt = GetSelectedSigFormat();
            return fmt == "Hex"
                ? StringUtil.WrapTextEvery(Convert.ToHexString(sig).ToUpperInvariant(), 50)
                : fmt == "DER（二进制）"
                    ? Convert.ToHexString(sig).ToLowerInvariant()
                    : StringUtil.WrapTextEvery(Convert.ToBase64String(sig), 50);
        }

        /// <summary>生成文件信息描述文本</summary>
        private static string DescribeFile(string path)
        {
            var fi = new FileInfo(path);
            return string.Join("\n",
                $"文件名：{fi.Name}",
                $"路径：{fi.FullName}",
                $"大小：{FormatSize(fi.Length)}（{fi.Length:N0} 字节）",
                $"修改：{fi.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
        }

        /// <summary>人类可读文件大小</summary>
        private static string FormatSize(long bytes)
        {
            string[] units = ["B", "KB", "MB", "GB"];
            double size = bytes;
            int u = 0;
            while (size >= 1024 && u < units.Length - 1)
            {
                size /= 1024;
                u++;
            }
            return $"{size:0.##} {units[u]}";
        }

        #endregion

        #region 验签流程

        /// <summary>选择文件并显示文件信息到“文件验签”文本框</summary>
        private void SelectVerifyFileForHash()
        {
            var dlg = new OpenFileDialog { Title = "选择验签文件", Filter = "所有文件|*.*" };
            if (dlg.ShowDialog() != true) return;

            _leftVerifyFilePath = dlg.FileName;
            var fi = new FileInfo(dlg.FileName);
            textVerifyHash.Text = string.Join("\n",
                $"文件名：{fi.Name}",
                $"路径：{fi.FullName}",
                $"大小：{fi.Length:N0} 字节",
                $"修改：{fi.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
            LogInfo($"已选择验签文件并显示信息：{dlg.FileName}");
        }

        /// <summary>选择签名文件并显示内容到“签名信息”文本框</summary>
        private void SelectSigFileForInfo()
        {
            var dlg = new OpenFileDialog { Title = "选择签名文件", Filter = "签名文件 (*.sig)|*.sig;*.txt|所有文件|*.*" };
            if (dlg.ShowDialog() != true) return;

            try
            {
                string content = File.ReadAllText(dlg.FileName, new UTF8Encoding(false));
                textVerifySigInfo.Text = content.Length > 600
                    ? content[..600] + "..."
                    : content;
                LogInfo($"已选择签名文件并显示内容：{dlg.FileName}");
            }
            catch (Exception ex)
            {
                textVerifySigInfo.Text = $"读取签名文件失败：{ex.Message}";
                LogInfo($"⚠️ 读取签名文件失败：{ex.Message}");
            }
        }

        /// <summary>使用左侧“文件验签”与“签名信息”行所选文件，基于顶部公钥和右侧 Hash 算法执行验签</summary>
        private async Task VerifyLeftHashAndSigAsync()
        {
            if (string.IsNullOrEmpty(_leftVerifyFilePath) || !File.Exists(_leftVerifyFilePath))
            {
                LogInfo("⚠️ 请先在左侧“文件验签”行选择原始文件。");
                return;
            }
            string sigText = textVerifySigInfo.Text?.Trim() ?? "";
            if (string.IsNullOrEmpty(sigText))
            {
                LogInfo("⚠️ 请先在左侧“签名信息”框中粘贴或选择签名内容。");
                return;
            }
            string pubPem = PublicKeyProvider?.Invoke() ?? "";
            if (string.IsNullOrEmpty(pubPem))
            {
                LogInfo("⚠️ 请先在顶部 ECDSA 面板生成或导入公钥。");
                return;
            }

            string hashAlg = GetSelectedHash();
            btnVerifyHashAndSig.IsEnabled = false;
            try
            {
                LogInfo($"正在对文件验签：{Path.GetFileName(_leftVerifyFilePath)}（{hashAlg}）...");
                var (sigBytes, valid) = await Task.Run(() =>
                {
                    byte[] sig = ParseSignatureText(sigText);
                    var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);
                    ISigner verifier = SignerUtilities.GetSigner(EcdsaKGenerator.GetSignerAlgorithm(hashAlg));
                    verifier.Init(false, pub);
                    StreamThrough(verifier.BlockUpdate, _leftVerifyFilePath!);
                    return (sigBytes: sig, valid: verifier.VerifySignature(sig));
                });

                if (valid)
                {
                    SetLeftVerifyResult($"✅ 通过\n{hashAlg}\n{sigBytes.Length} 字节", Brushes.Green);
                    LogInfo($"✅ 文件签名验证通过（{hashAlg}）。");
                }
                else
                {
                    SetLeftVerifyResult("❌ 验证失败\n请确认 Hash 一致", Brushes.Red);
                    LogInfo("❌ 文件签名验证失败。");
                }
            }
            catch (Exception ex)
            {
                SetLeftVerifyResult($"❌ 异常\n{ex.Message}", Brushes.Red);
                LogInfo($"❌ 文件验签异常：{ex.Message}");
            }
            finally
            {
                btnVerifyHashAndSig.IsEnabled = true;
            }
        }

        /// <summary>更新红色框内验签结果文本，不动任何左侧文本框</summary>
        private void SetLeftVerifyResult(string detail, SolidColorBrush? brush = null)
        {
            textLeftVerifyResult.Text = detail;
            textLeftVerifyResult.Foreground = brush ?? Brushes.Black;
        }

        #endregion

        #region 辅助

        /// <summary>当前所选 Hash 算法名</summary>
        private string GetSelectedHash() => GetComboText(comboHashAlgorithm) ?? "SHA-256";

        /// <summary>当前所选签名格式</summary>
        private string GetSelectedSigFormat() => GetComboText(comboSignatureFormat) ?? "Base64";

        private static string? GetComboText(ComboBox? combo)
            => combo?.SelectedItem is ComboBoxItem item ? item.Content?.ToString() : combo?.SelectedItem?.ToString();

        /// <summary>将文件按 64KB 分块喂给回调（用于签名器/验签器 BlockUpdate）</summary>
        private static void StreamThrough(Action<byte[], int, int> blockUpdate, string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);
            byte[] buf = new byte[BufferSize];
            int n;
            while ((n = fs.Read(buf, 0, buf.Length)) > 0)
                blockUpdate(buf, 0, n);
        }

        /// <summary>从签名字符串自动识别编码并转换为原始 DER 字节：Hex / Base64 / 原始 DER 二进制</summary>
        private static byte[] ParseSignatureText(string text)
        {
            byte[] raw = Encoding.UTF8.GetBytes(text.Trim().Trim('\uFEFF'));
            if (raw.Length == 0) throw new InvalidDataException("签名为空");
            return ParseSignatureBytes(raw);
        }

        private static byte[] ParseSignatureBytes(byte[] raw)
        {
            string text = Encoding.UTF8.GetString(raw).Trim().Trim('\uFEFF');
            string normalized = new([.. text.Where(c => !char.IsWhiteSpace(c))]);
            if (normalized.Length == 0) throw new InvalidDataException("签名为空");

            // Hex：偶数长度且全为十六进制字符
            if (normalized.Length % 2 == 0 && normalized.All(Uri.IsHexDigit))
                return Convert.FromHexString(normalized);

            // Base64：可解码
            try
            {
                return Convert.FromBase64String(normalized);
            }
            catch (FormatException)
            {
                // 非文本编码，按原始 DER 二进制处理
            }
            return raw;
        }

        /// <summary>将验签得到的 Hash 算法文本导出为 .txt 文件</summary>
        private void ExportHashFile()
        {
            if (string.IsNullOrWhiteSpace(textVerifyHash.Text))
            {
                LogInfo("⚠️ 尚无验签产生的 Hash 算法可导出。");
                return;
            }

            var dlg = new SaveFileDialog
            {
                Title = "保存 Hash 算法",
                Filter = "文本文件 (*.txt)|*.txt|所有文件|*.*",
                FileName = "verify-hash.txt"
            };
            if (dlg.ShowDialog() != true) return;

            try
            {
                File.WriteAllText(dlg.FileName, textVerifyHash.Text, new UTF8Encoding(false));
                LogInfo($"✅ Hash 算法已保存：{dlg.FileName}");
            }
            catch (Exception ex)
            {
                LogInfo($"❌ 保存 Hash 算法失败：{ex.Message}");
            }
        }

        /// <summary>将最近一次签名结果按当前格式导出为 .sig 文件</summary>
        private void ExportSignatureFile()
        {
            if (_lastSignatureBytes == null || _lastSignatureBytes.Length == 0)
            {
                LogInfo("⚠️ 尚未生成签名，请先点击“开始签名”。");
                return;
            }

            string defaultName = string.IsNullOrEmpty(_signFilePath)
                ? "signature.sig"
                : $"{Path.GetFileNameWithoutExtension(_signFilePath)}.sig";

            var dlg = new SaveFileDialog
            {
                Title = "导出签名文件",
                Filter = "签名文件 (*.sig)|*.sig|所有文件|*.*",
                FileName = defaultName
            };
            if (dlg.ShowDialog() != true) return;

            try
            {
                string fmt = GetSelectedSigFormat();
                if (fmt == "DER（二进制）")
                {
                    File.WriteAllBytes(dlg.FileName, _lastSignatureBytes);
                }
                else if (fmt == "Hex")
                {
                    File.WriteAllText(dlg.FileName,
                        Convert.ToHexString(_lastSignatureBytes).ToLowerInvariant(),
                        new UTF8Encoding(false));
                }
                else // Base64
                {
                    File.WriteAllText(dlg.FileName,
                        Convert.ToBase64String(_lastSignatureBytes),
                        new UTF8Encoding(false));
                }
                LogInfo($"✅ 签名已导出（{fmt}）：{dlg.FileName}");
            }
            catch (Exception ex)
            {
                LogInfo($"❌ 导出签名文件失败：{ex.Message}");
            }
        }

        /// <summary>复制文本到剪贴板</summary>
        private void TryCopy(string text, string label)
        {
            if (string.IsNullOrWhiteSpace(text)) return;
            try
            {
                Clipboard.SetText(text);
                LogInfo($"📋 {label} 已复制到剪贴板");
            }
            catch
            {
                // 剪贴板被占用时静默失败
            }
        }

        #endregion
    }
}
