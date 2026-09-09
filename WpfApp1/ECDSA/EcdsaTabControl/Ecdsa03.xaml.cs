using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Algorithm.Utils;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using WpfApp1.IconLogic;

namespace WpfApp1.ECDSA.EcdsaTabControl
{
    /// <summary>
    /// ECDHE / ECIES 加密方案视图。
    /// 业务逻辑 1:1 复刻自 CryptoTool.Win 的 EcdsaTabControl.ECDSA03。
    /// </summary>
    public partial class Ecdsa03 : UserControl
    {
        #region 注入

        public Func<string>? PrivateKeyProvider { get; set; }
        public Func<string>? PublicKeyProvider { get; set; }
        public Action<string, SolidColorBrush>? AppendToHost { get; set; }

        #endregion

        #region 状态（加密后暂存，供"密文拼接标准"使用）

        private byte[]? _lastEncCipher;       // 密文字节（不含 IV / ePub 前缀）
        private byte[]? _lastEncIV;            // IV / Nonce
        private byte[]? _lastEncTag;           // 16 字节 AuthTag（GCM / Poly1305）
        private byte[]? _lastEncKey;           // 32 字节 AES 密钥（HKDF 派生或用户填写）
        private byte[]? _lastEphemeralPubKey;  // X9.62 编码的临时公钥
        private string? _lastEphemeralCurveName;
        private string? _lastAlgorithmName;
        private string? _lastEncInfo;          // HKDF info 原文（用于"密文拼接标准"展示）

        #endregion

        public Ecdsa03()
        {
            InitializeComponent();

            Loaded += (_, _) =>
            {
                if (comboEncMode != null && comboEncMode.Items.Count > 0 && comboEncMode.SelectedIndex < 0)
                    comboEncMode.SelectedIndex = 0;
                if (comboEncInputFormat != null && comboEncInputFormat.Items.Count > 0 && comboEncInputFormat.SelectedIndex < 0)
                    comboEncInputFormat.SelectedIndex = 0;
                if (comboEncOutputFormat != null && comboEncOutputFormat.Items.Count > 0 && comboEncOutputFormat.SelectedIndex < 0)
                    comboEncOutputFormat.SelectedIndex = 0;
                if (comboEncKeyConvert != null && comboEncKeyConvert.Items.Count > 0 && comboEncKeyConvert.SelectedIndex < 0)
                    comboEncKeyConvert.SelectedIndex = 0;

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
                        LogInfo("未发现顶部 ECDSA 面板，请通过 PrivateKeyProvider/PublicKeyProvider 属性注入密钥，或在参数区手动填写 Bob 公钥。");
                    }
                }

                // ===== 图标栏点击事件（与按钮事件复用同一逻辑）=====
                // 左列：加密/解密输入输出
                imgCopyEncInput.MouseLeftButtonDown += (s, e) => BtnEncInputCopy_Click(s!, e!);
                imgPasteEncInput.MouseLeftButtonDown += (s, e) => BtnEncInputPaste_Click(s!, e!);
                imgClearEncInput.MouseLeftButtonDown += (s, e) => TryClear(textEncInput, "明文输入");
                imgCopyEncOutput.MouseLeftButtonDown += (s, e) => BtnEncOutputCopy_Click(s!, e!);
                imgPasteEncOutput.MouseLeftButtonDown += (s, e) => BtnEncOutputPaste_Click(s!, e!);
                imgClearEncOutput.MouseLeftButtonDown += (s, e) => TryClear(textEncOutput, "密文结果");
                imgCopyEncExtra.MouseLeftButtonDown += (s, e) => BtnEncExtraCopy_Click(s!, e!);
                imgPasteEncExtra.MouseLeftButtonDown += (s, e) => BtnEncExtraPaste_Click(s!, e!);
                imgClearEncExtra.MouseLeftButtonDown += (s, e) => TryClear(textEncExtra, "临时私钥ePriv");
                imgCopyEncEphemeralPub.MouseLeftButtonDown += (s, e) => BtnEncEphemeralPubCopy_Click(s!, e!);
                imgPasteEncEphemeralPub.MouseLeftButtonDown += (s, e) => BtnEncEphemeralPubPaste_Click(s!, e!);
                imgClearEncEphemeralPub.MouseLeftButtonDown += (s, e) => TryClear(textEncEphemeralPub, "临时公钥ePub");

                // 中列：参数
                imgCopyEncKey.MouseLeftButtonDown += (s, e) => TryCopy(textEncKey?.Text, "对称密钥");
                imgPasteEncKey.MouseLeftButtonDown += (s, e) => TryPaste(textEncKey);
                imgClearEncKey.MouseLeftButtonDown += (s, e) => TryClear(textEncKey, "对称密钥");
                imgCopyEncIV.MouseLeftButtonDown += (s, e) => TryCopy(textEncIV?.Text, "初始向量(IV)");
                imgPasteEncIV.MouseLeftButtonDown += (s, e) => TryPaste(textEncIV);
                imgClearEncIV.MouseLeftButtonDown += (s, e) => TryClear(textEncIV, "初始向量(IV)");
                imgCopyEncBobPublic.MouseLeftButtonDown += (s, e) => TryCopy(textEncBobPublic?.Text, "Bob 公钥");
                imgPasteEncBobPublic.MouseLeftButtonDown += (s, e) => TryPaste(textEncBobPublic);
                imgClearEncBobPublic.MouseLeftButtonDown += (s, e) => TryClear(textEncBobPublic, "Bob 公钥");
                imgCopyEncTest.MouseLeftButtonDown += (s, e) => TryCopy(textEncTest?.Text, "测试");
                imgPasteEncTest.MouseLeftButtonDown += (s, e) => TryPaste(textEncTest);
                imgClearEncTest.MouseLeftButtonDown += (s, e) => TryClear(textEncTest, "测试");

