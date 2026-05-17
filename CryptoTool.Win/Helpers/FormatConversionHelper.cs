using CryptoTool.Algorithm.Utils;
using CryptoTool.Win.Enums;
using System.Text;

namespace CryptoTool.Win.Helpers
{

    /// <summary>
    /// 格式转换助手类 - 专门用于UI层的数据格式转换
    /// 将格式转换逻辑从Common类库移动到UI层，提高代码职责分离
    /// </summary>
    public static class FormatConversionHelper
    {
        #region 输入格式转换方法

        /// <summary>
        /// 将字符串转换为字节数组
        /// </summary>
        /// <param name="str">输入字符串</param>
        /// <param name="format">输入格式</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>字节数组</returns>
        public static byte[] StringToBytes(string str, UIInputFormat format, Encoding? encoding = null)
        {
            if (string.IsNullOrEmpty(str))
                throw new ArgumentException("输入字符串不能为空", nameof(str));

            encoding = encoding ?? Encoding.UTF8;

            return format switch
            {
                UIInputFormat.UTF8 => encoding.GetBytes(str),
                UIInputFormat.Base64 => Convert.FromBase64String(str),
                UIInputFormat.Hex => StringUtil.HexToBytes(str),
                _ => throw new ArgumentException($"不支持的输入格式: {format}")
            };
        }

        /// <summary>
        /// 将字节数组转换为字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="format">输出格式</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>字符串</returns>
        public static string BytesToString(byte[] bytes, UIOutputFormat format, Encoding? encoding = null)
        {
            if (bytes == null || bytes.Length == 0)
                throw new ArgumentException("字节数组不能为空", nameof(bytes));

            encoding = encoding ?? Encoding.UTF8;

            return format switch
            {
                UIOutputFormat.UTF8 => encoding.GetString(bytes),
                UIOutputFormat.Base64 => Convert.ToBase64String(bytes),
                UIOutputFormat.Hex => StringUtil.BytesToHex(bytes),
                UIOutputFormat.PEM => Convert.ToBase64String(bytes), // PEM使用Base64编码
                _ => throw new ArgumentException($"不支持的输出格式: {format}")
            };
        }

        #endregion

        #region 格式解析方法

        /// <summary>
        /// 解析UI输入格式字符串
        /// </summary>
        /// <param name="formatText">格式文本</param>
        /// <returns>输入格式枚举</returns>
        public static UIInputFormat ParseInputFormat(string formatText)
        {
            return formatText?.ToUpperInvariant() switch
            {
                "BASE64" => UIInputFormat.Base64,
                "HEX" => UIInputFormat.Hex,
                "UTF8" => UIInputFormat.UTF8,
                "TEXT" => UIInputFormat.UTF8, // 兼容旧版本
                _ => UIInputFormat.UTF8 // 默认值
            };
        }

        /// <summary>
        /// 解析UI输出格式字符串
        /// </summary>
        /// <param name="formatText">格式文本</param>
        /// <returns>输出格式枚举</returns>
        public static UIOutputFormat ParseOutputFormat(string formatText)
        {
            return formatText?.ToUpperInvariant() switch
            {
                "BASE64" => UIOutputFormat.Base64,
                "HEX" => UIOutputFormat.Hex,
                "UTF8" => UIOutputFormat.UTF8,
                "TEXT" => UIOutputFormat.UTF8, // 兼容旧版本
                "PEM" => UIOutputFormat.PEM,
                _ => UIOutputFormat.Base64 // 默认值
            };
        }

        #endregion


        #region 格式转换方法

        /// <summary>
        /// 转换字符串格式
        /// </summary>
        /// <param name="input">输入字符串</param>
        /// <param name="fromFormat">源格式</param>
        /// <param name="toFormat">目标格式</param>
        /// <param name="encoding">字符编码</param>
        /// <returns>转换后的字符串</returns>
        public static string ConvertStringFormat(string input, UIInputFormat fromFormat, UIOutputFormat toFormat, Encoding? encoding = null)
        {
            if (string.IsNullOrEmpty(input))
                return string.Empty;

            // 先转换为字节数组
            byte[] bytes = StringToBytes(input, fromFormat, encoding);

            // 再转换为目标格式
            return BytesToString(bytes, toFormat, encoding);
        }

        #endregion
    }
}
