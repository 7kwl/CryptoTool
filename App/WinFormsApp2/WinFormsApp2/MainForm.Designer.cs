namespace WinFormsApp2
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
            if (disposing && (components != null))
            {
                components.Dispose();
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
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            tabControl1.SuspendLayout();
            statusStrip1.SuspendLayout();
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
            tabControl1.Margin = new Padding(4);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(2400, 1080);
            tabControl1.TabIndex = 0;
            // 
            // tabEcdsa
            // 
            tabEcdsa.Location = new Point(4, 33);
            tabEcdsa.Margin = new Padding(4);
            tabEcdsa.Name = "tabEcdsa";
            tabEcdsa.Padding = new Padding(4);
            tabEcdsa.Size = new Size(2392, 1043);
            tabEcdsa.TabIndex = 0;
            tabEcdsa.Text = "ECDSA算法";
            tabEcdsa.UseVisualStyleBackColor = true;
            // 
            // tabRSA
            // 
            tabRSA.Location = new Point(4, 33);
            tabRSA.Margin = new Padding(4);
            tabRSA.Name = "tabRSA";
            tabRSA.Padding = new Padding(4);
            tabRSA.Size = new Size(2392, 1017);
            tabRSA.TabIndex = 1;
            tabRSA.Text = "RSA算法";
            tabRSA.UseVisualStyleBackColor = true;
            // 
            // tabRSAConvert
            // 
            tabRSAConvert.Location = new Point(4, 33);
            tabRSAConvert.Margin = new Padding(4);
            tabRSAConvert.Name = "tabRSAConvert";
            tabRSAConvert.Padding = new Padding(4);
            tabRSAConvert.Size = new Size(2392, 1017);
            tabRSAConvert.TabIndex = 2;
            tabRSAConvert.Text = "RSA格式转换";
            tabRSAConvert.UseVisualStyleBackColor = true;
            // 
            // tabAES
            // 
            tabAES.Location = new Point(4, 33);
            tabAES.Margin = new Padding(4);
            tabAES.Name = "tabAES";
            tabAES.Padding = new Padding(4);
            tabAES.Size = new Size(2392, 1017);
            tabAES.TabIndex = 3;
            tabAES.Text = "AES算法";
            tabAES.UseVisualStyleBackColor = true;
            // 
            // tabDES
            // 
            tabDES.Location = new Point(4, 33);
            tabDES.Margin = new Padding(4);
            tabDES.Name = "tabDES";
            tabDES.Padding = new Padding(4);
            tabDES.Size = new Size(2392, 1017);
            tabDES.TabIndex = 4;
            tabDES.Text = "DES算法";
            tabDES.UseVisualStyleBackColor = true;
            // 
            // tabSM4
            // 
            tabSM4.Location = new Point(4, 33);
            tabSM4.Margin = new Padding(4);
            tabSM4.Name = "tabSM4";
            tabSM4.Padding = new Padding(4);
            tabSM4.Size = new Size(2392, 1017);
            tabSM4.TabIndex = 5;
            tabSM4.Text = "SM4算法";
            tabSM4.UseVisualStyleBackColor = true;
            // 
            // tabSM2
            // 
            tabSM2.Location = new Point(4, 33);
            tabSM2.Margin = new Padding(4);
            tabSM2.Name = "tabSM2";
            tabSM2.Padding = new Padding(4);
            tabSM2.Size = new Size(2392, 1017);
            tabSM2.TabIndex = 6;
            tabSM2.Text = "SM2算法";
            tabSM2.UseVisualStyleBackColor = true;
            // 
            // tabSM3
            // 
            tabSM3.Location = new Point(4, 33);
            tabSM3.Margin = new Padding(4);
            tabSM3.Name = "tabSM3";
            tabSM3.Padding = new Padding(4);
            tabSM3.Size = new Size(2392, 1017);
            tabSM3.TabIndex = 7;
            tabSM3.Text = "SM3算法";
            tabSM3.UseVisualStyleBackColor = true;
            // 
            // tabMD5
            // 
            tabMD5.Location = new Point(4, 33);
            tabMD5.Margin = new Padding(4);
            tabMD5.Name = "tabMD5";
            tabMD5.Padding = new Padding(4);
            tabMD5.Size = new Size(2392, 1017);
            tabMD5.TabIndex = 8;
            tabMD5.Text = "MD5算法";
            tabMD5.UseVisualStyleBackColor = true;
            // 
            // tabMedicare
            // 
            tabMedicare.Location = new Point(4, 33);
            tabMedicare.Margin = new Padding(4);
            tabMedicare.Name = "tabMedicare";
            tabMedicare.Padding = new Padding(3, 4, 3, 4);
            tabMedicare.Size = new Size(2392, 1017);
            tabMedicare.TabIndex = 9;
            tabMedicare.Text = "医保接口";
            tabMedicare.UseVisualStyleBackColor = true;
            // 
            // tabAbout
            // 
            tabAbout.Location = new Point(4, 33);
            tabAbout.Margin = new Padding(4);
            tabAbout.Name = "tabAbout";
            tabAbout.Padding = new Padding(3, 4, 3, 4);
            tabAbout.Size = new Size(2392, 1017);
            tabAbout.TabIndex = 10;
            tabAbout.Text = "关于";
            tabAbout.UseVisualStyleBackColor = true;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 1049);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Padding = new Padding(1, 0, 18, 0);
            statusStrip1.Size = new Size(2400, 31);
            statusStrip1.TabIndex = 1;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(46, 24);
            toolStripStatusLabel1.Text = "就绪";
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2400, 1080);
            Controls.Add(statusStrip1);
            Controls.Add(tabControl1);
            Name = "MainForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "加解密工具";
            tabControl1.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabEcdsa;
        private System.Windows.Forms.TabPage tabRSA;
        private System.Windows.Forms.TabPage tabRSAConvert;
        private System.Windows.Forms.TabPage tabAES;
        private System.Windows.Forms.TabPage tabDES;
        private System.Windows.Forms.TabPage tabSM4;
        private System.Windows.Forms.TabPage tabSM2;
        private System.Windows.Forms.TabPage tabSM3;
        private System.Windows.Forms.TabPage tabMD5;
        private System.Windows.Forms.TabPage tabMedicare;
        private System.Windows.Forms.TabPage tabAbout;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
    }
}
