namespace WinFormsApp2;

partial class Page4Control
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
        tableLayoutPanel4 = new TableLayoutPanel();
        label4 = new Label();
        tableLayoutPanel4.SuspendLayout();
        SuspendLayout();
        // 
        // tableLayoutPanel4
        // 
        tableLayoutPanel4.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;
        tableLayoutPanel4.ColumnCount = 2;
        tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        tableLayoutPanel4.Controls.Add(label4, 0, 0);
        tableLayoutPanel4.Dock = DockStyle.Fill;
        tableLayoutPanel4.Location = new Point(0, 0);
        tableLayoutPanel4.Margin = new Padding(4);
        tableLayoutPanel4.Name = "tableLayoutPanel4";
        tableLayoutPanel4.RowCount = 2;
        tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 50F));
        tableLayoutPanel4.Size = new Size(2400, 676);
        tableLayoutPanel4.TabIndex = 0;
        // 
        // label4
        // 
        label4.Dock = DockStyle.Fill;
        label4.Font = new Font("Microsoft YaHei", 16F, FontStyle.Bold);
        label4.Location = new Point(5, 2);
        label4.Margin = new Padding(4, 0, 4, 0);
        label4.Name = "label4";
        label4.Size = new Size(1275, 313);
        label4.TabIndex = 0;
        label4.Text = "这是第 4 个界面";
        label4.TextAlign = ContentAlignment.MiddleCenter;
        tableLayoutPanel4.SetColumnSpan(label4, 2);
        tableLayoutPanel4.SetRowSpan(label4, 2);
        // 
        // Page4Control
        // 
        AutoScaleDimensions = new SizeF(11F, 24F);
        AutoScaleMode = AutoScaleMode.Font;
        Controls.Add(tableLayoutPanel4);
        Margin = new Padding(4);
        Name = "Page4Control";
        Size = new Size(2400, 676);
        tableLayoutPanel4.ResumeLayout(false);
        ResumeLayout(false);
    }

    #endregion

    private TableLayoutPanel tableLayoutPanel4;
    private Label label4;
}
