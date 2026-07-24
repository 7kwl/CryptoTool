#nullable disable

using System.Drawing;
using System.Windows.Forms;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;

namespace CryptoTool.Win
{
    /// <summary>
    /// =================================================================================
    /// ECDSA Tab 页 - ECDH 视图布局初始化代码
    /// =================================================================================
    /// 
    /// 此处代码负责动态构建 ECDH 视图布局。
    /// 这些布局无法在 WinForms 设计器中可视化编辑，因此以手写方式实现。
    /// 
    /// 职责分离:
    ///   - EcdsaTabControl.Designer.cs → 设计器自动生成的控件声明和属性
    ///   - EcdsaTabControl.ECDH.cs (本文件) → ECDH 布局与视图初始化
    ///   - EcdsaTabControl.ECIES-EncDec.cs → ECIES 加解密布局与业务逻辑
    ///   - EcdsaTabControl.cs → 业务逻辑和事件处理
    /// 
    /// 板块组织:
    ///   1. 双缓冲与抗闪烁 (SetControlDoubleBuffered / EnableDoubleBuffering)
    ///   2. 签名布局标签定位 (InitializeSignLayoutLabels)
    ///   3. 视图切换器 (InitializeViewSwitcher / SwitchView)
    ///   4. ECDH 视图布局 (InitializeEcdhLayout / InitializeEcdhCurveList)
    ///   5. 缩放/布局辅助 (InitializeResizeTimer / OnResizeTimerRestart / ApplySplitterRatios)
    /// </summary>
    public partial class EcdsaTabControl
    {
        // ═══════════════════════════════════════════════════════════════════
        // [板块 1] 双缓冲与抗闪烁 - 控件绘制优化
        //   开启双缓冲可有效减少界面频繁刷新时的闪烁问题。
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// 通过反射为指定控件启用双缓冲绘制。
        /// DoubleBuffered 是 Control 的 protected 属性，只能通过反射设置。
        /// </summary>
        /// <param name="control">目标控件</param>
        private static void SetControlDoubleBuffered(Control control)
        {
            if (control == null) return;
            typeof(Control).GetProperty("DoubleBuffered",
                System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance)
                ?.SetValue(control, true);
        }

        /// <summary>
        /// 为本 Tab 页启用双缓冲，减少布局刷新时的闪烁。
        /// 同时递归为 mainTableLayout 及其子控件启用双缓冲。
        /// </summary>
        public void EnableDoubleBuffering()
        {
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            SetControlDoubleBuffered(mainTableLayout);
        }

        // ═══════════════════════════════════════════════════════════════════
        // [板块 2] 签名布局标签定位 - 动态 Resize 事件
        //   参照 CreateKeyPairBox 中的做法，为 panelPlainDataBox / panelSignatureBox
        //   绑定 Resize 事件，确保 labelPlainData / labelSignature 始终固定在右上角。
        //   与 ECDH 动态面板一致：label 定位 = panelWidth - labelWidth - 4px
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// 初始化签名布局中标签的动态定位。
        /// 签名区的 labelPlainData 和 labelSignature 使用 Anchor+Resize 双重保障，
        /// 确保面板宽度变化时标签始终固定在右上角，间距 4px。
        /// </summary>
        private void InitializeSignLayoutLabels()
        {
            panelPlainDataBox.Resize += (_, _) =>
            {
                labelPlainData.Location = new Point(
                    panelPlainDataBox.ClientSize.Width - labelPlainData.Width - 4, 4);
            };
            panelSignatureBox.Resize += (_, _) =>
            {
                labelSignature.Location = new Point(
                    panelSignatureBox.ClientSize.Width - labelSignature.Width - 4, 4);
            };
        }

