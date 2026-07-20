#nullable disable

using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;

namespace CryptoTool.Win
{
    /// <summary>
    /// =================================================================================
    /// ECDSA 椭圆曲线数字签名 Tab 页 - 设计器自动生成代码
    /// =================================================================================
    /// 
    /// 此文件由 Windows 窗体设计器生成，包含所有控件的声明和属性初始化。
    /// 请勿手动修改此文件中的代码（在 InitializeComponent 方法外部的字段声明区除外）。
    /// 
    /// <para>整体布局结构 (mainTableLayout 左右两栏):</para>
    ///   <item><b>左栏 50%:</b> 密钥管理(groupKey) | 签名验签(groupSign) | 加解密(groupEncrypt) | 文件操作(groupFile) | ECDH密钥协商(groupEcdh)</item>
    ///   <item><b>右栏 50%:</b> 运行结果(groupRunResult) / 计算结果(groupComputeResult)</item>
    /// 
    /// <para>视图切换机制:</para>
    ///   <item>panelViewBar → 签名视图 / 加解密视图 / 文件视图 / ECDH视图 (按钮组)</item>
    ///   <item>panelViewContent → 根据当前选中视图显示对应的操作面板</item>
    /// </summary>
    partial class EcdsaTabControl
    {
        /// <summary>设计器组件容器，用于管理所有控件的生命周期。</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>释放 Tab 页所使用的所有资源。</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 初始化本 Tab 页上的所有控件及其布局属性。
        /// </summary>
        private void InitializeComponent()
        {
            mainTableLayout = new TableLayoutPanel();
            groupKey = new GroupBox();
            tableLayoutKey = new TableLayoutPanel();
            panelPrivateKeyBox = new Panel();
            labelPrivateKey = new Label();
            textPrivateKey = new TextBox();
            panelPrivateKeyActions = new TableLayoutPanel();
            labelPrivateActionsTitle = new Label();
            btnCopyPrivateKey = new Button();
            btnPastePrivateKey = new Button();
            btnImportPrivateKey = new Button();
            btnSavePrivateKey = new Button();
            btnClearPrivateKey = new Button();
            panelPublicKeyBox = new Panel();
            labelPublicKey = new Label();
            textPublicKey = new TextBox();
            panelPublicKeyActions = new TableLayoutPanel();
            labelPublicActionsTitle = new Label();
            btnCopyPublicKey = new Button();
            btnPastePublicKey = new Button();
            btnImportPublicKey = new Button();
            btnSavePublicKey = new Button();
            btnClearPublicKey = new Button();
            panelActionButtons = new Panel();
            tableActionButtons = new TableLayoutPanel();
            groupKeyActions = new GroupBox();
            tableKeyActions = new TableLayoutPanel();
            panelButtonArea = new Panel();
            tableButtonArea = new TableLayoutPanel();
            tableRightActions = new TableLayoutPanel();
            panelKeyControlsContainer = new Panel();
            panelKeyControls = new FlowLayoutPanel();
            btnGenerateKeyPair = new Button();
            btnValidateKeyPair = new Button();
            btnGetPublicKeyFromPrivate = new Button();
            btnGetCurveType = new Button();
            btnClearAll = new Button();
            panelRightSettings = new FlowLayoutPanel();
            panelFormatRow = new FlowLayoutPanel();
            labelOutputFormat = new Label();
            comboOutputFormat = new ComboBox();
            panelKeyTypeRow = new FlowLayoutPanel();
            labelKeyType = new Label();
            radioPanel = new FlowLayoutPanel();
            radioPrivateKey = new RadioButton();
            radioPublicKey = new RadioButton();
            btnConvertKey = new Button();
            panelCurveContainer = new Panel();
            panelCurveRow = new FlowLayoutPanel();
            labelCurve = new Label();
            comboCategory = new ComboBox();
            lblArrow = new Label();
            comboCurve = new ComboBox();
            groupComputeResult = new GroupBox();
            textKeyResult = new RichTextBox();
            groupRunResult = new GroupBox();
            labelValidationResult = new RichTextBox();
            panelViewBar = new FlowLayoutPanel();
            btnViewEcdh = new Button();
            btnViewSign = new Button();
            btnViewEncrypt = new Button();
            btnViewFile = new Button();
            panelViewContent = new Panel();
            groupSign = new Panel();
            tableLayoutSign = new TableLayoutPanel();
            groupSignInput = new GroupBox();
            panelSignInput = new TableLayoutPanel();
            panelPlainDataBox = new Panel();
            labelPlainData = new Label();
            textPlainData = new TextBox();
            panelPlainDataActions = new TableLayoutPanel();
            labelPlainDataActionsTitle = new Label();
            btnCopyPlainData = new Button();
            btnPastePlainData = new Button();
            btnClearPlainData = new Button();
            panelSignatureBox = new Panel();
            labelSignature = new Label();
            textSignature = new TextBox();
            panelSignatureActions = new TableLayoutPanel();
            labelSignatureActionsTitle = new Label();
            btnCopySignatureData = new Button();
            btnPasteSignatureData = new Button();
            btnClearSignatureData = new Button();
            groupSignActions = new GroupBox();
            panelSignActions = new TableLayoutPanel();
            btnSign = new Button();
            btnVerify = new Button();
            btnCopySignature = new Button();
            panelSignOptions = new TableLayoutPanel();
            labelHashAlgorithm = new Label();
            comboHashAlgorithm = new ComboBox();
            labelSignatureFormat = new Label();
            comboSignatureFormat = new ComboBox();
            groupEncrypt = new Panel();
            tableLayoutEncrypt = new TableLayoutPanel();
            labelEncMode = new Label();
            comboEncMode = new ComboBox();
            labelEncInputFormat = new Label();
            comboEncInputFormat = new ComboBox();
            labelEncOutputFormat = new Label();
            comboEncOutputFormat = new ComboBox();
            labelEncKey = new Label();
            textEncKey = new TextBox();
            labelEncIV = new Label();
            textEncIV = new TextBox();
            labelEncBobPublic = new Label();
            textEncBobPublic = new TextBox();
            labelEncCurve = new Label();
            comboEncCurveCategory = new ComboBox();
            labelEncCurveArrow = new Label();
            comboEncCurve = new ComboBox();
            labelEncInput = new Label();
            textEncInput = new TextBox();
            textEncOutput = new TextBox();
            panelEncBtns = new FlowLayoutPanel();
            btnEncrypt = new Button();
            btnDecrypt = new Button();
            btnEncClear = new Button();
            btnEncCopy = new Button();
            btnEncPaste = new Button();
            labelEncOutputLabel = new Label();
            groupFile = new GroupBox();
            panelFileControls = new FlowLayoutPanel();
            btnSignFile = new Button();
            btnVerifyFile = new Button();
            groupEcdh = new Panel();
            labelEncResult = new TextBox();
            splitSignEncrypt = new SplitContainer();
            groupEncFile = new GroupBox();
            panelEncFileBtns = new FlowLayoutPanel();
            btnEncryptFile = new Button();
            btnDecryptFile = new Button();
            splitFileResult = new SplitContainer();
            // ECDH 视图专属控件
            comboEcdhMode = new ComboBox();
            comboEcdhCategory = new ComboBox();
            comboEcdhCurve = new ComboBox();
            textEcdhAlicePrivate = new TextBox();
            textEcdhAlicePublic = new TextBox();
            textEcdhBobPrivate = new TextBox();
            textEcdhBobPublic = new TextBox();
            textEcdhSharedKey = new TextBox();
            textEcdhIV = new TextBox();
            textEcdhInput = new TextBox();
            textEcdhOutput = new TextBox();
            btnGenerateEcdhKeys = new Button();
            btnEcdhEncrypt = new Button();
            btnEcdhDecrypt = new Button();
            btnEcdhCopyResult = new Button();
            btnEcdhPasteInput = new Button();
            btnEcdhClear = new Button();
            comboEcdhEncoding = new ComboBox();
            labelEcdhMode = new Label();
            labelEcdhCategory = new Label();
            labelEcdhCurve = new Label();
            labelEcdhAlicePriv = new Label();
            labelEcdhAlicePub = new Label();
            labelEcdhBobPriv = new Label();
            labelEcdhBobPub = new Label();
            labelEcdhShared = new Label();
            labelEcdhIV = new Label();
            labelEcdhInput = new Label();
            labelEcdhOutput = new Label();
            labelEcdhEncoding = new Label();
            mainTableLayout.SuspendLayout();
            groupKey.SuspendLayout();
            tableLayoutKey.SuspendLayout();
            panelPrivateKeyBox.SuspendLayout();
            panelPrivateKeyActions.SuspendLayout();
            panelPublicKeyBox.SuspendLayout();
            panelPublicKeyActions.SuspendLayout();
            panelActionButtons.SuspendLayout();
            tableActionButtons.SuspendLayout();
            groupKeyActions.SuspendLayout();
            tableKeyActions.SuspendLayout();
            panelButtonArea.SuspendLayout();
            tableButtonArea.SuspendLayout();
            tableRightActions.SuspendLayout();
            panelKeyControlsContainer.SuspendLayout();
            panelKeyControls.SuspendLayout();
            panelRightSettings.SuspendLayout();
            panelFormatRow.SuspendLayout();
            panelKeyTypeRow.SuspendLayout();
            radioPanel.SuspendLayout();
            panelCurveContainer.SuspendLayout();
            panelCurveRow.SuspendLayout();
            groupComputeResult.SuspendLayout();
            groupRunResult.SuspendLayout();
            panelViewBar.SuspendLayout();
            panelViewContent.SuspendLayout();
            groupSign.SuspendLayout();
            tableLayoutSign.SuspendLayout();
            groupSignInput.SuspendLayout();
            panelSignInput.SuspendLayout();
            panelPlainDataBox.SuspendLayout();
            panelPlainDataActions.SuspendLayout();
            panelSignatureBox.SuspendLayout();
            panelSignatureActions.SuspendLayout();
            groupSignActions.SuspendLayout();
            panelSignActions.SuspendLayout();
            panelSignOptions.SuspendLayout();
            groupEncrypt.SuspendLayout();
            tableLayoutEncrypt.SuspendLayout();
            panelEncBtns.SuspendLayout();
            groupFile.SuspendLayout();
            panelFileControls.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitSignEncrypt).BeginInit();
            splitSignEncrypt.SuspendLayout();
            groupEncFile.SuspendLayout();
            panelEncFileBtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitFileResult).BeginInit();
            splitFileResult.SuspendLayout();

