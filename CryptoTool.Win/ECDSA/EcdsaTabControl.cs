namespace CryptoTool.Win
{
    public partial class EcdsaTabControl : Form
    {
        public event Action<string>? StatusChanged;

        // 4 个独立可设计的切换界面（每个都有各自的 Designer.cs）
        private Page1Control? pageControl1;
        private Page2Control? pageControl2;
        private Page3Control? pageControl3;
        private Page4Control? pageControl4;
        private bool isInitialized = false;

        public EcdsaTabControl()
        {
            InitializeComponent();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);

            if (isInitialized) return;
            if (DesignMode) return;

            isInitialized = true;

            // 设计器中的 tableLayoutPanel1 不再使用，运行时隐藏
            if (tableLayoutPanel1 != null) tableLayoutPanel1.Visible = false;

            // tableLayoutPanel2 作为导航栏，显示边框便于观察
            if (tableLayoutPanel2 != null) tableLayoutPanel2.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            // 创建 4 个独立可设计的切换界面，添加到 tableLayoutPanel2 的内容行（第 1 行）
            pageControl1 = new Page1Control { Dock = DockStyle.Fill, Name = "pageControl1" };
            pageControl2 = new Page2Control { Dock = DockStyle.Fill, Name = "pageControl2" };
            pageControl3 = new Page3Control { Dock = DockStyle.Fill, Name = "pageControl3" };
            pageControl4 = new Page4Control { Dock = DockStyle.Fill, Name = "pageControl4" };

            if (tableLayoutPanel2 != null)
            {
                tableLayoutPanel2.Controls.Add(pageControl1, 0, 1);
                tableLayoutPanel2.Controls.Add(pageControl2, 0, 1);
                tableLayoutPanel2.Controls.Add(pageControl3, 0, 1);
                tableLayoutPanel2.Controls.Add(pageControl4, 0, 1);
            }

            // 绑定按钮事件
            button10.Click += Button10_Click;
            button11.Click += Button11_Click;
            button12.Click += Button12_Click;
            button13.Click += Button13_Click;

            // 默认显示第一个面板
            ShowPanel(1);
        }

        private void ShowPanel(int index)
        {
            if (pageControl1 != null) pageControl1.Visible = (index == 1);
            if (pageControl2 != null) pageControl2.Visible = (index == 2);
            if (pageControl3 != null) pageControl3.Visible = (index == 3);
            if (pageControl4 != null) pageControl4.Visible = (index == 4);

            if (index == 1 && pageControl1 != null) pageControl1.BringToFront();
            if (index == 2 && pageControl2 != null) pageControl2.BringToFront();
            if (index == 3 && pageControl3 != null) pageControl3.BringToFront();
            if (index == 4 && pageControl4 != null) pageControl4.BringToFront();

            StatusChanged?.Invoke($"已切换到子界面 {index}");
        }

        private void Button10_Click(object? sender, EventArgs e) => ShowPanel(1);
        private void Button11_Click(object? sender, EventArgs e) => ShowPanel(2);
        private void Button12_Click(object? sender, EventArgs e) => ShowPanel(3);
        private void Button13_Click(object? sender, EventArgs e) => ShowPanel(4);
    }
}
