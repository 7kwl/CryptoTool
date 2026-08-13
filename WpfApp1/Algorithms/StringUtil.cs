using System.Security.Cryptography;
using System.Text;

namespace CryptoTool.Algorithm.Utils
{
    /// <summary>
    /// 加密工具类
    /// </summary>
    public static class StringUtil
    {
        /// <summary>
        /// 生成随机字符串
        /// </summary>
        /// <param name="length">长度</param>
        /// <param name="charset">字符集</param>
        /// <returns>随机字符串</returns>
        public static string GenerateRandomString(int length, string? charset = null)
        {
            if (length <= 0)
                throw new ArgumentException("长度必须大于0", nameof(length));

            charset ??= "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

            StringBuilder sb = new(length);
            byte[] randomBytes = GenerateRandomBytes(length);

            for (int i = 0; i < length; i++)
            {
                sb.Append(charset[randomBytes[i] % charset.Length]);
            }

            return sb.ToString();
        }

        /// <summary>
        /// 生成随机字节数组
        /// </summary>
        /// <param name="length">长度</param>
        /// <returns>随机字节数组</returns>
        public static byte[] GenerateRandomBytes(int length)
        {
            if (length <= 0)
                throw new ArgumentException("长度必须大于0", nameof(length));
            var bytes = new byte[length];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
            }
            return bytes;
        }

        /// <summary>
        /// 生成随机密钥
        /// </summary>
        /// <param name="keySize">密钥长度（位）</param>
        /// <returns>随机密钥</returns>
        public static byte[] GenerateRandomKey(int keySize)
        {
            return GenerateRandomBytes(keySize / 8);
        }

        /// <summary>
        /// 生成随机IV
        /// </summary>
        /// <param name="ivSize">IV长度（位）</param>
        /// <returns>随机IV</returns>
        public static byte[] GenerateRandomIV(int ivSize)
        {
            return GenerateRandomBytes(ivSize / 8);
        }

        /// <summary>
        /// 字节数组转十六进制字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <param name="upperCase">是否大写</param>
        /// <returns>十六进制字符串</returns>
        public static string BytesToHex(byte[] bytes, bool upperCase = false)
        {
            if (bytes == null || bytes.Length == 0)
                return string.Empty;
            var format = upperCase ? "X2" : "x2";
            var sb = new StringBuilder(bytes.Length * 2);
            foreach (var b in bytes)
            {
                sb.Append(b.ToString(format));
            }
            return sb.ToString();
        }

        /// <summary>
        /// 十六进制字符串转字节数组
        /// </summary>
        /// <param name="hex">十六进制字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] HexToBytes(string hex)
        {
            if (string.IsNullOrEmpty(hex))
                throw new ArgumentException("十六进制字符串不能为空", nameof(hex));

            if (hex.Length % 2 != 0)
                throw new ArgumentException("十六进制字符串长度必须为偶数", nameof(hex));

            var bytes = new byte[hex.Length / 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                bytes[i] = Convert.ToByte(hex.Substring(i * 2, 2), 16);
            }
            return bytes;
        }

        /// <summary>
        /// 字节数组转Base64字符串
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>Base64字符串</returns>
        public static string BytesToBase64(byte[] bytes)
        {
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Base64字符串转字节数组
        /// </summary>
        /// <param name="base64">Base64字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] Base64ToBytes(string base64)
        {
            if (string.IsNullOrEmpty(base64))
                throw new ArgumentException("Base64字符串不能为空", nameof(base64));

            return Convert.FromBase64String(base64);
        }

        /// <summary>
        /// 字符串转字节数组（UTF-8编码）
        /// </summary>
        /// <param name="text">字符串</param>
        /// <returns>字节数组</returns>
        public static byte[] StringToBytes(string text)
        {
            return Encoding.UTF8.GetBytes(text);
        }

        /// <summary>
        /// 字节数组转字符串（UTF-8编码）
        /// </summary>
        /// <param name="bytes">字节数组</param>
        /// <returns>字符串</returns>
        public static string BytesToString(byte[] bytes)
        {
            return Encoding.UTF8.GetString(bytes);
        }

        /// <summary>
        /// 比较两个字节数组是否相等
        /// </summary>
        /// <param name="a">字节数组A</param>
        /// <param name="b">字节数组B</param>
        /// <returns>是否相等</returns>
        public static bool ByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null || b == null)
                return a == b;

            if (a.Length != b.Length)
                return false;

            for (int i = 0; i < a.Length; i++)
            {
                if (a[i] != b[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 安全地比较两个字节数组（防止时序攻击）
        /// </summary>
        /// <param name="a">字节数组A</param>
        /// <param name="b">字节数组B</param>
        /// <returns>是否相等</returns>
        public static bool SecureByteArraysEqual(byte[] a, byte[] b)
        {
            if (a == null || b == null)
                return a == b;

            if (a.Length != b.Length)
                return false;

            int result = 0;
            for (int i = 0; i < a.Length; i++)
            {
                result |= a[i] ^ b[i];
            }

            return result == 0;
        }
    }
}