                // ===== 图标悬停提示（红框白底 Popup）=====
                IconToolTipHelper.SetIconToolTip(imgCopyEncInput, "复制明文输入");
                IconToolTipHelper.SetIconToolTip(imgPasteEncInput, "粘贴明文输入");
                IconToolTipHelper.SetIconToolTip(imgClearEncInput, "清空明文输入");
                IconToolTipHelper.SetIconToolTip(imgCopyEncOutput, "复制密文结果");
                IconToolTipHelper.SetIconToolTip(imgPasteEncOutput, "粘贴密文结果");
                IconToolTipHelper.SetIconToolTip(imgClearEncOutput, "清空密文结果");
                IconToolTipHelper.SetIconToolTip(imgCopyEncExtra, "复制临时私钥ePriv");
                IconToolTipHelper.SetIconToolTip(imgPasteEncExtra, "粘贴临时私钥ePriv");
                IconToolTipHelper.SetIconToolTip(imgClearEncExtra, "清空临时私钥ePriv");
                IconToolTipHelper.SetIconToolTip(imgCopyEncEphemeralPub, "复制临时公钥ePub");
                IconToolTipHelper.SetIconToolTip(imgPasteEncEphemeralPub, "粘贴临时公钥ePub");
                IconToolTipHelper.SetIconToolTip(imgClearEncEphemeralPub, "清空临时公钥ePub");
                IconToolTipHelper.SetIconToolTip(imgCopyEncKey, "复制对称密钥");
                IconToolTipHelper.SetIconToolTip(imgPasteEncKey, "粘贴对称密钥");
                IconToolTipHelper.SetIconToolTip(imgClearEncKey, "清空对称密钥");
                IconToolTipHelper.SetIconToolTip(imgCopyEncIV, "复制初始向量(IV)");
                IconToolTipHelper.SetIconToolTip(imgPasteEncIV, "粘贴初始向量(IV)");
                IconToolTipHelper.SetIconToolTip(imgClearEncIV, "清空初始向量(IV)");
                IconToolTipHelper.SetIconToolTip(imgCopyEncBobPublic, "复制 Bob 公钥");
                IconToolTipHelper.SetIconToolTip(imgPasteEncBobPublic, "粘贴 Bob 公钥");
                IconToolTipHelper.SetIconToolTip(imgClearEncBobPublic, "清空 Bob 公钥");
                IconToolTipHelper.SetIconToolTip(imgCopyEncTest, "复制测试");
                IconToolTipHelper.SetIconToolTip(imgPasteEncTest, "粘贴测试");
                IconToolTipHelper.SetIconToolTip(imgClearEncTest, "清空测试");
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
        private void LogInfo(string msg) => Log("[ECIES] " + msg, Brushes.Black);
        private void LogOk(string msg) => Log("[ECIES] " + msg, Brushes.Green);
        private void LogErr(string msg) => Log("[ECIES] " + msg, Brushes.Red);

        #endregion

        #region Hex / Base64 工具

        private static string ToHex(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes) sb.Append(b.ToString("x2"));
            return sb.ToString();
        }

        private static byte[] FromHex(string hex)
        {
            hex = (hex ?? string.Empty).Trim();
            if (hex.Length % 2 != 0) throw new FormatException("Hex 长度必须为偶数");
            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
                bytes[i] = (byte)((HexDigit(hex[i * 2]) << 4) | HexDigit(hex[i * 2 + 1]));
            return bytes;
        }

        private static int HexDigit(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            throw new FormatException($"非法 Hex 字符: {c}");
        }

