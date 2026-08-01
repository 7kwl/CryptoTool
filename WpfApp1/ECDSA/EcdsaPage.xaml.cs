using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using WpfApp1.ECDSA.EcdsaTabControl;

namespace WpfApp1.ECDSA
{
    public partial class EcdsaPage : UserControl
    {
        public EcdsaPage()
        {
            InitializeComponent();
            BtnKeyGen.IsChecked = true;
            ShowSubPage("KeyGen");
        }

        private void SubNavButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is not ToggleButton clicked) return;

            if (clicked.Parent is StackPanel stackPanel)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is ToggleButton btn && btn != clicked)
                        btn.IsChecked = false;
                }
            }

            clicked.IsChecked = true;
            ShowSubPage(clicked.Tag?.ToString() ?? "");
        }

        private void ShowSubPage(string tag)
        {
            switch (tag)
            {
                case "KeyGen":
                    SubContent.Content = new Ecdsa01();
                    break;
                case "Ecdh":
                    SubContent.Content = new Ecdsa02();
                    break;
                case "Ecies":
                    SubContent.Content = new Ecdsa03();
                    break;
                case "FileSign":
                    SubContent.Content = new Ecdsa04();
                    break;
            }
        }
    }
}