using System;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Forms;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;

namespace CryptoTool.Win
{
    public partial class EcdsaTabControl
    {
        // ═══════════════════════════════════════════════════════════════════
        // ECIES 加密解密板块
        // ═══════════════════════════════════════════════════════════════════

        #region 字段
        private byte[]? _derivedEncKey = null;
        private byte[]? _lastEncIV = null;
        #endregion

        #region 初始化
        private void InitializeEncryptDefaults()
        {
            if (comboEncMode.Items.Count > 0)
                comboEncMode.SelectedIndex = 0;
            if (comboEncInputFormat.Items.Count > 0)
                comboEncInputFormat.SelectedIndex = 0;
            if (comboEncOutputFormat.Items.Count > 0)
                comboEncOutputFormat.SelectedIndex = 0;

            InitializeEncryptCurveList();
        }

        private void ComboEncCurveCategory_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (sender == null || comboEncCurveCategory.SelectedItem == null)
                return;

            dynamic selectedItem = comboEncCurveCategory.SelectedItem;
            string categoryKey = selectedItem.Value;

            if (!_allCurveData.TryGetValue(categoryKey, out var categoryData))
                return;

            comboEncCurve.Items.Clear();
            foreach (var c in categoryData.Curves)
                comboEncCurve.Items.Add(c);

            if (comboEncCurve.Items.Count > 0)
                comboEncCurve.SelectedIndex = 0;
        }
        #endregion

        #region UI 布局辅助
        private static TableLayoutPanel CreateLabelControlRow(Label label, Control control)
        {
            var panel = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 1,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            panel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            label.AutoSize = true;
            label.Dock = DockStyle.Fill;
            label.TextAlign = ContentAlignment.MiddleLeft;
            label.Margin = new Padding(0, 0, 4, 0);

            control.Dock = DockStyle.Fill;
            control.Margin = new Padding(0);

            panel.Controls.Add(label, 0, 0);
            panel.Controls.Add(control, 1, 0);
            return panel;
        }

