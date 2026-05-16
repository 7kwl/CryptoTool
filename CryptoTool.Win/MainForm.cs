using Octokit;

namespace CryptoTool.Win
{
    public partial class MainForm : Form
    {
        private RSATabControl rsaTabControl;
        private RSAConvertTabControl rsaConvertTabControl;
        private AESTabControl aesTabControl;
        private DESTabControl desTabControl;
        private SM4TabControl sm4TabControl;
        private SM2TabControl sm2TabControl;
        private SM3TabControl sm3TabControl;
        private MD5TabControl md5TabControl;
        private MedicareTabControl medicareTabControl;
        private AboutTabControl aboutTabControl;

        // 后台更新服务和通知控件
        private BackgroundUpdateService updateService;
        private UpdateNotificationControl updateNotification;
        private Release? pendingRelease;

        public MainForm()
        {
            InitializeComponent();

            // 设置窗口可调整大小
            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(1400, 800);

            InitializeTabControls();
            InitializeUpdateService();
            Load += MainForm_Load;
            FormClosed += MainForm_FormClosed;
        }

        private void InitializeTabControls()
        {
            // 创建各个用户控件
            rsaTabControl = new RSATabControl();
            rsaConvertTabControl = new RSAConvertTabControl();
            aesTabControl = new AESTabControl();
            desTabControl = new DESTabControl();
            sm4TabControl = new SM4TabControl();
            sm2TabControl = new SM2TabControl();
            sm3TabControl = new SM3TabControl();
            md5TabControl = new MD5TabControl();
            medicareTabControl = new MedicareTabControl();
            aboutTabControl = new AboutTabControl();

            // 设置控件尺寸和位置
            rsaTabControl.Dock = DockStyle.Fill;
            rsaConvertTabControl.Dock = DockStyle.Fill;
            aesTabControl.Dock = DockStyle.Fill;
            desTabControl.Dock = DockStyle.Fill;
            sm4TabControl.Dock = DockStyle.Fill;
            sm2TabControl.Dock = DockStyle.Fill;
            sm3TabControl.Dock = DockStyle.Fill;
            md5TabControl.Dock = DockStyle.Fill;
            medicareTabControl.Dock = DockStyle.Fill;
            aboutTabControl.Dock = DockStyle.Fill;

            // 将控件添加到对应的TabPage中
            tabRSA.Controls.Clear();
            tabRSA.Controls.Add(rsaTabControl);

            tabRSAConvert.Controls.Clear();
            tabRSAConvert.Controls.Add(rsaConvertTabControl);

            tabAES.Controls.Clear();
            tabAES.Controls.Add(aesTabControl);

            tabDES.Controls.Clear();
            tabDES.Controls.Add(desTabControl);

            tabSM4.Controls.Clear();
            tabSM4.Controls.Add(sm4TabControl);

            tabSM2.Controls.Clear();
            tabSM2.Controls.Add(sm2TabControl);

            tabSM3.Controls.Clear();
            tabSM3.Controls.Add(sm3TabControl);

            tabMD5.Controls.Clear();
            tabMD5.Controls.Add(md5TabControl);

            tabMedicare.Controls.Clear();
            tabMedicare.Controls.Add(medicareTabControl);

            tabAbout.Controls.Clear();
            tabAbout.Controls.Add(aboutTabControl);

            // 绑定状态更新事件
            rsaTabControl.StatusChanged += SetStatus;
            rsaConvertTabControl.StatusChanged += SetStatus;
            aesTabControl.StatusChanged += SetStatus;
            desTabControl.StatusChanged += SetStatus;
            sm4TabControl.StatusChanged += SetStatus;
            sm2TabControl.StatusChanged += SetStatus;
            sm3TabControl.StatusChanged += SetStatus;
            md5TabControl.StatusChanged += SetStatus;
            medicareTabControl.StatusChanged += SetStatus;
            aboutTabControl.StatusChanged += SetStatus;

            // 绑定医保SM4密钥生成事件到SM4控件
            medicareTabControl.SM4KeyGenerated += (key) => sm4TabControl.UpdateKeyFromMedicare(key);
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            SetStatus("就绪");

            try
            {
                updateService?.Start(5000, 7200000);
                SetStatus("后台更新检测已启动");
            }
            catch (Exception ex)
            {
                SetStatus($"启动后台更新检测失败: {ex.Message}");
            }

#if DEBUG
            this.KeyPreview = true;
            this.KeyDown += MainForm_KeyDown;
#endif
        }

