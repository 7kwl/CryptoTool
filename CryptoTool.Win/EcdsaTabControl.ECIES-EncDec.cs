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
        private byte[]? _lastEphemeralPubKey = null;
        private byte[]? _lastEphemeralPrivKey = null;
        private string? _lastEphemeralCurveName = null;
        private System.Windows.Forms.Label labelEncTest = null!;
        private System.Windows.Forms.TextBox textEncTest = null!;
        private System.Windows.Forms.Label labelEncEphemeralPub = null!;
        private System.Windows.Forms.TextBox textEncEphemeralPub = null!;
        private System.Windows.Forms.Label labelEncExtra = null!;
        private System.Windows.Forms.TextBox textEncExtra = null!;
        private System.Windows.Forms.Label labelEncKeyConvert = null!;
        private System.Windows.Forms.ComboBox comboEncKeyConvert = null!;
        private System.Windows.Forms.Button btnEncKeyConvert = null!;
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
            labelEncEphemeralPub = new Label();
            textEncEphemeralPub = new TextBox();
            labelEncExtra = new Label();
            textEncExtra = new TextBox();
            labelEncKeyConvert = new Label();
            comboEncKeyConvert = new ComboBox();
            btnEncKeyConvert = new Button();

            // ---- 标签文本 ----
            labelEncMode.Text = "加密模式：";
            labelEncInputFormat.Text = "明文格式：";
            labelEncOutputFormat.Text = "密文格式：";
            labelEncKey.Text = "对称密钥 (HEX，留空自动派生)：";
            labelEncIV.Text = "IV/Nonce (HEX，留空随机生成)：";
            labelEncBobPublic.Text = "Bob 公钥 (接收方)：";
            labelEncTest.Text = "测试：";
            labelEncInput.Text = "明文输入：";
            labelEncOutputLabel.Text = "密文结果：";
            labelEncEphemeralPub.Text = "临时公钥ePub：";
            labelEncExtra.Text = "临时私钥ePriv：";
            labelEncKeyConvert.Text = "临时密钥：";

            // ---- 加密模式下拉框 ----
            comboEncMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncMode.FormattingEnabled = true;
            comboEncMode.Items.AddRange(["ECIES (ECDH+AES-GCM, SHA-256)", "ECIES (ECDH+AES-GCM, SHA-512)", "AES-256-GCM", "AES-256-CBC", "ChaCha20-Poly1305"]);

            // ---- 输入格式下拉框 ----
            comboEncInputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncInputFormat.FormattingEnabled = true;
            comboEncInputFormat.Items.AddRange(["UTF-8文本", "Base64", "Hex"]);

            // ---- 输出格式下拉框 ----
            comboEncOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncOutputFormat.FormattingEnabled = true;
            comboEncOutputFormat.Items.AddRange(["Base64", "Hex"]);

            // ---- 临时密钥转换下拉框 ----
            comboEncKeyConvert.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncKeyConvert.FormattingEnabled = true;
            comboEncKeyConvert.Items.AddRange(["临时公钥 → Base64", "临时公钥 → PEM", "临时私钥 → Base64", "临时私钥 → PEM"]);

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

            // ---- 临时公钥/私钥显示框（只读） ----
            textEncEphemeralPub.Font = new System.Drawing.Font("Consolas", 9F);
            textEncEphemeralPub.Multiline = true;
            textEncEphemeralPub.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textEncEphemeralPub.ReadOnly = true;

            textEncExtra.Font = new System.Drawing.Font("Consolas", 9F);
            textEncExtra.Multiline = true;
            textEncExtra.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textEncExtra.ReadOnly = true;

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
            btnEncKeyConvert.Text = "转换";
            btnEncKeyConvert.Click += BtnEncKeyConvert_Click;
        }

        private void InitializeEncryptDefaults()
        {
            if (comboEncMode.Items.Count > 0)
                comboEncMode.SelectedIndex = 0;
            if (comboEncInputFormat.Items.Count > 0)
                comboEncInputFormat.SelectedIndex = 0;
            if (comboEncOutputFormat.Items.Count > 0)
                comboEncOutputFormat.SelectedIndex = 0;
            if (comboEncKeyConvert.Items.Count > 0)
                comboEncKeyConvert.SelectedIndex = 0;
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
                    RowCount = 4,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                leftLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

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

                // 临时私钥面板
                var extraPanel = CreateTextPanelWithLabel(textEncExtra, labelEncExtra.Text);
                leftLayout.Controls.Add(extraPanel, 0, 2);

                // 临时公钥面板
                var ephemeralPubPanel = CreateTextPanelWithLabel(textEncEphemeralPub, labelEncEphemeralPub.Text);
                leftLayout.Controls.Add(ephemeralPubPanel, 0, 3);

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
                var extraBtnPanel = CreateButtonPanel(textEncExtra);
                var ephemeralPubBtnPanel = CreateButtonPanel(textEncEphemeralPub);

                leftLayout.Controls.Add(inputBtnPanel, 1, 0);
                leftLayout.Controls.Add(outputBtnPanel, 1, 1);
                leftLayout.Controls.Add(extraBtnPanel, 1, 2);
                leftLayout.Controls.Add(ephemeralPubBtnPanel, 1, 3);

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
                    buttons[i].Height = 32;
                    buttons[i].Width = 140;
                    buttons[i].Margin = new Padding(0, 3, 0, 3);
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
                for (int i = 0; i < 3; i++)
                    configPanel.RowStyles.Add(new RowStyle(SizeType.AutoSize));
                configPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));

                // ---- 加密模式 ----
                labelEncMode.AutoSize = false;
                labelEncMode.Size = new Size(120, 28);
                labelEncMode.Margin = new Padding(0, 4, 4, 4);
                labelEncMode.TextAlign = ContentAlignment.MiddleRight;
                comboEncMode.Margin = new Padding(0, 3, 0, 3);
                comboEncMode.Width = 360;
                comboEncMode.Anchor = AnchorStyles.Left;

                // ---- 输入格式 ----
                labelEncInputFormat.AutoSize = false;
                labelEncInputFormat.Size = new Size(120, 28);
                labelEncInputFormat.Margin = new Padding(0, 4, 4, 4);
                labelEncInputFormat.TextAlign = ContentAlignment.MiddleRight;
                comboEncInputFormat.Margin = new Padding(0, 3, 0, 3);
                comboEncInputFormat.Width = 140;
                comboEncInputFormat.Anchor = AnchorStyles.Left;

                // ---- 输出格式 ----
                labelEncOutputFormat.AutoSize = false;
                labelEncOutputFormat.Size = new Size(120, 28);
                labelEncOutputFormat.Margin = new Padding(0, 4, 4, 4);
                labelEncOutputFormat.TextAlign = ContentAlignment.MiddleRight;
                comboEncOutputFormat.Margin = new Padding(0, 3, 0, 3);
                comboEncOutputFormat.Width = 140;
                comboEncOutputFormat.Anchor = AnchorStyles.Left;

                // ---- 临时密钥转换 ----
                labelEncKeyConvert.AutoSize = false;
                labelEncKeyConvert.Size = new Size(120, 32);
                labelEncKeyConvert.Margin = new Padding(0, 3, 4, 3);
                labelEncKeyConvert.TextAlign = ContentAlignment.MiddleRight;
                comboEncKeyConvert.Margin = new Padding(0, 3, 6, 3);
                comboEncKeyConvert.Size = new Size(360, 32);
                comboEncKeyConvert.Anchor = AnchorStyles.Left;
                btnEncKeyConvert.AutoSize = false;
                btnEncKeyConvert.Size = new Size(100, 32);
                btnEncKeyConvert.Anchor = AnchorStyles.Left;
                btnEncKeyConvert.Margin = new Padding(0, 3, 0, 3);
                var keyConvertPanel = new FlowLayoutPanel
                {
                    AutoSize = true,
                    AutoSizeMode = AutoSizeMode.GrowAndShrink,
                    FlowDirection = FlowDirection.LeftToRight,
                    WrapContents = false,
                    Margin = new Padding(0),
                    Padding = new Padding(0, 2, 0, 2)
                };
                keyConvertPanel.Controls.Add(comboEncKeyConvert);
                keyConvertPanel.Controls.Add(btnEncKeyConvert);

                // ---- 加入 configPanel ----
                configPanel.Controls.Add(labelEncMode, 0, 0);
                configPanel.Controls.Add(comboEncMode, 1, 0);
                configPanel.Controls.Add(labelEncInputFormat, 0, 1);
                configPanel.Controls.Add(comboEncInputFormat, 1, 1);
                configPanel.Controls.Add(labelEncOutputFormat, 0, 2);
                configPanel.Controls.Add(comboEncOutputFormat, 1, 2);
                configPanel.Controls.Add(labelEncKeyConvert, 0, 3);
                configPanel.Controls.Add(keyConvertPanel, 1, 3);
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

                textEncBobPublic.Visible = true;

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

        /// <summary>
        /// 获取实际使用的私钥 PEM：优先用已导入的 _privateKeyPem，
        /// 为空时尝试从文本框 textPrivateKey 解析 PEM / Base64 / Hex。
        /// </summary>
        private string GetEffectivePrivateKeyPem()
        {
            if (!string.IsNullOrWhiteSpace(_privateKeyPem))
                return _privateKeyPem;

            string displayText = textPrivateKey.Text.Trim();
            if (string.IsNullOrWhiteSpace(displayText))
                return string.Empty;

            try
            {
                return ConvertDisplayToPem(displayText, true);
            }
            catch
            {
                return displayText;
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

            string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM, SHA-256)";
            bool isEcies = mode.StartsWith("ECIES", StringComparison.OrdinalIgnoreCase);
            string privateKeyPem = GetEffectivePrivateKeyPem();

            if (!isEcies)
            {
                // 非 ECIES 模式：直接以私钥值作为 HKDF 种子派生密钥
                if (string.IsNullOrWhiteSpace(privateKeyPem))
                    throw new InvalidOperationException("请先生成/导入ECDSA私钥，或手动填写对称密钥");

                var privateKey = EcdsaKeyHelper.ImportPrivateKeyPem(privateKeyPem);
                byte[] rawKey = privateKey.D.ToByteArrayUnsigned();
                return DeriveKeyFromSecret(rawKey, mode);
            }

            // ======== ECIES 模式：标准 ECDH + 临时密钥对 ========
            // 从顶部公钥区域的 PEM 框读取 Bob 公钥
            string bobPem = textPublicKey?.Text ?? string.Empty;

            if (string.IsNullOrWhiteSpace(bobPem))
                throw new InvalidOperationException("ECIES 加密需要在顶部公钥区域生成/导入 Bob 公钥");

            // 1. 导入 Bob 公钥
            var bobPublicKey = EcdsaKeyHelper.ImportPublicKeyPem(bobPem);

            // 2. 获取曲线名
            string curveName = EcdsaKeyHelper.GetCurveName(bobPublicKey)
                ?? throw new InvalidOperationException("无法识别 Bob 公钥的曲线类型");

            // 3. 生成临时密钥对
            var ephKp = EcdhAlgorithm.GenerateKeyPair(curveName);
            var ephPriv = (ECPrivateKeyParameters)ephKp.Private;
            var ephPub = (ECPublicKeyParameters)ephKp.Public;

            // 4. 缓存临时密钥对及曲线名，供加密时拼入密文头部及 UI 展示/转换
            _lastEphemeralCurveName = curveName;
            _lastEphemeralPubKey = ephPub.Q.GetEncoded(false);
            _lastEphemeralPrivKey = ephPriv.D.ToByteArrayUnsigned();

            // 5. ECDH：临时私钥 × Bob 公钥 → 共享密钥
            byte[] sharedSecret = EcdhAlgorithm.DeriveSharedSecret(ephPriv, bobPublicKey);

            // 6. HKDF 派生 AES-256 密钥
            return DeriveKeyFromSecret(sharedSecret, mode);
        }

        /// <summary>
        /// 通过 HKDF（SHA-256 或 SHA-512）从共享密钥/私钥值派生 AES-256 密钥
        /// </summary>
        private static byte[] DeriveKeyFromSecret(byte[] secret, string mode)
        {
            bool useSha512 = mode.Contains("SHA-512", StringComparison.OrdinalIgnoreCase);
            var digest = useSha512 ? (Org.BouncyCastle.Crypto.IDigest)new Sha512Digest() : new Sha256Digest();
            var hkdf = new HkdfBytesGenerator(digest);
            hkdf.Init(new HkdfParameters(secret, null, Encoding.UTF8.GetBytes("CryptoTool-ECIES-EncKey")));
            byte[] derivedKey = new byte[32];
            hkdf.GenerateBytes(derivedKey, 0, derivedKey.Length);
            return derivedKey;
        }

        /// <summary>
        /// 从密文中提取临时公钥并执行 ECDH 还原对称密钥（仅 ECIES 解密时调用）
        /// </summary>
        private byte[] RecoverEciesKeyFromEphemeralPub(byte[] ephemeralPubBytes, string mode)
        {
            string privateKeyPem = GetEffectivePrivateKeyPem();
            if (string.IsNullOrWhiteSpace(privateKeyPem))
                throw new InvalidOperationException("ECIES 解密需要当前 ECDSA 私钥（Bob 的私钥）");

            var ourPrivateKey = EcdsaKeyHelper.ImportPrivateKeyPem(privateKeyPem);
            var domain = ourPrivateKey.Parameters;

            // 从编码字节还原临时公钥
            var point = domain.Curve.DecodePoint(ephemeralPubBytes);
            var epub = new ECPublicKeyParameters("ECDH", point, domain);

            // ECDH：Bob 私钥 × 临时公钥 → 共享密钥
            byte[] sharedSecret = EcdhAlgorithm.DeriveSharedSecret(ourPrivateKey, epub);

            // HKDF 派生 AES-256 密钥
            return DeriveKeyFromSecret(sharedSecret, mode);
        }

        private byte[] GetEncIV(string mode)
        {
            int ivLen = mode switch
            {
                "AES-256-CBC" => 16,
                _ => 12
            };

            if (!string.IsNullOrWhiteSpace(textEncIV.Text))
            {
                string currentHex = textEncIV.Text.Trim().ToLowerInvariant();

                // 与上次自动生成的 IV 不同时才视为用户手动指定
                string lastHex = _lastEncIV != null ? Convert.ToHexString(_lastEncIV).ToLowerInvariant() : string.Empty;
                if (currentHex != lastHex)
                {
                    return Convert.FromHexString(currentHex);
                }
            }

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

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM, SHA-256)";
                bool isEcies = mode.StartsWith("ECIES", StringComparison.OrdinalIgnoreCase);
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

                // 拼接输出：ECIES 模式 = 临时公钥 || IV || 密文+tag；非 ECIES = IV || 密文+tag
                byte[] combined = isEcies && _lastEphemeralPubKey != null
                    ? [.. _lastEphemeralPubKey, .. iv, .. cipher]
                    : [.. iv, .. cipher];

                textEncIV.Text = Convert.ToHexString(iv).ToLowerInvariant();
                string outFormat = comboEncOutputFormat.SelectedItem?.ToString() ?? "Base64";
                textEncOutput.Text = outFormat == "Hex"
                    ? Convert.ToHexString(combined).ToLowerInvariant()
                    : Convert.ToBase64String(combined);

                AppendValidationResult($"✅ 加密成功\r\n算法: {mode}\r\nIV: {Convert.ToHexString(iv).ToLowerInvariant()}\r\n密文长度: {cipher.Length}字节", Color.Green);

                // 展示临时密钥对到 UI 文本框（首次生成使用 Base64 格式）
                if (isEcies && _lastEphemeralPubKey != null)
                    textEncEphemeralPub.Text = Convert.ToBase64String(_lastEphemeralPubKey);
                if (isEcies && _lastEphemeralPrivKey != null)
                    textEncExtra.Text = Convert.ToBase64String(_lastEphemeralPrivKey);

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

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM, SHA-256)";
                bool isEcies = mode.StartsWith("ECIES", StringComparison.OrdinalIgnoreCase);

                // 从密文 blob 中提取 IV 和密文（ECIES 模式还需先提取临时公钥）
                byte[] combined = GetCipherBytes(cipherSource);
                byte[] iv;
                byte[] cipher;
                byte[] key;

                if (isEcies)
                {
                    // ECIES: 临时公钥 || IV(12B) || 密文+tag(16B)
                    string privateKeyPem = GetEffectivePrivateKeyPem();
                    if (string.IsNullOrWhiteSpace(privateKeyPem))
                        throw new InvalidOperationException("ECIES 解密需要当前 ECDSA 私钥");
                    var privKey = EcdsaKeyHelper.ImportPrivateKeyPem(privateKeyPem);
                    string curveName = EcdsaKeyHelper.GetCurveName(privKey)
                        ?? throw new InvalidOperationException("无法识别私钥的曲线类型");
                    int epubLen = EcdhAlgorithm.GetEphemeralPubKeyLength(curveName);

                    byte[] epubBytes = [.. combined.Take(epubLen)];
                    byte[] rest = [.. combined.Skip(epubLen)];
                    iv = [.. rest.Take(12)];
                    cipher = [.. rest.Skip(12)];

                    // 用 ECDH 恢复对称密钥
                    key = RecoverEciesKeyFromEphemeralPub(epubBytes, mode);
                    textEncIV.Text = Convert.ToHexString(iv).ToLowerInvariant();
                }
                else
                {
                    // 非 ECIES: IV || 密文+tag
                    int ivLen = mode == "AES-256-CBC" ? 16 : 12;
                    iv = [.. combined.Take(ivLen)];
                    cipher = [.. combined.Skip(ivLen)];
                    textEncIV.Text = Convert.ToHexString(iv).ToLowerInvariant();
                    key = GetEncKey();
                }

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
            _lastEphemeralPubKey = null;
            _lastEphemeralPrivKey = null;
            _lastEphemeralCurveName = null;
            textEncEphemeralPub.Text = string.Empty;
            textEncExtra.Text = string.Empty;
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

        private void BtnEncKeyConvert_Click(object? sender, EventArgs e)
        {
            try
            {
                string selected = comboEncKeyConvert.SelectedItem?.ToString() ?? string.Empty;
                if (string.IsNullOrEmpty(selected))
                {
                    MessageBox.Show("请选择要转换的临时密钥", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                bool isPub = selected.Contains("公钥");
                TextBox targetBox = isPub ? textEncEphemeralPub : textEncExtra;
                if (string.IsNullOrWhiteSpace(targetBox.Text))
                {
                    MessageBox.Show("目标文本框为空，没有可转换的内容", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                string currentText = targetBox.Text.Trim();
                string currentFormat = DetectKeyFormat(currentText);
                if (currentFormat == "Unknown")
                {
                    MessageBox.Show("无法识别当前文本格式（需要 Hex、Base64 或 PEM）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 按当前格式自动决定下一个格式：Hex -> Base64 -> PEM -> Hex
                string targetFormat = currentFormat switch
                {
                    "Hex" => "Base64",
                    "Base64" => "PEM",
                    "PEM" => "Hex",
                    _ => "Base64"
                };

                string result;
                if (currentFormat == "PEM")
                {
                    // PEM 直接解析为原始字节，再转成 Hex
                    byte[] bytes = isPub
                        ? EcdsaKeyHelper.ImportPublicKeyPem(currentText).Q.GetEncoded(false)
                        : EcdsaKeyHelper.ImportPrivateKeyPem(currentText).D.ToByteArrayUnsigned();
                    result = Convert.ToHexString(bytes).ToLowerInvariant();
                }
                else if (targetFormat == "PEM")
                {
                    // Hex/Base64 -> PEM 需要曲线信息
                    byte[] bytes = currentFormat == "Hex"
                        ? Convert.FromHexString(currentText.Replace(" ", ""))
                        : Convert.FromBase64String(currentText.Replace("\r", "").Replace("\n", ""));

                    if (string.IsNullOrEmpty(_lastEphemeralCurveName))
                    {
                        MessageBox.Show("未找到曲线信息，无法转换为 PEM 格式（请先执行 ECIES 加密）", "提示", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }

                    var curveParams = Org.BouncyCastle.Asn1.X9.ECNamedCurveTable.GetByName(_lastEphemeralCurveName)
                        ?? throw new InvalidOperationException($"不支持的曲线: {_lastEphemeralCurveName}");
                    var domain = new Org.BouncyCastle.Crypto.Parameters.ECDomainParameters(
                        curveParams.Curve, curveParams.G, curveParams.N, curveParams.H, curveParams.GetSeed());

                    if (isPub)
                    {
                        var point = domain.Curve.DecodePoint(bytes);
                        var pubKey = new Org.BouncyCastle.Crypto.Parameters.ECPublicKeyParameters("ECDSA", point, domain);
                        result = EcdsaKeyHelper.ExportPublicKeyPem(pubKey);
                    }
                    else
                    {
                        var privKey = new Org.BouncyCastle.Crypto.Parameters.ECPrivateKeyParameters(
                            new Org.BouncyCastle.Math.BigInteger(1, bytes), domain);
                        result = EcdsaKeyHelper.ExportPrivateKeyPem(privKey);
                    }
                }
                else if (targetFormat == "Base64")
                {
                    byte[] bytes = Convert.FromHexString(currentText.Replace(" ", ""));
                    result = Convert.ToBase64String(bytes);
                }
                else
                {
                    byte[] bytes = Convert.FromBase64String(currentText.Replace("\r", "").Replace("\n", ""));
                    result = Convert.ToHexString(bytes).ToLowerInvariant();
                }

                targetBox.Text = result;
                AppendValidationResult($"✅ 转换成功: {currentFormat} → {targetFormat}", Color.Green);
            }
            catch (Exception ex)
            {
                AppendValidationResult($"❌ 转换失败: {ex.Message}", Color.Red);
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

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM, SHA-256)";
                bool isEcies = mode.StartsWith("ECIES", StringComparison.OrdinalIgnoreCase);
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
                if (isEcies && _lastEphemeralPubKey != null)
                    fs.Write(_lastEphemeralPubKey, 0, _lastEphemeralPubKey.Length);
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

                string mode = comboEncMode.SelectedItem?.ToString() ?? "ECIES (ECDH+AES-GCM, SHA-256)";
                bool isEcies = mode.StartsWith("ECIES", StringComparison.OrdinalIgnoreCase);
                byte[] allBytes = File.ReadAllBytes(openDlg.FileName);
                byte[] iv;
                byte[] cipher;
                byte[] key;

                if (isEcies)
                {
                    // ECIES 文件格式：临时公钥 || IV || 密文+tag
                    string privateKeyPem = GetEffectivePrivateKeyPem();
                    if (string.IsNullOrWhiteSpace(privateKeyPem))
                        throw new InvalidOperationException("ECIES 解密需要当前 ECDSA 私钥");
                    var privKey = EcdsaKeyHelper.ImportPrivateKeyPem(privateKeyPem);
                    string curveName = EcdsaKeyHelper.GetCurveName(privKey)
                        ?? throw new InvalidOperationException("无法识别私钥的曲线类型");
                    int epubLen = EcdhAlgorithm.GetEphemeralPubKeyLength(curveName);

                    byte[] epubBytes = [.. allBytes.Take(epubLen)];
                    byte[] rest = [.. allBytes.Skip(epubLen)];
                    iv = [.. rest.Take(12)];
                    cipher = [.. rest.Skip(12)];
                    key = RecoverEciesKeyFromEphemeralPub(epubBytes, mode);
                }
                else
                {
                    int ivLen = mode == "AES-256-CBC" ? 16 : 12;
                    iv = [.. allBytes.Take(ivLen)];
                    cipher = [.. allBytes.Skip(ivLen)];
                    key = GetEncKey();
                }

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
