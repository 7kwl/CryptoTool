using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
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
    /// 行业流程（Hash-then-Sign，同 OpenSSL dgst）：对文件内容流式计算摘要 → 私钥签名 → 输出 DER(.sig)；
    /// 验签用公钥校验 Hash(file) 与签名是否匹配。
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

        /// <summary>验签用原始文件路径</summary>
        private string? _verifyFilePath;

        /// <summary>验签用签名文件路径</summary>
        private string? _verifySigPath;

        /// <summary>最近一次签名 DER 原始字节（用于切换格式时重新呈现与另存）</summary>
        private byte[]? _lastSignature;

        /// <summary>流式处理分块大小（64KB）</summary>
        private const int BufferSize = 64 * 1024;

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
                btnSign.Click += async (_, _) => await SignFileAsync();
                btnSelectVerifyFile.Click += (_, _) => SelectVerifyFile();
                btnSelectVerifySig.Click += (_, _) => SelectVerifySignature();
                btnVerify.Click += async (_, _) => await VerifySignatureAsync();

                // ===== 图标按钮 =====
                imgCopyFileHash.MouseLeftButtonDown += (s, e) => TryCopy(textFileHash.Text, "文件哈希");
                imgCopySignature.MouseLeftButtonDown += (s, e) => TryCopy(textSignature.Text, "签名");
                imgClearSignature.MouseLeftButtonDown += (s, e) => { textSignature.Text = ""; _lastSignature = null; SetExportEnabled(false); };
                imgCopyVerifySig.MouseLeftButtonDown += (s, e) => TryCopy(textVerifySigPreview.Text, "签名内容");
                imgCopyVerifyResult.MouseLeftButtonDown += (s, e) => TryCopy(textVerifyResult.Text, "验签结果");

                borderImportFile.MouseLeftButtonDown += (_, _) => SelectFileForSign();
                borderExportSignature.MouseLeftButtonDown += (_, _) => SaveSignatureFile();

                // 签名格式切换时重新呈现签名
                if (comboSignatureFormat != null)
                    comboSignatureFormat.SelectionChanged += (_, _) => ReRenderSignature();

                IconToolTipHelper.SetIconToolTip(imgCopyFileHash, "复制文件哈希");
                IconToolTipHelper.SetIconToolTip(imgCopySignature, "复制签名");
                IconToolTipHelper.SetIconToolTip(imgClearSignature, "清空签名");
                IconToolTipHelper.SetIconToolTip(imgCopyVerifySig, "复制签名内容");
                IconToolTipHelper.SetIconToolTip(imgCopyVerifyResult, "复制验签结果");
                IconToolTipHelper.SetIconToolTip(borderImportFile, "选择要签名的文件");
                IconToolTipHelper.SetIconToolTip(borderExportSignature, "另存签名文件 (.sig)");

                // 初始状态：未生成签名时导出不可用
                SetExportEnabled(false);
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

        /// <summary>选择待签名文件并显示文件信息与哈希</summary>
        private async void SelectFileForSign()
        {
            var dlg = new OpenFileDialog { Title = "选择要签名的文件", Filter = "所有文件|*.*" };
            if (dlg.ShowDialog() != true) return;

            _signFilePath = dlg.FileName;
            SetExportEnabled(false);
            textSignature.Text = "（尚未生成签名）";
            _lastSignature = null;
            SetVerifyResult(null);

            try
            {
                textFileInfo.Text = DescribeFile(_signFilePath);
                LogInfo($"已选择签名文件：{_signFilePath}");
                string hash = await ComputeFileHashAsync(_signFilePath);
                textFileHash.Text = StringUtil.WrapTextEvery(hash, 50);
                LogInfo($"文件 SHA-256：{hash}");
            }
            catch (Exception ex)
            {
                textFileInfo.Text = $"读取文件信息失败：{ex.Message}";
                LogInfo($"⚠️ 读取文件信息失败：{ex.Message}");
            }
        }

        /// <summary>对选中的文件生成 ECDSA 签名（流式，不读入内存）</summary>
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
            btnSign.IsEnabled = false;
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

                _lastSignature = sig;
                ReRenderSignature();
                SetExportEnabled(true);
                LogInfo($"✅ 文件签名完成（{sig.Length} 字节 DER），可另存为 .sig 文件。");
            }
            catch (Exception ex)
            {
                LogInfo($"❌ 文件签名失败：{ex.Message}");
            }
            finally
            {
                btnSign.IsEnabled = true;
            }
        }

        /// <summary>切换导出签名图标的可用状态（禁用时置灰）</summary>
        private void SetExportEnabled(bool enabled)
        {
            double opacity = enabled ? 1.0 : 0.45;
            borderExportSignature.Opacity = opacity;
            imgExportSignature.Opacity = opacity;
        }

        /// <summary>将签名按当前格式另存为 .sig 文件</summary>
        private void SaveSignatureFile()
        {
            if (_lastSignature == null || string.IsNullOrEmpty(_signFilePath))
            {
                LogInfo("⚠️ 请先生成签名后再导出。");
                return;
            }

            var dlg = new SaveFileDialog
            {
                Title = "保存签名文件",
                Filter = "签名文件 (*.sig)|*.sig|所有文件|*.*",
                FileName = Path.GetFileNameWithoutExtension(_signFilePath) + ".sig"
            };
            if (dlg.ShowDialog() != true) return;

            try
            {
                string fmt = GetSelectedSigFormat();
                if (fmt == "DER（二进制）")
                {
                    File.WriteAllBytes(dlg.FileName, _lastSignature);
                }
                else
                {
                    string text = fmt == "Hex"
                        ? Convert.ToHexString(_lastSignature).ToUpperInvariant()
                        : Convert.ToBase64String(_lastSignature);
                    File.WriteAllText(dlg.FileName, text, new UTF8Encoding(false));
                }
                LogInfo($"✅ 签名已保存：{dlg.FileName}");
            }
            catch (Exception ex)
            {
                LogInfo($"❌ 保存签名文件失败：{ex.Message}");
            }
        }

        #endregion

        #region 验签流程

        /// <summary>选择验签用原始文件</summary>
        private void SelectVerifyFile()
        {
            var dlg = new OpenFileDialog { Title = "选择原始文件", Filter = "所有文件|*.*" };
            if (dlg.ShowDialog() != true) return;
            _verifyFilePath = dlg.FileName;
            SetVerifyResult(null);
            RefreshVerifyPaths();
            LogInfo($"已选择原始文件：{_verifyFilePath}");
        }

        /// <summary>选择验签用签名文件并预览内容</summary>
        private void SelectVerifySignature()
        {
            var dlg = new OpenFileDialog { Title = "选择签名文件", Filter = "签名文件 (*.sig)|*.sig;*.txt|所有文件|*.*" };
            if (dlg.ShowDialog() != true) return;
            _verifySigPath = dlg.FileName;
            SetVerifyResult(null);
            RefreshVerifyPaths();

            try
            {
                byte[] sig = ReadSignatureBytes(_verifySigPath);
                string fmt = GetSelectedSigFormat();
                string preview = fmt == "Base64"
                    ? Convert.ToBase64String(sig)
                    : Convert.ToHexString(sig).ToLowerInvariant();
                textVerifySigPreview.Text = StringUtil.WrapTextEvery(preview, 50);
                LogInfo($"已选择签名文件：{_verifySigPath}（{sig.Length} 字节，{fmt}）");
            }
            catch (Exception ex)
            {
                textVerifySigPreview.Text = "";
                LogInfo($"⚠️ 读取签名文件失败：{ex.Message}");
            }
        }

        /// <summary>刷新验签区路径信息文本</summary>
        private void RefreshVerifyPaths()
        {
            textVerifyPaths.Text = string.Join("\n",
                $"原文件：{_verifyFilePath ?? "（未选择）"}",
                $"签名文件：{_verifySigPath ?? "（未选择）"}");
        }

        /// <summary>验签：流式计算原文件摘要并用公钥校验签名</summary>
        private async Task VerifySignatureAsync()
        {
            if (string.IsNullOrEmpty(_verifyFilePath) || !File.Exists(_verifyFilePath))
            {
                LogInfo("⚠️ 请先选择原始文件。");
                return;
            }
            if (string.IsNullOrEmpty(_verifySigPath) || !File.Exists(_verifySigPath))
            {
                LogInfo("⚠️ 请先选择签名文件。");
                return;
            }
            string pubPem = PublicKeyProvider?.Invoke() ?? "";
            if (string.IsNullOrEmpty(pubPem))
            {
                LogInfo("⚠️ 请先在顶部 ECDSA 面板生成或导入公钥。");
                return;
            }

            string hashAlg = GetSelectedHash();
            btnVerify.IsEnabled = false;
            try
            {
                LogInfo($"正在验签：{Path.GetFileName(_verifyFilePath)}（{hashAlg}）...");
                var (sigBytes, valid) = await Task.Run(() =>
                {
                    byte[] sig = ReadSignatureBytes(_verifySigPath!);
                    var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);
                    // 验签与 k 的生成方式无关，使用标准 ECDSA 验签器即可
                    ISigner verifier = SignerUtilities.GetSigner(EcdsaKGenerator.GetSignerAlgorithm(hashAlg));
                    verifier.Init(false, pub);
                    StreamThrough(verifier.BlockUpdate, _verifyFilePath!);
                    return (sigBytes: sig, valid: verifier.VerifySignature(sig));
                });

                if (valid)
                {
                    SetVerifyResult($"✅ 签名验证通过\nHash：{hashAlg}\n签名 {sigBytes.Length} 字节",
                        Brushes.Green);
                    LogInfo($"✅ 文件签名验证通过（{hashAlg}）。");
                }
                else
                {
                    SetVerifyResult($"❌ 签名验证失败\n文件内容或签名与公钥不匹配\n（请确认 Hash/k 与签名时一致）",
                        Brushes.Red);
                    LogInfo("❌ 文件签名验证失败。");
                }
            }
            catch (Exception ex)
            {
                SetVerifyResult($"❌ 验签异常：{ex.Message}", Brushes.Red);
                LogInfo($"❌ 文件验签异常：{ex.Message}");
            }
            finally
            {
                btnVerify.IsEnabled = true;
            }
        }

        /// <summary>设置验签结果文本框内容与颜色</summary>
        private void SetVerifyResult(string? text, SolidColorBrush? brush = null)
        {
            if (text == null)
            {
                textVerifyResult.Text = "验签结果将在此显示。";
                textVerifyResult.Foreground = Brushes.Black;
                return;
            }
            textVerifyResult.Text = text;
            textVerifyResult.Foreground = brush ?? Brushes.Black;
        }

        #endregion

        #region 辅助

        /// <summary>当前所选 Hash 算法名</summary>
        private string GetSelectedHash() => GetComboText(comboHashAlgorithm) ?? "SHA-256";

        /// <summary>当前所选 k 生成方式（前缀匹配 EcdsaKGenerator.CreateSigner）</summary>
        private string GetSelectedKMode() => GetComboText(comboKGeneration) ?? "混合熵随机 k";

        /// <summary>当前所选签名格式</summary>
        private string GetSelectedSigFormat() => GetComboText(comboSignatureFormat) ?? "Base64";

        private static string? GetComboText(ComboBox? combo)
            => combo?.SelectedItem is ComboBoxItem item ? item.Content?.ToString() : combo?.SelectedItem?.ToString();

        /// <summary>按当前格式重新呈现最近一次签名</summary>
        private void ReRenderSignature()
        {
            if (_lastSignature == null) return;
            string fmt = GetSelectedSigFormat();
            textSignature.Text = StringUtil.WrapTextEvery(
                fmt == "Hex" ? Convert.ToHexString(_lastSignature).ToUpperInvariant()
                : fmt == "DER（二进制）" ? Convert.ToHexString(_lastSignature).ToLowerInvariant()
                : Convert.ToBase64String(_lastSignature), 50);
        }

        /// <summary>流式计算文件 SHA-256 哈希（.NET ComputeHash(Stream) 本身为流式）</summary>
        private static async Task<string> ComputeFileHashAsync(string path)
            => await Task.Run(() =>
            {
                using var sha = SHA256.Create();
                using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);
                return Convert.ToHexString(sha.ComputeHash(fs)).ToLowerInvariant();
            });

        /// <summary>将文件按 64KB 分块喂给回调（用于签名器/验签器 BlockUpdate）</summary>
        private static void StreamThrough(Action<byte[], int, int> blockUpdate, string path)
        {
            using var fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.Read, BufferSize);
            byte[] buf = new byte[BufferSize];
            int n;
            while ((n = fs.Read(buf, 0, buf.Length)) > 0)
                blockUpdate(buf, 0, n);
        }

        /// <summary>读取签名文件并自动识别编码：Hex / Base64 / 原始 DER 二进制</summary>
        private static byte[] ReadSignatureBytes(string path)
        {
            byte[] raw = File.ReadAllBytes(path);
            if (raw.Length == 0) throw new InvalidDataException("签名文件为空");

            string text = Encoding.UTF8.GetString(raw).Trim().Trim('\uFEFF');
            string normalized = new([.. text.Where(c => !char.IsWhiteSpace(c))]);

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

        /// <summary>生成文件信息描述文本</summary>
        private static string DescribeFile(string path)
        {
            var fi = new FileInfo(path);
            return string.Join("\n",
                $"文件名：{fi.Name}",
                $"路径：{fi.FullName}",
                $"大小：{FormatSize(fi.Length)}（{fi.Length:N0} 字节） | 修改：{fi.LastWriteTime:yyyy-MM-dd HH:mm:ss}");
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
    }
}
