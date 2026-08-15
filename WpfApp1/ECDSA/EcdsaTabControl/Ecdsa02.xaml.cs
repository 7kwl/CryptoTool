using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using WpfApp1.IconLogic;

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

        // 最近一次签名的 DER 原始字节（用于切换签名格式时实时重新呈现）
        private byte[]? _lastSignatureBytes;

        // 最近一次签名解出的 r / s（各 32 字节大端，用于 R|S 格式重新呈现与验签时回包成 DER）
        private byte[]? _lastSignatureR;
        private byte[]? _lastSignatureS;

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
                if (comboKGeneration != null && comboKGeneration.Items.Count > 0 && comboKGeneration.SelectedIndex < 0)
                    comboKGeneration.SelectedIndex = 0; // 混合熵随机 k（默认）

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
                btnVerifyRSBase64.Click += BtnVerifyRSBase64_Click;
                btnVerifyDerBase64.Click += BtnVerifyDerBase64_Click;

                // 中间"复制/粘贴/清空"图标按钮
                imgCopyPlainData.MouseLeftButtonDown += (s, e) => BtnCopyPlainData_Click(s!, e!);
                imgPastePlainData.MouseLeftButtonDown += (s, e) => BtnPastePlainData_Click(s!, e!);
                imgClearPlainData.MouseLeftButtonDown += (s, e) => BtnClearPlainData_Click(s!, e!);
                imgCopySignatureData.MouseLeftButtonDown += (s, e) => BtnCopySignatureData_Click(s!, e!);
                imgPasteSignatureData.MouseLeftButtonDown += (s, e) => BtnPasteSignatureData_Click(s!, e!);
                imgClearSignatureData.MouseLeftButtonDown += (s, e) => BtnClearSignatureData_Click(s!, e!);
                imgCopySignatureRSBase64.MouseLeftButtonDown += (s, e) => TryCopy(textSignatureRSBase64.Text, "R|S (Base64)");
                imgPasteSignatureRSBase64.MouseLeftButtonDown += (s, e) => TryPaste(textSignatureRSBase64, "R|S (Base64)");
                imgClearSignatureRSBase64.MouseLeftButtonDown += (s, e) => BtnClearSignatureRSBase64_Click(s!, e!);
                imgCopySignatureDerBase64.MouseLeftButtonDown += (s, e) => TryCopy(textSignatureDerBase64.Text, "DER (Base64)");
                imgPasteSignatureDerBase64.MouseLeftButtonDown += (s, e) => TryPaste(textSignatureDerBase64, "DER (Base64)");
                imgClearSignatureDerBase64.MouseLeftButtonDown += (s, e) => BtnClearSignatureDerBase64_Click(s!, e!);

                // 签名格式下拉框切换时实时重新呈现签名
                if (comboSignatureFormat != null)
                    comboSignatureFormat.SelectionChanged += ComboSignatureFormat_SelectionChanged;

                // 注：三个签名相关文本框彼此独立，不再 TextChanged 联动，避免互相覆盖。

                IconToolTipHelper.SetIconToolTip(imgCopyPlainData, "复制原始数据");
                IconToolTipHelper.SetIconToolTip(imgPastePlainData, "粘贴原始数据");
                IconToolTipHelper.SetIconToolTip(imgClearPlainData, "清空原始数据");
                IconToolTipHelper.SetIconToolTip(imgCopySignatureData, "复制签名");
                IconToolTipHelper.SetIconToolTip(imgPasteSignatureData, "粘贴签名");
                IconToolTipHelper.SetIconToolTip(imgClearSignatureData, "清空签名");
                IconToolTipHelper.SetIconToolTip(imgCopySignatureRSBase64, "复制 R|S (Base64) 签名");
                IconToolTipHelper.SetIconToolTip(imgPasteSignatureRSBase64, "粘贴 R|S (Base64) 签名");
                IconToolTipHelper.SetIconToolTip(imgClearSignatureRSBase64, "清空 R|S (Base64) 签名");
                IconToolTipHelper.SetIconToolTip(imgCopySignatureDerBase64, "复制 DER (Base64) 签名");
                IconToolTipHelper.SetIconToolTip(imgPasteSignatureDerBase64, "粘贴 DER (Base64) 签名");
                IconToolTipHelper.SetIconToolTip(imgClearSignatureDerBase64, "清空 DER (Base64) 签名");
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

        /// <summary>
        /// 当前选中的 k 生成方式：
        ///   混合熵随机 k（私钥派生 + 随机盐）（默认） / RFC 6979 确定性 k / 纯 CSPRNG 随机 k
        /// </summary>
        private string GetKGenerationMode() => SelectedText(comboKGeneration, "混合熵随机 k（私钥派生 + 随机盐）（默认）");

        #endregion

        #region 签名器工厂

        // k 生成策略、混合熵签名器、RFC 6979 + 随机盐派生、Hash 摘要构造均已
        // 外移到 CryptoTool.Algorithm.Algorithms.ECDSA.EcdsaKGenerator，
        // 其他界面（文件签名、批量任务等）可直接复用 EcdsaKGenerator.CreateSigner。
        // 三种 k 模式常量：ModeHybridEntropy / ModeRfc6979 / ModeCsprng。

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
                string signerAlg = EcdsaKGenerator.GetSignerAlgorithm(hashAlg);
                string kMode = GetKGenerationMode();

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(privPem);
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);

                // 探针验证密钥对匹配（探针签名用当前模式，验签用默认安全模式即可 —— 验签只看 (r,s)）
                byte[] probe = Encoding.UTF8.GetBytes("__ECDSA_INTERNAL_PROBE__");
                ISigner ps = EcdsaKGenerator.CreateSigner(kMode, hashAlg); ps.Init(true, priv); ps.BlockUpdate(probe, 0, probe.Length);
                byte[] psig = ps.GenerateSignature();
                ISigner pv = SignerUtilities.GetSigner(signerAlg); pv.Init(false, pub); pv.BlockUpdate(probe, 0, probe.Length);
                if (!pv.VerifySignature(psig))
                { LogErr("❌ 密钥对不匹配，无法签名"); return; }

                var data = Encoding.UTF8.GetBytes(textPlainData.Text);
                ISigner signer = EcdsaKGenerator.CreateSigner(kMode, hashAlg); signer.Init(true, priv); signer.BlockUpdate(data, 0, data.Length);
                byte[] signature = signer.GenerateSignature();

                // 统一刷新三个签名框：签名框（按所选格式）+ Base64(Raw 二进制) + Base64(DER 二进制)
                var (rs_r, rs_s) = ParseDerSignature(signature);
                RefreshSignatureTextboxes(signature, PadLeftTo32(rs_r.ToByteArrayUnsigned()), PadLeftTo32(rs_s.ToByteArrayUnsigned()));

                LogOk($"签名成功（{signerAlg} / {kMode}），签名已写入下方签名框");
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
                // 解析签名框：Base64 / Hex / DER → 直接给 BC 的 DER 字节
                string fmt2 = GetSignatureFormat();
                byte[] sig = DecodeSignatureForVerify(textSignature.Text.Trim(), fmt2);
                VerifyWithBytes(sig, "签名框");
            }
            catch (Exception ex)
            {
                LogErr($"❌ 验签异常: {ex.Message}");
            }
        }

        /// <summary>对 Base64(Raw 二进制) 文本框内的签名做验签（先 Base64 解码 → RsToDer 回包成 DER）。</summary>
        private void BtnVerifyRSBase64_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textSignatureRSBase64.Text))
                { LogErr("❌ Base64(Raw 二进制) 签名为空"); return; }

                byte[] rs = Convert.FromBase64String(textSignatureRSBase64.Text.Trim());
                if (rs.Length != 64)
                { LogErr($"❌ Base64(Raw 二进制) 签名应为 64 字节（32+32），实际 {rs.Length}"); return; }

                VerifyWithBytes(RsToDer(rs), "Base64(Raw 二进制)");
            }
            catch (Exception ex)
            {
                LogErr($"❌ R|S 验签异常: {ex.Message}");
            }
        }

        /// <summary>对 Base64(DER 二进制) 文本框内的签名做验签（先 Base64 解码）。</summary>
        private void BtnVerifyDerBase64_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textSignatureDerBase64.Text))
                { LogErr("❌ Base64(DER 二进制) 签名为空"); return; }

                VerifyWithBytes(Convert.FromBase64String(textSignatureDerBase64.Text.Trim()), "Base64(DER 二进制)");
            }
            catch (Exception ex)
            {
                LogErr($"❌ DER 验签异常: {ex.Message}");
            }
        }

        /// <summary>
        /// 公钥/Hash/数据准备与日志输出的公共验签入口。
        /// 调用方需保证 <paramref name="derSig"/> 是 BC 期望的 ASN.1 DER 字节。
        /// </summary>
        private void VerifyWithBytes(byte[] derSig, string source)
        {
            var pubPem = PublicKeyProvider?.Invoke()?.Trim() ?? string.Empty;
            if (string.IsNullOrEmpty(pubPem))
            { LogErr("❌ 公钥为空，无法验签"); return; }

            if (string.IsNullOrWhiteSpace(textPlainData.Text))
            { LogErr("❌ 原始数据为空，无法验签"); return; }

            string hashAlg = GetSelectedHash();
            string signerAlg = EcdsaKGenerator.GetSignerAlgorithm(hashAlg);

            var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);
            ISigner s = SignerUtilities.GetSigner(signerAlg);
            s.Init(false, pub);
            byte[] data = Encoding.UTF8.GetBytes(textPlainData.Text);
            s.BlockUpdate(data, 0, data.Length);

            if (s.VerifySignature(derSig))
                LogOk($"签名验证通过（{hashAlg}，来源：{source}）");
            else
                LogErr($"❌ 签名验证失败（来源：{source}）");
        }

        #endregion

        #region 复制 / 粘贴 / 清空

        private void BtnCopySignature_Click(object sender, RoutedEventArgs e) => TryCopy(textSignature.Text, "签名");

        private void BtnCopyPlainData_Click(object sender, RoutedEventArgs e) => TryCopy(textPlainData.Text, "原始数据");

        private void BtnPastePlainData_Click(object sender, RoutedEventArgs e) => TryPaste(textPlainData, "原始数据");

        private void BtnClearPlainData_Click(object sender, RoutedEventArgs e) => TryClear(textPlainData, "原始数据");

        private void BtnCopySignatureData_Click(object sender, RoutedEventArgs e) => TryCopy(textSignature.Text, "签名");

        private void BtnPasteSignatureData_Click(object sender, RoutedEventArgs e) => TryPaste(textSignature, "签名");

        /// <summary>只清空"签名"框，对其它两个签名相关文本框互不干扰。</summary>
        private void BtnClearSignatureData_Click(object sender, RoutedEventArgs e)
        {
            _lastSignatureBytes = null; // 同步清缓存，避免下拉框切换时把已清空的签名框"复活"
            _lastSignatureR = null;
            _lastSignatureS = null;
            TryClear(textSignature, "签名");
        }

        /// <summary>只清空"Base64(Raw 二进制)"框，对其它两个签名相关文本框互不干扰。</summary>
        private void BtnClearSignatureRSBase64_Click(object sender, RoutedEventArgs e)
        {
            // 即使 R/S 框只是局部被清空，也清缓存，避免下拉框切换触发 RefreshSignatureTextboxes 把已清空的 R|S 框重新填回去
            _lastSignatureBytes = null;
            _lastSignatureR = null;
            _lastSignatureS = null;
            TryClear(textSignatureRSBase64, "R|S (Base64)");
        }

        /// <summary>只清空"Base64(DER 二进制)"框，对其它两个签名相关文本框互不干扰。</summary>
        private void BtnClearSignatureDerBase64_Click(object sender, RoutedEventArgs e)
        {
            // 同步清缓存，避免下拉框切换触发 RefreshSignatureTextboxes 把已清空的 DER 框重新填回去
            _lastSignatureBytes = null;
            _lastSignatureR = null;
            _lastSignatureS = null;
            TryClear(textSignatureDerBase64, "DER (Base64)");
        }

        /// <summary>
        /// 签名格式下拉框切换时，用最近一次签名缓存按新格式重新呈现签名框。
        /// 无缓存（用户尚未签名）时什么都不做。
        /// </summary>
        private void ComboSignatureFormat_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_lastSignatureBytes == null) return;
            RefreshSignatureTextboxes(_lastSignatureBytes, _lastSignatureR, _lastSignatureS);
        }

        /// <summary>
        /// 签名成功后统一刷新三个签名框，以及下拉框切换时刷新签名框本身。
        ///   签名框 textSignature       → 按下拉框所选格式呈现（Base64 / Hex / DER）；
        ///   textSignatureRSBase64     → 固定 Base64(r ‖ s)（路径一：Raw 编码，P-256 恒 64 字节）；
        ///   textSignatureDerBase64    → 固定 Base64(DER)（路径二：ASN.1 SEQUENCE，70~72 字节）。
        /// 同时刷新 _lastSignatureBytes / _lastSignatureR / _lastSignatureS 缓存。
        /// 注：三框之间不再联动（任一文本框被用户手动修改后不会被联动覆盖）。
        /// </summary>
        private void RefreshSignatureTextboxes(byte[] der, byte[]? r, byte[]? s)
        {
            _lastSignatureBytes = der;
            _lastSignatureR = r;
            _lastSignatureS = s;

            textSignature.Text = FormatSignatureBytes(der, GetSignatureFormat());
            textSignatureRSBase64.Text = (r != null && s != null)
                ? Convert.ToBase64String(ConcatRs(r, s))
                : Convert.ToBase64String(der);
            textSignatureDerBase64.Text = Convert.ToBase64String(der);
        }

        /// <summary>三框独立：TextChanged 联动已移除，按钮验签各自只读对应文本框，互不干扰。</summary>

        /// <summary>
        /// 按格式呈现签名字节：Base64 / Hex / DER（Hex 与 DER 视觉等价 Hex，语义标注不同）。
        /// R|S 形式已拆到下方的两个独立文本框（Base64(Raw 二进制) / Base64(DER 二进制)），不再经下拉框切换。
        /// </summary>
        private static string FormatSignatureBytes(byte[] signature, string format) => format switch
        {
            "Base64" => Convert.ToBase64String(signature),
            "Hex" or "DER" => Convert.ToHexString(signature).ToLowerInvariant(),
            _ => Convert.ToBase64String(signature)
        };

        /// <summary>
        /// 按格式解码签名框文本，给 BC 的 VerifySignature 使用 DER 字节。
        /// Base64 → Base64 解码；Hex / DER → 十六进制解码。
        /// </summary>
        private static byte[] DecodeSignatureForVerify(string text, string format) => format switch
        {
            "Base64" => Convert.FromBase64String(text),
            "Hex" or "DER" => Convert.FromHexString(text),
            _ => Convert.FromBase64String(text)
        };

        /// <summary>把 r 与 s 拼接成一段连续字节（调用方需保证两者等长）。</summary>
        private static byte[] ConcatRs(byte[] r, byte[] s)
        {
            var rs = new byte[r.Length + s.Length];
            Buffer.BlockCopy(r, 0, rs, 0, r.Length);
            Buffer.BlockCopy(s, 0, rs, r.Length, s.Length);
            return rs;
        }

        /// <summary>把大端字节补齐到 32 字节（R|S 要求定长，超长原样返回）。</summary>
        private static byte[] PadLeftTo32(byte[] src)
        {
            if (src.Length >= 32) return src;
            var padded = new byte[32];
            Array.Copy(src, 0, padded, 32 - src.Length, src.Length);
            return padded;
        }

        /// <summary>读取 ASN.1 DER 长度字段（短格式 / 长格式 0x80+n）。</summary>
        private static int ReadDerLength(byte[] data, ref int offset)
        {
            int b = data[offset++];
            if ((b & 0x80) == 0) return b;
            int n = b & 0x7F;
            int len = 0;
            for (int i = 0; i < n; i++) len = (len << 8) | data[offset++];
            return len;
        }

        /// <summary>
        /// 解析 ECDSA 的 ASN.1 DER 签名：SEQUENCE { INTEGER r, INTEGER s }。
        /// 只识别 BC 的 ECDSA 输出（30 02 len_r r-bytes 02 len_s s-bytes，或 30 81 ... 长格式 SEQUENCE）。
        /// </summary>
        private static (BigInteger r, BigInteger s) ParseDerSignature(byte[] der)
        {
            if (der == null || der.Length < 8 || der[0] != 0x30)
                throw new ArgumentException("不是合法的 DER 签名（缺少 SEQUENCE 头）");

            int offset = 1;
            int seqLen = ReadDerLength(der, ref offset);
            // 可选一致性检查：offset + seqLen == der.Length
            _ = seqLen;

            if (offset >= der.Length || der[offset] != 0x02)
                throw new ArgumentException("不是合法的 DER 签名（r 字段缺 INTEGER 头）");
            offset++;
            int rLen = ReadDerLength(der, ref offset);
            var rBytes = new byte[rLen];
            Array.Copy(der, offset, rBytes, 0, rLen);
            offset += rLen;

            if (offset >= der.Length || der[offset] != 0x02)
                throw new ArgumentException("不是合法的 DER 签名（s 字段缺 INTEGER 头）");
            offset++;
            int sLen = ReadDerLength(der, ref offset);
            var sBytes = new byte[sLen];
            Array.Copy(der, offset, sBytes, 0, sLen);

            return (new BigInteger(1, rBytes), new BigInteger(1, sBytes));
        }

        /// <summary>
        /// 把 64 字节 (r ‖ s) 拼装成 ASN.1 DER，让 BC 的 VerifySignature 能识别。
        /// 输入必须是定长 32 + 32 字节大端；r / s 最高位 ≥ 0x80 时自动补 00 防负数。
        /// </summary>
        private static byte[] RsToDer(byte[] rs)
        {
            if (rs == null || rs.Length != 64)
                throw new ArgumentException($"R|S 拼接应为 64 字节（32+32），实际 {(rs == null ? 0 : rs.Length)}");

            var rBytes = new byte[32];
            var sBytes = new byte[32];
            Buffer.BlockCopy(rs, 0, rBytes, 0, 32);
            Buffer.BlockCopy(rs, 32, sBytes, 0, 32);

            var rInt = ToDerIntegerBytes(rBytes);
            var sInt = ToDerIntegerBytes(sBytes);

            // SEQUENCE { INTEGER r, INTEGER s }，总长 ≤ 127 用短格式编码
            int contentLen = 2 + rInt.Length + 2 + sInt.Length;
            var der = new byte[2 + contentLen];
            int idx = 0;
            der[idx++] = 0x30;
            der[idx++] = (byte)contentLen;
            der[idx++] = 0x02;
            der[idx++] = (byte)rInt.Length;
            Array.Copy(rInt, 0, der, idx, rInt.Length);
            idx += rInt.Length;
            der[idx++] = 0x02;
            der[idx++] = (byte)sInt.Length;
            Array.Copy(sInt, 0, der, idx, sInt.Length);
            return der;
        }

        /// <summary>把 32 字节大端转成 ASN.1 INTEGER 字节（去前导零，高位 ≥0x80 时补 00）。</summary>
        private static byte[] ToDerIntegerBytes(byte[] b32)
        {
            // 去前导 0
            int start = 0;
            while (start < b32.Length - 1 && b32[start] == 0) start++;
            int len = b32.Length - start;
            var raw = new byte[len];
            Array.Copy(b32, start, raw, 0, len);

            // 高位 ≥ 0x80 时补 00，避免被解析为负数
            if ((raw[0] & 0x80) != 0)
            {
                var padded = new byte[raw.Length + 1];
                Array.Copy(raw, 0, padded, 1, raw.Length);
                return padded;
            }
            return raw;
        }

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

    }
}
