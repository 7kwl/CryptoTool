using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;

namespace WpfApp1.IconLogic
{
    /// <summary>
    /// 图标悬停提示助手。
    /// 原 EcdsaTopPanel / Ecdsa01 / Ecdsa02 / Ecdsa03 四个页面各自维护了一份相同实现，
    /// 现统一收敛到此静态类，任何界面直接调用 IconToolTipHelper.SetIconToolTip 即可复用。
    /// </summary>
    public static class IconToolTipHelper
    {
        /// <summary>
        /// 为图标设置悬停提示：鼠标悬停时在图标右侧显示红色文字，移走自动消失。
        /// 使用 Popup + 延迟关闭实现：
        ///  - 鼠标从图标移到气泡上时不会立即关闭（延迟 250ms 内移入气泡即取消关闭），避免闪烁；
        ///  - Popup 不拦截图标的点击事件，复制/粘贴可正常触发。
        /// </summary>
        /// <param name="icon">任意 FrameworkElement（Image / Button / Border 等）均可</param>
        /// <param name="text">悬停提示文本</param>
        public static void SetIconToolTip(FrameworkElement icon, string text)
        {
            var popup = new Popup
            {
                PlacementTarget = icon,
                Placement = PlacementMode.Right,
                HorizontalOffset = 6,
                AllowsTransparency = true,
                StaysOpen = true,
                Child = new Border
                {
                    BorderBrush = Brushes.Red,
                    BorderThickness = new Thickness(1),
                    Child = new TextBlock
                    {
                        Text = text,
                        Foreground = Brushes.Red,
                        Background = Brushes.White,
                        Padding = new Thickness(6, 2, 6, 2)
                    }
                }
            };

            var closeTimer = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(250) };
            closeTimer.Tick += (s, e) =>
            {
                closeTimer.Stop();
                popup.IsOpen = false;
            };

            icon.MouseEnter += (s, e) => { closeTimer.Stop(); popup.IsOpen = true; };
            icon.MouseLeave += (s, e) => { closeTimer.Stop(); closeTimer.Start(); };
            popup.MouseEnter += (s, e) => { closeTimer.Stop(); };
            popup.MouseLeave += (s, e) => { closeTimer.Stop(); closeTimer.Start(); };
            // 点击图标时立即关闭气泡，避免点击操作后残留
            icon.MouseLeftButtonDown += (s, e) => { closeTimer.Stop(); popup.IsOpen = false; };
        }
    }
}
