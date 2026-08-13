using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

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
            // 把顶部两个结果框的写入能力桥接到子页面容器：
            //  - ResultAppender   → 右侧"运行结果"框（操作日志/校验结果）
            //  - KeyResultAppender → 左侧"计算结果"框（私钥提取/曲线检测）
            TabHost.ResultAppender = EcdsaMainPage.AppendValidationResult;
            // AppendKeyResult 带可选参数 curveName，方法组不能直接转 Action<string,SolidColorBrush>，用 Lambda 包装
            TabHost.KeyResultAppender = (msg, brush) => EcdsaMainPage.AppendKeyResult(msg, brush);

            // 当前默认在 KeyGen，已实例化过 Ecdsa01，需要重建以拿到刚设置的桥接
            TabHost.ShowSubPage("KeyGen");
        }
    }
}
