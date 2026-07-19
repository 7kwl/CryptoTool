#nullable disable

using System.Drawing;
using System.Windows.Forms;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;

namespace CryptoTool.Win
{
    /// <summary>
    /// 手写 UI 布局初始化代码，与 EcdsaTabControl.Designer.cs 中设计器生成的代码分离
    /// 与 EcdsaTabControl.cs 中的业务逻辑分离
    /// </summary>
    public partial class EcdsaTabControl
    {
        // ============ 加密布局初始化 ============

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

        private static void SetControlDoubleBuffered(Control control)
        {
            if (control == null) return;
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(control, true);
        }

        public void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            SetControlDoubleBuffered(mainTableLayout);
        }

        public void InitializeEncryptLayout()
        {
            tableLayoutEncrypt.SuspendLayout();
            try
            {
                tableLayoutEncrypt.Controls.Clear();
                tableLayoutEncrypt.ColumnStyles.Clear();
                tableLayoutEncrypt.RowStyles.Clear();

                tableLayoutEncrypt.ColumnCount = 3;
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45F));
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 15F));
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 40F));
                tableLayoutEncrypt.RowCount = 1;
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                labelEncResult.Visible = false;
                labelEncResult.Enabled = false;

                // 左侧：输入 / 输出区域
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
                    RowCount = 4,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                labelEncInput.Dock = DockStyle.Fill;
                labelEncInput.TextAlign = ContentAlignment.MiddleLeft;
                leftLayout.Controls.Add(labelEncInput, 0, 0);
                leftLayout.Controls.Add(textEncInput, 0, 1);

                labelEncOutputLabel.Dock = DockStyle.Fill;
                labelEncOutputLabel.TextAlign = ContentAlignment.MiddleLeft;
                leftLayout.Controls.Add(labelEncOutputLabel, 0, 2);
                leftLayout.Controls.Add(textEncOutput, 0, 3);

                leftGroup.Controls.Add(leftLayout);

                // 中间：操作按钮
                var middleGroup = new GroupBox
                {
                    Text = "操作",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var middleLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 5,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                middleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
                middleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
                middleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
                middleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
                middleLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));

                Button[] buttons = [btnEncrypt, btnDecrypt, btnEncClear, btnEncCopy, btnEncPaste];
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].Dock = DockStyle.Fill;
                    buttons[i].Margin = new Padding(0, 4, 0, 4);
                    buttons[i].AutoSize = false;
                    middleLayout.Controls.Add(buttons[i], 0, i);
                }

                middleGroup.Controls.Add(middleLayout);

                // 右侧：参数配置
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
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));

                rightLayout.Controls.Add(CreateLabelControlRow(labelEncMode, comboEncMode), 0, 0);
                rightLayout.Controls.Add(CreateFormatRow(
                    labelEncInputFormat, comboEncInputFormat,
                    labelEncOutputFormat, comboEncOutputFormat), 0, 1);
                rightLayout.Controls.Add(CreateLabelControlRow(labelEncKey, textEncKey), 0, 2);
                rightLayout.Controls.Add(CreateLabelControlRow(labelEncIV, textEncIV), 0, 3);

                rightGroup.Controls.Add(rightLayout);

                tableLayoutEncrypt.Controls.Add(leftGroup, 0, 0);
                tableLayoutEncrypt.Controls.Add(middleGroup, 1, 0);
                tableLayoutEncrypt.Controls.Add(rightGroup, 2, 0);

                groupEncFile.Visible = false;
                groupEncFile.Enabled = false;
            }
            finally
            {
                tableLayoutEncrypt.ResumeLayout(true);
            }
        }

        // ============ 视图切换器 ============

        public void InitializeViewSwitcher()
        {
            btnViewEcdh.Click += (_, _) => SwitchView(0);
            btnViewSign.Click += (_, _) => SwitchView(1);
            btnViewEncrypt.Click += (_, _) => SwitchView(2);
            btnViewFile.Click += (_, _) => SwitchView(3);

            _currentViewIndex = -1;
            SwitchView(0);
        }

        public void SwitchView(int index)
        {
            if (_currentViewIndex == index) return;
            _currentViewIndex = index;

            var buttons = new[] { btnViewEcdh, btnViewSign, btnViewEncrypt, btnViewFile };
            var groups = new Control[] { groupEcdh, groupSign, groupEncrypt, groupFile };

            for (int i = 0; i < buttons.Length; i++)
            {
                bool isActive = (i == index);
                buttons[i].BackColor = isActive ? SystemColors.Highlight : SystemColors.Control;
                buttons[i].ForeColor = isActive ? Color.White : SystemColors.ControlText;
                groups[i].Visible = isActive;
            }
        }

        // ============ ECDH 视图布局 ============

        public void InitializeEcdhLayout()
        {
            groupEcdh.SuspendLayout();
            try
            {
                var main = new TableLayoutPanel
                {
                    Name = "tableLayoutEcdh",
                    Dock = DockStyle.Fill,
                    ColumnCount = 3,
                    RowCount = 2,
                    Margin = new Padding(0),
                    Padding = new Padding(4)
                };
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));
                main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                var leftPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 2,
                    Margin = new Padding(0),
                    Padding = new Padding(4)
                };
                leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                var aliceBox = CreateKeyPairBox("爱丽丝的钥匙 (Alice)", out textEcdhAlicePrivate, out textEcdhAlicePublic);
                leftPanel.Controls.Add(aliceBox, 0, 0);

                var bobBox = CreateKeyPairBox("鲍勃的钥匙 (Bob)", out textEcdhBobPrivate, out textEcdhBobPublic);
                leftPanel.Controls.Add(bobBox, 0, 1);

                main.Controls.Add(leftPanel, 0, 0);
                main.SetRowSpan(leftPanel, 2);

                var operationsPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 6,
                    Margin = new Padding(3),
                    Padding = new Padding(6)
                };
                operationsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
                operationsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                for (int i = 0; i < 6; i++)
                {
                    float pct = (i == 2 || i == 5) ? 16.6666F : 16.6667F;
                    operationsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, pct));
                }

                const int ecdhBtnWidth = 150;
                var curveRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 4, 0, 4),
                    WrapContents = false
                };
                var lblCurve = new Label
                {
                    Text = "椭圆曲线：",
                    AutoSize = false,
                    Size = new Size(100, 32),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 3, 8, 3)
                };
                comboEcdhCategory = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new Size(183, 32),
                    Margin = new Padding(0, 3, 4, 3)
                };
                var lblArrow = new Label
                {
                    Text = "→",
                    AutoSize = false,
                    Size = new Size(36, 32),
                    TextAlign = ContentAlignment.MiddleCenter,
                    Margin = new Padding(4, 3, 4, 3)
                };
                comboEcdhCurve = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    DisplayMember = "Value",
                    ValueMember = "Key",
                    Size = new Size(330, 32),
                    Margin = new Padding(0, 3, 8, 3)
                };
                var lblMode = new Label
                {
                    Text = "密钥模型：",
                    AutoSize = false,
                    Size = new Size(100, 32),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 3, 4, 3)
                };
                comboEcdhMode = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new Size(160, 32),
                    Margin = new Padding(0, 3, 8, 3)
                };
                comboEcdhMode.Items.AddRange([
                    "8gwifi.org",
                    "CryptoTool",
                    "ANSI X9.63",
                    "IEEE 1363a",
                    "ISO/IEC 18033-2",
                    "SECG SEC 1"
                ]);
                comboEcdhMode.SelectedIndex = 0;
                comboEcdhMode.SelectedIndexChanged += (s, e) =>
                {
                    lblEcdhIV.Text = GetEcdhMode() == EcdhMode.GwifiOrg ? "IV (Hex):" : "IV (Base64):";
                };
                btnGenerateEcdhKeys = new Button
                {
                    Text = "生成 EC 密钥对",
                    Width = ecdhBtnWidth,
                    Dock = DockStyle.Fill,
                    Padding = new Padding(8, 2, 8, 2),
                    Margin = new Padding(0, 2, 0, 2)
                };
                btnGenerateEcdhKeys.Click += BtnGenerateEcdhKeys_Click;
                curveRow.Controls.Add(lblCurve);
                curveRow.Controls.Add(comboEcdhCategory);
                curveRow.Controls.Add(lblArrow);
                curveRow.Controls.Add(comboEcdhCurve);

                var modeRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 4, 0, 4),
                    WrapContents = false
                };
                modeRow.Controls.Add(lblMode);
                modeRow.Controls.Add(comboEcdhMode);

                var encRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 4, 0, 4),
                    WrapContents = false
                };
                var lblEncoding = new Label
                {
                    Text = "明文编码：",
                    AutoSize = false,
                    Size = new Size(100, 32),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 3, 4, 3)
                };
                comboEcdhEncoding = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new Size(120, 32),
                    Margin = new Padding(0, 3, 8, 3)
                };
                comboEcdhEncoding.Items.AddRange(["UTF-8", "GBK (GB2312)", "Unicode (UTF-16 LE)"]);
                comboEcdhEncoding.SelectedIndex = 0;
                encRow.Controls.Add(lblEncoding);
                encRow.Controls.Add(comboEcdhEncoding);

                var btnPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 6,
                    Margin = new Padding(0),
                    Padding = new Padding(0, 4, 0, 4)
                };
                for (int i = 0; i < 6; i++)
                    btnPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 16.6667F));

                btnEcdhEncrypt = new Button { Text = "加密", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhDecrypt = new Button { Text = "解密", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhCopyResult = new Button { Text = "复制结果", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhPasteInput = new Button { Text = "粘贴输入", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhClear = new Button { Text = "清空", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                lblEcdhIV = new Label { Text = "IV (Base64):", AutoSize = false, Height = 22, Margin = new Padding(0, 0, 4, 2), TextAlign = ContentAlignment.MiddleLeft };
                textEcdhIV = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = false,
                    BackColor = SystemColors.Window,
                    Font = new Font("Consolas", 9F)
                };

                btnEcdhEncrypt.Click += BtnEcdhEncrypt_Click;
                btnEcdhDecrypt.Click += BtnEcdhDecrypt_Click;
                btnEcdhCopyResult.Click += BtnEcdhCopyResult_Click;
                btnEcdhPasteInput.Click += BtnEcdhPasteInput_Click;
                btnEcdhClear.Click += BtnEcdhClear_Click;

                btnPanel.Controls.Add(btnGenerateEcdhKeys, 0, 0);
                btnPanel.Controls.Add(btnEcdhEncrypt, 0, 1);
                btnPanel.Controls.Add(btnEcdhDecrypt, 0, 2);
                btnPanel.Controls.Add(btnEcdhCopyResult, 0, 3);
                btnPanel.Controls.Add(btnEcdhPasteInput, 0, 4);
                btnPanel.Controls.Add(btnEcdhClear, 0, 5);
                operationsPanel.Controls.Add(btnPanel, 0, 0);
                operationsPanel.SetRowSpan(btnPanel, 6);

                operationsPanel.Controls.Add(curveRow, 1, 0);
                operationsPanel.Controls.Add(modeRow, 1, 1);
                operationsPanel.Controls.Add(encRow, 1, 2);

                main.Controls.Add(operationsPanel, 2, 0);

                var encryptDecryptBox = new GroupBox
                {
                    Text = "ECDH 加密 / 解密（执行标准右边自行选择）",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                var rightLayout = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 8,
                    Margin = new Padding(0),
                    Padding = new Padding(0)
                };
                rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));

                rightLayout.Controls.Add(new Label { Text = "明文:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
                textEcdhInput = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Consolas", 9F)
                };
                rightLayout.Controls.Add(textEcdhInput, 0, 1);

                rightLayout.Controls.Add(new Label { Text = "密文（可编辑）:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft, Height = 22 }, 0, 2);
                textEcdhOutput = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = false,
                    BackColor = SystemColors.Window,
                    Font = new Font("Consolas", 9F)
                };
                rightLayout.Controls.Add(textEcdhOutput, 0, 3);

                rightLayout.Controls.Add(new Label { Text = "共享密钥 (Base64, 可编辑):", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 4);
                textEcdhSharedKey = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = false,
                    BackColor = SystemColors.Window,
                    Font = new Font("Consolas", 9F)
                };
                rightLayout.Controls.Add(textEcdhSharedKey, 0, 5);

                lblEcdhIV.Dock = DockStyle.Fill;
                lblEcdhIV.Margin = new Padding(0);
                rightLayout.Controls.Add(lblEcdhIV, 0, 6);
                rightLayout.Controls.Add(textEcdhIV, 0, 7);

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
                        if (string.IsNullOrEmpty(target.Text))
                        {
                            MessageBox.Show("复制为空", "提示", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            Clipboard.SetText(target.Text);
                        }
                    };
                    btnPaste.Click += (s, e) => { if (Clipboard.ContainsText()) target.Text = Clipboard.GetText(); };
                    panel.Controls.Add(btnCopy, 0, 0);
                    panel.Controls.Add(btnPaste, 0, 1);
                    return panel;
                }

                var plainBtnPanel = CreateButtonPanel(textEcdhInput);
                var cipherBtnPanel = CreateButtonPanel(textEcdhOutput);
                var sharedBtnPanel = CreateButtonPanel(textEcdhSharedKey);
                var ivBtnPanel = CreateButtonPanel(textEcdhIV);

                rightLayout.Controls.Add(plainBtnPanel, 1, 0);
                rightLayout.SetRowSpan(plainBtnPanel, 2);
                rightLayout.Controls.Add(cipherBtnPanel, 1, 2);
                rightLayout.SetRowSpan(cipherBtnPanel, 2);
                rightLayout.Controls.Add(sharedBtnPanel, 1, 4);
                rightLayout.SetRowSpan(sharedBtnPanel, 2);
                rightLayout.Controls.Add(ivBtnPanel, 1, 6);
                rightLayout.SetRowSpan(ivBtnPanel, 2);

                encryptDecryptBox.Controls.Add(rightLayout);
                main.Controls.Add(encryptDecryptBox, 1, 0);
                main.SetRowSpan(encryptDecryptBox, 2);

                groupEcdh.Controls.Add(main);
                InitializeEcdhCurveList();
            }
            finally
            {
                groupEcdh.ResumeLayout(true);
            }
        }

        public void InitializeEcdhCurveList()
        {
            if (_allCurveData.Count == 0)
                _allCurveData = EcdsaCurveNames.GetAllCurvesByCategory();

            comboEcdhCategory.DisplayMember = "Text";
            comboEcdhCategory.ValueMember = "Value";
            foreach (var cat in _allCurveData)
            {
                comboEcdhCategory.Items.Add(new
                {
                    Text = $"{cat.Value.Icon} {cat.Key}",
                    Value = cat.Key
                });
            }

            comboEcdhCategory.SelectedIndexChanged += ComboEcdhCategory_SelectedIndexChanged;

            if (comboEcdhCategory.Items.Count > 0)
                comboEcdhCategory.SelectedIndex = 0;
        }

        private static GroupBox CreateKeyPairBox(string title, out TextBox privateBox, out TextBox publicBox)
        {
            string prefix = title.Contains("Alice") || title.Contains("爱丽丝") ? "Alice" : "Bob";
            var box = new GroupBox
            {
                Name = $"groupEcdh{prefix}Key",
                Text = title,
                Dock = DockStyle.Fill,
                Padding = new Padding(6),
                MinimumSize = new Size(0, 220)
            };
            var layout = new TableLayoutPanel
            {
                Name = $"tableLayoutEcdh{prefix}Key",
                Dock = DockStyle.Fill,
                ColumnCount = 2,
                RowCount = 2,
                Margin = new Padding(0),
                Padding = new Padding(0)
            };
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            layout.RowCount = 1;
            layout.RowStyles.Clear();
            layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

            privateBox = new TextBox
            {
                Name = $"textEcdh{prefix}Private",
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9F),
                ReadOnly = false,
                Enabled = true,
                TabStop = true,
                BackColor = SystemColors.Window,
                MinimumSize = new Size(0, 60)
            };
            publicBox = new TextBox
            {
                Name = $"textEcdh{prefix}Public",
                Dock = DockStyle.Fill,
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Font = new Font("Consolas", 9F),
                ReadOnly = false,
                Enabled = true,
                TabStop = true,
                BackColor = SystemColors.Window,
                MinimumSize = new Size(0, 60)
            };

            var privateTag = new Label
            {
                Name = $"labelEcdh{prefix}Private",
                Text = "私钥 (PEM)：",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.Red,
                Margin = new Padding(4, 4, 4, 2),
                Padding = new Padding(4, 0, 4, 0)
            };
            var publicTag = new Label
            {
                Name = $"labelEcdh{prefix}Public",
                Text = "公钥 (PEM)：",
                Anchor = AnchorStyles.Top | AnchorStyles.Right,
                AutoSize = true,
                BackColor = Color.Transparent,
                ForeColor = Color.Red,
                Margin = new Padding(4, 4, 4, 2),
                Padding = new Padding(4, 0, 4, 0)
            };

            var privatePanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(0, 0, 4, 0)
            };
            privatePanel.Controls.Add(privateTag);
            privatePanel.Controls.Add(privateBox);
            privatePanel.Resize += (s, e) =>
            {
                privateTag.Location = new Point(privatePanel.ClientSize.Width - privateTag.Width - 6, 4);
            };

            var publicPanel = new Panel
            {
                Dock = DockStyle.Fill,
                Margin = new Padding(4, 0, 0, 0)
            };
            publicPanel.Controls.Add(publicTag);
            publicPanel.Controls.Add(publicBox);
            publicPanel.Resize += (s, e) =>
            {
                publicTag.Location = new Point(publicPanel.ClientSize.Width - publicTag.Width - 6, 4);
            };

            layout.Controls.Add(privatePanel, 0, 0);
            layout.Controls.Add(publicPanel, 1, 0);

            box.Controls.Add(layout);
            return box;
        }

        // ============ 缩放/布局辅助 ============

        public void InitializeResizeTimer()
        {
            components ??= new System.ComponentModel.Container();
            _resizeTimer = new(components) { Interval = 150 };
            _resizeTimer.Tick += (_, __) =>
            {
                _resizeTimer.Stop();
                ApplySplitterRatios();
            };
        }

        public void OnResizeTimerRestart(object sender, EventArgs e)
        {
            if (this.Width == _lastWidth && this.Height == _lastHeight)
                return;

            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private void ApplySplitterRatios()
        {
            _lastWidth = this.Width;
            _lastHeight = this.Height;
        }
    }
}