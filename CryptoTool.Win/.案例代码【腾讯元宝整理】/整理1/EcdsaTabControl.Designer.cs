#nullable disable

using CryptoTool.Algorithm.Algorithms.ECDSA;
using CryptoTool.Win.Enums;
using CryptoTool.Win.Helpers;

namespace CryptoTool.Win
{
    /// <summary>
    /// ECDSA Tab 页 - 设计器代码
    /// 
    /// 整体布局:
    ///   左栏 50%: 密钥管理(groupKey) | 视图内容(panelViewContent)
    ///   右栏 50%: 密钥操作(panelActionButtons) | 视图内容(panelViewContent)
    /// 
    /// 视图切换:
    ///   panelViewBar -> 签名视图 / 加解密视图 / 文件视图 / ECDH视图
    /// </summary>
    partial class EcdsaTabControl
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        #region 字段声明
        // ---- 主布局 ----
        private System.Windows.Forms.TableLayoutPanel mainTableLayout;

        // ---- 密钥管理区 ----
        private System.Windows.Forms.GroupBox groupKey;
        private System.Windows.Forms.TableLayoutPanel tableLayoutKey;
        private System.Windows.Forms.Panel panelPrivateKeyBox;
        private System.Windows.Forms.Panel panelPublicKeyBox;
        private System.Windows.Forms.Label labelPrivateKey;
        private System.Windows.Forms.Label labelPublicKey;
        private System.Windows.Forms.TextBox textPrivateKey;
        private System.Windows.Forms.TextBox textPublicKey;
        private System.Windows.Forms.TableLayoutPanel panelPrivateKeyActions;
        private System.Windows.Forms.TableLayoutPanel panelPublicKeyActions;
        private System.Windows.Forms.Label labelPrivateActionsTitle;
        private System.Windows.Forms.Label labelPublicActionsTitle;
        private System.Windows.Forms.Button btnCopyPrivateKey;
        private System.Windows.Forms.Button btnPastePrivateKey;
        private System.Windows.Forms.Button btnImportPrivateKey;
        private System.Windows.Forms.Button btnSavePrivateKey;
        private System.Windows.Forms.Button btnClearPrivateKey;
        private System.Windows.Forms.Button btnCopyPublicKey;
        private System.Windows.Forms.Button btnPastePublicKey;
        private System.Windows.Forms.Button btnImportPublicKey;
        private System.Windows.Forms.Button btnSavePublicKey;
        private System.Windows.Forms.Button btnClearPublicKey;

        // ---- 右侧操作面板 ----
        private System.Windows.Forms.Panel panelActionButtons;
        private System.Windows.Forms.TableLayoutPanel tableActionButtons;
        private System.Windows.Forms.GroupBox groupKeyActions;
        private System.Windows.Forms.TableLayoutPanel tableKeyActions;
        private System.Windows.Forms.Panel panelButtonArea;
        private System.Windows.Forms.TableLayoutPanel tableButtonArea;
        private System.Windows.Forms.TableLayoutPanel tableRightActions;
        private System.Windows.Forms.Panel panelKeyControlsContainer;
        private System.Windows.Forms.FlowLayoutPanel panelKeyControls;
        private System.Windows.Forms.Button btnGenerateKeyPair;
        private System.Windows.Forms.Button btnValidateKeyPair;
        private System.Windows.Forms.Button btnGetPublicKeyFromPrivate;
        private System.Windows.Forms.Button btnGetCurveType;
        private System.Windows.Forms.Button btnClearAll;
        private System.Windows.Forms.FlowLayoutPanel panelRightSettings;
        private System.Windows.Forms.FlowLayoutPanel panelFormatRow;
        private System.Windows.Forms.FlowLayoutPanel panelKeyTypeRow;
        private System.Windows.Forms.FlowLayoutPanel radioPanel;
        private System.Windows.Forms.Panel panelCurveContainer;
        private System.Windows.Forms.FlowLayoutPanel panelCurveRow;
        private System.Windows.Forms.Label labelOutputFormat;
        private System.Windows.Forms.ComboBox comboOutputFormat;
        private System.Windows.Forms.Label labelKeyType;
        private System.Windows.Forms.RadioButton radioPrivateKey;
        private System.Windows.Forms.RadioButton radioPublicKey;
        private System.Windows.Forms.Button btnConvertKey;
        private System.Windows.Forms.Label labelCurve;
        private System.Windows.Forms.ComboBox comboCategory;
        private System.Windows.Forms.Label lblArrow;
        private System.Windows.Forms.ComboBox comboCurve;

        // ---- 结果显示区 ----
        private System.Windows.Forms.GroupBox groupComputeResult;
        private System.Windows.Forms.RichTextBox textKeyResult;
        private System.Windows.Forms.GroupBox groupRunResult;
        private System.Windows.Forms.RichTextBox labelValidationResult;

        // ---- 视图切换栏 ----
        private System.Windows.Forms.FlowLayoutPanel panelViewBar;
        private System.Windows.Forms.Button btnViewEcdh;
        private System.Windows.Forms.Button btnViewSign;
        private System.Windows.Forms.Button btnViewEncrypt;
        private System.Windows.Forms.Button btnViewFile;

        // ---- 视图内容容器 ----
        private System.Windows.Forms.Panel panelViewContent;

        // ---- 签名验签面板 ----
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
        private System.Windows.Forms.Button btnSign;
        private System.Windows.Forms.Button btnVerify;
        private System.Windows.Forms.Button btnCopySignature;
        private System.Windows.Forms.TableLayoutPanel panelSignOptions;
        private System.Windows.Forms.Label labelHashAlgorithm;
        private System.Windows.Forms.ComboBox comboHashAlgorithm;
        private System.Windows.Forms.Label labelSignatureFormat;
        private System.Windows.Forms.ComboBox comboSignatureFormat;

        // ---- 加密解密面板 ----
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
        private System.Windows.Forms.FlowLayoutPanel panelEncBtns;
        private System.Windows.Forms.Button btnEncrypt;
        private System.Windows.Forms.Button btnDecrypt;
        private System.Windows.Forms.Button btnEncClear;
        private System.Windows.Forms.Button btnEncCopy;
        private System.Windows.Forms.Button btnEncPaste;

        // ---- 文件操作面板 ----
        private System.Windows.Forms.GroupBox groupFile;
        private System.Windows.Forms.FlowLayoutPanel panelFileControls;
        private System.Windows.Forms.Button btnSignFile;
        private System.Windows.Forms.Button btnVerifyFile;
        private System.Windows.Forms.GroupBox groupEncFile;
        private System.Windows.Forms.FlowLayoutPanel panelEncFileBtns;
        private System.Windows.Forms.Button btnEncryptFile;
        private System.Windows.Forms.Button btnDecryptFile;

        // ---- ECDH 面板 ----
        private System.Windows.Forms.Panel groupEcdh;
        private System.Windows.Forms.TextBox labelEncResult;

        // ---- 分隔器 ----
        private System.Windows.Forms.SplitContainer splitSignEncrypt;
        private System.Windows.Forms.SplitContainer splitFileResult;
        #endregion