        private static bool IsHexOnly(string s)
        {
            if (string.IsNullOrEmpty(s)) return false;
            foreach (var c in s)
            {
                bool ok = (c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F') || c == ' ' || c == '\r' || c == '\n';
                if (!ok) return false;
            }
            return true;
        }

        /// <summary>
        /// 智能解码：优先 PEM（外部调用识别），否则尝试 Hex（按用户偏好），否则 Base64。
        /// </summary>
        private static byte[] SmartDecode(string text, string prefer, string label)
        {
            text = (text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(text)) throw new ArgumentException($"{label}为空");

            string cleaned = text.Replace(" ", "").Replace("\r", "").Replace("\n", "");
            if (cleaned.StartsWith("----")) throw new ArgumentException($"{label}是 PEM，请直接走 PEM 通道");

            if (string.Equals(prefer, "Hex", StringComparison.OrdinalIgnoreCase) || IsHexOnly(cleaned))
            {
                try { return FromHex(cleaned); } catch { }
            }
            try { return Convert.FromBase64String(cleaned); } catch { }

            throw new ArgumentException($"{label}既不是合法的 Hex 也不是合法的 Base64");
        }

        private static string EncodeByFmt(byte[] bytes, string fmt)
        {
            return string.Equals(fmt, "Hex", StringComparison.OrdinalIgnoreCase) ? ToHex(bytes) : Convert.ToBase64String(bytes);
        }

        #endregion

        #region 复制 / 粘贴

        private void BtnEncInputCopy_Click(object sender, RoutedEventArgs e) => TryCopy(textEncInput?.Text, "明文输入");
        private void BtnEncInputPaste_Click(object sender, RoutedEventArgs e) => TryPaste(textEncInput);
        private void BtnEncOutputCopy_Click(object sender, RoutedEventArgs e) => TryCopy(textEncOutput?.Text, "密文结果");
        private void BtnEncOutputPaste_Click(object sender, RoutedEventArgs e) => TryPaste(textEncOutput);
        private void BtnEncExtraCopy_Click(object sender, RoutedEventArgs e) => TryCopy(textEncExtra?.Text, "临时私钥ePriv");
        private void BtnEncExtraPaste_Click(object sender, RoutedEventArgs e) => TryPaste(textEncExtra);
        private void BtnEncEphemeralPubCopy_Click(object sender, RoutedEventArgs e) => TryCopy(textEncEphemeralPub?.Text, "临时公钥ePub");
        private void BtnEncEphemeralPubPaste_Click(object sender, RoutedEventArgs e) => TryPaste(textEncEphemeralPub);

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

        private void TryPaste(TextBox? target)
        {
            try
            {
                if (target == null) return;
                if (!Clipboard.ContainsText()) { LogInfo("剪贴板无文本"); return; }
                target.Text = Clipboard.GetText().Trim();
                LogOk("已粘贴到文本框");
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

        #region ComboBox / 文本框取值

        private string InputFmt() => SelectedText(comboEncInputFormat, "UTF-8文本");
        private string OutputFmt() => SelectedText(comboEncOutputFormat, "Base64");
        private string EncMode() => SelectedText(comboEncMode, "ECIES (ECDH+AES-GCM, SHA-256)");

        private static string SelectedText(ComboBox? cb, string fallback)
        {
            if (cb == null) return fallback;
            if (cb.SelectedItem is ComboBoxItem ci)
                return ci.Content?.ToString() ?? fallback;
            return cb.SelectedValue?.ToString() ?? fallback;
        }

        private byte[] GetEncInputBytes()
        {
            var text = textEncInput?.Text ?? string.Empty;
            if (string.IsNullOrEmpty(text)) return [];
            return InputFmt() switch
            {
                "Hex" => FromHex(text),
                "Base64" => Convert.FromBase64String(text.Trim()),
                _ => Encoding.UTF8.GetBytes(text)
            };
        }

        private byte[] GetEncOutputBytes()
        {
            var text = textEncOutput?.Text ?? string.Empty;
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("密文为空");
            return SmartDecode(text, OutputFmt(), "密文结果");
        }

        #endregion

        #region 加密模式枚举

        private enum EncryptionAlgorithm
        {
            EciesAesGcmSha256,
            EciesAesGcmSha512,
            AesGcm,
            AesCbc,
            ChaCha20
        }

        private static string AlgoName(EncryptionAlgorithm a) => a switch
        {
            EncryptionAlgorithm.EciesAesGcmSha256 => "ECIES (ECDH+AES-GCM, SHA-256)",
            EncryptionAlgorithm.EciesAesGcmSha512 => "ECIES (ECDH+AES-GCM, SHA-512)",
            EncryptionAlgorithm.AesGcm => "AES-256-GCM",
            EncryptionAlgorithm.AesCbc => "AES-256-CBC",
            EncryptionAlgorithm.ChaCha20 => "ChaCha20-Poly1305",
            _ => "未知算法"
        };

        private EncryptionAlgorithm GetAlgorithm()
        {
            return EncMode() switch
            {
                "ECIES (ECDH+AES-GCM, SHA-256)" => EncryptionAlgorithm.EciesAesGcmSha256,
                "ECIES (ECDH+AES-GCM, SHA-512)" => EncryptionAlgorithm.EciesAesGcmSha512,
                "AES-256-GCM" => EncryptionAlgorithm.AesGcm,
                "AES-256-CBC" => EncryptionAlgorithm.AesCbc,
                "ChaCha20-Poly1305" => EncryptionAlgorithm.ChaCha20,
                _ => EncryptionAlgorithm.EciesAesGcmSha256
            };
        }

        #endregion

        #region 密钥获取

        private ECPublicKeyParameters GetBobPublicKey()
        {
            var pem = (textEncBobPublic?.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(pem))
                pem = PublicKeyProvider?.Invoke()?.Trim() ?? string.Empty;

            if (string.IsNullOrEmpty(pem))
                throw new InvalidOperationException("未找到 Bob 公钥：请填写参数区 Bob 公钥或先生成顶部 ECDSA 密钥对。");
            if (!pem.StartsWith("-----"))
                throw new InvalidOperationException("Bob 公钥必须是 PEM 格式（参数区或顶部）。");

            try { return EcdsaKeyHelper.ImportPublicKeyPem(pem); }
            catch (Exception ex) { throw new InvalidOperationException($"Bob 公钥解析失败: {ex.Message}"); }
        }

        private ECPrivateKeyParameters GetBobPrivateKey()
        {
            var pem = PrivateKeyProvider?.Invoke()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(pem))
                throw new InvalidOperationException("未找到 Bob 私钥：请先生成顶部 ECDSA 密钥对。");
            try { return EcdsaKeyHelper.ImportPrivateKeyPem(pem); }
            catch (Exception ex) { throw new InvalidOperationException($"Bob 私钥解析失败: {ex.Message}"); }
        }

        /// <summary>
        /// 获取 HKDF info 字节（留空为 0 字节）。
        /// </summary>
        private byte[] GetEncInfoBytes()
        {
            var info = textEncInfo?.Text ?? string.Empty;
            return string.IsNullOrEmpty(info) ? [] : Encoding.UTF8.GetBytes(info);
        }

        #endregion

        #region 加密 / 解密原始算法（AES-GCM / AES-CBC / ChaCha20-Poly1305）

        private static (byte[] ct, byte[] tag) EncryptAesGcm(byte[] key, byte[] iv, byte[] plain, byte[]? aad)
        {
            using var aes = new AesGcm(key, 16);
            var ct = new byte[plain.Length];
            var tag = new byte[16];
            aes.Encrypt(iv, plain, ct, tag, aad);
            return (ct, tag);
        }

        private static byte[] DecryptAesGcm(byte[] key, byte[] iv, byte[] cipher, byte[] tag, byte[]? aad)
        {
            using var aes = new AesGcm(key, 16);
            var pt = new byte[cipher.Length];
            aes.Decrypt(iv, cipher, tag, pt, aad);
            return pt;
        }

        private static byte[] EncryptAesCbc(byte[] key, byte[] iv, byte[] plain)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var enc = aes.CreateEncryptor();
            return enc.TransformFinalBlock(plain, 0, plain.Length);
        }

        private static byte[] DecryptAesCbc(byte[] key, byte[] iv, byte[] cipher)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var dec = aes.CreateDecryptor();
            return dec.TransformFinalBlock(cipher, 0, cipher.Length);
        }