        private void MainForm_FormClosed(object? sender, FormClosedEventArgs e)
        {
            updateService?.Dispose();
        }

#if DEBUG
        /// <summary>
        /// 调试模式下的快捷键处理（用于测试）
        /// </summary>
        private async void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
            // Ctrl+U 手动触发更新检测
            if (e.Control && e.KeyCode == Keys.U)
            {
                SetStatus("手动触发更新检测...");
                try
                {
                    if (updateService != null)
                    {
                        await updateService.ManualCheckAsync();
                    }
                }
                catch (Exception ex)
                {
                    SetStatus($"手动检测更新失败: {ex.Message}");
                }
            }
        }
#endif

        #region 辅助方法

        private void SetStatus(string message)
        {
            toolStripStatusLabel1.Text = message;
            System.Windows.Forms.Application.DoEvents();
        }

        #endregion

        /// <summary>
        /// 初始化后台更新服务
        /// </summary>
        private void InitializeUpdateService()
        {
            try
            {
                // 创建后台更新服务
                updateService = new BackgroundUpdateService();
                updateService.NewVersionFound += OnNewVersionFound;
                updateService.StatusUpdated += OnUpdateStatusChanged;

                // 创建更新通知控件
                updateNotification = new UpdateNotificationControl
                {
                    Visible = false,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };

                // 设置通知控件位置（右上角）
                updateNotification.Location = new Point(
                    this.ClientSize.Width - updateNotification.Width - 20, 20);

                // 绑定通知控件事件
                updateNotification.UpdateClicked += OnUpdateNotificationClicked;
                updateNotification.CloseClicked += OnUpdateNotificationClosed;

                // 添加到主窗体
                this.Controls.Add(updateNotification);
                updateNotification.BringToFront();

                // 监听窗体大小变化，调整通知位置
                this.SizeChanged += MainForm_SizeChanged;

                SetStatus("后台更新检测服务初始化完成");
            }
            catch (Exception ex)
            {
                SetStatus($"初始化更新服务失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 窗体大小变化时调整通知位置
        /// </summary>
        private void MainForm_SizeChanged(object? sender, EventArgs e)
        {
            if (updateNotification != null && this.WindowState != FormWindowState.Minimized)
            {
                updateNotification.Location = new Point(
                    this.ClientSize.Width - updateNotification.Width - 20, 20);
            }
        }

        /// <summary>
        /// 发现新版本时的处理
        /// </summary>
        private void OnNewVersionFound(Release release)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(() => OnNewVersionFound(release));
                return;
            }

            try
            {
                pendingRelease = release;
                updateNotification.Message = $"发现新版本 {release.TagName}";
                updateNotification.ShowNotification();

                SetStatus($"发现新版本 {release.TagName}，点击右上角提示进行更新");
            }
            catch (Exception ex)
            {
                SetStatus($"显示更新通知失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 更新状态变化时的处理
        /// </summary>
        private void OnUpdateStatusChanged(string status)
        {
            // 只在调试模式下显示详细的后台检测状态
#if DEBUG
            if (this.InvokeRequired)
            {
                this.Invoke(() => SetStatus(status));
            }
            else
            {
                SetStatus(status);
            }
#endif
        }

        /// <summary>
        /// 更新通知被点击时的处理
        /// </summary>
        private async void OnUpdateNotificationClicked(object? sender, EventArgs e)
        {
            if (pendingRelease == null) return;

            try
            {
                updateNotification.HideNotification();

                // 切换到关于选项卡
                tabControl1.SelectedIndex = tabControl1.TabCount - 1;

                // 开始下载更新
                await aboutTabControl.StartDownloadUpdateAsync(pendingRelease);

                pendingRelease = null;
            }
            catch (Exception ex)
            {
                SetStatus($"启动更新下载失败: {ex.Message}");
                MessageBox.Show($"启动更新下载失败: {ex.Message}", "错误",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 更新通知被关闭时的处理
        /// </summary>
        private void OnUpdateNotificationClosed(object? sender, EventArgs e)
        {
            updateNotification.HideNotification();
            pendingRelease = null;
            SetStatus("已忽略更新提示");
        }
    }
}
