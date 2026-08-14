using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.ECDSA.EcdsaTabControl
{
    public partial class EcdsaTabPage : UserControl
    {
        public EcdsaTabPage()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 子页面（ECDH/ECIES/文件签名）操作结果的桥接写入器 → 顶部"运行结果"框。
        /// 由 EcdsaMainControl 在 EcdsaTopPanel 加载完毕后赋值。
        /// </summary>
        public Action<string, SolidColorBrush>? ResultAppender { get; set; }

        /// <summary>
        /// 子页面（ECDH/ECIES/文件签名）曲线/密钥检测结果的桥接写入器 → 顶部"计算结果"框。
        /// 由 EcdsaMainControl 在 EcdsaTopPanel 加载完毕后赋值。
        /// </summary>
        public Action<string, SolidColorBrush>? KeyResultAppender { get; set; }

        /// <summary>
        /// 子页面实例缓存：切换菜单/重新附着时复用，避免清空各子页面已有记录
        /// </summary>
        private readonly Dictionary<string, UserControl> _subPages = [];

        public void ShowSubPage(string tag)
        {
            if (!_subPages.TryGetValue(tag, out var sub))
            {
                sub = tag switch
                {
                    "KeyGen" => new Ecdsa01 { AppendToHost = ResultAppender, AppendKeyToHost = KeyResultAppender },
                    "Ecdh" => new Ecdsa02(),
                    "Ecies" => new Ecdsa03(),
                    "FileSign" => new Ecdsa04(),
                    _ => null
                };
                if (sub != null)
                    _subPages[tag] = sub;
            }

            SubContent.Content = sub;
        }
    }
}