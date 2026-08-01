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
            switch (tag)
            {
                case "Ecdsa":
                    MainContent.Content = new EcdsaPage();
                    break;
                case "Rsa":
                    MainContent.Content = new Views.RsaView();
                    break;
                case "RsaFmt":
                    MainContent.Content = new Views.RsaFormatView();
                    break;
                case "Aes":
                    MainContent.Content = new Views.AesView();
                    break;
                case "Des":
                    MainContent.Content = new Views.DesView();
                    break;
                case "Sm4":
                    MainContent.Content = new Views.Sm4View();
                    break;
                case "Sm2":
                    MainContent.Content = new Views.Sm2View();
                    break;
                case "Sm3":
                    MainContent.Content = new Views.Sm3View();
                    break;
                case "Md5":
                    MainContent.Content = new Views.Md5View();
                    break;
                case "Yb":
                    MainContent.Content = new Views.YibaoView();
                    break;
                case "About":
                    MainContent.Content = new Views.AboutView();
                    break;
                default:
                    MainContent.Content = new TextBlock
                    {
                        Text = $"功能开发中: {tag}",
                        FontSize = 24,
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    };
                    break;
            }
        }
    }
}