        private void InitializeComponent()
        {
            // ===== 主布局: 2列(50/50) × 3行(45% / 60px / 55%) =====
            mainTableLayout = new System.Windows.Forms.TableLayoutPanel();
            groupKey = new System.Windows.Forms.GroupBox();
            tableLayoutKey = new System.Windows.Forms.TableLayoutPanel();
            panelPrivateKeyBox = new System.Windows.Forms.Panel();
            labelPrivateKey = new System.Windows.Forms.Label();
            textPrivateKey = new System.Windows.Forms.TextBox();
            panelPrivateKeyActions = new System.Windows.Forms.TableLayoutPanel();
            labelPrivateActionsTitle = new System.Windows.Forms.Label();
            btnCopyPrivateKey = new System.Windows.Forms.Button();
            btnPastePrivateKey = new System.Windows.Forms.Button();
            btnImportPrivateKey = new System.Windows.Forms.Button();
            btnSavePrivateKey = new System.Windows.Forms.Button();
            btnClearPrivateKey = new System.Windows.Forms.Button();
            panelPublicKeyBox = new System.Windows.Forms.Panel();
            labelPublicKey = new System.Windows.Forms.Label();
            textPublicKey = new System.Windows.Forms.TextBox();
            panelPublicKeyActions = new System.Windows.Forms.TableLayoutPanel();
            labelPublicActionsTitle = new System.Windows.Forms.Label();
            btnCopyPublicKey = new System.Windows.Forms.Button();
            btnPastePublicKey = new System.Windows.Forms.Button();
            btnImportPublicKey = new System.Windows.Forms.Button();
            btnSavePublicKey = new System.Windows.Forms.Button();
            btnClearPublicKey = new System.Windows.Forms.Button();
            panelActionButtons = new System.Windows.Forms.Panel();
            tableActionButtons = new System.Windows.Forms.TableLayoutPanel();
            groupKeyActions = new System.Windows.Forms.GroupBox();
            tableKeyActions = new System.Windows.Forms.TableLayoutPanel();
            panelButtonArea = new System.Windows.Forms.Panel();
            tableButtonArea = new System.Windows.Forms.TableLayoutPanel();
            tableRightActions = new System.Windows.Forms.TableLayoutPanel();
            panelKeyControlsContainer = new System.Windows.Forms.Panel();
            panelKeyControls = new System.Windows.Forms.FlowLayoutPanel();
            btnGenerateKeyPair = new System.Windows.Forms.Button();
            btnValidateKeyPair = new System.Windows.Forms.Button();
            btnGetPublicKeyFromPrivate = new System.Windows.Forms.Button();
            btnGetCurveType = new System.Windows.Forms.Button();
            btnClearAll = new System.Windows.Forms.Button();
            panelRightSettings = new System.Windows.Forms.FlowLayoutPanel();
            panelFormatRow = new System.Windows.Forms.FlowLayoutPanel();
            labelOutputFormat = new System.Windows.Forms.Label();
            comboOutputFormat = new System.Windows.Forms.ComboBox();
            panelKeyTypeRow = new System.Windows.Forms.FlowLayoutPanel();
            labelKeyType = new System.Windows.Forms.Label();
            radioPanel = new System.Windows.Forms.FlowLayoutPanel();
            radioPrivateKey = new System.Windows.Forms.RadioButton();
            radioPublicKey = new System.Windows.Forms.RadioButton();
            btnConvertKey = new System.Windows.Forms.Button();
            panelCurveContainer = new System.Windows.Forms.Panel();
            panelCurveRow = new System.Windows.Forms.FlowLayoutPanel();
            labelCurve = new System.Windows.Forms.Label();
            comboCategory = new System.Windows.Forms.ComboBox();
            lblArrow = new System.Windows.Forms.Label();
            comboCurve = new System.Windows.Forms.ComboBox();
            groupComputeResult = new System.Windows.Forms.GroupBox();
            textKeyResult = new System.Windows.Forms.RichTextBox();
            groupRunResult = new System.Windows.Forms.GroupBox();
            labelValidationResult = new System.Windows.Forms.RichTextBox();
            panelViewBar = new System.Windows.Forms.FlowLayoutPanel();
            btnViewEcdh = new System.Windows.Forms.Button();
            btnViewSign = new System.Windows.Forms.Button();
            btnViewEncrypt = new System.Windows.Forms.Button();
            btnViewFile = new System.Windows.Forms.Button();
            panelViewContent = new System.Windows.Forms.Panel();
            groupSign = new System.Windows.Forms.Panel();
            tableLayoutSign = new System.Windows.Forms.TableLayoutPanel();
            groupSignInput = new System.Windows.Forms.GroupBox();
            panelSignInput = new System.Windows.Forms.TableLayoutPanel();
            panelPlainDataBox = new System.Windows.Forms.Panel();
            labelPlainData = new System.Windows.Forms.Label();
            textPlainData = new System.Windows.Forms.TextBox();
            panelPlainDataActions = new System.Windows.Forms.TableLayoutPanel();
            labelPlainDataActionsTitle = new System.Windows.Forms.Label();
            btnCopyPlainData = new System.Windows.Forms.Button();
            btnPastePlainData = new System.Windows.Forms.Button();
            btnClearPlainData = new System.Windows.Forms.Button();
            panelSignatureBox = new System.Windows.Forms.Panel();
            labelSignature = new System.Windows.Forms.Label();
            textSignature = new System.Windows.Forms.TextBox();
            panelSignatureActions = new System.Windows.Forms.TableLayoutPanel();
            labelSignatureActionsTitle = new System.Windows.Forms.Label();
            btnCopySignatureData = new System.Windows.Forms.Button();
            btnPasteSignatureData = new System.Windows.Forms.Button();
            btnClearSignatureData = new System.Windows.Forms.Button();
            groupSignActions = new System.Windows.Forms.GroupBox();
            panelSignActions = new System.Windows.Forms.TableLayoutPanel();
            btnSign = new System.Windows.Forms.Button();
            btnVerify = new System.Windows.Forms.Button();
            btnCopySignature = new System.Windows.Forms.Button();
            panelSignOptions = new System.Windows.Forms.TableLayoutPanel();
            labelHashAlgorithm = new System.Windows.Forms.Label();
            comboHashAlgorithm = new System.Windows.Forms.ComboBox();
            labelSignatureFormat = new System.Windows.Forms.Label();
            comboSignatureFormat = new System.Windows.Forms.ComboBox();
            groupEncrypt = new System.Windows.Forms.Panel();
            tableLayoutEncrypt = new System.Windows.Forms.TableLayoutPanel();
            labelEncMode = new System.Windows.Forms.Label();
            comboEncMode = new System.Windows.Forms.ComboBox();
            labelEncInputFormat = new System.Windows.Forms.Label();
            comboEncInputFormat = new System.Windows.Forms.ComboBox();
            labelEncOutputFormat = new System.Windows.Forms.Label();
            comboEncOutputFormat = new System.Windows.Forms.ComboBox();
            labelEncKey = new System.Windows.Forms.Label();
            textEncKey = new System.Windows.Forms.TextBox();
            labelEncIV = new System.Windows.Forms.Label();
            textEncIV = new System.Windows.Forms.TextBox();
            labelEncBobPublic = new System.Windows.Forms.Label();
            textEncBobPublic = new System.Windows.Forms.TextBox();
            labelEncCurve = new System.Windows.Forms.Label();
            comboEncCurveCategory = new System.Windows.Forms.ComboBox();
            labelEncCurveArrow = new System.Windows.Forms.Label();
            comboEncCurve = new System.Windows.Forms.ComboBox();
            labelEncInput = new System.Windows.Forms.Label();
            textEncInput = new System.Windows.Forms.TextBox();
            textEncOutput = new System.Windows.Forms.TextBox();
            labelEncOutputLabel = new System.Windows.Forms.Label();
            panelEncBtns = new System.Windows.Forms.FlowLayoutPanel();
            btnEncrypt = new System.Windows.Forms.Button();
            btnDecrypt = new System.Windows.Forms.Button();
            btnEncClear = new System.Windows.Forms.Button();
            btnEncCopy = new System.Windows.Forms.Button();
            btnEncPaste = new System.Windows.Forms.Button();
            groupFile = new System.Windows.Forms.GroupBox();
            panelFileControls = new System.Windows.Forms.FlowLayoutPanel();
            btnSignFile = new System.Windows.Forms.Button();
            btnVerifyFile = new System.Windows.Forms.Button();
            groupEcdh = new System.Windows.Forms.Panel();
            labelEncResult = new System.Windows.Forms.TextBox();
            splitSignEncrypt = new System.Windows.Forms.SplitContainer();
            groupEncFile = new System.Windows.Forms.GroupBox();
            panelEncFileBtns = new System.Windows.Forms.FlowLayoutPanel();
            btnEncryptFile = new System.Windows.Forms.Button();
            btnDecryptFile = new System.Windows.Forms.Button();
            splitFileResult = new System.Windows.Forms.SplitContainer();

            // ===== SuspendLayout 区域 =====
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
            SuspendLayout();

            // ==================================================
            // 主布局 - 整体页面为2列：左(密钥面板) | 右(操作面板)
            // ==================================================
            mainTableLayout.ColumnCount = 2;
            mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            mainTableLayout.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            mainTableLayout.Controls.Add(groupKey, 0, 0);
            mainTableLayout.Controls.Add(panelActionButtons, 1, 0);
            mainTableLayout.Controls.Add(panelViewBar, 0, 1);
            mainTableLayout.Controls.Add(panelViewContent, 0, 2);
            mainTableLayout.Dock = System.Windows.Forms.DockStyle.Fill;
            mainTableLayout.Location = new System.Drawing.Point(0, 0);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new System.Windows.Forms.Padding(8);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 45F));
            mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 60F));
            mainTableLayout.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 55F));
            mainTableLayout.Size = new System.Drawing.Size(3287, 1616);
            mainTableLayout.TabIndex = 0;

            // ==================================================
            // 密钥管理区 - groupKey (左列行0)
            // ==================================================
            groupKey.Controls.Add(tableLayoutKey);
            groupKey.Dock = System.Windows.Forms.DockStyle.Fill;
            groupKey.Location = new System.Drawing.Point(11, 11);
            groupKey.Name = "groupKey";
            groupKey.Padding = new System.Windows.Forms.Padding(8);
            groupKey.Size = new System.Drawing.Size(1629, 687);
            groupKey.TabIndex = 0;
            groupKey.TabStop = false;
            groupKey.Text = "密钥生成";

            // ---- tableLayoutKey: 2列(PEM|按钮) × 4行(私/间隔/公/间隔) ----
            tableLayoutKey.ColumnCount = 2;
            tableLayoutKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutKey.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            tableLayoutKey.Controls.Add(panelPrivateKeyBox, 0, 0);
            tableLayoutKey.Controls.Add(panelPrivateKeyActions, 1, 0);
            tableLayoutKey.Controls.Add(panelPublicKeyBox, 0, 2);
            tableLayoutKey.Controls.Add(panelPublicKeyActions, 1, 2);
            tableLayoutKey.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutKey.Location = new System.Drawing.Point(8, 31);
            tableLayoutKey.Name = "tableLayoutKey";
            tableLayoutKey.Padding = new System.Windows.Forms.Padding(6);
            tableLayoutKey.RowCount = 4;
            tableLayoutKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            tableLayoutKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutKey.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            tableLayoutKey.Size = new System.Drawing.Size(1613, 648);
            tableLayoutKey.TabIndex = 0;

            // ---- 私钥输入面板 (列0, 跨行0~1) ----
            panelPrivateKeyBox.Controls.Add(labelPrivateKey);
            panelPrivateKeyBox.Controls.Add(textPrivateKey);
            panelPrivateKeyBox.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPrivateKeyBox.Location = new System.Drawing.Point(9, 9);
            panelPrivateKeyBox.Name = "panelPrivateKeyBox";
            tableLayoutKey.SetRowSpan(panelPrivateKeyBox, 2);
            panelPrivateKeyBox.Size = new System.Drawing.Size(1395, 312);
            panelPrivateKeyBox.TabIndex = 6;

            labelPrivateKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelPrivateKey.AutoSize = true;
            labelPrivateKey.BackColor = System.Drawing.Color.Transparent;
            labelPrivateKey.Location = new System.Drawing.Point(1263, 4);
            labelPrivateKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 2);
            labelPrivateKey.Name = "labelPrivateKey";
            labelPrivateKey.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPrivateKey.Size = new System.Drawing.Size(128, 24);
            labelPrivateKey.TabIndex = 0;
            labelPrivateKey.Text = "私钥 (PEM)：";

            textPrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            textPrivateKey.Location = new System.Drawing.Point(0, 0);
            textPrivateKey.Multiline = true;
            textPrivateKey.Name = "textPrivateKey";
            textPrivateKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textPrivateKey.Size = new System.Drawing.Size(1395, 312);
            textPrivateKey.TabIndex = 1;
            textPrivateKey.TextChanged += TextPrivateKey_TextChanged;

            // ---- 私钥操作按钮列 (列1, 跨行0~1): 6行网格 ----
            panelPrivateKeyActions.ColumnCount = 1;
            panelPrivateKeyActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelPrivateKeyActions.Controls.Add(labelPrivateActionsTitle, 0, 0);
            panelPrivateKeyActions.Controls.Add(btnCopyPrivateKey, 0, 1);
            panelPrivateKeyActions.Controls.Add(btnPastePrivateKey, 0, 2);
            panelPrivateKeyActions.Controls.Add(btnImportPrivateKey, 0, 3);
            panelPrivateKeyActions.Controls.Add(btnSavePrivateKey, 0, 4);
            panelPrivateKeyActions.Controls.Add(btnClearPrivateKey, 0, 5);
            panelPrivateKeyActions.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPrivateKeyActions.Location = new System.Drawing.Point(1410, 9);
            panelPrivateKeyActions.Name = "panelPrivateKeyActions";
            panelPrivateKeyActions.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            panelPrivateKeyActions.RowCount = 6;
            tableLayoutKey.SetRowSpan(panelPrivateKeyActions, 2);
            panelPrivateKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            panelPrivateKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPrivateKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPrivateKeyActions.Size = new System.Drawing.Size(194, 312);
            panelPrivateKeyActions.TabIndex = 4;

            labelPrivateActionsTitle.AutoSize = true;
            labelPrivateActionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            labelPrivateActionsTitle.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
            labelPrivateActionsTitle.Location = new System.Drawing.Point(8, 4);
            labelPrivateActionsTitle.Margin = new System.Windows.Forms.Padding(0);
            labelPrivateActionsTitle.Name = "labelPrivateActionsTitle";
            labelPrivateActionsTitle.Size = new System.Drawing.Size(123, 25);
            labelPrivateActionsTitle.TabIndex = 0;
            labelPrivateActionsTitle.Text = "🔑 私钥操作";

            btnCopyPrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCopyPrivateKey.Location = new System.Drawing.Point(10, 32);
            btnCopyPrivateKey.Margin = new System.Windows.Forms.Padding(2);
            btnCopyPrivateKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnCopyPrivateKey.Name = "btnCopyPrivateKey";
            btnCopyPrivateKey.Size = new System.Drawing.Size(174, 51);
            btnCopyPrivateKey.TabIndex = 0;
            btnCopyPrivateKey.Text = "复制私钥";
            btnCopyPrivateKey.Click += BtnCopyPrivateKey_Click;

            btnPastePrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPastePrivateKey.Location = new System.Drawing.Point(10, 87);
            btnPastePrivateKey.Margin = new System.Windows.Forms.Padding(2);
            btnPastePrivateKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnPastePrivateKey.Name = "btnPastePrivateKey";
            btnPastePrivateKey.Size = new System.Drawing.Size(174, 51);
            btnPastePrivateKey.TabIndex = 1;
            btnPastePrivateKey.Text = "粘贴私钥";
            btnPastePrivateKey.Click += BtnPastePrivateKey_Click;

            btnImportPrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnImportPrivateKey.Location = new System.Drawing.Point(10, 142);
            btnImportPrivateKey.Margin = new System.Windows.Forms.Padding(2);
            btnImportPrivateKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnImportPrivateKey.Name = "btnImportPrivateKey";
            btnImportPrivateKey.Size = new System.Drawing.Size(174, 51);
            btnImportPrivateKey.TabIndex = 2;
            btnImportPrivateKey.Text = "导入私钥";
            btnImportPrivateKey.Click += BtnImportPrivateKey_Click;

            btnSavePrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnSavePrivateKey.Location = new System.Drawing.Point(10, 197);
            btnSavePrivateKey.Margin = new System.Windows.Forms.Padding(2);
            btnSavePrivateKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnSavePrivateKey.Name = "btnSavePrivateKey";
            btnSavePrivateKey.Size = new System.Drawing.Size(174, 51);
            btnSavePrivateKey.TabIndex = 3;
            btnSavePrivateKey.Text = "保存私钥";
            btnSavePrivateKey.Click += BtnSavePrivateKey_Click;

            btnClearPrivateKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnClearPrivateKey.Location = new System.Drawing.Point(10, 252);
            btnClearPrivateKey.Margin = new System.Windows.Forms.Padding(2);
            btnClearPrivateKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnClearPrivateKey.Name = "btnClearPrivateKey";
            btnClearPrivateKey.Size = new System.Drawing.Size(174, 54);
            btnClearPrivateKey.TabIndex = 4;
            btnClearPrivateKey.Text = "清空私钥";
            btnClearPrivateKey.Click += BtnClearPrivateKey_Click;

            // ---- 公钥输入面板 (列0, 跨行2~3) ----
            panelPublicKeyBox.Controls.Add(labelPublicKey);
            panelPublicKeyBox.Controls.Add(textPublicKey);
            panelPublicKeyBox.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPublicKeyBox.Location = new System.Drawing.Point(9, 327);
            panelPublicKeyBox.Name = "panelPublicKeyBox";
            tableLayoutKey.SetRowSpan(panelPublicKeyBox, 2);
            panelPublicKeyBox.Size = new System.Drawing.Size(1395, 312);
            panelPublicKeyBox.TabIndex = 7;

            labelPublicKey.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelPublicKey.AutoSize = true;
            labelPublicKey.BackColor = System.Drawing.Color.Transparent;
            labelPublicKey.Location = new System.Drawing.Point(1263, 4);
            labelPublicKey.Margin = new System.Windows.Forms.Padding(4, 4, 4, 2);
            labelPublicKey.Name = "labelPublicKey";
            labelPublicKey.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPublicKey.Size = new System.Drawing.Size(128, 24);
            labelPublicKey.TabIndex = 2;
            labelPublicKey.Text = "公钥 (PEM)：";

            textPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            textPublicKey.Location = new System.Drawing.Point(0, 0);
            textPublicKey.Multiline = true;
            textPublicKey.Name = "textPublicKey";
            textPublicKey.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textPublicKey.Size = new System.Drawing.Size(1395, 312);
            textPublicKey.TabIndex = 3;

            // ---- 公钥操作按钮列 (列1, 跨行2~3): 6行网格 ----
            panelPublicKeyActions.ColumnCount = 1;
            panelPublicKeyActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelPublicKeyActions.Controls.Add(labelPublicActionsTitle, 0, 0);
            panelPublicKeyActions.Controls.Add(btnCopyPublicKey, 0, 1);
            panelPublicKeyActions.Controls.Add(btnPastePublicKey, 0, 2);
            panelPublicKeyActions.Controls.Add(btnImportPublicKey, 0, 3);
            panelPublicKeyActions.Controls.Add(btnSavePublicKey, 0, 4);
            panelPublicKeyActions.Controls.Add(btnClearPublicKey, 0, 5);
            panelPublicKeyActions.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPublicKeyActions.Location = new System.Drawing.Point(1410, 327);
            panelPublicKeyActions.Name = "panelPublicKeyActions";
            panelPublicKeyActions.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            panelPublicKeyActions.RowCount = 6;
            tableLayoutKey.SetRowSpan(panelPublicKeyActions, 2);
            panelPublicKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            panelPublicKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPublicKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 20F));
            panelPublicKeyActions.Size = new System.Drawing.Size(194, 312);
            panelPublicKeyActions.TabIndex = 5;

            labelPublicActionsTitle.AutoSize = true;
            labelPublicActionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            labelPublicActionsTitle.ForeColor = System.Drawing.Color.FromArgb(0, 100, 180);
            labelPublicActionsTitle.Location = new System.Drawing.Point(8, 4);
            labelPublicActionsTitle.Margin = new System.Windows.Forms.Padding(0);
            labelPublicActionsTitle.Name = "labelPublicActionsTitle";
            labelPublicActionsTitle.Size = new System.Drawing.Size(123, 25);
            labelPublicActionsTitle.TabIndex = 0;
            labelPublicActionsTitle.Text = "🔓 公钥操作";

            btnCopyPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCopyPublicKey.Location = new System.Drawing.Point(10, 32);
            btnCopyPublicKey.Margin = new System.Windows.Forms.Padding(2);
            btnCopyPublicKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnCopyPublicKey.Name = "btnCopyPublicKey";
            btnCopyPublicKey.Size = new System.Drawing.Size(174, 51);
            btnCopyPublicKey.TabIndex = 0;
            btnCopyPublicKey.Text = "复制公钥";
            btnCopyPublicKey.Click += BtnCopyPublicKey_Click;

            btnPastePublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPastePublicKey.Location = new System.Drawing.Point(10, 87);
            btnPastePublicKey.Margin = new System.Windows.Forms.Padding(2);
            btnPastePublicKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnPastePublicKey.Name = "btnPastePublicKey";
            btnPastePublicKey.Size = new System.Drawing.Size(174, 51);
            btnPastePublicKey.TabIndex = 1;
            btnPastePublicKey.Text = "粘贴公钥";
            btnPastePublicKey.Click += BtnPastePublicKey_Click;

            btnImportPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnImportPublicKey.Location = new System.Drawing.Point(10, 142);
            btnImportPublicKey.Margin = new System.Windows.Forms.Padding(2);
            btnImportPublicKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnImportPublicKey.Name = "btnImportPublicKey";
            btnImportPublicKey.Size = new System.Drawing.Size(174, 51);
            btnImportPublicKey.TabIndex = 2;
            btnImportPublicKey.Text = "导入公钥";
            btnImportPublicKey.Click += BtnImportPublicKey_Click;

            btnSavePublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnSavePublicKey.Location = new System.Drawing.Point(10, 197);
            btnSavePublicKey.Margin = new System.Windows.Forms.Padding(2);
            btnSavePublicKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnSavePublicKey.Name = "btnSavePublicKey";
            btnSavePublicKey.Size = new System.Drawing.Size(174, 51);
            btnSavePublicKey.TabIndex = 3;
            btnSavePublicKey.Text = "保存公钥";
            btnSavePublicKey.Click += BtnSavePublicKey_Click;

            btnClearPublicKey.Dock = System.Windows.Forms.DockStyle.Fill;
            btnClearPublicKey.Location = new System.Drawing.Point(10, 252);
            btnClearPublicKey.Margin = new System.Windows.Forms.Padding(2);
            btnClearPublicKey.MinimumSize = new System.Drawing.Size(120, 30);
            btnClearPublicKey.Name = "btnClearPublicKey";
            btnClearPublicKey.Size = new System.Drawing.Size(174, 54);
            btnClearPublicKey.TabIndex = 4;
            btnClearPublicKey.Text = "清空公钥";
            btnClearPublicKey.Click += BtnClearPublicKey_Click;

            // ==================================================
            // 右侧操作面板 - panelActionButtons (列1行0)
            // ==================================================
            panelActionButtons.Controls.Add(tableActionButtons);
            panelActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            panelActionButtons.Location = new System.Drawing.Point(1643, 8);
            panelActionButtons.Margin = new System.Windows.Forms.Padding(0);
            panelActionButtons.Name = "panelActionButtons";
            panelActionButtons.Size = new System.Drawing.Size(1636, 693);
            panelActionButtons.TabIndex = 1;

            // ---- tableActionButtons: 2列(50/50) × 1行 ----
            tableActionButtons.ColumnCount = 2;
            tableActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableActionButtons.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableActionButtons.Controls.Add(groupKeyActions, 0, 0);
            tableActionButtons.Dock = System.Windows.Forms.DockStyle.Fill;
            tableActionButtons.Location = new System.Drawing.Point(0, 0);
            tableActionButtons.Margin = new System.Windows.Forms.Padding(0);
            tableActionButtons.Name = "tableActionButtons";
            tableActionButtons.RowCount = 1;
            tableActionButtons.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableActionButtons.Size = new System.Drawing.Size(1636, 693);
            tableActionButtons.TabIndex = 0;

            // ---- groupKeyActions: 密钥操作按钮区 (跨2列) ----
            tableActionButtons.SetColumnSpan(groupKeyActions, 2);
            groupKeyActions.Controls.Add(tableKeyActions);
            groupKeyActions.Dock = System.Windows.Forms.DockStyle.Fill;
            groupKeyActions.Location = new System.Drawing.Point(3, 3);
            groupKeyActions.Name = "groupKeyActions";
            groupKeyActions.Padding = new System.Windows.Forms.Padding(8);
            groupKeyActions.Size = new System.Drawing.Size(1630, 687);
            groupKeyActions.TabIndex = 1;
            groupKeyActions.TabStop = false;
            groupKeyActions.Text = "密钥操作";

            // ---- tableKeyActions: 2列 × 2行(50/50) ----
            tableKeyActions.ColumnCount = 2;
            tableKeyActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableKeyActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableKeyActions.Controls.Add(panelButtonArea, 0, 0);
            tableKeyActions.Controls.Add(groupComputeResult, 0, 1);
            tableKeyActions.Controls.Add(groupRunResult, 1, 1);
            tableKeyActions.Dock = System.Windows.Forms.DockStyle.Fill;
            tableKeyActions.Location = new System.Drawing.Point(8, 31);
            tableKeyActions.Name = "tableKeyActions";
            tableKeyActions.RowCount = 2;
            tableKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableKeyActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableKeyActions.Size = new System.Drawing.Size(1614, 648);
            tableKeyActions.TabIndex = 0;

            // ---- panelButtonArea: 按钮+设置区 (跨2列行0) ----
            tableKeyActions.SetColumnSpan(panelButtonArea, 2);
            panelButtonArea.Controls.Add(tableButtonArea);
            panelButtonArea.Dock = System.Windows.Forms.DockStyle.Fill;
            panelButtonArea.Location = new System.Drawing.Point(3, 3);
            panelButtonArea.Name = "panelButtonArea";
            panelButtonArea.Size = new System.Drawing.Size(1608, 318);
            panelButtonArea.TabIndex = 1;

            // ---- tableButtonArea: 1列 × 1行 ----
            tableButtonArea.ColumnCount = 1;
            tableButtonArea.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableButtonArea.Controls.Add(tableRightActions, 0, 0);
            tableButtonArea.Dock = System.Windows.Forms.DockStyle.Fill;
            tableButtonArea.Location = new System.Drawing.Point(0, 0);
            tableButtonArea.Name = "tableButtonArea";
            tableButtonArea.RowCount = 1;
            tableButtonArea.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableButtonArea.Size = new System.Drawing.Size(1608, 318);
            tableButtonArea.TabIndex = 0;

            // ---- tableRightActions: 2列(180px|填充) × 3行 ----
            tableRightActions.ColumnCount = 2;
            tableRightActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 180F));
            tableRightActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableRightActions.Controls.Add(panelKeyControlsContainer, 0, 0);
            tableRightActions.Controls.Add(panelRightSettings, 1, 0);
            tableRightActions.Dock = System.Windows.Forms.DockStyle.Fill;
            tableRightActions.Location = new System.Drawing.Point(3, 3);
            tableRightActions.Name = "tableRightActions";
            tableRightActions.RowCount = 3;
            tableRightActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            tableRightActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 75F));
            tableRightActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 70F));
            tableRightActions.Size = new System.Drawing.Size(1602, 312);
            tableRightActions.TabIndex = 0;

            // ---- 左侧: 纵向按钮列 (跨3行) ----
            panelKeyControlsContainer.Controls.Add(panelKeyControls);
            panelKeyControlsContainer.Dock = System.Windows.Forms.DockStyle.Fill;
            panelKeyControlsContainer.Location = new System.Drawing.Point(3, 3);
            panelKeyControlsContainer.Name = "panelKeyControlsContainer";
            panelKeyControlsContainer.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            tableRightActions.SetRowSpan(panelKeyControlsContainer, 3);
            panelKeyControlsContainer.Size = new System.Drawing.Size(174, 306);
            panelKeyControlsContainer.TabIndex = 1;

            // FlowLayoutPanel: 纵向排列5个核心按钮
            panelKeyControls.Controls.Add(btnGenerateKeyPair);
            panelKeyControls.Controls.Add(btnValidateKeyPair);
            panelKeyControls.Controls.Add(btnGetPublicKeyFromPrivate);
            panelKeyControls.Controls.Add(btnGetCurveType);
            panelKeyControls.Controls.Add(btnClearAll);
            panelKeyControls.Dock = System.Windows.Forms.DockStyle.Fill;
            panelKeyControls.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            panelKeyControls.Location = new System.Drawing.Point(6, 0);
            panelKeyControls.Name = "panelKeyControls";
            panelKeyControls.Size = new System.Drawing.Size(162, 306);
            panelKeyControls.TabIndex = 1;
            panelKeyControls.WrapContents = false;

            btnGenerateKeyPair.Location = new System.Drawing.Point(0, 0);
            btnGenerateKeyPair.Margin = new System.Windows.Forms.Padding(0, 0, 6, 0);
            btnGenerateKeyPair.Name = "btnGenerateKeyPair";
            btnGenerateKeyPair.Size = new System.Drawing.Size(150, 40);
            btnGenerateKeyPair.TabIndex = 0;
            btnGenerateKeyPair.Text = "生成密钥对";
            btnGenerateKeyPair.Click += BtnGenerateKeyPair_Click;

            btnValidateKeyPair.Location = new System.Drawing.Point(0, 43);
            btnValidateKeyPair.Margin = new System.Windows.Forms.Padding(0, 3, 6, 0);
            btnValidateKeyPair.Name = "btnValidateKeyPair";
            btnValidateKeyPair.Size = new System.Drawing.Size(150, 40);
            btnValidateKeyPair.TabIndex = 1;
            btnValidateKeyPair.Text = "验证密钥对";
            btnValidateKeyPair.Click += BtnValidateKeyPair_Click;

            btnGetPublicKeyFromPrivate.Location = new System.Drawing.Point(0, 86);
            btnGetPublicKeyFromPrivate.Margin = new System.Windows.Forms.Padding(0, 3, 6, 0);
            btnGetPublicKeyFromPrivate.Name = "btnGetPublicKeyFromPrivate";
            btnGetPublicKeyFromPrivate.Size = new System.Drawing.Size(150, 40);
            btnGetPublicKeyFromPrivate.TabIndex = 2;
            btnGetPublicKeyFromPrivate.Text = "从私钥提取公钥";
            btnGetPublicKeyFromPrivate.Click += BtnGetPublicKeyFromPrivate_Click;

            btnGetCurveType.Location = new System.Drawing.Point(0, 129);
            btnGetCurveType.Margin = new System.Windows.Forms.Padding(0, 3, 6, 0);
            btnGetCurveType.Name = "btnGetCurveType";
            btnGetCurveType.Size = new System.Drawing.Size(150, 40);
            btnGetCurveType.TabIndex = 3;
            btnGetCurveType.Text = "获取私钥曲线类型";
            btnGetCurveType.Click += BtnGetCurveType_Click;

            btnClearAll.Location = new System.Drawing.Point(0, 172);
            btnClearAll.Margin = new System.Windows.Forms.Padding(0, 3, 6, 0);
            btnClearAll.Name = "btnClearAll";
            btnClearAll.Size = new System.Drawing.Size(150, 40);
            btnClearAll.TabIndex = 4;
            btnClearAll.Text = "清空全部";
            btnClearAll.Click += BtnClearAll_Click;

            // ---- 右侧: 设置面板 (跨3行, 纵向FlowLayout) ----
            panelRightSettings.AutoSize = true;
            panelRightSettings.Controls.Add(panelFormatRow);
            panelRightSettings.Controls.Add(panelKeyTypeRow);
            panelRightSettings.Controls.Add(panelCurveContainer);
            panelRightSettings.Dock = System.Windows.Forms.DockStyle.Top;
            panelRightSettings.FlowDirection = System.Windows.Forms.FlowDirection.TopDown;
            panelRightSettings.Location = new System.Drawing.Point(183, 3);
            panelRightSettings.Name = "panelRightSettings";
            tableRightActions.SetRowSpan(panelRightSettings, 3);
            panelRightSettings.Size = new System.Drawing.Size(1416, 134);
            panelRightSettings.TabIndex = 2;
            panelRightSettings.WrapContents = false;

            // 第1行: 输出格式 (PEM/Base64/Hex)
            panelFormatRow.AutoSize = true;
            panelFormatRow.Controls.Add(labelOutputFormat);
            panelFormatRow.Controls.Add(comboOutputFormat);
            panelFormatRow.Location = new System.Drawing.Point(3, 3);
            panelFormatRow.Name = "panelFormatRow";
            panelFormatRow.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            panelFormatRow.Size = new System.Drawing.Size(303, 38);
            panelFormatRow.TabIndex = 0;

            labelOutputFormat.Location = new System.Drawing.Point(40, 3);
            labelOutputFormat.Margin = new System.Windows.Forms.Padding(34, 3, 2, 3);
            labelOutputFormat.Name = "labelOutputFormat";
            labelOutputFormat.Size = new System.Drawing.Size(100, 32);
            labelOutputFormat.TabIndex = 2;
            labelOutputFormat.Text = "输出格式：";
            labelOutputFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            comboOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboOutputFormat.FormattingEnabled = true;
            comboOutputFormat.Items.AddRange(new object[] { "PEM", "Base64", "Hex大写", "Hex小写" });
            comboOutputFormat.Location = new System.Drawing.Point(142, 3);
            comboOutputFormat.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            comboOutputFormat.Name = "comboOutputFormat";
            comboOutputFormat.Size = new System.Drawing.Size(147, 32);
            comboOutputFormat.TabIndex = 3;
            comboOutputFormat.SelectedIndexChanged += ComboOutputFormat_SelectedIndexChanged;

            // 第2行: 密钥类型转换 (私钥↔公钥)
            panelKeyTypeRow.AutoSize = true;
            panelKeyTypeRow.Controls.Add(labelKeyType);
            panelKeyTypeRow.Controls.Add(radioPanel);
            panelKeyTypeRow.Controls.Add(btnConvertKey);
            panelKeyTypeRow.Location = new System.Drawing.Point(3, 47);
            panelKeyTypeRow.Name = "panelKeyTypeRow";
            panelKeyTypeRow.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            panelKeyTypeRow.Size = new System.Drawing.Size(409, 40);
            panelKeyTypeRow.TabIndex = 1;

            labelKeyType.Location = new System.Drawing.Point(40, 3);
            labelKeyType.Margin = new System.Windows.Forms.Padding(34, 3, 2, 3);
            labelKeyType.Name = "labelKeyType";
            labelKeyType.Size = new System.Drawing.Size(100, 34);
            labelKeyType.TabIndex = 4;
            labelKeyType.Text = "密钥类型：";
            labelKeyType.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            radioPanel.AutoSize = true;
            radioPanel.Controls.Add(radioPrivateKey);
            radioPanel.Controls.Add(radioPublicKey);
            radioPanel.Location = new System.Drawing.Point(142, 3);
            radioPanel.Margin = new System.Windows.Forms.Padding(0, 3, 8, 3);
            radioPanel.Name = "radioPanel";
            radioPanel.Size = new System.Drawing.Size(157, 34);
            radioPanel.TabIndex = 5;

            radioPrivateKey.AutoSize = true;
            radioPrivateKey.Checked = true;
            radioPrivateKey.Location = new System.Drawing.Point(3, 3);
            radioPrivateKey.Margin = new System.Windows.Forms.Padding(3, 3, 6, 3);
            radioPrivateKey.Name = "radioPrivateKey";
            radioPrivateKey.Size = new System.Drawing.Size(71, 28);
            radioPrivateKey.TabIndex = 0;
            radioPrivateKey.TabStop = true;
            radioPrivateKey.Text = "私钥";

            radioPublicKey.AutoSize = true;
            radioPublicKey.Location = new System.Drawing.Point(83, 3);
            radioPublicKey.Name = "radioPublicKey";
            radioPublicKey.Size = new System.Drawing.Size(71, 28);
            radioPublicKey.TabIndex = 1;
            radioPublicKey.Text = "公钥";

            btnConvertKey.AutoSize = true;
            btnConvertKey.Location = new System.Drawing.Point(319, 3);
            btnConvertKey.Margin = new System.Windows.Forms.Padding(12, 3, 4, 3);
            btnConvertKey.MinimumSize = new System.Drawing.Size(80, 26);
            btnConvertKey.Name = "btnConvertKey";
            btnConvertKey.Size = new System.Drawing.Size(80, 34);
            btnConvertKey.TabIndex = 6;
            btnConvertKey.Text = "转换";
            btnConvertKey.Click += BtnConvertKey_Click;

            // 第3行: 椭圆曲线选择 (类别 → 曲线名)
            panelCurveContainer.Controls.Add(panelCurveRow);
            panelCurveContainer.Location = new System.Drawing.Point(3, 93);
            panelCurveContainer.Name = "panelCurveContainer";
            panelCurveContainer.Padding = new System.Windows.Forms.Padding(6, 0, 6, 0);
            panelCurveContainer.Size = new System.Drawing.Size(920, 38);
            panelCurveContainer.TabIndex = 2;

            panelCurveRow.Controls.Add(labelCurve);
            panelCurveRow.Controls.Add(comboCategory);
            panelCurveRow.Controls.Add(lblArrow);
            panelCurveRow.Controls.Add(comboCurve);
            panelCurveRow.Location = new System.Drawing.Point(6, 0);
            panelCurveRow.Name = "panelCurveRow";
            panelCurveRow.Padding = new System.Windows.Forms.Padding(0, 2, 0, 2);
            panelCurveRow.Size = new System.Drawing.Size(900, 38);
            panelCurveRow.TabIndex = 1;
            panelCurveRow.WrapContents = false;

            labelCurve.Location = new System.Drawing.Point(34, 5);
            labelCurve.Margin = new System.Windows.Forms.Padding(34, 3, 2, 3);
            labelCurve.Name = "labelCurve";
            labelCurve.Size = new System.Drawing.Size(100, 32);
            labelCurve.TabIndex = 0;
            labelCurve.Text = "椭圆曲线：";
            labelCurve.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            labelCurve.Click += LabelCurve_Click;

            comboCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboCategory.FormattingEnabled = true;
            comboCategory.Location = new System.Drawing.Point(136, 5);
            comboCategory.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboCategory.Name = "comboCategory";
            comboCategory.Size = new System.Drawing.Size(183, 32);
            comboCategory.TabIndex = 1;
            comboCategory.SelectedIndexChanged += ComboCategory_SelectedIndexChanged;

            lblArrow.Location = new System.Drawing.Point(327, 5);
            lblArrow.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            lblArrow.Name = "lblArrow";
            lblArrow.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblArrow.Size = new System.Drawing.Size(36, 32);
            lblArrow.TabIndex = 2;
            lblArrow.Text = "→";
            lblArrow.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;

            comboCurve.DisplayMember = "Value";
            comboCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboCurve.FormattingEnabled = true;
            comboCurve.Location = new System.Drawing.Point(367, 5);
            comboCurve.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboCurve.Name = "comboCurve";
            comboCurve.Size = new System.Drawing.Size(520, 32);
            comboCurve.TabIndex = 3;
            comboCurve.ValueMember = "Key";

            // ---- 计算结果输出框 (行1, 左) ----
            groupComputeResult.Controls.Add(textKeyResult);
            groupComputeResult.Dock = System.Windows.Forms.DockStyle.Fill;
            groupComputeResult.Location = new System.Drawing.Point(3, 327);
            groupComputeResult.Name = "groupComputeResult";
            groupComputeResult.Padding = new System.Windows.Forms.Padding(8);
            groupComputeResult.Size = new System.Drawing.Size(801, 318);
            groupComputeResult.TabIndex = 0;
            groupComputeResult.TabStop = false;
            groupComputeResult.Text = "计算结果";

            textKeyResult.BackColor = System.Drawing.Color.White;
            textKeyResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            textKeyResult.Dock = System.Windows.Forms.DockStyle.Fill;
            textKeyResult.Font = new System.Drawing.Font("Segoe UI", 9F);
            textKeyResult.Location = new System.Drawing.Point(8, 31);
            textKeyResult.Name = "textKeyResult";
            textKeyResult.ReadOnly = true;
            textKeyResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            textKeyResult.Size = new System.Drawing.Size(785, 279);
            textKeyResult.TabIndex = 1;
            textKeyResult.Text = "从私钥提取/曲线检测：\n等待操作...";

            // ---- 运行结果输出框 (行1, 右) ----
            groupRunResult.Controls.Add(labelValidationResult);
            groupRunResult.Dock = System.Windows.Forms.DockStyle.Fill;
            groupRunResult.Location = new System.Drawing.Point(810, 327);
            groupRunResult.Name = "groupRunResult";
            groupRunResult.Padding = new System.Windows.Forms.Padding(8);
            groupRunResult.Size = new System.Drawing.Size(801, 318);
            groupRunResult.TabIndex = 1;
            groupRunResult.TabStop = false;
            groupRunResult.Text = "运行结果";

            labelValidationResult.BackColor = System.Drawing.Color.White;
            labelValidationResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            labelValidationResult.Dock = System.Windows.Forms.DockStyle.Fill;
            labelValidationResult.Font = new System.Drawing.Font("Segoe UI", 9F);
            labelValidationResult.ForeColor = System.Drawing.Color.Gray;
            labelValidationResult.Location = new System.Drawing.Point(8, 31);
            labelValidationResult.Name = "labelValidationResult";
            labelValidationResult.ReadOnly = true;
            labelValidationResult.ScrollBars = System.Windows.Forms.RichTextBoxScrollBars.Vertical;
            labelValidationResult.Size = new System.Drawing.Size(785, 279);
            labelValidationResult.TabIndex = 0;
            labelValidationResult.Text = "验证结果: 未验证";

            // ==================================================
            // 视图切换栏 - panelViewBar (跨2列, 行1)
            //   4个Tab按钮: ECDH | 签名验签 | 加密解密 | 文件操作
            // ==================================================
            mainTableLayout.SetColumnSpan(panelViewBar, 2);
            panelViewBar.Controls.Add(btnViewEcdh);
            panelViewBar.Controls.Add(btnViewSign);
            panelViewBar.Controls.Add(btnViewEncrypt);
            panelViewBar.Controls.Add(btnViewFile);
            panelViewBar.Dock = System.Windows.Forms.DockStyle.Fill;
            panelViewBar.Location = new System.Drawing.Point(11, 704);
            panelViewBar.Name = "panelViewBar";
            panelViewBar.Padding = new System.Windows.Forms.Padding(4);
            panelViewBar.Size = new System.Drawing.Size(3265, 54);
            panelViewBar.TabIndex = 10;
            panelViewBar.WrapContents = false;

            btnViewEcdh.AutoSize = true;
            btnViewEcdh.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewEcdh.Location = new System.Drawing.Point(6, 6);
            btnViewEcdh.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            btnViewEcdh.Name = "btnViewEcdh";
            btnViewEcdh.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            btnViewEcdh.Size = new System.Drawing.Size(191, 44);
            btnViewEcdh.TabIndex = 0;
            btnViewEcdh.Text = "ECDH 加密解密";
            btnViewEcdh.UseVisualStyleBackColor = true;

            btnViewSign.AutoSize = true;
            btnViewSign.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewSign.Location = new System.Drawing.Point(207, 6);
            btnViewSign.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            btnViewSign.Name = "btnViewSign";
            btnViewSign.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            btnViewSign.Size = new System.Drawing.Size(126, 44);
            btnViewSign.TabIndex = 1;
            btnViewSign.Text = "ECIES签名验签";
            btnViewSign.UseVisualStyleBackColor = true;

            btnViewEncrypt.AutoSize = true;
            btnViewEncrypt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewEncrypt.Location = new System.Drawing.Point(343, 6);
            btnViewEncrypt.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            btnViewEncrypt.Name = "btnViewEncrypt";
            btnViewEncrypt.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            btnViewEncrypt.Size = new System.Drawing.Size(136, 44);
            btnViewEncrypt.TabIndex = 2;
            btnViewEncrypt.Text = "ECIES加密解密";
            btnViewEncrypt.UseVisualStyleBackColor = true;

            btnViewFile.AutoSize = true;
            btnViewFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            btnViewFile.Location = new System.Drawing.Point(489, 6);
            btnViewFile.Margin = new System.Windows.Forms.Padding(2, 2, 8, 2);
            btnViewFile.Name = "btnViewFile";
            btnViewFile.Padding = new System.Windows.Forms.Padding(12, 4, 12, 4);
            btnViewFile.Size = new System.Drawing.Size(162, 44);
            btnViewFile.TabIndex = 3;
            btnViewFile.Text = "文件签名/验签";
            btnViewFile.UseVisualStyleBackColor = true;

            // ==================================================
            // 视图内容容器 - panelViewContent (跨2列, 行2)
            //   包含4个可切换面板: groupSign | groupEncrypt | groupFile | groupEcdh
            // ==================================================
            mainTableLayout.SetColumnSpan(panelViewContent, 2);
            panelViewContent.Controls.Add(groupSign);
            panelViewContent.Controls.Add(groupEncrypt);
            panelViewContent.Controls.Add(groupFile);
            panelViewContent.Controls.Add(groupEcdh);
            panelViewContent.Dock = System.Windows.Forms.DockStyle.Fill;
            panelViewContent.Location = new System.Drawing.Point(11, 764);
            panelViewContent.Name = "panelViewContent";
            panelViewContent.Size = new System.Drawing.Size(3265, 841);
            panelViewContent.TabIndex = 11;

            // ==================================================
            // 签名验签面板 - groupSign
            //   tableLayoutSign(2列): groupSignInput(左) | groupSignActions(右)
            // ==================================================
            groupSign.Controls.Add(tableLayoutSign);
            groupSign.Dock = System.Windows.Forms.DockStyle.Fill;
            groupSign.Location = new System.Drawing.Point(0, 0);
            groupSign.Name = "groupSign";
            groupSign.Padding = new System.Windows.Forms.Padding(4);
            groupSign.Size = new System.Drawing.Size(3265, 841);
            groupSign.TabIndex = 0;

            // ---- tableLayoutSign: 2列(50/50) × 1行 ----
            tableLayoutSign.ColumnCount = 2;
            tableLayoutSign.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutSign.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            tableLayoutSign.Controls.Add(groupSignInput, 0, 0);
            tableLayoutSign.Controls.Add(groupSignActions, 1, 0);
            tableLayoutSign.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutSign.Location = new System.Drawing.Point(4, 4);
            tableLayoutSign.Name = "tableLayoutSign";
            tableLayoutSign.RowCount = 1;
            tableLayoutSign.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
            tableLayoutSign.Size = new System.Drawing.Size(3257, 833);
            tableLayoutSign.TabIndex = 0;

            // ---- 左侧: 签名输入区 (groupSignInput) ----
            groupSignInput.Controls.Add(panelSignInput);
            groupSignInput.Dock = System.Windows.Forms.DockStyle.Fill;
            groupSignInput.Location = new System.Drawing.Point(3, 3);
            groupSignInput.Name = "groupSignInput";
            groupSignInput.Padding = new System.Windows.Forms.Padding(8);
            groupSignInput.Size = new System.Drawing.Size(1622, 827);
            groupSignInput.TabIndex = 0;
            groupSignInput.TabStop = false;
            groupSignInput.Text = "签名验签";

            // panelSignInput: 2列(PEM|200px) × 4行(原文/间隔/签名/间隔)
            panelSignInput.ColumnCount = 2;
            panelSignInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelSignInput.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Absolute, 200F));
            panelSignInput.Controls.Add(panelPlainDataBox, 0, 0);
            panelSignInput.Controls.Add(panelPlainDataActions, 1, 0);
            panelSignInput.Controls.Add(panelSignatureBox, 0, 2);
            panelSignInput.Controls.Add(panelSignatureActions, 1, 2);
            panelSignInput.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSignInput.Location = new System.Drawing.Point(8, 31);
            panelSignInput.Name = "panelSignInput";
            panelSignInput.Padding = new System.Windows.Forms.Padding(6);
            panelSignInput.RowCount = 4;
            panelSignInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            panelSignInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            panelSignInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            panelSignInput.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 0F));
            panelSignInput.Size = new System.Drawing.Size(1606, 788);
            panelSignInput.TabIndex = 0;

            // 原始数据面板 (列0, 跨行0~1)
            panelPlainDataBox.Controls.Add(labelPlainData);
            panelPlainDataBox.Controls.Add(textPlainData);
            panelPlainDataBox.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPlainDataBox.Location = new System.Drawing.Point(9, 9);
            panelPlainDataBox.Name = "panelPlainDataBox";
            panelSignInput.SetRowSpan(panelPlainDataBox, 2);
            panelPlainDataBox.Size = new System.Drawing.Size(1388, 382);
            panelPlainDataBox.TabIndex = 0;

            labelPlainData.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelPlainData.AutoSize = true;
            labelPlainData.BackColor = System.Drawing.Color.Transparent;
            labelPlainData.Location = new System.Drawing.Point(1276, 4);
            labelPlainData.Margin = new System.Windows.Forms.Padding(4, 4, 4, 2);
            labelPlainData.Name = "labelPlainData";
            labelPlainData.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelPlainData.Size = new System.Drawing.Size(108, 24);
            labelPlainData.TabIndex = 0;
            labelPlainData.Text = "原始数据：";

            textPlainData.Dock = System.Windows.Forms.DockStyle.Fill;
            textPlainData.Location = new System.Drawing.Point(0, 0);
            textPlainData.Multiline = true;
            textPlainData.Name = "textPlainData";
            textPlainData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textPlainData.Size = new System.Drawing.Size(1388, 382);
            textPlainData.TabIndex = 2;

            // 原始数据操作按钮 (列1, 跨行0~1): 3行
            panelPlainDataActions.ColumnCount = 1;
            panelPlainDataActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelPlainDataActions.Controls.Add(labelPlainDataActionsTitle, 0, 0);
            panelPlainDataActions.Controls.Add(btnCopyPlainData, 0, 1);
            panelPlainDataActions.Controls.Add(btnPastePlainData, 0, 2);
            panelPlainDataActions.Controls.Add(btnClearPlainData, 0, 3);
            panelPlainDataActions.Dock = System.Windows.Forms.DockStyle.Fill;
            panelPlainDataActions.Location = new System.Drawing.Point(1403, 9);
            panelPlainDataActions.Name = "panelPlainDataActions";
            panelPlainDataActions.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            panelPlainDataActions.RowCount = 4;
            panelSignInput.SetRowSpan(panelPlainDataActions, 2);
            panelPlainDataActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            panelPlainDataActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            panelPlainDataActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            panelPlainDataActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            panelPlainDataActions.Size = new System.Drawing.Size(194, 382);
            panelPlainDataActions.TabIndex = 4;

            labelPlainDataActionsTitle.AutoSize = true;
            labelPlainDataActionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            labelPlainDataActionsTitle.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
            labelPlainDataActionsTitle.Location = new System.Drawing.Point(8, 4);
            labelPlainDataActionsTitle.Margin = new System.Windows.Forms.Padding(0);
            labelPlainDataActionsTitle.Name = "labelPlainDataActionsTitle";
            labelPlainDataActionsTitle.Size = new System.Drawing.Size(92, 25);
            labelPlainDataActionsTitle.TabIndex = 0;
            labelPlainDataActionsTitle.Text = "数据操作";

            btnCopyPlainData.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCopyPlainData.Location = new System.Drawing.Point(10, 32);
            btnCopyPlainData.Margin = new System.Windows.Forms.Padding(2);
            btnCopyPlainData.MinimumSize = new System.Drawing.Size(120, 30);
            btnCopyPlainData.Name = "btnCopyPlainData";
            btnCopyPlainData.Size = new System.Drawing.Size(174, 110);
            btnCopyPlainData.TabIndex = 1;
            btnCopyPlainData.Text = "复制数据";
            btnCopyPlainData.Click += BtnCopyPlainData_Click;

            btnPastePlainData.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPastePlainData.Location = new System.Drawing.Point(10, 146);
            btnPastePlainData.Margin = new System.Windows.Forms.Padding(2);
            btnPastePlainData.MinimumSize = new System.Drawing.Size(120, 30);
            btnPastePlainData.Name = "btnPastePlainData";
            btnPastePlainData.Size = new System.Drawing.Size(174, 110);
            btnPastePlainData.TabIndex = 2;
            btnPastePlainData.Text = "粘贴数据";
            btnPastePlainData.Click += BtnPastePlainData_Click;

            btnClearPlainData.Dock = System.Windows.Forms.DockStyle.Fill;
            btnClearPlainData.Location = new System.Drawing.Point(10, 260);
            btnClearPlainData.Margin = new System.Windows.Forms.Padding(2);
            btnClearPlainData.MinimumSize = new System.Drawing.Size(120, 30);
            btnClearPlainData.Name = "btnClearPlainData";
            btnClearPlainData.Size = new System.Drawing.Size(174, 116);
            btnClearPlainData.TabIndex = 3;
            btnClearPlainData.Text = "清空数据";
            btnClearPlainData.Click += BtnClearPlainData_Click;

            // 签名数据面板 (列0, 跨行2~3)
            panelSignatureBox.Controls.Add(labelSignature);
            panelSignatureBox.Controls.Add(textSignature);
            panelSignatureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSignatureBox.Location = new System.Drawing.Point(9, 397);
            panelSignatureBox.Name = "panelSignatureBox";
            panelSignInput.SetRowSpan(panelSignatureBox, 2);
            panelSignatureBox.Size = new System.Drawing.Size(1388, 382);
            panelSignatureBox.TabIndex = 1;

            labelSignature.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right;
            labelSignature.AutoSize = true;
            labelSignature.BackColor = System.Drawing.Color.Transparent;
            labelSignature.Location = new System.Drawing.Point(1312, 4);
            labelSignature.Margin = new System.Windows.Forms.Padding(4, 4, 4, 2);
            labelSignature.Name = "labelSignature";
            labelSignature.Padding = new System.Windows.Forms.Padding(4, 0, 4, 0);
            labelSignature.Size = new System.Drawing.Size(72, 24);
            labelSignature.TabIndex = 3;
            labelSignature.Text = "签名：";

            textSignature.Dock = System.Windows.Forms.DockStyle.Fill;
            textSignature.Location = new System.Drawing.Point(0, 0);
            textSignature.Multiline = true;
            textSignature.Name = "textSignature";
            textSignature.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textSignature.Size = new System.Drawing.Size(1388, 382);
            textSignature.TabIndex = 4;

            // 签名操作按钮 (列1, 跨行2~3): 3行
            panelSignatureActions.ColumnCount = 1;
            panelSignatureActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelSignatureActions.Controls.Add(labelSignatureActionsTitle, 0, 0);
            panelSignatureActions.Controls.Add(btnCopySignatureData, 0, 1);
            panelSignatureActions.Controls.Add(btnPasteSignatureData, 0, 2);
            panelSignatureActions.Controls.Add(btnClearSignatureData, 0, 3);
            panelSignatureActions.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSignatureActions.Location = new System.Drawing.Point(1403, 397);
            panelSignatureActions.Name = "panelSignatureActions";
            panelSignatureActions.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            panelSignatureActions.RowCount = 4;
            panelSignInput.SetRowSpan(panelSignatureActions, 2);
            panelSignatureActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 26F));
            panelSignatureActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            panelSignatureActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33F));
            panelSignatureActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 34F));
            panelSignatureActions.Size = new System.Drawing.Size(194, 382);
            panelSignatureActions.TabIndex = 5;

            labelSignatureActionsTitle.AutoSize = true;
            labelSignatureActionsTitle.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold);
            labelSignatureActionsTitle.ForeColor = System.Drawing.Color.FromArgb(192, 0, 0);
            labelSignatureActionsTitle.Location = new System.Drawing.Point(8, 4);
            labelSignatureActionsTitle.Margin = new System.Windows.Forms.Padding(0);
            labelSignatureActionsTitle.Name = "labelSignatureActionsTitle";
            labelSignatureActionsTitle.Size = new System.Drawing.Size(92, 25);
            labelSignatureActionsTitle.TabIndex = 0;
            labelSignatureActionsTitle.Text = "签名操作";

            btnCopySignatureData.Dock = System.Windows.Forms.DockStyle.Fill;
            btnCopySignatureData.Location = new System.Drawing.Point(10, 32);
            btnCopySignatureData.Margin = new System.Windows.Forms.Padding(2);
            btnCopySignatureData.MinimumSize = new System.Drawing.Size(120, 30);
            btnCopySignatureData.Name = "btnCopySignatureData";
            btnCopySignatureData.Size = new System.Drawing.Size(174, 110);
            btnCopySignatureData.TabIndex = 1;
            btnCopySignatureData.Text = "复制签名";
            btnCopySignatureData.Click += BtnCopySignatureData_Click;

            btnPasteSignatureData.Dock = System.Windows.Forms.DockStyle.Fill;
            btnPasteSignatureData.Location = new System.Drawing.Point(10, 146);
            btnPasteSignatureData.Margin = new System.Windows.Forms.Padding(2);
            btnPasteSignatureData.MinimumSize = new System.Drawing.Size(120, 30);
            btnPasteSignatureData.Name = "btnPasteSignatureData";
            btnPasteSignatureData.Size = new System.Drawing.Size(174, 110);
            btnPasteSignatureData.TabIndex = 2;
            btnPasteSignatureData.Text = "粘贴签名";
            btnPasteSignatureData.Click += BtnPasteSignatureData_Click;

            btnClearSignatureData.Dock = System.Windows.Forms.DockStyle.Fill;
            btnClearSignatureData.Location = new System.Drawing.Point(10, 260);
            btnClearSignatureData.Margin = new System.Windows.Forms.Padding(2);
            btnClearSignatureData.MinimumSize = new System.Drawing.Size(120, 30);
            btnClearSignatureData.Name = "btnClearSignatureData";
            btnClearSignatureData.Size = new System.Drawing.Size(174, 116);
            btnClearSignatureData.TabIndex = 3;
            btnClearSignatureData.Text = "清空签名";
            btnClearSignatureData.Click += BtnClearSignatureData_Click;

            // ---- 右侧: 签名操作区 (groupSignActions) ----
            groupSignActions.Controls.Add(panelSignActions);
            groupSignActions.Dock = System.Windows.Forms.DockStyle.Fill;
            groupSignActions.Location = new System.Drawing.Point(1631, 3);
            groupSignActions.Name = "groupSignActions";
            groupSignActions.Padding = new System.Windows.Forms.Padding(8);
            groupSignActions.Size = new System.Drawing.Size(1623, 827);
            groupSignActions.TabIndex = 1;
            groupSignActions.TabStop = false;
            groupSignActions.Text = "操作按钮";

            // panelSignActions: 2列 × 3行(按钮|选项)
            panelSignActions.ColumnCount = 2;
            panelSignActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            panelSignActions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelSignActions.Controls.Add(btnSign, 0, 0);
            panelSignActions.Controls.Add(btnVerify, 0, 1);
            panelSignActions.Controls.Add(btnCopySignature, 0, 2);
            panelSignActions.Controls.Add(panelSignOptions, 1, 0);
            panelSignActions.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSignActions.Location = new System.Drawing.Point(8, 31);
            panelSignActions.Name = "panelSignActions";
            panelSignActions.Padding = new System.Windows.Forms.Padding(8, 4, 8, 4);
            panelSignActions.RowCount = 3;
            panelSignActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            panelSignActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3333F));
            panelSignActions.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 33.3334F));
            panelSignActions.Size = new System.Drawing.Size(1607, 788);
            panelSignActions.TabIndex = 1;

            // 签名操作按钮 (左列, 垂直排列)
            btnSign.AutoSize = true;
            btnSign.Dock = System.Windows.Forms.DockStyle.Top;
            btnSign.Location = new System.Drawing.Point(8, 4);
            btnSign.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            btnSign.MinimumSize = new System.Drawing.Size(120, 30);
            btnSign.Name = "btnSign";
            btnSign.Size = new System.Drawing.Size(120, 40);
            btnSign.TabIndex = 4;
            btnSign.Text = "签名";
            btnSign.Click += BtnSign_Click;

            btnVerify.AutoSize = true;
            btnVerify.Dock = System.Windows.Forms.DockStyle.Top;
            btnVerify.Location = new System.Drawing.Point(8, 263);
            btnVerify.Margin = new System.Windows.Forms.Padding(0, 0, 0, 4);
            btnVerify.MinimumSize = new System.Drawing.Size(120, 30);
            btnVerify.Name = "btnVerify";
            btnVerify.Size = new System.Drawing.Size(120, 40);
            btnVerify.TabIndex = 5;
            btnVerify.Text = "验签";
            btnVerify.Click += BtnVerify_Click;

            btnCopySignature.AutoSize = true;
            btnCopySignature.Dock = System.Windows.Forms.DockStyle.Top;
            btnCopySignature.Location = new System.Drawing.Point(8, 522);
            btnCopySignature.Margin = new System.Windows.Forms.Padding(0);
            btnCopySignature.MinimumSize = new System.Drawing.Size(120, 30);
            btnCopySignature.Name = "btnCopySignature";
            btnCopySignature.Size = new System.Drawing.Size(120, 40);
            btnCopySignature.TabIndex = 6;
            btnCopySignature.Text = "复制签名";
            btnCopySignature.Click += BtnCopySignature_Click;

            // 签名选项面板 (右列, 跨3行): Hash算法 + 签名格式
            panelSignOptions.ColumnCount = 2;
            panelSignOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            panelSignOptions.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            panelSignOptions.Controls.Add(labelHashAlgorithm, 0, 0);
            panelSignOptions.Controls.Add(comboHashAlgorithm, 1, 0);
            panelSignOptions.Controls.Add(labelSignatureFormat, 0, 1);
            panelSignOptions.Controls.Add(comboSignatureFormat, 1, 1);
            panelSignOptions.Dock = System.Windows.Forms.DockStyle.Fill;
            panelSignOptions.Location = new System.Drawing.Point(131, 7);
            panelSignOptions.Name = "panelSignOptions";
            panelSignOptions.RowCount = 2;
            panelSignActions.SetRowSpan(panelSignOptions, 3);
            panelSignOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            panelSignOptions.RowStyles.Add(new System.Windows.Forms.RowStyle());
            panelSignOptions.Size = new System.Drawing.Size(1465, 774);
            panelSignOptions.TabIndex = 7;

            labelHashAlgorithm.AutoSize = true;
            labelHashAlgorithm.Location = new System.Drawing.Point(0, 4);
            labelHashAlgorithm.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            labelHashAlgorithm.Name = "labelHashAlgorithm";
            labelHashAlgorithm.Size = new System.Drawing.Size(107, 24);
            labelHashAlgorithm.TabIndex = 0;
            labelHashAlgorithm.Text = "Hash算法：";
            labelHashAlgorithm.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            comboHashAlgorithm.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboHashAlgorithm.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboHashAlgorithm.FormattingEnabled = true;
            comboHashAlgorithm.Items.AddRange(new object[] { "SHA-224", "SHA-256", "SHA-384", "SHA-512", "SHA3-224", "SHA3-256", "SHA3-384", "SHA3-512" });
            comboHashAlgorithm.Location = new System.Drawing.Point(111, 0);
            comboHashAlgorithm.Margin = new System.Windows.Forms.Padding(0, 0, 16, 0);
            comboHashAlgorithm.Name = "comboHashAlgorithm";
            comboHashAlgorithm.Size = new System.Drawing.Size(1338, 32);
            comboHashAlgorithm.TabIndex = 1;

            labelSignatureFormat.AutoSize = true;
            labelSignatureFormat.Location = new System.Drawing.Point(0, 36);
            labelSignatureFormat.Margin = new System.Windows.Forms.Padding(0, 4, 4, 0);
            labelSignatureFormat.Name = "labelSignatureFormat";
            labelSignatureFormat.Size = new System.Drawing.Size(100, 24);
            labelSignatureFormat.TabIndex = 2;
            labelSignatureFormat.Text = "签名格式：";
            labelSignatureFormat.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;

            comboSignatureFormat.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            comboSignatureFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboSignatureFormat.FormattingEnabled = true;
            comboSignatureFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboSignatureFormat.Location = new System.Drawing.Point(111, 32);
            comboSignatureFormat.Margin = new System.Windows.Forms.Padding(0);
            comboSignatureFormat.Name = "comboSignatureFormat";
            comboSignatureFormat.Size = new System.Drawing.Size(1354, 32);
            comboSignatureFormat.TabIndex = 3;

            // ==================================================
            // 加密解密面板 - groupEncrypt
            //   tableLayoutEncrypt: 9行 (模式|输入格式|输出格式|密钥|IV|Bob公钥|曲线|输入|输出|按钮)
            // ==================================================
            groupEncrypt.BorderStyle = System.Windows.Forms.BorderStyle.None;
            groupEncrypt.Controls.Add(tableLayoutEncrypt);
            groupEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            groupEncrypt.Location = new System.Drawing.Point(0, 0);
            groupEncrypt.Name = "groupEncrypt";
            groupEncrypt.Padding = new System.Windows.Forms.Padding(8);
            groupEncrypt.Size = new System.Drawing.Size(3265, 841);
            groupEncrypt.TabIndex = 0;

            tableLayoutEncrypt.ColumnCount = 1;
            tableLayoutEncrypt.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
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
            tableLayoutEncrypt.Controls.Add(labelEncBobPublic, 0, 4);
            tableLayoutEncrypt.Controls.Add(textEncBobPublic, 0, 4);
            tableLayoutEncrypt.Controls.Add(labelEncCurve, 0, 5);
            tableLayoutEncrypt.Controls.Add(comboEncCurveCategory, 0, 5);
            tableLayoutEncrypt.Controls.Add(labelEncInput, 0, 4);
            tableLayoutEncrypt.Controls.Add(textEncInput, 0, 5);
            tableLayoutEncrypt.Controls.Add(textEncOutput, 0, 7);
            tableLayoutEncrypt.Controls.Add(panelEncBtns, 0, 8);
            tableLayoutEncrypt.Controls.Add(labelEncOutputLabel, 0, 6);
            tableLayoutEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            tableLayoutEncrypt.Location = new System.Drawing.Point(8, 31);
            tableLayoutEncrypt.Name = "tableLayoutEncrypt";
            tableLayoutEncrypt.RowCount = 9;
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 36F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 22F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 35F));
            tableLayoutEncrypt.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 44F));
            tableLayoutEncrypt.Size = new System.Drawing.Size(3249, 802);
            tableLayoutEncrypt.TabIndex = 0;

            labelEncMode.AutoSize = true;
            labelEncMode.Location = new System.Drawing.Point(4, 40);
            labelEncMode.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncMode.Name = "labelEncMode";
            labelEncMode.Size = new System.Drawing.Size(100, 24);
            labelEncMode.TabIndex = 1;
            labelEncMode.Text = "加密模式：";

            comboEncMode.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncMode.FormattingEnabled = true;
            comboEncMode.Items.AddRange(new object[] { "ECIES (ECDH+AES-GCM)", "AES-256-GCM", "AES-256-CBC", "ChaCha20-Poly1305" });
            comboEncMode.Location = new System.Drawing.Point(0, 3);
            comboEncMode.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboEncMode.Name = "comboEncMode";
            comboEncMode.Size = new System.Drawing.Size(364, 32);
            comboEncMode.TabIndex = 2;

            labelEncInputFormat.AutoSize = true;
            labelEncInputFormat.Location = new System.Drawing.Point(4, 148);
            labelEncInputFormat.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncInputFormat.Name = "labelEncInputFormat";
            labelEncInputFormat.Size = new System.Drawing.Size(100, 14);
            labelEncInputFormat.TabIndex = 3;
            labelEncInputFormat.Text = "输入格式：";

            comboEncInputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncInputFormat.FormattingEnabled = true;
            comboEncInputFormat.Items.AddRange(new object[] { "UTF-8文本", "Base64", "Hex" });
            comboEncInputFormat.Location = new System.Drawing.Point(0, 169);
            comboEncInputFormat.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboEncInputFormat.Name = "comboEncInputFormat";
            comboEncInputFormat.Size = new System.Drawing.Size(120, 32);
            comboEncInputFormat.TabIndex = 4;

            labelEncOutputFormat.AutoSize = true;
            labelEncOutputFormat.Location = new System.Drawing.Point(8, 76);
            labelEncOutputFormat.Margin = new System.Windows.Forms.Padding(8, 4, 2, 4);
            labelEncOutputFormat.Name = "labelEncOutputFormat";
            labelEncOutputFormat.Size = new System.Drawing.Size(100, 24);
            labelEncOutputFormat.TabIndex = 5;
            labelEncOutputFormat.Text = "输出格式：";

            comboEncOutputFormat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncOutputFormat.FormattingEnabled = true;
            comboEncOutputFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboEncOutputFormat.Location = new System.Drawing.Point(0, 111);
            comboEncOutputFormat.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboEncOutputFormat.Name = "comboEncOutputFormat";
            comboEncOutputFormat.Size = new System.Drawing.Size(120, 32);
            comboEncOutputFormat.TabIndex = 6;

            labelEncKey.AutoSize = true;
            labelEncKey.Location = new System.Drawing.Point(4, 417);
            labelEncKey.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncKey.Name = "labelEncKey";
            labelEncKey.Size = new System.Drawing.Size(279, 24);
            labelEncKey.TabIndex = 7;
            labelEncKey.Text = "对称密钥 (HEX，留空自动派生)：";

            textEncKey.Dock = System.Windows.Forms.DockStyle.Fill;
            textEncKey.Font = new System.Drawing.Font("Consolas", 9F);
            textEncKey.Location = new System.Drawing.Point(4, 394);
            textEncKey.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textEncKey.Name = "textEncKey";
            textEncKey.PlaceholderText = "从ECDSA私钥自动派生（HKDF-SHA256）";
            textEncKey.Size = new System.Drawing.Size(3241, 29);
            textEncKey.TabIndex = 8;

            labelEncIV.AutoSize = true;
            labelEncIV.Location = new System.Drawing.Point(4, 686);
            labelEncIV.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncIV.Name = "labelEncIV";
            labelEncIV.Size = new System.Drawing.Size(288, 12);
            labelEncIV.TabIndex = 9;
            labelEncIV.Text = "IV/Nonce (HEX，留空随机生成)：";

            textEncIV.Dock = System.Windows.Forms.DockStyle.Fill;
            textEncIV.Font = new System.Drawing.Font("Consolas", 9F);
            textEncIV.Location = new System.Drawing.Point(4, 641);
            textEncIV.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textEncIV.Name = "textEncIV";
            textEncIV.PlaceholderText = "加密时自动生成，解密时需填写";
            textEncIV.Size = new System.Drawing.Size(3241, 29);
            textEncIV.TabIndex = 10;

            labelEncBobPublic.AutoSize = true;
            labelEncBobPublic.Location = new System.Drawing.Point(4, 468);
            labelEncBobPublic.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncBobPublic.Name = "labelEncBobPublic";
            labelEncBobPublic.Size = new System.Drawing.Size(288, 24);
            labelEncBobPublic.TabIndex = 15;
            labelEncBobPublic.Text = "Bob 公钥 (接收方)：";

            textEncBobPublic.Dock = System.Windows.Forms.DockStyle.Fill;
            textEncBobPublic.Font = new System.Drawing.Font("Consolas", 9F);
            textEncBobPublic.Location = new System.Drawing.Point(4, 445);
            textEncBobPublic.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textEncBobPublic.Name = "textEncBobPublic";
            textEncBobPublic.PlaceholderText = "粘贴 PEM 格式公钥（支持多行）";
            textEncBobPublic.Multiline = true;
            textEncBobPublic.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textEncBobPublic.Size = new System.Drawing.Size(3241, 29);
            textEncBobPublic.TabIndex = 16;

            labelEncCurve.AutoSize = true;
            labelEncCurve.Location = new System.Drawing.Point(4, 555);
            labelEncCurve.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncCurve.Name = "labelEncCurve";
            labelEncCurve.Size = new System.Drawing.Size(100, 24);
            labelEncCurve.TabIndex = 17;
            labelEncCurve.Text = "曲线：";

            comboEncCurveCategory.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncCurveCategory.FormattingEnabled = true;
            comboEncCurveCategory.Location = new System.Drawing.Point(0, 579);
            comboEncCurveCategory.Margin = new System.Windows.Forms.Padding(0, 3, 2, 3);
            comboEncCurveCategory.Name = "comboEncCurveCategory";
            comboEncCurveCategory.Size = new System.Drawing.Size(160, 32);
            comboEncCurveCategory.TabIndex = 18;

            labelEncCurveArrow.AutoSize = true;
            labelEncCurveArrow.Location = new System.Drawing.Point(166, 583);
            labelEncCurveArrow.Margin = new System.Windows.Forms.Padding(2, 7, 2, 0);
            labelEncCurveArrow.Name = "labelEncCurveArrow";
            labelEncCurveArrow.Size = new System.Drawing.Size(22, 24);
            labelEncCurveArrow.TabIndex = 19;
            labelEncCurveArrow.Text = "→";

            comboEncCurve.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            comboEncCurve.FormattingEnabled = true;
            comboEncCurve.Location = new System.Drawing.Point(192, 579);
            comboEncCurve.Margin = new System.Windows.Forms.Padding(0, 3, 4, 3);
            comboEncCurve.Name = "comboEncCurve";
            comboEncCurve.Size = new System.Drawing.Size(260, 32);
            comboEncCurve.TabIndex = 20;

            labelEncInput.AutoSize = true;
            labelEncInput.Location = new System.Drawing.Point(4, 706);
            labelEncInput.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncInput.Name = "labelEncInput";
            labelEncInput.Size = new System.Drawing.Size(154, 12);
            labelEncInput.TabIndex = 11;
            labelEncInput.Text = "明文 / 密文输入：";

            textEncInput.Dock = System.Windows.Forms.DockStyle.Fill;
            textEncInput.Font = new System.Drawing.Font("Consolas", 9F);
            textEncInput.Location = new System.Drawing.Point(4, 725);
            textEncInput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textEncInput.Multiline = true;
            textEncInput.Name = "textEncInput";
            textEncInput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textEncInput.Size = new System.Drawing.Size(3241, 14);
            textEncInput.TabIndex = 12;

            textEncOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            textEncOutput.Font = new System.Drawing.Font("Consolas", 9F);
            textEncOutput.Location = new System.Drawing.Point(4, 765);
            textEncOutput.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            textEncOutput.Multiline = true;
            textEncOutput.Name = "textEncOutput";
            textEncOutput.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            textEncOutput.Size = new System.Drawing.Size(3241, 14);
            textEncOutput.TabIndex = 14;

            labelEncOutputLabel.AutoSize = true;
            labelEncOutputLabel.Location = new System.Drawing.Point(4, 746);
            labelEncOutputLabel.Margin = new System.Windows.Forms.Padding(4, 4, 2, 4);
            labelEncOutputLabel.Name = "labelEncOutputLabel";
            labelEncOutputLabel.Size = new System.Drawing.Size(200, 12);
            labelEncOutputLabel.TabIndex = 13;
            labelEncOutputLabel.Text = "加密结果 / 解密输入：";

            // 加密操作按钮行
            panelEncBtns.Controls.Add(btnEncrypt);
            panelEncBtns.Controls.Add(btnDecrypt);
            panelEncBtns.Controls.Add(btnEncClear);
            panelEncBtns.Controls.Add(btnEncCopy);
            panelEncBtns.Controls.Add(btnEncPaste);
            panelEncBtns.Dock = System.Windows.Forms.DockStyle.Fill;
            panelEncBtns.Location = new System.Drawing.Point(3, 785);
            panelEncBtns.Name = "panelEncBtns";
            panelEncBtns.Padding = new System.Windows.Forms.Padding(0, 4, 0, 0);
            panelEncBtns.Size = new System.Drawing.Size(3243, 14);
            panelEncBtns.TabIndex = 15;
            panelEncBtns.WrapContents = false;

            btnEncrypt.AutoSize = true;
            btnEncrypt.Location = new System.Drawing.Point(3, 7);
            btnEncrypt.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            btnEncrypt.Name = "btnEncrypt";
            btnEncrypt.Size = new System.Drawing.Size(75, 34);
            btnEncrypt.TabIndex = 0;
            btnEncrypt.Text = "加密";
            btnEncrypt.Click += BtnEncrypt_Click;

            btnDecrypt.AutoSize = true;
            btnDecrypt.Location = new System.Drawing.Point(88, 7);
            btnDecrypt.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            btnDecrypt.Name = "btnDecrypt";
            btnDecrypt.Size = new System.Drawing.Size(75, 34);
            btnDecrypt.TabIndex = 1;
            btnDecrypt.Text = "解密";
            btnDecrypt.Click += BtnDecrypt_Click;

            btnEncClear.AutoSize = true;
            btnEncClear.Location = new System.Drawing.Point(175, 7);
            btnEncClear.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            btnEncClear.Name = "btnEncClear";
            btnEncClear.Size = new System.Drawing.Size(75, 34);
            btnEncClear.TabIndex = 2;
            btnEncClear.Text = "清空";
            btnEncClear.Click += BtnEncClear_Click;

            btnEncCopy.AutoSize = true;
            btnEncCopy.Location = new System.Drawing.Point(262, 7);
            btnEncCopy.Margin = new System.Windows.Forms.Padding(6, 3, 6, 3);
            btnEncCopy.Name = "btnEncCopy";
            btnEncCopy.Size = new System.Drawing.Size(92, 34);
            btnEncCopy.TabIndex = 3;
            btnEncCopy.Text = "复制结果";
            btnEncCopy.Click += BtnEncCopy_Click;

            btnEncPaste.AutoSize = true;
            btnEncPaste.Location = new System.Drawing.Point(366, 7);
            btnEncPaste.Margin = new System.Windows.Forms.Padding(6, 3, 4, 3);
            btnEncPaste.Name = "btnEncPaste";
            btnEncPaste.Size = new System.Drawing.Size(92, 34);
            btnEncPaste.TabIndex = 4;
            btnEncPaste.Text = "粘贴输入";
            btnEncPaste.Click += BtnEncPaste_Click;

            // ==================================================
            // 文件签名/验签面板 - groupFile
            // ==================================================
            groupFile.Controls.Add(panelFileControls);
            groupFile.Dock = System.Windows.Forms.DockStyle.Fill;
            groupFile.Location = new System.Drawing.Point(0, 0);
            groupFile.Name = "groupFile";
            groupFile.Padding = new System.Windows.Forms.Padding(8);
            groupFile.Size = new System.Drawing.Size(3265, 841);
            groupFile.TabIndex = 0;
            groupFile.TabStop = false;

            panelFileControls.Controls.Add(btnSignFile);
            panelFileControls.Controls.Add(btnVerifyFile);
            panelFileControls.Dock = System.Windows.Forms.DockStyle.Fill;
            panelFileControls.Location = new System.Drawing.Point(8, 31);
            panelFileControls.Name = "panelFileControls";
            panelFileControls.Padding = new System.Windows.Forms.Padding(4, 2, 4, 2);
            panelFileControls.Size = new System.Drawing.Size(3249, 802);
            panelFileControls.TabIndex = 0;
            panelFileControls.WrapContents = false;

            btnSignFile.AutoSize = true;
            btnSignFile.Location = new System.Drawing.Point(8, 5);
            btnSignFile.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            btnSignFile.Name = "btnSignFile";
            btnSignFile.Size = new System.Drawing.Size(92, 34);
            btnSignFile.TabIndex = 0;
            btnSignFile.Text = "签名文件";
            btnSignFile.Click += BtnSignFile_Click;

            btnVerifyFile.AutoSize = true;
            btnVerifyFile.Location = new System.Drawing.Point(110, 5);
            btnVerifyFile.Margin = new System.Windows.Forms.Padding(6, 3, 4, 3);
            btnVerifyFile.Name = "btnVerifyFile";
            btnVerifyFile.Size = new System.Drawing.Size(92, 34);
            btnVerifyFile.TabIndex = 1;
            btnVerifyFile.Text = "验签文件";
            btnVerifyFile.Click += BtnVerifyFile_Click;

            // ==================================================
            // ECDH 密钥协商面板 - groupEcdh
            // ==================================================
            groupEcdh.Dock = System.Windows.Forms.DockStyle.Fill;
            groupEcdh.Location = new System.Drawing.Point(0, 0);
            groupEcdh.Name = "groupEcdh";
            groupEcdh.Padding = new System.Windows.Forms.Padding(8);
            groupEcdh.Size = new System.Drawing.Size(3265, 841);
            groupEcdh.TabIndex = 0;

            labelEncResult.BackColor = System.Drawing.Color.White;
            labelEncResult.BorderStyle = System.Windows.Forms.BorderStyle.None;
            labelEncResult.Font = new System.Drawing.Font("Segoe UI", 9F);
            labelEncResult.ForeColor = System.Drawing.Color.Gray;
            labelEncResult.Location = new System.Drawing.Point(3, 39);
            labelEncResult.Multiline = true;
            labelEncResult.Name = "labelEncResult";
            labelEncResult.ReadOnly = true;
            labelEncResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            labelEncResult.Size = new System.Drawing.Size(364, 338);
            labelEncResult.TabIndex = 0;
            labelEncResult.Text = "加密结果:\r\n等待操作...";

            // ==================================================
            // 分隔器 - splitSignEncrypt / splitFileResult
            // ==================================================
            splitSignEncrypt.Dock = System.Windows.Forms.DockStyle.Fill;
            splitSignEncrypt.Location = new System.Drawing.Point(11, 710);
            splitSignEncrypt.Name = "splitSignEncrypt";
            splitSignEncrypt.Size = new System.Drawing.Size(3265, 460);
            splitSignEncrypt.SplitterDistance = 1766;
            splitSignEncrypt.TabIndex = 2;

            // 文件加密/解密子面板
            groupEncFile.Controls.Add(panelEncFileBtns);
            groupEncFile.Dock = System.Windows.Forms.DockStyle.Fill;
            groupEncFile.Location = new System.Drawing.Point(1112, 403);
            groupEncFile.Name = "groupEncFile";
            groupEncFile.Padding = new System.Windows.Forms.Padding(8);
            groupEncFile.Size = new System.Drawing.Size(364, 15);
            groupEncFile.TabIndex = 16;
            groupEncFile.TabStop = false;
            groupEncFile.Text = "文件加密/解密";

            panelEncFileBtns.Controls.Add(btnEncryptFile);
            panelEncFileBtns.Controls.Add(btnDecryptFile);
            panelEncFileBtns.Dock = System.Windows.Forms.DockStyle.Fill;
            panelEncFileBtns.Location = new System.Drawing.Point(8, 31);
            panelEncFileBtns.Name = "panelEncFileBtns";
            panelEncFileBtns.Size = new System.Drawing.Size(348, 0);
            panelEncFileBtns.TabIndex = 0;
            panelEncFileBtns.WrapContents = false;

            btnEncryptFile.AutoSize = true;
            btnEncryptFile.Location = new System.Drawing.Point(3, 3);
            btnEncryptFile.Margin = new System.Windows.Forms.Padding(3, 3, 4, 3);
            btnEncryptFile.Name = "btnEncryptFile";
            btnEncryptFile.Size = new System.Drawing.Size(92, 34);
            btnEncryptFile.TabIndex = 0;
            btnEncryptFile.Text = "加密文件";
            btnEncryptFile.Click += BtnEncryptFile_Click;

            btnDecryptFile.AutoSize = true;
            btnDecryptFile.Location = new System.Drawing.Point(105, 3);
            btnDecryptFile.Margin = new System.Windows.Forms.Padding(6, 3, 4, 3);
            btnDecryptFile.Name = "btnDecryptFile";
            btnDecryptFile.Size = new System.Drawing.Size(92, 34);
            btnDecryptFile.TabIndex = 1;
            btnDecryptFile.Text = "解密文件";
            btnDecryptFile.Click += BtnDecryptFile_Click;

            splitFileResult.Dock = System.Windows.Forms.DockStyle.Fill;
            splitFileResult.Location = new System.Drawing.Point(11, 1176);
            splitFileResult.Name = "splitFileResult";
            splitFileResult.Size = new System.Drawing.Size(3265, 429);
            splitFileResult.SplitterDistance = 2633;
            splitFileResult.TabIndex = 3;

            // ==================================================
            // EcdsaTabControl 自身
            // ==================================================
            AutoScaleDimensions = new System.Drawing.SizeF(11F, 24F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            Controls.Add(mainTableLayout);
            Name = "EcdsaTabControl";
            Size = new System.Drawing.Size(3287, 1616);

            // ===== ResumeLayout 区域 =====
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
    }
}
