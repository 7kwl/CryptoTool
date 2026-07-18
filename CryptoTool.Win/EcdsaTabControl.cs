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

        // ECDH 视图相关控件与状态
        private ComboBox comboEcdhCategory = null!;
        private ComboBox comboEcdhCurve = null!;
        private ComboBox comboEcdhMode = null!;
        private ComboBox comboEcdhEncoding = null!;
        private Button btnGenerateEcdhKeys = null!;
        private TextBox textEcdhAlicePrivate = null!;
        private TextBox textEcdhAlicePublic = null!;
        private TextBox textEcdhBobPrivate = null!;
        private TextBox textEcdhBobPublic = null!;
        private TextBox textEcdhInput = null!;
        private TextBox textEcdhOutput = null!;
        private TextBox textEcdhSharedKey = null!;
        private TextBox textEcdhStaticShared = null!;
        private TextBox textEcdhIV = null!;
        private Label lblEcdhIV = null!;
        private Button btnEcdhEncrypt = null!;
        private Button btnEcdhDecrypt = null!;
        private Button btnEcdhCopyResult = null!;
        private Button btnEcdhPasteInput = null!;
        private Button btnEcdhClear = null!;
        private byte[]? _ecdhLastIV = null;

        public EcdsaTabControl()
        {
            InitializeComponent();
            InitializeEncryptLayout();
            InitializeViewSwitcher();
            InitializeEcdhLayout();
            InitializeDefaults();
            InitializeResizeTimer();
            EnableDoubleBuffering();
            InitializeResultHeaders();

            // ✅ 初始加载时保持拆分容器为50%
            this.Load += (_, __) => ApplySplitterRatios();
            // ✅ 缩放时防抖更新分隔条，避免每像素都触发完整布局导致卡顿
            this.SizeChanged += OnResizeTimerRestart;
        }

        private void OnResizeTimerRestart(object? sender, EventArgs e)
        {
            // 如果尺寸没有实质变化，避免重新布局
            if (this.Width == _lastWidth && this.Height == _lastHeight)
                return;

            _resizeTimer.Stop();
            _resizeTimer.Start();
        }

        private void InitializeResizeTimer()
        {
            // 将 Timer 加入 components 容器，由设计器生成的 Dispose 统一释放
            components ??= new System.ComponentModel.Container();
            _resizeTimer = new(components) { Interval = 150 };
            // 150ms 防抖，覆盖全屏/拖拽等连续尺寸变化，只在缩放稳定后重算一次
            _resizeTimer.Tick += (_, __) =>
            {
                _resizeTimer.Stop();
                ApplySplitterRatios();
            };
        }

        private void EnableDoubleBuffering()
        {
            // 开启双缓冲，减少缩放时因多次重绘造成的闪烁和卡顿
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer
                | ControlStyles.AllPaintingInWmPaint
                | ControlStyles.UserPaint
                | ControlStyles.ResizeRedraw, true);
            this.UpdateStyles();

            // 对 SplitContainer 也开启双缓冲（DoubleBuffered 是 protected，使用反射）
            SetControlDoubleBuffered(mainTableLayout);
        }

        private void InitializeResultHeaders()
        {
            // 清空 GroupBox 标题，用左右两列的标题代替，避免重复显示
            groupRunResult.Text = string.Empty;

            // 为运行结果区域的左右两列增加标题
            var labelRunResult = new Label
            {
                Name = "labelRunResultHeader",
                Text = "运行结果",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Font = new Font(this.Font, FontStyle.Bold)
            };
            var labelCalcResult = new Label
            {
                Name = "labelCalcResultHeader",
                Text = "计算结果",
                Dock = DockStyle.Fill,
                TextAlign = ContentAlignment.MiddleLeft,
                AutoSize = false,
                Font = new Font(this.Font, FontStyle.Bold)
            };

            tableRunResult.SuspendLayout();
            try
            {
                // 改为 2 列 2 行：左列计算结果，右列运行结果
                tableRunResult.Controls.Remove(labelValidationResult);
                tableRunResult.Controls.Remove(textKeyResult);
                tableRunResult.ColumnStyles.Clear();
                tableRunResult.RowStyles.Clear();
                tableRunResult.ColumnCount = 2;
                tableRunResult.RowCount = 2;

                tableRunResult.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tableRunResult.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
                tableRunResult.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
                tableRunResult.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));

                tableRunResult.Controls.Add(labelCalcResult, 0, 0);
                tableRunResult.Controls.Add(labelRunResult, 1, 0);
                tableRunResult.Controls.Add(textKeyResult, 0, 1);
                tableRunResult.Controls.Add(labelValidationResult, 1, 1);
            }
            finally
            {
                tableRunResult.ResumeLayout(true);
            }
        }

        private void InitializeEncryptLayout()
        {
            // 重建加密/解密区域布局，避免设计器把多个控件堆在同一单元格导致重叠
            tableLayoutEncrypt.SuspendLayout();
            try
            {
                tableLayoutEncrypt.Controls.Clear();
                tableLayoutEncrypt.ColumnStyles.Clear();
                tableLayoutEncrypt.RowStyles.Clear();

                tableLayoutEncrypt.ColumnCount = 1;
                tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));

                tableLayoutEncrypt.RowCount = 9;
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
                tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));

                // 原左侧结果框已废弃，改为输出到下方运行结果区域
                labelEncResult.Visible = false;
                labelEncResult.Enabled = false;

                // 右侧：加密模式
                tableLayoutEncrypt.Controls.Add(CreateLabelControlRow(labelEncMode, comboEncMode), 0, 0);

                // 右侧：输入/输出格式放在同一行
                tableLayoutEncrypt.Controls.Add(CreateFormatRow(
                    labelEncInputFormat, comboEncInputFormat,
                    labelEncOutputFormat, comboEncOutputFormat), 0, 1);

                // 右侧：对称密钥
                tableLayoutEncrypt.Controls.Add(CreateLabelControlRow(labelEncKey, textEncKey), 0, 2);

                // 右侧：IV/Nonce
                tableLayoutEncrypt.Controls.Add(CreateLabelControlRow(labelEncIV, textEncIV), 0, 3);

                // 右侧：明文/密文输入标签
                labelEncInput.Dock = DockStyle.Fill;
                labelEncInput.TextAlign = ContentAlignment.MiddleLeft;
                tableLayoutEncrypt.Controls.Add(labelEncInput, 0, 4);

                // 右侧：明文/密文输入框
                tableLayoutEncrypt.Controls.Add(textEncInput, 0, 5);

                // 右侧：加密/解密结果标签
                labelEncOutputLabel.Dock = DockStyle.Fill;
                labelEncOutputLabel.TextAlign = ContentAlignment.MiddleLeft;
                tableLayoutEncrypt.Controls.Add(labelEncOutputLabel, 0, 6);

                // 右侧：加密/解密结果框
                tableLayoutEncrypt.Controls.Add(textEncOutput, 0, 7);

                // 右侧：按钮
                tableLayoutEncrypt.Controls.Add(panelEncBtns, 0, 8);

                // 文件加密/解密已在其他区域提供，此处不再重复显示
                groupEncFile.Visible = false;
                groupEncFile.Enabled = false;
            }
            finally
            {
                tableLayoutEncrypt.ResumeLayout(true);
            }
        }

        private void InitializeViewSwitcher()
        {
            // Designer.cs 已设置 panelViewBar/panelViewContent 布局，此处只需绑定事件和初始状态

            // 按钮点击事件
            btnViewSign.Click += (_, _) => SwitchView(0);
            btnViewEncrypt.Click += (_, _) => SwitchView(1);
            btnViewFile.Click += (_, _) => SwitchView(2);
            btnViewEcdh.Click += (_, _) => SwitchView(3);

            _currentViewIndex = -1;
            SwitchView(0);
        }

        private void SwitchView(int index)
        {
            if (_currentViewIndex == index) return;
            _currentViewIndex = index;

            var buttons = new[] { btnViewSign, btnViewEncrypt, btnViewFile, btnViewEcdh };
            var groups = new Control[] { groupSign, groupEncrypt, groupFile, groupEcdh };

            for (int i = 0; i < buttons.Length; i++)
            {
                bool isActive = (i == index);
                buttons[i].BackColor = isActive ? SystemColors.Highlight : SystemColors.Control;
                buttons[i].ForeColor = isActive ? Color.White : SystemColors.ControlText;
                groups[i].Visible = isActive;
            }
        }

        #region ECDH 视图布局与逻辑
        private void InitializeEcdhLayout()
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
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));    // Alice/Bob 密钥列
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 30F));    // 加密/解密区
                main.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 35F));    // 操作区
                main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));          // Alice 密钥对
                main.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));          // Bob 密钥对

                // 0. 左侧总容器：静态共享密钥 + Alice/Bob 密钥对
                var leftPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 1,
                    RowCount = 3,
                    Margin = new Padding(0),
                    Padding = new Padding(4)
                };
                leftPanel.RowStyles.Add(new RowStyle(SizeType.Absolute, 80F));   // 静态共享密钥
                leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));    // Alice 密钥对
                leftPanel.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));    // Bob 密钥对

                var staticSharedBox = new GroupBox
                {
                    Text = "共同的秘密（Alice = Bob）",
                    Dock = DockStyle.Fill,
                    BackColor = Color.FromArgb(76, 175, 80),
                    ForeColor = Color.White,
                    Padding = new Padding(6)
                };
                textEcdhStaticShared = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    ReadOnly = true,
                    BackColor = Color.FromArgb(200, 230, 201),
                    ForeColor = Color.Black,
                    Font = new Font("Consolas", 9F),
                    BorderStyle = BorderStyle.None
                };
                staticSharedBox.Controls.Add(textEcdhStaticShared);
                leftPanel.Controls.Add(staticSharedBox, 0, 0);

                // 1. Alice 密钥对
                var aliceBox = CreateKeyPairBox("爱丽丝的钥匙 (Alice)", out textEcdhAlicePrivate, out textEcdhAlicePublic);
                leftPanel.Controls.Add(aliceBox, 0, 1);

                // 2. Bob 密钥对
                var bobBox = CreateKeyPairBox("鲍勃的钥匙 (Bob)", out textEcdhBobPrivate, out textEcdhBobPublic);
                leftPanel.Controls.Add(bobBox, 0, 2);

                main.Controls.Add(leftPanel, 0, 0);
                main.SetRowSpan(leftPanel, 2);

                // 2. 中侧操作区（跨两行）
                var operationsPanel = new TableLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    ColumnCount = 2,
                    RowCount = 6,
                    Margin = new Padding(3),  // 与左侧 GroupBox 默认外边距一致，高度对齐
                    Padding = new Padding(6)
                };
                operationsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 170F));  // 按钮列：加宽以容纳"生成 EC 密钥对"单行文本
                operationsPanel.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));   // 右侧区域：占剩余宽度
                // 六行，与左侧 Alice/Bob 两半高度一致：上 3 行 = 50%，下 3 行 = 50%
                for (int i = 0; i < 6; i++)
                {
                    float pct = (i == 2 || i == 5) ? 16.6666F : 16.6667F;
                    operationsPanel.RowStyles.Add(new RowStyle(SizeType.Percent, pct));
                }

                // 曲线选择 + 生成按钮（生成按钮后续会并入按钮列）
                const int ecdhBtnWidth = 150; // 加宽，避免"生成 EC 密钥对"文字换行
                var curveRow = new FlowLayoutPanel
                {
                    Dock = DockStyle.Fill,
                    Margin = new Padding(50, 0, 0, 0), // 增加与左侧按钮列的间距
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
                // 曲线行移到右侧（第二列），与按钮列上下错开
                curveRow.Controls.Add(lblCurve);
                curveRow.Controls.Add(comboEcdhCategory);
                curveRow.Controls.Add(lblArrow);
                curveRow.Controls.Add(comboEcdhCurve);

                // 密钥模型行（位于曲线行下方）
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

                // 明文编码行（位于密钥模型行下方）
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

                // 加密/解密按钮（竖立一列，按百分比均分高度，生成按钮也纳入同一列）
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
                operationsPanel.SetRowSpan(btnPanel, 6); // 按钮列跨六行

                // 右上：曲线行
                operationsPanel.Controls.Add(curveRow, 1, 0);

                // 右中：密钥模型行
                operationsPanel.Controls.Add(modeRow, 1, 1);

                // 右下：明文编码行
                operationsPanel.Controls.Add(encRow, 1, 2);

                main.Controls.Add(operationsPanel, 2, 0);
                // 操作区仅占用 Alice 密钥框所在的第一行高度，与左侧单个密钥框对齐

                // 3. 加密/解密区域（跨两行）
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
                rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F)); // 左侧输入区
                rightLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 80F));   // 右侧复制/粘贴按钮列
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));   // 明文标签
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));    // 明文输入
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));   // 密文标签
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));    // 密文输入
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));   // 共享密钥标签
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));    // 共享密钥输入
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));   // IV 标签
                rightLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));    // IV 输入

                // 信息输入
                rightLayout.Controls.Add(new Label { Text = "明文:", Dock = DockStyle.Fill, TextAlign = ContentAlignment.MiddleLeft }, 0, 0);
                textEcdhInput = new TextBox
                {
                    Dock = DockStyle.Fill,
                    Multiline = true,
                    ScrollBars = ScrollBars.Vertical,
                    Font = new Font("Consolas", 9F)
                };
                rightLayout.Controls.Add(textEcdhInput, 0, 1);

                // 信息输出
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

                // 共享密钥
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

                // IV（与上面 3 个输入区同宽、等高）
                lblEcdhIV.Dock = DockStyle.Fill;
                lblEcdhIV.Margin = new Padding(0);
                rightLayout.Controls.Add(lblEcdhIV, 0, 6);
                rightLayout.Controls.Add(textEcdhIV, 0, 7);

                // 右侧按钮列：每个输入区对应一组复制/粘贴按钮
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

        private void InitializeEcdhCurveList()
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
                textEcdhStaticShared.Text = textEcdhSharedKey.Text;

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
                    // ECIES（CryptoTool / ANSI X9.63 / IEEE 1363a / ISO/IEC 18033-2 / SECG SEC 1）：
                    // 发送方生成临时密钥对，只需接收方公钥
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
                    // 密文格式：Base64(IV + cipher+tag)，IV 会自动从密文头部解析
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
            textEcdhStaticShared.Clear();
            textEcdhIV.Clear();
            _ecdhLastIV = null;
            SetStatus("ECDH 输入/输出已清空");
        }
        #endregion


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

        private void ApplySplitterRatios()
        {
            _lastWidth = this.Width;
            _lastHeight = this.Height;
        }


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
            comboHashAlgorithm.Items.AddRange(["SHA256", "SHA384", "SHA512"]);
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
            "SHA256" => "SHA-256withECDSA",
            "SHA384" => "SHA-384withECDSA",
            "SHA512" => "SHA-512withECDSA",
            _ => "SHA-256withECDSA"
        };

        private string GetSelectedHash() => comboHashAlgorithm.SelectedItem?.ToString() ?? "SHA256";
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

        // ✅ 新增：复制私钥按钮点击事件
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

        // ✅ 新增：复制公钥按钮点击事件
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
            AppendValidationResult($"{status}\n生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n使用曲线: {EcdsaCurveNames.GetDisplayName(curveName)}", color);
        }

        private void SetSignResult(string status, Color color)
        {
            AppendValidationResult($"{status}\n生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n使用曲线: {EcdsaCurveNames.GetDisplayName(GetSelectedCurve())}（{GetSelectedHash()}）", color);
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

            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {message}{Environment.NewLine}{Environment.NewLine}";

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
                detail += $"\n生成时间: {DateTime.Now:yyyy-MM-dd HH:mm:ss}\n使用曲线: {EcdsaCurveNames.GetDisplayName(curveName)}";
            }

            string entry = $"{DateTime.Now:yyyy-MM-dd HH:mm:ss}  {detail}{Environment.NewLine}{Environment.NewLine}";

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


        #region ✅ 加密与解密逻辑

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