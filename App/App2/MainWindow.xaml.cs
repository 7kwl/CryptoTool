using App2.Services;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Runtime.InteropServices;
using System.Text;

namespace App2
{
    /// <summary>
    /// ECDSA 签名验签工具 - MainWindow
    /// </summary>
    public sealed partial class MainWindow : Window
    {
        // ========== 运行时状态 ==========
        private ECPrivateKeyParameters? _currentPrivateKey;
        private ECPublicKeyParameters? _currentPublicKey;
        private byte[]? _currentSignature;

        [DllImport("user32.dll")]
        private static extern uint GetDpiForWindow(IntPtr hwnd);

        public MainWindow()
        {
            InitializeComponent();

            // WinUI 3 的 Window 没有 Height/Width 属性，需通过 AppWindow 设置
            var hwnd = Win32Interop.GetWindowFromWindowId(AppWindow.Id);
            var scale = GetDpiForWindow(hwnd) / 96.0;
            AppWindow.Resize(new Windows.Graphics.SizeInt32
            {
                Width = (int)(800 * scale),
                Height = (int)(700 * scale)
            });
        }

        // ========== 参数变更：切换曲线/哈希时清除已有密钥 ==========
        private void OnParameterChanged(object sender, SelectionChangedEventArgs e)
        {
            _currentPrivateKey = null;
            _currentPublicKey = null;
            _currentSignature = null;
            PrivateKeyBox.Text = "";
            PublicKeyBox.Text = "";
            SignatureBox.Text = "";
            SignBtn.IsEnabled = false;
            VerifyBtn.IsEnabled = false;
            SetStatus("曲线或哈希已更改，请重新生成密钥对", InfoBarSeverity.Informational);
        }

        // ========== 生成密钥对 ==========
        private void OnGenerateKeyPair(object sender, RoutedEventArgs e)
        {
            try
            {
                var curve = ((ComboBoxItem)CurveCombo.SelectedItem).Content.ToString();
                SetStatus($"正在生成 {curve} 密钥对...", InfoBarSeverity.Informational);

                var keyPair = EcdsaAlgorithm.GenerateKeyPair(curve);
                _currentPrivateKey = (ECPrivateKeyParameters)keyPair.Private;
                _currentPublicKey = (ECPublicKeyParameters)keyPair.Public;

                // 导出 PEM 并显示
                PrivateKeyBox.Text = EcdsaKeyHelper.ExportPrivateKeyPem(_currentPrivateKey);
                PublicKeyBox.Text = EcdsaKeyHelper.ExportPublicKeyPem(_currentPublicKey);

                // 清空之前的签名
                _currentSignature = null;
                SignatureBox.Text = "";

                SignBtn.IsEnabled = true;
                VerifyBtn.IsEnabled = false;
                SetResult("密钥对已生成", true);
                SetStatus($"{curve} 密钥对生成成功", InfoBarSeverity.Success);
            }
            catch (Exception ex)
            {
                SetResult($"生成失败：{ex.Message}", false);
                SetStatus("密钥生成出错", InfoBarSeverity.Error);
            }
        }

        // ========== 签名 ==========
        private void OnSign(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentPrivateKey == null)
                {
                    SetResult("请先生成密钥对", false);
                    return;
                }
                if (string.IsNullOrWhiteSpace(PlainDataBox.Text))
                {
                    SetResult("请输入待签名数据", false);
                    return;
                }

                var hash = ((ComboBoxItem)HashCombo.SelectedItem).Content.ToString();
                var payload = Encoding.UTF8.GetBytes(PlainDataBox.Text);

                _currentSignature = EcdsaAlgorithm.Sign(payload, _currentPrivateKey, hash);

                SignatureBox.Text = Convert.ToBase64String(_currentSignature);
                VerifyBtn.IsEnabled = true;
                SetResult("签名完成", true);
                SetStatus($"已用 {hash} 签名，长度 {_currentSignature.Length} 字节", InfoBarSeverity.Success);
            }
            catch (Exception ex)
            {
                SetResult($"签名失败：{ex.Message}", false);
                SetStatus("签名出错", InfoBarSeverity.Error);
            }
        }

        // ========== 验签 ==========
        private void OnVerify(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_currentPublicKey == null || _currentSignature == null)
                {
                    SetResult("请先生成密钥并签名", false);
                    return;
                }
                if (string.IsNullOrWhiteSpace(PlainDataBox.Text))
                {
                    SetResult("请输入待验签数据", false);
                    return;
                }

                var hash = ((ComboBoxItem)HashCombo.SelectedItem).Content.ToString();
                var payload = Encoding.UTF8.GetBytes(PlainDataBox.Text);

                var isValid = EcdsaAlgorithm.Verify(payload, _currentSignature, _currentPublicKey, hash);

                if (isValid)
                {
                    SetResult("✓ 验签通过：签名有效", true);
                    SetStatus("验签成功", InfoBarSeverity.Success);
                }
                else
                {
                    SetResult("✗ 验签失败：签名无效或数据被篡改", false);
                    SetStatus("验签失败", InfoBarSeverity.Warning);
                }
            }
            catch (Exception ex)
            {
                SetResult($"验签异常：{ex.Message}", false);
                SetStatus("验签出错", InfoBarSeverity.Error);
            }
        }

        // ========== 辅助方法 ==========
        private void SetResult(string text, bool success)
        {
            ResultBlock.Text = text;
            ResultBlock.Foreground = new SolidColorBrush(success
                ? Color.FromArgb(255, 16, 185, 129)   // 绿色
                : Color.FromArgb(255, 239, 68, 68));   // 红色
        }

        /// <summary>
        /// 设置状态栏文本（用不同颜色区分严重程度）
        /// </summary>
        private void SetStatus(string text, InfoBarSeverity severity)
        {
            StatusBlock.Text = text;
            StatusBlock.Foreground = severity switch
            {
                InfoBarSeverity.Success => new SolidColorBrush(Color.FromArgb(255, 16, 185, 129)),
                InfoBarSeverity.Warning => new SolidColorBrush(Color.FromArgb(255, 234, 179, 8)),
                InfoBarSeverity.Error => new SolidColorBrush(Color.FromArgb(255, 239, 68, 68)),
                _ => (SolidColorBrush)App.Current.Resources["TextFillColorSecondaryBrush"]
            };
        }
    }
}
