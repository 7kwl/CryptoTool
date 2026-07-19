using Octokit;
using System.Diagnostics;
using System.Reflection;
using System.Runtime.InteropServices;

namespace CryptoTool.Win
{
    /// <summary>
    /// 关于软件的用户控件，包含软件信息和自动更新功能
    /// </summary>
    public partial class AboutTabControl : UserControl
    {
        #region 事件定义

        /// <summary>
        /// 状态更新事件
        /// </summary>
        public event Action<string>? StatusChanged;

        #endregion

        #region 私有字段

        private GitHubClient? _gitHubClient;
        private Release? _latestRelease;
        private readonly string _repositoryOwner = "jinjupeng";
        private readonly string _repositoryName = "CryptoTool";
        private readonly HttpClient _httpClient;

        #endregion

        #region 构造函数

        public AboutTabControl()
        {
            InitializeComponent();
            _httpClient = new HttpClient();
            InitializeGitHubClient();
            InitializeAppInfo();
        }

        #endregion

        #region GitHub 客户端初始化

        /// <summary>
        /// 初始化 GitHub 客户端，支持认证
        /// </summary>
        private void InitializeGitHubClient()
        {
            try
            {
                // 从环境变量获取 GitHub Token
                var token = Environment.GetEnvironmentVariable("GITHUB_TOKEN");

                if (!string.IsNullOrWhiteSpace(token))
                {
                    // 使用认证的客户端
                    _gitHubClient = new GitHubClient(new ProductHeaderValue("CryptoTool"))
                    {
                        Credentials = new Credentials(token)
                    };
                    SetStatus("GitHub 客户端已使用认证初始化");
                }
                else
                {
                    // 使用未认证的客户端（较低配额）
                    _gitHubClient = new GitHubClient(new ProductHeaderValue("CryptoTool"));
                    SetStatus("GitHub 客户端使用未认证模式（API 配额较低）");
                }
            }
            catch (Exception ex)
            {
                _gitHubClient = new GitHubClient(new ProductHeaderValue("CryptoTool"));
                SetStatus($"GitHub 客户端初始化警告: {ex.Message}");
            }
        }

        /// <summary>
        /// 检查 GitHub API 配额
        /// </summary>
        private async Task<(int remaining, int limit, DateTimeOffset reset)> CheckApiRateLimitAsync()
        {
            if (_gitHubClient == null)
            {
                return (0, 0, DateTimeOffset.MinValue);
            }

            try
            {
                var rateLimit = await _gitHubClient.RateLimit.GetRateLimits();
                var core = rateLimit.Resources.Core;
                return (core.Remaining, core.Limit, core.Reset);
            }
            catch (Exception)
            {
                return (0, 0, DateTimeOffset.MinValue);
            }
        }

        #endregion

        #region 初始化方法

