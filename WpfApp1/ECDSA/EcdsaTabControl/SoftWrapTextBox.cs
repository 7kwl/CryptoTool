using System.Globalization;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace WpfApp1.ECDSA.EcdsaTabControl;

/// <summary>
/// 给 <see cref="TextBox"/> 提供"按字符数固定宽度"的软换行能力。
/// <para>
/// 只约束 UI 宽度（通过 <see cref="FrameworkElement.MaxWidthProperty"/>），让
/// <c>TextWrapping="Wrap"</c> 接管换行；<c>.Text</c> 内部不含任何换行符，
/// 复制 / 粘贴 / 算法层拿到的都是"一体长度"的原始字符串。
/// </para>
/// <example>
/// <code>&lt;TextBox TextWrapping="Wrap"
///        local:SoftWrapTextBox.MaxChars="64" /&gt;</code>
/// </example>
/// </summary>
public static class SoftWrapTextBox
{
    /// <summary>
    /// 软换行的字符数。&lt;=0 表示不限制。
    /// </summary>
    public static readonly DependencyProperty MaxCharsProperty =
        DependencyProperty.RegisterAttached(
            "MaxChars",
            typeof(int),
            typeof(SoftWrapTextBox),
            new FrameworkPropertyMetadata(
                0,
                FrameworkPropertyMetadataOptions.AffectsMeasure,
                OnMaxCharsChanged));

    public static void SetMaxChars(DependencyObject obj, int value) => obj.SetValue(MaxCharsProperty, value);
    public static int GetMaxChars(DependencyObject obj) => (int)obj.GetValue(MaxCharsProperty);

    private static void OnMaxCharsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (d is not TextBox tb) return;
        if (e.NewValue is not int n || n <= 0)
        {
            tb.ClearValue(FrameworkElement.MaxWidthProperty);
            tb.Loaded -= ApplyOnLoaded;
            return;
        }

        tb.Loaded -= ApplyOnLoaded;
        tb.Loaded += ApplyOnLoaded;

        if (tb.IsLoaded)
        {
            ApplyMaxWidth(tb);
        }
    }

    private static void ApplyOnLoaded(object sender, RoutedEventArgs e)
    {
        if (sender is TextBox tb)
        {
            ApplyMaxWidth(tb);
        }
    }

    private static void ApplyMaxWidth(TextBox tb)
    {
        var n = GetMaxChars(tb);
        if (n <= 0) return;

        // 等宽字体下 M 是最宽字符，按 n 个 M 估算宽度上界，规避等宽/不等宽差异。
        var typeface = new Typeface(tb.FontFamily, tb.FontStyle, tb.FontWeight, tb.FontStretch);
        double pixelsPerDip = 1.0;
        try
        {
            var dpi = VisualTreeHelper.GetDpi(tb);
            pixelsPerDip = dpi.PixelsPerDip > 0 ? dpi.PixelsPerDip : 1.0;
        }
        catch
        {
            // GetDpi 在元素未接入视觉树时可能抛出，回退 1.0。
        }

        var sample = new FormattedText(
            new string('M', n),
            CultureInfo.CurrentCulture,
            FlowDirection.LeftToRight,
            typeface,
            tb.FontSize,
            Brushes.Black,
            pixelsPerDip);

        var padH = tb.Padding.Left + tb.Padding.Right;
        // +2 富余：TextBox 边框（BORDER 默认 1 像素 ×2） + TextWrapping 边距。
        tb.MaxWidth = Math.Ceiling(sample.Width) + padH + 2;
    }
}
