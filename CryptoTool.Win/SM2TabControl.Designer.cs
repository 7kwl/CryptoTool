namespace CryptoTool.Win
{
    partial class SM2TabControl : UserControl
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
            groupBoxSM2Keys = new GroupBox();
            tableLayoutSM2Keys = new TableLayoutPanel();
            panelSM2KeyControls = new Panel();
            label16 = new Label();
            comboSM2KeyFormat = new ComboBox();
            btnGenerateSM2Key = new Button();
            btnImportSM2Key = new Button();
            btnExportSM2Key = new Button();
            label17 = new Label();
            textSM2PublicKey = new TextBox();
            label18 = new Label();
            textSM2PrivateKey = new TextBox();
            groupBoxSM2Encrypt = new GroupBox();
            tableLayoutSM2Encrypt = new TableLayoutPanel();
            panelSM2EncryptControls = new Panel();
            label19 = new Label();
            comboSM2CipherFormat = new ComboBox();
            btnSM2Encrypt = new Button();
            btnSM2Decrypt = new Button();
            label20 = new Label();
            textSM2PlainText = new TextBox();
            label21 = new Label();
            textSM2CipherText = new TextBox();
            groupBoxSM2Sign = new GroupBox();
            tableLayoutSM2Sign = new TableLayoutPanel();
            panelSM2SignControls = new Panel();
            label22 = new Label();
            comboSM2SignFormat = new ComboBox();
            btnSM2Sign = new Button();
            btnSM2Verify = new Button();
            label23 = new Label();
            textSM2SignData = new TextBox();
            label24 = new Label();
            textSM2Signature = new TextBox();
            labelSM2VerifyResult = new Label();
            mainTableLayout.SuspendLayout();
            groupBoxSM2Keys.SuspendLayout();
            tableLayoutSM2Keys.SuspendLayout();
            panelSM2KeyControls.SuspendLayout();
            groupBoxSM2Encrypt.SuspendLayout();
            tableLayoutSM2Encrypt.SuspendLayout();
            panelSM2EncryptControls.SuspendLayout();
            groupBoxSM2Sign.SuspendLayout();
            tableLayoutSM2Sign.SuspendLayout();
            panelSM2SignControls.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 1;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayout.Controls.Add(groupBoxSM2Keys, 0, 0);
            mainTableLayout.Controls.Add(groupBoxSM2Encrypt, 0, 1);
            mainTableLayout.Controls.Add(groupBoxSM2Sign, 0, 2);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Margin = new Padding(4);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(8);
            mainTableLayout.RowCount = 3;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 25F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 45F));
            mainTableLayout.Size = new Size(1278, 850);
            mainTableLayout.TabIndex = 0;
            // 
            // groupBoxSM2Keys
            // 
            groupBoxSM2Keys.Controls.Add(tableLayoutSM2Keys);
            groupBoxSM2Keys.Dock = DockStyle.Fill;
            groupBoxSM2Keys.Location = new Point(12, 12);
            groupBoxSM2Keys.Margin = new Padding(4);
            groupBoxSM2Keys.Name = "groupBoxSM2Keys";
            groupBoxSM2Keys.Padding = new Padding(8);
            groupBoxSM2Keys.Size = new Size(1254, 242);
            groupBoxSM2Keys.TabIndex = 0;
            groupBoxSM2Keys.TabStop = false;
            groupBoxSM2Keys.Text = "SM2密钥生成";
            // 
            // tableLayoutSM2Keys
            // 
            tableLayoutSM2Keys.ColumnCount = 1;
            tableLayoutSM2Keys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutSM2Keys.Controls.Add(panelSM2KeyControls, 0, 0);
            tableLayoutSM2Keys.Controls.Add(label17, 0, 1);
            tableLayoutSM2Keys.Controls.Add(textSM2PublicKey, 0, 2);
            tableLayoutSM2Keys.Controls.Add(label18, 0, 3);
            tableLayoutSM2Keys.Controls.Add(textSM2PrivateKey, 0, 4);
            tableLayoutSM2Keys.Dock = DockStyle.Fill;
            tableLayoutSM2Keys.Location = new Point(8, 28);
            tableLayoutSM2Keys.Name = "tableLayoutSM2Keys";
            tableLayoutSM2Keys.RowCount = 5;
            tableLayoutSM2Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutSM2Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM2Keys.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutSM2Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM2Keys.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutSM2Keys.Size = new Size(1238, 206);
            tableLayoutSM2Keys.TabIndex = 0;
            // 
            // panelSM2KeyControls
            // 
            panelSM2KeyControls.Controls.Add(btnExportSM2Key);
            panelSM2KeyControls.Controls.Add(btnImportSM2Key);
            panelSM2KeyControls.Controls.Add(btnGenerateSM2Key);
            panelSM2KeyControls.Controls.Add(comboSM2KeyFormat);
            panelSM2KeyControls.Controls.Add(label16);
            panelSM2KeyControls.Dock = DockStyle.Fill;
            panelSM2KeyControls.Location = new Point(3, 3);
            panelSM2KeyControls.Name = "panelSM2KeyControls";
            panelSM2KeyControls.Size = new Size(1232, 34);
            panelSM2KeyControls.TabIndex = 0;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(0, 8);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(73, 20);
            label16.TabIndex = 2;
            label16.Text = "密钥格式:";
            // 
            // comboSM2KeyFormat
            // 
            comboSM2KeyFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM2KeyFormat.FormattingEnabled = true;
            comboSM2KeyFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboSM2KeyFormat.Location = new Point(80, 4);
            comboSM2KeyFormat.Margin = new Padding(4);
            comboSM2KeyFormat.Name = "comboSM2KeyFormat";
            comboSM2KeyFormat.Size = new Size(127, 28);
            comboSM2KeyFormat.TabIndex = 1;
            // 
            // btnGenerateSM2Key
            // 
            btnGenerateSM2Key.Location = new Point(220, 2);
            btnGenerateSM2Key.Margin = new Padding(4);
            btnGenerateSM2Key.Name = "btnGenerateSM2Key";
            btnGenerateSM2Key.Size = new Size(129, 30);
            btnGenerateSM2Key.TabIndex = 0;
            btnGenerateSM2Key.Text = "生成密钥对";
            btnGenerateSM2Key.UseVisualStyleBackColor = true;
            btnGenerateSM2Key.Click += btnGenerateSM2Key_Click;
            // 
            // btnImportSM2Key
            // 
            btnImportSM2Key.Location = new Point(365, 2);
            btnImportSM2Key.Margin = new Padding(4);
            btnImportSM2Key.Name = "btnImportSM2Key";
            btnImportSM2Key.Size = new Size(103, 30);
            btnImportSM2Key.TabIndex = 7;
            btnImportSM2Key.Text = "导入密钥";
            btnImportSM2Key.UseVisualStyleBackColor = true;
            btnImportSM2Key.Click += btnImportSM2Key_Click;
            // 
            // btnExportSM2Key
            // 
            btnExportSM2Key.Location = new Point(485, 2);
            btnExportSM2Key.Margin = new Padding(4);
            btnExportSM2Key.Name = "btnExportSM2Key";
            btnExportSM2Key.Size = new Size(103, 30);
            btnExportSM2Key.TabIndex = 8;
            btnExportSM2Key.Text = "导出密钥";
            btnExportSM2Key.UseVisualStyleBackColor = true;
            btnExportSM2Key.Click += btnExportSM2Key_Click;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Dock = DockStyle.Bottom;
            label17.Location = new Point(4, 45);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(1230, 20);
            label17.TabIndex = 4;
            label17.Text = "公钥:";
            // 
            // textSM2PublicKey
            // 
            textSM2PublicKey.Dock = DockStyle.Fill;
            textSM2PublicKey.Location = new Point(4, 69);
            textSM2PublicKey.Margin = new Padding(4);
            textSM2PublicKey.Multiline = true;
            textSM2PublicKey.Name = "textSM2PublicKey";
            textSM2PublicKey.ScrollBars = ScrollBars.Both;
            textSM2PublicKey.Size = new Size(1230, 50);
            textSM2PublicKey.TabIndex = 3;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Dock = DockStyle.Bottom;
            label18.Location = new Point(4, 144);
            label18.Margin = new Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new Size(1230, 20);
            label18.TabIndex = 6;
            label18.Text = "私钥:";
            // 
            // textSM2PrivateKey
            // 
            textSM2PrivateKey.Dock = DockStyle.Fill;
            textSM2PrivateKey.Location = new Point(4, 168);
            textSM2PrivateKey.Margin = new Padding(4);
            textSM2PrivateKey.Multiline = true;
            textSM2PrivateKey.Name = "textSM2PrivateKey";
            textSM2PrivateKey.ScrollBars = ScrollBars.Both;
            textSM2PrivateKey.Size = new Size(1230, 34);
            textSM2PrivateKey.TabIndex = 5;
            // 
            // groupBoxSM2Encrypt
            // 
            groupBoxSM2Encrypt.Controls.Add(tableLayoutSM2Encrypt);
            groupBoxSM2Encrypt.Dock = DockStyle.Fill;
            groupBoxSM2Encrypt.Location = new Point(12, 262);
            groupBoxSM2Encrypt.Margin = new Padding(4);
            groupBoxSM2Encrypt.Name = "groupBoxSM2Encrypt";
            groupBoxSM2Encrypt.Padding = new Padding(8);
            groupBoxSM2Encrypt.Size = new Size(1254, 200);
            groupBoxSM2Encrypt.TabIndex = 1;
            groupBoxSM2Encrypt.TabStop = false;
            groupBoxSM2Encrypt.Text = "SM2加密解密";
            // 
            // tableLayoutSM2Encrypt
            // 
            tableLayoutSM2Encrypt.ColumnCount = 1;
            tableLayoutSM2Encrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutSM2Encrypt.Controls.Add(panelSM2EncryptControls, 0, 0);
            tableLayoutSM2Encrypt.Controls.Add(label20, 0, 1);
            tableLayoutSM2Encrypt.Controls.Add(textSM2PlainText, 0, 2);
            tableLayoutSM2Encrypt.Controls.Add(label21, 0, 3);
            tableLayoutSM2Encrypt.Controls.Add(textSM2CipherText, 0, 4);
            tableLayoutSM2Encrypt.Dock = DockStyle.Fill;
            tableLayoutSM2Encrypt.Location = new Point(8, 28);
            tableLayoutSM2Encrypt.Name = "tableLayoutSM2Encrypt";
            tableLayoutSM2Encrypt.RowCount = 5;
            tableLayoutSM2Encrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutSM2Encrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM2Encrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            tableLayoutSM2Encrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM2Encrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
            tableLayoutSM2Encrypt.Size = new Size(1238, 164);
            tableLayoutSM2Encrypt.TabIndex = 0;
            // 
            // panelSM2EncryptControls
            // 
            panelSM2EncryptControls.Controls.Add(btnSM2Decrypt);
            panelSM2EncryptControls.Controls.Add(btnSM2Encrypt);
            panelSM2EncryptControls.Controls.Add(comboSM2CipherFormat);
            panelSM2EncryptControls.Controls.Add(label19);
            panelSM2EncryptControls.Dock = DockStyle.Fill;
            panelSM2EncryptControls.Location = new Point(3, 3);
            panelSM2EncryptControls.Name = "panelSM2EncryptControls";
            panelSM2EncryptControls.Size = new Size(1232, 34);
            panelSM2EncryptControls.TabIndex = 0;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(0, 8);
            label19.Margin = new Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new Size(73, 20);
            label19.TabIndex = 1;
            label19.Text = "密文格式:";
            // 
            // comboSM2CipherFormat
            // 
            comboSM2CipherFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM2CipherFormat.FormattingEnabled = true;
            comboSM2CipherFormat.Items.AddRange(new object[] { "C1C3C2", "C1C2C3" });
            comboSM2CipherFormat.Location = new Point(80, 4);
            comboSM2CipherFormat.Margin = new Padding(4);
            comboSM2CipherFormat.Name = "comboSM2CipherFormat";
            comboSM2CipherFormat.Size = new Size(127, 28);
            comboSM2CipherFormat.TabIndex = 0;
            // 
            // btnSM2Encrypt
            // 
            btnSM2Encrypt.Location = new Point(220, 2);
            btnSM2Encrypt.Margin = new Padding(4);
            btnSM2Encrypt.Name = "btnSM2Encrypt";
            btnSM2Encrypt.Size = new Size(103, 30);
            btnSM2Encrypt.TabIndex = 2;
            btnSM2Encrypt.Text = "加密";
            btnSM2Encrypt.UseVisualStyleBackColor = true;
            btnSM2Encrypt.Click += btnSM2Encrypt_Click;
            // 
            // btnSM2Decrypt
            // 
            btnSM2Decrypt.Location = new Point(340, 2);
            btnSM2Decrypt.Margin = new Padding(4);
            btnSM2Decrypt.Name = "btnSM2Decrypt";
            btnSM2Decrypt.Size = new Size(103, 30);
            btnSM2Decrypt.TabIndex = 3;
            btnSM2Decrypt.Text = "解密";
            btnSM2Decrypt.UseVisualStyleBackColor = true;
            btnSM2Decrypt.Click += btnSM2Decrypt_Click;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Dock = DockStyle.Bottom;
            label20.Location = new Point(4, 45);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(1230, 20);
            label20.TabIndex = 5;
            label20.Text = "明文:";
            // 
            // textSM2PlainText
            // 
            textSM2PlainText.Dock = DockStyle.Fill;
            textSM2PlainText.Location = new Point(4, 69);
            textSM2PlainText.Margin = new Padding(4);
            textSM2PlainText.Multiline = true;
            textSM2PlainText.Name = "textSM2PlainText";
            textSM2PlainText.ScrollBars = ScrollBars.Both;
            textSM2PlainText.Size = new Size(1230, 20);
            textSM2PlainText.TabIndex = 4;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Dock = DockStyle.Bottom;
            label21.Location = new Point(4, 114);
            label21.Margin = new Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new Size(1230, 20);
            label21.TabIndex = 7;
            label21.Text = "密文:";
            // 
            // textSM2CipherText
            // 
            textSM2CipherText.Dock = DockStyle.Fill;
            textSM2CipherText.Location = new Point(4, 138);
            textSM2CipherText.Margin = new Padding(4);
            textSM2CipherText.Multiline = true;
            textSM2CipherText.Name = "textSM2CipherText";
            textSM2CipherText.ScrollBars = ScrollBars.Both;
            textSM2CipherText.Size = new Size(1230, 22);
            textSM2CipherText.TabIndex = 6;
            // 
            // groupBoxSM2Sign
            // 
            groupBoxSM2Sign.Controls.Add(tableLayoutSM2Sign);
            groupBoxSM2Sign.Dock = DockStyle.Fill;
            groupBoxSM2Sign.Location = new Point(12, 470);
            groupBoxSM2Sign.Margin = new Padding(4);
            groupBoxSM2Sign.Name = "groupBoxSM2Sign";
            groupBoxSM2Sign.Padding = new Padding(8);
            groupBoxSM2Sign.Size = new Size(1254, 368);
            groupBoxSM2Sign.TabIndex = 2;
            groupBoxSM2Sign.TabStop = false;
            groupBoxSM2Sign.Text = "SM2数字签名";
            // 
            // tableLayoutSM2Sign
            // 
            tableLayoutSM2Sign.ColumnCount = 1;
            tableLayoutSM2Sign.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutSM2Sign.Controls.Add(panelSM2SignControls, 0, 0);
            tableLayoutSM2Sign.Controls.Add(label23, 0, 1);
            tableLayoutSM2Sign.Controls.Add(textSM2SignData, 0, 2);
            tableLayoutSM2Sign.Controls.Add(label24, 0, 3);
            tableLayoutSM2Sign.Controls.Add(textSM2Signature, 0, 4);
            tableLayoutSM2Sign.Controls.Add(labelSM2VerifyResult, 0, 5);
            tableLayoutSM2Sign.Dock = DockStyle.Fill;
            tableLayoutSM2Sign.Location = new Point(8, 28);
            tableLayoutSM2Sign.Name = "tableLayoutSM2Sign";
            tableLayoutSM2Sign.RowCount = 6;
            tableLayoutSM2Sign.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutSM2Sign.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM2Sign.RowStyles.Add(new RowStyle(SizeType.Percent, 60F));
            tableLayoutSM2Sign.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM2Sign.RowStyles.Add(new RowStyle(SizeType.Percent, 30F));
            tableLayoutSM2Sign.RowStyles.Add(new RowStyle(SizeType.Percent, 10F));
            tableLayoutSM2Sign.Size = new Size(1238, 332);
            tableLayoutSM2Sign.TabIndex = 0;
            // 
            // panelSM2SignControls
            // 
            panelSM2SignControls.Controls.Add(btnSM2Verify);
            panelSM2SignControls.Controls.Add(btnSM2Sign);
            panelSM2SignControls.Controls.Add(comboSM2SignFormat);
            panelSM2SignControls.Controls.Add(label22);
            panelSM2SignControls.Dock = DockStyle.Fill;
            panelSM2SignControls.Location = new Point(3, 3);
            panelSM2SignControls.Name = "panelSM2SignControls";
            panelSM2SignControls.Size = new Size(1232, 34);
            panelSM2SignControls.TabIndex = 0;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(0, 8);
            label22.Margin = new Padding(4, 0, 4, 0);
            label22.Name = "label22";
            label22.Size = new Size(73, 20);
            label22.TabIndex = 1;
            label22.Text = "签名格式:";
            // 
            // comboSM2SignFormat
            // 
            comboSM2SignFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM2SignFormat.FormattingEnabled = true;
            comboSM2SignFormat.Items.AddRange(new object[] { "ASN1", "RS" });
            comboSM2SignFormat.Location = new Point(80, 4);
            comboSM2SignFormat.Margin = new Padding(4);
            comboSM2SignFormat.Name = "comboSM2SignFormat";
            comboSM2SignFormat.Size = new Size(127, 28);
            comboSM2SignFormat.TabIndex = 0;
            // 
            // btnSM2Sign
            // 
            btnSM2Sign.Location = new Point(220, 2);
            btnSM2Sign.Margin = new Padding(4);
            btnSM2Sign.Name = "btnSM2Sign";
            btnSM2Sign.Size = new Size(103, 30);
            btnSM2Sign.TabIndex = 2;
            btnSM2Sign.Text = "签名";
            btnSM2Sign.UseVisualStyleBackColor = true;
            btnSM2Sign.Click += btnSM2Sign_Click;
            // 
            // btnSM2Verify
            // 
            btnSM2Verify.Location = new Point(340, 2);
            btnSM2Verify.Margin = new Padding(4);
            btnSM2Verify.Name = "btnSM2Verify";
            btnSM2Verify.Size = new Size(103, 30);
            btnSM2Verify.TabIndex = 3;
            btnSM2Verify.Text = "验签";
            btnSM2Verify.UseVisualStyleBackColor = true;
            btnSM2Verify.Click += btnSM2Verify_Click;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Dock = DockStyle.Bottom;
            label23.Location = new Point(4, 45);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(1230, 20);
            label23.TabIndex = 5;
            label23.Text = "原文数据:";
            // 
            // textSM2SignData
            // 
            textSM2SignData.Dock = DockStyle.Fill;
            textSM2SignData.Location = new Point(4, 69);
            textSM2SignData.Margin = new Padding(4);
            textSM2SignData.Multiline = true;
            textSM2SignData.Name = "textSM2SignData";
            textSM2SignData.ScrollBars = ScrollBars.Both;
            textSM2SignData.Size = new Size(1230, 136);
            textSM2SignData.TabIndex = 4;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Dock = DockStyle.Bottom;
            label24.Location = new Point(4, 230);
            label24.Margin = new Padding(4, 0, 4, 0);
            label24.Name = "label24";
            label24.Size = new Size(1230, 20);
            label24.TabIndex = 7;
            label24.Text = "签名:";
            // 
            // textSM2Signature
            // 
            textSM2Signature.Dock = DockStyle.Fill;
            textSM2Signature.Location = new Point(4, 254);
            textSM2Signature.Margin = new Padding(4);
            textSM2Signature.Multiline = true;
            textSM2Signature.Name = "textSM2Signature";
            textSM2Signature.ScrollBars = ScrollBars.Both;
            textSM2Signature.Size = new Size(1230, 48);
            textSM2Signature.TabIndex = 6;
            // 
            // labelSM2VerifyResult
            // 
            labelSM2VerifyResult.AutoSize = true;
            labelSM2VerifyResult.Dock = DockStyle.Fill;
            labelSM2VerifyResult.Location = new Point(4, 306);
            labelSM2VerifyResult.Margin = new Padding(4, 0, 4, 0);
            labelSM2VerifyResult.Name = "labelSM2VerifyResult";
            labelSM2VerifyResult.Size = new Size(1230, 22);
            labelSM2VerifyResult.TabIndex = 8;
            labelSM2VerifyResult.Text = "验签结果:";
            labelSM2VerifyResult.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // SM2TabControl
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainTableLayout);
            Margin = new Padding(4);
            Name = "SM2TabControl";
            Size = new Size(1278, 850);
            mainTableLayout.ResumeLayout(false);
            groupBoxSM2Keys.ResumeLayout(false);
            tableLayoutSM2Keys.ResumeLayout(false);
            tableLayoutSM2Keys.PerformLayout();
            panelSM2KeyControls.ResumeLayout(false);
            panelSM2KeyControls.PerformLayout();
            groupBoxSM2Encrypt.ResumeLayout(false);
            tableLayoutSM2Encrypt.ResumeLayout(false);
            tableLayoutSM2Encrypt.PerformLayout();
            panelSM2EncryptControls.ResumeLayout(false);
            panelSM2EncryptControls.PerformLayout();
            groupBoxSM2Sign.ResumeLayout(false);
            tableLayoutSM2Sign.ResumeLayout(false);
            tableLayoutSM2Sign.PerformLayout();
            panelSM2SignControls.ResumeLayout(false);
            panelSM2SignControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainTableLayout;
        private GroupBox groupBoxSM2Keys;
        private TableLayoutPanel tableLayoutSM2Keys;
        private Panel panelSM2KeyControls;
        private Label label16;
        private ComboBox comboSM2KeyFormat;
        private Button btnGenerateSM2Key;
        private Button btnImportSM2Key;
        private Button btnExportSM2Key;
        private Label label17;
        private TextBox textSM2PublicKey;
        private Label label18;
        private TextBox textSM2PrivateKey;
        private GroupBox groupBoxSM2Encrypt;
        private TableLayoutPanel tableLayoutSM2Encrypt;
        private Panel panelSM2EncryptControls;
        private Label label19;
        private ComboBox comboSM2CipherFormat;
        private Button btnSM2Encrypt;
        private Button btnSM2Decrypt;
        private Label label20;
        private TextBox textSM2PlainText;
        private Label label21;
        private TextBox textSM2CipherText;
        private GroupBox groupBoxSM2Sign;
        private TableLayoutPanel tableLayoutSM2Sign;
        private Panel panelSM2SignControls;
        private Label label22;
        private ComboBox comboSM2SignFormat;
        private Button btnSM2Sign;
        private Button btnSM2Verify;
        private Label label23;
        private TextBox textSM2SignData;
        private Label label24;
        private TextBox textSM2Signature;
        private Label labelSM2VerifyResult;
    }
}
