using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;

namespace WpfApp1.ECDSA
{
    public partial class EcdsaMainControl : UserControl
    {
        public EcdsaMainControl()
        {
            InitializeComponent();

            // 避免在设计器中初始化子页面导致设计器崩溃
            if (DesignerProperties.GetIsInDesignMode(this))
                return;

            // 首次创建子页面延迟到 EcdsaTopPanel_Loaded 中（此时 ResultAppender/KeyResultAppender 桥接已就绪，
            // 且 EcdsaTabPage 已加上 _subPages 缓存，重复进入 ECDSA 菜单会复用同一 Ecdsa01，不会清空记录）
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

        private void EcdsaTopPanel_Loaded(object sender, RoutedEventArgs e)
        {
            // 把顶部两个结果框的写入能力桥接到子页面容器：
            //  - ResultAppender   → 右侧"运行结果"框（操作日志/校验结果）
            //  - KeyResultAppender → 左侧"计算结果"框（私钥提取/曲线检测）
            TabHost.ResultAppender = TopPanel.AppendValidationResult;
            // AppendKeyResult 带可选参数 curveName，方法组不能直接转 Action<string,SolidColorBrush>，用 Lambda 包装
            TabHost.KeyResultAppender = (msg, brush) => TopPanel.AppendKeyResult(msg, brush);

            // 把顶部 ECDSA 面板的当前私钥/公钥 PEM 也桥接到子页面容器。
            // 注：EcdsaTopPanel 与 EcdsaTabPage 是 Grid 的兄弟节点，子页面内部用 FindAncestor 走不到，
            //   所以必须由 EcdsaMainControl 显式注入到 TabHost，再随 ShowSubPage 转发到各子页面。
            TabHost.PrivateKeyProvider = TopPanel.GetCurrentPrivateKeyPem;
            TabHost.PublicKeyProvider = TopPanel.GetCurrentPublicKeyPem;

            // 默认进入 KeyGen 子页面（首次创建；后续切走再回来会复用同一 Ecdsa01）
            if (BtnKeyGen.IsChecked != true)
                BtnKeyGen.IsChecked = true;
            TabHost.ShowSubPage("KeyGen");
        }
    }
}


