using Octokit;

namespace CryptoTool.Win
{
    public partial class MainForm : Form
    {
        private RSATabControl rsaTabControl = null!;
        private RSAConvertTabControl rsaConvertTabControl = null!;
        private AESTabControl aesTabControl = null!;
        private DESTabControl desTabControl = null!;
        private SM4TabControl sm4TabControl = null!;
        private SM2TabControl sm2TabControl = null!;
        private SM3TabControl sm3TabControl = null!;
        private EcdsaTabControl ecdsaTabControl = null!;
        private MD5TabControl md5TabControl = null!;
        private MedicareTabControl medicareTabControl = null!;
        private AboutTabControl aboutTabControl = null!;

        private BackgroundUpdateService updateService = null!;
        private UpdateNotificationControl updateNotification = null!;
        private Release? pendingRelease;

        public MainForm()
        {
            InitializeComponent();

            this.WindowState = FormWindowState.Maximized;
            this.MinimumSize = new Size(2400, 1400);

            InitializeTabControls();
            InitializeUpdateService();
            Load += MainForm_Load;
            FormClosed += MainForm_FormClosed;
        }

        private void InitializeTabControls()
        {
            rsaTabControl = new RSATabControl();
            rsaConvertTabControl = new RSAConvertTabControl();
            aesTabControl = new AESTabControl();
            desTabControl = new DESTabControl();
            sm4TabControl = new SM4TabControl();
            sm2TabControl = new SM2TabControl();
            sm3TabControl = new SM3TabControl();
            ecdsaTabControl = new EcdsaTabControl();
            md5TabControl = new MD5TabControl();
            medicareTabControl = new MedicareTabControl();
            aboutTabControl = new AboutTabControl();

            rsaTabControl.Dock = DockStyle.Fill;
            rsaConvertTabControl.Dock = DockStyle.Fill;
            aesTabControl.Dock = DockStyle.Fill;
            desTabControl.Dock = DockStyle.Fill;
            sm4TabControl.Dock = DockStyle.Fill;
            sm2TabControl.Dock = DockStyle.Fill;
            sm3TabControl.Dock = DockStyle.Fill;
            ecdsaTabControl.Dock = DockStyle.Fill;
            md5TabControl.Dock = DockStyle.Fill;
            medicareTabControl.Dock = DockStyle.Fill;
            aboutTabControl.Dock = DockStyle.Fill;

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

            // ECDSA TabPage - 使用索引避免命名问题
            // 放在 SM3 之后、MD5 之前（索引 7）
            if (tabControl1.TabPages.Count >= 8)
            {
                tabControl1.TabPages[7].Controls.Clear();
                tabControl1.TabPages[7].Controls.Add(ecdsaTabControl);
                tabControl1.TabPages[7].Text = "ECDSA";
            }

            tabMD5.Controls.Clear();
            tabMD5.Controls.Add(md5TabControl);

            tabMedicare.Controls.Clear();
            tabMedicare.Controls.Add(medicareTabControl);

            tabAbout.Controls.Clear();
            tabAbout.Controls.Add(aboutTabControl);

            rsaTabControl.StatusChanged += SetStatus;
            rsaConvertTabControl.StatusChanged += SetStatus;
            aesTabControl.StatusChanged += SetStatus;
            desTabControl.StatusChanged += SetStatus;
            sm4TabControl.StatusChanged += SetStatus;
            sm2TabControl.StatusChanged += SetStatus;
            sm3TabControl.StatusChanged += SetStatus;
            ecdsaTabControl.StatusChanged += SetStatus;
            md5TabControl.StatusChanged += SetStatus;
            medicareTabControl.StatusChanged += SetStatus;
            aboutTabControl.StatusChanged += SetStatus;

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
        private async void MainForm_KeyDown(object? sender, KeyEventArgs e)
        {
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

        private void InitializeUpdateService()
        {
            try
            {
                updateService = new BackgroundUpdateService();
                updateService.NewVersionFound += OnNewVersionFound;
                updateService.StatusUpdated += OnUpdateStatusChanged;

                updateNotification = new UpdateNotificationControl
                {
                    Visible = false,
                    Anchor = AnchorStyles.Top | AnchorStyles.Right
                };

                updateNotification.Location = new Point(
                    this.ClientSize.Width - updateNotification.Width - 20, 20);

                updateNotification.UpdateClicked += OnUpdateNotificationClicked;
                updateNotification.CloseClicked += OnUpdateNotificationClosed;

                this.Controls.Add(updateNotification);
                updateNotification.BringToFront();

                this.SizeChanged += MainForm_SizeChanged;

                SetStatus("后台更新检测服务初始化完成");
            }
            catch (Exception ex)
            {
                SetStatus($"初始化更新服务失败: {ex.Message}");
            }
        }

        private void MainForm_SizeChanged(object? sender, EventArgs e)
        {
            if (updateNotification != null && this.WindowState != FormWindowState.Minimized)
            {
                updateNotification.Location = new Point(
                    this.ClientSize.Width - updateNotification.Width - 20, 20);
            }
        }

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

        private void OnUpdateStatusChanged(string status)
        {
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

        private async void OnUpdateNotificationClicked(object? sender, EventArgs e)
        {
            if (pendingRelease == null) return;

            try
            {
                updateNotification.HideNotification();
                tabControl1.SelectedIndex = tabControl1.TabCount - 1;
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

        private void OnUpdateNotificationClosed(object? sender, EventArgs e)
        {
            updateNotification.HideNotification();
            pendingRelease = null;
            SetStatus("已忽略更新提示");
        }
    }
}