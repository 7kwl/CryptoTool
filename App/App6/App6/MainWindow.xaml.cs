using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace App6
{
    public sealed partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainNav.SelectedItem = MainNav.MenuItems[0];
        }

        private void MainNav_SelectionChanged(NavigationView sender, NavigationViewSelectionChangedEventArgs args)
        {
            if (args.SelectedItem is NavigationViewItem item && item.Tag != null)
            {
                string tag = item.Tag.ToString()!;
                switch (tag)
                {
                    case "EcdsaPage": ContentFrame.Navigate(typeof(Pages.EcdsaPage)); break;
                    case "RsaPage": ContentFrame.Navigate(typeof(Pages.RsaPage)); break;
                    case "RsaFormatPage": ContentFrame.Navigate(typeof(Pages.RsaFormatPage)); break;
                    case "AesPage": ContentFrame.Navigate(typeof(Pages.AesPage)); break;
                    case "DesPage": ContentFrame.Navigate(typeof(Pages.DesPage)); break;
                    case "Sm4Page": ContentFrame.Navigate(typeof(Pages.Sm4Page)); break;
                    case "Sm2Page": ContentFrame.Navigate(typeof(Pages.Sm2Page)); break;
                    case "Sm3Page": ContentFrame.Navigate(typeof(Pages.Sm3Page)); break;
                    case "Md5Page": ContentFrame.Navigate(typeof(Pages.Md5Page)); break;
                    case "MedicalPage": ContentFrame.Navigate(typeof(Pages.MedicalPage)); break;
                    case "AboutPage": ContentFrame.Navigate(typeof(Pages.AboutPage)); break;
                }
            }
        }
    }
}
