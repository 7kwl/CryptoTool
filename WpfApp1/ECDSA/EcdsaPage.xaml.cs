using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;

namespace WpfApp1.ECDSA
{
    public partial class EcdsaPage : UserControl
    {
        public EcdsaPage()
        {
            InitializeComponent();

            // 避免在设计器中初始化子页面导致设计器崩溃
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            BtnKeyGen.IsChecked = true;
            TabHost.ShowSubPage("KeyGen");
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
            TabHost.ShowSubPage(clicked.Tag?.ToString() ?? "");
        }

        private void EcdsaMainPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}
