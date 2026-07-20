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
        private System.Windows.Forms.Label labelEncTest = null!;
        private System.Windows.Forms.TextBox textEncTest = null!;
        #endregion

        #region 初始化

        /// <summary>
        /// 初始化加密/解密控件的非布局属性（文本、字体、事件绑定等）。
        /// 必须在 InitializeComponent() 之后、InitializeEncryptLayout() 之前调用。
        /// </summary>
        private void InitializeEncryptControlDefaults()
        {
            labelEncTest = new Label();
            textEncTest = new TextBox();

            // ---- 标签文本 ----
            labelEncMode.Text = "加密模式：";
            labelEncInputFormat.Text = "输入格式：";
            labelEncOutputFormat.Text = "输出格式：";
            labelEncKey.Text = "对称密钥 (HEX，留空自动派生)：";
            labelEncIV.Text = "IV/Nonce (HEX，留空随机生成)：";
            labelEncBobPublic.Text = "Bob 公钥 (接收方)：";
            labelEncTest.Text = "测试：";
            labelEncInput.Text = "明文 / 密文输入：";
            labelEncOutputLabel.Text = "加密结果 / 解密输入：";

            // ---- 加密模式下拉框 ----
            comboEncMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncMode.FormattingEnabled = true;
            comboEncMode.Items.AddRange(["ECIES (ECDH+AES-GCM)", "AES-256-GCM", "AES-256-CBC", "ChaCha20-Poly1305"]);

            // ---- 输入格式下拉框 ----
            comboEncInputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncInputFormat.FormattingEnabled = true;
            comboEncInputFormat.Items.AddRange(["UTF-8文本", "Base64", "Hex"]);

            // ---- 输出格式下拉框 ----
            comboEncOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncOutputFormat.FormattingEnabled = true;
            comboEncOutputFormat.Items.AddRange(["Base64", "Hex"]);

            // ---- 曲线分类下拉框 ----
            comboEncCurveCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncCurveCategory.FormattingEnabled = true;

            // ---- 具体曲线下拉框 ----
            comboEncCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncCurve.FormattingEnabled = true;

            // ---- 对称密钥输入框 ----
            textEncKey.Font = new System.Drawing.Font("Consolas", 9F);
            textEncKey.Multiline = true;
            textEncKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // ---- IV/Nonce 输入框 ----
            textEncIV.Font = new System.Drawing.Font("Consolas", 9F);
            textEncIV.Multiline = true;
            textEncIV.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // ---- Bob 公钥输入框 ----
            textEncBobPublic.Font = new System.Drawing.Font("Consolas", 9F);
            textEncBobPublic.Multiline = true;
            textEncBobPublic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // ---- 新增测试输入框 ----
            textEncTest.Font = new System.Drawing.Font("Consolas", 9F);
            textEncTest.Multiline = true;
            textEncTest.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // ---- 明文/密文输入框 ----
            textEncInput.Font = new System.Drawing.Font("Consolas", 9F);
            textEncInput.Multiline = true;
            textEncInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // ---- 加密结果/解密输入框 ----
            textEncOutput.Font = new System.Drawing.Font("Consolas", 9F);
            textEncOutput.Multiline = true;
            textEncOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;

            // ---- 按钮文本和事件绑定 ----
            btnEncrypt.Text = "加密";
            btnEncrypt.Click += BtnEncrypt_Click;
            btnDecrypt.Text = "解密";
            btnDecrypt.Click += BtnDecrypt_Click;
            btnEncClear.Text = "清空";
            btnEncClear.Click += BtnEncClear_Click;
            btnEncCopy.Text = "复制结果";
            btnEncCopy.Click += BtnEncCopy_Click;
            btnEncPaste.Text = "粘贴输入";
            btnEncPaste.Click += BtnEncPaste_Click;
        }

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
                // 4行: 输入标签 | 输入框 | 输出标签 | 输出框
                // 2列: 左列100%放标签+文本框 | 右列80px放复制/粘贴按钮组
                var leftGroup = new GroupBox
                {
                    Text = "加密 / 解密",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var leftLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 2,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                // ---- 局部函数: 创建带有右上角标签的输入面板 ----
                Panel CreateTextPanelWithLabel(TextBox textBox, string labelText)
                {
                    var panel = new Panel { Dock = DockStyle.Fill, Margin = new Padding(0) };
                    textBox.Dock = DockStyle.Fill;
                    panel.Controls.Add(textBox);

                    var lbl = new Label
                    {
                        Text = labelText,
                        AutoSize = true,
                        BackColor = Color.Transparent,
                        ForeColor = Color.Red,
                        Padding = new Padding(0, 2, 6, 0)
                    };
                    panel.Controls.Add(lbl);
                    lbl.BringToFront();

                    panel.Layout += (s, e) =>
                    {
                        lbl.Location = new Point(panel.ClientSize.Width - lbl.PreferredWidth - 2, 2);
                    };
                    panel.Resize += (s, e) =>
                    {
                        lbl.Location = new Point(panel.ClientSize.Width - lbl.PreferredWidth - 2, 2);
                    };

                    return panel;
                }

                // 输入面板（文本框 + 右上角标签）
                var inputPanel = CreateTextPanelWithLabel(textEncInput, labelEncInput.Text);
                leftLayout.Controls.Add(inputPanel, 0, 0);

                // 输出面板（文本框 + 右上角标签）
                var outputPanel = CreateTextPanelWithLabel(textEncOutput, labelEncOutputLabel.Text);
                leftLayout.Controls.Add(outputPanel, 0, 1);

                // ---- 局部函数: 为输入框创建复制/粘贴按钮面板 ----
                // 每个面板包含2个按钮(复制/粘贴)，绑定到目标 TextBox
                TableLayoutPanel CreateButtonPanel(TextBox target)
                {
                    var panel = new TableLayoutPanel
                    {
                        Dock = DockStyle.Fill,
                        ColumnCount = 1,
                        RowCount = 2,
                        Margin = new Padding(4, 0, 0, 0),
                        Padding = new Padding(0)
                    };
                    panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                    panel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                    var btnCopy = new Button
                    {
                        Text = "复制",
                        ForeColor = Color.Red,
                        Dock = DockStyle.Fill,
                        Padding = new Padding(2),
                        Margin = new Padding(0, 1, 0, 1)
                    };
                    var btnPaste = new Button
                    {
                        Text = "粘贴",
                        ForeColor = Color.Red,
                        Dock = DockStyle.Fill,
                        Padding = new Padding(2),
                        Margin = new Padding(0, 1, 0, 1)
                    };
                    btnCopy.Click += (s, e) =>
                    {
                        TrySetClipboardText(target.Text, "加密结果已复制", "复制内容为空");
                    };
                    btnPaste.Click += (s, e) => { if (Clipboard.ContainsText()) target.Text = Clipboard.GetText().Trim(); };
                    panel.Controls.Add(btnCopy, 0, 0);
                    panel.Controls.Add(btnPaste, 0, 1);
                    return panel;
                }

                var inputBtnPanel = CreateButtonPanel(textEncInput);
                var outputBtnPanel = CreateButtonPanel(textEncOutput);

                leftLayout.Controls.Add(inputBtnPanel, 1, 0);
                leftLayout.Controls.Add(outputBtnPanel, 1, 1);

                leftGroup.Controls.Add(leftLayout);

                // --------------------- 右栏: 操作与配置区 ---------------------
                // 操作区使用左侧按钮列 + 右侧配置行的布局，参考密钥操作区 tableRightActions
                var actionGroup = new GroupBox
                {
                    Text = "操作",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var actionLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 1,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                actionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 150F));
                actionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                actionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

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
                actionLayout.Controls.Add(buttonPanel, 0, 0);

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
                actionLayout.Controls.Add(configPanel, 1, 0);

                actionGroup.Controls.Add(actionLayout);

                // --------------------- 中栏: 参数配置区 ---------------------
                // 3行设置: 对称密钥 | IV向量 | Bob 公钥
                var paramGroup = new GroupBox
                {
                    Text = "参数",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var paramLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 5,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                paramLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                paramLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                paramLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                paramLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                paramLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));

                paramLayout.Controls.Add(CreateTextPanelWithLabel(textEncKey, labelEncKey.Text), 0, 0);
                paramLayout.Controls.Add(CreateTextPanelWithLabel(textEncIV, labelEncIV.Text), 0, 1);
                paramLayout.Controls.Add(CreateTextPanelWithLabel(textEncBobPublic, labelEncBobPublic.Text), 0, 2);
                paramLayout.Controls.Add(CreateTextPanelWithLabel(textEncTest, labelEncTest.Text), 0, 3);

                // 非 ECIES 模式时隐藏 Bob 公钥；曲线选择保持与 ECDH 区一致
                textEncBobPublic.Visible = true;
                labelEncCurve.Visible = true; comboEncCurveCategory.Visible = true; labelEncCurveArrow.Visible = true; comboEncCurve.Visible = true;

                paramGroup.Controls.Add(paramLayout);

                // 左: 输入输出 | 中: 参数 | 右: 操作
                tableLayoutEncrypt.Controls.Add(leftGroup, 0, 0);
                tableLayoutEncrypt.Controls.Add(paramGroup, 1, 0);
                tableLayoutEncrypt.Controls.Add(actionGroup, 2, 0);

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
