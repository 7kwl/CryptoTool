using System.Drawing;
using System.Windows.Forms;

namespace CryptoTool.Win
{
    partial class EcdsaTabControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        private void InitializeComponent()
        {
            mainTableLayout = new TableLayoutPanel();
            groupKey = new GroupBox();
            tableLayoutKey = new TableLayoutPanel();
            labelPrivateKey = new Label();
            labelPrivateActionsTitle = new Label();
            textPrivateKey = new TextBox();
            panelPrivateKeyActions = new FlowLayoutPanel();
            btnCopyPrivateKey = new Button();
            btnPastePrivateKey = new Button();
            btnImportPrivateKey = new Button();
            btnSavePrivateKey = new Button();
            btnClearPrivateKey = new Button();
            labelPublicKey = new Label();
            labelPublicActionsTitle = new Label();
            textPublicKey = new TextBox();
            panelPublicKeyActions = new FlowLayoutPanel();
            btnCopyPublicKey = new Button();
            btnPastePublicKey = new Button();
            btnImportPublicKey = new Button();
            btnSavePublicKey = new Button();
            btnClearPublicKey = new Button();
            groupActionButtons = new GroupBox();
            tableActionButtons = new TableLayoutPanel();
            panelButtonArea = new Panel();
            tableButtonArea = new TableLayoutPanel();
            panelRightScroll = new Panel();
            tableRightActions = new TableLayoutPanel();
            panelKeyControlsContainer = new Panel();
            panelKeyControls = new FlowLayoutPanel();
            btnGenerateKeyPair = new Button();
            btnValidateKeyPair = new Button();
            btnGetPublicKeyFromPrivate = new Button();
            btnGetCurveType = new Button();
            btnClearAll = new Button();
            panelFormatRow = new FlowLayoutPanel();
            labelInputFormat = new Label();
            comboInputFormat = new ComboBox();
            labelOutputFormat = new Label();
            comboOutputFormat = new ComboBox();
            panelKeyTypeRow = new FlowLayoutPanel();
            labelKeyType = new Label();
            radioPanel = new FlowLayoutPanel();
            radioPrivateKey = new RadioButton();
            radioPublicKey = new RadioButton();
            btnConvertKey = new Button();
            panelCurveContainer = new Panel();
            labelCurveHeader = new Label();
            panelCurveRow = new FlowLayoutPanel();
            labelCurve = new Label();
            comboCategory = new ComboBox();
            lblArrow = new Label();
            comboCurve = new ComboBox();
            splitSignEncrypt = new SplitContainer();
            groupSign = new GroupBox();
            tableLayoutSign = new TableLayoutPanel();
            labelPlainData = new Label();
            panelSignActions = new FlowLayoutPanel();
            labelHashAlgorithm = new Label();
            comboHashAlgorithm = new ComboBox();
            labelSignatureFormat = new Label();
            comboSignatureFormat = new ComboBox();
            btnSign = new Button();
            btnVerify = new Button();
            btnCopySignature = new Button();
            textPlainData = new TextBox();
            labelSignature = new Label();
            textSignature = new TextBox();
            groupEncrypt = new GroupBox();
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
            labelEncInput = new Label();
            textEncInput = new TextBox();
            textEncOutput = new TextBox();
            panelEncBtns = new FlowLayoutPanel();
            btnEncrypt = new Button();
            btnDecrypt = new Button();
            btnEncClear = new Button();
            btnEncCopy = new Button();
            btnEncPaste = new Button();
            groupEncFile = new GroupBox();
            panelEncFileBtns = new FlowLayoutPanel();
            btnEncryptFile = new Button();
            btnDecryptFile = new Button();
            labelEncResult = new TextBox();
            labelEncOutputLabel = new Label();
            splitFileResult = new SplitContainer();
            groupFile = new GroupBox();
            panelFileControls = new FlowLayoutPanel();
            btnSignFile = new Button();
            btnVerifyFile = new Button();
            groupRunResult = new GroupBox();
            labelValidationResult = new TextBox();
            mainTableLayout.SuspendLayout();
            groupKey.SuspendLayout();
            tableLayoutKey.SuspendLayout();
            panelPrivateKeyActions.SuspendLayout();
            panelPublicKeyActions.SuspendLayout();
            groupActionButtons.SuspendLayout();
            tableActionButtons.SuspendLayout();
            panelButtonArea.SuspendLayout();
            tableButtonArea.SuspendLayout();
            panelRightScroll.SuspendLayout();
            tableRightActions.SuspendLayout();
            panelKeyControlsContainer.SuspendLayout();
            panelKeyControls.SuspendLayout();
            panelFormatRow.SuspendLayout();
            panelKeyTypeRow.SuspendLayout();
            radioPanel.SuspendLayout();
            panelCurveContainer.SuspendLayout();
            panelCurveRow.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitSignEncrypt).BeginInit();
            splitSignEncrypt.Panel1.SuspendLayout();
            splitSignEncrypt.Panel2.SuspendLayout();
            splitSignEncrypt.SuspendLayout();
            groupSign.SuspendLayout();
            tableLayoutSign.SuspendLayout();
            panelSignActions.SuspendLayout();
            groupEncrypt.SuspendLayout();
            tableLayoutEncrypt.SuspendLayout();
            panelEncBtns.SuspendLayout();
            groupEncFile.SuspendLayout();
            panelEncFileBtns.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitFileResult).BeginInit();
            splitFileResult.Panel1.SuspendLayout();
            splitFileResult.Panel2.SuspendLayout();
            splitFileResult.SuspendLayout();
            groupFile.SuspendLayout();
            panelFileControls.SuspendLayout();
            groupRunResult.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 2;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 54.1118927F));
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 45.8881073F));
            mainTableLayout.Controls.Add(groupKey, 0, 0);
            mainTableLayout.Controls.Add(groupActionButtons, 1, 0);
            mainTableLayout.Controls.Add(splitSignEncrypt, 0, 1);
            mainTableLayout.Controls.Add(splitFileResult, 0, 2);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(8);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 28F));
            mainTableLayout.Size = new Size(3287, 1616);
            mainTableLayout.TabIndex = 0;
            // 
            // groupKey
            // 
            groupKey.Controls.Add(tableLayoutKey);
            groupKey.Dock = DockStyle.Fill;
            groupKey.Location = new Point(11, 11);
            groupKey.Name = "groupKey";
            groupKey.Padding = new Padding(8);
            groupKey.Size = new Size(1764, 693);
            groupKey.TabIndex = 0;
            groupKey.TabStop = false;
            groupKey.Text = "密钥生成";
            // 
            // tableLayoutKey
            // 
            tableLayoutKey.ColumnCount = 2;
            tableLayoutKey.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 89.0553F));
            tableLayoutKey.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.9447F));
            tableLayoutKey.Controls.Add(labelPrivateKey, 0, 0);
            tableLayoutKey.Controls.Add(labelPrivateActionsTitle, 1, 0);
            tableLayoutKey.Controls.Add(textPrivateKey, 0, 1);
            tableLayoutKey.Controls.Add(panelPrivateKeyActions, 1, 1);
            tableLayoutKey.Controls.Add(labelPublicKey, 0, 2);
            tableLayoutKey.Controls.Add(labelPublicActionsTitle, 1, 2);
            tableLayoutKey.Controls.Add(textPublicKey, 0, 3);
            tableLayoutKey.Controls.Add(panelPublicKeyActions, 1, 3);
            tableLayoutKey.Dock = DockStyle.Fill;
            tableLayoutKey.Location = new Point(8, 31);
            tableLayoutKey.Name = "tableLayoutKey";
            tableLayoutKey.Padding = new Padding(6);
            tableLayoutKey.RowCount = 4;
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutKey.RowStyles.Add(new RowStyle(SizeType.Percent, 20F));
            tableLayoutKey.Size = new Size(1748, 654);
            tableLayoutKey.TabIndex = 0;
            // 
            // labelPrivateKey
            // 
            labelPrivateKey.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelPrivateKey.AutoSize = true;
            labelPrivateKey.Location = new Point(10, 10);
            labelPrivateKey.Margin = new Padding(4, 4, 4, 2);
            labelPrivateKey.Name = "labelPrivateKey";
            labelPrivateKey.Padding = new Padding(4, 0, 4, 0);
            labelPrivateKey.Size = new Size(128, 22);
            labelPrivateKey.TabIndex = 0;
            labelPrivateKey.Text = "私钥 (PEM)：";
            // 
            // labelPrivateActionsTitle
            // 
            labelPrivateActionsTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelPrivateActionsTitle.AutoSize = true;
            labelPrivateActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPrivateActionsTitle.ForeColor = Color.FromArgb(192, 0, 0);
            labelPrivateActionsTitle.Location = new Point(1556, 10);
            labelPrivateActionsTitle.Margin = new Padding(4, 4, 4, 2);
            labelPrivateActionsTitle.Name = "labelPrivateActionsTitle";
            labelPrivateActionsTitle.Padding = new Padding(4, 0, 4, 0);
            labelPrivateActionsTitle.Size = new Size(131, 22);
            labelPrivateActionsTitle.TabIndex = 0;
            labelPrivateActionsTitle.Text = "🔑 私钥操作";
            // 
            // textPrivateKey
            // 
            textPrivateKey.Dock = DockStyle.Fill;
            textPrivateKey.Location = new Point(9, 37);
            textPrivateKey.Multiline = true;
            textPrivateKey.Name = "textPrivateKey";
            textPrivateKey.ScrollBars = ScrollBars.Vertical;
            textPrivateKey.Size = new Size(1540, 287);
            textPrivateKey.TabIndex = 1;
            textPrivateKey.TextChanged += textPrivateKey_TextChanged;
            // 
            // panelPrivateKeyActions
            // 
            panelPrivateKeyActions.Controls.Add(btnCopyPrivateKey);
            panelPrivateKeyActions.Controls.Add(btnPastePrivateKey);
            panelPrivateKeyActions.Controls.Add(btnImportPrivateKey);
            panelPrivateKeyActions.Controls.Add(btnSavePrivateKey);
            panelPrivateKeyActions.Controls.Add(btnClearPrivateKey);
            panelPrivateKeyActions.FlowDirection = FlowDirection.TopDown;
            panelPrivateKeyActions.Location = new Point(1555, 37);
            panelPrivateKeyActions.Name = "panelPrivateKeyActions";
            panelPrivateKeyActions.Padding = new Padding(8, 4, 8, 4);
            panelPrivateKeyActions.Size = new Size(184, 287);
            panelPrivateKeyActions.TabIndex = 4;
            panelPrivateKeyActions.WrapContents = false;
            // 
            // btnCopyPrivateKey
            // 
            btnCopyPrivateKey.AutoSize = true;
            btnCopyPrivateKey.Dock = DockStyle.Top;
            btnCopyPrivateKey.Location = new Point(8, 4);
            btnCopyPrivateKey.Margin = new Padding(0, 0, 0, 4);
            btnCopyPrivateKey.MinimumSize = new Size(120, 30);
            btnCopyPrivateKey.Name = "btnCopyPrivateKey";
            btnCopyPrivateKey.Size = new Size(120, 40);
            btnCopyPrivateKey.TabIndex = 0;
            btnCopyPrivateKey.Text = "复制私钥";
            btnCopyPrivateKey.Click += btnCopyPrivateKey_Click;
            // 
            // btnPastePrivateKey
            // 
            btnPastePrivateKey.AutoSize = true;
            btnPastePrivateKey.Dock = DockStyle.Top;
            btnPastePrivateKey.Location = new Point(8, 48);
            btnPastePrivateKey.Margin = new Padding(0, 0, 0, 4);
            btnPastePrivateKey.MinimumSize = new Size(120, 30);
            btnPastePrivateKey.Name = "btnPastePrivateKey";
            btnPastePrivateKey.Size = new Size(120, 40);
            btnPastePrivateKey.TabIndex = 1;
            btnPastePrivateKey.Text = "粘贴私钥";
            btnPastePrivateKey.Click += btnPastePrivateKey_Click;
            // 
            // btnImportPrivateKey
            // 
            btnImportPrivateKey.AutoSize = true;
            btnImportPrivateKey.Dock = DockStyle.Top;
            btnImportPrivateKey.Location = new Point(8, 92);
            btnImportPrivateKey.Margin = new Padding(0, 0, 0, 4);
            btnImportPrivateKey.MinimumSize = new Size(120, 30);
            btnImportPrivateKey.Name = "btnImportPrivateKey";
            btnImportPrivateKey.Size = new Size(120, 40);
            btnImportPrivateKey.TabIndex = 2;
            btnImportPrivateKey.Text = "导入私钥";
            btnImportPrivateKey.Click += btnImportPrivateKey_Click;
            // 
            // btnSavePrivateKey
            // 
            btnSavePrivateKey.AutoSize = true;
            btnSavePrivateKey.Dock = DockStyle.Top;
            btnSavePrivateKey.Location = new Point(8, 136);
            btnSavePrivateKey.Margin = new Padding(0, 0, 0, 4);
            btnSavePrivateKey.MinimumSize = new Size(120, 30);
            btnSavePrivateKey.Name = "btnSavePrivateKey";
            btnSavePrivateKey.Size = new Size(120, 40);
            btnSavePrivateKey.TabIndex = 3;
            btnSavePrivateKey.Text = "保存私钥";
            btnSavePrivateKey.Click += btnSavePrivateKey_Click;
            // 
            // btnClearPrivateKey
            // 
            btnClearPrivateKey.AutoSize = true;
            btnClearPrivateKey.Dock = DockStyle.Top;
            btnClearPrivateKey.Location = new Point(8, 180);
            btnClearPrivateKey.Margin = new Padding(0);
            btnClearPrivateKey.MinimumSize = new Size(120, 30);
            btnClearPrivateKey.Name = "btnClearPrivateKey";
            btnClearPrivateKey.Size = new Size(120, 40);
            btnClearPrivateKey.TabIndex = 4;
            btnClearPrivateKey.Text = "清空私钥";
            btnClearPrivateKey.Click += btnClearPrivateKey_Click;
            // 
            // labelPublicKey
            // 
            labelPublicKey.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelPublicKey.AutoSize = true;
            labelPublicKey.Location = new Point(10, 331);
            labelPublicKey.Margin = new Padding(4, 4, 4, 2);
            labelPublicKey.Name = "labelPublicKey";
            labelPublicKey.Padding = new Padding(4, 0, 4, 0);
            labelPublicKey.Size = new Size(128, 22);
            labelPublicKey.TabIndex = 2;
            labelPublicKey.Text = "公钥 (PEM)：";
            // 
            // labelPublicActionsTitle
            // 
            labelPublicActionsTitle.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelPublicActionsTitle.AutoSize = true;
            labelPublicActionsTitle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelPublicActionsTitle.ForeColor = Color.FromArgb(0, 100, 180);
            labelPublicActionsTitle.Location = new Point(1556, 331);
            labelPublicActionsTitle.Margin = new Padding(4, 4, 4, 2);
            labelPublicActionsTitle.Name = "labelPublicActionsTitle";
            labelPublicActionsTitle.Padding = new Padding(4, 0, 4, 0);
            labelPublicActionsTitle.Size = new Size(131, 22);
            labelPublicActionsTitle.TabIndex = 0;
            labelPublicActionsTitle.Text = "🔓 公钥操作";
            // 
            // textPublicKey
            // 
            textPublicKey.Dock = DockStyle.Fill;
            textPublicKey.Location = new Point(9, 358);
            textPublicKey.Multiline = true;
            textPublicKey.Name = "textPublicKey";
            textPublicKey.ScrollBars = ScrollBars.Vertical;
            textPublicKey.Size = new Size(1540, 287);
            textPublicKey.TabIndex = 3;
            // 
            // panelPublicKeyActions
            // 
            panelPublicKeyActions.Controls.Add(btnCopyPublicKey);
            panelPublicKeyActions.Controls.Add(btnPastePublicKey);
            panelPublicKeyActions.Controls.Add(btnImportPublicKey);
            panelPublicKeyActions.Controls.Add(btnSavePublicKey);
            panelPublicKeyActions.Controls.Add(btnClearPublicKey);
            panelPublicKeyActions.Dock = DockStyle.Fill;
            panelPublicKeyActions.FlowDirection = FlowDirection.TopDown;
            panelPublicKeyActions.Location = new Point(1555, 358);
            panelPublicKeyActions.Name = "panelPublicKeyActions";
            panelPublicKeyActions.Padding = new Padding(8, 4, 8, 4);
            panelPublicKeyActions.Size = new Size(184, 287);
            panelPublicKeyActions.TabIndex = 5;
            panelPublicKeyActions.WrapContents = false;
            // 
            // btnCopyPublicKey
            // 
            btnCopyPublicKey.AutoSize = true;
            btnCopyPublicKey.Dock = DockStyle.Top;
            btnCopyPublicKey.Location = new Point(8, 4);
            btnCopyPublicKey.Margin = new Padding(0, 0, 0, 4);
            btnCopyPublicKey.MinimumSize = new Size(120, 30);
            btnCopyPublicKey.Name = "btnCopyPublicKey";
            btnCopyPublicKey.Size = new Size(120, 40);
            btnCopyPublicKey.TabIndex = 0;
            btnCopyPublicKey.Text = "复制公钥";
            btnCopyPublicKey.Click += btnCopyPublicKey_Click;
            // 
            // btnPastePublicKey
            // 
            btnPastePublicKey.AutoSize = true;
            btnPastePublicKey.Dock = DockStyle.Top;
            btnPastePublicKey.Location = new Point(8, 48);
            btnPastePublicKey.Margin = new Padding(0, 0, 0, 4);
            btnPastePublicKey.MinimumSize = new Size(120, 30);
            btnPastePublicKey.Name = "btnPastePublicKey";
            btnPastePublicKey.Size = new Size(120, 40);
            btnPastePublicKey.TabIndex = 1;
            btnPastePublicKey.Text = "粘贴公钥";
            btnPastePublicKey.Click += btnPastePublicKey_Click;
            // 
            // btnImportPublicKey
            // 
            btnImportPublicKey.AutoSize = true;
            btnImportPublicKey.Dock = DockStyle.Top;
            btnImportPublicKey.Location = new Point(8, 92);
            btnImportPublicKey.Margin = new Padding(0, 0, 0, 4);
            btnImportPublicKey.MinimumSize = new Size(120, 30);
            btnImportPublicKey.Name = "btnImportPublicKey";
            btnImportPublicKey.Size = new Size(120, 40);
            btnImportPublicKey.TabIndex = 2;
            btnImportPublicKey.Text = "导入公钥";
            btnImportPublicKey.Click += btnImportPublicKey_Click;
            // 
            // btnSavePublicKey
            // 
            btnSavePublicKey.AutoSize = true;
            btnSavePublicKey.Dock = DockStyle.Top;
            btnSavePublicKey.Location = new Point(8, 136);
            btnSavePublicKey.Margin = new Padding(0, 0, 0, 4);
            btnSavePublicKey.MinimumSize = new Size(120, 30);
            btnSavePublicKey.Name = "btnSavePublicKey";
            btnSavePublicKey.Size = new Size(120, 40);
            btnSavePublicKey.TabIndex = 3;
            btnSavePublicKey.Text = "保存公钥";
            btnSavePublicKey.Click += btnSavePublicKey_Click;
            // 
            // btnClearPublicKey
            // 
            btnClearPublicKey.AutoSize = true;
            btnClearPublicKey.Dock = DockStyle.Top;
            btnClearPublicKey.Location = new Point(8, 180);
            btnClearPublicKey.Margin = new Padding(0);
            btnClearPublicKey.MinimumSize = new Size(120, 30);
            btnClearPublicKey.Name = "btnClearPublicKey";
            btnClearPublicKey.Size = new Size(120, 40);
            btnClearPublicKey.TabIndex = 4;
            btnClearPublicKey.Text = "清空公钥";
            btnClearPublicKey.Click += btnClearPublicKey_Click;
            // 
            // groupActionButtons
            // 
            groupActionButtons.Controls.Add(tableActionButtons);
            groupActionButtons.Dock = DockStyle.Fill;
            groupActionButtons.Location = new Point(1781, 11);
            groupActionButtons.Name = "groupActionButtons";
            groupActionButtons.Padding = new Padding(8);
            groupActionButtons.Size = new Size(1495, 693);
            groupActionButtons.TabIndex = 1;
            groupActionButtons.TabStop = false;
            groupActionButtons.Text = "操作按钮";
            // 
            // tableActionButtons
            // 
            tableActionButtons.ColumnCount = 1;
            tableActionButtons.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableActionButtons.Controls.Add(panelButtonArea, 0, 0);
            tableActionButtons.Location = new Point(8, 31);
            tableActionButtons.Name = "tableActionButtons";
            tableActionButtons.RowCount = 1;
            tableActionButtons.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableActionButtons.Size = new Size(1288, 654);
            tableActionButtons.TabIndex = 0;
            // 
            // panelButtonArea
            // 
            panelButtonArea.Controls.Add(tableButtonArea);
            panelButtonArea.Dock = DockStyle.Fill;
            panelButtonArea.Location = new Point(3, 3);
            panelButtonArea.Name = "panelButtonArea";
            panelButtonArea.Size = new Size(1282, 648);
            panelButtonArea.TabIndex = 1;
            // 
            // tableButtonArea
            // 
            tableButtonArea.ColumnCount = 1;
            tableButtonArea.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableButtonArea.Controls.Add(panelRightScroll, 0, 0);
            tableButtonArea.Dock = DockStyle.Fill;
            tableButtonArea.Location = new Point(0, 0);
            tableButtonArea.Name = "tableButtonArea";
            tableButtonArea.RowCount = 1;
            tableButtonArea.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableButtonArea.Size = new Size(1282, 648);
            tableButtonArea.TabIndex = 0;
            // 
            // panelRightScroll
            // 
            panelRightScroll.AutoScroll = true;
            panelRightScroll.Controls.Add(tableRightActions);
            panelRightScroll.Dock = DockStyle.Fill;
            panelRightScroll.Location = new Point(3, 3);
            panelRightScroll.Name = "panelRightScroll";
            panelRightScroll.Padding = new Padding(6);
            panelRightScroll.Size = new Size(1276, 642);
            panelRightScroll.TabIndex = 1;
            // 
            // tableRightActions
            // 
            tableRightActions.ColumnCount = 2;
            tableRightActions.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 180F));
            tableRightActions.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableRightActions.Controls.Add(panelKeyControlsContainer, 0, 0);
            tableRightActions.Controls.Add(panelFormatRow, 1, 0);
            tableRightActions.Controls.Add(panelKeyTypeRow, 1, 1);
            tableRightActions.Controls.Add(panelCurveContainer, 1, 2);
            tableRightActions.Dock = DockStyle.Top;
            tableRightActions.Location = new Point(6, 6);
            tableRightActions.Name = "tableRightActions";
            tableRightActions.RowCount = 3;
            tableRightActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tableRightActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 58F));
            tableRightActions.RowStyles.Add(new RowStyle(SizeType.Absolute, 73F));
            tableRightActions.Size = new Size(1264, 633);
            tableRightActions.TabIndex = 0;
            // 
            // panelKeyControlsContainer
            // 
            panelKeyControlsContainer.Controls.Add(panelKeyControls);
            panelKeyControlsContainer.Dock = DockStyle.Fill;
            panelKeyControlsContainer.Location = new Point(3, 3);
            panelKeyControlsContainer.Name = "panelKeyControlsContainer";
            panelKeyControlsContainer.Padding = new Padding(6, 4, 6, 8);
            tableRightActions.SetRowSpan(panelKeyControlsContainer, 3);
            panelKeyControlsContainer.Size = new Size(174, 627);
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
            panelKeyControls.Location = new Point(6, 4);
            panelKeyControls.Name = "panelKeyControls";
            panelKeyControls.Padding = new Padding(0, 4, 0, 0);
            panelKeyControls.Size = new Size(162, 615);
            panelKeyControls.TabIndex = 1;
            panelKeyControls.WrapContents = false;
            // 
            // btnGenerateKeyPair
            // 
            btnGenerateKeyPair.Location = new Point(0, 9);
            btnGenerateKeyPair.Margin = new Padding(0, 5, 6, 4);
            btnGenerateKeyPair.Name = "btnGenerateKeyPair";
            btnGenerateKeyPair.Size = new Size(150, 40);
            btnGenerateKeyPair.TabIndex = 0;
            btnGenerateKeyPair.Text = "生成密钥对";
            btnGenerateKeyPair.Click += btnGenerateKeyPair_Click;
            // 
            // btnValidateKeyPair
            // 
            btnValidateKeyPair.Location = new Point(0, 63);
            btnValidateKeyPair.Margin = new Padding(0, 10, 6, 4);
            btnValidateKeyPair.Name = "btnValidateKeyPair";
            btnValidateKeyPair.Size = new Size(150, 40);
            btnValidateKeyPair.TabIndex = 1;
            btnValidateKeyPair.Text = "验证密钥对";
            btnValidateKeyPair.Click += btnValidateKeyPair_Click;
            // 
            // btnGetPublicKeyFromPrivate
            // 
            btnGetPublicKeyFromPrivate.Location = new Point(0, 117);
            btnGetPublicKeyFromPrivate.Margin = new Padding(0, 10, 6, 4);
            btnGetPublicKeyFromPrivate.Name = "btnGetPublicKeyFromPrivate";
            btnGetPublicKeyFromPrivate.Size = new Size(150, 40);
            btnGetPublicKeyFromPrivate.TabIndex = 2;
            btnGetPublicKeyFromPrivate.Text = "从私钥提取公钥";
            btnGetPublicKeyFromPrivate.Click += btnGetPublicKeyFromPrivate_Click;
            // 
            // btnGetCurveType
            // 
            btnGetCurveType.Location = new Point(0, 171);
            btnGetCurveType.Margin = new Padding(0, 10, 6, 4);
            btnGetCurveType.Name = "btnGetCurveType";
            btnGetCurveType.Size = new Size(150, 40);
            btnGetCurveType.TabIndex = 3;
            btnGetCurveType.Text = "获取私钥曲线类型";
            btnGetCurveType.Click += btnGetCurveType_Click;
            // 
            // btnClearAll
            // 
            btnClearAll.Location = new Point(0, 225);
            btnClearAll.Margin = new Padding(0, 10, 6, 4);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.Size = new Size(150, 40);
            btnClearAll.TabIndex = 4;
            btnClearAll.Text = "清空全部";
            btnClearAll.Click += btnClearAll_Click;
            // 
            // panelFormatRow
            // 
            panelFormatRow.AutoSize = true;
            panelFormatRow.Controls.Add(labelInputFormat);
            panelFormatRow.Controls.Add(comboInputFormat);
            panelFormatRow.Controls.Add(labelOutputFormat);
            panelFormatRow.Controls.Add(comboOutputFormat);
            panelFormatRow.Dock = DockStyle.Fill;
            panelFormatRow.Location = new Point(183, 3);
            panelFormatRow.Name = "panelFormatRow";
            panelFormatRow.Padding = new Padding(6, 6, 6, 8);
            panelFormatRow.Size = new Size(1078, 52);
            panelFormatRow.TabIndex = 0;
            // 
            // labelInputFormat
            // 
            labelInputFormat.AutoSize = true;
            labelInputFormat.Location = new Point(10, 10);
            labelInputFormat.Margin = new Padding(4, 4, 2, 4);
            labelInputFormat.Name = "labelInputFormat";
            labelInputFormat.Size = new Size(100, 24);
            labelInputFormat.TabIndex = 0;
            labelInputFormat.Text = "输入格式：";
            // 
            // comboInputFormat
            // 
            comboInputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboInputFormat.FormattingEnabled = true;
            comboInputFormat.Items.AddRange(new object[] { "PEM", "Base64", "Hex" });
            comboInputFormat.Location = new Point(112, 9);
            comboInputFormat.Margin = new Padding(0, 3, 8, 3);
            comboInputFormat.Name = "comboInputFormat";
            comboInputFormat.Size = new Size(90, 32);
            comboInputFormat.TabIndex = 1;
            // 
            // labelOutputFormat
            // 
            labelOutputFormat.AutoSize = true;
            labelOutputFormat.Location = new Point(218, 10);
            labelOutputFormat.Margin = new Padding(8, 4, 2, 4);
            labelOutputFormat.Name = "labelOutputFormat";
            labelOutputFormat.Size = new Size(100, 24);
            labelOutputFormat.TabIndex = 2;
            labelOutputFormat.Text = "输出格式：";
            // 
            // comboOutputFormat
            // 
            comboOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboOutputFormat.FormattingEnabled = true;
            comboOutputFormat.Items.AddRange(new object[] { "PEM", "Base64", "Hex" });
            comboOutputFormat.Location = new Point(320, 10);
            comboOutputFormat.Margin = new Padding(0, 4, 8, 3);
            comboOutputFormat.Name = "comboOutputFormat";
            comboOutputFormat.Size = new Size(90, 32);
            comboOutputFormat.TabIndex = 3;
            comboOutputFormat.SelectedIndexChanged += comboOutputFormat_SelectedIndexChanged;
            // 
            // panelKeyTypeRow
            // 
            panelKeyTypeRow.AutoSize = true;
            panelKeyTypeRow.Controls.Add(labelKeyType);
            panelKeyTypeRow.Controls.Add(radioPanel);
            panelKeyTypeRow.Controls.Add(btnConvertKey);
            panelKeyTypeRow.Dock = DockStyle.Fill;
            panelKeyTypeRow.Location = new Point(183, 61);
            panelKeyTypeRow.Name = "panelKeyTypeRow";
            panelKeyTypeRow.Padding = new Padding(6, 6, 6, 8);
            panelKeyTypeRow.Size = new Size(1078, 52);
            panelKeyTypeRow.TabIndex = 1;
            // 
            // labelKeyType
            // 
            labelKeyType.AutoSize = true;
            labelKeyType.Location = new Point(14, 10);
            labelKeyType.Margin = new Padding(8, 4, 2, 4);
            labelKeyType.Name = "labelKeyType";
            labelKeyType.Size = new Size(100, 24);
            labelKeyType.TabIndex = 4;
            labelKeyType.Text = "密钥类型：";
            // 
            // radioPanel
            // 
            radioPanel.AutoSize = true;
            radioPanel.Controls.Add(radioPrivateKey);
            radioPanel.Controls.Add(radioPublicKey);
            radioPanel.Location = new Point(116, 9);
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
            btnConvertKey.Location = new Point(293, 9);
            btnConvertKey.Margin = new Padding(12, 3, 4, 3);
            btnConvertKey.MinimumSize = new Size(80, 26);
            btnConvertKey.Name = "btnConvertKey";
            btnConvertKey.Size = new Size(80, 34);
            btnConvertKey.TabIndex = 6;
            btnConvertKey.Text = "转换";
            btnConvertKey.Click += btnConvertKey_Click;
            // 
            // panelCurveContainer
            // 
            panelCurveContainer.AutoSize = true;
            panelCurveContainer.Controls.Add(labelCurveHeader);
            panelCurveContainer.Controls.Add(panelCurveRow);
            panelCurveContainer.Dock = DockStyle.Top;
            panelCurveContainer.Location = new Point(183, 119);
            panelCurveContainer.Name = "panelCurveContainer";
            panelCurveContainer.Padding = new Padding(6, 4, 6, 8);
            panelCurveContainer.Size = new Size(1078, 79);
            panelCurveContainer.TabIndex = 2;
            // 
            // labelCurveHeader
            // 
            labelCurveHeader.AutoSize = true;
            labelCurveHeader.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelCurveHeader.ForeColor = Color.FromArgb(80, 80, 80);
            labelCurveHeader.Location = new Point(6, 6);
            labelCurveHeader.Margin = new Padding(4, 4, 4, 6);
            labelCurveHeader.Name = "labelCurveHeader";
            labelCurveHeader.Size = new Size(123, 25);
            labelCurveHeader.TabIndex = 0;
            labelCurveHeader.Text = "📐 曲线选择";
            // 
            // panelCurveRow
            // 
            panelCurveRow.Controls.Add(labelCurve);
            panelCurveRow.Controls.Add(comboCategory);
            panelCurveRow.Controls.Add(lblArrow);
            panelCurveRow.Controls.Add(comboCurve);
            panelCurveRow.Location = new Point(6, 30);
            panelCurveRow.Name = "panelCurveRow";
            panelCurveRow.Padding = new Padding(0, 2, 0, 2);
            panelCurveRow.Size = new Size(600, 38);
            panelCurveRow.TabIndex = 1;
            panelCurveRow.WrapContents = false;
            // 
            // labelCurve
            // 
            labelCurve.AutoSize = true;
            labelCurve.Location = new Point(4, 6);
            labelCurve.Margin = new Padding(4, 4, 2, 4);
            labelCurve.Name = "labelCurve";
            labelCurve.Size = new Size(64, 24);
            labelCurve.TabIndex = 0;
            labelCurve.Text = "曲线：";
            // 
            // comboCategory
            // 
            comboCategory.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new Point(70, 5);
            comboCategory.Margin = new Padding(0, 3, 4, 3);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new Size(120, 32);
            comboCategory.TabIndex = 1;
            comboCategory.SelectedIndexChanged += ComboCategory_SelectedIndexChanged;
            // 
            // lblArrow
            // 
            lblArrow.AutoSize = true;
            lblArrow.Location = new Point(198, 6);
            lblArrow.Margin = new Padding(4, 4, 4, 0);
            lblArrow.Name = "lblArrow";
            lblArrow.Padding = new Padding(4, 6, 4, 0);
            lblArrow.Size = new Size(36, 30);
            lblArrow.TabIndex = 2;
            lblArrow.Text = "→";
            // 
            // comboCurve
            // 
            comboCurve.DisplayMember = "Value";
            comboCurve.DropDownStyle = ComboBoxStyle.DropDownList;
            comboCurve.FormattingEnabled = true;
            comboCurve.Location = new Point(238, 5);
            comboCurve.Margin = new Padding(0, 3, 4, 3);
            comboCurve.Name = "comboCurve";
            comboCurve.Size = new Size(240, 32);
            comboCurve.TabIndex = 3;
            comboCurve.ValueMember = "Key";
            // 
            // splitSignEncrypt
            // 
            mainTableLayout.SetColumnSpan(splitSignEncrypt, 2);
            splitSignEncrypt.Dock = DockStyle.Fill;
            splitSignEncrypt.Location = new Point(11, 710);
            splitSignEncrypt.Name = "splitSignEncrypt";
            // 
            // splitSignEncrypt.Panel1
            // 
            splitSignEncrypt.Panel1.Controls.Add(groupSign);
            // 
            // splitSignEncrypt.Panel2
            // 
            splitSignEncrypt.Panel2.Controls.Add(groupEncrypt);
            splitSignEncrypt.Size = new Size(3265, 460);
            splitSignEncrypt.SplitterDistance = 1766;
            splitSignEncrypt.TabIndex = 2;
            // 
            // groupSign
            // 
            groupSign.Controls.Add(tableLayoutSign);
            groupSign.Dock = DockStyle.Fill;
            groupSign.Location = new Point(0, 0);
            groupSign.Name = "groupSign";
            groupSign.Padding = new Padding(8);
            groupSign.Size = new Size(1766, 460);
            groupSign.TabIndex = 0;
            groupSign.TabStop = false;
            groupSign.Text = "签名与验签";
            // 
            // tableLayoutSign
            // 
            tableLayoutSign.ColumnCount = 2;
            tableLayoutSign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 89.0553F));
            tableLayoutSign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 10.9447F));
            tableLayoutSign.Controls.Add(labelPlainData, 0, 0);
            tableLayoutSign.Controls.Add(panelSignActions, 1, 0);
            tableLayoutSign.Controls.Add(textPlainData, 0, 1);
            tableLayoutSign.Controls.Add(labelSignature, 0, 2);
            tableLayoutSign.Controls.Add(textSignature, 0, 3);
            tableLayoutSign.Dock = DockStyle.Fill;
            tableLayoutSign.Location = new Point(8, 31);
            tableLayoutSign.Name = "tableLayoutSign";
            tableLayoutSign.RowCount = 4;
            tableLayoutSign.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutSign.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutSign.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
            tableLayoutSign.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutSign.Size = new Size(1750, 421);
            tableLayoutSign.TabIndex = 0;
            // 
            // labelPlainData
            // 
            labelPlainData.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelPlainData.AutoSize = true;
            labelPlainData.Location = new Point(4, 4);
            labelPlainData.Margin = new Padding(4, 4, 4, 2);
            labelPlainData.Name = "labelPlainData";
            labelPlainData.Padding = new Padding(4, 0, 4, 0);
            labelPlainData.Size = new Size(108, 22);
            labelPlainData.TabIndex = 0;
            labelPlainData.Text = "原始数据：";
            // 
            // panelSignActions
            // 
            panelSignActions.Controls.Add(labelHashAlgorithm);
            panelSignActions.Controls.Add(comboHashAlgorithm);
            panelSignActions.Controls.Add(labelSignatureFormat);
            panelSignActions.Controls.Add(comboSignatureFormat);
            panelSignActions.Controls.Add(btnSign);
            panelSignActions.Controls.Add(btnVerify);
            panelSignActions.Controls.Add(btnCopySignature);
            panelSignActions.Dock = DockStyle.Fill;
            panelSignActions.FlowDirection = FlowDirection.TopDown;
            panelSignActions.Location = new Point(1561, 3);
            panelSignActions.Name = "panelSignActions";
            panelSignActions.Padding = new Padding(8, 4, 8, 4);
            tableLayoutSign.SetRowSpan(panelSignActions, 4);
            panelSignActions.Size = new Size(186, 415);
            panelSignActions.TabIndex = 1;
            panelSignActions.WrapContents = false;
            // 
            // labelHashAlgorithm
            // 
            labelHashAlgorithm.AutoSize = true;
            labelHashAlgorithm.Dock = DockStyle.Top;
            labelHashAlgorithm.Location = new Point(8, 4);
            labelHashAlgorithm.Margin = new Padding(0, 0, 0, 2);
            labelHashAlgorithm.Name = "labelHashAlgorithm";
            labelHashAlgorithm.Size = new Size(120, 24);
            labelHashAlgorithm.TabIndex = 0;
            labelHashAlgorithm.Text = "Hash算法：";
            // 
            // comboHashAlgorithm
            // 
            comboHashAlgorithm.Dock = DockStyle.Top;
            comboHashAlgorithm.DropDownStyle = ComboBoxStyle.DropDownList;
            comboHashAlgorithm.FormattingEnabled = true;
            comboHashAlgorithm.Items.AddRange(new object[] { "SHA256", "SHA384", "SHA512" });
            comboHashAlgorithm.Location = new Point(8, 30);
            comboHashAlgorithm.Margin = new Padding(0, 0, 0, 6);
            comboHashAlgorithm.Name = "comboHashAlgorithm";
            comboHashAlgorithm.Size = new Size(120, 32);
            comboHashAlgorithm.TabIndex = 1;
            // 
            // labelSignatureFormat
            // 
            labelSignatureFormat.AutoSize = true;
            labelSignatureFormat.Dock = DockStyle.Top;
            labelSignatureFormat.Location = new Point(8, 72);
            labelSignatureFormat.Margin = new Padding(0, 4, 0, 2);
            labelSignatureFormat.Name = "labelSignatureFormat";
            labelSignatureFormat.Size = new Size(120, 24);
            labelSignatureFormat.TabIndex = 2;
            labelSignatureFormat.Text = "签名格式：";
            // 
            // comboSignatureFormat
            // 
            comboSignatureFormat.Dock = DockStyle.Top;
            comboSignatureFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSignatureFormat.FormattingEnabled = true;
            comboSignatureFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboSignatureFormat.Location = new Point(8, 98);
            comboSignatureFormat.Margin = new Padding(0, 0, 0, 8);
            comboSignatureFormat.Name = "comboSignatureFormat";
            comboSignatureFormat.Size = new Size(120, 32);
            comboSignatureFormat.TabIndex = 3;
            // 
            // btnSign
            // 
            btnSign.AutoSize = true;
            btnSign.Dock = DockStyle.Top;
            btnSign.Location = new Point(8, 138);
            btnSign.Margin = new Padding(0, 0, 0, 4);
            btnSign.MinimumSize = new Size(120, 30);
            btnSign.Name = "btnSign";
            btnSign.Size = new Size(120, 40);
            btnSign.TabIndex = 4;
            btnSign.Text = "签名";
            btnSign.Click += btnSign_Click;
            // 
            // btnVerify
            // 
            btnVerify.AutoSize = true;
            btnVerify.Dock = DockStyle.Top;
            btnVerify.Location = new Point(8, 182);
            btnVerify.Margin = new Padding(0, 0, 0, 4);
            btnVerify.MinimumSize = new Size(120, 30);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new Size(120, 40);
            btnVerify.TabIndex = 5;
            btnVerify.Text = "验签";
            btnVerify.Click += btnVerify_Click;
            // 
            // btnCopySignature
            // 
            btnCopySignature.AutoSize = true;
            btnCopySignature.Dock = DockStyle.Top;
            btnCopySignature.Location = new Point(8, 226);
            btnCopySignature.Margin = new Padding(0);
            btnCopySignature.MinimumSize = new Size(120, 30);
            btnCopySignature.Name = "btnCopySignature";
            btnCopySignature.Size = new Size(120, 40);
            btnCopySignature.TabIndex = 6;
            btnCopySignature.Text = "复制签名";
            btnCopySignature.Click += btnCopySignature_Click;
            // 
            // textPlainData
            // 
            textPlainData.Dock = DockStyle.Fill;
            textPlainData.Location = new Point(3, 31);
            textPlainData.Multiline = true;
            textPlainData.Name = "textPlainData";
            textPlainData.ScrollBars = ScrollBars.Vertical;
            textPlainData.Size = new Size(1552, 176);
            textPlainData.TabIndex = 2;
            // 
            // labelSignature
            // 
            labelSignature.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            labelSignature.AutoSize = true;
            labelSignature.Location = new Point(4, 214);
            labelSignature.Margin = new Padding(4, 4, 4, 2);
            labelSignature.Name = "labelSignature";
            labelSignature.Padding = new Padding(4, 0, 4, 0);
            labelSignature.Size = new Size(72, 22);
            labelSignature.TabIndex = 3;
            labelSignature.Text = "签名：";
            // 
            // textSignature
            // 
            textSignature.Dock = DockStyle.Fill;
            textSignature.Location = new Point(3, 241);
            textSignature.Multiline = true;
            textSignature.Name = "textSignature";
            textSignature.ScrollBars = ScrollBars.Vertical;
            textSignature.Size = new Size(1552, 177);
            textSignature.TabIndex = 4;
            // 
            // groupEncrypt
            // 
            groupEncrypt.Controls.Add(tableLayoutEncrypt);
            groupEncrypt.Dock = DockStyle.Fill;
            groupEncrypt.Location = new Point(0, 0);
            groupEncrypt.Name = "groupEncrypt";
            groupEncrypt.Padding = new Padding(8);
            groupEncrypt.Size = new Size(1495, 460);
            groupEncrypt.TabIndex = 0;
            groupEncrypt.TabStop = false;
            groupEncrypt.Text = "加密与解密";
            // 
            // tableLayoutEncrypt
            // 
            tableLayoutEncrypt.ColumnCount = 2;
            tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 75F));
            tableLayoutEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 25F));
            tableLayoutEncrypt.Controls.Add(labelEncMode, 0, 0);
            tableLayoutEncrypt.Controls.Add(comboEncMode, 0, 0);
            tableLayoutEncrypt.Controls.Add(labelEncInputFormat, 0, 1);
            tableLayoutEncrypt.Controls.Add(comboEncInputFormat, 0, 1);
            tableLayoutEncrypt.Controls.Add(labelEncOutputFormat, 0, 1);
            tableLayoutEncrypt.Controls.Add(comboEncOutputFormat, 0, 1);
            tableLayoutEncrypt.Controls.Add(labelEncKey, 0, 2);
            tableLayoutEncrypt.Controls.Add(textEncKey, 0, 2);
            tableLayoutEncrypt.Controls.Add(labelEncIV, 0, 3);
            tableLayoutEncrypt.Controls.Add(textEncIV, 0, 3);
            tableLayoutEncrypt.Controls.Add(labelEncInput, 0, 4);
            tableLayoutEncrypt.Controls.Add(textEncInput, 0, 5);
            tableLayoutEncrypt.Controls.Add(textEncOutput, 0, 7);
            tableLayoutEncrypt.Controls.Add(panelEncBtns, 0, 8);
            tableLayoutEncrypt.Controls.Add(groupEncFile, 0, 9);
            tableLayoutEncrypt.Controls.Add(labelEncResult, 1, 0);
            tableLayoutEncrypt.Controls.Add(labelEncOutputLabel, 0, 6);
            tableLayoutEncrypt.Dock = DockStyle.Fill;
            tableLayoutEncrypt.Location = new Point(8, 31);
            tableLayoutEncrypt.Name = "tableLayoutEncrypt";
            tableLayoutEncrypt.RowCount = 10;
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 22F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 44F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 50F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 20F));
            tableLayoutEncrypt.Size = new Size(1479, 421);
            tableLayoutEncrypt.TabIndex = 0;
            // 
            // labelEncMode
            // 
            labelEncMode.AutoSize = true;
            labelEncMode.Location = new Point(1113, 4);
            labelEncMode.Margin = new Padding(4, 4, 2, 4);
            labelEncMode.Name = "labelEncMode";
            labelEncMode.Size = new Size(100, 24);
            labelEncMode.TabIndex = 1;
            labelEncMode.Text = "加密模式：";
            // 
            // comboEncMode
            // 
            comboEncMode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncMode.FormattingEnabled = true;
            comboEncMode.Items.AddRange(new object[] { "ECIES (ECDH+AES-GCM)", "AES-256-GCM", "AES-256-CBC", "ChaCha20-Poly1305" });
            comboEncMode.Location = new Point(0, 3);
            comboEncMode.Margin = new Padding(0, 3, 4, 3);
            comboEncMode.Name = "comboEncMode";
            comboEncMode.Size = new Size(364, 32);
            comboEncMode.TabIndex = 2;
            // 
            // labelEncInputFormat
            // 
            labelEncInputFormat.AutoSize = true;
            labelEncInputFormat.Location = new Point(1113, 148);
            labelEncInputFormat.Margin = new Padding(4, 4, 2, 4);
            labelEncInputFormat.Name = "labelEncInputFormat";
            labelEncInputFormat.Size = new Size(100, 14);
            labelEncInputFormat.TabIndex = 3;
            labelEncInputFormat.Text = "输入格式：";
            // 
            // comboEncInputFormat
            // 
            comboEncInputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncInputFormat.FormattingEnabled = true;
            comboEncInputFormat.Items.AddRange(new object[] { "UTF-8文本", "Base64", "Hex" });
            comboEncInputFormat.Location = new Point(1109, 111);
            comboEncInputFormat.Margin = new Padding(0, 3, 4, 3);
            comboEncInputFormat.Name = "comboEncInputFormat";
            comboEncInputFormat.Size = new Size(120, 32);
            comboEncInputFormat.TabIndex = 4;
            // 
            // labelEncOutputFormat
            // 
            labelEncOutputFormat.AutoSize = true;
            labelEncOutputFormat.Location = new Point(1117, 76);
            labelEncOutputFormat.Margin = new Padding(8, 4, 2, 4);
            labelEncOutputFormat.Name = "labelEncOutputFormat";
            labelEncOutputFormat.Size = new Size(100, 24);
            labelEncOutputFormat.TabIndex = 5;
            labelEncOutputFormat.Text = "输出格式：";
            // 
            // comboEncOutputFormat
            // 
            comboEncOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboEncOutputFormat.FormattingEnabled = true;
            comboEncOutputFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboEncOutputFormat.Location = new Point(1109, 39);
            comboEncOutputFormat.Margin = new Padding(0, 3, 4, 3);
            comboEncOutputFormat.Name = "comboEncOutputFormat";
            comboEncOutputFormat.Size = new Size(120, 32);
            comboEncOutputFormat.TabIndex = 6;
            // 
            // labelEncKey
            // 
            labelEncKey.AutoSize = true;
            labelEncKey.Location = new Point(1113, 170);
            labelEncKey.Margin = new Padding(4, 4, 2, 4);
            labelEncKey.Name = "labelEncKey";
            labelEncKey.Size = new Size(279, 24);
            labelEncKey.TabIndex = 7;
            labelEncKey.Text = "对称密钥 (HEX，留空自动派生)：";
            // 
            // textEncKey
            // 
            textEncKey.Dock = DockStyle.Fill;
            textEncKey.Font = new Font("Consolas", 9F);
            textEncKey.Location = new Point(1113, 208);
            textEncKey.Margin = new Padding(4, 3, 4, 3);
            textEncKey.Name = "textEncKey";
            textEncKey.PlaceholderText = "从ECDSA私钥自动派生（HKDF-SHA256）";
            textEncKey.Size = new Size(362, 29);
            textEncKey.TabIndex = 8;
            // 
            // labelEncIV
            // 
            labelEncIV.AutoSize = true;
            labelEncIV.Location = new Point(1113, 270);
            labelEncIV.Margin = new Padding(4, 4, 2, 4);
            labelEncIV.Name = "labelEncIV";
            labelEncIV.Size = new Size(288, 24);
            labelEncIV.TabIndex = 9;
            labelEncIV.Text = "IV/Nonce (HEX，留空随机生成)：";
            // 
            // textEncIV
            // 
            textEncIV.Dock = DockStyle.Fill;
            textEncIV.Font = new Font("Consolas", 9F);
            textEncIV.Location = new Point(1113, 230);
            textEncIV.Margin = new Padding(4, 3, 4, 3);
            textEncIV.Name = "textEncIV";
            textEncIV.PlaceholderText = "加密时自动生成，解密时需填写";
            textEncIV.Size = new Size(362, 29);
            textEncIV.TabIndex = 10;
            // 
            // labelEncInput
            // 
            labelEncInput.AutoSize = true;
            labelEncInput.Location = new Point(1113, 314);
            labelEncInput.Margin = new Padding(4, 4, 2, 4);
            labelEncInput.Name = "labelEncInput";
            labelEncInput.Size = new Size(154, 24);
            labelEncInput.TabIndex = 11;
            labelEncInput.Text = "明文 / 密文输入：";
            // 
            // textEncInput
            // 
            textEncInput.Dock = DockStyle.Fill;
            textEncInput.Font = new Font("Consolas", 9F);
            textEncInput.Location = new Point(1113, 363);
            textEncInput.Margin = new Padding(4, 3, 4, 3);
            textEncInput.Multiline = true;
            textEncInput.Name = "textEncInput";
            textEncInput.ScrollBars = ScrollBars.Vertical;
            textEncInput.Size = new Size(362, 14);
            textEncInput.TabIndex = 12;
            // 
            // textEncOutput
            // 
            textEncOutput.Dock = DockStyle.Fill;
            textEncOutput.Font = new Font("Consolas", 9F);
            textEncOutput.Location = new Point(1113, 383);
            textEncOutput.Margin = new Padding(4, 3, 4, 3);
            textEncOutput.Multiline = true;
            textEncOutput.Name = "textEncOutput";
            textEncOutput.ReadOnly = true;
            textEncOutput.ScrollBars = ScrollBars.Vertical;
            textEncOutput.Size = new Size(362, 14);
            textEncOutput.TabIndex = 14;
            // 
            // panelEncBtns
            // 
            panelEncBtns.Controls.Add(btnEncrypt);
            panelEncBtns.Controls.Add(btnDecrypt);
            panelEncBtns.Controls.Add(btnEncClear);
            panelEncBtns.Controls.Add(btnEncCopy);
            panelEncBtns.Controls.Add(btnEncPaste);
            panelEncBtns.Dock = DockStyle.Fill;
            panelEncBtns.Location = new Point(3, 403);
            panelEncBtns.Name = "panelEncBtns";
            panelEncBtns.Padding = new Padding(0, 4, 0, 0);
            panelEncBtns.Size = new Size(1103, 15);
            panelEncBtns.TabIndex = 15;
            panelEncBtns.WrapContents = false;
            // 
            // btnEncrypt
            // 
            btnEncrypt.AutoSize = true;
            btnEncrypt.Location = new Point(3, 7);
            btnEncrypt.Margin = new Padding(3, 3, 4, 3);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new Size(75, 34);
            btnEncrypt.TabIndex = 0;
            btnEncrypt.Text = "加密";
            btnEncrypt.Click += btnEncrypt_Click;
            // 
            // btnDecrypt
            // 
            btnDecrypt.AutoSize = true;
            btnDecrypt.Location = new Point(88, 7);
            btnDecrypt.Margin = new Padding(6, 3, 6, 3);
            btnDecrypt.Name = "btnDecrypt";
            btnDecrypt.Size = new Size(75, 34);
            btnDecrypt.TabIndex = 1;
            btnDecrypt.Text = "解密";
            btnDecrypt.Click += btnDecrypt_Click;
            // 
            // btnEncClear
            // 
            btnEncClear.AutoSize = true;
            btnEncClear.Location = new Point(175, 7);
            btnEncClear.Margin = new Padding(6, 3, 6, 3);
            btnEncClear.Name = "btnEncClear";
            btnEncClear.Size = new Size(75, 34);
            btnEncClear.TabIndex = 2;
            btnEncClear.Text = "清空";
            btnEncClear.Click += btnEncClear_Click;
            // 
            // btnEncCopy
            // 
            btnEncCopy.AutoSize = true;
            btnEncCopy.Location = new Point(262, 7);
            btnEncCopy.Margin = new Padding(6, 3, 6, 3);
            btnEncCopy.Name = "btnEncCopy";
            btnEncCopy.Size = new Size(92, 34);
            btnEncCopy.TabIndex = 3;
            btnEncCopy.Text = "复制结果";
            btnEncCopy.Click += btnEncCopy_Click;
            // 
            // btnEncPaste
            // 
            btnEncPaste.AutoSize = true;
            btnEncPaste.Location = new Point(366, 7);
            btnEncPaste.Margin = new Padding(6, 3, 4, 3);
            btnEncPaste.Name = "btnEncPaste";
            btnEncPaste.Size = new Size(92, 34);
            btnEncPaste.TabIndex = 4;
            btnEncPaste.Text = "粘贴输入";
            btnEncPaste.Click += btnEncPaste_Click;
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
            btnEncryptFile.Click += btnEncryptFile_Click;
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
            btnDecryptFile.Click += btnDecryptFile_Click;
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
            tableLayoutEncrypt.SetRowSpan(labelEncResult, 10);
            labelEncResult.ScrollBars = ScrollBars.Vertical;
            labelEncResult.Size = new Size(364, 338);
            labelEncResult.TabIndex = 0;
            labelEncResult.Text = "加密结果:\r\n等待操作...";
            // 
            // labelEncOutputLabel
            // 
            labelEncOutputLabel.AutoSize = true;
            labelEncOutputLabel.Location = new Point(4, 384);
            labelEncOutputLabel.Margin = new Padding(4, 4, 2, 4);
            labelEncOutputLabel.Name = "labelEncOutputLabel";
            labelEncOutputLabel.Size = new Size(144, 12);
            labelEncOutputLabel.TabIndex = 13;
            labelEncOutputLabel.Text = "加密/解密结果：";
            // 
            // splitFileResult
            // 
            mainTableLayout.SetColumnSpan(splitFileResult, 2);
            splitFileResult.Dock = DockStyle.Fill;
            splitFileResult.Location = new Point(11, 1176);
            splitFileResult.Name = "splitFileResult";
            // 
            // splitFileResult.Panel1
            // 
            splitFileResult.Panel1.Controls.Add(groupFile);
            // 
            // splitFileResult.Panel2
            // 
            splitFileResult.Panel2.Controls.Add(groupRunResult);
            splitFileResult.Size = new Size(3265, 429);
            splitFileResult.SplitterDistance = 2633;
            splitFileResult.TabIndex = 3;
            // 
            // groupFile
            // 
            groupFile.Controls.Add(panelFileControls);
            groupFile.Dock = DockStyle.Fill;
            groupFile.Location = new Point(0, 0);
            groupFile.Name = "groupFile";
            groupFile.Padding = new Padding(8);
            groupFile.Size = new Size(2633, 429);
            groupFile.TabIndex = 0;
            groupFile.TabStop = false;
            groupFile.Text = "文件操作";
            // 
            // panelFileControls
            // 
            panelFileControls.Controls.Add(btnSignFile);
            panelFileControls.Controls.Add(btnVerifyFile);
            panelFileControls.Dock = DockStyle.Fill;
            panelFileControls.Location = new Point(8, 31);
            panelFileControls.Name = "panelFileControls";
            panelFileControls.Padding = new Padding(4, 2, 4, 2);
            panelFileControls.Size = new Size(2617, 390);
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
            btnSignFile.Click += btnSignFile_Click;
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
            btnVerifyFile.Click += btnVerifyFile_Click;
            // 
            // groupRunResult
            // 
            groupRunResult.Controls.Add(labelValidationResult);
            groupRunResult.Dock = DockStyle.Fill;
            groupRunResult.Location = new Point(0, 0);
            groupRunResult.Name = "groupRunResult";
            groupRunResult.Padding = new Padding(8);
            groupRunResult.Size = new Size(628, 429);
            groupRunResult.TabIndex = 0;
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
            labelValidationResult.Multiline = true;
            labelValidationResult.Name = "labelValidationResult";
            labelValidationResult.ReadOnly = true;
            labelValidationResult.ScrollBars = ScrollBars.Vertical;
            labelValidationResult.Size = new Size(612, 390);
            labelValidationResult.TabIndex = 0;
            labelValidationResult.Text = "验证结果: 未验证";
            // 
            // EcdsaTabControl
            // 
            AutoScaleMode = AutoScaleMode.None;
            Controls.Add(mainTableLayout);
            Name = "EcdsaTabControl";
            Size = new Size(3287, 1616);
            mainTableLayout.ResumeLayout(false);
            groupKey.ResumeLayout(false);
            tableLayoutKey.ResumeLayout(false);
            tableLayoutKey.PerformLayout();
            panelPrivateKeyActions.ResumeLayout(false);
            panelPrivateKeyActions.PerformLayout();
            panelPublicKeyActions.ResumeLayout(false);
            panelPublicKeyActions.PerformLayout();
            groupActionButtons.ResumeLayout(false);
            tableActionButtons.ResumeLayout(false);
            panelButtonArea.ResumeLayout(false);
            tableButtonArea.ResumeLayout(false);
            panelRightScroll.ResumeLayout(false);
            tableRightActions.ResumeLayout(false);
            tableRightActions.PerformLayout();
            panelKeyControlsContainer.ResumeLayout(false);
            panelKeyControls.ResumeLayout(false);
            panelFormatRow.ResumeLayout(false);
            panelFormatRow.PerformLayout();
            panelKeyTypeRow.ResumeLayout(false);
            panelKeyTypeRow.PerformLayout();
            radioPanel.ResumeLayout(false);
            radioPanel.PerformLayout();
            panelCurveContainer.ResumeLayout(false);
            panelCurveContainer.PerformLayout();
            panelCurveRow.ResumeLayout(false);
            panelCurveRow.PerformLayout();
            splitSignEncrypt.Panel1.ResumeLayout(false);
            splitSignEncrypt.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitSignEncrypt).EndInit();
            splitSignEncrypt.ResumeLayout(false);
            groupSign.ResumeLayout(false);
            tableLayoutSign.ResumeLayout(false);
            tableLayoutSign.PerformLayout();
            panelSignActions.ResumeLayout(false);
            panelSignActions.PerformLayout();
            groupEncrypt.ResumeLayout(false);
            tableLayoutEncrypt.ResumeLayout(false);
            tableLayoutEncrypt.PerformLayout();
            panelEncBtns.ResumeLayout(false);
            panelEncBtns.PerformLayout();
            groupEncFile.ResumeLayout(false);
            panelEncFileBtns.ResumeLayout(false);
            panelEncFileBtns.PerformLayout();
            splitFileResult.Panel1.ResumeLayout(false);
            splitFileResult.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitFileResult).EndInit();
            splitFileResult.ResumeLayout(false);
            groupFile.ResumeLayout(false);
            panelFileControls.ResumeLayout(false);
            panelFileControls.PerformLayout();
            groupRunResult.ResumeLayout(false);
            groupRunResult.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // ==========================================
        // 字段声明（全部补全，避免编译错误）
        // ==========================================
        private System.Windows.Forms.TableLayoutPanel mainTableLayout;
        private System.Windows.Forms.GroupBox groupKey;
        private System.Windows.Forms.TableLayoutPanel tableLayoutKey;
        private System.Windows.Forms.Label labelPrivateKey;
        private System.Windows.Forms.Label labelPrivateActionsTitle;
        private System.Windows.Forms.TextBox textPrivateKey;
        private System.Windows.Forms.FlowLayoutPanel panelPrivateKeyActions;
        private System.Windows.Forms.Button btnCopyPrivateKey;
        private System.Windows.Forms.Button btnPastePrivateKey;
        private System.Windows.Forms.Button btnImportPrivateKey;
        private System.Windows.Forms.Button btnSavePrivateKey;
        private System.Windows.Forms.Button btnClearPrivateKey;
        private System.Windows.Forms.Label labelPublicKey;
        private System.Windows.Forms.Label labelPublicActionsTitle;
        private System.Windows.Forms.TextBox textPublicKey;
        private System.Windows.Forms.FlowLayoutPanel panelPublicKeyActions;
        private System.Windows.Forms.Button btnCopyPublicKey;
        private System.Windows.Forms.Button btnPastePublicKey;
        private System.Windows.Forms.Button btnImportPublicKey;
        private System.Windows.Forms.Button btnSavePublicKey;
        private System.Windows.Forms.Button btnClearPublicKey;
        private System.Windows.Forms.GroupBox groupActionButtons;
        private System.Windows.Forms.TableLayoutPanel tableActionButtons;
        private System.Windows.Forms.Panel panelButtonArea;
        private System.Windows.Forms.TableLayoutPanel tableButtonArea;
        private System.Windows.Forms.Panel panelRightScroll;
        private System.Windows.Forms.TableLayoutPanel tableRightActions;
        private System.Windows.Forms.FlowLayoutPanel panelFormatRow;
        private System.Windows.Forms.FlowLayoutPanel panelKeyTypeRow;
        private System.Windows.Forms.Label labelInputFormat;
        private System.Windows.Forms.ComboBox comboInputFormat;
        private System.Windows.Forms.Label labelOutputFormat;
        private System.Windows.Forms.ComboBox comboOutputFormat;
        private System.Windows.Forms.Label labelKeyType;
        private System.Windows.Forms.FlowLayoutPanel radioPanel;
        private System.Windows.Forms.RadioButton radioPrivateKey;
        private System.Windows.Forms.RadioButton radioPublicKey;
        private System.Windows.Forms.Button btnConvertKey;
        private System.Windows.Forms.Panel panelKeyControlsContainer;
        private System.Windows.Forms.FlowLayoutPanel panelKeyControls;
        private System.Windows.Forms.Button btnGenerateKeyPair;
        private System.Windows.Forms.Button btnValidateKeyPair;
        private System.Windows.Forms.Button btnGetPublicKeyFromPrivate;
        private System.Windows.Forms.Button btnGetCurveType;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.Panel panelCurveContainer;
        private System.Windows.Forms.Label labelCurveHeader;
        private System.Windows.Forms.FlowLayoutPanel panelCurveRow;
        private System.Windows.Forms.Label labelCurve;
        private System.Windows.Forms.ComboBox comboCategory;
        private System.Windows.Forms.Label lblArrow;
        private System.Windows.Forms.ComboBox comboCurve;
        private System.Windows.Forms.SplitContainer splitSignEncrypt;
        private System.Windows.Forms.GroupBox groupSign;
        private System.Windows.Forms.TableLayoutPanel tableLayoutSign;
        private System.Windows.Forms.Label labelPlainData;
        private System.Windows.Forms.TextBox textPlainData;
        private System.Windows.Forms.Label labelSignature;
        private System.Windows.Forms.TextBox textSignature;
        private System.Windows.Forms.FlowLayoutPanel panelSignActions;
        private System.Windows.Forms.Label labelHashAlgorithm;
        private System.Windows.Forms.ComboBox comboHashAlgorithm;
        private System.Windows.Forms.Label labelSignatureFormat;
        private System.Windows.Forms.ComboBox comboSignatureFormat;
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnCopySignature;
        private System.Windows.Forms.GroupBox groupEncrypt;
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
        private System.Windows.Forms.Label labelEncInput;
        private System.Windows.Forms.TextBox textEncInput;
        private System.Windows.Forms.Label labelEncOutputLabel;
        private System.Windows.Forms.TextBox textEncOutput;
        private System.Windows.Forms.FlowLayoutPanel panelEncBtns;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnEncClear;
        private System.Windows.Forms.Button btnEncCopy;
        private System.Windows.Forms.Button btnEncPaste;
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
        private System.Windows.Forms.GroupBox groupRunResult;
        private System.Windows.Forms.TextBox labelValidationResult;
    }
}