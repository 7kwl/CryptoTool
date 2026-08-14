using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using CryptoTool.Algorithm.Algorithms.ECDSA;
using Org.BouncyCastle.Crypto.Parameters;

namespace WpfApp1.ECDSA.EcdsaTopPanel
{
    /// <summary>
    /// ECDSA 主页 - 密钥存储标准独立分部文件（由 EcdsaTopPanel.xaml.cs 拆出）
    /// 负责：存储标准常量、按标准导出、SEC1 短/长编码、namedCurve↔specifiedCurve 转换、下拉联动
    /// </summary>
    public partial class EcdsaTopPanel
    {
        #region 密钥存储标准常量（与 WPF 下拉选项一一对应，与 WinForms 源版 EcdsaTabControl.CurveAndStandard.cs 一致）

        /// <summary>私钥存储标准：SEC1 / RFC 5915（短编码 / namedCurve）</summary>
        private const string PrivateKeyStandardNamedCurve = "SEC1 / RFC 5915（短编码 / namedCurve）";

        /// <summary>私钥存储标准：SEC1/RFC 5915（长编码 / specifiedCurve）</summary>
        private const string PrivateKeyStandardSec1 = "SEC1/RFC 5915（长编码 / specifiedCurve）";

        /// <summary>公钥存储标准：RFC 5480/namedCurve（曲线用 OID 引用，体积小）</summary>
        private const string PublicKeyStandardNamedCurve = "RFC 5480/namedCurve";

        /// <summary>公钥存储标准：RFC 5480/specifiedCurve（曲线用显式参数）</summary>
        private const string PublicKeyStandardSpecifiedCurve = "RFC 5480/specifiedCurve";

        #endregion

        #region 密钥导出（按存储标准）

        /// <summary>
        /// 按当前私钥存储标准下拉选项导出私钥 PEM（SEC1 短编码/namedCurve 或 长编码/specifiedCurve）
        /// </summary>
        private string ExportPrivateKeyByStandard(ECPrivateKeyParameters priv)
        {
            string standard = GetComboSelectedText(comboPrivateKeyStandard);
            if (standard == PrivateKeyStandardNamedCurve)
                return EcdsaKeyHelper.ExportPrivateKeyPemNamedCurve(priv);

            // SEC1/RFC 5915: 强制使用显式参数 (specifiedCurve)，避免 namedCurve 私钥被输出为短编码
            var explicitParams = new ECDomainParameters(
                priv.Parameters.Curve, priv.Parameters.G, priv.Parameters.N,
                priv.Parameters.H, priv.Parameters.GetSeed());
            var explicitPriv = new ECPrivateKeyParameters(priv.D, explicitParams);
            return EcdsaKeyHelper.ExportPrivateKeyPem(explicitPriv);
        }

        /// <summary>
        /// 按当前公钥存储标准下拉选项导出公钥 PEM（namedCurve 或 specifiedCurve）
        /// </summary>
        private string ExportPublicKeyByStandard(ECPublicKeyParameters pub)
        {
            string standard = GetComboSelectedText(comboPublicKeyStandard);
            if (standard == PublicKeyStandardNamedCurve)
                return EcdsaKeyHelper.ExportPublicKeyPemNamedCurve(pub);

            // specifiedCurve: 强制使用显式参数，避免 namedCurve 导入后又被输出为 OID 格式
            var explicitParams = new ECDomainParameters(
                pub.Parameters.Curve, pub.Parameters.G, pub.Parameters.N,
                pub.Parameters.H, pub.Parameters.GetSeed());
            var explicitPub = new ECPublicKeyParameters(pub.Q, explicitParams);
            return EcdsaKeyHelper.ExportPublicKeyPem(explicitPub);
        }

        #endregion

        #region 密钥存储标准转换

        /// <summary>
        /// 按当前私钥存储标准下拉选项将私钥重新导出。
        /// 优先从文本框实时解析 PEM，缓存为空时自动从 UI 补充。
        /// </summary>
        private bool TryConvertPrivateKeyStandard()
        {
            string sourceText = textPrivateKey.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(sourceText) && string.IsNullOrWhiteSpace(_privateKeyPem))
                return false;