            // ==================================================
            // 主布局 - 整体页面为2列：左(密钥面板) | 右(操作面板)
            // ==================================================
            mainTableLayout.ColumnCount = 2;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            mainTableLayout.Controls.Add(groupKey, 0, 0);
            mainTableLayout.Controls.Add(panelActionButtons, 1, 0);
            mainTableLayout.Controls.Add(panelViewBar, 0, 1);
            mainTableLayout.Controls.Add(panelViewContent, 0, 2);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(8);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 55F));
            mainTableLayout.Size = new Size(3287, 1616);
            mainTableLayout.TabIndex = 0;

            // ==================================================
            // 密钥管理区 - 整个密钥管理 GroupBox（左列上半部分）
            //   内嵌 tableLayoutKey (2列): 私钥列 | 公钥列 + 操作按钮列
            // ==================================================
            groupKey.Controls.Add(tableLayoutKey);
            groupKey.Dock = DockStyle.Fill;
            groupKey.Location = new Point(11, 11);
            groupKey.Name = "groupKey";
            groupKey.Padding = new Padding(8);
            groupKey.Size = new Size(1629, 687);
            groupKey.TabIndex = 0;
            groupKey.TabStop = false;
            groupKey.Text = "密钥生成";

            // ---- tableLayoutKey: 2列(PEM输入框 | 操作按钮) × 4行(私/间隔/公/间隔) ----
            tableLayoutKey.ColumnCount = 2;
            tableLayoutKey.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutKey.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            tableLayoutKey.Controls.Add(panelPrivateKeyBox, 0, 0);
            tableLayoutKey.Controls.Add(panelPrivateKeyActions, 1, 0);
            tableLayoutKey.Controls.Add(panelPublicKeyBox, 0, 2);
            tableLayoutKey.Controls.Add(panelPublicKeyActions, 1, 2);
            tableLayoutKey.Dock = DockStyle.Fill;
            tableLayoutKey.Location = new Point(8, 31);
            tableLayoutKey.Name = "tableLayoutKey";
            tableLayoutKey.Padding = new Padding(6);
            tableLayoutKey.RowCount = 4;
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            tableLayoutKey.Size = new Size(1613, 648);
            tableLayoutKey.TabIndex = 0;

            // ---- 私钥面板 (列0行0, 跨2行) ----
            panelPrivateKeyBox.Controls.Add(labelPrivateKey);
            panelPrivateKeyBox.Controls.Add(textPrivateKey);
            panelPrivateKeyBox.Dock = DockStyle.Fill;
            panelPrivateKeyBox.Location = new Point(9, 9);
            panelPrivateKeyBox.Name = "panelPrivateKeyBox";
            tableLayoutKey.SetRowSpan(panelPrivateKeyBox, 2);
            panelPrivateKeyBox.Size = new Size(1395, 312);
            panelPrivateKeyBox.TabIndex = 6;

            labelPrivateKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelPrivateKey.AutoSize = true;
            labelPrivateKey.BackColor = Color.Transparent;
            labelPrivateKey.Location = new Point(1263, 4);
            labelPrivateKey.Margin = new Padding(4, 4, 4, 2);
            labelPrivateKey.Name = "labelPrivateKey";
            labelPrivateKey.Padding = new Padding(4, 0, 4, 0);
            labelPrivateKey.Size = new Size(128, 24);
            labelPrivateKey.TabIndex = 0;
            labelPrivateKey.Text = "私钥 (PEM)：";

            textPrivateKey.Dock = DockStyle.Fill;
            textPrivateKey.Location = new Point(0, 0);
            textPrivateKey.Multiline = true;
            textPrivateKey.Name = "textPrivateKey";
            textPrivateKey.ScrollBars = ScrollBars.Vertical;
            textPrivateKey.Size = new Size(1395, 312);
            textPrivateKey.TabIndex = 1;
            textPrivateKey.TextChanged += TextPrivateKey_TextChanged;

            // ---- 私钥操作按钮列 (列1行0, 跨2行): 6行网格 ----
            panelPrivateKeyActions.ColumnCount = 1;
            panelPrivateKeyActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelPrivateKeyActions.Controls.Add(labelPrivateActionsTitle, 0, 0);
            panelPrivateKeyActions.Controls.Add(btnCopyPrivateKey, 0, 1);
            panelPrivateKeyActions.Controls.Add(btnPastePrivateKey, 0, 2);
            panelPrivateKeyActions.Controls.Add(btnImportPrivateKey, 0, 3);
            panelPrivateKeyActions.Controls.Add(btnSavePrivateKey, 0, 4);
            panelPrivateKeyActions.Controls.Add(btnClearPrivateKey, 0, 5);
            panelPrivateKeyActions.Dock = DockStyle.Fill;
            panelPrivateKeyActions.Location = new Point(1410, 9);
            panelPrivateKeyActions.Name = "panelPrivateKeyActions";
            panelPrivateKeyActions.Padding = new Padding(8, 4, 8, 4);
            panelPrivateKeyActions.RowCount = 6;
            tableLayoutKey.SetRowSpan(panelPrivateKeyActions, 2);
            panelPrivateKeyActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            panelPrivateKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPrivateKeyActions.Size = new Size(194, 312);
            panelPrivateKeyActions.TabIndex = 4;

            labelPrivateActionsTitle.AutoSize = true;
            labelPrivateActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPrivateActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelPrivateActionsTitle.Location = new Point(8, 4);
            labelPrivateActionsTitle.Margin = new Padding(0);
            labelPrivateActionsTitle.Name = "labelPrivateActionsTitle";
            labelPrivateActionsTitle.Size = new Size(123, 25);
            labelPrivateActionsTitle.TabIndex = 0;
            labelPrivateActionsTitle.Text = "🔑 私钥操作";

            btnCopyPrivateKey.Dock = DockStyle.Fill;
            btnCopyPrivateKey.Location = new Point(10, 32);
            btnCopyPrivateKey.Margin = new Padding(2);
            btnCopyPrivateKey.MinimumSize = new Size(120, 30);
            btnCopyPrivateKey.Name = "btnCopyPrivateKey";
            btnCopyPrivateKey.Size = new Size(174, 51);
            btnCopyPrivateKey.TabIndex = 0;
            btnCopyPrivateKey.Text = "复制私钥";
            btnCopyPrivateKey.Click += BtnCopyPrivateKey_Click;

            btnPastePrivateKey.Dock = DockStyle.Fill;
            btnPastePrivateKey.Location = new Point(10, 87);
            btnPastePrivateKey.Margin = new Padding(2);
            btnPastePrivateKey.MinimumSize = new Size(120, 30);
            btnPastePrivateKey.Name = "btnPastePrivateKey";
            btnPastePrivateKey.Size = new Size(174, 51);
            btnPastePrivateKey.TabIndex = 1;
            btnPastePrivateKey.Text = "粘贴私钥";
            btnPastePrivateKey.Click += BtnPastePrivateKey_Click;

            btnImportPrivateKey.Dock = DockStyle.Fill;
            btnImportPrivateKey.Location = new Point(10, 142);
            btnImportPrivateKey.Margin = new Padding(2);
            btnImportPrivateKey.MinimumSize = new Size(120, 30);
            btnImportPrivateKey.Name = "btnImportPrivateKey";
            btnImportPrivateKey.Size = new Size(174, 51);
            btnImportPrivateKey.TabIndex = 2;
            btnImportPrivateKey.Text = "导入私钥";
            btnImportPrivateKey.Click += BtnImportPrivateKey_Click;

            btnSavePrivateKey.Dock = DockStyle.Fill;
            btnSavePrivateKey.Location = new Point(10, 197);
            btnSavePrivateKey.Margin = new Padding(2);
            btnSavePrivateKey.MinimumSize = new Size(120, 30);
            btnSavePrivateKey.Name = "btnSavePrivateKey";
            btnSavePrivateKey.Size = new Size(174, 51);
            btnSavePrivateKey.TabIndex = 3;
            btnSavePrivateKey.Text = "保存私钥";
            btnSavePrivateKey.Click += BtnSavePrivateKey_Click;

            btnClearPrivateKey.Dock = DockStyle.Fill;
            btnClearPrivateKey.Location = new Point(10, 252);
            btnClearPrivateKey.Margin = new Padding(2);
            btnClearPrivateKey.MinimumSize = new Size(120, 30);
            btnClearPrivateKey.Name = "btnClearPrivateKey";
            btnClearPrivateKey.Size = new Size(174, 54);
            btnClearPrivateKey.TabIndex = 4;
            btnClearPrivateKey.Text = "清空私钥";
            btnClearPrivateKey.Click += BtnClearPrivateKey_Click;

            // ---- 公钥面板 (列0行2, 跨2行) ----
            panelPublicKeyBox.Controls.Add(labelPublicKey);
            panelPublicKeyBox.Controls.Add(textPublicKey);
            panelPublicKeyBox.Dock = DockStyle.Fill;
            panelPublicKeyBox.Location = new Point(9, 327);
            panelPublicKeyBox.Name = "panelPublicKeyBox";
            tableLayoutKey.SetRowSpan(panelPublicKeyBox, 2);
            panelPublicKeyBox.Size = new Size(1395, 312);
            panelPublicKeyBox.TabIndex = 7;

            labelPublicKey.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelPublicKey.AutoSize = true;
            labelPublicKey.BackColor = Color.Transparent;
            labelPublicKey.Location = new Point(1263, 4);
            labelPublicKey.Margin = new Padding(4, 4, 4, 2);
            labelPublicKey.Name = "labelPublicKey";
            labelPublicKey.Padding = new Padding(4, 0, 4, 0);
            labelPublicKey.Size = new Size(128, 24);
            labelPublicKey.TabIndex = 2;
            labelPublicKey.Text = "公钥 (PEM)：";

            textPublicKey.Dock = DockStyle.Fill;
            textPublicKey.Location = new Point(0, 0);
            textPublicKey.Multiline = true;
            textPublicKey.Name = "textPublicKey";
            textPublicKey.ScrollBars = ScrollBars.Vertical;
            textPublicKey.Size = new Size(1395, 312);
            textPublicKey.TabIndex = 3;

            // ---- 公钥操作按钮列 (列1行2, 跨2行): 6行网格 ----
            panelPublicKeyActions.ColumnCount = 1;
            panelPublicKeyActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelPublicKeyActions.Controls.Add(labelPublicActionsTitle, 0, 0);
            panelPublicKeyActions.Controls.Add(btnCopyPublicKey, 0, 1);
            panelPublicKeyActions.Controls.Add(btnPastePublicKey, 0, 2);
            panelPublicKeyActions.Controls.Add(btnImportPublicKey, 0, 3);
            panelPublicKeyActions.Controls.Add(btnSavePublicKey, 0, 4);
            panelPublicKeyActions.Controls.Add(btnClearPublicKey, 0, 5);
            panelPublicKeyActions.Dock = DockStyle.Fill;
            panelPublicKeyActions.Location = new Point(1410, 327);
            panelPublicKeyActions.Name = "panelPublicKeyActions";
            panelPublicKeyActions.Padding = new Padding(8, 4, 8, 4);
            panelPublicKeyActions.RowCount = 6;
            tableLayoutKey.SetRowSpan(panelPublicKeyActions, 2);
            panelPublicKeyActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            panelPublicKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            panelPublicKeyActions.Size = new Size(194, 312);
            panelPublicKeyActions.TabIndex = 5;

            labelPublicActionsTitle.AutoSize = true;
            labelPublicActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPublicActionsTitle.ForeColor = Color.FromArgb(0, 100, 180);
            labelPublicActionsTitle.Location = new Point(8, 4);
            labelPublicActionsTitle.Margin = new Padding(0);
            labelPublicActionsTitle.Name = "labelPublicActionsTitle";
            labelPublicActionsTitle.Size = new Size(123, 25);
            labelPublicActionsTitle.TabIndex = 0;
            labelPublicActionsTitle.Text = "🔓 公钥操作";

            btnCopyPublicKey.Dock = DockStyle.Fill;
            btnCopyPublicKey.Location = new Point(10, 32);
            btnCopyPublicKey.Margin = new Padding(2);
            btnCopyPublicKey.MinimumSize = new Size(120, 30);
            btnCopyPublicKey.Name = "btnCopyPublicKey";
            btnCopyPublicKey.Size = new Size(174, 51);
            btnCopyPublicKey.TabIndex = 0;
            btnCopyPublicKey.Text = "复制公钥";
            btnCopyPublicKey.Click += BtnCopyPublicKey_Click;

            btnPastePublicKey.Dock = DockStyle.Fill;
            btnPastePublicKey.Location = new Point(10, 87);
            btnPastePublicKey.Margin = new Padding(2);
            btnPastePublicKey.MinimumSize = new Size(120, 30);
            btnPastePublicKey.Name = "btnPastePublicKey";
            btnPastePublicKey.Size = new Size(174, 51);
            btnPastePublicKey.TabIndex = 1;
            btnPastePublicKey.Text = "粘贴公钥";
            btnPastePublicKey.Click += BtnPastePublicKey_Click;

            btnImportPublicKey.Dock = DockStyle.Fill;
            btnImportPublicKey.Location = new Point(10, 142);
            btnImportPublicKey.Margin = new Padding(2);
            btnImportPublicKey.MinimumSize = new Size(120, 30);
            btnImportPublicKey.Name = "btnImportPublicKey";
            btnImportPublicKey.Size = new Size(174, 51);
            btnImportPublicKey.TabIndex = 2;
            btnImportPublicKey.Text = "导入公钥";
            btnImportPublicKey.Click += BtnImportPublicKey_Click;

            btnSavePublicKey.Dock = DockStyle.Fill;
            btnSavePublicKey.Location = new Point(10, 197);
            btnSavePublicKey.Margin = new Padding(2);
            btnSavePublicKey.MinimumSize = new Size(120, 30);
            btnSavePublicKey.Name = "btnSavePublicKey";
            btnSavePublicKey.Size = new Size(174, 51);
            btnSavePublicKey.TabIndex = 3;
            btnSavePublicKey.Text = "保存公钥";
            btnSavePublicKey.Click += BtnSavePublicKey_Click;

            btnClearPublicKey.Dock = DockStyle.Fill;
            btnClearPublicKey.Location = new Point(10, 252);
            btnClearPublicKey.Margin = new Padding(2);
            btnClearPublicKey.MinimumSize = new Size(120, 30);
            btnClearPublicKey.Name = "btnClearPublicKey";
            btnClearPublicKey.Size = new Size(174, 54);
            btnClearPublicKey.TabIndex = 4;
            btnClearPublicKey.Text = "清空公钥";
            btnClearPublicKey.Click += BtnClearPublicKey_Click;

            // ==================================================
            // 密钥操作面板区 - mainTableLayout 右侧列
            //   panelActionButtons > tableActionButtons(2列) > groupKeyActions + 结果区
            // ==================================================
            panelActionButtons.Controls.Add(tableActionButtons);
            panelActionButtons.Dock = DockStyle.Fill;
            panelActionButtons.Location = new Point(1643, 8);
            panelActionButtons.Margin = new Padding(0);
            panelActionButtons.Name = "panelActionButtons";
            panelActionButtons.Size = new Size(1636, 693);
            panelActionButtons.TabIndex = 1;

            // ---- tableActionButtons: 2列 × 1行, 左右平分 ----
            tableActionButtons.ColumnCount = 2;
            tableActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableActionButtons.Controls.Add(groupKeyActions, 0, 0);
            tableActionButtons.Dock = DockStyle.Fill;
            tableActionButtons.Location = new Point(0, 0);
            tableActionButtons.Margin = new Padding(0);
            tableActionButtons.Name = "tableActionButtons";
            tableActionButtons.RowCount = 1;
            tableActionButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableActionButtons.Size = new Size(1636, 693);
            tableActionButtons.TabIndex = 0;

            // ---- groupKeyActions: 密钥操作按钮区, 跨2列 ----
            tableActionButtons.SetColumnSpan(groupKeyActions, 2);
            groupKeyActions.Controls.Add(tableKeyActions);
            groupKeyActions.Dock = DockStyle.Fill;
            groupKeyActions.Location = new Point(3, 3);
            groupKeyActions.Name = "groupKeyActions";
            groupKeyActions.Padding = new Padding(8);
            groupKeyActions.Size = new Size(1630, 687);
            groupKeyActions.TabIndex = 1;
            groupKeyActions.TabStop = false;
            groupKeyActions.Text = "密钥操作";

            // ---- tableKeyActions: 2列 × 2行 (上:按钮区 | 下:结果区) ----
            tableKeyActions.ColumnCount = 2;
            tableKeyActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableKeyActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableKeyActions.Controls.Add(panelButtonArea, 0, 0);
            tableKeyActions.Controls.Add(groupComputeResult, 0, 1);
            tableKeyActions.Controls.Add(groupRunResult, 1, 1);
            tableKeyActions.Dock = DockStyle.Fill;
            tableKeyActions.Location = new Point(8, 31);
            tableKeyActions.Name = "tableKeyActions";
            tableKeyActions.RowCount = 2;
            tableKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableKeyActions.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableKeyActions.Size = new Size(1614, 648);
            tableKeyActions.TabIndex = 0;

            // ---- panelButtonArea: 按钮区, 跨2列 ----
            tableKeyActions.SetColumnSpan(panelButtonArea, 2);
            panelButtonArea.Controls.Add(tableButtonArea);
            panelButtonArea.Dock = DockStyle.Fill;
            panelButtonArea.Location = new Point(3, 3);
            panelButtonArea.Name = "panelButtonArea";
            panelButtonArea.Size = new Size(1608, 318);
            panelButtonArea.TabIndex = 1;

            // ---- tableButtonArea: 1列 × 1行, 内含 tableRightActions ----
            tableButtonArea.ColumnCount = 1;
            tableButtonArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableButtonArea.Controls.Add(tableRightActions, 0, 0);
            tableButtonArea.Dock = DockStyle.Fill;
            tableButtonArea.Location = new Point(0, 0);
            tableButtonArea.Name = "tableButtonArea";
            tableButtonArea.RowCount = 1;
            tableButtonArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableButtonArea.Size = new Size(1608, 318);
            tableButtonArea.TabIndex = 0;

            // ---- tableRightActions: 2列(按钮列180px | 设置区) × 3行 ----
            tableRightActions.ColumnCount = 2;
            tableRightActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tableRightActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableRightActions.Controls.Add(panelKeyControlsContainer, 0, 0);
            tableRightActions.Controls.Add(panelRightSettings, 1, 0);
            tableRightActions.Dock = DockStyle.Fill;
            tableRightActions.Location = new Point(3, 3);
            tableRightActions.Name = "tableRightActions";
            tableRightActions.RowCount = 3;
            tableRightActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableRightActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 75F));
            tableRightActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 70F));
            tableRightActions.Size = new Size(1602, 312);
            tableRightActions.TabIndex = 0;

            // ---- 左侧: 密钥操作按钮容器 (FlowLayout 纵向排列, 跨3行) ----
            panelKeyControlsContainer.Controls.Add(panelKeyControls);
            panelKeyControlsContainer.Dock = DockStyle.Fill;
            panelKeyControlsContainer.Location = new Point(3, 3);
            panelKeyControlsContainer.Name = "panelKeyControlsContainer";
            panelKeyControlsContainer.Padding = new Padding(6, 0, 6, 0);
            tableRightActions.SetRowSpan(panelKeyControlsContainer, 3);
            panelKeyControlsContainer.Size = new Size(174, 306);
            panelKeyControlsContainer.TabIndex = 1;

            panelKeyControls.Controls.Add(btnGenerateKeyPair);
            panelKeyControls.Controls.Add(btnValidateKeyPair);
            panelKeyControls.Controls.Add(btnGetPublicKeyFromPrivate);
            panelKeyControls.Controls.Add(btnGetCurveType);
            panelKeyControls.Controls.Add(btnClearAll);
            panelKeyControls.Dock = DockStyle.Fill;
            panelKeyControls.FlowDirection = FlowDirection.TopDown;
            panelKeyControls.Location = new Point(6, 0);
            panelKeyControls.Name = "panelKeyControls";
            panelKeyControls.Size = new Size(162, 306);
            panelKeyControls.TabIndex = 1;
            panelKeyControls.WrapContents = false;

            // 生成密钥对
            btnGenerateKeyPair.Location = new Point(0, 0);
            btnGenerateKeyPair.Margin = new Padding(0, 0, 6, 0);
            btnGenerateKeyPair.Name = "btnGenerateKeyPair";
            btnGenerateKeyPair.Size = new Size(150, 40);
            btnGenerateKeyPair.TabIndex = 0;
            btnGenerateKeyPair.Text = "生成密钥对";
            btnGenerateKeyPair.Click += BtnGenerateKeyPair_Click;

            // 验证密钥对
            btnValidateKeyPair.Location = new Point(0, 43);
            btnValidateKeyPair.Margin = new Padding(0, 3, 6, 0);
            btnValidateKeyPair.Name = "btnValidateKeyPair";
            btnValidateKeyPair.Size = new Size(150, 40);
            btnValidateKeyPair.TabIndex = 1;
            btnValidateKeyPair.Text = "验证密钥对";
            btnValidateKeyPair.Click += BtnValidateKeyPair_Click;

            // 从私钥提取公钥
            btnGetPublicKeyFromPrivate.Location = new Point(0, 86);
            btnGetPublicKeyFromPrivate.Margin = new Padding(0, 3, 6, 0);
            btnGetPublicKeyFromPrivate.Name = "btnGetPublicKeyFromPrivate";
            btnGetPublicKeyFromPrivate.Size = new Size(150, 40);
            btnGetPublicKeyFromPrivate.TabIndex = 2;
            btnGetPublicKeyFromPrivate.Text = "从私钥提取公钥";
            btnGetPublicKeyFromPrivate.Click += BtnGetPublicKeyFromPrivate_Click;

            // 获取私钥曲线类型
            btnGetCurveType.Location = new Point(0, 129);
            btnGetCurveType.Margin = new Padding(0, 3, 6, 0);
            btnGetCurveType.Name = "btnGetCurveType";
            btnGetCurveType.Size = new Size(150, 40);
            btnGetCurveType.TabIndex = 3;
            btnGetCurveType.Text = "获取私钥曲线类型";
            btnGetCurveType.Click += BtnGetCurveType_Click;

            // 清空全部
            btnClearAll.Location = new Point(0, 172);
            btnClearAll.Margin = new Padding(0, 3, 6, 0);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.Size = new Size(150, 40);
            btnClearAll.TabIndex = 4;
            btnClearAll.Text = "清空全部";
            btnClearAll.Click += BtnClearAll_Click;

            // ---- 右侧: 设置面板 (跨3行, 纵向FlowLayout) ----
            panelRightSettings.AutoSize = true;
            panelRightSettings.Controls.Add(panelFormatRow);
            panelRightSettings.Controls.Add(panelKeyTypeRow);
            panelRightSettings.Controls.Add(panelCurveContainer);
            panelRightSettings.Dock = DockStyle.Top;
            panelRightSettings.FlowDirection = FlowDirection.TopDown;
            panelRightSettings.Location = new Point(183, 3);
            panelRightSettings.Name = "panelRightSettings";
            tableRightActions.SetRowSpan(panelRightSettings, 3);
            panelRightSettings.Size = new Size(1416, 134);
            panelRightSettings.TabIndex = 2;
            panelRightSettings.WrapContents = false;

            // ---- 第1行: 输出格式 (PEM/Base64/Hex) ----
            panelFormatRow.AutoSize = true;
            panelFormatRow.Controls.Add(labelOutputFormat);
            panelFormatRow.Controls.Add(comboOutputFormat);
            panelFormatRow.Location = new Point(3, 3);
            panelFormatRow.Name = "panelFormatRow";
            panelFormatRow.Padding = new Padding(6, 0, 6, 0);
            panelFormatRow.Size = new Size(303, 38);
            panelFormatRow.TabIndex = 0;

            labelOutputFormat.Location = new Point(40, 3);
            labelOutputFormat.Margin = new Padding(34, 3, 2, 3);
            labelOutputFormat.Name = "labelOutputFormat";
            labelOutputFormat.Size = new Size(100, 32);
            labelOutputFormat.TabIndex = 2;
            labelOutputFormat.Text = "输出格式：";
            labelOutputFormat.TextAlign = ContentAlignment.MiddleLeft;

            comboOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboOutputFormat.FormattingEnabled = true;
            comboOutputFormat.Items.AddRange(new object[] { "PEM", "Base64", "Hex大写", "Hex小写" });
            comboOutputFormat.Location = new Point(142, 3);
            comboOutputFormat.Margin = new Padding(0, 3, 8, 3);
            comboOutputFormat.Name = "comboOutputFormat";
            comboOutputFormat.Size = new Size(147, 32);
            comboOutputFormat.TabIndex = 3;
            comboOutputFormat.SelectedIndexChanged += ComboOutputFormat_SelectedIndexChanged;

            // ---- 第2行: 密钥类型转换 (私钥↔公钥) ----
            panelKeyTypeRow.AutoSize = true;
            panelKeyTypeRow.Controls.Add(labelKeyType);
            panelKeyTypeRow.Controls.Add(radioPanel);
            panelKeyTypeRow.Controls.Add(btnConvertKey);
            panelKeyTypeRow.Location = new Point(3, 47);
            panelKeyTypeRow.Name = "panelKeyTypeRow";
            panelKeyTypeRow.Padding = new Padding(6, 0, 6, 0);
            panelKeyTypeRow.Size = new Size(409, 40);
            panelKeyTypeRow.TabIndex = 1;

            labelKeyType.Location = new Point(40, 3);
            labelKeyType.Margin = new Padding(34, 3, 2, 3);
            labelKeyType.Name = "labelKeyType";
            labelKeyType.Size = new Size(100, 34);
            labelKeyType.TabIndex = 4;
            labelKeyType.Text = "密钥类型：";
            labelKeyType.TextAlign = ContentAlignment.MiddleLeft;

            radioPanel.AutoSize = true;
            radioPanel.Controls.Add(radioPrivateKey);
            radioPanel.Controls.Add(radioPublicKey);
            radioPanel.Location = new Point(142, 3);
            radioPanel.Margin = new Padding(0, 3, 8, 3);
            radioPanel.Name = "radioPanel";
            radioPanel.Size = new Size(157, 34);
            radioPanel.TabIndex = 5;

            radioPrivateKey.AutoSize = true;
            radioPrivateKey.Checked = true;
            radioPrivateKey.Location = new Point(3, 3);
            radioPrivateKey.Margin = new Padding(3, 3, 6, 3);
            radioPrivateKey.Name = "radioPrivateKey";
            radioPrivateKey.Size = new Size(71, 28);
            radioPrivateKey.TabIndex = 0;
            radioPrivateKey.TabStop = true;
            radioPrivateKey.Text = "私钥";

            radioPublicKey.AutoSize = true;
            radioPublicKey.Location = new Point(83, 3);
            radioPublicKey.Name = "radioPublicKey";
            radioPublicKey.Size = new Size(71, 28);
            radioPublicKey.TabIndex = 1;
            radioPublicKey.Text = "公钥";

            btnConvertKey.AutoSize = true;
            btnConvertKey.Location = new Point(319, 3);
            btnConvertKey.Margin = new Padding(12, 3, 4, 3);
            btnConvertKey.MinimumSize = new Size(80, 26);
            btnConvertKey.Name = "btnConvertKey";
            btnConvertKey.Size = new Size(80, 34);
            btnConvertKey.TabIndex = 6;
            btnConvertKey.Text = "转换";
            btnConvertKey.Click += BtnConvertKey_Click;

            // ---- 第3行: 椭圆曲线选择 (类别 → 曲线名) ----
            panelCurveContainer.Controls.Add(panelCurveRow);
            panelCurveContainer.Location = new Point(3, 93);
            panelCurveContainer.Name = "panelCurveContainer";
            panelCurveContainer.Padding = new Padding(6, 0, 6, 0);
            panelCurveContainer.Size = new Size(920, 38);
            panelCurveContainer.TabIndex = 2;

            panelCurveRow.Controls.Add(labelCurve);
            panelCurveRow.Controls.Add(comboCategory);
            panelCurveRow.Controls.Add(lblArrow);
            panelCurveRow.Controls.Add(comboCurve);
            panelCurveRow.Location = new Point(6, 0);
            panelCurveRow.Name = "panelCurveRow";
            panelCurveRow.Padding = new Padding(0, 2, 0, 2);
            panelCurveRow.Size = new Size(900, 38);
            panelCurveRow.TabIndex = 1;
            panelCurveRow.WrapContents = false;

            labelCurve.Location = new Point(34, 5);
            labelCurve.Margin = new Padding(34, 3, 2, 3);
            labelCurve.Name = "labelCurve";
            labelCurve.Size = new Size(100, 32);
            labelCurve.TabIndex = 0;
            labelCurve.Text = "椭圆曲线：";
            labelCurve.TextAlign = ContentAlignment.MiddleLeft;
            labelCurve.Click += LabelCurve_Click;

            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new Point(136, 5);
            comboCategory.Margin = new Padding(0, 3, 4, 3);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new Size(183, 32);
            comboCategory.TabIndex = 1;
            comboCategory.SelectedIndexChanged += ComboCategory_SelectedIndexChanged;

            lblArrow.Location = new Point(327, 5);
            lblArrow.Margin = new Padding(4, 3, 4, 3);
            lblArrow.Name = "lblArrow";
            lblArrow.Padding = new Padding(4, 0, 4, 0);
            lblArrow.Size = new Size(36, 32);
            lblArrow.TabIndex = 2;
            lblArrow.Text = "→";
            lblArrow.TextAlign = ContentAlignment.MiddleCenter;

            comboCurve.DisplayMember = "Value";
            comboCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCurve.FormattingEnabled = true;
            comboCurve.Location = new Point(367, 5);
            comboCurve.Margin = new Padding(0, 3, 4, 3);
            comboCurve.Name = "comboCurve";
            comboCurve.Size = new Size(520, 32);
            comboCurve.TabIndex = 3;
            comboCurve.ValueMember = "Key";

            // ---- groupComputeResult: 计算结果输出框 (行1左) ----
            groupComputeResult.Controls.Add(textKeyResult);
            groupComputeResult.Dock = DockStyle.Fill;
            groupComputeResult.Location = new Point(3, 327);
            groupComputeResult.Name = "groupComputeResult";
            groupComputeResult.Padding = new Padding(8);
            groupComputeResult.Size = new Size(801, 318);
            groupComputeResult.TabIndex = 0;
            groupComputeResult.TabStop = false;
            groupComputeResult.Text = "计算结果";

            textKeyResult.BackColor = Color.White;
            textKeyResult.BorderStyle = BorderStyle.None;
            textKeyResult.Dock = DockStyle.Fill;
            textKeyResult.Font = new Font("Segoe UI", 9F);
            textKeyResult.Location = new Point(8, 31);
            textKeyResult.Name = "textKeyResult";
            textKeyResult.ReadOnly = true;
            textKeyResult.ScrollBars = RichTextBoxScrollBars.Vertical;
            textKeyResult.Size = new Size(785, 279);
            textKeyResult.TabIndex = 1;
            textKeyResult.Text = "从私钥提取/曲线检测：\n等待操作...";

            // ---- groupRunResult: 运行结果输出框 (行1右) ----
            groupRunResult.Controls.Add(labelValidationResult);
            groupRunResult.Dock = DockStyle.Fill;
            groupRunResult.Location = new Point(810, 327);
            groupRunResult.Name = "groupRunResult";
            groupRunResult.Padding = new Padding(8);
            groupRunResult.Size = new Size(801, 318);
            groupRunResult.TabIndex = 1;
            groupRunResult.TabStop = false;
            groupRunResult.Text = "运行结果";

            labelValidationResult.BackColor = Color.White;
            labelValidationResult.BorderStyle = BorderStyle.None;
            labelValidationResult.Dock = DockStyle.Fill;
            labelValidationResult.Font = new Font("Segoe UI", 9F);
            labelValidationResult.ForeColor = Color.Gray;
            labelValidationResult.Location = new Point(8, 31);
            labelValidationResult.Name = "labelValidationResult";
            labelValidationResult.ReadOnly = true;
            labelValidationResult.ScrollBars = RichTextBoxScrollBars.Vertical;
            labelValidationResult.Size = new Size(785, 279);
            labelValidationResult.TabIndex = 0;
            labelValidationResult.Text = "验证结果: 未验证";

            // ==================================================
            // 视图切换栏 (panelViewBar) - 跨mainTableLayout两列
            //   4个Tab按钮: ECDH | 签名验签 | 加密解密 | 文件操作
            // ==================================================
            mainTableLayout.SetColumnSpan(panelViewBar, 2);
            panelViewBar.Controls.Add(btnViewEcdh);
            panelViewBar.Controls.Add(btnViewSign);
            panelViewBar.Controls.Add(btnViewEncrypt);
            panelViewBar.Controls.Add(btnViewFile);
            panelViewBar.Dock = DockStyle.Fill;
            panelViewBar.Location = new Point(11, 704);
            panelViewBar.Name = "panelViewBar";
            panelViewBar.Padding = new Padding(4);
            panelViewBar.Size = new Size(3265, 54);
            panelViewBar.TabIndex = 10;
            panelViewBar.WrapContents = false;

            btnViewEcdh.AutoSize = true;
            btnViewEcdh.FlatStyle = FlatStyle.Flat;
            btnViewEcdh.Location = new Point(6, 6);
            btnViewEcdh.Margin = new Padding(2, 2, 8, 2);
            btnViewEcdh.Name = "btnViewEcdh";
            btnViewEcdh.Padding = new Padding(12, 4, 12, 4);
            btnViewEcdh.Size = new Size(191, 44);
            btnViewEcdh.TabIndex = 0;
            btnViewEcdh.Text = "ECDH 加密解密";
            btnViewEcdh.UseVisualStyleBackColor = true;

            btnViewSign.AutoSize = true;
            btnViewSign.FlatStyle = FlatStyle.Flat;
            btnViewSign.Location = new Point(207, 6);
            btnViewSign.Margin = new Padding(2, 2, 8, 2);
            btnViewSign.Name = "btnViewSign";
            btnViewSign.Padding = new Padding(12, 4, 12, 4);
            btnViewSign.Size = new Size(126, 44);
            btnViewSign.TabIndex = 1;
            btnViewSign.Text = "ECIES签名验签";
            btnViewSign.UseVisualStyleBackColor = true;

            btnViewEncrypt.AutoSize = true;
            btnViewEncrypt.FlatStyle = FlatStyle.Flat;
            btnViewEncrypt.Location = new Point(343, 6);
            btnViewEncrypt.Margin = new Padding(2, 2, 8, 2);
            btnViewEncrypt.Name = "btnViewEncrypt";
            btnViewEncrypt.Padding = new Padding(12, 4, 12, 4);
            btnViewEncrypt.Size = new Size(136, 44);
            btnViewEncrypt.TabIndex = 2;
            btnViewEncrypt.Text = "ECIES加密解密";
            btnViewEncrypt.UseVisualStyleBackColor = true;

            btnViewFile.AutoSize = true;
            btnViewFile.FlatStyle = FlatStyle.Flat;
            btnViewFile.Location = new Point(489, 6);
            btnViewFile.Margin = new Padding(2, 2, 8, 2);
            btnViewFile.Name = "btnViewFile";
            btnViewFile.Padding = new Padding(12, 4, 12, 4);
            btnViewFile.Size = new Size(162, 44);
            btnViewFile.TabIndex = 3;
            btnViewFile.Text = "文件签名/验签";
            btnViewFile.UseVisualStyleBackColor = true;

            // ==================================================
            // 视图内容面板 (panelViewContent) - 包含4个可切换面板
            //   groupSign(签名验签) | groupEncrypt(加密解密) | groupFile(文件操作) | groupEcdh(密钥协商)
            // ==================================================
            mainTableLayout.SetColumnSpan(panelViewContent, 2);
            panelViewContent.Controls.Add(groupSign);
            panelViewContent.Controls.Add(groupEncrypt);
            panelViewContent.Controls.Add(groupFile);
            panelViewContent.Controls.Add(groupEcdh);
            panelViewContent.Dock = DockStyle.Fill;
            panelViewContent.Location = new Point(11, 764);
            panelViewContent.Name = "panelViewContent";
            panelViewContent.Size = new Size(3265, 841);
            panelViewContent.TabIndex = 11;

            // ==================================================
            // 签名验签面板 (groupSign) - ECIES签名/验签功能
            //   tableLayoutSign(2列): groupSignInput(左) | groupSignActions(右)
            // ==================================================
            groupSign.Controls.Add(tableLayoutSign);
            groupSign.Dock = DockStyle.Fill;
            groupSign.Location = new Point(0, 0);
            groupSign.Name = "groupSign";
            groupSign.Padding = new Padding(4);
            groupSign.Size = new Size(3265, 841);
            groupSign.TabIndex = 0;

            // ---- tableLayoutSign: 2列 × 1行 ----
            tableLayoutSign.ColumnCount = 2;
            tableLayoutSign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutSign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutSign.Controls.Add(groupSignInput, 0, 0);
            tableLayoutSign.Controls.Add(groupSignActions, 1, 0);
            tableLayoutSign.Dock = DockStyle.Fill;
            tableLayoutSign.Location = new Point(4, 4);
            tableLayoutSign.Name = "tableLayoutSign";
            tableLayoutSign.RowCount = 1;
            tableLayoutSign.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutSign.Size = new Size(3257, 833);
            tableLayoutSign.TabIndex = 0;

            // ---- groupSignInput: 签名输入区 (左列) ----
            groupSignInput.Controls.Add(panelSignInput);
            groupSignInput.Dock = DockStyle.Fill;
            groupSignInput.Location = new Point(3, 3);
            groupSignInput.Name = "groupSignInput";
            groupSignInput.Padding = new Padding(8);
            groupSignInput.Size = new Size(1622, 827);
            groupSignInput.TabIndex = 0;
            groupSignInput.TabStop = false;
            groupSignInput.Text = "签名验签";

            // ---- panelSignInput: 2列(PEM框|按钮) × 4行(原文/间隔/签名/间隔) ----
            panelSignInput.ColumnCount = 2;
            panelSignInput.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelSignInput.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 200F));
            panelSignInput.Controls.Add(panelPlainDataBox, 0, 0);
            panelSignInput.Controls.Add(panelPlainDataActions, 1, 0);
            panelSignInput.Controls.Add(panelSignatureBox, 0, 2);
            panelSignInput.Controls.Add(panelSignatureActions, 1, 2);
            panelSignInput.Dock = DockStyle.Fill;
            panelSignInput.Location = new Point(8, 31);
            panelSignInput.Name = "panelSignInput";
            panelSignInput.Padding = new Padding(6);
            panelSignInput.RowCount = 4;
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Absolute, 0F));
            panelSignInput.Size = new Size(1606, 788);
            panelSignInput.TabIndex = 0;

            // ---- 原始数据区 (列0行0, 跨2行) ----
            panelPlainDataBox.Controls.Add(labelPlainData);
            panelPlainDataBox.Controls.Add(textPlainData);
            panelPlainDataBox.Dock = DockStyle.Fill;
            panelPlainDataBox.Location = new Point(9, 9);
            panelPlainDataBox.Name = "panelPlainDataBox";
            panelSignInput.SetRowSpan(panelPlainDataBox, 2);
            panelPlainDataBox.Size = new Size(1388, 382);
            panelPlainDataBox.TabIndex = 0;

            labelPlainData.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelPlainData.AutoSize = true;
            labelPlainData.BackColor = Color.Transparent;
            labelPlainData.Location = new Point(1276, 4);
            labelPlainData.Margin = new Padding(4, 4, 4, 2);
            labelPlainData.Name = "labelPlainData";
            labelPlainData.Padding = new Padding(4, 0, 4, 0);
            labelPlainData.Size = new Size(108, 24);
            labelPlainData.TabIndex = 0;
            labelPlainData.Text = "原始数据：";

            textPlainData.Dock = DockStyle.Fill;
            textPlainData.Location = new Point(0, 0);
            textPlainData.Multiline = true;
            textPlainData.Name = "textPlainData";
            textPlainData.ScrollBars = ScrollBars.Vertical;
            textPlainData.Size = new Size(1388, 382);
            textPlainData.TabIndex = 2;

            // ---- 原始数据操作按钮 (列1行0, 跨2行): 复制|粘贴|清空 ----
            panelPlainDataActions.ColumnCount = 1;
            panelPlainDataActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelPlainDataActions.Controls.Add(labelPlainDataActionsTitle, 0, 0);
            panelPlainDataActions.Controls.Add(btnCopyPlainData, 0, 1);
            panelPlainDataActions.Controls.Add(btnPastePlainData, 0, 2);
            panelPlainDataActions.Controls.Add(btnClearPlainData, 0, 3);
            panelPlainDataActions.Dock = DockStyle.Fill;
            panelPlainDataActions.Location = new Point(1403, 9);
            panelPlainDataActions.Name = "panelPlainDataActions";
            panelPlainDataActions.Padding = new Padding(8, 4, 8, 4);
            panelPlainDataActions.RowCount = 4;
            panelSignInput.SetRowSpan(panelPlainDataActions, 2);
            panelPlainDataActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            panelPlainDataActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            panelPlainDataActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            panelPlainDataActions.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
            panelPlainDataActions.Size = new Size(194, 382);
            panelPlainDataActions.TabIndex = 4;

            labelPlainDataActionsTitle.AutoSize = true;
            labelPlainDataActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPlainDataActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelPlainDataActionsTitle.Location = new Point(8, 4);
            labelPlainDataActionsTitle.Margin = new Padding(0);
            labelPlainDataActionsTitle.Name = "labelPlainDataActionsTitle";
            labelPlainDataActionsTitle.Size = new Size(92, 25);
            labelPlainDataActionsTitle.TabIndex = 0;
            labelPlainDataActionsTitle.Text = "数据操作";

            btnCopyPlainData.Dock = DockStyle.Fill;
            btnCopyPlainData.Location = new Point(10, 32);
            btnCopyPlainData.Margin = new Padding(2);
            btnCopyPlainData.MinimumSize = new Size(120, 30);
            btnCopyPlainData.Name = "btnCopyPlainData";
            btnCopyPlainData.Size = new Size(174, 110);
            btnCopyPlainData.TabIndex = 1;
            btnCopyPlainData.Text = "复制数据";
            btnCopyPlainData.Click += BtnCopyPlainData_Click;

            btnPastePlainData.Dock = DockStyle.Fill;
            btnPastePlainData.Location = new Point(10, 146);
            btnPastePlainData.Margin = new Padding(2);
            btnPastePlainData.MinimumSize = new Size(120, 30);
            btnPastePlainData.Name = "btnPastePlainData";
            btnPastePlainData.Size = new Size(174, 110);
            btnPastePlainData.TabIndex = 2;
            btnPastePlainData.Text = "粘贴数据";
            btnPastePlainData.Click += BtnPastePlainData_Click;

            btnClearPlainData.Dock = DockStyle.Fill;
            btnClearPlainData.Location = new Point(10, 260);
            btnClearPlainData.Margin = new Padding(2);
            btnClearPlainData.MinimumSize = new Size(120, 30);
            btnClearPlainData.Name = "btnClearPlainData";
            btnClearPlainData.Size = new Size(174, 116);
            btnClearPlainData.TabIndex = 3;
            btnClearPlainData.Text = "清空数据";
            btnClearPlainData.Click += BtnClearPlainData_Click;

            // ---- 签名数据区 (列0行2, 跨2行) ----
            panelSignatureBox.Controls.Add(labelSignature);
            panelSignatureBox.Controls.Add(textSignature);
            panelSignatureBox.Dock = DockStyle.Fill;
            panelSignatureBox.Location = new Point(9, 397);
            panelSignatureBox.Name = "panelSignatureBox";
            panelSignInput.SetRowSpan(panelSignatureBox, 2);
            panelSignatureBox.Size = new Size(1388, 382);
            panelSignatureBox.TabIndex = 1;

            labelSignature.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            labelSignature.AutoSize = true;
            labelSignature.BackColor = Color.Transparent;
            labelSignature.Location = new Point(1312, 4);
            labelSignature.Margin = new Padding(4, 4, 4, 2);
            labelSignature.Name = "labelSignature";
            labelSignature.Padding = new Padding(4, 0, 4, 0);
            labelSignature.Size = new Size(72, 24);
            labelSignature.TabIndex = 3;
            labelSignature.Text = "签名：";

            textSignature.Dock = DockStyle.Fill;
            textSignature.Location = new Point(0, 0);
            textSignature.Multiline = true;
            textSignature.Name = "textSignature";
            textSignature.ScrollBars = ScrollBars.Vertical;
            textSignature.Size = new Size(1388, 382);
            textSignature.TabIndex = 4;

            // ---- 签名操作按钮 (列1行2, 跨2行): 复制|粘贴|清空 ----
            panelSignatureActions.ColumnCount = 1;
            panelSignatureActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelSignatureActions.Controls.Add(labelSignatureActionsTitle, 0, 0);
            panelSignatureActions.Controls.Add(btnCopySignatureData, 0, 1);
            panelSignatureActions.Controls.Add(btnPasteSignatureData, 0, 2);
            panelSignatureActions.Controls.Add(btnClearSignatureData, 0, 3);
            panelSignatureActions.Dock = DockStyle.Fill;
            panelSignatureActions.Location = new Point(1403, 397);
            panelSignatureActions.Name = "panelSignatureActions";
            panelSignatureActions.Padding = new Padding(8, 4, 8, 4);
            panelSignatureActions.RowCount = 4;
            panelSignInput.SetRowSpan(panelSignatureActions, 2);
            panelSignatureActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 26F));
            panelSignatureActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            panelSignatureActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33F));
            panelSignatureActions.RowStyles.Add(new RowStyle(SizeType.Percent, 34F));
            panelSignatureActions.Size = new Size(194, 382);
            panelSignatureActions.TabIndex = 5;

            labelSignatureActionsTitle.AutoSize = true;
            labelSignatureActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelSignatureActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelSignatureActionsTitle.Location = new Point(8, 4);
            labelSignatureActionsTitle.Margin = new Padding(0);
            labelSignatureActionsTitle.Name = "labelSignatureActionsTitle";
            labelSignatureActionsTitle.Size = new Size(92, 25);
            labelSignatureActionsTitle.TabIndex = 0;
            labelSignatureActionsTitle.Text = "签名操作";

            btnCopySignatureData.Dock = DockStyle.Fill;
            btnCopySignatureData.Location = new Point(10, 32);
            btnCopySignatureData.Margin = new Padding(2);
            btnCopySignatureData.MinimumSize = new Size(120, 30);
            btnCopySignatureData.Name = "btnCopySignatureData";
            btnCopySignatureData.Size = new Size(174, 110);
            btnCopySignatureData.TabIndex = 1;
            btnCopySignatureData.Text = "复制签名";
            btnCopySignatureData.Click += BtnCopySignatureData_Click;

            btnPasteSignatureData.Dock = DockStyle.Fill;
            btnPasteSignatureData.Location = new Point(10, 146);
            btnPasteSignatureData.Margin = new Padding(2);
            btnPasteSignatureData.MinimumSize = new Size(120, 30);
            btnPasteSignatureData.Name = "btnPasteSignatureData";
            btnPasteSignatureData.Size = new Size(174, 110);
            btnPasteSignatureData.TabIndex = 2;
            btnPasteSignatureData.Text = "粘贴签名";
            btnPasteSignatureData.Click += BtnPasteSignatureData_Click;

            btnClearSignatureData.Dock = DockStyle.Fill;
            btnClearSignatureData.Location = new Point(10, 260);
            btnClearSignatureData.Margin = new Padding(2);
            btnClearSignatureData.MinimumSize = new Size(120, 30);
            btnClearSignatureData.Name = "btnClearSignatureData";
            btnClearSignatureData.Size = new Size(174, 116);
            btnClearSignatureData.TabIndex = 3;
            btnClearSignatureData.Text = "清空签名";
            btnClearSignatureData.Click += BtnClearSignatureData_Click;

            // ---- groupSignActions: 签名操作区 (右列) ----
            groupSignActions.Controls.Add(panelSignActions);
            groupSignActions.Dock = DockStyle.Fill;
            groupSignActions.Location = new Point(1631, 3);
            groupSignActions.Name = "groupSignActions";
            groupSignActions.Padding = new Padding(8);
            groupSignActions.Size = new Size(1623, 827);
            groupSignActions.TabIndex = 1;
            groupSignActions.TabStop = false;
            groupSignActions.Text = "操作按钮";

            // ---- panelSignActions: 2列(按钮|选项) × 3行 ----
            panelSignActions.ColumnCount = 2;
            panelSignActions.ColumnStyles.Add(new ColumnStyle());
            panelSignActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelSignActions.Controls.Add(btnSign, 0, 0);
            panelSignActions.Controls.Add(btnVerify, 0, 1);
            panelSignActions.Controls.Add(btnCopySignature, 0, 2);
            panelSignActions.Controls.Add(panelSignOptions, 1, 0);
            panelSignActions.Dock = DockStyle.Fill;
            panelSignActions.Location = new Point(8, 31);
            panelSignActions.Name = "panelSignActions";
            panelSignActions.Padding = new Padding(8, 4, 8, 4);
            panelSignActions.RowCount = 3;
            panelSignActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
            panelSignActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3333F));
            panelSignActions.RowStyles.Add(new RowStyle(SizeType.Percent, 33.3334F));
            panelSignActions.Size = new Size(1607, 788);
            panelSignActions.TabIndex = 1;

            // 签名按钮
            btnSign.AutoSize = true;
            btnSign.Dock = DockStyle.Top;
            btnSign.Location = new Point(8, 4);
            btnSign.Margin = new Padding(0, 0, 0, 4);
            btnSign.MinimumSize = new Size(120, 30);
            btnSign.Name = "btnSign";
            btnSign.Size = new Size(120, 40);
            btnSign.TabIndex = 4;
            btnSign.Text = "签名";
            btnSign.Click += BtnSign_Click;

            // 验签按钮
            btnVerify.AutoSize = true;
            btnVerify.Dock = DockStyle.Top;
            btnVerify.Location = new Point(8, 263);
            btnVerify.Margin = new Padding(0, 0, 0, 4);
            btnVerify.MinimumSize = new Size(120, 30);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new Size(120, 40);
            btnVerify.TabIndex = 5;
            btnVerify.Text = "验签";
            btnVerify.Click += BtnVerify_Click;

            // 复制签名按钮
            btnCopySignature.AutoSize = true;
            btnCopySignature.Dock = DockStyle.Top;
            btnCopySignature.Location = new Point(8, 522);
            btnCopySignature.Margin = new Padding(0);
            btnCopySignature.MinimumSize = new Size(120, 30);
            btnCopySignature.Name = "btnCopySignature";
            btnCopySignature.Size = new Size(120, 40);
            btnCopySignature.TabIndex = 6;
            btnCopySignature.Text = "复制签名";
            btnCopySignature.Click += BtnCopySignature_Click;

            // ---- panelSignOptions: 签名选项 (哈希算法|签名格式) ----
            panelSignOptions.ColumnCount = 2;
            panelSignOptions.ColumnStyles.Add(new ColumnStyle());
            panelSignOptions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            panelSignOptions.Controls.Add(labelHashAlgorithm, 0, 0);
            panelSignOptions.Controls.Add(comboHashAlgorithm, 1, 0);
            panelSignOptions.Controls.Add(labelSignatureFormat, 0, 1);
            panelSignOptions.Controls.Add(comboSignatureFormat, 1, 1);
            panelSignOptions.Dock = DockStyle.Fill;
            panelSignOptions.Location = new Point(131, 7);
            panelSignOptions.Name = "panelSignOptions";
            panelSignOptions.RowCount = 2;
            panelSignActions.SetRowSpan(panelSignOptions, 3);
            panelSignOptions.RowStyles.Add(new RowStyle());
            panelSignOptions.RowStyles.Add(new RowStyle());
            panelSignOptions.Size = new Size(1465, 774);
            panelSignOptions.TabIndex = 7;

            labelHashAlgorithm.AutoSize = true;
            labelHashAlgorithm.Location = new Point(0, 4);
            labelHashAlgorithm.Margin = new Padding(0, 4, 4, 0);
            labelHashAlgorithm.Name = "labelHashAlgorithm";
            labelHashAlgorithm.Size = new Size(107, 24);
            labelHashAlgorithm.TabIndex = 0;
            labelHashAlgorithm.Text = "Hash算法：";
            labelHashAlgorithm.TextAlign = ContentAlignment.MiddleLeft;

            comboHashAlgorithm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboHashAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            comboHashAlgorithm.FormattingEnabled = true;
            comboHashAlgorithm.Items.AddRange(new object[] { "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512" });
            comboHashAlgorithm.Location = new Point(111, 0);
            comboHashAlgorithm.Margin = new Padding(0, 0, 16, 0);
            comboHashAlgorithm.Name = "comboHashAlgorithm";
            comboHashAlgorithm.Size = new Size(1338, 32);
            comboHashAlgorithm.TabIndex = 1;

            labelSignatureFormat.AutoSize = true;
            labelSignatureFormat.Location = new Point(0, 36);
            labelSignatureFormat.Margin = new Padding(0, 4, 4, 0);
            labelSignatureFormat.Name = "labelSignatureFormat";
            labelSignatureFormat.Size = new Size(100, 24);
            labelSignatureFormat.TabIndex = 2;
            labelSignatureFormat.Text = "签名格式：";
            labelSignatureFormat.TextAlign = ContentAlignment.MiddleLeft;

            comboSignatureFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboSignatureFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSignatureFormat.FormattingEnabled = true;
            comboSignatureFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboSignatureFormat.Location = new Point(111, 32);
            comboSignatureFormat.Margin = new Padding(0);
            comboSignatureFormat.Name = "comboSignatureFormat";
            comboSignatureFormat.Size = new Size(1354, 32);
            comboSignatureFormat.TabIndex = 3;

            // ==================================================
            // 加密/解密面板 (groupEncrypt) - ECIES + AES + ChaCha20
            //   tableLayoutEncrypt: 单列多行, 从上到下依次为模式/格式/密钥/IV/公钥/曲线/输入/输出/按钮
            // ==================================================
            groupEncrypt.BorderStyle = BorderStyle.None;
            groupEncrypt.Controls.Add(tableLayoutEncrypt);
            groupEncrypt.Dock = DockStyle.Fill;
            groupEncrypt.Location = new Point(0, 0);
            groupEncrypt.Name = "groupEncrypt";
            groupEncrypt.Padding = new Padding(8);
            groupEncrypt.Size = new Size(3265, 841);
            groupEncrypt.TabIndex = 0;

            // ---- tableLayoutEncrypt: 单列 × 多行 ----
            tableLayoutEncrypt.ColumnCount = 1;
            tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutEncrypt.Dock = DockStyle.Fill;
            tableLayoutEncrypt.Location = new Point(8, 31);
            tableLayoutEncrypt.Name = "tableLayoutEncrypt";
            tableLayoutEncrypt.RowCount = 9;
            // 行高: 模式36 | 输入格式36 | 输出格式36 | 密钥36 | IV22 | 曲线35% | 输出标签22 | 输出框35% | 按钮44
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutEncrypt.Size = new Size(3249, 802);
            tableLayoutEncrypt.TabIndex = 0;

            // ---- 第0行: 加密模式 ----
            labelEncMode.AutoSize = true;
            labelEncMode.Location = new Point(4, 4);
            labelEncMode.Margin = new Padding(4, 4, 2, 4);
            labelEncMode.Name = "labelEncMode";
            labelEncMode.Size = new Size(100, 24);
            labelEncMode.TabIndex = 1;
            labelEncMode.Text = "加密模式：";

            comboEncMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncMode.FormattingEnabled = true;
            comboEncMode.Items.AddRange(new object[] { "ECIES (ECDH+AES-GCM)", "AES-256-GCM", "AES-256-CBC", "ChaCha20-Poly1305" });
            comboEncMode.Location = new Point(0, 3);
            comboEncMode.Margin = new Padding(0, 3, 4, 3);
            comboEncMode.Name = "comboEncMode";
            comboEncMode.Size = new Size(364, 32);
            comboEncMode.TabIndex = 2;

            // ---- 第1行: 输入格式 ----
            labelEncInputFormat.AutoSize = true;
            labelEncInputFormat.Location = new Point(4, 40);
            labelEncInputFormat.Margin = new Padding(4, 4, 2, 4);
            labelEncInputFormat.Name = "labelEncInputFormat";
            labelEncInputFormat.Size = new Size(100, 14);
            labelEncInputFormat.TabIndex = 3;
            labelEncInputFormat.Text = "输入格式：";

            comboEncInputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncInputFormat.FormattingEnabled = true;
            comboEncInputFormat.Items.AddRange(new object[] { "UTF-8文本", "Base64", "Hex" });
            comboEncInputFormat.Location = new Point(0, 39);
            comboEncInputFormat.Margin = new Padding(0, 3, 4, 3);
            comboEncInputFormat.Name = "comboEncInputFormat";
            comboEncInputFormat.Size = new Size(120, 32);
            comboEncInputFormat.TabIndex = 4;

            // ---- 第1行: 输出格式 (与输入格式同行) ----
            labelEncOutputFormat.AutoSize = true;
            labelEncOutputFormat.Location = new Point(140, 40);
            labelEncOutputFormat.Margin = new Padding(8, 4, 2, 4);
            labelEncOutputFormat.Name = "labelEncOutputFormat";
            labelEncOutputFormat.Size = new Size(100, 24);
            labelEncOutputFormat.TabIndex = 5;
            labelEncOutputFormat.Text = "输出格式：";

            // ★ 输出格式下拉框
            comboEncOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncOutputFormat.FormattingEnabled = true;
            comboEncOutputFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboEncOutputFormat.Location = new Point(244, 39);
            comboEncOutputFormat.Margin = new Padding(0, 3, 4, 3);
            comboEncOutputFormat.Name = "comboEncOutputFormat";
            comboEncOutputFormat.Size = new Size(120, 32);
            comboEncOutputFormat.TabIndex = 6;

            // ---- 第2行: 对称密钥 (HEX, 留空自动从ECDSA私钥HKDF派生) ----
            labelEncKey.AutoSize = true;
            labelEncKey.Location = new Point(4, 76);
            labelEncKey.Margin = new Padding(4, 4, 2, 4);
            labelEncKey.Name = "labelEncKey";
            labelEncKey.Size = new Size(279, 24);
            labelEncKey.TabIndex = 7;
            labelEncKey.Text = "对称密钥 (HEX，留空自动派生)：";

            textEncKey.Dock = DockStyle.Fill;
            textEncKey.Font = new Font("Consolas", 9F);
            textEncKey.Location = new Point(4, 75);
            textEncKey.Margin = new Padding(4, 3, 4, 3);
            textEncKey.Name = "textEncKey";
            textEncKey.PlaceholderText = "从ECDSA私钥自动派生（HKDF-SHA256）";
            textEncKey.Size = new Size(3241, 29);
            textEncKey.TabIndex = 8;

            // ---- 第3行: IV/Nonce (HEX, 留空随机生成) ----
            labelEncIV.AutoSize = true;
            labelEncIV.Location = new Point(4, 112);
            labelEncIV.Margin = new Padding(4, 4, 2, 4);
            labelEncIV.Name = "labelEncIV";
            labelEncIV.Size = new Size(288, 12);
            labelEncIV.TabIndex = 9;
            labelEncIV.Text = "IV/Nonce (HEX，留空随机生成)：";

            textEncIV.Dock = DockStyle.Fill;
            textEncIV.Font = new Font("Consolas", 9F);
            textEncIV.Location = new Point(4, 111);
            textEncIV.Margin = new Padding(4, 3, 4, 3);
            textEncIV.Name = "textEncIV";
            textEncIV.PlaceholderText = "加密时自动生成，解密时需填写";
            textEncIV.Size = new Size(3241, 29);
            textEncIV.TabIndex = 10;

            // ---- 第3行: Bob 公钥 (接收方, ECIES加密时使用) ----
            labelEncBobPublic.AutoSize = true;
            labelEncBobPublic.Location = new Point(4, 148);
            labelEncBobPublic.Margin = new Padding(4, 4, 2, 4);
            labelEncBobPublic.Name = "labelEncBobPublic";
            labelEncBobPublic.Size = new Size(288, 24);
            labelEncBobPublic.TabIndex = 15;
            labelEncBobPublic.Text = "Bob 公钥 (接收方)：";

            textEncBobPublic.Dock = DockStyle.Fill;
            textEncBobPublic.Font = new Font("Consolas", 9F);
            textEncBobPublic.Location = new Point(4, 147);
            textEncBobPublic.Margin = new Padding(4, 3, 4, 3);
            textEncBobPublic.Name = "textEncBobPublic";
            textEncBobPublic.PlaceholderText = "粘贴 PEM 格式公钥（支持多行）";
            textEncBobPublic.Multiline = true;
            textEncBobPublic.ScrollBars = ScrollBars.Vertical;
            textEncBobPublic.Size = new Size(3241, 29);
            textEncBobPublic.TabIndex = 16;

            // ---- 第4行: 椭圆曲线选择 (类别 → 曲线) ----
            labelEncCurve.AutoSize = true;
            labelEncCurve.Location = new Point(4, 183);
            labelEncCurve.Margin = new Padding(4, 4, 2, 4);
            labelEncCurve.Name = "labelEncCurve";
            labelEncCurve.Size = new Size(100, 24);
            labelEncCurve.TabIndex = 17;
            labelEncCurve.Text = "曲线：";

            comboEncCurveCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncCurveCategory.FormattingEnabled = true;
            comboEncCurveCategory.Location = new Point(0, 182);
            comboEncCurveCategory.Margin = new Padding(0, 3, 2, 3);
            comboEncCurveCategory.Name = "comboEncCurveCategory";
            comboEncCurveCategory.Size = new Size(160, 32);
            comboEncCurveCategory.TabIndex = 18;

            labelEncCurveArrow.AutoSize = true;
            labelEncCurveArrow.Location = new Point(166, 186);
            labelEncCurveArrow.Margin = new Padding(2, 7, 2, 0);
            labelEncCurveArrow.Name = "labelEncCurveArrow";
            labelEncCurveArrow.Size = new Size(22, 24);
            labelEncCurveArrow.TabIndex = 19;
            labelEncCurveArrow.Text = "→";

            comboEncCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncCurve.FormattingEnabled = true;
            comboEncCurve.Location = new Point(192, 182);
            comboEncCurve.Margin = new Padding(0, 3, 4, 3);
            comboEncCurve.Name = "comboEncCurve";
            comboEncCurve.Size = new Size(260, 32);
            comboEncCurve.TabIndex = 20;

            // ---- 第5行: 明文/密文输入标签 ----
            labelEncInput.AutoSize = true;
            labelEncInput.Location = new Point(4, 220);
            labelEncInput.Margin = new Padding(4, 4, 2, 4);
            labelEncInput.Name = "labelEncInput";
            labelEncInput.Size = new Size(154, 12);
            labelEncInput.TabIndex = 11;
            labelEncInput.Text = "明文 / 密文输入：";

            // ---- 第6行: 明文/密文输入框 (占35%高度) ----
            textEncInput.Dock = DockStyle.Fill;
            textEncInput.Font = new Font("Consolas", 9F);
            textEncInput.Location = new Point(4, 235);
            textEncInput.Margin = new Padding(4, 3, 4, 3);
            textEncInput.Multiline = true;
            textEncInput.Name = "textEncInput";
            textEncInput.ScrollBars = ScrollBars.Vertical;
            textEncInput.Size = new Size(3241, 14);
            textEncInput.TabIndex = 12;

            // ---- 第7行: 加密结果/解密输入标签 ----
            labelEncOutputLabel.AutoSize = true;
            labelEncOutputLabel.Location = new Point(4, 256);
            labelEncOutputLabel.Margin = new Padding(4, 4, 2, 4);
            labelEncOutputLabel.Name = "labelEncOutputLabel";
            labelEncOutputLabel.Size = new Size(200, 12);
            labelEncOutputLabel.TabIndex = 13;
            labelEncOutputLabel.Text = "加密结果 / 解密输入：";

            // ---- 第8行: 加密结果输出框 (占35%高度) ----
            textEncOutput.Dock = DockStyle.Fill;
            textEncOutput.Font = new Font("Consolas", 9F);
            textEncOutput.Location = new Point(4, 271);
            textEncOutput.Margin = new Padding(4, 3, 4, 3);
            textEncOutput.Multiline = true;
            textEncOutput.Name = "textEncOutput";
            textEncOutput.ScrollBars = ScrollBars.Vertical;
            textEncOutput.Size = new Size(3241, 14);
            textEncOutput.TabIndex = 14;

            // ---- 第9行: 加密操作按钮面板 ----
            panelEncBtns.Controls.Add(btnEncrypt);
            panelEncBtns.Controls.Add(btnDecrypt);
            panelEncBtns.Controls.Add(btnEncClear);
            panelEncBtns.Controls.Add(btnEncCopy);
            panelEncBtns.Controls.Add(btnEncPaste);
            panelEncBtns.Dock = DockStyle.Fill;
            panelEncBtns.Location = new Point(3, 290);
            panelEncBtns.Name = "panelEncBtns";
            panelEncBtns.Padding = new Padding(0, 4, 0, 0);
            panelEncBtns.Size = new Size(3243, 14);
            panelEncBtns.TabIndex = 15;
            panelEncBtns.WrapContents = false;

            btnEncrypt.AutoSize = true;
            btnEncrypt.Location = new Point(3, 7);
            btnEncrypt.Margin = new Padding(3, 3, 4, 3);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new Size(75, 34);
            btnEncrypt.TabIndex = 0;
            btnEncrypt.Text = "加密";
            btnEncrypt.Click += BtnEncrypt_Click;

            btnDecrypt.AutoSize = true;
            btnDecrypt.Location = new Point(88, 7);
            btnDecrypt.Margin = new Padding(6, 3, 6, 3);
            btnDecrypt.Name = "btnDecrypt";
            btnDecrypt.Size = new Size(75, 34);
            btnDecrypt.TabIndex = 1;
            btnDecrypt.Text = "解密";
            btnDecrypt.Click += BtnDecrypt_Click;

            btnEncClear.AutoSize = true;
            btnEncClear.Location = new Point(175, 7);
            btnEncClear.Margin = new Padding(6, 3, 6, 3);
            btnEncClear.Name = "btnEncClear";
            btnEncClear.Size = new Size(75, 34);
            btnEncClear.TabIndex = 2;
            btnEncClear.Text = "清空";
            btnEncClear.Click += BtnEncClear_Click;

            btnEncCopy.AutoSize = true;
            btnEncCopy.Location = new Point(262, 7);
            btnEncCopy.Margin = new Padding(6, 3, 6, 3);
            btnEncCopy.Name = "btnEncCopy";
            btnEncCopy.Size = new Size(92, 34);
            btnEncCopy.TabIndex = 3;
            btnEncCopy.Text = "复制结果";
            btnEncCopy.Click += BtnEncCopy_Click;

            btnEncPaste.AutoSize = true;
            btnEncPaste.Location = new Point(366, 7);
            btnEncPaste.Margin = new Padding(6, 3, 4, 3);
            btnEncPaste.Name = "btnEncPaste";
            btnEncPaste.Size = new Size(92, 34);
            btnEncPaste.TabIndex = 4;
            btnEncPaste.Text = "粘贴输入";
            btnEncPaste.Click += BtnEncPaste_Click;

            // ==================================================
            // 文件签名/验签面板 (groupFile)
            // ==================================================
            groupFile.Controls.Add(panelFileControls);
            groupFile.Dock = DockStyle.Fill;
            groupFile.Location = new Point(0, 0);
            groupFile.Name = "groupFile";
            groupFile.Padding = new Padding(8);
            groupFile.Size = new Size(3265, 841);
            groupFile.TabIndex = 0;
            groupFile.TabStop = false;

            panelFileControls.Controls.Add(btnSignFile);
            panelFileControls.Controls.Add(btnVerifyFile);
            panelFileControls.Dock = DockStyle.Fill;
            panelFileControls.Location = new Point(8, 31);
            panelFileControls.Name = "panelFileControls";
            panelFileControls.Padding = new Padding(4, 2, 4, 2);
            panelFileControls.Size = new Size(3249, 802);
            panelFileControls.TabIndex = 0;
            panelFileControls.WrapContents = false;

            btnSignFile.AutoSize = true;
            btnSignFile.Location = new Point(8, 5);
            btnSignFile.Margin = new Padding(4, 3, 4, 3);
            btnSignFile.Name = "btnSignFile";
            btnSignFile.Size = new Size(92, 34);
            btnSignFile.TabIndex = 0;
            btnSignFile.Text = "签名文件";
            btnSignFile.Click += BtnSignFile_Click;

            btnVerifyFile.AutoSize = true;
            btnVerifyFile.Location = new Point(110, 5);
            btnVerifyFile.Margin = new Padding(6, 3, 4, 3);
            btnVerifyFile.Name = "btnVerifyFile";
            btnVerifyFile.Size = new Size(92, 34);
            btnVerifyFile.TabIndex = 1;
            btnVerifyFile.Text = "验签文件";
            btnVerifyFile.Click += BtnVerifyFile_Click;

            // ==================================================
            // ECDH 密钥协商面板 (groupEcdh)
            // ==================================================
            groupEcdh.Dock = DockStyle.Fill;
            groupEcdh.Location = new Point(0, 0);
            groupEcdh.Name = "groupEcdh";
            groupEcdh.Padding = new Padding(8);
            groupEcdh.Size = new Size(3265, 841);
            groupEcdh.TabIndex = 0;

            // ECDH 模式选择
            labelEcdhMode.AutoSize = true;
            labelEcdhMode.Location = new Point(8, 8);
            labelEcdhMode.Name = "labelEcdhMode";
            labelEcdhMode.Text = "ECDH 模式：";
            groupEcdh.Controls.Add(labelEcdhMode);
            groupEcdh.Controls.Add(comboEcdhMode);

            comboEcdhMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEcdhMode.FormattingEnabled = true;
            comboEcdhMode.Items.AddRange(new object[] { "CryptoTool", "8gwifi.org", "ANSI X9.63", "IEEE 1363a", "ISO/IEC 18033-2", "SECG SEC 1" });
            comboEcdhMode.Location = new Point(100, 5);
            comboEcdhMode.Size = new Size(200, 32);
            comboEcdhMode.TabIndex = 0;
            comboEcdhMode.SelectedIndexChanged += ComboEcdhMode_SelectedIndexChanged;

            // ECDH 曲线类别 + 曲线
            labelEcdhCategory.AutoSize = true;
            labelEcdhCategory.Location = new Point(320, 8);
            labelEcdhCategory.Name = "labelEcdhCategory";
            labelEcdhCategory.Text = "曲线：";
            groupEcdh.Controls.Add(labelEcdhCategory);
            groupEcdh.Controls.Add(comboEcdhCategory);

            comboEcdhCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEcdhCategory.FormattingEnabled = true;
            comboEcdhCategory.Location = new Point(365, 5);
            comboEcdhCategory.Size = new Size(160, 32);
            comboEcdhCategory.TabIndex = 1;
            comboEcdhCategory.SelectedIndexChanged += ComboEcdhCategory_SelectedIndexChanged;

            labelEcdhCurve.AutoSize = true;
            labelEcdhCurve.Location = new Point(532, 8);
            labelEcdhCurve.Name = "labelEcdhCurve";
            labelEcdhCurve.Text = "→";
            groupEcdh.Controls.Add(labelEcdhCurve);
            groupEcdh.Controls.Add(comboEcdhCurve);

            comboEcdhCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEcdhCurve.FormattingEnabled = true;
            comboEcdhCurve.Location = new Point(555, 5);
            comboEcdhCurve.Size = new Size(260, 32);
            comboEcdhCurve.TabIndex = 2;

            // ECDH 编码格式
            labelEcdhEncoding.AutoSize = true;
            labelEcdhEncoding.Location = new Point(840, 8);
            labelEcdhEncoding.Name = "labelEcdhEncoding";
            labelEcdhEncoding.Text = "编码：";
            groupEcdh.Controls.Add(labelEcdhEncoding);
            groupEcdh.Controls.Add(comboEcdhEncoding);

            comboEcdhEncoding.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEcdhEncoding.FormattingEnabled = true;
            comboEcdhEncoding.Items.AddRange(new object[] { "UTF-8", "GBK (GB2312)", "Unicode (UTF-16 LE)" });
            comboEcdhEncoding.Location = new Point(885, 5);
            comboEcdhEncoding.Size = new Size(140, 32);
            comboEcdhEncoding.TabIndex = 3;

            // 生成密钥对按钮
            btnGenerateEcdhKeys.AutoSize = true;
            btnGenerateEcdhKeys.Location = new Point(1050, 5);
            btnGenerateEcdhKeys.Size = new Size(120, 34);
            btnGenerateEcdhKeys.TabIndex = 4;
            btnGenerateEcdhKeys.Text = "生成密钥对";
            btnGenerateEcdhKeys.Click += BtnGenerateEcdhKeys_Click;
            groupEcdh.Controls.Add(btnGenerateEcdhKeys);

            // Alice 私钥
            labelEcdhAlicePriv.AutoSize = true;
            labelEcdhAlicePriv.Location = new Point(8, 50);
            labelEcdhAlicePriv.Name = "labelEcdhAlicePriv";
            labelEcdhAlicePriv.Text = "Alice 私钥：";
            groupEcdh.Controls.Add(labelEcdhAlicePriv);

            textEcdhAlicePrivate.Dock = DockStyle.None;
            textEcdhAlicePrivate.Font = new Font("Consolas", 9F);
            textEcdhAlicePrivate.Location = new Point(100, 47);
            textEcdhAlicePrivate.Size = new Size(400, 60);
            textEcdhAlicePrivate.Multiline = true;
            textEcdhAlicePrivate.ScrollBars = ScrollBars.Vertical;
            textEcdhAlicePrivate.Name = "textEcdhAlicePrivate";
            textEcdhAlicePrivate.TabIndex = 5;
            groupEcdh.Controls.Add(textEcdhAlicePrivate);

            // Alice 公钥
            labelEcdhAlicePub.AutoSize = true;
            labelEcdhAlicePub.Location = new Point(520, 50);
            labelEcdhAlicePub.Name = "labelEcdhAlicePub";
            labelEcdhAlicePub.Text = "Alice 公钥：";
            groupEcdh.Controls.Add(labelEcdhAlicePub);

            textEcdhAlicePublic.Dock = DockStyle.None;
            textEcdhAlicePublic.Font = new Font("Consolas", 9F);
            textEcdhAlicePublic.Location = new Point(600, 47);
            textEcdhAlicePublic.Size = new Size(400, 60);
            textEcdhAlicePublic.Multiline = true;
            textEcdhAlicePublic.ScrollBars = ScrollBars.Vertical;
            textEcdhAlicePublic.Name = "textEcdhAlicePublic";
            textEcdhAlicePublic.TabIndex = 6;
            groupEcdh.Controls.Add(textEcdhAlicePublic);

            // Bob 私钥
            labelEcdhBobPriv.AutoSize = true;
            labelEcdhBobPriv.Location = new Point(8, 120);
            labelEcdhBobPriv.Name = "labelEcdhBobPriv";
            labelEcdhBobPriv.Text = "Bob 私钥：";
            groupEcdh.Controls.Add(labelEcdhBobPriv);

            textEcdhBobPrivate.Dock = DockStyle.None;
            textEcdhBobPrivate.Font = new Font("Consolas", 9F);
            textEcdhBobPrivate.Location = new Point(100, 117);
            textEcdhBobPrivate.Size = new Size(400, 60);
            textEcdhBobPrivate.Multiline = true;
            textEcdhBobPrivate.ScrollBars = ScrollBars.Vertical;
            textEcdhBobPrivate.Name = "textEcdhBobPrivate";
            textEcdhBobPrivate.TabIndex = 7;
            groupEcdh.Controls.Add(textEcdhBobPrivate);

            // Bob 公钥
            labelEcdhBobPub.AutoSize = true;
            labelEcdhBobPub.Location = new Point(520, 120);
            labelEcdhBobPub.Name = "labelEcdhBobPub";
            labelEcdhBobPub.Text = "Bob 公钥：";
            groupEcdh.Controls.Add(labelEcdhBobPub);

            textEcdhBobPublic.Dock = DockStyle.None;
            textEcdhBobPublic.Font = new Font("Consolas", 9F);
            textEcdhBobPublic.Location = new Point(600, 117);
            textEcdhBobPublic.Size = new Size(400, 60);
            textEcdhBobPublic.Multiline = true;
            textEcdhBobPublic.ScrollBars = ScrollBars.Vertical;
            textEcdhBobPublic.Name = "textEcdhBobPublic";
            textEcdhBobPublic.TabIndex = 8;
            groupEcdh.Controls.Add(textEcdhBobPublic);

            // 共享密钥
            labelEcdhShared.AutoSize = true;
            labelEcdhShared.Location = new Point(8, 190);
            labelEcdhShared.Name = "labelEcdhShared";
            labelEcdhShared.Text = "共享密钥：";
            groupEcdh.Controls.Add(labelEcdhShared);

            textEcdhSharedKey.Dock = DockStyle.None;
            textEcdhSharedKey.Font = new Font("Consolas", 9F);
            textEcdhSharedKey.Location = new Point(100, 187);
            textEcdhSharedKey.Size = new Size(400, 29);
            textEcdhSharedKey.Name = "textEcdhSharedKey";
            textEcdhSharedKey.ReadOnly = true;
            textEcdhSharedKey.TabIndex = 9;
            groupEcdh.Controls.Add(textEcdhSharedKey);

            // IV
            labelEcdhIV.AutoSize = true;
            labelEcdhIV.Location = new Point(520, 190);
            labelEcdhIV.Name = "labelEcdhIV";
            labelEcdhIV.Text = "IV/Nonce：";
            groupEcdh.Controls.Add(labelEcdhIV);

            textEcdhIV.Dock = DockStyle.None;
            textEcdhIV.Font = new Font("Consolas", 9F);
            textEcdhIV.Location = new Point(590, 187);
            textEcdhIV.Size = new Size(200, 29);
            textEcdhIV.Name = "textEcdhIV";
            textEcdhIV.TabIndex = 10;
            groupEcdh.Controls.Add(textEcdhIV);

            // 加密/解密按钮
            btnEcdhEncrypt.AutoSize = true;
            btnEcdhEncrypt.Location = new Point(8, 230);
            btnEcdhEncrypt.Size = new Size(75, 34);
            btnEcdhEncrypt.TabIndex = 11;
            btnEcdhEncrypt.Text = "加密";
            btnEcdhEncrypt.Click += BtnEcdhEncrypt_Click;
            groupEcdh.Controls.Add(btnEcdhEncrypt);

            btnEcdhDecrypt.AutoSize = true;
            btnEcdhDecrypt.Location = new Point(93, 230);
            btnEcdhDecrypt.Size = new Size(75, 34);
            btnEcdhDecrypt.TabIndex = 12;
            btnEcdhDecrypt.Text = "解密";
            btnEcdhDecrypt.Click += BtnEcdhDecrypt_Click;
            groupEcdh.Controls.Add(btnEcdhDecrypt);

            btnEcdhCopyResult.AutoSize = true;
            btnEcdhCopyResult.Location = new Point(178, 230);
            btnEcdhCopyResult.Size = new Size(92, 34);
            btnEcdhCopyResult.TabIndex = 13;
            btnEcdhCopyResult.Text = "复制结果";
            btnEcdhCopyResult.Click += BtnEcdhCopyResult_Click;
            groupEcdh.Controls.Add(btnEcdhCopyResult);

            btnEcdhPasteInput.AutoSize = true;
            btnEcdhPasteInput.Location = new Point(280, 230);
            btnEcdhPasteInput.Size = new Size(92, 34);
            btnEcdhPasteInput.TabIndex = 14;
            btnEcdhPasteInput.Text = "粘贴输入";
            btnEcdhPasteInput.Click += BtnEcdhPasteInput_Click;
            groupEcdh.Controls.Add(btnEcdhPasteInput);

            btnEcdhClear.AutoSize = true;
            btnEcdhClear.Location = new Point(382, 230);
            btnEcdhClear.Size = new Size(75, 34);
            btnEcdhClear.TabIndex = 15;
            btnEcdhClear.Text = "清空";
            btnEcdhClear.Click += BtnEcdhClear_Click;
            groupEcdh.Controls.Add(btnEcdhClear);

            // 明文输入
            labelEcdhInput.AutoSize = true;
            labelEcdhInput.Location = new Point(8, 275);
            labelEcdhInput.Name = "labelEcdhInput";
            labelEcdhInput.Text = "明文输入：";
            groupEcdh.Controls.Add(labelEcdhInput);

            textEcdhInput.Dock = DockStyle.None;
            textEcdhInput.Font = new Font("Consolas", 9F);
            textEcdhInput.Location = new Point(100, 272);
            textEcdhInput.Size = new Size(900, 60);
            textEcdhInput.Multiline = true;
            textEcdhInput.ScrollBars = ScrollBars.Vertical;
            textEcdhInput.Name = "textEcdhInput";
            textEcdhInput.TabIndex = 16;
            groupEcdh.Controls.Add(textEcdhInput);

            // 密文输出
            labelEcdhOutput.AutoSize = true;
            labelEcdhOutput.Location = new Point(8, 345);
            labelEcdhOutput.Name = "labelEcdhOutput";
            labelEcdhOutput.Text = "密文输出：";
            groupEcdh.Controls.Add(labelEcdhOutput);

            textEcdhOutput.Dock = DockStyle.None;
            textEcdhOutput.Font = new Font("Consolas", 9F);
            textEcdhOutput.Location = new Point(100, 342);
            textEcdhOutput.Size = new Size(900, 60);
            textEcdhOutput.Multiline = true;
            textEcdhOutput.ScrollBars = ScrollBars.Vertical;
            textEcdhOutput.Name = "textEcdhOutput";
            textEcdhOutput.TabIndex = 17;
            groupEcdh.Controls.Add(textEcdhOutput);

            // 加密结果提示
            labelEncResult.BackColor = Color.White;
            labelEncResult.BorderStyle = BorderStyle.None;
            labelEncResult.Font = new Font("Segoe UI", 9F);
            labelEncResult.ForeColor = Color.Gray;
            labelEncResult.Location = new Point(3, 39);
            labelEncResult.Multiline = true;
            labelEncResult.Name = "labelEncResult";
            labelEncResult.ReadOnly = true;
            labelEncResult.ScrollBars = ScrollBars.Vertical;
            labelEncResult.Size = new Size(364, 338);
            labelEncResult.TabIndex = 0;
            labelEncResult.Text = "加密结果:\r\n等待操作...";

            // ==================================================
            // SplitContainer - 签名/加密分隔器 & 文件结果分隔器
            // ==================================================
            splitSignEncrypt.Dock = DockStyle.Fill;
            splitSignEncrypt.Location = new Point(11, 710);
            splitSignEncrypt.Name = "splitSignEncrypt";
            splitSignEncrypt.Size = new Size(3265, 460);
            splitSignEncrypt.SplitterDistance = 1766;
            splitSignEncrypt.TabIndex = 2;

            // 文件加密/解密面板
            groupEncFile.Controls.Add(panelEncFileBtns);
            groupEncFile.Dock = DockStyle.Fill;
            groupEncFile.Location = new Point(1112, 403);
            groupEncFile.Name = "groupEncFile";
            groupEncFile.Padding = new Padding(8);
            groupEncFile.Size = new Size(364, 15);
            groupEncFile.TabIndex = 16;
            groupEncFile.TabStop = false;
            groupEncFile.Text = "文件加密/解密";

            panelEncFileBtns.Controls.Add(btnEncryptFile);
            panelEncFileBtns.Controls.Add(btnDecryptFile);
            panelEncFileBtns.Dock = DockStyle.Fill;
            panelEncFileBtns.Location = new Point(8, 31);
            panelEncFileBtns.Name = "panelEncFileBtns";
            panelEncFileBtns.Size = new Size(348, 0);
            panelEncFileBtns.TabIndex = 0;
            panelEncFileBtns.WrapContents = false;

            btnEncryptFile.AutoSize = true;
            btnEncryptFile.Location = new Point(3, 3);
            btnEncryptFile.Margin = new Padding(3, 3, 4, 3);
            btnEncryptFile.Name = "btnEncryptFile";
            btnEncryptFile.Size = new Size(92, 34);
            btnEncryptFile.TabIndex = 0;
            btnEncryptFile.Text = "加密文件";
            btnEncryptFile.Click += BtnEncryptFile_Click;

            btnDecryptFile.AutoSize = true;
            btnDecryptFile.Location = new Point(105, 3);
            btnDecryptFile.Margin = new Padding(6, 3, 4, 3);
            btnDecryptFile.Name = "btnDecryptFile";
            btnDecryptFile.Size = new Size(92, 34);
            btnDecryptFile.TabIndex = 1;
            btnDecryptFile.Text = "解密文件";
            btnDecryptFile.Click += BtnDecryptFile_Click;

            splitFileResult.Dock = DockStyle.Fill;
            splitFileResult.Location = new Point(11, 1176);
            splitFileResult.Name = "splitFileResult";
            splitFileResult.Size = new Size(3265, 429);
            splitFileResult.SplitterDistance = 2633;
            splitFileResult.TabIndex = 3;

            // ==================================================
            // EcdsaTabControl 自身
            // ==================================================
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainTableLayout);
            Name = "EcdsaTabControl";
            Size = new Size(3287, 1616);

            // ResumeLayout
            mainTableLayout.ResumeLayout(false);
            groupKey.ResumeLayout(false);
            tableLayoutKey.ResumeLayout(false);
            panelPrivateKeyBox.ResumeLayout(false);
            panelPrivateKeyBox.PerformLayout();
            panelPrivateKeyActions.ResumeLayout(false);
            panelPrivateKeyActions.PerformLayout();
            panelPublicKeyBox.ResumeLayout(false);
            panelPublicKeyBox.PerformLayout();
            panelPublicKeyActions.ResumeLayout(false);
            panelPublicKeyActions.PerformLayout();
            panelActionButtons.ResumeLayout(false);
            tableActionButtons.ResumeLayout(false);
            groupKeyActions.ResumeLayout(false);
            tableKeyActions.ResumeLayout(false);
            panelButtonArea.ResumeLayout(false);
            tableButtonArea.ResumeLayout(false);
            tableRightActions.ResumeLayout(false);
            tableRightActions.PerformLayout();
            panelKeyControlsContainer.ResumeLayout(false);
            panelKeyControls.ResumeLayout(false);
            panelRightSettings.ResumeLayout(false);
            panelRightSettings.PerformLayout();
            panelFormatRow.ResumeLayout(false);
            panelKeyTypeRow.ResumeLayout(false);
            panelKeyTypeRow.PerformLayout();
            radioPanel.ResumeLayout(false);
            radioPanel.PerformLayout();
            panelCurveContainer.ResumeLayout(false);
            panelCurveRow.ResumeLayout(false);
            groupComputeResult.ResumeLayout(false);
            groupRunResult.ResumeLayout(false);
            panelViewBar.ResumeLayout(false);
            panelViewBar.PerformLayout();
            panelViewContent.ResumeLayout(false);
            groupSign.ResumeLayout(false);
            tableLayoutSign.ResumeLayout(false);
            groupSignInput.ResumeLayout(false);
            panelSignInput.ResumeLayout(false);
            panelPlainDataBox.ResumeLayout(false);
            panelPlainDataBox.PerformLayout();
            panelPlainDataActions.ResumeLayout(false);
            panelPlainDataActions.PerformLayout();
            panelSignatureBox.ResumeLayout(false);
            panelSignatureBox.PerformLayout();
            panelSignatureActions.ResumeLayout(false);
            panelSignatureActions.PerformLayout();
            groupSignActions.ResumeLayout(false);
            panelSignActions.ResumeLayout(false);
            panelSignActions.PerformLayout();
            panelSignOptions.ResumeLayout(false);
            panelSignOptions.PerformLayout();
            groupEncrypt.ResumeLayout(false);
            tableLayoutEncrypt.ResumeLayout(false);
            tableLayoutEncrypt.PerformLayout();
            panelEncBtns.ResumeLayout(false);
            panelEncBtns.PerformLayout();
            groupFile.ResumeLayout(false);
            panelFileControls.ResumeLayout(false);
            panelFileControls.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitSignEncrypt).EndInit();
            splitSignEncrypt.ResumeLayout(false);
            groupEncFile.ResumeLayout(false);
            panelEncFileBtns.ResumeLayout(false);
            panelEncFileBtns.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)splitFileResult).EndInit();
            splitFileResult.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        #region 字段声明 - 按功能分组

        // [主布局]
        private TableLayoutPanel mainTableLayout = null!;

        // [密钥管理区]
        private GroupBox groupKey = null!;
        private TableLayoutPanel tableLayoutKey = null!;
        private Panel panelPrivateKeyBox = null!;
        private Panel panelPublicKeyBox = null!;
        private Label labelPrivateKey = null!;
        private Label labelPrivateActionsTitle = null!;
        private TextBox textPrivateKey = null!;
        private TableLayoutPanel panelPrivateKeyActions = null!;
        private Button btnCopyPrivateKey = null!;
        private Button btnPastePrivateKey = null!;
        private Button btnImportPrivateKey = null!;
        private Button btnSavePrivateKey = null!;
        private Button btnClearPrivateKey = null!;
        private Label labelPublicKey = null!;
        private Label labelPublicActionsTitle = null!;
        private TextBox textPublicKey = null!;
        private TableLayoutPanel panelPublicKeyActions = null!;
        private Button btnCopyPublicKey = null!;
        private Button btnPastePublicKey = null!;
        private Button btnImportPublicKey = null!;
        private Button btnSavePublicKey = null!;
        private Button btnClearPublicKey = null!;

        // [密钥操作面板 - 右侧]
        private Panel panelActionButtons = null!;
        private TableLayoutPanel tableActionButtons = null!;
        private GroupBox groupKeyActions = null!;
        private TableLayoutPanel tableKeyActions = null!;
        private Panel panelButtonArea = null!;
        private TableLayoutPanel tableButtonArea = null!;
        private TableLayoutPanel tableRightActions = null!;
        private Panel panelKeyControlsContainer = null!;
        private FlowLayoutPanel panelKeyControls = null!;
        private Button btnGenerateKeyPair = null!;
        private Button btnValidateKeyPair = null!;
        private Button btnGetPublicKeyFromPrivate = null!;
        private Button btnGetCurveType = null!;
        private Button btnClearAll = null!;
        private FlowLayoutPanel panelRightSettings = null!;
        private FlowLayoutPanel panelFormatRow = null!;
        private Label labelOutputFormat = null!;
        private ComboBox comboOutputFormat = null!;
        private FlowLayoutPanel panelKeyTypeRow = null!;
        private Label labelKeyType = null!;
        private FlowLayoutPanel radioPanel = null!;
        private RadioButton radioPrivateKey = null!;
        private RadioButton radioPublicKey = null!;
        private Button btnConvertKey = null!;
        private Panel panelCurveContainer = null!;
        private FlowLayoutPanel panelCurveRow = null!;
        private Label labelCurve = null!;
        private ComboBox comboCategory = null!;
        private Label lblArrow = null!;
        private ComboBox comboCurve = null!;
        private GroupBox groupComputeResult = null!;
        private RichTextBox textKeyResult = null!;
        private GroupBox groupRunResult = null!;
        private RichTextBox labelValidationResult = null!;

        // [视图切换]
        private FlowLayoutPanel panelViewBar = null!;
        private Button btnViewEcdh = null!;
        private Button btnViewSign = null!;
        private Button btnViewEncrypt = null!;
        private Button btnViewFile = null!;
        private Panel panelViewContent = null!;

        // [签名验签]
        private Panel groupSign = null!;
        private TableLayoutPanel tableLayoutSign = null!;
        private GroupBox groupSignInput = null!;
        private TableLayoutPanel panelSignInput = null!;
        private Panel panelPlainDataBox = null!;
        private Label labelPlainData = null!;
        private TextBox textPlainData = null!;
        private TableLayoutPanel panelPlainDataActions = null!;
        private Label labelPlainDataActionsTitle = null!;
        private Button btnCopyPlainData = null!;
        private Button btnPastePlainData = null!;
        private Button btnClearPlainData = null!;
        private Panel panelSignatureBox = null!;
        private Label labelSignature = null!;
        private TextBox textSignature = null!;
        private TableLayoutPanel panelSignatureActions = null!;
        private Label labelSignatureActionsTitle = null!;
        private Button btnCopySignatureData = null!;
        private Button btnPasteSignatureData = null!;
        private Button btnClearSignatureData = null!;
        private GroupBox groupSignActions = null!;
        private TableLayoutPanel panelSignActions = null!;
        private Button btnSign = null!;
        private Button btnVerify = null!;
        private Button btnCopySignature = null!;
        private TableLayoutPanel panelSignOptions = null!;
        private Label labelHashAlgorithm = null!;
        private ComboBox comboHashAlgorithm = null!;
        private Label labelSignatureFormat = null!;
        private ComboBox comboSignatureFormat = null!;

        // [加密/解密]
        private Panel groupEncrypt = null!;
        private TableLayoutPanel tableLayoutEncrypt = null!;
        private Label labelEncMode = null!;
        private ComboBox comboEncMode = null!;
        private Label labelEncInputFormat = null!;
        private ComboBox comboEncInputFormat = null!;
        private Label labelEncOutputFormat = null!;
        private ComboBox comboEncOutputFormat = null!;
        private Label labelEncKey = null!;
        private TextBox textEncKey = null!;
        private Label labelEncIV = null!;
        private TextBox textEncIV = null!;
        private Label labelEncBobPublic = null!;
        private TextBox textEncBobPublic = null!;
        private Label labelEncCurve = null!;
        private ComboBox comboEncCurveCategory = null!;
        private Label labelEncCurveArrow = null!;
        private ComboBox comboEncCurve = null!;
        private Label labelEncInput = null!;
        private TextBox textEncInput = null!;
        private TextBox textEncOutput = null!;
        private FlowLayoutPanel panelEncBtns = null!;
        private Button btnEncrypt = null!;
        private Button btnDecrypt = null!;
        private Button btnEncClear = null!;
        private Button btnEncCopy = null!;
        private Button btnEncPaste = null!;
        private Label labelEncOutputLabel = null!;

        // [文件操作]
        private GroupBox groupFile = null!;
        private FlowLayoutPanel panelFileControls = null!;
        private Button btnSignFile = null!;
        private Button btnVerifyFile = null!;

        // [ECDH 密钥协商]
        private Panel groupEcdh = null!;
        private TextBox labelEncResult = null!;
        private ComboBox comboEcdhMode = null!;
        private ComboBox comboEcdhCategory = null!;
        private ComboBox comboEcdhCurve = null!;
        private TextBox textEcdhAlicePrivate = null!;
        private TextBox textEcdhAlicePublic = null!;
        private TextBox textEcdhBobPrivate = null!;
        private TextBox textEcdhBobPublic = null!;
        private TextBox textEcdhSharedKey = null!;
        private TextBox textEcdhIV = null!;
        private TextBox textEcdhInput = null!;
        private TextBox textEcdhOutput = null!;
        private Button btnGenerateEcdhKeys = null!;
        private Button btnEcdhEncrypt = null!;
        private Button btnEcdhDecrypt = null!;
        private Button btnEcdhCopyResult = null!;
        private Button btnEcdhPasteInput = null!;
        private Button btnEcdhClear = null!;
        private ComboBox comboEcdhEncoding = null!;
        private Label labelEcdhMode = null!;
        private Label labelEcdhCategory = null!;
        private Label labelEcdhCurve = null!;
        private Label labelEcdhAlicePriv = null!;
        private Label labelEcdhAlicePub = null!;
        private Label labelEcdhBobPriv = null!;
        private Label labelEcdhBobPub = null!;
        private Label labelEcdhShared = null!;
        private Label labelEcdhIV = null!;
        private Label labelEcdhInput = null!;
        private Label labelEcdhOutput = null!;
        private Label labelEcdhEncoding = null!;

        // [分隔容器]
        private SplitContainer splitSignEncrypt = null!;
        private GroupBox groupEncFile = null!;
        private FlowLayoutPanel panelEncFileBtns = null!;
        private Button btnEncryptFile = null!;
        private Button btnDecryptFile = null!;
        private SplitContainer splitFileResult = null!;

        #endregion
    }
}
