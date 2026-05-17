namespace CryptoTool.Win
{
    /// <summary>
    /// 更新提示控件，显示在窗体右上角
    /// </summary>
    public partial class UpdateNotificationControl : UserControl
    {
        #region 事件定义

        /// <summary>
        /// 更新按钮点击事件
        /// </summary>
        public event EventHandler? UpdateClicked;

        /// <summary>
        /// 关闭按钮点击事件
        /// </summary>
        public event EventHandler? CloseClicked;

        #endregion

        #region 私有字段

        private Label lblMessage = null!;
        private Button btnUpdate = null!;
        private Button btnClose = null!;
        private Panel mainPanel = null!;

        #endregion

        #region 属性

        /// <summary>
        /// 设置或获取提示消息
        /// </summary>
        public string Message
        {
            get => lblMessage?.Text ?? string.Empty;
            set
            {
                if (lblMessage != null)
                {
                    lblMessage.Text = value;
                }
            }
        }

        #endregion

        #region 构造函数

        public UpdateNotificationControl()
        {
            InitializeComponent();
            SetStyle(ControlStyles.SupportsTransparentBackColor, true);
        }

        #endregion

        #region 初始化方法

        private void InitializeComponent()
        {
            this.SuspendLayout();

            // 主面板
            mainPanel = new Panel
            {
                BackColor = Color.FromArgb(220, 53, 69), // 红色背景
                BorderStyle = BorderStyle.None,
                Size = new Size(300, 60),
                Location = new Point(0, 0)
            };

            // 消息标签
            lblMessage = new Label
            {
                Text = "发现新版本可更新",
                ForeColor = Color.White,
                Font = new Font("微软雅黑", 9F, FontStyle.Regular),
                AutoSize = false,
                Size = new Size(180, 20),
                Location = new Point(10, 8),
                TextAlign = ContentAlignment.MiddleLeft
            };

            // 更新按钮
            btnUpdate = new Button
            {
                Text = "更新",
                BackColor = Color.FromArgb(255, 255, 255, 30), // 半透明白色
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("微软雅黑", 8F, FontStyle.Regular),
                Size = new Size(50, 25),
                Location = new Point(200, 8),
                Cursor = Cursors.Hand
            };
            btnUpdate.FlatAppearance.BorderSize = 1;
            btnUpdate.FlatAppearance.BorderColor = Color.White;
            btnUpdate.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 255, 255, 50);
            btnUpdate.Click += BtnUpdate_Click;

            // 关闭按钮
            btnClose = new Button
            {
                Text = "×",
                BackColor = Color.Transparent,
                ForeColor = Color.White,
                FlatStyle = FlatStyle.Flat,
                Font = new Font("微软雅黑", 10F, FontStyle.Bold),
                Size = new Size(25, 25),
                Location = new Point(260, 8),
                Cursor = Cursors.Hand
            };
            btnClose.FlatAppearance.BorderSize = 0;
            btnClose.FlatAppearance.MouseOverBackColor = Color.FromArgb(255, 255, 255, 50);
            btnClose.Click += BtnClose_Click;

            // 添加控件到主面板
            mainPanel.Controls.Add(lblMessage);
            mainPanel.Controls.Add(btnUpdate);
            mainPanel.Controls.Add(btnClose);

            // 设置控件属性
            this.BackColor = Color.Transparent;
            this.Size = new Size(300, 60);
            this.Controls.Add(mainPanel);

            // 设置圆角效果
            SetRoundedCorners();

            this.ResumeLayout(false);
        }

        /// <summary>
        /// 设置圆角效果
        /// </summary>
        private void SetRoundedCorners()
        {
            var path = new System.Drawing.Drawing2D.GraphicsPath();
            var radius = 8;
            var rect = new Rectangle(0, 0, mainPanel.Width, mainPanel.Height);

            path.AddArc(rect.X, rect.Y, radius * 2, radius * 2, 180, 90);
            path.AddArc(rect.Right - radius * 2, rect.Y, radius * 2, radius * 2, 270, 90);
            path.AddArc(rect.Right - radius * 2, rect.Bottom - radius * 2, radius * 2, radius * 2, 0, 90);
            path.AddArc(rect.X, rect.Bottom - radius * 2, radius * 2, radius * 2, 90, 90);
            path.CloseFigure();

            mainPanel.Region = new Region(path);
        }

        #endregion

        #region 事件处理方法

        private void BtnUpdate_Click(object? sender, EventArgs e)
        {
            UpdateClicked?.Invoke(this, EventArgs.Empty);
        }

        private void BtnClose_Click(object? sender, EventArgs e)
        {
            CloseClicked?.Invoke(this, EventArgs.Empty);
        }

        #endregion

        #region 公共方法

        /// <summary>
        /// 显示更新提示（带动画效果）
        /// </summary>
        public void ShowNotification()
        {
            this.Visible = true;
            this.BringToFront();

            // 淡入动画效果
            var timer = new System.Windows.Forms.Timer { Interval = 30 };
            var opacity = 0.0;
            timer.Tick += (s, e) =>
            {
                opacity += 0.1;
                if (opacity >= 1.0)
                {
                    opacity = 1.0;
                    timer.Stop();
                    timer.Dispose();
                }
                this.BackColor = Color.FromArgb((int)(opacity * 255), this.BackColor.R, this.BackColor.G, this.BackColor.B);
            };
            timer.Start();
        }

        /// <summary>
        /// 隐藏更新提示（带动画效果）
        /// </summary>
        public void HideNotification()
        {
            // 淡出动画效果
            var timer = new System.Windows.Forms.Timer { Interval = 30 };
            var opacity = 1.0;
            timer.Tick += (s, e) =>
            {
                opacity -= 0.1;
                if (opacity <= 0)
                {
                    opacity = 0;
                    this.Visible = false;
                    timer.Stop();
                    timer.Dispose();
                }
                this.BackColor = Color.FromArgb((int)(opacity * 255), this.BackColor.R, this.BackColor.G, this.BackColor.B);
            };
            timer.Start();
        }

        #endregion
    }
}