            // 优先使用缓存，缓存为空或异常时回退到文本框
            string pem = sourceText;
            if (!string.IsNullOrWhiteSpace(_privateKeyPem))
                pem = _privateKeyPem;

            pem = ConvertDisplayToPem(pem, true);
            ECPrivateKeyParameters priv = EcdsaKeyHelper.ImportPrivateKeyPem(pem);
            _privateKeyPem = ExportPrivateKeyByStandard(priv);
            textPrivateKey.Text = FormatKeyForDisplay(_privateKeyPem, GetCurrentOutputFormat());

            string standard = GetComboSelectedText(comboPrivateKeyStandard);
            AppendValidationResult($"私钥已转换为 {standard}", Brushes.Green);
            SetStatus($"私钥存储标准转换完成 - {standard}");
            return true;
        }

        /// <summary>
        /// 按当前公钥存储标准下拉选项将公钥重新导出。
        /// 优先从文本框实时解析 PEM，缓存为空时自动从 UI 补充。
        /// </summary>
        private bool TryConvertPublicKeyStandard()
        {
            string sourceText = textPublicKey.Text?.Trim() ?? string.Empty;
            if (string.IsNullOrWhiteSpace(sourceText) && string.IsNullOrWhiteSpace(_publicKeyPem))
                return false;

            string pem = sourceText;
            if (!string.IsNullOrWhiteSpace(_publicKeyPem))
                pem = _publicKeyPem;

            pem = ConvertDisplayToPem(pem, false);
            ECPublicKeyParameters pub = EcdsaKeyHelper.ImportPublicKeyPem(pem);
            _publicKeyPem = ExportPublicKeyByStandard(pub);
            textPublicKey.Text = FormatKeyForDisplay(_publicKeyPem, GetCurrentOutputFormat());

            string standard = GetComboSelectedText(comboPublicKeyStandard);
            AppendValidationResult($"公钥已转换为 {standard}", Brushes.Green);
            SetStatus($"公钥存储标准转换完成 - {standard}");
            return true;
        }

        private void BtnConvertPrivateKeyStandard_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (!TryConvertPrivateKeyStandard())
                {
                    MessageBox.Show("当前没有私钥内容可转换，请先生成或导入私钥。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换私钥存储标准失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                SetStatus("私钥存储标准转换失败");
            }
        }

        private void BtnConvertPublicKeyStandard_Click(object? sender, RoutedEventArgs e)
        {
            try
            {
                if (!TryConvertPublicKeyStandard())
                {
                    MessageBox.Show("当前没有公钥内容可转换，请先生成或导入公钥。", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"转换公钥存储标准失败：{ex.Message}", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                SetStatus("公钥存储标准转换失败");
            }
        }

        private void ComboPrivateKeyStandard_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
        {
            try
            {
                // 下拉选择后立即转换当前已存在的私钥；无内容则静默跳过
                if (!string.IsNullOrWhiteSpace(textPrivateKey.Text) || !string.IsNullOrWhiteSpace(_privateKeyPem))
                    TryConvertPrivateKeyStandard();
            }
            catch (Exception ex)
            {
                AppendValidationResult($"私钥标准切换失败：{ex.Message}", Brushes.Red);
                SetStatus("私钥存储标准切换失败");
            }
        }

        private void ComboPublicKeyStandard_SelectedIndexChanged(object? sender, SelectionChangedEventArgs e)
        {
            try
            {
                // 下拉选择后立即转换当前已存在的公钥；无内容则静默跳过
                if (!string.IsNullOrWhiteSpace(textPublicKey.Text) || !string.IsNullOrWhiteSpace(_publicKeyPem))
                    TryConvertPublicKeyStandard();
            }
            catch (Exception ex)
            {
                AppendValidationResult($"公钥标准切换失败：{ex.Message}", Brushes.Red);
                SetStatus("公钥存储标准切换失败");
            }
        }

        #endregion
    }
}