        // ═══════════════════════════════════════════════════════════════════
        // [板块 3] 视图切换器 - 签名/加解密/文件/ECDH 视图切换
        //   通过 panelViewBar 中的按钮切换 panelViewContent 中的显示面板。
        //   索引: 0=ECDH, 1=签名, 2=加解密, 3=文件
        //   高亮效果: 活跃按钮使用系统高亮色+白色文字，非活跃按钮使用默认色
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>初始化视图切换器，绑定按钮事件并设置默认视图(ECDH)。</summary>
        public void InitializeViewSwitcher()
        {
            // 绑定4个视图按钮的点击事件 → SwitchView(视图索引)
            btnViewEcdh.Click += (_, _) => SwitchView(0);
            btnViewSign.Click += (_, _) => SwitchView(1);
            btnViewEncrypt.Click += (_, _) => SwitchView(2);
            btnViewFile.Click += (_, _) => SwitchView(3);

            _currentViewIndex = -1;
            SwitchView(0); // 默认显示 ECDH 视图
        }

        /// <summary>
        /// 切换到指定索引的视图面板。
        /// 切换逻辑: 更新按钮高亮/默认样式，显示/隐藏对应的面板。
        /// </summary>
        /// <param name="index">视图索引: 0=ECDH, 1=签名, 2=加解密, 3=文件</param>
        public void SwitchView(int index)
        {
            if (_currentViewIndex == index) return;
            _currentViewIndex = index;

            var buttons = new[] { btnViewEcdh, btnViewSign, btnViewEncrypt, btnViewFile };
            var groups = new Control[] { groupEcdh, groupSign, groupEncrypt, groupFile };

            for (int i = 0; i < buttons.Length; i++)
            {
                bool isActive = (i == index);
                // 活跃按钮: 蓝色背景 + 白色文字; 非活跃: 默认灰色
                buttons[i].BackColor = isActive ? SystemColors.Highlight : SystemColors.Control;
                buttons[i].ForeColor = isActive ? Color.White : SystemColors.ControlText;
                groups[i].Visible = isActive;
            }
        }

        // ═══════════════════════════════════════════════════════════════════
        // [板块 4] ECDH 密钥协商视图布局
        //   构建三栏布局:
        //     左栏(35%): Alice 密钥对 + ECDH 加解密输入输出区
        //     中栏(30%): Bob 密钥对 + 共享密钥区
        //     右栏(35%): 操作按钮 + IV + 加解密结果
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// 动态构建 ECDH 密钥协商视图布局。
        /// 布局结构 (三栏两行):
        ///   左栏(35%): Alice密钥对(私钥/公钥上半) + Bob密钥对(私钥/公钥下半) → 跨2行
        ///   中栏(30%): 加密解密操作区(明文/密文/共享密钥/IV) → GroupBox → 跨2行
        ///   右栏(35%): 曲线选择 + 密钥模型 + 编码格式 + 操作按钮 → 上2行
        /// ⚠ groupRunResult 和 groupComputeResult 在 Designer 中已定义，不在此方法中处理
        /// </summary>
        public void InitializeEcdhLayout()
        {
            groupEcdh.SuspendLayout();
            try
            {
                // ---- 根布局: 三栏两行 ----
                var main = new TableLayoutPanel
                {
                    Name = "tableLayoutEcdh",
                    Dock = DockStyle.Fill,
                    ColumnCount = 3,
                    RowCount = 2,
                    Margin = new Padding(0),
                    Padding = new Padding(4)
                };
                main.ColumnStyles.Clear();
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 20F));
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                main.RowStyles.Clear();
                main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                // --------------------- 左栏: Alice + Bob 密钥对 ---------------------
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