        private static (byte[] ct, byte[] tag) EncryptChaCha(byte[] key, byte[] iv, byte[] plain, byte[]? aad)
        {
            using var cc = new ChaCha20Poly1305(key);
            var ct = new byte[plain.Length];
            var tag = new byte[16];
            cc.Encrypt(iv, plain, ct, tag, aad);
            return (ct, tag);
        }

        private static byte[] DecryptChaCha(byte[] key, byte[] iv, byte[] cipher, byte[] tag, byte[]? aad)
        {
            using var cc = new ChaCha20Poly1305(key);
            var pt = new byte[cipher.Length];
            cc.Decrypt(iv, cipher, tag, pt, aad);
            return pt;
        }

        #endregion

        #region HKDF + 随机

        private static byte[] HkdfDerive(byte[] secret, byte[] info, int keyLen, bool useSha256)
        {
            IDigest digest = useSha256 ? new Sha256Digest() : new Sha512Digest();
            var hkdf = new HkdfBytesGenerator(digest);
            hkdf.Init(new HkdfParameters(secret, null, info));
            var derived = new byte[keyLen];
            hkdf.GenerateBytes(derived, 0, derived.Length);
            return derived;
        }

        private static byte[] RandomBytes(int len)
        {
            var b = new byte[len];
            RandomNumberGenerator.Fill(b);
            return b;
        }

        #endregion

        #region 一键复制密钥对（顶部 → 下方 ePriv / ePub）

        /// <summary>
        /// 把顶部 ECDSA 私钥(PEM)写入下方"临时私钥 ePriv"，公钥(PEM)写入"临时公钥 ePub"。
        /// 用于让 ECIES 加密直接复用顶部那对长期密钥对，避免每次都新生成。
        /// </summary>
        private void BtnCopyTopKeyPair_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var priv = PrivateKeyProvider?.Invoke()?.Trim();
                var pub = PublicKeyProvider?.Invoke()?.Trim();

                if (string.IsNullOrEmpty(priv) && string.IsNullOrEmpty(pub))
                {
                    LogErr("顶部 ECDSA 面板未提供私钥/公钥，请先在顶部生成或导入一对密钥。");
                    return;
                }

                var lines = new List<string>(2);
                if (!string.IsNullOrEmpty(priv))
                {
                    if (textEncExtra != null) textEncExtra.Text = priv!;
                    lines.Add($"ePriv 已写入（{priv!.Length} 字符）");
                }
                else
                {
                    lines.Add("顶部私钥为空（仅复制公钥）");
                }

                if (!string.IsNullOrEmpty(pub))
                {
                    if (textEncEphemeralPub != null) textEncEphemeralPub.Text = pub!;
                    lines.Add($"ePub 已写入（{pub!.Length} 字符）");
                }
                else
                {
                    lines.Add("顶部公钥为空（仅复制私钥）");
                }

