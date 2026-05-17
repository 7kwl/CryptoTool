namespace CryptoTool.Win
{
    partial class SM4TabControl : UserControl
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
            groupBoxSM4Keys = new GroupBox();
            tableLayoutSM4Keys = new TableLayoutPanel();
            panelSM4KeyControls = new Panel();
            labelSM4KeyFormat = new Label();
            comboSM4KeyFormat = new ComboBox();
            btnGenerateSM4Key = new Button();
            label10 = new Label();
            panelSM4Key = new Panel();
            textSM4Key = new TextBox();
            labelSM4IVFormat = new Label();
            comboSM4IVFormat = new ComboBox();
            btnGenerateSM4IV = new Button();
            label11 = new Label();
            panelSM4IV = new Panel();
            textSM4IV = new TextBox();
            groupBoxSM4Encrypt = new GroupBox();
            tableLayoutSM4Encrypt = new TableLayoutPanel();
            panelSM4EncryptControls = new Panel();
            label14 = new Label();
            comboSM4Mode = new ComboBox();
            label15 = new Label();
            comboSM4Padding = new ComboBox();
            labelSM4PlaintextFormat = new Label();
            comboSM4PlaintextFormat = new ComboBox();
            labelSM4CiphertextFormat = new Label();
            comboSM4CiphertextFormat = new ComboBox();
            btnSM4Encrypt = new Button();
            btnSM4Decrypt = new Button();
            label12 = new Label();
            textSM4PlainText = new TextBox();
            label13 = new Label();
            textSM4CipherText = new TextBox();
            mainTableLayout.SuspendLayout();
            groupBoxSM4Keys.SuspendLayout();
            tableLayoutSM4Keys.SuspendLayout();
            panelSM4KeyControls.SuspendLayout();
            panelSM4Key.SuspendLayout();
            panelSM4IV.SuspendLayout();
            groupBoxSM4Encrypt.SuspendLayout();
            tableLayoutSM4Encrypt.SuspendLayout();
            panelSM4EncryptControls.SuspendLayout();
            SuspendLayout();
            // 
            // mainTableLayout
            // 
            mainTableLayout.ColumnCount = 1;
            mainTableLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            mainTableLayout.Controls.Add(groupBoxSM4Keys, 0, 0);
            mainTableLayout.Controls.Add(groupBoxSM4Encrypt, 0, 1);
            mainTableLayout.Dock = DockStyle.Fill;
            mainTableLayout.Location = new Point(0, 0);
            mainTableLayout.Margin = new Padding(4);
            mainTableLayout.Name = "mainTableLayout";
            mainTableLayout.Padding = new Padding(8);
            mainTableLayout.RowCount = 2;
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 35F));
            mainTableLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 65F));
            mainTableLayout.Size = new Size(1278, 500);
            mainTableLayout.TabIndex = 0;
            // 
            // groupBoxSM4Keys
            // 
            groupBoxSM4Keys.Controls.Add(tableLayoutSM4Keys);
            groupBoxSM4Keys.Dock = DockStyle.Fill;
            groupBoxSM4Keys.Location = new Point(12, 12);
            groupBoxSM4Keys.Margin = new Padding(4);
            groupBoxSM4Keys.Name = "groupBoxSM4Keys";
            groupBoxSM4Keys.Padding = new Padding(8);
            groupBoxSM4Keys.Size = new Size(1254, 159);
            groupBoxSM4Keys.TabIndex = 0;
            groupBoxSM4Keys.TabStop = false;
            groupBoxSM4Keys.Text = "SM4密钥生成";
            // 
            // tableLayoutSM4Keys
            // 
            tableLayoutSM4Keys.ColumnCount = 1;
            tableLayoutSM4Keys.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutSM4Keys.Controls.Add(panelSM4KeyControls, 0, 0);
            tableLayoutSM4Keys.Controls.Add(label10, 0, 1);
            tableLayoutSM4Keys.Controls.Add(panelSM4Key, 0, 2);
            tableLayoutSM4Keys.Controls.Add(label11, 0, 3);
            tableLayoutSM4Keys.Controls.Add(panelSM4IV, 0, 4);
            tableLayoutSM4Keys.Dock = DockStyle.Fill;
            tableLayoutSM4Keys.Location = new Point(8, 28);
            tableLayoutSM4Keys.Name = "tableLayoutSM4Keys";
            tableLayoutSM4Keys.RowCount = 5;
            tableLayoutSM4Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutSM4Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM4Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
            tableLayoutSM4Keys.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM4Keys.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutSM4Keys.Size = new Size(1238, 123);
            tableLayoutSM4Keys.TabIndex = 0;
            // 
            // panelSM4KeyControls
            // 
            panelSM4KeyControls.Controls.Add(btnGenerateSM4Key);
            panelSM4KeyControls.Controls.Add(comboSM4KeyFormat);
            panelSM4KeyControls.Controls.Add(labelSM4KeyFormat);
            panelSM4KeyControls.Dock = DockStyle.Fill;
            panelSM4KeyControls.Location = new Point(3, 3);
            panelSM4KeyControls.Name = "panelSM4KeyControls";
            panelSM4KeyControls.Size = new Size(1232, 34);
            panelSM4KeyControls.TabIndex = 0;
            // 
            // labelSM4KeyFormat
            // 
            labelSM4KeyFormat.AutoSize = true;
            labelSM4KeyFormat.Location = new Point(0, 8);
            labelSM4KeyFormat.Margin = new Padding(4, 0, 4, 0);
            labelSM4KeyFormat.Name = "labelSM4KeyFormat";
            labelSM4KeyFormat.Size = new Size(73, 20);
            labelSM4KeyFormat.TabIndex = 9;
            labelSM4KeyFormat.Text = "密钥格式:";
            // 
            // comboSM4KeyFormat
            // 
            comboSM4KeyFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM4KeyFormat.FormattingEnabled = true;
            comboSM4KeyFormat.Items.AddRange(new object[] { "Base64", "Hex", "Text" });
            comboSM4KeyFormat.Location = new Point(80, 4);
            comboSM4KeyFormat.Margin = new Padding(4);
            comboSM4KeyFormat.Name = "comboSM4KeyFormat";
            comboSM4KeyFormat.Size = new Size(127, 28);
            comboSM4KeyFormat.TabIndex = 8;
            // 
            // btnGenerateSM4Key
            // 
            btnGenerateSM4Key.Location = new Point(220, 2);
            btnGenerateSM4Key.Margin = new Padding(4);
            btnGenerateSM4Key.Name = "btnGenerateSM4Key";
            btnGenerateSM4Key.Size = new Size(103, 30);
            btnGenerateSM4Key.TabIndex = 0;
            btnGenerateSM4Key.Text = "生成密钥";
            btnGenerateSM4Key.UseVisualStyleBackColor = true;
            btnGenerateSM4Key.Click += btnGenerateSM4Key_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Dock = DockStyle.Bottom;
            label10.Location = new Point(4, 45);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(1230, 20);
            label10.TabIndex = 3;
            label10.Text = "SM4密钥:";
            // 
            // panelSM4Key
            // 
            panelSM4Key.Controls.Add(textSM4Key);
            panelSM4Key.Dock = DockStyle.Fill;
            panelSM4Key.Location = new Point(3, 68);
            panelSM4Key.Name = "panelSM4Key";
            panelSM4Key.Size = new Size(1232, 29);
            panelSM4Key.TabIndex = 4;
            // 
            // textSM4Key
            // 
            textSM4Key.Dock = DockStyle.Fill;
            textSM4Key.Location = new Point(0, 0);
            textSM4Key.Margin = new Padding(4);
            textSM4Key.Name = "textSM4Key";
            textSM4Key.Size = new Size(1232, 27);
            textSM4Key.TabIndex = 2;
            // 
            // labelSM4IVFormat
            // 
            labelSM4IVFormat.AutoSize = true;
            labelSM4IVFormat.Location = new Point(0, 8);
            labelSM4IVFormat.Margin = new Padding(4, 0, 4, 0);
            labelSM4IVFormat.Name = "labelSM4IVFormat";
            labelSM4IVFormat.Size = new Size(73, 20);
            labelSM4IVFormat.TabIndex = 11;
            labelSM4IVFormat.Text = "向量格式:";
            // 
            // comboSM4IVFormat
            // 
            comboSM4IVFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM4IVFormat.FormattingEnabled = true;
            comboSM4IVFormat.Items.AddRange(new object[] { "Base64", "Hex", "Text" });
            comboSM4IVFormat.Location = new Point(80, 4);
            comboSM4IVFormat.Margin = new Padding(4);
            comboSM4IVFormat.Name = "comboSM4IVFormat";
            comboSM4IVFormat.Size = new Size(127, 28);
            comboSM4IVFormat.TabIndex = 10;
            // 
            // btnGenerateSM4IV
            // 
            btnGenerateSM4IV.Location = new Point(220, 2);
            btnGenerateSM4IV.Margin = new Padding(4);
            btnGenerateSM4IV.Name = "btnGenerateSM4IV";
            btnGenerateSM4IV.Size = new Size(103, 30);
            btnGenerateSM4IV.TabIndex = 1;
            btnGenerateSM4IV.Text = "生成向量";
            btnGenerateSM4IV.UseVisualStyleBackColor = true;
            btnGenerateSM4IV.Click += btnGenerateSM4IV_Click;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Dock = DockStyle.Bottom;
            label11.Location = new Point(4, 105);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(1230, 20);
            label11.TabIndex = 5;
            label11.Text = "初始向量:";
            // 
            // panelSM4IV
            // 
            panelSM4IV.Controls.Add(btnGenerateSM4IV);
            panelSM4IV.Controls.Add(comboSM4IVFormat);
            panelSM4IV.Controls.Add(labelSM4IVFormat);
            panelSM4IV.Controls.Add(textSM4IV);
            panelSM4IV.Dock = DockStyle.Fill;
            panelSM4IV.Location = new Point(3, 128);
            panelSM4IV.Name = "panelSM4IV";
            panelSM4IV.Size = new Size(1232, 1);
            panelSM4IV.TabIndex = 6;
            // 
            // textSM4IV
            // 
            textSM4IV.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            textSM4IV.Location = new Point(340, 4);
            textSM4IV.Margin = new Padding(4);
            textSM4IV.Name = "textSM4IV";
            textSM4IV.Size = new Size(892, 27);
            textSM4IV.TabIndex = 4;
            // 
            // groupBoxSM4Encrypt
            // 
            groupBoxSM4Encrypt.Controls.Add(tableLayoutSM4Encrypt);
            groupBoxSM4Encrypt.Dock = DockStyle.Fill;
            groupBoxSM4Encrypt.Location = new Point(12, 179);
            groupBoxSM4Encrypt.Margin = new Padding(4);
            groupBoxSM4Encrypt.Name = "groupBoxSM4Encrypt";
            groupBoxSM4Encrypt.Padding = new Padding(8);
            groupBoxSM4Encrypt.Size = new Size(1254, 309);
            groupBoxSM4Encrypt.TabIndex = 1;
            groupBoxSM4Encrypt.TabStop = false;
            groupBoxSM4Encrypt.Text = "SM4加密解密";
            // 
            // tableLayoutSM4Encrypt
            // 
            tableLayoutSM4Encrypt.ColumnCount = 1;
            tableLayoutSM4Encrypt.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutSM4Encrypt.Controls.Add(panelSM4EncryptControls, 0, 0);
            tableLayoutSM4Encrypt.Controls.Add(label12, 0, 1);
            tableLayoutSM4Encrypt.Controls.Add(textSM4PlainText, 0, 2);
            tableLayoutSM4Encrypt.Controls.Add(label13, 0, 3);
            tableLayoutSM4Encrypt.Controls.Add(textSM4CipherText, 0, 4);
            tableLayoutSM4Encrypt.Dock = DockStyle.Fill;
            tableLayoutSM4Encrypt.Location = new Point(8, 28);
            tableLayoutSM4Encrypt.Name = "tableLayoutSM4Encrypt";
            tableLayoutSM4Encrypt.RowCount = 5;
            tableLayoutSM4Encrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
            tableLayoutSM4Encrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM4Encrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutSM4Encrypt.RowStyles.Add(new RowStyle(SizeType.Absolute, 25F));
            tableLayoutSM4Encrypt.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutSM4Encrypt.Size = new Size(1238, 273);
            tableLayoutSM4Encrypt.TabIndex = 0;
            // 
            // panelSM4EncryptControls
            // 
            panelSM4EncryptControls.Controls.Add(btnSM4Decrypt);
            panelSM4EncryptControls.Controls.Add(btnSM4Encrypt);
            panelSM4EncryptControls.Controls.Add(comboSM4CiphertextFormat);
            panelSM4EncryptControls.Controls.Add(labelSM4CiphertextFormat);
            panelSM4EncryptControls.Controls.Add(comboSM4PlaintextFormat);
            panelSM4EncryptControls.Controls.Add(labelSM4PlaintextFormat);
            panelSM4EncryptControls.Controls.Add(comboSM4Padding);
            panelSM4EncryptControls.Controls.Add(label15);
            panelSM4EncryptControls.Controls.Add(comboSM4Mode);
            panelSM4EncryptControls.Controls.Add(label14);
            panelSM4EncryptControls.Dock = DockStyle.Fill;
            panelSM4EncryptControls.Location = new Point(3, 3);
            panelSM4EncryptControls.Name = "panelSM4EncryptControls";
            panelSM4EncryptControls.Size = new Size(1232, 34);
            panelSM4EncryptControls.TabIndex = 0;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(0, 8);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(73, 20);
            label14.TabIndex = 5;
            label14.Text = "加密模式:";
            // 
            // comboSM4Mode
            // 
            comboSM4Mode.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM4Mode.FormattingEnabled = true;
            comboSM4Mode.Items.AddRange(new object[] { "ECB", "CBC" });
            comboSM4Mode.Location = new Point(80, 4);
            comboSM4Mode.Margin = new Padding(4);
            comboSM4Mode.Name = "comboSM4Mode";
            comboSM4Mode.Size = new Size(127, 28);
            comboSM4Mode.TabIndex = 4;
            comboSM4Mode.SelectedIndexChanged += comboSM4Mode_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(220, 8);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(73, 20);
            label15.TabIndex = 7;
            label15.Text = "填充模式:";
            // 
            // comboSM4Padding
            // 
            comboSM4Padding.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM4Padding.FormattingEnabled = true;
            comboSM4Padding.Items.AddRange(new object[] { "PKCS7", "PKCS5", "NoPadding" });
            comboSM4Padding.Location = new Point(300, 4);
            comboSM4Padding.Margin = new Padding(4);
            comboSM4Padding.Name = "comboSM4Padding";
            comboSM4Padding.Size = new Size(127, 28);
            comboSM4Padding.TabIndex = 6;
            // 
            // labelSM4PlaintextFormat
            // 
            labelSM4PlaintextFormat.AutoSize = true;
            labelSM4PlaintextFormat.Location = new Point(440, 8);
            labelSM4PlaintextFormat.Margin = new Padding(4, 0, 4, 0);
            labelSM4PlaintextFormat.Name = "labelSM4PlaintextFormat";
            labelSM4PlaintextFormat.Size = new Size(73, 20);
            labelSM4PlaintextFormat.TabIndex = 11;
            labelSM4PlaintextFormat.Text = "明文格式:";
            // 
            // comboSM4PlaintextFormat
            // 
            comboSM4PlaintextFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM4PlaintextFormat.FormattingEnabled = true;
            comboSM4PlaintextFormat.Items.AddRange(new object[] { "Text", "Base64", "Hex" });
            comboSM4PlaintextFormat.Location = new Point(520, 4);
            comboSM4PlaintextFormat.Margin = new Padding(4);
            comboSM4PlaintextFormat.Name = "comboSM4PlaintextFormat";
            comboSM4PlaintextFormat.Size = new Size(127, 28);
            comboSM4PlaintextFormat.TabIndex = 10;
            // 
            // labelSM4CiphertextFormat
            // 
            labelSM4CiphertextFormat.AutoSize = true;
            labelSM4CiphertextFormat.Location = new Point(660, 8);
            labelSM4CiphertextFormat.Margin = new Padding(4, 0, 4, 0);
            labelSM4CiphertextFormat.Name = "labelSM4CiphertextFormat";
            labelSM4CiphertextFormat.Size = new Size(73, 20);
            labelSM4CiphertextFormat.TabIndex = 9;
            labelSM4CiphertextFormat.Text = "密文格式:";
            // 
            // comboSM4CiphertextFormat
            // 
            comboSM4CiphertextFormat.DropDownStyle = ComboBoxStyle.DropDownList;
            comboSM4CiphertextFormat.FormattingEnabled = true;
            comboSM4CiphertextFormat.Items.AddRange(new object[] { "Base64", "Hex" });
            comboSM4CiphertextFormat.Location = new Point(740, 4);
            comboSM4CiphertextFormat.Margin = new Padding(4);
            comboSM4CiphertextFormat.Name = "comboSM4CiphertextFormat";
            comboSM4CiphertextFormat.Size = new Size(127, 28);
            comboSM4CiphertextFormat.TabIndex = 8;
            // 
            // btnSM4Encrypt
            // 
            btnSM4Encrypt.Location = new Point(880, 2);
            btnSM4Encrypt.Margin = new Padding(4);
            btnSM4Encrypt.Name = "btnSM4Encrypt";
            btnSM4Encrypt.Size = new Size(103, 30);
            btnSM4Encrypt.TabIndex = 0;
            btnSM4Encrypt.Text = "加密";
            btnSM4Encrypt.UseVisualStyleBackColor = true;
            btnSM4Encrypt.Click += btnSM4Encrypt_Click;
            // 
            // btnSM4Decrypt
            // 
            btnSM4Decrypt.Location = new Point(1000, 2);
            btnSM4Decrypt.Margin = new Padding(4);
            btnSM4Decrypt.Name = "btnSM4Decrypt";
            btnSM4Decrypt.Size = new Size(103, 30);
            btnSM4Decrypt.TabIndex = 1;
            btnSM4Decrypt.Text = "解密";
            btnSM4Decrypt.UseVisualStyleBackColor = true;
            btnSM4Decrypt.Click += btnSM4Decrypt_Click;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Dock = DockStyle.Bottom;
            label12.Location = new Point(4, 45);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(1230, 20);
            label12.TabIndex = 3;
            label12.Text = "明文:";
            // 
            // textSM4PlainText
            // 
            textSM4PlainText.Dock = DockStyle.Fill;
            textSM4PlainText.Location = new Point(4, 69);
            textSM4PlainText.Margin = new Padding(4);
            textSM4PlainText.Multiline = true;
            textSM4PlainText.Name = "textSM4PlainText";
            textSM4PlainText.ScrollBars = ScrollBars.Both;
            textSM4PlainText.Size = new Size(1230, 93);
            textSM4PlainText.TabIndex = 2;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Dock = DockStyle.Bottom;
            label13.Location = new Point(4, 187);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(1230, 20);
            label13.TabIndex = 5;
            label13.Text = "密文:";
            // 
            // textSM4CipherText
            // 
            textSM4CipherText.Dock = DockStyle.Fill;
            textSM4CipherText.Location = new Point(4, 211);
            textSM4CipherText.Margin = new Padding(4);
            textSM4CipherText.Multiline = true;
            textSM4CipherText.Name = "textSM4CipherText";
            textSM4CipherText.ScrollBars = ScrollBars.Both;
            textSM4CipherText.Size = new Size(1230, 58);
            textSM4CipherText.TabIndex = 3;
            // 
            // SM4TabControl
            // 
            AutoScaleDimensions = new SizeF(9F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            Controls.Add(mainTableLayout);
            Margin = new Padding(4);
            Name = "SM4TabControl";
            Size = new Size(1278, 500);
            mainTableLayout.ResumeLayout(false);
            groupBoxSM4Keys.ResumeLayout(false);
            tableLayoutSM4Keys.ResumeLayout(false);
            tableLayoutSM4Keys.PerformLayout();
            panelSM4KeyControls.ResumeLayout(false);
            panelSM4KeyControls.PerformLayout();
            panelSM4Key.ResumeLayout(false);
            panelSM4Key.PerformLayout();
            panelSM4IV.ResumeLayout(false);
            panelSM4IV.PerformLayout();
            groupBoxSM4Encrypt.ResumeLayout(false);
            tableLayoutSM4Encrypt.ResumeLayout(false);
            tableLayoutSM4Encrypt.PerformLayout();
            panelSM4EncryptControls.ResumeLayout(false);
            panelSM4EncryptControls.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TableLayoutPanel mainTableLayout;
        private GroupBox groupBoxSM4Keys;
        private TableLayoutPanel tableLayoutSM4Keys;
        private Panel panelSM4KeyControls;
        private Label labelSM4KeyFormat;
        private ComboBox comboSM4KeyFormat;
        private Button btnGenerateSM4Key;
        private Label label10;
        private Panel panelSM4Key;
        private TextBox textSM4Key;
        private Label labelSM4IVFormat;
        private ComboBox comboSM4IVFormat;
        private Button btnGenerateSM4IV;
        private Label label11;
        private Panel panelSM4IV;
        private TextBox textSM4IV;
        private GroupBox groupBoxSM4Encrypt;
        private TableLayoutPanel tableLayoutSM4Encrypt;
        private Panel panelSM4EncryptControls;
        private Label label14;
        private ComboBox comboSM4Mode;
        private Label label15;
        private ComboBox comboSM4Padding;
        private Label labelSM4PlaintextFormat;
        private ComboBox comboSM4PlaintextFormat;
        private Label labelSM4CiphertextFormat;
        private ComboBox comboSM4CiphertextFormat;
        private Button btnSM4Encrypt;
        private Button btnSM4Decrypt;
        private Label label12;
        private TextBox textSM4PlainText;
        private Label label13;
        private TextBox textSM4CipherText;
    }
}
