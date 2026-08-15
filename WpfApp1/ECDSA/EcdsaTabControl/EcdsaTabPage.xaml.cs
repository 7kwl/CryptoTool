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
        /// 顶部 ECDSA 面板私钥提供者（如未设置，子页面会回退到顶部"私钥"文本框）。
        /// 由 EcdsaMainControl 在 EcdsaTopPanel 加载完毕后赋值。
        /// </summary>
        public Func<string>? PrivateKeyProvider { get; set; }

        /// <summary>
        /// 顶部 ECDSA 面板公钥提供者（如未设置，子页面会回退到顶部"公钥"文本框）。
        /// 由 EcdsaMainControl 在 EcdsaTopPanel 加载完毕后赋值。
        /// </summary>
        public Func<string>? PublicKeyProvider { get; set; }

        /// <summary>
        /// 子页面实例缓存：切换菜单/重新附着时复用，避免清空各子页面已有记录
        /// </summary>
        private readonly Dictionary<string, UserControl> _subPages = [];

        public void ShowSubPage(string tag)
        {
            // 每次都重建 Ecies 子页面，保证宿主回调（AppendToHost）和顶部密钥委托永远最新
            UserControl? sub = tag switch
            {
                "KeyGen" => _subPages.TryGetValue(tag, out var kg) ? kg : new Ecdsa01 { AppendToHost = ResultAppender, AppendKeyToHost = KeyResultAppender },
                "Ecdh" => _subPages.TryGetValue(tag, out var e2) ? e2 : new Ecdsa02 { AppendToHost = ResultAppender, PrivateKeyProvider = PrivateKeyProvider, PublicKeyProvider = PublicKeyProvider },
                "Ecies" => new Ecdsa03 { AppendToHost = ResultAppender, PrivateKeyProvider = PrivateKeyProvider, PublicKeyProvider = PublicKeyProvider },
                "FileSign" => _subPages.TryGetValue(tag, out var e4) ? e4 : new Ecdsa04 { AppendToHost = ResultAppender, PrivateKeyProvider = PrivateKeyProvider, PublicKeyProvider = PublicKeyProvider },
                _ => null
            };
            if (sub != null)
                _subPages[tag] = sub;

            SubContent.Content = sub;
        }
    }
}