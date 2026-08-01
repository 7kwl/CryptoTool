namespace CryptoTool.Win
{
    partial class EcdsaTabControl
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
            components = new System.ComponentModel.Container();
            contextMenuStrip1 = new ContextMenuStrip(components);
            printDialog1 = new PrintDialog();
            process1 = new System.Diagnostics.Process();
            process2 = new System.Diagnostics.Process();
            tableLayoutPanel1 = new TableLayoutPanel();
            button10 = new Button();
            button11 = new Button();
            button12 = new Button();
            button13 = new Button();
            flowLayoutPanel1 = new FlowLayoutPanel();
            tableLayoutPanel2 = new TableLayoutPanel();
            flowLayoutPanel1.SuspendLayout();
            tableLayoutPanel2.SuspendLayout();
            SuspendLayout();
            // 
            // contextMenuStrip1
            // 
            contextMenuStrip1.ImageScalingSize = new Size(24, 24);
            contextMenuStrip1.Name = "contextMenuStrip1";
            contextMenuStrip1.Size = new Size(61, 4);
            // 
            // printDialog1
            // 
            printDialog1.UseEXDialog = true;
            // 
            // process1
            // 
            process1.StartInfo.Domain = "";
            process1.StartInfo.LoadUserProfile = false;
            process1.StartInfo.Password = null;
            process1.StartInfo.StandardErrorEncoding = null;
            process1.StartInfo.StandardInputEncoding = null;
            process1.StartInfo.StandardOutputEncoding = null;
            process1.StartInfo.UseCredentialsForNetworkingOnly = false;
            process1.StartInfo.UserName = "";
            process1.SynchronizingObject = this;
            // 
            // process2
            // 
            process2.StartInfo.Domain = "";
            process2.StartInfo.LoadUserProfile = false;
            process2.StartInfo.Password = null;
            process2.StartInfo.StandardErrorEncoding = null;
            process2.StartInfo.StandardInputEncoding = null;
            process2.StartInfo.StandardOutputEncoding = null;
            process2.StartInfo.UseCredentialsForNetworkingOnly = false;
            process2.StartInfo.UserName = "";
            process2.SynchronizingObject = this;
            // 
            // tableLayoutPanel1
            // 
            tableLayoutPanel1.ColumnCount = 2;
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Location = new Point(6, 509);
            tableLayoutPanel1.Name = "tableLayoutPanel1";
            tableLayoutPanel1.RowCount = 2;
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
            tableLayoutPanel1.Size = new Size(1289, 321);
            tableLayoutPanel1.TabIndex = 8;
            // 
            // button10
            // 
            button10.Location = new Point(13, 18);
            button10.Name = "button10";
            button10.Size = new Size(150, 50);
            button10.TabIndex = 15;
            button10.Text = "子界面1";
            button10.UseVisualStyleBackColor = true;
            // 
            // button11
            // 
            button11.Location = new Point(169, 18);
            button11.Name = "button11";
            button11.Size = new Size(150, 50);
            button11.TabIndex = 16;
            button11.Text = "子界面2";
            button11.UseVisualStyleBackColor = true;
            // 
            // button12
            // 
            button12.Location = new Point(325, 18);
            button12.Name = "button12";
            button12.Size = new Size(150, 50);
            button12.TabIndex = 17;
            button12.Text = "子界面3";
            button12.UseVisualStyleBackColor = true;
            // 
            // button13
            // 
            button13.Location = new Point(481, 18);
            button13.Name = "button13";
            button13.Size = new Size(150, 50);
            button13.TabIndex = 18;
            button13.Text = "子界面4";
            button13.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.Controls.Add(button10);
            flowLayoutPanel1.Controls.Add(button11);
            flowLayoutPanel1.Controls.Add(button12);
            flowLayoutPanel1.Controls.Add(button13);
            flowLayoutPanel1.Dock = DockStyle.Fill;
            flowLayoutPanel1.Location = new Point(3, 3);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Padding = new Padding(10, 15, 0, 0);
            flowLayoutPanel1.Size = new Size(2394, 100);
            flowLayoutPanel1.TabIndex = 0;
            flowLayoutPanel1.WrapContents = false;
            // 
            // tableLayoutPanel2
            // 
            tableLayoutPanel2.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tableLayoutPanel2.ColumnCount = 1;
            tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Controls.Add(flowLayoutPanel1, 0, 0);
            tableLayoutPanel2.Location = new Point(0, 404);
            tableLayoutPanel2.Name = "tableLayoutPanel2";
            tableLayoutPanel2.RowCount = 2;
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Absolute, 106F));
            tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            tableLayoutPanel2.Size = new Size(2400, 676);
            tableLayoutPanel2.TabIndex = 14;
            // 
            // EcdsaTabControl
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(2400, 1080);
            Controls.Add(tableLayoutPanel2);
            Controls.Add(tableLayoutPanel1);
            Name = "EcdsaTabControl";
            Text = "EcdsaTabControl";
            flowLayoutPanel1.ResumeLayout(false);
            tableLayoutPanel2.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private ContextMenuStrip contextMenuStrip1;
        private PrintDialog printDialog1;
        private System.Diagnostics.Process process1;
        private System.Diagnostics.Process process2;
        private TableLayoutPanel tableLayoutPanel1;
        private Button button10;
        private Button button11;
        private Button button12;
        private Button button13;
        private FlowLayoutPanel flowLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
    }
}
