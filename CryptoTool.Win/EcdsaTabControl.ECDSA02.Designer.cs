#nullable disable

using System;
using System.Windows.Forms;

namespace CryptoTool.Win
{
    public partial class EcdsaTabControl : UserControl
    {
        #region 签名验签区字段
        // [签名验签区 - groupSign]
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
        private TableLayoutPanel panelSignOptions;
        #endregion

        /// <summary>
        /// 初始化签名验签面板布局。
        /// 这部分原位于 EcdsaTabControl.Designer.cs 的 InitializeComponent 中，
        /// 拆分到本文件以隔离 ECDSA 签名/验签相关界面代码。
        /// </summary>
        private void InitializeSignLayout()
        {
            // 实例化签名验签区控件
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
            // 挂起布局
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
            panelViewContent.Controls.Add(groupSign);

            // 恢复布局
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
        }
    }
}
