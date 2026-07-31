namespace WinFormsApp2
{
    partial class Form1
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
            button1 = new Button();
            checkBox1 = new CheckBox();
            linkLabel1 = new LinkLabel();
            comboBox1 = new ComboBox();
            treeView1 = new TreeView();
            numericUpDown1 = new NumericUpDown();
            flowLayoutPanel1 = new FlowLayoutPanel();
            splitContainer1 = new SplitContainer();
            contextMenuStrip1 = new ContextMenuStrip(components);
            printDialog1 = new PrintDialog();
            richTextBox1 = new RichTextBox();
            dateTimePicker1 = new DateTimePicker();
            trackBar1 = new TrackBar();
            process1 = new System.Diagnostics.Process();
            process2 = new System.Diagnostics.Process();
            listView1 = new ListView();
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)trackBar1).BeginInit();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(339, 84);
            button1.Name = "button1";
            button1.Size = new Size(112, 34);
            button1.TabIndex = 0;
            button1.Text = "button1";
            button1.UseVisualStyleBackColor = true;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(466, 88);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(127, 28);
            checkBox1.TabIndex = 1;
            checkBox1.Text = "checkBox1";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // linkLabel1
            // 
            linkLabel1.AutoSize = true;
            linkLabel1.Location = new Point(219, 255);
            linkLabel1.Name = "linkLabel1";
            linkLabel1.Size = new Size(98, 24);
            linkLabel1.TabIndex = 2;
            linkLabel1.TabStop = true;
            linkLabel1.Text = "linkLabel1";
            // 
            // comboBox1
            // 
            comboBox1.FormattingEnabled = true;
            comboBox1.Location = new Point(743, 168);
            comboBox1.Name = "comboBox1";
            comboBox1.Size = new Size(182, 32);
            comboBox1.TabIndex = 3;
            // 
            // treeView1
            // 
            treeView1.Location = new Point(12, 541);
            treeView1.Name = "treeView1";
            treeView1.Size = new Size(396, 289);
            treeView1.TabIndex = 4;
            // 
            // numericUpDown1
            // 
            numericUpDown1.Location = new Point(596, 401);
            numericUpDown1.Name = "numericUpDown1";
            numericUpDown1.Size = new Size(180, 30);
            numericUpDown1.TabIndex = 5;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.FromArgb(128, 255, 128);
            flowLayoutPanel1.ForeColor = Color.FromArgb(128, 255, 128);
            flowLayoutPanel1.Location = new Point(64, 461);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(677, 343);
            flowLayoutPanel1.TabIndex = 6;
            // 
            // splitContainer1
            // 
            splitContainer1.Location = new Point(457, 88);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Size = new Size(478, 343);
            splitContainer1.SplitterDistance = 240;
            splitContainer1.TabIndex = 7;
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
            // richTextBox1
            // 
            richTextBox1.Location = new Point(908, 604);
            richTextBox1.Name = "richTextBox1";
            richTextBox1.Size = new Size(150, 144);
            richTextBox1.TabIndex = 9;
            richTextBox1.Text = "";
            // 
            // dateTimePicker1
            // 
            dateTimePicker1.Location = new Point(974, 322);
            dateTimePicker1.Name = "dateTimePicker1";
            dateTimePicker1.Size = new Size(300, 30);
            dateTimePicker1.TabIndex = 10;
            // 
            // trackBar1
            // 
            trackBar1.Location = new Point(767, 560);
            trackBar1.Name = "trackBar1";
            trackBar1.Size = new Size(156, 69);
            trackBar1.TabIndex = 11;
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
            // listView1
            // 
            listView1.Location = new Point(1017, 69);
            listView1.Name = "listView1";
            listView1.Size = new Size(182, 146);
            listView1.TabIndex = 12;
            listView1.UseCompatibleStateImageBehavior = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(11F, 24F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1309, 842);
            Controls.Add(listView1);
            Controls.Add(trackBar1);
            Controls.Add(dateTimePicker1);
            Controls.Add(richTextBox1);
            Controls.Add(splitContainer1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(numericUpDown1);
            Controls.Add(treeView1);
            Controls.Add(comboBox1);
            Controls.Add(linkLabel1);
            Controls.Add(checkBox1);
            Controls.Add(button1);
            Name = "Form1";
            Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)numericUpDown1).EndInit();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)trackBar1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private CheckBox checkBox1;
        private LinkLabel linkLabel1;
        private ComboBox comboBox1;
        private TreeView treeView1;
        private NumericUpDown numericUpDown1;
        private FlowLayoutPanel flowLayoutPanel1;
        private SplitContainer splitContainer1;
        private ContextMenuStrip contextMenuStrip1;
        private PrintDialog printDialog1;
        private RichTextBox richTextBox1;
        private DateTimePicker dateTimePicker1;
        private TrackBar trackBar1;
        private System.Diagnostics.Process process1;
        private ListView listView1;
        private System.Diagnostics.Process process2;
    }
}
