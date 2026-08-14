using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WpfApp1.ECDSA;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            // 默认选中ECDSA
            BtnEcdsa.IsChecked = true;
            ShowPage("Ecdsa");
        }

        private void NavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton clicked) return;

            // 单选逻辑：取消其他按钮的选中状态
            if (clicked.Parent is StackPanel stackPanel)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is ToggleButton btn && btn != clicked)
                        btn.IsChecked = false;
                }
            }

            clicked.IsChecked = true;
            ShowPage(clicked.Tag?.ToString() ?? "");
        }

        private void ShowPage(string tag)
        {
            MainContent.Content = tag switch
            {
                "Ecdsa"  => new EcdsaMainControl(),
                "Rsa"    => new Views.RsaView(),
                "RsaFmt" => new Views.RsaFormatView(),
                "Aes"    => new Views.AesView(),
                "Des"    => new Views.DesView(),
                "Sm4"    => new Views.Sm4View(),
                "Sm2"    => new Views.Sm2View(),
                "Sm3"    => new Views.Sm3View(),
                "Md5"    => new Views.Md5View(),
                "Yb"     => new Views.YibaoView(),
                "About"  => new Views.AboutView(),
                _        => CreateDevelopView(tag)
            };
        }

        private static TextBlock CreateDevelopView(string tag) => new()
        {
            Text = $"功能开发中: {tag}",
            FontSize = 24,
            HorizontalAlignment = HorizontalAlignment.Center,
            VerticalAlignment = VerticalAlignment.Center
        };
    }
}