        private static FlowLayoutPanel CreateFormatRow(Label inputLabel, ComboBox inputCombo, Label outputLabel, ComboBox outputCombo)
        {
            var panel = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0),
                Padding = new Padding(0),
                WrapContents = false
            };

            inputLabel.AutoSize = true;
            inputLabel.Margin = new Padding(0, 0, 2, 0);
            inputLabel.TextAlign = ContentAlignment.MiddleLeft;
            inputCombo.Margin = new Padding(0, 0, 16, 0);
            inputCombo.Width = 120;
            inputCombo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            outputLabel.AutoSize = true;
            outputLabel.Margin = new Padding(0, 0, 2, 0);
            outputLabel.TextAlign = ContentAlignment.MiddleLeft;
            outputCombo.Margin = new Padding(0);
            outputCombo.Width = 120;
            outputCombo.Anchor = AnchorStyles.Left | AnchorStyles.Right;

            panel.Controls.Add(inputLabel);
            panel.Controls.Add(inputCombo);
            panel.Controls.Add(outputLabel);
            panel.Controls.Add(outputCombo);
            return panel;
        }
        #endregion

        #region 加解密布局初始化
        public void InitializeEncryptLayout()
        {
            tableLayoutEncrypt.SuspendLayout();
            try
            {
                // 清理旧布局
                tableLayoutEncrypt.Controls.Clear();
                tableLayoutEncrypt.ColumnStyles.Clear();
                tableLayoutEncrypt.RowStyles.Clear();

                // 设置三栏比例: 左30% | 中30% | 右40%
                tableLayoutEncrypt.ColumnCount = 3;
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                tableLayoutEncrypt.RowCount = 1;
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                // 默认隐藏加密结果提示标签
                labelEncResult.Visible = false;
                labelEncResult.Enabled = false;

                // --------------------- 左栏: 加密/解密输入输出区 ---------------------
                var leftGroup = new GroupBox
                {
                    Text = "加密 / 解密",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var leftLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 2,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                // 输入框容器，标签与粘贴按钮悬浮在文本框内部右侧
                var inputPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                textEncInput.Dock = DockStyle.Fill;
                inputPanel.Controls.Add(textEncInput);

                // 输入框标签
                labelEncInput.Dock = DockStyle.None;
                labelEncInput.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                labelEncInput.AutoSize = true;
                labelEncInput.BackColor = Color.Transparent;
                labelEncInput.Location = new Point(0, 0);
                labelEncInput.Margin = new Padding(0);
                labelEncInput.Padding = new Padding(4, 0, 4, 0);
                labelEncInput.TextAlign = ContentAlignment.MiddleRight;
                inputPanel.Controls.Add(labelEncInput);
                labelEncInput.BringToFront();

                // 粘贴按钮
                btnEncPaste.Dock = DockStyle.None;
                btnEncPaste.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnEncPaste.Size = new Size(50, 22);
                btnEncPaste.Location = new Point(0, labelEncInput.Height);
                btnEncPaste.Margin = new Padding(0);
                btnEncPaste.Text = "粘贴";
                btnEncPaste.AutoSize = false;
                inputPanel.Controls.Add(btnEncPaste);
                btnEncPaste.BringToFront();
                leftLayout.Controls.Add(inputPanel, 0, 0);

                // 输出框容器，标签与复制按钮悬浮在文本框内部右侧
                var outputPanel = new Panel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                textEncOutput.Dock = DockStyle.Fill;
                outputPanel.Controls.Add(textEncOutput);

                // 输出框标签
                labelEncOutputLabel.Dock = DockStyle.None;
                labelEncOutputLabel.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                labelEncOutputLabel.AutoSize = true;
                labelEncOutputLabel.BackColor = Color.Transparent;
                labelEncOutputLabel.Location = new Point(0, 0);
                labelEncOutputLabel.Margin = new Padding(0);
                labelEncOutputLabel.Padding = new Padding(4, 0, 4, 0);
                labelEncOutputLabel.TextAlign = ContentAlignment.MiddleRight;
                outputPanel.Controls.Add(labelEncOutputLabel);
                labelEncOutputLabel.BringToFront();

                // 复制按钮
                btnEncCopy.Dock = DockStyle.None;
                btnEncCopy.Anchor = AnchorStyles.Top | AnchorStyles.Right;
                btnEncCopy.Size = new Size(50, 22);
                btnEncCopy.Location = new Point(0, labelEncOutputLabel.Height);
                btnEncCopy.Margin = new Padding(0);
                btnEncCopy.Text = "复制";
                btnEncCopy.AutoSize = false;
                outputPanel.Controls.Add(btnEncCopy);
                btnEncCopy.BringToFront();
                leftLayout.Controls.Add(outputPanel, 0, 1);

                leftGroup.Controls.Add(leftLayout);

                // --------------------- 右栏: 操作与配置区 ---------------------
                // 操作区使用左侧按钮列 + 右侧配置行的布局，参考密钥操作区 tableRightActions
                var middleGroup = new GroupBox
                {
                    Text = "操作",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var middleLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                middleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
                middleLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                middleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                // 左侧按钮列
                var buttonPanel = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.TopDown,
                    WrapContents = false,
                    Margin = new Padding(0),
                    Padding = new Padding(0, 4, 6, 0)
                };
                Button[] buttons = [btnEncrypt, btnDecrypt, btnEncClear];
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].AutoSize = false;
                    buttons[i].Height = 28;
                    buttons[i].Width = 140;
                    buttons[i].Margin = new Padding(0, 2, 0, 2);
                    buttonPanel.Controls.Add(buttons[i]);
                }
                middleLayout.Controls.Add(buttonPanel, 0, 0);

                // 右侧配置项：每行一个标签 + 控件，冒号后对齐
                var configPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 4,
                    Margin = new Padding(0),
                    Padding = new Padding(0, 4, 0, 0)
                };
                configPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 120F));
                configPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                for (int i = 0; i < 4; i++)
                    configPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));

                labelEncMode.AutoSize = false;
                labelEncMode.Size = new Size(120, 28);
                labelEncMode.Margin = new Padding(0, 4, 4, 4);
                labelEncMode.TextAlign = ContentAlignment.MiddleRight;
                comboEncMode.Margin = new Padding(0, 3, 0, 3);
                comboEncMode.Width = 300;
                comboEncMode.Anchor = AnchorStyles.Left;

                labelEncInputFormat.AutoSize = false;
                labelEncInputFormat.Size = new Size(120, 28);
                labelEncInputFormat.Margin = new Padding(0, 4, 4, 4);
                labelEncInputFormat.TextAlign = ContentAlignment.MiddleRight;
                comboEncInputFormat.Margin = new Padding(0, 3, 0, 3);
                comboEncInputFormat.Width = 140;
                comboEncInputFormat.Anchor = AnchorStyles.Left;

                labelEncOutputFormat.AutoSize = false;
                labelEncOutputFormat.Size = new Size(120, 28);
                labelEncOutputFormat.Margin = new Padding(0, 4, 4, 4);
                labelEncOutputFormat.TextAlign = ContentAlignment.MiddleRight;
                comboEncOutputFormat.Margin = new Padding(0, 3, 0, 3);
                comboEncOutputFormat.Width = 140;
                comboEncOutputFormat.Anchor = AnchorStyles.Left;

                labelEncCurve.AutoSize = false;
                labelEncCurve.Size = new Size(120, 28);
                labelEncCurve.Margin = new Padding(0, 4, 4, 4);
                labelEncCurve.Text = "椭圆曲线：";
                labelEncCurve.TextAlign = ContentAlignment.MiddleRight;
                comboEncCurveCategory.Margin = new Padding(0, 3, 2, 3);
                comboEncCurveCategory.Width = 130;
                comboEncCurveCategory.Anchor = AnchorStyles.Left;
                labelEncCurveArrow.AutoSize = true;
                labelEncCurveArrow.Margin = new Padding(0, 6, 2, 0);
                labelEncCurveArrow.Text = "→";
                labelEncCurveArrow.TextAlign = ContentAlignment.MiddleCenter;
                comboEncCurve.Margin = new Padding(0, 3, 0, 3);
                comboEncCurve.Dock = DockStyle.Fill;
                comboEncCurve.Anchor = AnchorStyles.Left | AnchorStyles.Right;
                var curveRow = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(0),
                    Padding = new Padding(0),
                    ColumnCount = 3,
                    RowCount = 1
                };
                curveRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                curveRow.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
                curveRow.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                curveRow.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
                curveRow.Controls.Add(comboEncCurveCategory, 0, 0);
                curveRow.Controls.Add(labelEncCurveArrow, 1, 0);
                curveRow.Controls.Add(comboEncCurve, 2, 0);

                configPanel.Controls.Add(labelEncMode, 0, 0);
                configPanel.Controls.Add(comboEncMode, 1, 0);
                configPanel.Controls.Add(labelEncInputFormat, 0, 1);
                configPanel.Controls.Add(comboEncInputFormat, 1, 1);
                configPanel.Controls.Add(labelEncOutputFormat, 0, 2);
                configPanel.Controls.Add(comboEncOutputFormat, 1, 2);
                configPanel.Controls.Add(labelEncCurve, 0, 3);
                configPanel.Controls.Add(curveRow, 1, 3);
                middleLayout.Controls.Add(configPanel, 1, 0);

                middleGroup.Controls.Add(middleLayout);

                // --------------------- 中栏: 参数配置区 ---------------------
                // 3行设置: 对称密钥 | IV向量 | Bob 公钥
                var rightGroup = new GroupBox
                {
                    Text = "参数",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var rightLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 4,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 72F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                rightLayout.Controls.Add(CreateLabelControlRow(labelEncKey, textEncKey), 0, 0);
                rightLayout.Controls.Add(CreateLabelControlRow(labelEncIV, textEncIV), 0, 1);
                rightLayout.Controls.Add(CreateLabelControlRow(labelEncBobPublic, textEncBobPublic), 0, 2);

                // 非 ECIES 模式时隐藏 Bob 公钥；曲线选择保持与 ECDH 区一致
                labelEncBobPublic.Visible = false; textEncBobPublic.Visible = false;
                labelEncCurve.Visible = true; comboEncCurve.Visible = true;

                rightGroup.Controls.Add(rightLayout);

                // 左: 输入输出 | 中: 参数 | 右: 操作
                tableLayoutEncrypt.Controls.Add(leftGroup, 0, 0);
                tableLayoutEncrypt.Controls.Add(rightGroup, 1, 0);
                tableLayoutEncrypt.Controls.Add(middleGroup, 2, 0);

                groupEncFile.Visible = false;
                groupEncFile.Enabled = false;
            }
            finally
            {
                tableLayoutEncrypt.ResumeLayout(true);
            }
        }

        public void InitializeEncryptCurveList()
        {
            if (_allCurveData.Count == 0)
                _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

            comboEncCurveCategory.DisplayMember = "Text";
            comboEncCurveCategory.ValueMember = "Value";
            foreach (var cat in _allCurveData)
            {
                comboEncCurveCategory.Items.Add(new
                {
                    Text = $"{cat.Value.Icon} {cat.Key}",
                    Value = cat.Key
                });
            }

            comboEncCurve.DisplayMember = "Value";
            comboEncCurve.ValueMember = "Key";

            comboEncCurveCategory.SelectedIndexChanged += ComboEncCurveCategory_SelectedIndexChanged;

            if (comboEncCurveCategory.Items.Count > 0)
                comboEncCurveCategory.SelectedIndex = 0;
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

        /// <summary>
        /// 解析加密结果框中的密文，按输出格式（Base64/Hex）解码。
        /// </summary>
        private byte[] GetCipherBytes(string cipherText)
        {
            string outFormat = comboEncOutputFormat.SelectedItem?.ToString() ?? "Base64";
            if (outFormat == "Hex")
            {
                return Convert.FromHexString(cipherText.Trim().Replace("0x", "").Replace("0X", "").Replace(" ", ""));
            }
            else
            {
                return Convert.FromBase64String(cipherText.Trim());
            }
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

        private byte[] GetEncIV(string mode, bool forEncryption)
        {
            int ivLen = mode switch
            {
                "AES-256-CBC" => 16,
                _ => 12
            };

            if (!string.IsNullOrWhiteSpace(textEncIV.Text))
            {
                string currentHex = textEncIV.Text.Trim().ToLowerInvariant();

                // 解密时直接使用 IV 框中的内容
                if (!forEncryption)
                    return Convert.FromHexString(currentHex);

                // 加密时，只有与上次自动生成的 IV 不同时才视为用户手动指定
                string lastHex = _lastEncIV != null ? Convert.ToHexString(_lastEncIV).ToLowerInvariant() : string.Empty;
                if (currentHex != lastHex)
                {
                    return Convert.FromHexString(currentHex);
                }
            }

            // 解密时 IV 框不能为空
            if (!forEncryption)
                throw new ArgumentException("解密时必须提供 IV（HEX 格式），请从运行结果中复制加密时使用的 IV");

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
                byte[] iv = GetEncIV(mode, true);

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
                // 解密优先从"加密结果 / 解密输入"框读取密文，方便用户直接编辑密文后解密
                string cipherSource = string.IsNullOrWhiteSpace(textEncOutput.Text)
                    ? textEncInput.Text.Trim()
                    : textEncOutput.Text.Trim();

                if (string.IsNullOrWhiteSpace(cipherSource))
                {
                    MessageBox.Show("请输入要解密的密文！", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM)";
                byte[] cipher = GetCipherBytes(cipherSource);
                byte[] key = GetEncKey();
                byte[] iv = GetEncIV(mode, false);

                byte[] plain = mode switch
                {
                    "AES-256-GCM" => DecryptAesGcm(cipher, key, iv),
                    "AES-256-CBC" => DecryptAesCbc(cipher, key, iv),
                    "ChaCha20-Poly1305" => DecryptChaCha20(cipher, key, iv),
                    _ => DecryptAesGcm(cipher, key, iv)
                };

                textEncInput.Text = Encoding.UTF8.GetString(plain);
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
            TrySetClipboardText(textEncOutput.Text, "加密结果已复制");
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
    }
}
