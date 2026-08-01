namespace WinFormsApp2;

partial class Page2Control
{
    /// <summary> 
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary> 
    /// Clean up any resources being used.
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

    #region Component Designer generated code

    /// <summary> 
    /// Required method for Designer support - do not modify 
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
        tableLayoutPanel2 = new TableLayoutPanel();
        label2 = new Label();
        tableLayoutPanel2.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel2
        // 
        tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tableLayoutPanel2.ColumnCount = 2;
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.Controls.Add(label2, 0, 0);
        tableLayoutPanel2.Dock = DockStyle.Fill;
        tableLayoutPanel2.Location = new Point(0, 0);
        tableLayoutPanel2.Margin = new Padding(4);
        tableLayoutPanel2.Name = "tableLayoutPanel2";
        tableLayoutPanel2.RowCount = 2;
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel2.Size = new Size(2400, 676);
        tableLayoutPanel2.TabIndex = 0;
        // 
        // label2
        // 
        tableLayoutPanel2.SetColumnSpan(label2, 2);
        label2.Dock = DockStyle.Fill;
        label2.Font = new Font("微软雅黑", 16F, FontStyle.Bold);
        label2.Location = new Point(5, 1);
        label2.Margin = new Padding(4, 0, 4, 0);
        label2.Name = "label2";
        tableLayoutPanel2.SetRowSpan(label2, 2);
        label2.Size = new Size(2390, 674);
        label2.TabIndex = 0;
        label2.Text = "这是第 2 个界面";
        label2.TextAlign = ContentAlignment.MiddleCenter;
        label2.Click += label2_Click;
        // 
        // Page2Control
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel2);
        Margin = new Padding(4);
        Name = "Page2Control";
        Size = new Size(2400, 676);
        tableLayoutPanel2.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel2;
    private Label label2;
}
