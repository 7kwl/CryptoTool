using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace App1
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }
        // 👇 这就是按钮点击的逻辑！和XAML里的Click="TestButton_Click"一一对应
        private void TestButton_Click(object sender, RoutedEventArgs e)
        {
            // 这句代码会把结果框的文字改成当前时间，点按钮就能看到效果
            ResultText.Text = $"界面跑通啦！当前时间：{DateTime.Now:HH:mm:ss}";
        }
    }
}
