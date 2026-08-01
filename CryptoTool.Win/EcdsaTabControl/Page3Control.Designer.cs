namespace WinFormsApp2;

partial class Page3Control
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
        tableLayoutPanel3 = new TableLayoutPanel();
        label3 = new Label();
        tableLayoutPanel3.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel3
        // 
        tableLayoutPanel3.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tableLayoutPanel3.ColumnCount = 2;
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.Controls.Add(label3, 0, 0);
        tableLayoutPanel3.Dock = DockStyle.Fill;
        tableLayoutPanel3.Location = new Point(0, 0);
        tableLayoutPanel3.Margin = new Padding(4);
        tableLayoutPanel3.Name = "tableLayoutPanel3";
        tableLayoutPanel3.RowCount = 2;
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel3.Size = new Size(2400, 676);
        tableLayoutPanel3.TabIndex = 0;
        // 
        // label3
        // 
        label3.Dock = DockStyle.Fill;
        label3.Font = new Font("Microsoft YaHei", 16F, FontStyle.Bold);
        label3.Location = new Point(5, 2);
        label3.Margin = new Padding(4, 0, 4, 0);
        label3.Name = "label3";
        label3.Size = new Size(1275, 313);
        label3.TabIndex = 0;
        label3.Text = "这是第 3 个界面";
        label3.TextAlign = ContentAlignment.MiddleCenter;
        tableLayoutPanel3.SetColumnSpan(label3, 2);
        tableLayoutPanel3.SetRowSpan(label3, 2);
        // 
        // Page3Control
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel3);
        Margin = new Padding(4);
        Name = "Page3Control";
        Size = new Size(2400, 676);
        tableLayoutPanel3.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel3;
    private Label label3;
}