                // --------------------- 右栏: 配置参数 + 操作按钮 ---------------------
                // 两列: 列0=按钮面板(170px固定) | 列1=设置项(100%)
                // 6行: 密钥模型 | 编码格式 | 私钥存储标准 | 公钥存储标准 | 椭圆曲线 | 计算结果
                var operationsPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 7,
                    Margin = new Padding(3),
                    Padding = new Padding(6)
                };
                operationsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));
                operationsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
                operationsPanel.RowStyles.Clear();
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F)); // 私钥存储标准
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F)); // 公钥存储标准
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
                operationsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));

                const int ecdhBtnWidth = 150;
                var curveRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 8, 0, 9),
                    WrapContents = false
                };
                var lblCurve = new Label
                {
                    Text = "椭圆曲线：",
                    AutoSize = false,
                    Size = new Size(200, 32),
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
                    lblEcdhIV.Text = GetEcdhMode() == EcdhMode.GwifiOrg ? "IV (Hex，可编辑/留空随机):" : "IV (Base64，可编辑/留空随机):";
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
                    Padding = new Padding(0, 8, 0, 9),
                    WrapContents = false
                };
                modeRow.Controls.Add(lblMode);
                modeRow.Controls.Add(comboEcdhMode);

                var encRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 8, 0, 9),
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

                // --------------------- 右栏: 密钥标准转换区 ---------------------
                var privateStandardRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 8, 0, 9),
                    WrapContents = false
                };
                var lblPrivateStandard = new Label
                {
                    Text = "私钥存储标准：",
                    AutoSize = false,
                    Size = new Size(130, 32),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 3, 4, 3)
                };
                comboEcdhPrivateKeyStandard = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new Size(300, 32),
                    Margin = new Padding(0, 3, 4, 3)
                };
                comboEcdhPrivateKeyStandard.Items.AddRange([PrivateKeyStandardPkcs8, PrivateKeyStandardSec1]);
                comboEcdhPrivateKeyStandard.SelectedIndex = 0;
                btnConvertEcdhPrivateKeyStandard = new Button
                {
                    Text = "转换",
                    Size = new Size(60, 32),
                    Margin = new Padding(0, 3, 0, 3)
                };
                btnConvertEcdhPrivateKeyStandard.Click += BtnConvertEcdhPrivateKeyStandard_Click;
                comboEcdhPrivateKeyStandard.SelectedIndexChanged += ComboEcdhPrivateKeyStandard_SelectedIndexChanged;
                privateStandardRow.Controls.Add(lblPrivateStandard);
                privateStandardRow.Controls.Add(comboEcdhPrivateKeyStandard);
                privateStandardRow.Controls.Add(btnConvertEcdhPrivateKeyStandard);

                var publicStandardRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    FlowDirection = FlowDirection.LeftToRight,
                    Margin = new Padding(50, 0, 0, 0),
                    Padding = new Padding(0, 8, 0, 9),
                    WrapContents = false
                };
                var lblPublicStandard = new Label
                {
                    Text = "公钥存储标准：",
                    AutoSize = false,
                    Size = new Size(130, 32),
                    TextAlign = ContentAlignment.MiddleLeft,
                    Margin = new Padding(0, 3, 4, 3)
                };
                comboEcdhPublicKeyStandard = new ComboBox
                {
                    DropDownStyle = ComboBoxStyle.DropDownList,
                    Size = new Size(300, 32),
                    Margin = new Padding(0, 3, 4, 3)
                };
                comboEcdhPublicKeyStandard.Items.AddRange([PublicKeyStandardNamedCurve, PublicKeyStandardSpecifiedCurve]);
                comboEcdhPublicKeyStandard.SelectedIndex = 0;
                btnConvertEcdhPublicKeyStandard = new Button
                {
                    Text = "转换",
                    Size = new Size(60, 32),
                    Margin = new Padding(0, 3, 0, 3)
                };
                btnConvertEcdhPublicKeyStandard.Click += BtnConvertEcdhPublicKeyStandard_Click;
                comboEcdhPublicKeyStandard.SelectedIndexChanged += ComboEcdhPublicKeyStandard_SelectedIndexChanged;
                publicStandardRow.Controls.Add(lblPublicStandard);
                publicStandardRow.Controls.Add(comboEcdhPublicKeyStandard);
                publicStandardRow.Controls.Add(btnConvertEcdhPublicKeyStandard);

                var btnPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 9,
                    Margin = new Padding(0),
                    Padding = new Padding(0, 4, 0, 4)
                };
                btnPanel.RowStyles.Clear();
                for (int i = 0; i < 8; i++)
                    btnPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 55F));
                btnPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 100F)); // 吸收剩余空间，避免最后一个按钮被拉伸

                btnEcdhEncrypt = new Button { Text = "加密", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhDecrypt = new Button { Text = "解密", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhCopyResult = new Button { Text = "复制结果", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhPasteInput = new Button { Text = "粘贴输入", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhClear = new Button { Text = "清空", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhAliceCurve = new Button { Text = "私钥1曲线", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                btnEcdhBobCurve = new Button { Text = "私钥2曲线", Width = ecdhBtnWidth, Dock = DockStyle.Fill, Padding = new Padding(8, 2, 8, 2), Margin = new Padding(0, 2, 0, 2) };
                lblEcdhIV = new Label { Text = "IV (Base64，可编辑/留空随):", AutoSize = false, Height = 22, Margin = new Padding(0, 0, 4, 2), TextAlign = ContentAlignment.MiddleLeft };
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
                btnEcdhAliceCurve.Click += (s, ev) =>
                {
                    try
                    {
                        string pem = textEcdhAlicePrivate.Text?.Trim() ?? string.Empty;
                        if (string.IsNullOrEmpty(pem))
                        {
                            AppendKeyResult("私钥1 为空，无法检测曲线", Color.Red);
                            SetStatus("私钥1 为空");
                            return;
                        }
                        var privateKey = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                        string curveName = EcdsaKeyHelper.GetCurveName(privateKey);
                        if (string.IsNullOrEmpty(curveName) || curveName == "未知曲线")
                        {
                            AppendKeyResult("私钥1 曲线检测: 非OpenSSL标准曲线", Color.Orange);
                            SetStatus("私钥1 非OpenSSL标准曲线");
                        }
                        else
                        {
                            AppendKeyResult($"私钥1 曲线检测: {curveName}", Color.Green, curveName);
                            SetStatus($"私钥1 曲线: {curveName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendKeyResult($"私钥1 曲线检测失败: {ex.Message}", Color.Red);
                        SetStatus("私钥1 曲线检测失败");
                    }
                };
                btnEcdhBobCurve.Click += (s, ev) =>
                {
                    try
                    {
                        string pem = textEcdhBobPrivate.Text?.Trim() ?? string.Empty;
                        if (string.IsNullOrEmpty(pem))
                        {
                            AppendKeyResult("私钥2 为空，无法检测曲线", Color.Red);
                            SetStatus("私钥2 为空");
                            return;
                        }
                        var privateKey = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
                        string curveName = EcdsaKeyHelper.GetCurveName(privateKey);
                        if (string.IsNullOrEmpty(curveName) || curveName == "未知曲线")
                        {
                            AppendKeyResult("私钥2 曲线检测: 非OpenSSL标准曲线", Color.Orange);
                            SetStatus("私钥2 非OpenSSL标准曲线");
                        }
                        else
                        {
                            AppendKeyResult($"私钥2 曲线检测: {curveName}", Color.Green, curveName);
                            SetStatus($"私钥2 曲线: {curveName}");
                        }
                    }
                    catch (Exception ex)
                    {
                        AppendKeyResult($"私钥2 曲线检测失败: {ex.Message}", Color.Red);
                        SetStatus("私钥2 曲线检测失败");
                    }
                };

                btnPanel.Controls.Add(btnGenerateEcdhKeys, 0, 0);
                btnPanel.Controls.Add(btnEcdhEncrypt, 0, 1);
                btnPanel.Controls.Add(btnEcdhDecrypt, 0, 2);
                btnPanel.Controls.Add(btnEcdhCopyResult, 0, 3);
                btnPanel.Controls.Add(btnEcdhPasteInput, 0, 4);
                btnPanel.Controls.Add(btnEcdhClear, 0, 5);
                btnPanel.Controls.Add(btnEcdhAliceCurve, 0, 6);
                btnPanel.Controls.Add(btnEcdhBobCurve, 0, 7);
                operationsPanel.Controls.Add(btnPanel, 0, 0);
                operationsPanel.SetRowSpan(btnPanel, 7);

                operationsPanel.Controls.Add(modeRow, 1, 0);
                operationsPanel.Controls.Add(encRow, 1, 1);
                operationsPanel.Controls.Add(privateStandardRow, 1, 2);
                operationsPanel.Controls.Add(publicStandardRow, 1, 3);
                operationsPanel.Controls.Add(curveRow, 1, 4);

                var operationsGroup = new GroupBox
                {
                    Name = "groupEcdhOperations",
                    Text = "密钥操作",
                    Dock = DockStyle.Fill,
                    Padding = new Padding(6)
                };
                operationsGroup.Controls.Add(operationsPanel);
                main.Controls.Add(operationsGroup, 2, 0);
                main.SetRowSpan(operationsGroup, 2);

                // --------------------- 中栏: ECDH 加解密操作区 ---------------------
                // 8行: 明文标签 | 明文输入框 | 密文标签 | 密文输入框 | 共享密钥标签 | 共享密钥输入框 | IV标签 | IV输入框
                // 每行右侧有 复制/粘贴 按钮面板
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
                        TrySetClipboardText(target.Text, "已复制到剪贴板", "复制为空");
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

        /// <summary>
        /// 初始化 ECDH 曲线选择下拉框的选项列表。
        /// 从 EcdsaCurveNames 获取按类别分组的所有曲线数据。
        /// 联动逻辑: comboEcdhCategory 选择变化 → comboEcdhCurve 更新对应类别的曲线列表。
        /// </summary>
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

        /// <summary>
        /// 创建 ECDH 密钥对 GroupBox (动态创建，不在设计器文件中)。
        /// 布局: 左50% 私钥(PEM) / 右50% 公钥(PEM)，各带红色标签浮于右上角。
        /// 标签通过 Panel.Resize 事件动态定位，始终保持右上角对齐。
        /// </summary>
        /// <param name="title">GroupBox 标题，如 "爱丽丝的钥匙 (Alice)"</param>
        /// <param name="privateBox">[out] 创建的私钥 TextBox 引用</param>
        /// <param name="publicBox">[out] 创建的公钥 TextBox 引用</param>
        /// <returns>包含完整布局的 GroupBox</returns>
        private static GroupBox CreateKeyPairBox(string title, out TextBox privateBox, out TextBox publicBox)
        {
            // 根据标题判断是 Alice 还是 Bob 的密钥对
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

        // ═══════════════════════════════════════════════════════════════════
        // [板块 5] 缩放/布局辅助 - 防抖 + 尺寸记忆
        //   窗口缩放时使用 Timer 防抖(150ms)，只在缩放停止后才执行布局调整。
        //   避免频繁重绘导致的性能问题和闪烁。
        // ═══════════════════════════════════════════════════════════════════

        /// <summary>
        /// 初始化窗口缩放防抖定时器。
        /// Interval=150ms，每次 Tick 触发时执行 ApplySplitterRatios()。
        /// 防止窗口拖拽过程中频繁执行重布局导致 CPU 飙升。
        /// </summary>
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

        /// <summary>
        /// 窗口尺寸变化时重启防抖定时器。
        /// 如果尺寸没有变化(多余触发)则直接跳过。
        /// 只有尺寸变化后的 150ms 静止期才会触发 ApplySplitterRatios。
        /// </summary>
        public void OnResizeTimerRestart(object sender, EventArgs e)
        {
            // 尺寸未变，跳过(Windows 可能重复触发 Resize 事件)
            if (this.Width == _lastWidth && this.Height == _lastHeight)
                return;

            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        /// <summary>
        /// 更新记录的窗口尺寸。在防抖定时器触发后调用。
        /// 实际的布局比例为设计器自动处理(TableLayoutPanel.Percent)，此方法仅更新缓存尺寸。
        /// </summary>
        private void ApplySplitterRatios()
        {
            _lastWidth = this.Width;
            _lastHeight = this.Height;
        }
    }
}