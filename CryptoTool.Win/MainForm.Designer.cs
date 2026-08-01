namespace CryptoTool.Win
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                // 释放后台更新服务
                updateService?.Dispose();

                if (components != null)
                {
                    components.Dispose();
                }
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            tabControl1 = new TabControl();
            tabEcdsa = new TabPage();
            tabRSA = new TabPage();
            tabRSAConvert = new TabPage();
            tabAES = new TabPage();
            tabDES = new TabPage();
            tabSM4 = new TabPage();
            tabSM2 = new TabPage();
            tabSM3 = new TabPage();
            tabMD5 = new TabPage();
            tabMedicare = new TabPage();
            tabAbout = new TabPage();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabEcdsa);
            tabControl1.Controls.Add(tabRSA);
            tabControl1.Controls.Add(tabRSAConvert);
            tabControl1.Controls.Add(tabAES);
            tabControl1.Controls.Add(tabDES);
            tabControl1.Controls.Add(tabSM4);
            tabControl1.Controls.Add(tabSM2);
            tabControl1.Controls.Add(tabSM3);
            tabControl1.Controls.Add(tabMD5);
            tabControl1.Controls.Add(tabMedicare);
            tabControl1.Controls.Add(tabAbout);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 0);
            tabControl1.Margin = new Padding(5, 5, 5, 5);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(2933, 1296);
            tabControl1.TabIndex = 0;
            // 
            // tabEcdsa
            // 
            tabEcdsa.Location = new Point(4, 33);
            tabEcdsa.Margin = new Padding(5, 5, 5, 5);
            tabEcdsa.Name = "tabEcdsa";
            tabEcdsa.Padding = new Padding(5, 5, 5, 5);
            tabEcdsa.Size = new Size(2925, 1259);
            tabEcdsa.TabIndex = 0;
            tabEcdsa.Text = "ECDSA算法";
            tabEcdsa.UseVisualStyleBackColor = true;
            // 
            // tabRSA
            // 
            tabRSA.Location = new Point(4, 33);
            tabRSA.Margin = new Padding(5, 5, 5, 5);
            tabRSA.Name = "tabRSA";
            tabRSA.Padding = new Padding(5, 5, 5, 5);
            tabRSA.Size = new Size(2925, 1259);
            tabRSA.TabIndex = 1;
            tabRSA.Text = "RSA算法";
            tabRSA.UseVisualStyleBackColor = true;
            // 
            // tabRSAConvert
            // 
            tabRSAConvert.Location = new Point(4, 33);
            tabRSAConvert.Margin = new Padding(5, 5, 5, 5);
            tabRSAConvert.Name = "tabRSAConvert";
            tabRSAConvert.Padding = new Padding(5, 5, 5, 5);
            tabRSAConvert.Size = new Size(2925, 1259);
            tabRSAConvert.TabIndex = 2;
            tabRSAConvert.Text = "RSA格式转换";
            tabRSAConvert.UseVisualStyleBackColor = true;
            // 
            // tabAES
            // 
            tabAES.Location = new Point(4, 33);
            tabAES.Margin = new Padding(5, 5, 5, 5);
            tabAES.Name = "tabAES";
            tabAES.Padding = new Padding(5, 5, 5, 5);
            tabAES.Size = new Size(2925, 1259);
            tabAES.TabIndex = 3;
            tabAES.Text = "AES算法";
            tabAES.UseVisualStyleBackColor = true;
            // 
            // tabDES
            // 
            tabDES.Location = new Point(4, 33);
            tabDES.Margin = new Padding(5, 5, 5, 5);
            tabDES.Name = "tabDES";
            tabDES.Padding = new Padding(5, 5, 5, 5);
            tabDES.Size = new Size(2925, 1259);
            tabDES.TabIndex = 4;
            tabDES.Text = "DES算法";
            tabDES.UseVisualStyleBackColor = true;
            // 
            // tabSM4
            // 
            tabSM4.Location = new Point(4, 33);
            tabSM4.Margin = new Padding(5, 5, 5, 5);
            tabSM4.Name = "tabSM4";
            tabSM4.Padding = new Padding(5, 5, 5, 5);
            tabSM4.Size = new Size(2925, 1259);
            tabSM4.TabIndex = 5;
            tabSM4.Text = "SM4算法";
            tabSM4.UseVisualStyleBackColor = true;
            // 
            // tabSM2
            // 
            tabSM2.Location = new Point(4, 33);
            tabSM2.Margin = new Padding(5, 5, 5, 5);
            tabSM2.Name = "tabSM2";
            tabSM2.Padding = new Padding(5, 5, 5, 5);
            tabSM2.Size = new Size(2925, 1259);
            tabSM2.TabIndex = 6;
            tabSM2.Text = "SM2算法";
            tabSM2.UseVisualStyleBackColor = true;
            // 
            // tabSM3
            // 
            tabSM3.Location = new Point(4, 33);
            tabSM3.Margin = new Padding(5, 5, 5, 5);
            tabSM3.Name = "tabSM3";
            tabSM3.Padding = new Padding(5, 5, 5, 5);
            tabSM3.Size = new Size(2925, 1259);
            tabSM3.TabIndex = 7;
            tabSM3.Text = "SM3算法";
            tabSM3.UseVisualStyleBackColor = true;
            // 
            // tabMD5
            // 
            tabMD5.Location = new Point(4, 33);
            tabMD5.Margin = new Padding(5, 5, 5, 5);
            tabMD5.Name = "tabMD5";
            tabMD5.Padding = new Padding(5, 5, 5, 5);
            tabMD5.Size = new Size(2925, 1259);
            tabMD5.TabIndex = 8;
            tabMD5.Text = "MD5算法";
            tabMD5.UseVisualStyleBackColor = true;
            // 
            // tabMedicare
            // 
            tabMedicare.Location = new Point(4, 33);
            tabMedicare.Margin = new Padding(5, 5, 5, 5);
            tabMedicare.Name = "tabMedicare";
            tabMedicare.Padding = new Padding(4, 5, 4, 5);
            tabMedicare.Size = new Size(2925, 1259);
            tabMedicare.TabIndex = 9;
            tabMedicare.Text = "医保接口";
            tabMedicare.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            tabAbout.Location = new Point(4, 33);
            tabAbout.Margin = new Padding(5, 5, 5, 5);
            tabAbout.Name = "tabAbout";
            tabAbout.Padding = new Padding(4, 5, 4, 5);
            tabAbout.Size = new Size(2925, 1259);
            tabAbout.TabIndex = 10;
            tabAbout.Text = "关于";
            tabAbout.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2933, 1296);
            Controls.Add(tabControl1);
            Margin = new Padding(5, 5, 5, 5);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "加解密工具";
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabRSA;
        private System.Windows.Forms.TabPage tabRSAConvert;
        private System.Windows.Forms.TabPage tabAES;
        private System.Windows.Forms.TabPage tabDES;
        private System.Windows.Forms.TabPage tabSM4;
        private System.Windows.Forms.TabPage tabSM2;
        private System.Windows.Forms.TabPage tabSM3;
        private System.Windows.Forms.TabPage tabEcdsa;   // ✅ ECDSA 成员变量
        private System.Windows.Forms.TabPage tabMD5;
        private System.Windows.Forms.TabPage tabMedicare;
        private System.Windows.Forms.TabPage tabAbout;
    }

    /// <summary>
    /// 定义一个表示下拉选项的类
    /// </summary>
    public class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }
    }
}