        /// <summary>
        /// 初始化软件信息
        /// </summary>
        private void InitializeAppInfo()
        {
            try
            {
                var assembly = Assembly.GetExecutingAssembly();
                var version = assembly.GetName().Version?.ToString() ?? "未知版本";
                var location = assembly.Location;
                var appPath = Path.GetDirectoryName(location) ?? "未知路径";

                // 软件基本信息
                textAppName.Text = "CryptoTool 加解密工具";
                textAppVersion.Text = version;
                textAppAuthor.Text = "CryptoTool";
                textAppDescription.Text = "一个功能强大的加解密工具，支持RSA、SM2、SM3、SM4等多种加解密算法，" +
                                        "以及医保接口的签名验签和加解密功能。提供直观的图形界面，方便用户进行各种加解密操作。";
                linkAppRepository.Text = "https://github.com/7kwl/CryptoTool";
                textAppLicense.Text = "MIT License";

                // 当前版本信息
                textCurrentVersion.Text = version;

                // 系统信息
                textOSInfo.Text = GetOSInfo();
                textDotNetVersion.Text = Environment.Version.ToString();
                textAppPath.Text = appPath;

                // 初始状态
                textUpdateStatus.Text = "点击检查更新按钮获取最新版本信息";
                textLatestVersion.Text = "未知";

                SetStatus("软件信息初始化完成");
            }
            catch (Exception ex)
            {
                SetStatus($"初始化软件信息失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取操作系统信息
        /// </summary>
        /// <returns></returns>
        private static string GetOSInfo()
        {
            try
            {
                var osDescription = RuntimeInformation.OSDescription;
                var architecture = RuntimeInformation.OSArchitecture.ToString();
                return $"{osDescription} ({architecture})";
            }
            catch
            {
                return "未知操作系统";
            }
        }

        #endregion

        #region 事件处理方法

        /// <summary>
        /// 仓库链接点击事件
        /// </summary>
        private void LinkAppRepository_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = linkAppRepository.Text,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"无法打开链接: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// 检查更新按钮点击事件
        /// </summary>
        private async void BtnCheckUpdate_Click(object sender, EventArgs e)
        {
            btnCheckUpdate.Enabled = false;
            textUpdateStatus.Text = "正在检查更新...";
            SetStatus("正在检查更新...");

            try
            {
                // 先检查 API 配额
                var (remaining, limit, reset) = await CheckApiRateLimitAsync();
                if (remaining <= 0)
                {
                    textUpdateStatus.Text = $"GitHub API 配额已用完，重置时间: {reset:HH:mm:ss}";
                    SetStatus("GitHub API 配额已用完");
                    return;
                }

                SetStatus($"GitHub API 配额剩余: {remaining}/{limit}");
                await CheckForUpdatesAsync();
            }
            catch (Exception ex)
            {
                textUpdateStatus.Text = $"检查更新失败: {ex.Message}";
                SetStatus($"检查更新失败: {ex.Message}");
            }
            finally
            {
                btnCheckUpdate.Enabled = true;
            }
        }

        /// <summary>
        /// 下载更新按钮点击事件
        /// </summary>
        private async void BtnDownloadUpdate_Click(object sender, EventArgs e)
        {
            if (_latestRelease == null)
            {
                MessageBox.Show("未获取到最新版本信息", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnDownloadUpdate.Enabled = false;
            progressUpdate.Visible = true;
            progressUpdate.Value = 0;
            textUpdateStatus.Text = "正在下载更新...";
            SetStatus("正在下载更新...");

            try
            {
                await DownloadAndInstallUpdateAsync(_latestRelease);
            }
            catch (Exception ex)
            {
                textUpdateStatus.Text = $"下载更新失败: {ex.Message}";
                SetStatus($"下载更新失败: {ex.Message}");
                MessageBox.Show($"下载更新失败: {ex.Message}", "错误", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnDownloadUpdate.Enabled = true;
                progressUpdate.Visible = false;
            }
        }

        #endregion

        #region 更新相关方法

        /// <summary>
        /// 异步检查更新
        /// </summary>
        private async Task CheckForUpdatesAsync()
        {
            if (_gitHubClient == null)
            {
                throw new InvalidOperationException("GitHub客户端未初始化");
            }

            try
            {
                _latestRelease = await _gitHubClient.Repository.Release.GetLatest(_repositoryOwner, _repositoryName);

                textLatestVersion.Text = _latestRelease.TagName;

                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                var latestVersionString = _latestRelease.TagName.TrimStart('v');

                if (Version.TryParse(latestVersionString, out var latestVersion) && currentVersion != null)
                {
                    var comparison = currentVersion.CompareTo(latestVersion);
                    if (comparison < 0)
                    {
                        textUpdateStatus.Text = "发现新版本，可以下载更新";
                        btnDownloadUpdate.Enabled = true;
                        SetStatus("发现新版本");
                    }
                    else if (comparison == 0)
                    {
                        textUpdateStatus.Text = "当前版本是最新版本";
                        btnDownloadUpdate.Enabled = false;
                        SetStatus("当前版本是最新版本");
                    }
                    else
                    {
                        textUpdateStatus.Text = "当前版本比最新版本更高（开发版本）";
                        btnDownloadUpdate.Enabled = false;
                        SetStatus("当前版本比最新版本更高");
                    }
                }
                else
                {
                    textUpdateStatus.Text = "版本比较失败，但已获取到最新版本信息";
                    btnDownloadUpdate.Enabled = true;
                    SetStatus("版本比较失败");
                }
            }
            catch (RateLimitExceededException ex)
            {
                var resetTime = ex.Reset.ToString("HH:mm:ss");
                textUpdateStatus.Text = $"GitHub API 请求次数限制，重置时间: {resetTime}";
                SetStatus($"GitHub API 请求次数限制，重置时间: {resetTime}");

                MessageBox.Show(
                    $"GitHub API 请求次数已达到限制。\n\n" +
                    $"建议解决方案：\n" +
                    $"1. 设置环境变量 GITHUB_TOKEN 以获得更高的API配额\n" +
                    $"2. 等待配额重置时间: {resetTime}\n\n" +
                    $"如需设置Token，请访问: https://github.com/settings/tokens",
                    "API 配额限制",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
            catch (NotFoundException)
            {
                textUpdateStatus.Text = "未找到软件仓库或版本信息";
                SetStatus("未找到软件仓库");
            }
            catch (Exception ex)
            {
                textUpdateStatus.Text = $"检查更新失败: {ex.Message}";
                SetStatus($"检查更新失败: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 静默检查更新（用于后台检测）
        /// </summary>
        /// <returns>如果有新版本返回 Release 对象，否则返回 null</returns>
        public async Task<Release?> SilentCheckForUpdatesAsync()
        {
            if (_gitHubClient == null)
            {
                return null;
            }

            try
            {
                // 先检查配额
                var (remaining, _, _) = await CheckApiRateLimitAsync();
                if (remaining <= 5) // 保留少量配额给用户手动检查
                {
                    return null;
                }

                var latestRelease = await _gitHubClient.Repository.Release.GetLatest(_repositoryOwner, _repositoryName);

                var currentVersion = Assembly.GetExecutingAssembly().GetName().Version;
                var latestVersionString = latestRelease.TagName.TrimStart('v');

                if (Version.TryParse(latestVersionString, out var latestVersion) && currentVersion != null)
                {
                    var comparison = currentVersion.CompareTo(latestVersion);
                    if (comparison < 0)
                    {
                        return latestRelease; // 发现新版本
                    }
                }

                return null; // 没有新版本
            }
            catch (Exception)
            {
                return null; // 检测失败，静默忽略
            }
        }

        /// <summary>
        /// 开始下载更新流程（从外部调用）
        /// </summary>
        /// <param name="release">要下载的版本</param>
        public async Task StartDownloadUpdateAsync(Release release)
        {
            _latestRelease = release;
            textLatestVersion.Text = release.TagName;
            btnDownloadUpdate.Enabled = false;

            // 切换到关于选项卡
            var parentForm = this.FindForm();
            if (parentForm is MainForm mainForm)
            {
                // 假设关于选项卡是最后一个
                var tabControl = mainForm.Controls.OfType<TabControl>().FirstOrDefault();
                if (tabControl != null)
                {
                    tabControl.SelectedIndex = tabControl.TabCount - 1; // 切换到关于选项卡
                }
            }

            await DownloadAndInstallUpdateAsync(release);
        }

        /// <summary>
        /// 下载并安装更新
        /// </summary>
        private async Task DownloadAndInstallUpdateAsync(Release release)
        {
            // 查找Windows可执行文件或安装包
            ReleaseAsset? installerAsset = null;

            // 优先查找可执行文件
            foreach (var asset in release.Assets)
            {
                // 查找exe文件（优先）
                if (asset.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) &&
                    (asset.Name.Contains("win", StringComparison.OrdinalIgnoreCase) ||
                     asset.Name.Contains("windows", StringComparison.OrdinalIgnoreCase) ||
                     asset.Name.Contains("CryptoTool", StringComparison.OrdinalIgnoreCase)))
                {
                    installerAsset = asset;
                    break;
                }
            }

            // 如果没找到exe，查找MSI安装包
            if (installerAsset == null)
            {
                foreach (var asset in release.Assets)
                {
                    if (asset.Name.EndsWith(".msi", StringComparison.OrdinalIgnoreCase))
                    {
                        installerAsset = asset;
                        break;
                    }
                }
            }

            // 如果还是没找到，查找ZIP包
            if (installerAsset == null)
            {
                foreach (var asset in release.Assets)
                {
                    if (asset.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase) &&
                        (asset.Name.Contains("win", StringComparison.OrdinalIgnoreCase) ||
                         asset.Name.Contains("windows", StringComparison.OrdinalIgnoreCase) ||
                         asset.Name.Contains("CryptoTool", StringComparison.OrdinalIgnoreCase)))
                    {
                        installerAsset = asset;
                        break;
                    }
                }
            }

            if (installerAsset == null)
            {
                throw new InvalidOperationException("未找到适用于Windows的安装包或可执行文件");
            }

            var tempPath = Path.GetTempPath();
            var downloadPath = Path.Combine(tempPath, installerAsset.Name);

            try
            {
                // 下载文件
                var progress = new Progress<int>(value =>
                {
                    if (progressUpdate.InvokeRequired)
                    {
                        progressUpdate.Invoke(() =>
                        {
                            progressUpdate.Value = value;
                            textUpdateStatus.Text = $"正在下载更新... ({value}%)";
                        });
                    }
                    else
                    {
                        progressUpdate.Value = value;
                        textUpdateStatus.Text = $"正在下载更新... ({value}%)";
                    }
                    SetStatus($"正在下载更新... ({value}%)");
                });

                await DownloadFileWithProgressAsync(installerAsset.BrowserDownloadUrl, downloadPath, progress);

                textUpdateStatus.Text = "下载完成，准备安装...";
                SetStatus("下载完成，准备安装...");

                // 根据文件类型处理不同的安装方式
                if (installerAsset.Name.EndsWith(".exe", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleExecutableUpdate(downloadPath, release, installerAsset);
                }
                else if (installerAsset.Name.EndsWith(".msi", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleMsiUpdate(downloadPath, release, installerAsset);
                }
                else if (installerAsset.Name.EndsWith(".zip", StringComparison.OrdinalIgnoreCase))
                {
                    await HandleZipUpdate(downloadPath, release, installerAsset);
                }
            }
            catch (Exception ex)
            {
                // 清理下载的文件
                try
                {
                    if (File.Exists(downloadPath))
                    {
                        File.Delete(downloadPath);
                    }
                }
                catch { }

                throw new InvalidOperationException($"下载或处理更新失败: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// 处理可执行文件更新
        /// </summary>
        private async Task HandleExecutableUpdate(string downloadPath, Release release, ReleaseAsset asset)
        {
            var result = MessageBox.Show(
                $"更新程序已下载完成，是否立即替换当前程序？\n\n版本: {release.TagName}\n文件: {asset.Name}\n\n点击\"是\"将关闭应用程序并更新到新版本。",
                "安装更新",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                try
                {
                    // 获取当前应用程序路径
                    var currentExePath = System.Windows.Forms.Application.ExecutablePath;
                    var backupPath = currentExePath + ".backup";

                    // 创建更新脚本
                    var updateScript = Path.Combine(Path.GetTempPath(), "CryptoTool_Update.bat");
                    var scriptContent = "@echo off\r\n" +
                        "echo 正在更新 CryptoTool...\r\n" +
                        "timeout /t 2 /nobreak >nul\r\n" +
                        "\r\n" +
                        "echo 备份当前版本...\r\n" +
                        $"if exist \"{backupPath}\" del \"{backupPath}\"\r\n" +
                        $"move \"{currentExePath}\" \"{backupPath}\"\r\n" +
                        "\r\n" +
                        "echo 安装新版本...\r\n" +
                        $"move \"{downloadPath}\" \"{currentExePath}\"\r\n" +
                        "\r\n" +
                        "echo 启动新版本...\r\n" +
                        $"start \"\" \"{currentExePath}\"\r\n" +
                        "\r\n" +
                        "echo 清理临时文件...\r\n" +
                        "timeout /t 2 /nobreak >nul\r\n" +
                        $"if exist \"{backupPath}\" del \"{backupPath}\"\r\n" +
                        "del \"%~f0\"\r\n";

                    await File.WriteAllTextAsync(updateScript, scriptContent, System.Text.Encoding.UTF8);

                    // 启动更新脚本
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = updateScript,
                        UseShellExecute = true,
                        WindowStyle = ProcessWindowStyle.Hidden
                    });

                    // 关闭应用程序
                    System.Windows.Forms.Application.Exit();
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"自动更新失败: {ex.Message}\n\n请手动替换程序文件：\n{downloadPath}",
                        "更新失败", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            else
            {
                textUpdateStatus.Text = $"更新程序已下载到: {downloadPath}";
                SetStatus("更新程序下载完成，用户选择稍后手动安装");

                MessageBox.Show($"更新程序已下载到临时目录：\n{downloadPath}\n\n请手动替换当前程序文件以完成更新。",
                    "下载完成", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        /// <summary>
        /// 处理MSI安装包更新
        /// </summary>
        private Task HandleMsiUpdate(string downloadPath, Release release, ReleaseAsset asset)
        {
            var result = MessageBox.Show(
                $"MSI安装包已下载完成，是否立即安装？\n\n版本: {release.TagName}\n文件: {asset.Name}\n\n点击\"是\"将启动安装程序。",
                "安装更新",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                Process.Start(new ProcessStartInfo
                {
                    FileName = "msiexec",
                    Arguments = $"/i \"{downloadPath}\" /quiet",
                    UseShellExecute = true
                });

                textUpdateStatus.Text = "MSI安装程序已启动";
                SetStatus("MSI安装程序已启动");
            }
            else
            {
                textUpdateStatus.Text = $"MSI安装包已下载到: {downloadPath}";
                SetStatus("MSI安装包下载完成，用户选择稍后安装");
            }
            return Task.CompletedTask;
        }

        /// <summary>
        /// 处理ZIP压缩包更新
        /// </summary>
        private Task HandleZipUpdate(string downloadPath, Release release, ReleaseAsset asset)
        {
            var result = MessageBox.Show(
                $"程序压缩包已下载完成。\n\n版本: {release.TagName}\n文件: {asset.Name}\n\n请手动解压并替换程序文件。\n\n是否打开下载文件夹？",
                "下载完成",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Information);

            if (result == DialogResult.Yes)
            {
                // 打开包含下载文件的文件夹
                Process.Start(new ProcessStartInfo
                {
                    FileName = "explorer",
                    Arguments = $"/select,\"{downloadPath}\"",
                    UseShellExecute = true
                });
            }

            textUpdateStatus.Text = $"ZIP压缩包已下载到: {downloadPath}";
            SetStatus("ZIP压缩包下载完成");
            return Task.CompletedTask;
        }

        /// <summary>
        /// 带进度的文件下载
        /// </summary>
        private async Task DownloadFileWithProgressAsync(string url, string destinationPath, IProgress<int> progress)
        {
            using var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();

            var totalBytes = response.Content.Headers.ContentLength ?? -1L;
            var downloadedBytes = 0L;

            using var contentStream = await response.Content.ReadAsStreamAsync();
            using var fileStream = new FileStream(destinationPath, System.IO.FileMode.Create, System.IO.FileAccess.Write, System.IO.FileShare.None);

            var buffer = new byte[8192];
            var bytesRead = 0;

            while ((bytesRead = await contentStream.ReadAsync(buffer)) > 0)
            {
                await fileStream.WriteAsync(buffer.AsMemory(0, bytesRead));
                downloadedBytes += bytesRead;

                if (totalBytes > 0)
                {
                    var percentage = (int)((downloadedBytes * 100) / totalBytes);
                    progress.Report(percentage);
                }
            }
        }

        #endregion

        #region 辅助方法

        /// <summary>
        /// 设置状态信息
        /// </summary>
        /// <param name="message">状态消息</param>
        private void SetStatus(string message)
        {
            StatusChanged?.Invoke(message);
        }

        #endregion
    }
}