                LogOk($"一键复制顶部密钥对完成：{string.Join("；", lines)}");
            }
            catch (Exception ex)
            {
                LogErr($"一键复制密钥对失败: {ex.Message}");
            }
        }

        #endregion

        #region 加密流程

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var algo = GetAlgorithm();
                _lastAlgorithmName = AlgoName(algo);

                var plain = GetEncInputBytes();
                if (algo is EncryptionAlgorithm.EciesAesGcmSha256 or EncryptionAlgorithm.EciesAesGcmSha512)
                    EciesEncrypt(plain, algo == EncryptionAlgorithm.EciesAesGcmSha256);
                else
                    SymmetricEncrypt(plain, algo);
            }
            catch (Exception ex)
            {
                LogErr($"加密失败: {ex.Message}");
            }
        }

        /// <summary>
        /// ECIES 加密：1:1 对齐 CryptoTool.Win ECDSA03。
        /// 流程：
        ///   1. 取 Bob 公钥；
        ///   2. 取/现场生成临时 ECDH 密钥对（密文头部需 ePub，每次加密重新生成）；
        ///   3. 用 临时私钥 × Bob 公钥 做 ECDH，HKDF 派生 32 字节 AES-256 密钥
        ///      （若 textEncKey 已手动填写，则覆盖派生结果，使用用户密钥）；
        ///   4. AES-256-GCM 加密；
        ///   5. 输出 = 临时公钥(X9.62 未压缩) || IV(12B) || Cipher || Tag(16B)。
        /// </summary>
        private void EciesEncrypt(byte[] plain, bool useSha256)
        {
            var bobPub = GetBobPublicKey();
            var curveName = EcdsaKeyHelper.GetCurveName(bobPub);
            if (curveName == "未知曲线")
                throw new InvalidOperationException("无法识别 Bob 公钥曲线（仅支持命名曲线）");

            var info = GetEncInfoBytes();
            _lastEncInfo = Encoding.UTF8.GetString(info);

            // 1) 取/现场生成临时密钥对（每次 ECIES 加密都用新对，密文头部的 ePub 必须与本次派生的 AES key 对应）
            var (ephPub, ephPriv) = GetOrCreateEphemeralKeyPair(curveName);
            byte[] ephPubEnc = ephPub.Q.GetEncoded(false);
            _lastEphemeralPubKey = ephPubEnc;
            _lastEphemeralCurveName = curveName;

            // 2) ECIES 标准派生：临时私钥 × Bob 公钥 + HKDF。
            //    注意：textEncKey（对称密钥框）不参与 ECIES——解密方由密文头部的
            //    ePub × BobPriv 还原同一密钥，这里总是走 ECDH 派生，
            //    避免残留的手动 key 或旧派生 key 导致密文无法解开。派生结果仅回填展示。
            byte[] shared = EcdhAlgorithm.DeriveSharedSecret(ephPriv, bobPub);
            byte[] aesKey = HkdfDerive(shared, info, 32, useSha256);
            var curKey = (textEncKey?.Text ?? string.Empty).Trim();
            if (!string.Equals(curKey, Convert.ToBase64String(aesKey), StringComparison.Ordinal))
                textEncKey!.Text = Convert.ToBase64String(aesKey);
            string keySource = $"ECIES(临时私钥×Bob) + HKDF(SHA-{(useSha256 ? "256" : "512")})";
            _lastEncKey = aesKey;

            // 3) 每次加密都生成新的随机 IV（覆盖回填），确保两次加密的密文不同（GCM IV 必须唯一）
            byte[] iv = GetEncIV(12, forceNew: true);
            _lastEncIV = iv;

            // 4) AES-GCM 加密
            var (cipher, tag) = EncryptAesGcm(aesKey, iv, plain, null);
            _lastEncCipher = cipher;
            _lastEncTag = tag;

            // 5) 输出 = 临时公钥(X9.62 未压缩) || IV || Cipher || Tag
            byte[] payload = [.. ephPubEnc, .. iv, .. cipher, .. tag];
            textEncOutput.Text = StringUtil.WrapTextEvery(EncodeByFmt(payload, OutputFmt()), 50);

            // 6) 一次性合并日志为单段（对齐 WinForms：每次操作只输出一个时间戳段）
            var ivHex = Convert.ToHexString(iv);
            LogOk(
                $"加密完成\r\n" +
                $"算法：{_lastAlgorithmName}\r\n" +
                $"AES 密钥来源：{keySource}\r\n" +
                $"IV(hex)：{ivHex}\r\n" +
                $"密文长度：{cipher.Length} 字节（Tag {tag.Length} 字节，明文 {plain.Length} 字节，载荷 {payload.Length} = ePub {ephPubEnc.Length} + IV {iv.Length} + cipher {cipher.Length} + tag {tag.Length}）\r\n" +
                $"曲线：{curveName}");
        }

        /// <summary>
        /// 取 IV：
        ///   - forceNew=true（ECIES 加密）：每次强制生成新的随机 IV 并覆盖回填，
        ///     确保两次加密的密文不同（GCM IV 唯一性）；
        ///   - forceNew=false（对称加密）：textEncIV 留空则随机生成并回填，
        ///     非空则按当前密文格式解码（尊重用户手动指定的 IV）。
        /// </summary>
        private byte[] GetEncIV(int targetLen, bool forceNew = false)
        {
            var ivText = (textEncIV?.Text ?? string.Empty).Trim();
            if (forceNew || string.IsNullOrEmpty(ivText))
            {
                var iv = RandomBytes(targetLen);
                textEncIV!.Text = EncodeByFmt(iv, OutputFmt());
                return iv;
            }
            var bytes = SmartDecode(ivText, OutputFmt(), "IV");
            if (bytes.Length != targetLen)
                throw new ArgumentException($"IV 必须为 {targetLen} 字节，当前 {bytes.Length} 字节");
            return bytes;
        }

        /// <summary>
        /// 取或新生成临时 ECDH 密钥对：textEncExtra (ePriv) 与 textEncEphemeralPub (ePub) 任一非空则尝试解析；
        /// 都为空则现场生成并回填。
        /// </summary>
        /// <summary>
        /// 取或现场生成临时 ECDH 密钥对。
        ///   - textEncExtra 是合法 SEC1 PEM → 使用用户的 ePriv，并从私钥派生 ePub；
        ///   - 其它情况（包括只填了 ePub，或 ePriv 是非 SEC1 文本）一律现场生成新对并回填。
        /// 对齐 WinForms ECDSA03：每次 ECIES 加密都用新的临时密钥对，确保密文头部的 ePub
        /// 与本次 AES key 派生所用的临时私钥一一对应。
        /// </summary>
        private (ECPublicKeyParameters pub, ECPrivateKeyParameters priv) GetOrCreateEphemeralKeyPair(string curveName)
        {
            var privText = (textEncExtra?.Text ?? string.Empty).Trim();
            var pubText = (textEncEphemeralPub?.Text ?? string.Empty).Trim();

            if (!string.IsNullOrEmpty(privText) && privText.StartsWith("-----"))
            {
                try
                {
                    var priv = EcdsaKeyHelper.ImportPrivateKeyPem(privText);
                    var derivedPub = DerivePublicFromPrivate(priv);
                    var derivedPubPem = EcdsaKeyHelper.ExportPublicKeyPem(derivedPub);

                    // 若用户同时填了 ePub 但与 ePriv 推导不一致，给出提示并以 ePriv 为准
                    if (!string.IsNullOrEmpty(pubText) && pubText.StartsWith("-----") &&
                        !string.Equals(pubText.Trim(), derivedPubPem.Trim(), StringComparison.Ordinal))
                    {
                        LogInfo("参数区 ePub 与 ePriv 推导不一致，已忽略 ePub，以 ePriv 推导的 ePub 为准");
                    }

                    // 始终保证展示的 ePub 与 ePriv 一致
                    var curPub = (textEncEphemeralPub?.Text ?? string.Empty).Trim();
                    if (!string.Equals(curPub, derivedPubPem.Trim(), StringComparison.Ordinal))
                        textEncEphemeralPub!.Text = derivedPubPem;

                    // 中间日志已全部去掉，最终统一由 EciesEncrypt 末尾 LogOk 合并输出。
                    return (derivedPub, priv);
                }
                catch
                {
                    // 解析失败静默回退到现场生成（避免污染主日志段）
                }
            }

            // 现场生成新对（覆盖 UI 上已有的任何 ePub / ePriv）
            var pair = EcdhAlgorithm.GenerateKeyPair(curveName);
            var ephPriv = (ECPrivateKeyParameters)pair.Private;
            var ephPub = (ECPublicKeyParameters)pair.Public;

            textEncExtra!.Text = EcdsaKeyHelper.ExportPrivateKeyPem(ephPriv);
            textEncEphemeralPub!.Text = EcdsaKeyHelper.ExportPublicKeyPem(ephPub);
            return (ephPub, ephPriv);
        }

        private static ECPublicKeyParameters DerivePublicFromPrivate(ECPrivateKeyParameters priv)
        {
            var q = priv.Parameters.G.Multiply(priv.D).Normalize();
            return new ECPublicKeyParameters(q, priv.Parameters);
        }

        private void SymmetricEncrypt(byte[] plain, EncryptionAlgorithm algo)
        {
            byte[] key = GetEncKeyFromBox(out bool autoDerivedKey);
            int ivLen = algo == EncryptionAlgorithm.AesCbc ? 16 : 12;
            byte[] iv = GetEncIV(ivLen);

            byte[] payload;
            if (algo == EncryptionAlgorithm.AesCbc)
            {
                var cipher = EncryptAesCbc(key, iv, plain);
                payload = [.. iv, .. cipher];
                _lastEncCipher = cipher;
                _lastEncTag = null;
                _lastEncIV = iv;
                _lastEncKey = key;
                textEncEphemeralPub.Clear();
                textEncExtra.Clear();
                _lastEphemeralPubKey = null;
                _lastEphemeralCurveName = null;
            }
            else if (algo == EncryptionAlgorithm.AesGcm)
            {
                var (cipher, tag) = EncryptAesGcm(key, iv, plain, null);
                payload = [.. iv, .. cipher, .. tag];
                _lastEncCipher = cipher;
                _lastEncTag = tag;
                _lastEncIV = iv;
                _lastEncKey = key;
                textEncEphemeralPub.Clear();
                textEncExtra.Clear();
                _lastEphemeralPubKey = null;
                _lastEphemeralCurveName = null;
            }
            else // ChaCha20
            {
                var (cipher, tag) = EncryptChaCha(key, iv, plain, null);
                payload = [.. iv, .. cipher, .. tag];
                _lastEncCipher = cipher;
                _lastEncTag = tag;
                _lastEncIV = iv;
                _lastEncKey = key;
                textEncEphemeralPub.Clear();
                textEncExtra.Clear();
                _lastEphemeralPubKey = null;
                _lastEphemeralCurveName = null;
            }

            textEncOutput.Text = StringUtil.WrapTextEvery(EncodeByFmt(payload, OutputFmt()), 50);

            LogOk($"{_lastAlgorithmName} 加密完成：明文 {plain.Length} 字节，密文 {_lastEncCipher.Length} 字节（IV {ivLen} 字节）" +
                  (autoDerivedKey
                      ? "\r\n对称密钥：参数区为空，已自动用当前 ECDSA 私钥派生（HKDF-SHA256）并回填到参数区"
                      : ""));
        }

        /// <summary>
        /// 取对称密钥（对称模式 AES-GCM / AES-CBC / ChaCha20 的加密与解密共用）：
        ///   - 参数区"对称密钥"已填写：按 Base64/Hex 智能解码并校验为 32 字节；
        ///   - 为空时不再抛错阻断执行（对齐 CryptoTool.Win ECDSA03 的 GetEncKey）：
        ///     改用当前顶部 ECDSA 私钥的标量值 + info 经 HKDF-SHA256 派生 32 字节
        ///     AES-256 密钥并回填参数区展示，保证"清空共享密钥后再点加密/解密"仍可执行。
        /// </summary>
        private byte[] GetEncKeyFromBox(out bool autoDerived)
        {
            autoDerived = false;
            var keyText = (textEncKey?.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(keyText))
            {
                var privPem = PrivateKeyProvider?.Invoke()?.Trim() ?? string.Empty;
                if (string.IsNullOrEmpty(privPem))
                    throw new InvalidOperationException(
                        "参数区对称密钥为空，且当前没有可用的 ECDSA 私钥：请先在顶部生成/导入密钥对，或手动填写 32 字节对称密钥。");

                ECPrivateKeyParameters priv;
                try { priv = EcdsaKeyHelper.ImportPrivateKeyPem(privPem); }
                catch (Exception ex) { throw new InvalidOperationException($"当前 ECDSA 私钥解析失败: {ex.Message}"); }

                byte[] derived = HkdfDerive(priv.D.ToByteArrayUnsigned(), GetEncInfoBytes(), 32, true);
                if (textEncKey != null)
                    textEncKey.Text = Convert.ToBase64String(derived);
                autoDerived = true;
                return derived;
            }

            if (keyText.StartsWith("-----"))
                throw new ArgumentException("对称密钥字段不支持 PEM，请填写 Base64 / Hex 编码的 32 字节密钥");
            var keyBytes = SmartDecode(keyText, "Base64", "对称密钥");
            if (keyBytes.Length != 32)
                throw new ArgumentException($"对称密钥必须为 32 字节（256 位），当前 {keyBytes.Length} 字节");
            return keyBytes;
        }

        #endregion

        #region 解密流程

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var algo = GetAlgorithm();
                _lastAlgorithmName = AlgoName(algo);
                byte[] payload = GetEncOutputBytes();

                byte[] plain = algo switch
                {
                    EncryptionAlgorithm.EciesAesGcmSha256 => EciesDecrypt(payload, true),
                    EncryptionAlgorithm.EciesAesGcmSha512 => EciesDecrypt(payload, false),
                    EncryptionAlgorithm.AesGcm => SymmetricDecrypt(payload, 12, true),
                    EncryptionAlgorithm.AesCbc => SymmetricDecrypt(payload, 16, false),
                    EncryptionAlgorithm.ChaCha20 => SymmetricDecrypt(payload, 12, true),
                    _ => throw new NotSupportedException()
                };

                try
                {
                    var s = Encoding.UTF8.GetString(plain);
                    textEncInput.Text = s;
                    LogOk($"{_lastAlgorithmName} 解密完成：明文 {plain.Length} 字节（UTF-8）");
                }
                catch
                {
                    textEncInput.Text = Convert.ToBase64String(plain);
                    LogOk($"{_lastAlgorithmName} 解密完成：明文 {plain.Length} 字节（非 UTF-8，已按 Base64 显示）");
                }
            }
            catch (Exception ex)
            {
                LogErr($"解密失败: {ex.Message}");
            }
        }

        private byte[] EciesDecrypt(byte[] payload, bool useSha256)
        {
            var bobPriv = GetBobPrivateKey();
            var curveName = EcdsaKeyHelper.GetCurveName(bobPriv);
            if (curveName == "未知曲线")
                throw new InvalidOperationException("无法识别 Bob 私钥曲线（仅支持命名曲线）");

            int ephPubLen = EcdhAlgorithm.GetEphemeralPubKeyLength(curveName);
            // 临时公钥 || 12字节 IV || 密文 || 16字节 Tag
            if (payload.Length < ephPubLen + 12 + 16)
                throw new ArgumentException($"密文长度不足：至少 {ephPubLen + 12 + 16} 字节，当前 {payload.Length} 字节");

            byte[] ephPubBytes = payload[..ephPubLen];
            byte[] iv = payload[ephPubLen..(ephPubLen + 12)];
            byte[] cipher = payload[(ephPubLen + 12)..^16];
            byte[] tag = payload[^16..];

            // 恢复临时公钥（ECPublicKeyParameters）
            var x9 = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"不支持的曲线: {curveName}");
            var oid = ECNamedCurveTable.GetOid(curveName);
            var domain = new ECNamedDomainParameters(oid, x9.Curve, x9.G, x9.N, x9.H, x9.GetSeed());
            var q = x9.Curve.DecodePoint(ephPubBytes);
            var ephPub = new ECPublicKeyParameters(q, domain);

            // 派生密钥
            var info = GetEncInfoBytes();
            _lastEncInfo = Encoding.UTF8.GetString(info);
            byte[] shared = EcdhAlgorithm.DeriveSharedSecret(bobPriv, ephPub);
            byte[] aesKey = HkdfDerive(shared, info, 32, useSha256);

            // 解密
            byte[] plain = DecryptAesGcm(aesKey, iv, cipher, tag, null);

            // 展示临时密钥
            textEncEphemeralPub.Text = EcdsaKeyHelper.ExportPublicKeyPem(ephPub);
            // 解密方没有 ePriv，把解密得到的临时公钥对应的曲线记录下来，方便"密文拼接标准"
            textEncExtra.Clear();

            _lastEphemeralPubKey = ephPubBytes;
            _lastEphemeralCurveName = curveName;
            _lastEncIV = iv;
            _lastEncCipher = cipher;
            _lastEncTag = tag;
            _lastEncKey = aesKey;

            return plain;
        }

        private byte[] SymmetricDecrypt(byte[] payload, int ivLen, bool hasTag)
        {
            byte[] key = GetEncKeyFromBox(out _);
            int minLen = ivLen + (hasTag ? 16 : 0);
            if (payload.Length < minLen)
                throw new ArgumentException($"密文长度不足：至少 {minLen} 字节，当前 {payload.Length} 字节");

            byte[] iv = payload[..ivLen];
            byte[] cipher = hasTag ? payload[ivLen..^16] : payload[ivLen..];
            byte[] tag = hasTag ? payload[^16..] : null!;

            _lastEncIV = iv;
            _lastEncKey = key;
            _lastEncCipher = cipher;
            _lastEncTag = hasTag ? tag : null;

            return GetAlgorithm() switch
            {
                EncryptionAlgorithm.AesGcm => DecryptAesGcm(key, iv, cipher, tag, null),
                EncryptionAlgorithm.AesCbc => DecryptAesCbc(key, iv, cipher),
                EncryptionAlgorithm.ChaCha20 => DecryptChaCha(key, iv, cipher, tag, null),
                _ => throw new NotSupportedException()
            };
        }

        #endregion

        #region 清空

        private void BtnEncClear_Click(object sender, RoutedEventArgs e)
        {
            textEncInput?.Clear();
            textEncOutput?.Clear();
            textEncExtra?.Clear();
            textEncEphemeralPub?.Clear();
            textEncBobPublic?.Clear();
            textEncKey?.Clear();
            textEncIV?.Clear();
            textEncInfo?.Clear();
            textEncTest?.Clear();

            _lastEncCipher = null;
            _lastEncIV = null;
            _lastEncTag = null;
            _lastEphemeralPubKey = null;
            _lastEphemeralCurveName = null;
            _lastAlgorithmName = null;
            _lastEncInfo = null;
            _lastEncKey = null;

            LogOk("已清空全部输入");
        }

        #endregion

        #region 密文拼接标准

        private void BtnEncFormatInfo_Click(object sender, RoutedEventArgs e)
        {
            if (_lastEncIV == null || _lastEncCipher == null || _lastAlgorithmName == null)
            {
                MessageBox.Show("请先执行一次加密操作，再查看密文拼接标准。", "提示",
                    MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var algo = GetAlgorithm();
            var msg = new StringBuilder();
            msg.AppendLine("================ 密文拼接标准 ================");
            msg.AppendLine($"算法      : {_lastAlgorithmName}");
            if (!string.IsNullOrEmpty(_lastEphemeralCurveName))
                msg.AppendLine($"曲线      : {_lastEphemeralCurveName}");
            msg.AppendLine($"HKDF info : {_lastEncInfo ?? string.Empty}");
            msg.AppendLine(_lastEncKey == null
                ? $"AES 密钥  : 32 字节 (HKDF 派生，对称模式时为参数区密钥)"
                : $"AES 密钥  : {ToHex(_lastEncKey)}");
            msg.AppendLine();

            int offset = 0;

            if (algo is EncryptionAlgorithm.EciesAesGcmSha256 or EncryptionAlgorithm.EciesAesGcmSha512)
            {
                if (_lastEphemeralPubKey == null)
                {
                    MessageBox.Show("缺少临时公钥信息，请重试。", "提示", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }
                msg.AppendLine($"[偏移 {offset,3}, 长度 {_lastEphemeralPubKey.Length,3}] 临时公钥 (X9.62 未压缩)");
                var ephPubHex = ToHex(_lastEphemeralPubKey);
                msg.AppendLine("    " + ephPubHex.AsSpan(0, Math.Min(64, ephPubHex.Length)).ToString() + " ...");
                offset += _lastEphemeralPubKey.Length;
                msg.AppendLine();
            }

            msg.AppendLine($"[偏移 {offset,3}, 长度 {_lastEncIV.Length,3}] 初始向量 IV / Nonce");
            msg.AppendLine("    " + ToHex(_lastEncIV));
            offset += _lastEncIV.Length;
            msg.AppendLine();

            msg.AppendLine($"[偏移 {offset,3}, 长度 {_lastEncCipher.Length,3}] 密文 CipherText");
            var cipherHex = ToHex(_lastEncCipher);
            msg.AppendLine("    " + (cipherHex.Length > 64
                ? cipherHex.AsSpan(0, 64).ToString() + " ..."
                : cipherHex));
            offset += _lastEncCipher.Length;
            msg.AppendLine();

            if (_lastEncTag != null)
            {
                msg.AppendLine($"[偏移 {offset,3}, 长度 {_lastEncTag.Length,3}] 认证标签 AuthTag (GCM/Poly1305)");
                msg.AppendLine("    " + ToHex(_lastEncTag));
                offset += _lastEncTag.Length;
                msg.AppendLine();
            }

            msg.AppendLine($"总长度      : {offset} 字节");
            msg.AppendLine();
            msg.AppendLine("说明：解密方按相同顺序与长度解析 payload，按相同 info/算法派生密钥后逐段还原。");

            MessageBox.Show(msg.ToString(), "密文拼接标准", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        #endregion

        #region 临时密钥转换（智能循环）

        private void BtnEncKeyConvert_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var dir = SelectedText(comboEncKeyConvert, "临时公钥 → Base64");
                bool isPub = dir.StartsWith("临时公钥", StringComparison.Ordinal);
                bool toPem = dir.EndsWith("PEM", StringComparison.Ordinal);

                if (isPub) ConvertEphemeralPublicKey(toPem);
                else ConvertEphemeralPrivateKey(toPem);
            }
            catch (Exception ex)
            {
                LogErr($"临时密钥转换失败: {ex.Message}");
            }
        }

        private void ConvertEphemeralPublicKey(bool toPem)
        {
            var src = (textEncEphemeralPub?.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(src))
                throw new ArgumentException("临时公钥 ePub 为空，请先加密或粘贴临时公钥");

            // PEM → Base64/Hex
            if (src.StartsWith("-----"))
            {
                var pub1 = EcdsaKeyHelper.ImportPublicKeyPem(src);
                var enc = pub1.Q.GetEncoded(false);
                textEncEphemeralPub!.Text = toPem ? src : EncodeByFmt(enc, OutputFmt());
                _lastEphemeralPubKey = enc;
                _lastEphemeralCurveName ??= EcdsaKeyHelper.GetCurveName(pub1);
                LogOk($"临时公钥 PEM → {(toPem ? "PEM" : EncodeByFmt(enc, OutputFmt()).Length < 50 ? "Base64" : "Hex")} 完成");
                return;
            }

            // Base64 / Hex → PEM
            var bytes = SmartDecode(src, OutputFmt(), "临时公钥");
            var curveName = _lastEphemeralCurveName ?? GuessCurveFromPubBytes(bytes);
            var x9 = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"无法识别曲线（按长度猜测: {curveName}）");
            var oid = ECNamedCurveTable.GetOid(curveName);
            var domain = new ECNamedDomainParameters(oid, x9.Curve, x9.G, x9.N, x9.H, x9.GetSeed());
            var q = x9.Curve.DecodePoint(bytes);
            var pub2 = new ECPublicKeyParameters(q, domain);
            textEncEphemeralPub!.Text = EcdsaKeyHelper.ExportPublicKeyPem(pub2);
            _lastEphemeralPubKey = bytes;
            _lastEphemeralCurveName = curveName;
            LogOk($"临时公钥 → PEM 完成（曲线 {curveName}）");
        }

        private void ConvertEphemeralPrivateKey(bool toPem)
        {
            var src = (textEncExtra?.Text ?? string.Empty).Trim();
            if (string.IsNullOrEmpty(src))
                throw new ArgumentException("临时私钥 ePriv 为空，请先加密或粘贴临时私钥");

            // PEM → Base64/Hex
            if (src.StartsWith("-----"))
            {
                var priv1 = EcdsaKeyHelper.ImportPrivateKeyPem(src);
                var d = priv1.D.ToByteArrayUnsigned();
                textEncExtra!.Text = toPem ? src : EncodeByFmt(d, OutputFmt());
                _lastEphemeralCurveName ??= EcdsaKeyHelper.GetCurveName(priv1);
                LogOk($"临时私钥 PEM → {(toPem ? "PEM" : "Base64/Hex")} 完成");
                return;
            }

            // Base64 / Hex → PEM
            var dBytes = SmartDecode(src, OutputFmt(), "临时私钥");
            var curveName = _lastEphemeralCurveName ?? "secp256r1";
            var x9 = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"不支持的曲线: {curveName}");
            var oid = ECNamedCurveTable.GetOid(curveName);
            var domain = new ECNamedDomainParameters(oid, x9.Curve, x9.G, x9.N, x9.H, x9.GetSeed());
            var priv2 = new ECPrivateKeyParameters(new BigInteger(1, dBytes), domain);
            textEncExtra!.Text = EcdsaKeyHelper.ExportPrivateKeyPem(priv2);
            _lastEphemeralCurveName = curveName;
            LogOk($"临时私钥 → PEM 完成（曲线 {curveName}）");
        }

        private static string GuessCurveFromPubBytes(byte[] enc)
        {
            return enc.Length switch
            {
                65 => "secp256r1",
                97 => "secp384r1",
                133 => "secp521r1",
                _ => "secp256r1"
            };
        }

        #endregion

    }
}
