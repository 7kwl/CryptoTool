using System.Windows.Controls;

namespace WpfApp1.ECDSA.EcdsaTabControl
{
    public partial class EcdsaTabPage : UserControl
    {
        public EcdsaTabPage()
        {
            InitializeComponent();
        }

        public void ShowSubPage(string tag)
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
