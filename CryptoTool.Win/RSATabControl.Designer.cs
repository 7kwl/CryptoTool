namespace CryptoTool.Win
{
    partial class RSATabControl : UserControl
    {
        /// <summary> 
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region 组件设计器生成的代码

        /// <summary> 
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            mainTableLayout = new TableLayoutPanel();
            groupBoxRSAKeys = new GroupBox();
            tableLayoutRSAKeys = new TableLayoutPanel();
            panelRSAKeyControls = new Panel();
            btnExportRSAKey = new Button();
            btnImportRSAKey = new Button();
            btnGenerateRSAKey = new Button();
            comboRSAKeyOutputFormat = new ComboBox();
            label26 = new Label();
            comboRSAKeyFormat = new ComboBox();
            label2 = new Label();
            comboRSAKeySize = new ComboBox();
            label1 = new Label();
            label3 = new Label();
            textRSAPublicKey = new TextBox();
            label4 = new Label();
            textRSAPrivateKey = new TextBox();
            groupBoxRSAEncrypt = new GroupBox();
            tableLayoutRSAEncrypt = new TableLayoutPanel();
            panelRSAEncryptControls = new Panel();
            btnRSADecrypt = new Button();
            btnRSAEncrypt = new Button();
            comboRSAEncryptOutputFormat = new ComboBox();
            label27 = new Label();
            comboRSAKeyPadding = new ComboBox();
            label25 = new Label();
            label5 = new Label();
            textRSAPlainText = new TextBox();
            label6 = new Label();
            textRSACipherText = new TextBox();
            groupBoxRSASign = new GroupBox();
            tableLayoutRSASign = new TableLayoutPanel();
            panelRSASignControls = new Panel();
            btnRSAVerify = new Button();
            btnRSASign = new Button();
            comboRSASignOutputFormat = new ComboBox();
            label28 = new Label();
            comboRSASignAlgmFormat = new ComboBox();
            label29 = new Label();
            label7 = new Label();
            textRSASignData = new TextBox();
            label8 = new Label();
            textRSASignature = new TextBox();
            labelRSAVerifyResult = new Label();
            mainTableLayout.SuspendLayout();
            groupBoxRSAKeys.SuspendLayout();
            tableLayoutRSAKeys.SuspendLayout();
            panelRSAKeyControls.SuspendLayout();
            groupBoxRSAEncrypt.SuspendLayout();
            tableLayoutRSAEncrypt.SuspendLayout();
            panelRSAEncryptControls.SuspendLayout();
            groupBoxRSASign.SuspendLayout();
            tableLayoutRSASign.SuspendLayout();
            panelRSASignControls.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 1;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayout.Controls.Add(groupBoxRSAKeys, 0, 0);
            mainTableLayout.Controls.Add(groupBoxRSAEncrypt, 0, 1);
            mainTableLayout.Controls.Add(groupBoxRSASign, 0, 2);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Margin = new Padding(5);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(10);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 40F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            mainTableLayout.Size = new Size(1562, 998);
            mainTableLayout.TabIndex = 0;
            // 
            // groupBoxRSAKeys
            // 
            groupBoxRSAKeys.Controls.Add(tableLayoutRSAKeys);
            groupBoxRSAKeys.Dock = DockStyle.Fill;
            groupBoxRSAKeys.Location = new Point(15, 15);
            groupBoxRSAKeys.Margin = new Padding(5);
            groupBoxRSAKeys.Name = "groupBoxRSAKeys";
            groupBoxRSAKeys.Padding = new Padding(10);
            groupBoxRSAKeys.Size = new Size(1532, 381);
            groupBoxRSAKeys.TabIndex = 0;
            groupBoxRSAKeys.TabStop = false;
            groupBoxRSAKeys.Text = "RSA密钥生成";
            // 
            // tableLayoutRSAKeys
            // 
            tableLayoutRSAKeys.ColumnCount = 1;
            tableLayoutRSAKeys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutRSAKeys.Controls.Add(panelRSAKeyControls, 0, 0);
            tableLayoutRSAKeys.Controls.Add(label3, 0, 1);
            tableLayoutRSAKeys.Controls.Add(textRSAPublicKey, 0, 2);
            tableLayoutRSAKeys.Controls.Add(label4, 0, 3);
            tableLayoutRSAKeys.Controls.Add(textRSAPrivateKey, 0, 4);
            tableLayoutRSAKeys.Dock = DockStyle.Fill;
            tableLayoutRSAKeys.Location = new Point(10, 33);
            tableLayoutRSAKeys.Margin = new Padding(4);
            tableLayoutRSAKeys.Name = "tableLayoutRSAKeys";
            tableLayoutRSAKeys.RowCount = 5;
            tableLayoutRSAKeys.RowStyles.Add(new RowStyle(SizeType.Absolute, 60F));
            tableLayoutRSAKeys.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutRSAKeys.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutRSAKeys.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutRSAKeys.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutRSAKeys.Size = new Size(1512, 338);
            tableLayoutRSAKeys.TabIndex = 0;
            // 
            // panelRSAKeyControls
            // 
            panelRSAKeyControls.Controls.Add(btnExportRSAKey);
            panelRSAKeyControls.Controls.Add(btnImportRSAKey);
            panelRSAKeyControls.Controls.Add(btnGenerateRSAKey);
            panelRSAKeyControls.Controls.Add(comboRSAKeyOutputFormat);
            panelRSAKeyControls.Controls.Add(label26);
            panelRSAKeyControls.Controls.Add(comboRSAKeyFormat);
            panelRSAKeyControls.Controls.Add(label2);
            panelRSAKeyControls.Controls.Add(comboRSAKeySize);
            panelRSAKeyControls.Controls.Add(label1);
            panelRSAKeyControls.Dock = DockStyle.Fill;
            panelRSAKeyControls.Location = new Point(4, 4);
            panelRSAKeyControls.Margin = new Padding(4);
            panelRSAKeyControls.Name = "panelRSAKeyControls";
            panelRSAKeyControls.Size = new Size(1504, 52);
            panelRSAKeyControls.TabIndex = 0;
            // 
            // btnExportRSAKey
            // 
            btnExportRSAKey.Location = new Point(1253, 2);
            btnExportRSAKey.Margin = new Padding(5);
            btnExportRSAKey.Name = "btnExportRSAKey";
            btnExportRSAKey.Size = new Size(126, 38);
            btnExportRSAKey.TabIndex = 10;
            btnExportRSAKey.Text = "导出密钥";
            btnExportRSAKey.UseVisualStyleBackColor = true;
            btnExportRSAKey.Click += btnExportRSAKey_Click;
            // 
            // btnImportRSAKey
            // 
            btnImportRSAKey.Location = new Point(1112, 2);
            btnImportRSAKey.Margin = new Padding(5);
            btnImportRSAKey.Name = "btnImportRSAKey";
            btnImportRSAKey.Size = new Size(126, 38);
            btnImportRSAKey.TabIndex = 9;
            btnImportRSAKey.Text = "导入密钥";
            btnImportRSAKey.UseVisualStyleBackColor = true;
            btnImportRSAKey.Click += btnImportRSAKey_Click;
            // 
            // btnGenerateRSAKey
            // 
            btnGenerateRSAKey.Location = new Point(941, 2);
            btnGenerateRSAKey.Margin = new Padding(5);
            btnGenerateRSAKey.Name = "btnGenerateRSAKey";
            btnGenerateRSAKey.Size = new Size(158, 38);
            btnGenerateRSAKey.TabIndex = 0;
            btnGenerateRSAKey.Text = "生成密钥对";
            btnGenerateRSAKey.UseVisualStyleBackColor = true;
            btnGenerateRSAKey.Click += btnGenerateRSAKey_Click;
            // 
            // comboRSAKeyOutputFormat
            // 
            comboRSAKeyOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSAKeyOutputFormat.FormattingEnabled = true;
            comboRSAKeyOutputFormat.Items.AddRange(new object[] { "PEM", "Base64", "Hex" });
            comboRSAKeyOutputFormat.Location = new Point(703, 5);
            comboRSAKeyOutputFormat.Margin = new Padding(5);
            comboRSAKeyOutputFormat.Name = "comboRSAKeyOutputFormat";
            comboRSAKeyOutputFormat.Size = new Size(211, 32);
            comboRSAKeyOutputFormat.TabIndex = 14;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(605, 10);
            label26.Margin = new Padding(5, 0, 5, 0);
            label26.Name = "label26";
            label26.Size = new Size(86, 24);
            label26.TabIndex = 13;
            label26.Text = "密钥格式:";
            // 
            // comboRSAKeyFormat
            // 
            comboRSAKeyFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSAKeyFormat.FormattingEnabled = true;
            comboRSAKeyFormat.Items.AddRange(new object[] { "PKCS1", "PKCS8" });
            comboRSAKeyFormat.Location = new Point(373, 5);
            comboRSAKeyFormat.Margin = new Padding(5);
            comboRSAKeyFormat.Name = "comboRSAKeyFormat";
            comboRSAKeyFormat.Size = new Size(211, 32);
            comboRSAKeyFormat.TabIndex = 3;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(275, 10);
            label2.Margin = new Padding(5, 0, 5, 0);
            label2.Name = "label2";
            label2.Size = new Size(86, 24);
            label2.TabIndex = 4;
            label2.Text = "密钥类型:";
            // 
            // comboRSAKeySize
            // 
            comboRSAKeySize.DisplayMember = "Text";
            comboRSAKeySize.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSAKeySize.FormattingEnabled = true;
            comboRSAKeySize.Items.AddRange(new object[] { "1024", "2048", "4096" });
            comboRSAKeySize.Location = new Point(98, 5);
            comboRSAKeySize.Margin = new Padding(5);
            comboRSAKeySize.Name = "comboRSAKeySize";
            comboRSAKeySize.Size = new Size(154, 32);
            comboRSAKeySize.TabIndex = 1;
            comboRSAKeySize.ValueMember = "Value";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(0, 10);
            label1.Margin = new Padding(5, 0, 5, 0);
            label1.Name = "label1";
            label1.Size = new Size(86, 24);
            label1.TabIndex = 2;
            label1.Text = "密钥长度:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Dock = DockStyle.Bottom;
            label3.Location = new Point(5, 66);
            label3.Margin = new Padding(5, 0, 5, 0);
            label3.Name = "label3";
            label3.Size = new Size(1502, 24);
            label3.TabIndex = 6;
            label3.Text = "公钥:";
            // 
            // textRSAPublicKey
            // 
            textRSAPublicKey.Dock = DockStyle.Fill;
            textRSAPublicKey.Location = new Point(5, 95);
            textRSAPublicKey.Margin = new Padding(5);
            textRSAPublicKey.Multiline = true;
            textRSAPublicKey.Name = "textRSAPublicKey";
            textRSAPublicKey.ScrollBars = ScrollBars.Both;
            textRSAPublicKey.Size = new Size(1502, 99);
            textRSAPublicKey.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Dock = DockStyle.Bottom;
            label4.Location = new Point(5, 205);
            label4.Margin = new Padding(5, 0, 5, 0);
            label4.Name = "label4";
            label4.Size = new Size(1502, 24);
            label4.TabIndex = 8;
            label4.Text = "私钥:";
            // 
            // textRSAPrivateKey
            // 
            textRSAPrivateKey.Dock = DockStyle.Fill;
            textRSAPrivateKey.Location = new Point(5, 234);
            textRSAPrivateKey.Margin = new Padding(5);
            textRSAPrivateKey.Multiline = true;
            textRSAPrivateKey.Name = "textRSAPrivateKey";
            textRSAPrivateKey.ScrollBars = ScrollBars.Both;
            textRSAPrivateKey.Size = new Size(1502, 99);
            textRSAPrivateKey.TabIndex = 7;
            // 
            // groupBoxRSAEncrypt
            // 
            groupBoxRSAEncrypt.Controls.Add(tableLayoutRSAEncrypt);
            groupBoxRSAEncrypt.Dock = DockStyle.Fill;
            groupBoxRSAEncrypt.Location = new Point(15, 406);
            groupBoxRSAEncrypt.Margin = new Padding(5);
            groupBoxRSAEncrypt.Name = "groupBoxRSAEncrypt";
            groupBoxRSAEncrypt.Padding = new Padding(10);
            groupBoxRSAEncrypt.Size = new Size(1532, 283);
            groupBoxRSAEncrypt.TabIndex = 1;
            groupBoxRSAEncrypt.TabStop = false;
            groupBoxRSAEncrypt.Text = "RSA加密解密";
            // 
            // tableLayoutRSAEncrypt
            // 
            tableLayoutRSAEncrypt.ColumnCount = 1;
            tableLayoutRSAEncrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutRSAEncrypt.Controls.Add(panelRSAEncryptControls, 0, 0);
            tableLayoutRSAEncrypt.Controls.Add(label5, 0, 1);
            tableLayoutRSAEncrypt.Controls.Add(textRSAPlainText, 0, 2);
            tableLayoutRSAEncrypt.Controls.Add(label6, 0, 3);
            tableLayoutRSAEncrypt.Controls.Add(textRSACipherText, 0, 4);
            tableLayoutRSAEncrypt.Dock = DockStyle.Fill;
            tableLayoutRSAEncrypt.Location = new Point(10, 33);
            tableLayoutRSAEncrypt.Margin = new Padding(4);
            tableLayoutRSAEncrypt.Name = "tableLayoutRSAEncrypt";
            tableLayoutRSAEncrypt.RowCount = 5;
            tableLayoutRSAEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutRSAEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutRSAEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutRSAEncrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutRSAEncrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutRSAEncrypt.Size = new Size(1512, 240);
            tableLayoutRSAEncrypt.TabIndex = 0;
            // 
            // panelRSAEncryptControls
            // 
            panelRSAEncryptControls.Controls.Add(btnRSADecrypt);
            panelRSAEncryptControls.Controls.Add(btnRSAEncrypt);
            panelRSAEncryptControls.Controls.Add(comboRSAEncryptOutputFormat);
            panelRSAEncryptControls.Controls.Add(label27);
            panelRSAEncryptControls.Controls.Add(comboRSAKeyPadding);
            panelRSAEncryptControls.Controls.Add(label25);
            panelRSAEncryptControls.Dock = DockStyle.Fill;
            panelRSAEncryptControls.Location = new Point(4, 4);
            panelRSAEncryptControls.Margin = new Padding(4);
            panelRSAEncryptControls.Name = "panelRSAEncryptControls";
            panelRSAEncryptControls.Size = new Size(1504, 40);
            panelRSAEncryptControls.TabIndex = 0;
            // 
            // btnRSADecrypt
            // 
            btnRSADecrypt.Location = new Point(1378, 1);
            btnRSADecrypt.Margin = new Padding(5);
            btnRSADecrypt.Name = "btnRSADecrypt";
            btnRSADecrypt.Size = new Size(126, 36);
            btnRSADecrypt.TabIndex = 1;
            btnRSADecrypt.Text = "解密";
            btnRSADecrypt.UseVisualStyleBackColor = true;
            btnRSADecrypt.Click += btnRSADecrypt_Click;
            // 
            // btnRSAEncrypt
            // 
            btnRSAEncrypt.Location = new Point(1112, 0);
            btnRSAEncrypt.Margin = new Padding(5);
            btnRSAEncrypt.Name = "btnRSAEncrypt";
            btnRSAEncrypt.Size = new Size(126, 36);
            btnRSAEncrypt.TabIndex = 0;
            btnRSAEncrypt.Text = "加密";
            btnRSAEncrypt.UseVisualStyleBackColor = true;
            btnRSAEncrypt.Click += btnRSAEncrypt_Click;
            // 
            // comboRSAEncryptOutputFormat
            // 
            comboRSAEncryptOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSAEncryptOutputFormat.FormattingEnabled = true;
            comboRSAEncryptOutputFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboRSAEncryptOutputFormat.Location = new Point(703, 7);
            comboRSAEncryptOutputFormat.Margin = new Padding(5);
            comboRSAEncryptOutputFormat.Name = "comboRSAEncryptOutputFormat";
            comboRSAEncryptOutputFormat.Size = new Size(211, 32);
            comboRSAEncryptOutputFormat.TabIndex = 15;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(605, 10);
            label27.Margin = new Padding(5, 0, 5, 0);
            label27.Name = "label27";
            label27.Size = new Size(86, 24);
            label27.TabIndex = 14;
            label27.Text = "密文格式:";
            // 
            // comboRSAKeyPadding
            // 
            comboRSAKeyPadding.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSAKeyPadding.FormattingEnabled = true;
            comboRSAKeyPadding.Items.AddRange(new object[] { "PKCS1", "OAEP" });
            comboRSAKeyPadding.Location = new Point(98, 5);
            comboRSAKeyPadding.Margin = new Padding(5);
            comboRSAKeyPadding.Name = "comboRSAKeyPadding";
            comboRSAKeyPadding.Size = new Size(211, 32);
            comboRSAKeyPadding.TabIndex = 12;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(0, 10);
            label25.Margin = new Padding(5, 0, 5, 0);
            label25.Name = "label25";
            label25.Size = new Size(86, 24);
            label25.TabIndex = 11;
            label25.Text = "填充方式:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Dock = DockStyle.Bottom;
            label5.Location = new Point(5, 54);
            label5.Margin = new Padding(5, 0, 5, 0);
            label5.Name = "label5";
            label5.Size = new Size(1502, 24);
            label5.TabIndex = 3;
            label5.Text = "明文:";
            // 
            // textRSAPlainText
            // 
            textRSAPlainText.Dock = DockStyle.Fill;
            textRSAPlainText.Location = new Point(5, 83);
            textRSAPlainText.Margin = new Padding(5);
            textRSAPlainText.Multiline = true;
            textRSAPlainText.Name = "textRSAPlainText";
            textRSAPlainText.ScrollBars = ScrollBars.Both;
            textRSAPlainText.Size = new Size(1502, 56);
            textRSAPlainText.TabIndex = 2;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Dock = DockStyle.Bottom;
            label6.Location = new Point(5, 150);
            label6.Margin = new Padding(5, 0, 5, 0);
            label6.Name = "label6";
            label6.Size = new Size(1502, 24);
            label6.TabIndex = 5;
            label6.Text = "密文:";
            // 
            // textRSACipherText
            // 
            textRSACipherText.Dock = DockStyle.Fill;
            textRSACipherText.Location = new Point(5, 179);
            textRSACipherText.Margin = new Padding(5);
            textRSACipherText.Multiline = true;
            textRSACipherText.Name = "textRSACipherText";
            textRSACipherText.ScrollBars = ScrollBars.Both;
            textRSACipherText.Size = new Size(1502, 56);
            textRSACipherText.TabIndex = 4;
            // 
            // groupBoxRSASign
            // 
            groupBoxRSASign.Controls.Add(tableLayoutRSASign);
            groupBoxRSASign.Dock = DockStyle.Fill;
            groupBoxRSASign.Location = new Point(15, 699);
            groupBoxRSASign.Margin = new Padding(5);
            groupBoxRSASign.Name = "groupBoxRSASign";
            groupBoxRSASign.Padding = new Padding(10);
            groupBoxRSASign.Size = new Size(1532, 284);
            groupBoxRSASign.TabIndex = 2;
            groupBoxRSASign.TabStop = false;
            groupBoxRSASign.Text = "RSA数字签名";
            // 
            // tableLayoutRSASign
            // 
            tableLayoutRSASign.ColumnCount = 1;
            tableLayoutRSASign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutRSASign.Controls.Add(panelRSASignControls, 0, 0);
            tableLayoutRSASign.Controls.Add(label7, 0, 1);
            tableLayoutRSASign.Controls.Add(textRSASignData, 0, 2);
            tableLayoutRSASign.Controls.Add(label8, 0, 3);
            tableLayoutRSASign.Controls.Add(textRSASignature, 0, 4);
            tableLayoutRSASign.Controls.Add(labelRSAVerifyResult, 0, 5);
            tableLayoutRSASign.Dock = DockStyle.Fill;
            tableLayoutRSASign.Location = new Point(10, 33);
            tableLayoutRSASign.Margin = new Padding(4);
            tableLayoutRSASign.Name = "tableLayoutRSASign";
            tableLayoutRSASign.RowCount = 6;
            tableLayoutRSASign.RowStyles.Add(new RowStyle(SizeType.Absolute, 48F));
            tableLayoutRSASign.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutRSASign.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutRSASign.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
            tableLayoutRSASign.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            tableLayoutRSASign.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutRSASign.Size = new Size(1512, 241);
            tableLayoutRSASign.TabIndex = 0;
            // 
            // panelRSASignControls
            // 
            panelRSASignControls.Controls.Add(btnRSAVerify);
            panelRSASignControls.Controls.Add(btnRSASign);
            panelRSASignControls.Controls.Add(comboRSASignOutputFormat);
            panelRSASignControls.Controls.Add(label28);
            panelRSASignControls.Controls.Add(comboRSASignAlgmFormat);
            panelRSASignControls.Controls.Add(label29);
            panelRSASignControls.Dock = DockStyle.Fill;
            panelRSASignControls.Location = new Point(4, 4);
            panelRSASignControls.Margin = new Padding(4);
            panelRSASignControls.Name = "panelRSASignControls";
            panelRSASignControls.Size = new Size(1504, 40);
            panelRSASignControls.TabIndex = 0;
            // 
            // btnRSAVerify
            // 
            btnRSAVerify.Location = new Point(1253, 1);
            btnRSAVerify.Margin = new Padding(5);
            btnRSAVerify.Name = "btnRSAVerify";
            btnRSAVerify.Size = new Size(126, 36);
            btnRSAVerify.TabIndex = 1;
            btnRSAVerify.Text = "验签";
            btnRSAVerify.UseVisualStyleBackColor = true;
            btnRSAVerify.Click += btnRSAVerify_Click;
            // 
            // btnRSASign
            // 
            btnRSASign.Location = new Point(1112, 0);
            btnRSASign.Margin = new Padding(5);
            btnRSASign.Name = "btnRSASign";
            btnRSASign.Size = new Size(126, 36);
            btnRSASign.TabIndex = 0;
            btnRSASign.Text = "签名";
            btnRSASign.UseVisualStyleBackColor = true;
            btnRSASign.Click += btnRSASign_Click;
            // 
            // comboRSASignOutputFormat
            // 
            comboRSASignOutputFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSASignOutputFormat.FormattingEnabled = true;
            comboRSASignOutputFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboRSASignOutputFormat.Location = new Point(464, 5);
            comboRSASignOutputFormat.Margin = new Padding(5);
            comboRSASignOutputFormat.Name = "comboRSASignOutputFormat";
            comboRSASignOutputFormat.Size = new Size(211, 32);
            comboRSASignOutputFormat.TabIndex = 16;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(367, 10);
            label28.Margin = new Padding(5, 0, 5, 0);
            label28.Name = "label28";
            label28.Size = new Size(86, 24);
            label28.TabIndex = 15;
            label28.Text = "签名格式:";
            // 
            // comboRSASignAlgmFormat
            // 
            comboRSASignAlgmFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboRSASignAlgmFormat.FormattingEnabled = true;
            comboRSASignAlgmFormat.Items.AddRange(new object[] { "SHA1withRSA(RSA1)", "SHA256withRSA(RSA2)", "SHA384withRSA", "SHA512withRSA", "MD5withRSA" });
            comboRSASignAlgmFormat.Location = new Point(98, 5);
            comboRSASignAlgmFormat.Margin = new Padding(5);
            comboRSASignAlgmFormat.Name = "comboRSASignAlgmFormat";
            comboRSASignAlgmFormat.Size = new Size(247, 32);
            comboRSASignAlgmFormat.TabIndex = 18;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(0, 10);
            label29.Margin = new Padding(5, 0, 5, 0);
            label29.Name = "label29";
            label29.Size = new Size(86, 24);
            label29.TabIndex = 17;
            label29.Text = "签名算法:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Dock = DockStyle.Bottom;
            label7.Location = new Point(5, 54);
            label7.Margin = new Padding(5, 0, 5, 0);
            label7.Name = "label7";
            label7.Size = new Size(1502, 24);
            label7.TabIndex = 3;
            label7.Text = "原文数据:";
            // 
            // textRSASignData
            // 
            textRSASignData.Dock = DockStyle.Fill;
            textRSASignData.Location = new Point(5, 83);
            textRSASignData.Margin = new Padding(5);
            textRSASignData.Multiline = true;
            textRSASignData.Name = "textRSASignData";
            textRSASignData.ScrollBars = ScrollBars.Both;
            textRSASignData.Size = new Size(1502, 49);
            textRSASignData.TabIndex = 2;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Dock = DockStyle.Bottom;
            label8.Location = new Point(5, 143);
            label8.Margin = new Padding(5, 0, 5, 0);
            label8.Name = "label8";
            label8.Size = new Size(1502, 24);
            label8.TabIndex = 5;
            label8.Text = "签名:";
            // 
            // textRSASignature
            // 
            textRSASignature.Dock = DockStyle.Fill;
            textRSASignature.Location = new Point(5, 172);
            textRSASignature.Margin = new Padding(5);
            textRSASignature.Multiline = true;
            textRSASignature.Name = "textRSASignature";
            textRSASignature.ScrollBars = ScrollBars.Both;
            textRSASignature.Size = new Size(1502, 49);
            textRSASignature.TabIndex = 4;
            // 
            // labelRSAVerifyResult
            // 
            labelRSAVerifyResult.AutoSize = true;
            labelRSAVerifyResult.Dock = DockStyle.Fill;
            labelRSAVerifyResult.Location = new Point(5, 226);
            labelRSAVerifyResult.Margin = new Padding(5, 0, 5, 0);
            labelRSAVerifyResult.Name = "labelRSAVerifyResult";
            labelRSAVerifyResult.Size = new Size(1502, 15);
            labelRSAVerifyResult.TabIndex = 8;
            labelRSAVerifyResult.Text = "验签结果:";
            labelRSAVerifyResult.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // RSATabControl
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainTableLayout);
            Margin = new Padding(5);
            Name = "RSATabControl";
            Size = new Size(1562, 998);
            mainTableLayout.ResumeLayout(false);
            groupBoxRSAKeys.ResumeLayout(false);
            tableLayoutRSAKeys.ResumeLayout(false);
            tableLayoutRSAKeys.PerformLayout();
            panelRSAKeyControls.ResumeLayout(false);
            panelRSAKeyControls.PerformLayout();
            groupBoxRSAEncrypt.ResumeLayout(false);
            tableLayoutRSAEncrypt.ResumeLayout(false);
            tableLayoutRSAEncrypt.PerformLayout();
            panelRSAEncryptControls.ResumeLayout(false);
            panelRSAEncryptControls.PerformLayout();
            groupBoxRSASign.ResumeLayout(false);
            tableLayoutRSASign.ResumeLayout(false);
            tableLayoutRSASign.PerformLayout();
            panelRSASignControls.ResumeLayout(false);
            panelRSASignControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainTableLayout;
        private GroupBox groupBoxRSAKeys;
        private TableLayoutPanel tableLayoutRSAKeys;
        private Panel panelRSAKeyControls;
        private Label label1;
        private ComboBox comboRSAKeySize;
        private Label label2;
        private ComboBox comboRSAKeyFormat;
        private Label label26;
        private ComboBox comboRSAKeyOutputFormat;
        private Button btnGenerateRSAKey;
        private Button btnImportRSAKey;
        private Button btnExportRSAKey;
        private Label label3;

        private TextBox textRSAPublicKey;
        private Label label4;
        private TextBox textRSAPrivateKey;
        private GroupBox groupBoxRSAEncrypt;
        private TableLayoutPanel tableLayoutRSAEncrypt;
        private Panel panelRSAEncryptControls;
        private Label label25;
        private ComboBox comboRSAKeyPadding;
        private Label label27;
        private ComboBox comboRSAEncryptOutputFormat;
        private Button btnRSAEncrypt;
        private Button btnRSADecrypt;
        private Label label5;
        private TextBox textRSAPlainText;
        private Label label6;
        private TextBox textRSACipherText;
        private GroupBox groupBoxRSASign;
        private TableLayoutPanel tableLayoutRSASign;
        private Panel panelRSASignControls;
        private Label label29;
        private ComboBox comboRSASignAlgmFormat;
        private Label label28;
        private ComboBox comboRSASignOutputFormat;
        private Button btnRSASign;
        private Button btnRSAVerify;
        private Label label7;
        private TextBox textRSASignData;
        private Label label8;
        private TextBox textRSASignature;
        private Label labelRSAVerifyResult;
    }
}
