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

        private Dictionary<string, (string Icon, List<KeyValuePair<string, string>> Curves)> _allCurveData = [];

        private byte[]? _derivedEncKey = null;
        private byte[]? _lastEncIV = null;
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

                textEcdhAlicePrivate.Text = EcdsaKeyHelper.ExportPrivateKeyPem(alicePriv);
                textEcdhAlicePublic.Text = EcdsaKeyHelper.ExportPublicKeyPem(alicePub);
                textEcdhBobPrivate.Text = EcdsaKeyHelper.ExportPrivateKeyPem(bobPriv);
                textEcdhBobPublic.Text = EcdsaKeyHelper.ExportPublicKeyPem(bobPub);

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
                byte[] shared, iv;
                string output;

                if (mode != EcdhMode.GwifiOrg)
                {
                    // ECIES：发送方生成临时密钥对，只需接收方公钥
                    if (string.IsNullOrWhiteSpace(textEcdhBobPublic.Text))
                    { MessageBox.Show("请输入 Bob 公钥（接收方公钥）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

                    var bobPub = EcdsaKeyHelper.ImportPublicKeyPem(textEcdhBobPublic.Text.Trim());
                    byte[] combined = EcdhAlgorithm.EciesEncrypt(plain, bobPub, curve, mode, out shared, out iv);
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
                    output = EcdhAlgorithm.StaticEcdhEncrypt(plain, alicePriv, bobPub, mode, out shared, out iv);
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
                string cipherSource = string.IsNullOrWhiteSpace(textEcdhOutput.Text) ? textEcdhInput.Text.Trim() : textEcdhOutput.Text.Trim();
                if (string.IsNullOrWhiteSpace(cipherSource))
                { MessageBox.Show("请输入或粘贴要解密的 Base64 密文", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning); return; }

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
                    byte[] combined = Convert.FromBase64String(cipherSource);
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
            if (!string.IsNullOrEmpty(textEcdhOutput.Text))
            {
                Clipboard.SetText(textEcdhOutput.Text);
                SetStatus("ECDH 结果已复制");
            }
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

            radioPrivateKey.Checked = true;
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

                _privateKeyPem = EcdsaKeyHelper.ExportPrivateKeyPem(priv);
                _publicKeyPem = EcdsaKeyHelper.ExportPublicKeyPem(pub);

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

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(textPrivateKey.Text.Trim());

                // 提取公钥前先判断私钥曲线是否可识别
                string curveName = EcdsaKeyHelper.GetCurveName(priv);
                if (curveName == "未知曲线")
                {
                    AppendKeyResult("❌ 私钥生成逻辑非法，不符合标准 OpenSSL", Color.Red);
                    SetStatus("私钥曲线无法识别，不符合标准 OpenSSL");
                    return;
                }

                var pub = EcdsaAlgorithm.GetPublicKey(priv);

                _privateKeyPem = textPrivateKey.Text.Trim();
                _publicKeyPem = EcdsaKeyHelper.ExportPublicKeyPem(pub);

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

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(textPrivateKey.Text.Trim());
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

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(privPem);
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(pubPem);

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

                var priv = EcdsaKeyHelper.ImportPrivateKeyPem(textPrivateKey.Text.Trim());
                var pub = EcdsaKeyHelper.ImportPublicKeyPem(textPublicKey.Text.Trim());

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

                var pub = EcdsaKeyHelper.ImportPublicKeyPem(textPublicKey.Text.Trim());
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
                if (isPrivate) EcdsaKeyHelper.ImportPrivateKeyPem(c);
                else EcdsaKeyHelper.ImportPublicKeyPem(c);
                UIOutputFormat fmt = GetCurrentOutputFormat();
                if (isPrivate) { _privateKeyPem = c; textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, fmt); }
                else { _publicKeyPem = c; textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, fmt); }
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
            if (!string.IsNullOrEmpty(textSignature.Text)) { Clipboard.SetText(textSignature.Text); SetStatus("签名已复制到剪贴板"); }
        }

        private void BtnCopyPlainData_Click(object? s, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textPlainData.Text)) { Clipboard.SetText(textPlainData.Text); SetStatus("原始数据已复制到剪贴板"); }
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
            if (!string.IsNullOrEmpty(textSignature.Text)) { Clipboard.SetText(textSignature.Text); SetStatus("签名已复制到剪贴板"); }
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
            if (Clipboard.ContainsText()) { textPrivateKey.Text = Clipboard.GetText().Trim(); _privateKeyPem = textPrivateKey.Text; SetStatus("私钥已从剪贴板粘贴"); }
            else MessageBox.Show("剪贴板中没有文本内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnClearPrivateKey_Click(object? s, EventArgs e) { textPrivateKey.Clear(); _privateKeyPem = string.Empty; SetStatus("私钥已清空"); }

        private void BtnPastePublicKey_Click(object? s, EventArgs e)
        {
            if (Clipboard.ContainsText()) { textPublicKey.Text = Clipboard.GetText().Trim(); _publicKeyPem = textPublicKey.Text; SetStatus("公钥已从剪贴板粘贴"); }
            else MessageBox.Show("剪贴板中没有文本内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void BtnClearPublicKey_Click(object? s, EventArgs e) { textPublicKey.Clear(); _publicKeyPem = string.Empty; SetStatus("公钥已清空"); }

        private void BtnCopyPrivateKey_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textPrivateKey.Text))
            {
                Clipboard.SetText(textPrivateKey.Text);
                SetStatus("私钥已复制到剪贴板");
            }
            else
            {
                MessageBox.Show("私钥为空，无法复制！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetStatus("复制私钥失败：私钥为空");
            }
        }

        private void BtnCopyPublicKey_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textPublicKey.Text))
            {
                Clipboard.SetText(textPublicKey.Text);
                SetStatus("公钥已复制到剪贴板");
            }
            else
            {
                MessageBox.Show("公钥为空，无法复制！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                SetStatus("复制公钥失败：公钥为空");
            }
        }
        #endregion

        #region 控件事件
        private void ComboOutputFormat_SelectedIndexChanged(object? s, EventArgs e) => RefreshKeyDisplay();

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
        #endregion


        #region 加密与解密逻辑

        #region 基础工具方法
        private byte[] GetEncInputBytes()
        {
            string input = textEncInput.Text.Trim();
            string format = comboEncInputFormat.SelectedItem?.ToString() ?? "UTF-8文本";
            return format switch
            {
                "Base64" => Convert.FromBase64String(input),
                "Hex" => Convert.FromHexString(input),
                _ => Encoding.UTF8.GetBytes(input)
            };
        }

        private byte[] GetEncKey()
        {
            if (!string.IsNullOrWhiteSpace(textEncKey.Text))
            {
                byte[] key = Convert.FromHexString(textEncKey.Text.Trim());
                if (key.Length != 32)
                    throw new ArgumentException("对称密钥必须是32字节（256位）HEX字符串");
                return key;
            }

            if (string.IsNullOrWhiteSpace(_privateKeyPem))
                throw new InvalidOperationException("请先生成/导入ECDSA私钥，或手动填写对称密钥");

            var privateKey = EcdsaKeyHelper.ImportPrivateKeyPem(_privateKeyPem);
            byte[] rawKey = privateKey.D.ToByteArrayUnsigned();

            var hkdf = new HkdfBytesGenerator(new Sha256Digest());
            hkdf.Init(new HkdfParameters(rawKey, null, Encoding.UTF8.GetBytes("CryptoTool-ECDSA-EncKey")));
            byte[] derivedKey = new byte[32];
            hkdf.GenerateBytes(derivedKey, 0, derivedKey.Length);
            _derivedEncKey = derivedKey;
            return derivedKey;
        }

        private byte[] GetEncIV(string mode)
        {
            if (!string.IsNullOrWhiteSpace(textEncIV.Text))
                return Convert.FromHexString(textEncIV.Text.Trim());

            int ivLen = mode switch
            {
                "AES-256-CBC" => 16,
                _ => 12
            };
            byte[] iv = RandomNumberGenerator.GetBytes(ivLen);
            _lastEncIV = iv;
            return iv;
        }
        #endregion

        #region 加密算法实现

        private static byte[] EncryptAesGcm(byte[] plain, byte[] key, byte[] nonce)
        {
            using var aes = new AesGcm(key, 16);
            byte[] cipher = new byte[plain.Length];
            byte[] tag = new byte[16];
            aes.Encrypt(nonce, plain, cipher, tag);
            byte[] result = new byte[cipher.Length + tag.Length];
            Buffer.BlockCopy(cipher, 0, result, 0, cipher.Length);
            Buffer.BlockCopy(tag, 0, result, cipher.Length, tag.Length);
            return result;
        }

        private static byte[] DecryptAesGcm(byte[] cipherWithTag, byte[] key, byte[] nonce)
        {
            byte[] cipher = cipherWithTag.AsSpan(0, cipherWithTag.Length - 16).ToArray();
            byte[] tag = cipherWithTag.AsSpan(cipherWithTag.Length - 16, 16).ToArray();
            byte[] plain = new byte[cipher.Length];
            using var aes = new AesGcm(key, 16);
            aes.Decrypt(nonce, cipher, tag, plain);
            return plain;
        }

        private static byte[] EncryptAesCbc(byte[] plain, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var encryptor = aes.CreateEncryptor();
            return encryptor.TransformFinalBlock(plain, 0, plain.Length);
        }

        private static byte[] DecryptAesCbc(byte[] cipher, byte[] key, byte[] iv)
        {
            using var aes = Aes.Create();
            aes.Key = key;
            aes.IV = iv;
            aes.Mode = CipherMode.CBC;
            aes.Padding = PaddingMode.PKCS7;
            using var decryptor = aes.CreateDecryptor();
            return decryptor.TransformFinalBlock(cipher, 0, cipher.Length);
        }

        private static byte[] EncryptChaCha20(byte[] plain, byte[] key, byte[] nonce)
        {
            using var chacha = new ChaCha20Poly1305(key);
            byte[] cipher = new byte[plain.Length];
            byte[] tag = new byte[16];
            chacha.Encrypt(nonce, plain, cipher, tag);
            byte[] result = new byte[cipher.Length + tag.Length];
            Buffer.BlockCopy(cipher, 0, result, 0, cipher.Length);
            Buffer.BlockCopy(tag, 0, result, cipher.Length, tag.Length);
            return result;
        }

        private static byte[] DecryptChaCha20(byte[] cipherWithTag, byte[] key, byte[] nonce)
        {
            byte[] cipher = cipherWithTag.AsSpan(0, cipherWithTag.Length - 16).ToArray();
            byte[] tag = cipherWithTag.AsSpan(cipherWithTag.Length - 16, 16).ToArray();
            byte[] plain = new byte[cipher.Length];
            using var chacha = new ChaCha20Poly1305(key);
            chacha.Decrypt(nonce, cipher, tag, plain);
            return plain;
        }
        #endregion

        #region 按钮事件：加密/解密/清空/复制/粘贴

        private void BtnEncrypt_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEncInput.Text))
                {
                    MessageBox.Show("请输入要加密的内容！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM)";
                byte[] plain = GetEncInputBytes();
                byte[] key = GetEncKey();
                byte[] iv = GetEncIV(mode);

                byte[] cipher = mode switch
                {
                    "AES-256-GCM" => EncryptAesGcm(plain, key, iv),
                    "AES-256-CBC" => EncryptAesCbc(plain, key, iv),
                    "ChaCha20-Poly1305" => EncryptChaCha20(plain, key, iv),
                    _ => EncryptAesGcm(plain, key, iv)
                };

                textEncIV.Text = Convert.ToHexString(iv).ToLowerInvariant();
                string outFormat = comboEncOutputFormat.SelectedItem?.ToString() ?? "Base64";
                textEncOutput.Text = outFormat == "Hex"
                    ? Convert.ToHexString(cipher).ToLowerInvariant()
                    : Convert.ToBase64String(cipher);

                AppendValidationResult($"✅ 加密成功\r\n算法: {mode}\r\nIV: {Convert.ToHexString(iv).ToLowerInvariant()}\r\n长度: {cipher.Length}字节", Color.Green);
                SetStatus("加密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 加密失败: {ex.Message}", Color.Red);
                SetStatus("加密失败");
            }
        }

        private void BtnDecrypt_Click(object? sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textEncInput.Text))
                {
                    MessageBox.Show("请输入要解密的密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM)";
                byte[] cipher = GetEncInputBytes();
                byte[] key = GetEncKey();
                byte[] iv = GetEncIV(mode);

                byte[] plain = mode switch
                {
                    "AES-256-GCM" => DecryptAesGcm(cipher, key, iv),
                    "AES-256-CBC" => DecryptAesCbc(cipher, key, iv),
                    "ChaCha20-Poly1305" => DecryptChaCha20(cipher, key, iv),
                    _ => DecryptAesGcm(cipher, key, iv)
                };

                textEncOutput.Text = Encoding.UTF8.GetString(plain);
                AppendValidationResult($"✅ 解密成功\r\n算法: {mode}\r\n明文长度: {plain.Length}字节", Color.Green);
                SetStatus("解密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 解密失败: {ex.Message}", Color.Red);
                SetStatus("解密失败");
            }
        }

        private void BtnEncClear_Click(object? sender, EventArgs e)
        {
            textEncInput.Clear();
            textEncOutput.Clear();
            textEncKey.Clear();
            textEncIV.Clear();
            _derivedEncKey = null;
            _lastEncIV = null;
        }

        private void BtnEncCopy_Click(object? sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(textEncOutput.Text))
            {
                Clipboard.SetText(textEncOutput.Text);
                SetStatus("加密结果已复制");
            }
        }

        private void BtnEncPaste_Click(object? sender, EventArgs e)
        {
            if (Clipboard.ContainsText())
            {
                textEncInput.Text = Clipboard.GetText().Trim();
                SetStatus("内容已粘贴到输入框");
            }
        }
        #endregion

        #region 按钮事件：文件加密/解密

        private void BtnEncryptFile_Click(object? sender, EventArgs e)
        {
            try
            {
                using var openDlg = new OpenFileDialog { Title = "选择要加密的文件", Filter = "所有文件|*.*" };
                if (openDlg.ShowDialog() != DialogResult.OK) return;

                using var saveDlg = new SaveFileDialog
                {
                    Title = "保存加密文件",
                    Filter = "加密文件|*.enc",
                    FileName = Path.GetFileNameWithoutExtension(openDlg.FileName) + ".enc"
                };
                if (saveDlg.ShowDialog() != DialogResult.OK) return;

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM)";
                byte[] key = GetEncKey();
                byte[] iv = RandomNumberGenerator.GetBytes(mode == "AES-256-CBC" ? 16 : 12);
                byte[] plain = File.ReadAllBytes(openDlg.FileName);
                byte[] cipher = mode switch
                {
                    "AES-256-GCM" => EncryptAesGcm(plain, key, iv),
                    "AES-256-CBC" => EncryptAesCbc(plain, key, iv),
                    "ChaCha20-Poly1305" => EncryptChaCha20(plain, key, iv),
                    _ => EncryptAesGcm(plain, key, iv)
                };

                using var fs = new FileStream(saveDlg.FileName, FileMode.Create);
                fs.Write(iv, 0, iv.Length);
                fs.Write(cipher, 0, cipher.Length);

                AppendValidationResult($"✅ 文件加密成功\r\n{Path.GetFileName(openDlg.FileName)} → {Path.GetFileName(saveDlg.FileName)}", Color.Green);
                SetStatus("文件加密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 文件加密失败: {ex.Message}", Color.Red);
                SetStatus("文件加密失败");
            }
        }

        private void BtnDecryptFile_Click(object? sender, EventArgs e)
        {
            try
            {
                using var openDlg = new OpenFileDialog
                {
                    Title = "选择要解密的加密文件",
                    Filter = "加密文件|*.enc|所有文件|*.*"
                };
                if (openDlg.ShowDialog() != DialogResult.OK) return;

                using var saveDlg = new SaveFileDialog
                {
                    Title = "保存解密文件",
                    Filter = "所有文件|*.*",
                    FileName = Path.GetFileNameWithoutExtension(openDlg.FileName)
                };
                if (saveDlg.ShowDialog() != DialogResult.OK) return;

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM)";
                byte[] key = GetEncKey();
                byte[] allBytes = File.ReadAllBytes(openDlg.FileName);
                int ivLen = mode == "AES-256-CBC" ? 16 : 12;
                byte[] iv = [.. allBytes.Take(ivLen)];
                byte[] cipher = [.. allBytes.Skip(ivLen)];

                byte[] plain = mode switch
                {
                    "AES-256-GCM" => DecryptAesGcm(cipher, key, iv),
                    "AES-256-CBC" => DecryptAesCbc(cipher, key, iv),
                    "ChaCha20-Poly1305" => DecryptChaCha20(cipher, key, iv),
                    _ => DecryptAesGcm(cipher, key, iv)
                };

                File.WriteAllBytes(saveDlg.FileName, plain);
                textEncIV.Text = Convert.ToHexString(iv).ToLowerInvariant();
                AppendValidationResult($"✅ 文件解密成功\r\n{Path.GetFileName(openDlg.FileName)} → {Path.GetFileName(saveDlg.FileName)}", Color.Green);
                SetStatus("文件解密完成");
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 文件解密失败: {ex.Message}", Color.Red);
                SetStatus("文件解密失败");
            }
        }
        #endregion

        #endregion

        private void LabelCurveHeader_Click(object sender, EventArgs e)
        {

        }

        private void LabelCurve_Click(object sender, EventArgs e)
        {

        }
    }
}
