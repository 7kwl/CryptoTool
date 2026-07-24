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
    /// <list type="bullet">
    ///   <item><b>左栏 50%:</b> 密钥管理(groupKey) | 签名验签(groupSign) | 加解密(groupEncrypt) | 文件操作(groupFile) | ECDH密钥协商(groupEcdh)</item>
    ///   <item><b>右栏 50%:</b> 运行结果(groupRunResult) / 计算结果(groupComputeResult)</item>
    /// </list>
    /// 
    /// <para>视图切换机制:</para>
    /// <list type="bullet">
    ///   <item>panelViewBar → 签名视图 / 加解密视图 / 文件视图 / ECDH视图 (按钮组)</item>
    ///   <item>panelViewContent → 根据当前选中视图显示对应的操作面板</item>
    /// </list>
    /// </summary>
    partial class EcdsaTabControl
    {
        /// <summary>设计器组件容器，用于管理所有控件的生命周期。</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 释放 Tab 页所使用的所有资源。
        /// </summary>
        /// <param name="disposing">true 表示同时释放托管资源和非托管资源；false 表示仅释放非托管资源。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary>
        /// 初始化本 Tab 页上的所有控件及其布局属性。
        /// 此方法由设计器自动生成，按以下板块组织:
        /// <list type="number">
        ///   <item><b>主布局</b> - mainTableLayout 左右两栏</item>
        ///   <item><b>顶层窗体</b> - AutoScaleDimensions, Size 等</item>
        ///   <item><b>视图切换栏</b> - panelViewBar + 4个视图按钮</item>
        ///   <item><b>密钥管理区</b> - 曲线选择、公私钥输入、生成/加载按钮</item>
        ///   <item><b>签名验签区</b> - Hash算法选择、消息输入、签名/验签按钮</item>
        ///   <item><b>加解密区</b> - 明文/密文输入框、加密/解密按钮</item>
        ///   <item><b>文件操作区</b> - 文件签名/验签、文件路径选择</item>
        ///   <item><b>ECDH密钥协商区</b> - Alice/Bob密钥对、共享密钥、加解密</item>
        ///   <item><b>运行结果区</b> - labelValidationResult + textKeyResult 输出框</item>
        ///   <item><b>面板收起/展开</b> - 各 GroupBox 的折叠控件</item>
        /// </list>
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
            panelPrivateKeyStandardRow = new FlowLayoutPanel();
            labelPrivateKeyStandard = new Label();
            comboPrivateKeyStandard = new ComboBox();
            btnConvertPrivateKeyStandard = new Button();
            panelPublicKeyStandardRow = new FlowLayoutPanel();
            labelPublicKeyStandard = new Label();
            comboPublicKeyStandard = new ComboBox();
            btnConvertPublicKeyStandard = new Button();
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
            panelPrivateKeyStandardRow.SuspendLayout();
            panelPublicKeyStandardRow.SuspendLayout();
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
            SuspendLayout();
            // ==================================================
            // 主布局 - 整体页面为2列：左(密钥面板) | 右(操作面板)
            // ==================================================
            // 
            // mainTableLayout
            // 
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
            //   内嵌 tableLayoutKey (1行3列): 私钥列 | 公钥列 | 操作按钮列
            // ==================================================
            // 
            // groupKey
            // 
            groupKey.Controls.Add(tableLayoutKey);
            groupKey.Dock = DockStyle.Fill;
            groupKey.Location = new Point(11, 11);
            groupKey.Name = "groupKey";
            groupKey.Padding = new Padding(8);
            groupKey.Size = new Size(1629, 687);
            groupKey.TabIndex = 0;
            groupKey.TabStop = false;
            groupKey.Text = "密钥生成";
            // 
            // tableLayoutKey
            // 
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
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 0F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 0F));
            tableLayoutKey.Size = new Size(1613, 648);
            tableLayoutKey.TabIndex = 0;
            // --------------------------------------------------
            // 私钥面板 - tableLayoutKey 第1列
            //   panelPrivateKeyBox 内含: labelPrivateKey + textPrivateKey
            // --------------------------------------------------
            // 
            // panelPrivateKeyBox
            // 
            panelPrivateKeyBox.Controls.Add(labelPrivateKey);
            panelPrivateKeyBox.Controls.Add(textPrivateKey);
            panelPrivateKeyBox.Dock = DockStyle.Fill;
            panelPrivateKeyBox.Location = new Point(9, 9);
            panelPrivateKeyBox.Name = "panelPrivateKeyBox";
            tableLayoutKey.SetRowSpan(panelPrivateKeyBox, 2);
            panelPrivateKeyBox.Size = new Size(1395, 312);
            panelPrivateKeyBox.TabIndex = 6;
            // 
            // labelPrivateKey
            // 
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
            // 
            // textPrivateKey
            // 
            textPrivateKey.Dock = DockStyle.Fill;
            textPrivateKey.Location = new Point(0, 0);
            textPrivateKey.Multiline = true;
            textPrivateKey.Name = "textPrivateKey";
            textPrivateKey.ScrollBars = ScrollBars.Vertical;
            textPrivateKey.Size = new Size(1395, 312);
            textPrivateKey.TabIndex = 1;
            textPrivateKey.TextChanged += TextPrivateKey_TextChanged;
            // --------------------------------------------------
            // 私钥操作按钮区 - tableLayoutKey 第2列上方
            //   6行网格: 标题 | 复制私钥 | 粘贴私钥 | 导入私钥 | 保存私钥 | 清空私钥
            // --------------------------------------------------
            // 
            // panelPrivateKeyActions
            // 
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
            // 
            // labelPrivateActionsTitle
            // 
            labelPrivateActionsTitle.AutoSize = true;
            labelPrivateActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPrivateActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelPrivateActionsTitle.Location = new Point(8, 4);
            labelPrivateActionsTitle.Margin = new Padding(0);
            labelPrivateActionsTitle.Name = "labelPrivateActionsTitle";
            labelPrivateActionsTitle.Size = new Size(123, 25);
            labelPrivateActionsTitle.TabIndex = 0;
            labelPrivateActionsTitle.Text = "🔑 私钥操作";
            // 
            // btnCopyPrivateKey
            // 
            btnCopyPrivateKey.Dock = DockStyle.Fill;
            btnCopyPrivateKey.Location = new Point(10, 32);
            btnCopyPrivateKey.Margin = new Padding(2);
            btnCopyPrivateKey.MinimumSize = new Size(120, 30);
            btnCopyPrivateKey.Name = "btnCopyPrivateKey";
            btnCopyPrivateKey.Size = new Size(174, 51);
            btnCopyPrivateKey.TabIndex = 0;
            btnCopyPrivateKey.Text = "复制私钥";
            btnCopyPrivateKey.Click += BtnCopyPrivateKey_Click;
            // 
            // btnPastePrivateKey
            // 
            btnPastePrivateKey.Dock = DockStyle.Fill;
            btnPastePrivateKey.Location = new Point(10, 87);
            btnPastePrivateKey.Margin = new Padding(2);
            btnPastePrivateKey.MinimumSize = new Size(120, 30);
            btnPastePrivateKey.Name = "btnPastePrivateKey";
            btnPastePrivateKey.Size = new Size(174, 51);
            btnPastePrivateKey.TabIndex = 1;
            btnPastePrivateKey.Text = "粘贴私钥";
            btnPastePrivateKey.Click += BtnPastePrivateKey_Click;
            // 
            // btnImportPrivateKey
            // 
            btnImportPrivateKey.Dock = DockStyle.Fill;
            btnImportPrivateKey.Location = new Point(10, 142);
            btnImportPrivateKey.Margin = new Padding(2);
            btnImportPrivateKey.MinimumSize = new Size(120, 30);
            btnImportPrivateKey.Name = "btnImportPrivateKey";
            btnImportPrivateKey.Size = new Size(174, 51);
            btnImportPrivateKey.TabIndex = 2;
            btnImportPrivateKey.Text = "导入私钥";
            btnImportPrivateKey.Click += BtnImportPrivateKey_Click;
            // 
            // btnSavePrivateKey
            // 
            btnSavePrivateKey.Dock = DockStyle.Fill;
            btnSavePrivateKey.Location = new Point(10, 197);
            btnSavePrivateKey.Margin = new Padding(2);
            btnSavePrivateKey.MinimumSize = new Size(120, 30);
            btnSavePrivateKey.Name = "btnSavePrivateKey";
            btnSavePrivateKey.Size = new Size(174, 51);
            btnSavePrivateKey.TabIndex = 3;
            btnSavePrivateKey.Text = "保存私钥";
            btnSavePrivateKey.Click += BtnSavePrivateKey_Click;
            // 
            // btnClearPrivateKey
            // 
            btnClearPrivateKey.Dock = DockStyle.Fill;
            btnClearPrivateKey.Location = new Point(10, 252);
            btnClearPrivateKey.Margin = new Padding(2);
            btnClearPrivateKey.MinimumSize = new Size(120, 30);
            btnClearPrivateKey.Name = "btnClearPrivateKey";
            btnClearPrivateKey.Size = new Size(174, 54);
            btnClearPrivateKey.TabIndex = 4;
            btnClearPrivateKey.Text = "清空私钥";
            btnClearPrivateKey.Click += BtnClearPrivateKey_Click;
            // --------------------------------------------------
            // 公钥面板 - tableLayoutKey 第1列下方
            //   panelPublicKeyBox 内含: labelPublicKey + textPublicKey
            // --------------------------------------------------
            // 
            // panelPublicKeyBox
            // 
            panelPublicKeyBox.Controls.Add(labelPublicKey);
            panelPublicKeyBox.Controls.Add(textPublicKey);
            panelPublicKeyBox.Dock = DockStyle.Fill;
            panelPublicKeyBox.Location = new Point(9, 327);
            panelPublicKeyBox.Name = "panelPublicKeyBox";
            tableLayoutKey.SetRowSpan(panelPublicKeyBox, 2);
            panelPublicKeyBox.Size = new Size(1395, 312);
            panelPublicKeyBox.TabIndex = 7;
            // 
            // labelPublicKey
            // 
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
            // 
            // textPublicKey
            // 
            textPublicKey.Dock = DockStyle.Fill;
            textPublicKey.Location = new Point(0, 0);
            textPublicKey.Multiline = true;
            textPublicKey.Name = "textPublicKey";
            textPublicKey.ScrollBars = ScrollBars.Vertical;
            textPublicKey.Size = new Size(1395, 312);
            textPublicKey.TabIndex = 3;
            // --------------------------------------------------
            // 公钥操作按钮区 - tableLayoutKey 第2列下方
            //   6行网格: 标题 | 复制公钥 | 粘贴公钥 | 导入公钥 | 保存公钥 | 清空公钥
            // --------------------------------------------------
            // 
            // panelPublicKeyActions
            // 
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
            // 
            // labelPublicActionsTitle
            // 
            labelPublicActionsTitle.AutoSize = true;
            labelPublicActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPublicActionsTitle.ForeColor = Color.FromArgb(0, 100, 180);
            labelPublicActionsTitle.Location = new Point(8, 4);
            labelPublicActionsTitle.Margin = new Padding(0);
            labelPublicActionsTitle.Name = "labelPublicActionsTitle";
            labelPublicActionsTitle.Size = new Size(123, 25);
            labelPublicActionsTitle.TabIndex = 0;
            labelPublicActionsTitle.Text = "🔓 公钥操作";
            // 
            // btnCopyPublicKey
            // 
            btnCopyPublicKey.Dock = DockStyle.Fill;
            btnCopyPublicKey.Location = new Point(10, 32);
            btnCopyPublicKey.Margin = new Padding(2);
            btnCopyPublicKey.MinimumSize = new Size(120, 30);
            btnCopyPublicKey.Name = "btnCopyPublicKey";
            btnCopyPublicKey.Size = new Size(174, 51);
            btnCopyPublicKey.TabIndex = 0;
            btnCopyPublicKey.Text = "复制公钥";
            btnCopyPublicKey.Click += BtnCopyPublicKey_Click;
            // 
            // btnPastePublicKey
            // 
            btnPastePublicKey.Dock = DockStyle.Fill;
            btnPastePublicKey.Location = new Point(10, 87);
            btnPastePublicKey.Margin = new Padding(2);
            btnPastePublicKey.MinimumSize = new Size(120, 30);
            btnPastePublicKey.Name = "btnPastePublicKey";
            btnPastePublicKey.Size = new Size(174, 51);
            btnPastePublicKey.TabIndex = 1;
            btnPastePublicKey.Text = "粘贴公钥";
            btnPastePublicKey.Click += BtnPastePublicKey_Click;
            // 
            // btnImportPublicKey
            // 
            btnImportPublicKey.Dock = DockStyle.Fill;
            btnImportPublicKey.Location = new Point(10, 142);
            btnImportPublicKey.Margin = new Padding(2);
            btnImportPublicKey.MinimumSize = new Size(120, 30);
            btnImportPublicKey.Name = "btnImportPublicKey";
            btnImportPublicKey.Size = new Size(174, 51);
            btnImportPublicKey.TabIndex = 2;
            btnImportPublicKey.Text = "导入公钥";
            btnImportPublicKey.Click += BtnImportPublicKey_Click;
            // 
            // btnSavePublicKey
            // 
            btnSavePublicKey.Dock = DockStyle.Fill;
            btnSavePublicKey.Location = new Point(10, 197);
            btnSavePublicKey.Margin = new Padding(2);
            btnSavePublicKey.MinimumSize = new Size(120, 30);
            btnSavePublicKey.Name = "btnSavePublicKey";
            btnSavePublicKey.Size = new Size(174, 51);
            btnSavePublicKey.TabIndex = 3;
            btnSavePublicKey.Text = "保存公钥";
            btnSavePublicKey.Click += BtnSavePublicKey_Click;
            // 
            // btnClearPublicKey
            // 
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
            //   panelActionButtons > tableActionButtons(2列) > groupKeyActions + right side
            // ==================================================
            // 
            // panelActionButtons
            // 
            panelActionButtons.Controls.Add(tableActionButtons);
            panelActionButtons.Dock = DockStyle.Fill;
            panelActionButtons.Location = new Point(1643, 8);
            panelActionButtons.Margin = new Padding(0);
            panelActionButtons.Name = "panelActionButtons";
            panelActionButtons.Size = new Size(1636, 693);
            panelActionButtons.TabIndex = 1;
            // 
            // tableActionButtons
            // 
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
            // --------------------------------------------------
            // 密钥操作 GroupBox - 内嵌 tableKeyActions(2行)
            //   上半部分: panelButtonArea(按钮+设置)
            //   下半部分: groupComputeResult(左) + groupRunResult(右)
            // --------------------------------------------------
            // 
            // groupKeyActions
            // 
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
            // 
            // tableKeyActions
            // 
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
            // --------------------------------------------------
            // 密钥操作按钮与设置区 (panelButtonArea)
            //   tableButtonArea(2列):
            //     左: panelKeyControlsContainer > panelKeyControls(FlowLayout按钮)
            //     右: panelRightSettings(输出格式/密钥类型/曲线选择)
            // --------------------------------------------------
            // 
            // panelButtonArea
            // 
            tableKeyActions.SetColumnSpan(panelButtonArea, 2);
            panelButtonArea.Controls.Add(tableButtonArea);
            panelButtonArea.Dock = DockStyle.Fill;
            panelButtonArea.Location = new Point(3, 3);
            panelButtonArea.Name = "panelButtonArea";
            panelButtonArea.Size = new Size(1608, 318);
            panelButtonArea.TabIndex = 1;
            // 
            // tableButtonArea
            // 
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
            // 
            // tableRightActions
            // 
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
            // 密钥操作按钮容器 - FlowLayoutPanel 动态排列
            // 
            // panelKeyControlsContainer
            // 
            panelKeyControlsContainer.Controls.Add(panelKeyControls);
            panelKeyControlsContainer.Dock = DockStyle.Fill;
            panelKeyControlsContainer.Location = new Point(3, 3);
            panelKeyControlsContainer.Name = "panelKeyControlsContainer";
            panelKeyControlsContainer.Padding = new Padding(6, 0, 6, 0);
            tableRightActions.SetRowSpan(panelKeyControlsContainer, 3);
            panelKeyControlsContainer.Size = new Size(174, 306);
            panelKeyControlsContainer.TabIndex = 1;
            // 
            // panelKeyControls
            // 
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
            // ==================================================
            // 密钥操作核心按钮 (panelKeyControls 内的 FlowLayout 子控件)
            //   生成密钥对 | 验证密钥对 | 从私钥导出公钥 | 获取曲线类型 | 全部清空
            // ==================================================
            // 
            // btnGenerateKeyPair
            // 
            btnGenerateKeyPair.Location = new Point(0, 0);
            btnGenerateKeyPair.Margin = new Padding(0, 0, 6, 0);
            btnGenerateKeyPair.Name = "btnGenerateKeyPair";
            btnGenerateKeyPair.Size = new Size(150, 40);
            btnGenerateKeyPair.TabIndex = 0;
            btnGenerateKeyPair.Text = "生成密钥对";
            btnGenerateKeyPair.Click += BtnGenerateKeyPair_Click;
            // 
            // btnValidateKeyPair
            // 
            btnValidateKeyPair.Location = new Point(0, 43);
            btnValidateKeyPair.Margin = new Padding(0, 3, 6, 0);
            btnValidateKeyPair.Name = "btnValidateKeyPair";
            btnValidateKeyPair.Size = new Size(150, 40);
            btnValidateKeyPair.TabIndex = 1;
            btnValidateKeyPair.Text = "验证密钥对";
            btnValidateKeyPair.Click += BtnValidateKeyPair_Click;
            // 
            // btnGetPublicKeyFromPrivate
            // 
            btnGetPublicKeyFromPrivate.Location = new Point(0, 86);
            btnGetPublicKeyFromPrivate.Margin = new Padding(0, 3, 6, 0);
            btnGetPublicKeyFromPrivate.Name = "btnGetPublicKeyFromPrivate";
            btnGetPublicKeyFromPrivate.Size = new Size(150, 40);
            btnGetPublicKeyFromPrivate.TabIndex = 2;
            btnGetPublicKeyFromPrivate.Text = "从私钥提取公钥";
            btnGetPublicKeyFromPrivate.Click += BtnGetPublicKeyFromPrivate_Click;
            // 
            // btnGetCurveType
            // 
            btnGetCurveType.Location = new Point(0, 129);
            btnGetCurveType.Margin = new Padding(0, 3, 6, 0);
            btnGetCurveType.Name = "btnGetCurveType";
            btnGetCurveType.Size = new Size(150, 40);
            btnGetCurveType.TabIndex = 3;
            btnGetCurveType.Text = "获取私钥曲线类型";
            btnGetCurveType.Click += BtnGetCurveType_Click;
            // 
            // btnClearAll
            // 
            btnClearAll.Location = new Point(0, 172);
            btnClearAll.Margin = new Padding(0, 3, 6, 0);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.Size = new Size(150, 40);
            btnClearAll.TabIndex = 4;
            btnClearAll.Text = "清空全部";
            btnClearAll.Click += BtnClearAll_Click;
            // --------------------------------------------------
            // 右侧设置面板 (panelRightSettings) - 6行网格布局
            //   输出格式行 | 密钥类型转换行 | 私钥存储标准行 | 公钥存储标准行 | 曲线分类行 | 降级曲线行
            // --------------------------------------------------
            // 
            // panelRightSettings
            // 
            panelRightSettings.AutoSize = true;
            panelRightSettings.Controls.Add(panelFormatRow);
            panelRightSettings.Controls.Add(panelKeyTypeRow);
            panelRightSettings.Controls.Add(panelPrivateKeyStandardRow);
            panelRightSettings.Controls.Add(panelPublicKeyStandardRow);
            panelRightSettings.Controls.Add(panelCurveContainer);
            panelRightSettings.Dock = DockStyle.Top;
            panelRightSettings.FlowDirection = FlowDirection.TopDown;
            panelRightSettings.Location = new Point(183, 3);
            panelRightSettings.Name = "panelRightSettings";
            tableRightActions.SetRowSpan(panelRightSettings, 3);
            panelRightSettings.Size = new Size(1416, 134);
            panelRightSettings.TabIndex = 2;
            panelRightSettings.WrapContents = false;
            // ---- 第1行: 输出格式 (PEM/DER/Base64/Hex) ----
            // 
            // panelFormatRow
            // 
            panelFormatRow.AutoSize = true;
            panelFormatRow.Controls.Add(labelOutputFormat);
            panelFormatRow.Controls.Add(comboOutputFormat);
            panelFormatRow.Location = new Point(3, 3);
            panelFormatRow.Name = "panelFormatRow";
            panelFormatRow.Padding = new Padding(6, 0, 6, 0);
            panelFormatRow.Size = new Size(303, 38);
            panelFormatRow.TabIndex = 0;
            // 
            // labelOutputFormat
            // 
            labelOutputFormat.Location = new Point(40, 3);
            labelOutputFormat.Margin = new Padding(34, 3, 2, 3);
            labelOutputFormat.Name = "labelOutputFormat";
            labelOutputFormat.Size = new Size(100, 32);
            labelOutputFormat.TabIndex = 2;
            labelOutputFormat.Text = "输出格式：";
            labelOutputFormat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboOutputFormat
            // 
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
            // 
            // panelKeyTypeRow
            // 
            panelKeyTypeRow.AutoSize = true;
            panelKeyTypeRow.Controls.Add(labelKeyType);
            panelKeyTypeRow.Controls.Add(radioPanel);
            panelKeyTypeRow.Controls.Add(btnConvertKey);
            panelKeyTypeRow.Location = new Point(3, 47);
            panelKeyTypeRow.Name = "panelKeyTypeRow";
            panelKeyTypeRow.Padding = new Padding(6, 0, 6, 0);
            panelKeyTypeRow.Size = new Size(409, 40);
            panelKeyTypeRow.TabIndex = 1;
            // 
            // labelKeyType
            // 
            labelKeyType.Location = new Point(40, 3);
            labelKeyType.Margin = new Padding(34, 3, 2, 3);
            labelKeyType.Name = "labelKeyType";
            labelKeyType.Size = new Size(100, 34);
            labelKeyType.TabIndex = 4;
            labelKeyType.Text = "密钥类型：";
            labelKeyType.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // radioPanel
            // 
            radioPanel.AutoSize = true;
            radioPanel.Controls.Add(radioPrivateKey);
            radioPanel.Controls.Add(radioPublicKey);
            radioPanel.Location = new Point(142, 3);
            radioPanel.Margin = new Padding(0, 3, 8, 3);
            radioPanel.Name = "radioPanel";
            radioPanel.Size = new Size(157, 34);
            radioPanel.TabIndex = 5;
            // 
            // radioPrivateKey
            // 
            radioPrivateKey.AutoSize = true;
            radioPrivateKey.Checked = true;
            radioPrivateKey.Location = new Point(3, 3);
            radioPrivateKey.Margin = new Padding(3, 3, 6, 3);
            radioPrivateKey.Name = "radioPrivateKey";
            radioPrivateKey.Size = new Size(71, 28);
            radioPrivateKey.TabIndex = 0;
            radioPrivateKey.TabStop = true;
            radioPrivateKey.Text = "私钥";
            // 
            // radioPublicKey
            // 
            radioPublicKey.AutoSize = true;
            radioPublicKey.Location = new Point(83, 3);
            radioPublicKey.Name = "radioPublicKey";
            radioPublicKey.Size = new Size(71, 28);
            radioPublicKey.TabIndex = 1;
            radioPublicKey.Text = "公钥";
            // 
            // btnConvertKey
            // 
            btnConvertKey.AutoSize = true;
            btnConvertKey.Location = new Point(319, 3);
            btnConvertKey.Margin = new Padding(12, 3, 4, 3);
            btnConvertKey.MinimumSize = new Size(80, 26);
            btnConvertKey.Name = "btnConvertKey";
            btnConvertKey.Size = new Size(80, 34);
            btnConvertKey.TabIndex = 6;
            btnConvertKey.Text = "转换";
            btnConvertKey.Click += BtnConvertKey_Click;
            // ---- 第3行: 私钥存储标准 (SEC1 / PKCS#8) ----
            // 
            // panelPrivateKeyStandardRow
            // 
            panelPrivateKeyStandardRow.AutoSize = true;
            panelPrivateKeyStandardRow.Controls.Add(labelPrivateKeyStandard);
            panelPrivateKeyStandardRow.Controls.Add(comboPrivateKeyStandard);
            panelPrivateKeyStandardRow.Controls.Add(btnConvertPrivateKeyStandard);
            panelPrivateKeyStandardRow.Location = new Point(3, 93);
            panelPrivateKeyStandardRow.Name = "panelPrivateKeyStandardRow";
            panelPrivateKeyStandardRow.Padding = new Padding(6, 0, 6, 0);
            panelPrivateKeyStandardRow.Size = new Size(450, 38);
            panelPrivateKeyStandardRow.TabIndex = 7;
            // 
            // labelPrivateKeyStandard
            // 
            labelPrivateKeyStandard.AutoSize = true;
            labelPrivateKeyStandard.Location = new Point(9, 7);
            labelPrivateKeyStandard.Margin = new Padding(3, 7, 3, 3);
            labelPrivateKeyStandard.Name = "labelPrivateKeyStandard";
            labelPrivateKeyStandard.Size = new Size(118, 24);
            labelPrivateKeyStandard.TabIndex = 0;
            labelPrivateKeyStandard.Text = "私钥存储标准：";
            // 
            // comboPrivateKeyStandard
            // 
            comboPrivateKeyStandard.DropDownStyle = ComboBoxStyle.DropDownList;
            comboPrivateKeyStandard.FormattingEnabled = true;
            comboPrivateKeyStandard.Location = new Point(133, 3);
            comboPrivateKeyStandard.Margin = new Padding(0, 3, 8, 3);
            comboPrivateKeyStandard.Name = "comboPrivateKeyStandard";
            comboPrivateKeyStandard.Size = new Size(420, 32);
            comboPrivateKeyStandard.TabIndex = 1;
            // 
            // btnConvertPrivateKeyStandard
            // 
            btnConvertPrivateKeyStandard.AutoSize = true;
            btnConvertPrivateKeyStandard.Location = new Point(361, 3);
            btnConvertPrivateKeyStandard.Margin = new Padding(0, 3, 4, 3);
            btnConvertPrivateKeyStandard.MinimumSize = new Size(80, 26);
            btnConvertPrivateKeyStandard.Name = "btnConvertPrivateKeyStandard";
            btnConvertPrivateKeyStandard.Size = new Size(80, 34);
            btnConvertPrivateKeyStandard.TabIndex = 2;
            btnConvertPrivateKeyStandard.Text = "转换";
            btnConvertPrivateKeyStandard.Click += BtnConvertPrivateKeyStandard_Click;
            // ---- 第3.5行: 公钥存储标准 (RFC 5480/namedCurve / specifiedCurve) ----
            // 
            // panelPublicKeyStandardRow
            // 
            panelPublicKeyStandardRow.AutoSize = true;
            panelPublicKeyStandardRow.Controls.Add(labelPublicKeyStandard);
            panelPublicKeyStandardRow.Controls.Add(comboPublicKeyStandard);
            panelPublicKeyStandardRow.Controls.Add(btnConvertPublicKeyStandard);
            panelPublicKeyStandardRow.Location = new Point(3, 93);
            panelPublicKeyStandardRow.Name = "panelPublicKeyStandardRow";
            panelPublicKeyStandardRow.Padding = new Padding(6, 0, 6, 0);
            panelPublicKeyStandardRow.Size = new Size(450, 38);
            panelPublicKeyStandardRow.TabIndex = 8;
            // 
            // labelPublicKeyStandard
            // 
            labelPublicKeyStandard.AutoSize = true;
            labelPublicKeyStandard.Location = new Point(9, 7);
            labelPublicKeyStandard.Margin = new Padding(3, 7, 3, 3);
            labelPublicKeyStandard.Name = "labelPublicKeyStandard";
            labelPublicKeyStandard.Size = new Size(118, 24);
            labelPublicKeyStandard.TabIndex = 0;
            labelPublicKeyStandard.Text = "公钥存储标准：";
            // 
            // comboPublicKeyStandard
            // 
            comboPublicKeyStandard.DropDownStyle = ComboBoxStyle.DropDownList;
            comboPublicKeyStandard.FormattingEnabled = true;
            comboPublicKeyStandard.Location = new Point(133, 3);
            comboPublicKeyStandard.Margin = new Padding(0, 3, 8, 3);
            comboPublicKeyStandard.Name = "comboPublicKeyStandard";
            comboPublicKeyStandard.Size = new Size(420, 32);
            comboPublicKeyStandard.TabIndex = 1;
            // 
            // btnConvertPublicKeyStandard
            // 
            btnConvertPublicKeyStandard.AutoSize = true;
            btnConvertPublicKeyStandard.Location = new Point(361, 3);
            btnConvertPublicKeyStandard.Margin = new Padding(0, 3, 4, 3);
            btnConvertPublicKeyStandard.MinimumSize = new Size(80, 26);
            btnConvertPublicKeyStandard.Name = "btnConvertPublicKeyStandard";
            btnConvertPublicKeyStandard.Size = new Size(80, 34);
            btnConvertPublicKeyStandard.TabIndex = 2;
            btnConvertPublicKeyStandard.Text = "转换";
            btnConvertPublicKeyStandard.Click += BtnConvertPublicKeyStandard_Click;
            // ---- 第4行: 椭圆曲线选择 (类别 → 曲线名) ----
            // 
            // panelCurveContainer
            // 
            panelCurveContainer.Controls.Add(panelCurveRow);
            panelCurveContainer.Location = new Point(3, 93);
            panelCurveContainer.Name = "panelCurveContainer";
            panelCurveContainer.Padding = new Padding(6, 0, 6, 0);
            panelCurveContainer.Size = new Size(920, 38);
            panelCurveContainer.TabIndex = 2;
            // 
            // panelCurveRow
            // 
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
            // 
            // labelCurve
            // 
            labelCurve.Location = new Point(34, 5);
            labelCurve.Margin = new Padding(34, 3, 2, 3);
            labelCurve.Name = "labelCurve";
            labelCurve.Size = new Size(200, 32);
            labelCurve.TabIndex = 0;
            labelCurve.Text = "椭圆曲线：";
            labelCurve.TextAlign = ContentAlignment.MiddleLeft;
            labelCurve.Click += LabelCurve_Click;
            // 
            // comboCategory
            // 
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new Point(136, 5);
            comboCategory.Margin = new Padding(0, 3, 4, 3);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new Size(183, 32);
            comboCategory.TabIndex = 1;
            comboCategory.SelectedIndexChanged += ComboCategory_SelectedIndexChanged;
            // 
            // lblArrow
            // 
            lblArrow.Location = new Point(327, 5);
            lblArrow.Margin = new Padding(4, 3, 4, 3);
            lblArrow.Name = "lblArrow";
            lblArrow.Padding = new Padding(4, 0, 4, 0);
            lblArrow.Size = new Size(36, 32);
            lblArrow.TabIndex = 2;
            lblArrow.Text = "→";
            lblArrow.TextAlign = ContentAlignment.MiddleCenter;
            // 
            // comboCurve
            // 
            comboCurve.DisplayMember = "Value";
            comboCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCurve.FormattingEnabled = true;
            comboCurve.Location = new Point(367, 5);
            comboCurve.Margin = new Padding(0, 3, 4, 3);
            comboCurve.Name = "comboCurve";
            comboCurve.Size = new Size(330, 32);
            comboCurve.TabIndex = 3;
            comboCurve.ValueMember = "Key";
            // ==================================================
            // 结果显示区 - tableKeyActions 第2行
            //   left (groupComputeResult): 计算结果文本框
            //   right (groupRunResult): 运行结果文本框
            // ==================================================
            // 
            // groupComputeResult
            // 
            groupComputeResult.Controls.Add(textKeyResult);
            groupComputeResult.Dock = DockStyle.Fill;
            groupComputeResult.Location = new Point(3, 327);
            groupComputeResult.Name = "groupComputeResult";
            groupComputeResult.Padding = new Padding(8);
            groupComputeResult.Size = new Size(801, 318);
            groupComputeResult.TabIndex = 0;
            groupComputeResult.TabStop = false;
            groupComputeResult.Text = "计算结果";
            // 
            // textKeyResult
            // 
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
            // 
            // groupRunResult
            // 
            groupRunResult.Controls.Add(labelValidationResult);
            groupRunResult.Dock = DockStyle.Fill;
            groupRunResult.Location = new Point(810, 327);
            groupRunResult.Name = "groupRunResult";
            groupRunResult.Padding = new Padding(8);
            groupRunResult.Size = new Size(801, 318);
            groupRunResult.TabIndex = 1;
            groupRunResult.TabStop = false;
            groupRunResult.Text = "运行结果";
            // 
            // labelValidationResult
            // 
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
            //   4个Tab按钮: ECDH | ECIES签名验签 | 加密解密 | 文件操作
            // ==================================================
            // 
            // panelViewBar
            // 
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
            // ---- 4个视图切换Button: ECDH | ECIES签名验签 | ECIES加密解密 | 文件签名/验签 ----
            // 
            // btnViewEcdh
            // 
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
            // 
            // btnViewSign
            // 
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
            // 
            // btnViewEncrypt
            // 
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
            // 
            // btnViewFile
            // 
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
            // --------------------------------------------------
            // 视图内容面板 (panelViewContent) - 包含4个可切换面板
            //   groupSign(签名验签) | groupEncrypt(加密解密) | groupFile(文件操作) | groupEcdh(密钥协商)
            // --------------------------------------------------
            // 
            // panelViewContent
            // 
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
            //   内嵌 tableLayoutSign(2列): groupSignInput(左) | groupSignActions(右)
            // ==================================================
            // 
            // groupSign
            // 
            groupSign.Controls.Add(tableLayoutSign);
            groupSign.Dock = DockStyle.Fill;
            groupSign.Location = new Point(0, 0);
            groupSign.Name = "groupSign";
            groupSign.Padding = new Padding(4);
            groupSign.Size = new Size(3265, 841);
            groupSign.TabIndex = 0;
            // 
            // tableLayoutSign
            // 
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
            // --------------------------------------------------
            // 签名验签输入区 (groupSignInput) - tableLayoutSign 左列
            //   panelSignInput (2列4行): 原始数据(上) | 签名数据(下)
            // --------------------------------------------------
            // 
            // groupSignInput
            // 
            groupSignInput.Controls.Add(panelSignInput);
            groupSignInput.Dock = DockStyle.Fill;
            groupSignInput.Location = new Point(3, 3);
            groupSignInput.Name = "groupSignInput";
            groupSignInput.Padding = new Padding(8);
            groupSignInput.Size = new Size(1622, 827);
            groupSignInput.TabIndex = 0;
            groupSignInput.TabStop = false;
            groupSignInput.Text = "签名验签";
            // 
            // panelSignInput
            // 
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
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Percent, 0F));
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            panelSignInput.RowStyles.Add(new RowStyle(SizeType.Percent, 0F));
            panelSignInput.Size = new Size(1606, 788);
            panelSignInput.TabIndex = 0;
            // ---- 原始数据区 (panelSignInput 第1行) ----
            //   panelPlainDataBox: labelPlainData(标签) + textPlainData(输入框)
            //   panelPlainDataActions: 复制|粘贴|清空 操作按钮
            // 
            // panelPlainDataBox
            // 
            panelPlainDataBox.Controls.Add(labelPlainData);
            panelPlainDataBox.Controls.Add(textPlainData);
            panelPlainDataBox.Dock = DockStyle.Fill;
            panelPlainDataBox.Location = new Point(9, 9);
            panelPlainDataBox.Name = "panelPlainDataBox";
            panelSignInput.SetRowSpan(panelPlainDataBox, 2);
            panelPlainDataBox.Size = new Size(1388, 382);
            panelPlainDataBox.TabIndex = 0;
            // 
            // labelPlainData
            // 
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
            // 
            // textPlainData
            // 
            textPlainData.Dock = DockStyle.Fill;
            textPlainData.Location = new Point(0, 0);
            textPlainData.Multiline = true;
            textPlainData.Name = "textPlainData";
            textPlainData.ScrollBars = ScrollBars.Vertical;
            textPlainData.Size = new Size(1388, 382);
            textPlainData.TabIndex = 2;
            // 
            // panelPlainDataActions
            // 
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
            // 
            // labelPlainDataActionsTitle
            // 
            labelPlainDataActionsTitle.AutoSize = true;
            labelPlainDataActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPlainDataActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelPlainDataActionsTitle.Location = new Point(8, 4);
            labelPlainDataActionsTitle.Margin = new Padding(0);
            labelPlainDataActionsTitle.Name = "labelPlainDataActionsTitle";
            labelPlainDataActionsTitle.Size = new Size(92, 25);
            labelPlainDataActionsTitle.TabIndex = 0;
            labelPlainDataActionsTitle.Text = "数据操作";
            // 
            // btnCopyPlainData
            // 
            btnCopyPlainData.Dock = DockStyle.Fill;
            btnCopyPlainData.Location = new Point(10, 32);
            btnCopyPlainData.Margin = new Padding(2);
            btnCopyPlainData.MinimumSize = new Size(120, 30);
            btnCopyPlainData.Name = "btnCopyPlainData";
            btnCopyPlainData.Size = new Size(174, 110);
            btnCopyPlainData.TabIndex = 1;
            btnCopyPlainData.Text = "复制数据";
            btnCopyPlainData.Click += BtnCopyPlainData_Click;
            // 
            // btnPastePlainData
            // 
            btnPastePlainData.Dock = DockStyle.Fill;
            btnPastePlainData.Location = new Point(10, 146);
            btnPastePlainData.Margin = new Padding(2);
            btnPastePlainData.MinimumSize = new Size(120, 30);
            btnPastePlainData.Name = "btnPastePlainData";
            btnPastePlainData.Size = new Size(174, 110);
            btnPastePlainData.TabIndex = 2;
            btnPastePlainData.Text = "粘贴数据";
            btnPastePlainData.Click += BtnPastePlainData_Click;
            // 
            // btnClearPlainData
            // 
            btnClearPlainData.Dock = DockStyle.Fill;
            btnClearPlainData.Location = new Point(10, 260);
            btnClearPlainData.Margin = new Padding(2);
            btnClearPlainData.MinimumSize = new Size(120, 30);
            btnClearPlainData.Name = "btnClearPlainData";
            btnClearPlainData.Size = new Size(174, 116);
            btnClearPlainData.TabIndex = 3;
            btnClearPlainData.Text = "清空数据";
            btnClearPlainData.Click += BtnClearPlainData_Click;
            // ---- 签名数据区 (panelSignInput 第3行) ----
            //   panelSignatureBox: labelSignature(标签) + textSignature(输入框)
            //   panelSignatureActions: 复制|粘贴|清空 操作按钮
            // 
            // panelSignatureBox
            // 
            panelSignatureBox.Controls.Add(labelSignature);
            panelSignatureBox.Controls.Add(textSignature);
            panelSignatureBox.Dock = DockStyle.Fill;
            panelSignatureBox.Location = new Point(9, 397);
            panelSignatureBox.Name = "panelSignatureBox";
            panelSignInput.SetRowSpan(panelSignatureBox, 2);
            panelSignatureBox.Size = new Size(1388, 382);
            panelSignatureBox.TabIndex = 1;
            // 
            // labelSignature
            // 
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
            // 
            // textSignature
            // 
            textSignature.Dock = DockStyle.Fill;
            textSignature.Location = new Point(0, 0);
            textSignature.Multiline = true;
            textSignature.Name = "textSignature";
            textSignature.ScrollBars = ScrollBars.Vertical;
            textSignature.Size = new Size(1388, 382);
            textSignature.TabIndex = 4;
            // 
            // panelSignatureActions
            // 
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
            // 
            // labelSignatureActionsTitle
            // 
            labelSignatureActionsTitle.AutoSize = true;
            labelSignatureActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelSignatureActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelSignatureActionsTitle.Location = new Point(8, 4);
            labelSignatureActionsTitle.Margin = new Padding(0);
            labelSignatureActionsTitle.Name = "labelSignatureActionsTitle";
            labelSignatureActionsTitle.Size = new Size(92, 25);
            labelSignatureActionsTitle.TabIndex = 0;
            labelSignatureActionsTitle.Text = "签名操作";
            // 
            // btnCopySignatureData
            // 
            btnCopySignatureData.Dock = DockStyle.Fill;
            btnCopySignatureData.Location = new Point(10, 32);
            btnCopySignatureData.Margin = new Padding(2);
            btnCopySignatureData.MinimumSize = new Size(120, 30);
            btnCopySignatureData.Name = "btnCopySignatureData";
            btnCopySignatureData.Size = new Size(174, 110);
            btnCopySignatureData.TabIndex = 1;
            btnCopySignatureData.Text = "复制签名";
            btnCopySignatureData.Click += BtnCopySignatureData_Click;
            // 
            // btnPasteSignatureData
            // 
            btnPasteSignatureData.Dock = DockStyle.Fill;
            btnPasteSignatureData.Location = new Point(10, 146);
            btnPasteSignatureData.Margin = new Padding(2);
            btnPasteSignatureData.MinimumSize = new Size(120, 30);
            btnPasteSignatureData.Name = "btnPasteSignatureData";
            btnPasteSignatureData.Size = new Size(174, 110);
            btnPasteSignatureData.TabIndex = 2;
            btnPasteSignatureData.Text = "粘贴签名";
            btnPasteSignatureData.Click += BtnPasteSignatureData_Click;
            // 
            // btnClearSignatureData
            // 
            btnClearSignatureData.Dock = DockStyle.Fill;
            btnClearSignatureData.Location = new Point(10, 260);
            btnClearSignatureData.Margin = new Padding(2);
            btnClearSignatureData.MinimumSize = new Size(120, 30);
            btnClearSignatureData.Name = "btnClearSignatureData";
            btnClearSignatureData.Size = new Size(174, 116);
            btnClearSignatureData.TabIndex = 3;
            btnClearSignatureData.Text = "清空签名";
            btnClearSignatureData.Click += BtnClearSignatureData_Click;
            // 
            // --------------------------------------------------
            // 签名验签操作区 (groupSignActions) - tableLayoutSign 右列
            //   panelSignActions(2列): 操作按钮(签名|验签|复制签名) | 签名选项(哈希算法|格式)
            // --------------------------------------------------
            // groupSignActions
            // 
            groupSignActions.Controls.Add(panelSignActions);
            groupSignActions.Dock = DockStyle.Fill;
            groupSignActions.Location = new Point(1631, 3);
            groupSignActions.Name = "groupSignActions";
            groupSignActions.Padding = new Padding(8);
            groupSignActions.Size = new Size(1623, 827);
            groupSignActions.TabIndex = 1;
            groupSignActions.TabStop = false;
            groupSignActions.Text = "操作按钮";
            // 
            // panelSignActions
            // 
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
            // ---- 签名操作按钮 (panelSignActions 左列) ----
            // btnSign | btnVerify | btnCopySignature (垂直排列)
            // 
            // btnSign
            // 
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
            // 
            // btnVerify
            // 
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
            // 
            // btnCopySignature
            // 
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
            // ---- 签名选项 (panelSignActions 右列) ----
            //   哈希算法 + 签名格式 选择
            // 
            // panelSignOptions
            // 
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
            // 
            // labelHashAlgorithm
            // 
            labelHashAlgorithm.AutoSize = true;
            labelHashAlgorithm.Location = new Point(0, 4);
            labelHashAlgorithm.Margin = new Padding(0, 4, 4, 0);
            labelHashAlgorithm.Name = "labelHashAlgorithm";
            labelHashAlgorithm.Size = new Size(107, 24);
            labelHashAlgorithm.TabIndex = 0;
            labelHashAlgorithm.Text = "Hash算法：";
            labelHashAlgorithm.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboHashAlgorithm
            // 
            comboHashAlgorithm.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboHashAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            comboHashAlgorithm.FormattingEnabled = true;
            comboHashAlgorithm.Items.AddRange(new object[] { "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512" });
            comboHashAlgorithm.Location = new Point(111, 0);
            comboHashAlgorithm.Margin = new Padding(0, 0, 16, 0);
            comboHashAlgorithm.Name = "comboHashAlgorithm";
            comboHashAlgorithm.Size = new Size(1338, 32);
            comboHashAlgorithm.TabIndex = 1;
            // 
            // labelSignatureFormat
            // 
            labelSignatureFormat.AutoSize = true;
            labelSignatureFormat.Location = new Point(0, 36);
            labelSignatureFormat.Margin = new Padding(0, 4, 4, 0);
            labelSignatureFormat.Name = "labelSignatureFormat";
            labelSignatureFormat.Size = new Size(100, 24);
            labelSignatureFormat.TabIndex = 2;
            labelSignatureFormat.Text = "签名格式：";
            labelSignatureFormat.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // comboSignatureFormat
            // 
            comboSignatureFormat.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboSignatureFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSignatureFormat.FormattingEnabled = true;
            comboSignatureFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboSignatureFormat.Location = new Point(111, 32);
            comboSignatureFormat.Margin = new Padding(0);
            comboSignatureFormat.Name = "comboSignatureFormat";
            comboSignatureFormat.Size = new Size(1354, 32);
            comboSignatureFormat.TabIndex = 3;
            groupEncrypt.Controls.Add(tableLayoutEncrypt);
            groupEncrypt.Dock = DockStyle.Fill;
            tableLayoutEncrypt.Dock = DockStyle.Fill;

            // ==================================================
            // 文件签名/验签面板 (groupFile)
            //   panelFileControls: btnSignFile | btnVerifyFile
            // ==================================================
            // 
            // 
            // groupFile
            // 
            groupFile.Controls.Add(panelFileControls);
            groupFile.Dock = DockStyle.Fill;
            groupFile.Location = new Point(0, 0);
            groupFile.Name = "groupFile";
            groupFile.Padding = new Padding(8);
            groupFile.Size = new Size(3265, 841);
            groupFile.TabIndex = 0;
            groupFile.TabStop = false;
            // 
            // panelFileControls
            // 
            panelFileControls.Controls.Add(btnSignFile);
            panelFileControls.Controls.Add(btnVerifyFile);
            panelFileControls.Dock = DockStyle.Fill;
            panelFileControls.Location = new Point(8, 31);
            panelFileControls.Name = "panelFileControls";
            panelFileControls.Padding = new Padding(4, 2, 4, 2);
            panelFileControls.Size = new Size(3249, 802);
            panelFileControls.TabIndex = 0;
            panelFileControls.WrapContents = false;
            // 
            // btnSignFile
            // 
            btnSignFile.AutoSize = true;
            btnSignFile.Location = new Point(8, 5);
            btnSignFile.Margin = new Padding(4, 3, 4, 3);
            btnSignFile.Name = "btnSignFile";
            btnSignFile.Size = new Size(92, 34);
            btnSignFile.TabIndex = 0;
            btnSignFile.Text = "签名文件";
            btnSignFile.Click += BtnSignFile_Click;
            // 
            // btnVerifyFile
            // 
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
            //   内部展示 encResult 等密钥协商结果
            // ==================================================
            // 
            // groupEcdh
            // 
            groupEcdh.Dock = DockStyle.Fill;
            groupEcdh.Location = new Point(0, 0);
            groupEcdh.Name = "groupEcdh";
            groupEcdh.Padding = new Padding(8);
            groupEcdh.Size = new Size(3265, 841);
            groupEcdh.TabIndex = 0;
            // 
            // labelEncResult
            // 
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
            // 签名/加密分隔器 (splitSignEncrypt)
            //   实现签名验签面板与加密解密面板可拖拽调整大小
            // ==================================================
            // 
            // splitSignEncrypt
            // 
            splitSignEncrypt.Dock = DockStyle.Fill;
            splitSignEncrypt.Location = new Point(11, 710);
            splitSignEncrypt.Name = "splitSignEncrypt";
            splitSignEncrypt.Size = new Size(3265, 460);
            splitSignEncrypt.SplitterDistance = 1766;
            splitSignEncrypt.TabIndex = 2;
            // ==================================================
            // 文件加密/解密面板 (groupEncFile)
            //   panelEncFileBtns > btnEncryptFile | btnDecryptFile
            // ==================================================
            // 
            // groupEncFile
            // 
            groupEncFile.Controls.Add(panelEncFileBtns);
            groupEncFile.Dock = DockStyle.Fill;
            groupEncFile.Location = new Point(1112, 403);
            groupEncFile.Name = "groupEncFile";
            groupEncFile.Padding = new Padding(8);
            groupEncFile.Size = new Size(364, 15);
            groupEncFile.TabIndex = 16;
            groupEncFile.TabStop = false;
            groupEncFile.Text = "文件加密/解密";
            // 
            // panelEncFileBtns
            // 
            panelEncFileBtns.Controls.Add(btnEncryptFile);
            panelEncFileBtns.Controls.Add(btnDecryptFile);
            panelEncFileBtns.Dock = DockStyle.Fill;
            panelEncFileBtns.Location = new Point(8, 31);
            panelEncFileBtns.Name = "panelEncFileBtns";
            panelEncFileBtns.Size = new Size(348, 0);
            panelEncFileBtns.TabIndex = 0;
            panelEncFileBtns.WrapContents = false;
            // 
            // btnEncryptFile
            // 
            btnEncryptFile.AutoSize = true;
            btnEncryptFile.Location = new Point(3, 3);
            btnEncryptFile.Margin = new Padding(3, 3, 4, 3);
            btnEncryptFile.Name = "btnEncryptFile";
            btnEncryptFile.Size = new Size(92, 34);
            btnEncryptFile.TabIndex = 0;
            btnEncryptFile.Text = "加密文件";
            btnEncryptFile.Click += BtnEncryptFile_Click;
            // 
            // btnDecryptFile
            // 
            btnDecryptFile.AutoSize = true;
            btnDecryptFile.Location = new Point(105, 3);
            btnDecryptFile.Margin = new Padding(6, 3, 4, 3);
            btnDecryptFile.Name = "btnDecryptFile";
            btnDecryptFile.Size = new Size(92, 34);
            btnDecryptFile.TabIndex = 1;
            btnDecryptFile.Text = "解密文件";
            btnDecryptFile.Click += BtnDecryptFile_Click;
            // ==================================================
            // 文件结果分隔器 (splitFileResult)
            //   分隔加解密面板与文件操作结果区域
            // ==================================================
            // 
            // splitFileResult
            // 
            splitFileResult.Dock = DockStyle.Fill;
            splitFileResult.Location = new Point(11, 1176);
            splitFileResult.Name = "splitFileResult";
            splitFileResult.Size = new Size(3265, 429);
            splitFileResult.SplitterDistance = 2633;
            splitFileResult.TabIndex = 3;
            // ==================================================
            // 顶层控件 EcdsaTabControl - 画面叶子节点
            //   使用 mainTableLayout 作为根布局面板
            // ==================================================
            // 
            // EcdsaTabControl
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainTableLayout);
            Name = "EcdsaTabControl";
            Size = new Size(3287, 1616);
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
            panelPrivateKeyStandardRow.ResumeLayout(false);
            panelPrivateKeyStandardRow.PerformLayout();
            panelPublicKeyStandardRow.ResumeLayout(false);
            panelPublicKeyStandardRow.PerformLayout();
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

        // ==========================================
        // 字段声明 - 所有控件的字段声明按板块分组
        // ==========================================
        // [主布局]
        private System.Windows.Forms.TableLayoutPanel mainTableLayout;
        // [密钥管理区 - groupKey]
        private System.Windows.Forms.GroupBox groupKey;
        private System.Windows.Forms.TableLayoutPanel tableLayoutKey;
        private System.Windows.Forms.Panel panelPrivateKeyBox;
        private System.Windows.Forms.Panel panelPublicKeyBox;
        private System.Windows.Forms.Label labelPrivateKey;
        private System.Windows.Forms.Label labelPrivateActionsTitle;
        private System.Windows.Forms.TextBox textPrivateKey;
        private System.Windows.Forms.TableLayoutPanel panelPrivateKeyActions;
        private System.Windows.Forms.Button btnCopyPrivateKey;
        private System.Windows.Forms.Button btnPastePrivateKey;
        private System.Windows.Forms.Button btnImportPrivateKey;
        private System.Windows.Forms.Button btnSavePrivateKey;
        private System.Windows.Forms.Button btnClearPrivateKey;
        private System.Windows.Forms.Label labelPublicKey;
        private System.Windows.Forms.Label labelPublicActionsTitle;
        private System.Windows.Forms.TextBox textPublicKey;
        private System.Windows.Forms.TableLayoutPanel panelPublicKeyActions;
        private System.Windows.Forms.Button btnCopyPublicKey;
        private System.Windows.Forms.Button btnPastePublicKey;
        private System.Windows.Forms.Button btnImportPublicKey;
        private System.Windows.Forms.Button btnSavePublicKey;
        private System.Windows.Forms.Button btnClearPublicKey;
        // [右侧设置面板 - panelActionButtons]
        private System.Windows.Forms.Panel panelActionButtons;
        private System.Windows.Forms.TableLayoutPanel tableActionButtons;
        private System.Windows.Forms.Panel panelButtonArea;
        private System.Windows.Forms.TableLayoutPanel tableButtonArea;
        private System.Windows.Forms.TableLayoutPanel tableRightActions;
        private System.Windows.Forms.FlowLayoutPanel panelFormatRow;
        private System.Windows.Forms.FlowLayoutPanel panelKeyTypeRow;
        private System.Windows.Forms.Label labelOutputFormat;
        private System.Windows.Forms.ComboBox comboOutputFormat;
        private System.Windows.Forms.Label labelKeyType;
        private System.Windows.Forms.FlowLayoutPanel radioPanel;
        private System.Windows.Forms.RadioButton radioPrivateKey;
        private System.Windows.Forms.RadioButton radioPublicKey;
        private System.Windows.Forms.Button btnConvertKey;
        private System.Windows.Forms.FlowLayoutPanel panelPrivateKeyStandardRow;
        private System.Windows.Forms.Label labelPrivateKeyStandard;
        private System.Windows.Forms.ComboBox comboPrivateKeyStandard;
        private System.Windows.Forms.Button btnConvertPrivateKeyStandard;
        private System.Windows.Forms.FlowLayoutPanel panelPublicKeyStandardRow;
        private System.Windows.Forms.Label labelPublicKeyStandard;
        private System.Windows.Forms.ComboBox comboPublicKeyStandard;
        private System.Windows.Forms.Button btnConvertPublicKeyStandard;
        private System.Windows.Forms.Panel panelKeyControlsContainer;
        private System.Windows.Forms.FlowLayoutPanel panelRightSettings;
        private System.Windows.Forms.FlowLayoutPanel panelKeyControls;
        // [密钥操作按钮 - groupKeyActions]
        private System.Windows.Forms.Button btnGenerateKeyPair;
        private System.Windows.Forms.Button btnValidateKeyPair;
        private System.Windows.Forms.Button btnGetPublicKeyFromPrivate;
        private System.Windows.Forms.Button btnGetCurveType;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Panel panelCurveContainer;
        private System.Windows.Forms.FlowLayoutPanel panelCurveRow;
        private System.Windows.Forms.Label labelCurve;
        private System.Windows.Forms.ComboBox comboCategory;
        private System.Windows.Forms.Label lblArrow;
        private System.Windows.Forms.ComboBox comboCurve;
        // [签名验签区 - groupSign]
        private System.Windows.Forms.SplitContainer splitSignEncrypt;
        private System.Windows.Forms.Panel groupSign;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSign;
        private System.Windows.Forms.GroupBox groupSignInput;
        private System.Windows.Forms.TableLayoutPanel panelSignInput;
        private System.Windows.Forms.Panel panelPlainDataBox;
        private System.Windows.Forms.Label labelPlainData;
        private System.Windows.Forms.TextBox textPlainData;
        private System.Windows.Forms.TableLayoutPanel panelPlainDataActions;
        private System.Windows.Forms.Label labelPlainDataActionsTitle;
        private System.Windows.Forms.Button btnCopyPlainData;
        private System.Windows.Forms.Button btnPastePlainData;
        private System.Windows.Forms.Button btnClearPlainData;
        private System.Windows.Forms.Panel panelSignatureBox;
        private System.Windows.Forms.Label labelSignature;
        private System.Windows.Forms.TextBox textSignature;
        private System.Windows.Forms.TableLayoutPanel panelSignatureActions;
        private System.Windows.Forms.Label labelSignatureActionsTitle;
        private System.Windows.Forms.Button btnCopySignatureData;
        private System.Windows.Forms.Button btnPasteSignatureData;
        private System.Windows.Forms.Button btnClearSignatureData;
        private System.Windows.Forms.GroupBox groupSignActions;
        private System.Windows.Forms.TableLayoutPanel panelSignActions;
        private System.Windows.Forms.Label labelHashAlgorithm;
        private System.Windows.Forms.ComboBox comboHashAlgorithm;
        private System.Windows.Forms.Label labelSignatureFormat;
        private System.Windows.Forms.ComboBox comboSignatureFormat;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnCopySignature;
        // [加解密区 - groupEncrypt]
        private System.Windows.Forms.Panel groupEncrypt;
        private System.Windows.Forms.TableLayoutPanel tableLayoutEncrypt;
        private System.Windows.Forms.Label labelEncMode;
        private System.Windows.Forms.ComboBox comboEncMode;
        private System.Windows.Forms.Label labelEncInputFormat;
        private System.Windows.Forms.ComboBox comboEncInputFormat;
        private System.Windows.Forms.Label labelEncOutputFormat;
        private System.Windows.Forms.ComboBox comboEncOutputFormat;
        private System.Windows.Forms.Label labelEncKey;
        private System.Windows.Forms.TextBox textEncKey;
        private System.Windows.Forms.Label labelEncIV;
        private System.Windows.Forms.TextBox textEncIV;
        private System.Windows.Forms.Label labelEncBobPublic;
        private System.Windows.Forms.TextBox textEncBobPublic;
        private System.Windows.Forms.Label labelEncCurve;
        private System.Windows.Forms.ComboBox comboEncCurveCategory;
        private System.Windows.Forms.Label labelEncCurveArrow;
        private System.Windows.Forms.ComboBox comboEncCurve;
        private System.Windows.Forms.Label labelEncInput;
        private System.Windows.Forms.TextBox textEncInput;
        private System.Windows.Forms.Label labelEncOutputLabel;
        private System.Windows.Forms.TextBox textEncOutput;
        private System.Windows.Forms.Label labelEncEphemeralPub;
        private System.Windows.Forms.TextBox textEncEphemeralPub;
        private System.Windows.Forms.Label labelEncExtra;
        private System.Windows.Forms.TextBox textEncExtra;
        private System.Windows.Forms.FlowLayoutPanel panelEncBtns;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnEncClear;
        private System.Windows.Forms.Button btnEncCopy;
        private System.Windows.Forms.Button btnEncPaste;
        // [文件操作区 - groupFile + groupEncFile]
        private System.Windows.Forms.GroupBox groupEncFile;
        private System.Windows.Forms.FlowLayoutPanel panelEncFileBtns;
        private System.Windows.Forms.Button btnEncryptFile;
        private System.Windows.Forms.Button btnDecryptFile;
        private System.Windows.Forms.TextBox labelEncResult;
        private System.Windows.Forms.SplitContainer splitFileResult;
        private System.Windows.Forms.GroupBox groupFile;
        private System.Windows.Forms.FlowLayoutPanel panelFileControls;
        private System.Windows.Forms.Button btnSignFile;
        private System.Windows.Forms.Button btnVerifyFile;
        // [运行结果区]
        private System.Windows.Forms.GroupBox groupRunResult;
        private System.Windows.Forms.GroupBox groupKeyActions;
        private System.Windows.Forms.GroupBox groupComputeResult;
        private System.Windows.Forms.TableLayoutPanel tableKeyActions;
        private System.Windows.Forms.RichTextBox textKeyResult;
        private System.Windows.Forms.RichTextBox labelValidationResult;
        // [视图切换栏]
        private System.Windows.Forms.FlowLayoutPanel panelViewBar;
        private System.Windows.Forms.Button btnViewSign;
        private System.Windows.Forms.Button btnViewEncrypt;
        private System.Windows.Forms.Button btnViewFile;
        private System.Windows.Forms.Button btnViewEcdh;
        private System.Windows.Forms.Panel panelViewContent;
        private System.Windows.Forms.Panel groupEcdh;

        // [ECDH 密钥协商区 - 动态创建的控件]
        private System.Windows.Forms.ComboBox comboEcdhCategory;
        private System.Windows.Forms.ComboBox comboEcdhCurve;
        private System.Windows.Forms.ComboBox comboEcdhMode;
        private System.Windows.Forms.ComboBox comboEcdhEncoding;
        private System.Windows.Forms.ComboBox comboEcdhPrivateKeyStandard;
        private System.Windows.Forms.ComboBox comboEcdhPublicKeyStandard;
        private System.Windows.Forms.Button btnConvertEcdhPrivateKeyStandard;
        private System.Windows.Forms.Button btnConvertEcdhPublicKeyStandard;
        private System.Windows.Forms.Button btnGenerateEcdhKeys;
        private System.Windows.Forms.TextBox textEcdhAlicePrivate;
        private System.Windows.Forms.TextBox textEcdhAlicePublic;
        private System.Windows.Forms.TextBox textEcdhBobPrivate;
        private System.Windows.Forms.TextBox textEcdhBobPublic;
        private System.Windows.Forms.TextBox textEcdhInput;
        private System.Windows.Forms.TextBox textEcdhOutput;
        private System.Windows.Forms.TextBox textEcdhSharedKey;
        private System.Windows.Forms.TextBox textEcdhIV;
        private System.Windows.Forms.Label lblEcdhIV;
        private System.Windows.Forms.Button btnEcdhEncrypt;
        private System.Windows.Forms.Button btnEcdhDecrypt;
        private System.Windows.Forms.Button btnEcdhCopyResult;
        private System.Windows.Forms.Button btnEcdhPasteInput;
        private System.Windows.Forms.Button btnEcdhClear;
        private System.Windows.Forms.Button btnEcdhAliceCurve;
        private System.Windows.Forms.Button btnEcdhBobCurve;
        private TableLayoutPanel panelSignOptions;
    }
}

