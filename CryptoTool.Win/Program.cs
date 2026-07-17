using System;
using System.Globalization;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace CryptoTool.Win
{
    internal static class Program
    {
        /// <summary>
        /// 应用程序的主入口点
        /// </summary>
        [STAThread]
        static void Main()
        {
            // ==========================================
            // ✅ 核心修改1：全局异常捕获（彻底解决崩溃弹调试器问题）
            // ==========================================
            // 设置异常捕获模式：优先由代码处理，不直接拉起调试器
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            
            // 捕获UI线程异常（界面点击、控件加载触发的异常，比如之前的ArgumentNullException）
            Application.ThreadException += (sender, args) =>
            {
                MessageBox.Show(
                    $"程序运行错误：{args.Exception.Message}\n\n详细堆栈：{args.Exception.StackTrace}",
                    "运行时错误",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error
                );
                // 输出到控制台方便调试
                Console.WriteLine($"[UI线程异常] {args.Exception}");
            };
            
            // 捕获非UI线程异常（后台任务触发的异常）
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                if (args.ExceptionObject is Exception ex)
                {
                    MessageBox.Show(
                        $"程序致命错误：{ex.Message}\n\n详细堆栈：{ex.StackTrace}",
                        "致命错误",
                        MessageBoxButtons.OK,
                        MessageBoxIcon.Stop
                    );
                    Console.WriteLine($"[非UI线程异常] {ex}");
                }
            };

            // ==========================================
            // ✅ 保留原有正确的编码设置
            // ==========================================
            try
            {
                // UTF-8编码支持中文输入输出
                Console.OutputEncoding = Encoding.UTF8;
                Console.InputEncoding = Encoding.UTF8;
                // 注册编码提供器，支持更多字符集
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            }
            catch
            {
                // 无控制台的环境（比如发布后的exe）可能无法设置编码，忽略即可
            }

            // ==========================================
            // ✅ 核心修改2：修正文化设置（避免中文显示异常）
            // ==========================================
            // ❌ 移除原有全局InvariantCulture设置：会导致中文界面、中文资源加载异常，甚至影响控件布局
            // ✅ 改用系统默认文化，确保中文正常显示
            Thread.CurrentThread.CurrentCulture = CultureInfo.CurrentCulture;
            Thread.CurrentThread.CurrentUICulture = CultureInfo.CurrentUICulture;
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.CurrentCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.CurrentUICulture;
            // 💡 如果后续需要统一数字/日期格式，请在具体格式化代码的局部单独使用InvariantCulture，不要全局设置

            // ==========================================
            // 原有启动逻辑（完全保留）
            // ==========================================
            ApplicationConfiguration.Initialize();
            Application.Run(new MainForm());
        }
    }
}