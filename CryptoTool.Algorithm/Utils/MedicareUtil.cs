using CryptoTool.Algorithm.Algorithms.SM2;
using CryptoTool.Algorithm.Algorithms.SM4;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CryptoTool.Algorithm.Utils
{
    /// <summary>
    /// 医保工具：按规范实现请求签名/验签与报文加解密（SM2/SM4）
    /// </summary>
    public class MedicareUtil
    {
        // 排除参与签名的参数名（不区分大小写）
        private static readonly string[] ExcludeKeys = new[] { "signData", "encData", "extra" };

        #region 对外主功能

        /// <summary>
        ///  构造待签名原文（筛选->排序->拼接后追加 &key=appSecret）
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string BuildSignatureBaseString(IDictionary<string, object> parameters, string appSecret)
        {
            if (parameters == null) throw new ArgumentNullException(nameof(parameters));
            if (string.IsNullOrEmpty(appSecret)) throw new ArgumentNullException(nameof(appSecret));

            // 1) 筛选：剔除 signData/encData/extra，忽略空值
            var filtered = parameters
                .Where(kv => !IsExcludedKey(kv.Key) && !IsNullOrEmptyValue(kv.Value))
                .ToDictionary(kv => kv.Key, kv => kv.Value);

            // 2) 排序：ASCII 升序
            var orderedKeys = filtered.Keys.OrderBy(k => k, StringComparer.Ordinal).ToArray();

            // 3) 拼接：key=value 用 & 连接；对象型按JSON内部字母序升序，空值不参与
            var parts = new List<string>(orderedKeys.Length);
            foreach (var key in orderedKeys)
            {
                string valueStr = SerializeValue(filtered[key]);
                parts.Add($"{key}={valueStr}");
            }

            string baseString = string.Join("&", parts) + $"&key={appSecret}";
            return baseString;
        }

        /// <summary>
        /// 使用SM2签名请求参数，返回Base64签名串（signData）
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="privateKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string SignParameters(IDictionary<string, object> parameters, byte[] privateKey, string appSecret)
        {
            if (privateKey == null) throw new ArgumentNullException(nameof(privateKey));
            string baseString = BuildSignatureBaseString(parameters, appSecret);

            var sm2Crypto = new Sm2Crypto();
            byte[] sigBytes = sm2Crypto.Sign(Encoding.UTF8.GetBytes(baseString), privateKey);
            return Convert.ToBase64String(sigBytes);
        }

        /// <summary>
        /// 验证返回参数SM2签名（signData为Base64）
        /// </summary>
        /// <param name="parameters"></param>
        /// <param name="signDataBase64"></param>
        /// <param name="publicKey"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static bool VerifyParametersSignature(IDictionary<string, object> parameters, string signDataBase64, byte[] publicKey, string appSecret)
        {
            if (publicKey == null) throw new ArgumentNullException(nameof(publicKey));
            if (string.IsNullOrEmpty(signDataBase64)) throw new ArgumentNullException(nameof(signDataBase64));

            string baseString = BuildSignatureBaseString(parameters, appSecret);
            byte[] sigBytes = Convert.FromBase64String(signDataBase64.Trim());
            var sm2Crypto = new Sm2Crypto();
            return sm2Crypto.VerifySign(Encoding.UTF8.GetBytes(baseString), sigBytes, publicKey);
        }

        /// <summary>
        /// 根据规范派生SM4密钥，并对data(JSON)进行加密，输出Hex大写的encData
        /// </summary>
        /// <param name="dataObject"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns>返回大写Hex格式</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string EncryptData(object dataObject, string appId, string appSecret)
        {
            if (dataObject == null) throw new ArgumentNullException(nameof(dataObject));
            if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));
            if (string.IsNullOrEmpty(appSecret)) throw new ArgumentNullException(nameof(appSecret));

            // 1) 生成data的规范化JSON字符串
            string jStr = SerializeObjectCanonicalJson(dataObject);

            // 2) 派生SM4密钥
            byte[] derived16 = GetSm4Key16Bytes(appId, appSecret);

            // 3) 使用派生密钥加密jStr，得到Base64，再转Hex大写
            var sm4Crypto = new Sm4Crypto(Enums.SymmetricCipherMode.ECB);
            byte[] cipherBytes = sm4Crypto.Encrypt(Encoding.UTF8.GetBytes(jStr), derived16);
            string encHex = StringUtil.BytesToHex(cipherBytes, true);
            return encHex;
        }

        /// <summary>
        /// 解密响应中的encData(Hex)，返回明文JSON字符串jStr。
        /// </summary>
        /// <param name="encDataHex"></param>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns>返回解密后的明文</returns>
        /// <exception cref="ArgumentNullException"></exception>
        public static string DecryptEncData(string encDataHex, string appId, string appSecret)
        {
            if (string.IsNullOrEmpty(encDataHex)) throw new ArgumentNullException(nameof(encDataHex));
            if (string.IsNullOrEmpty(appId)) throw new ArgumentNullException(nameof(appId));
            if (string.IsNullOrEmpty(appSecret)) throw new ArgumentNullException(nameof(appSecret));

            byte[] derived16Bytes = GetSm4Key16Bytes(appId, appSecret);
            byte[] cipherBytes = StringUtil.HexToBytes(encDataHex);
            var sm4Crypto = new Sm4Crypto(Enums.SymmetricCipherMode.ECB);
            byte[] decryptBytes = sm4Crypto.Decrypt(cipherBytes, derived16Bytes);
            return Encoding.UTF8.GetString(decryptBytes);
        }

        #endregion

        #region 辅助 - 规范化JSON与值序列化

        private static bool IsExcludedKey(string key)
        {
            return ExcludeKeys.Any(k => string.Equals(k, key, StringComparison.OrdinalIgnoreCase));
        }

        private static bool IsNullOrEmptyValue(object value)
        {
            if (value == null) return true;
            if (value is string s) return string.IsNullOrEmpty(s);
            if (value is JValue jv) return jv.Type == JTokenType.Null || jv.Type == JTokenType.String && string.IsNullOrEmpty(jv.Value?.ToString());
            return false;
        }

        private static string SerializeValue(object value)
        {
            if (value == null) return string.Empty;

            // 对象或数组 -> 规范化JSON
            if (value is JToken jt)
            {
                return SerializeTokenCanonical(jt);
            }

            // 尝试把任意对象转换为JToken并规范化
            if (!(value is string) && !IsSimple(value))
            {
                var token = JToken.FromObject(value);
                return SerializeTokenCanonical(token);
            }

            // 字符串/简单类型直接ToString
            return value.ToString();
        }

        private static bool IsSimple(object value)
        {
            var t = value.GetType();
            return t.IsPrimitive || t.IsEnum || t == typeof(string) || t == typeof(decimal) || t == typeof(DateTime) || t == typeof(Guid);
        }

        private static string SerializeObjectCanonicalJson(object obj)
        {
            if (obj is JToken token)
            {
                return SerializeTokenCanonical(token);
            }
            var t = JToken.FromObject(obj);
            return SerializeTokenCanonical(t);
        }

        private static string SerializeTokenCanonical(JToken token)
        {
            var canonical = ToCanonical(token);
            return JsonConvert.SerializeObject(canonical, Formatting.None, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                StringEscapeHandling = StringEscapeHandling.Default
            });
        }

        private static JToken ToCanonical(JToken token)
        {
            switch (token.Type)
            {
                case JTokenType.Object:
                    var obj = (JObject)token;
                    var props = obj.Properties()
                        .Where(p => p.Value.Type != JTokenType.Null)
                        .OrderBy(p => p.Name, StringComparer.Ordinal)
                        .Select(p => new JProperty(p.Name, ToCanonical(p.Value)));
                    return new JObject(props);
                case JTokenType.Array:
                    var arr = (JArray)token;
                    return new JArray(arr.Select(ToCanonical));
                default:
                    return token;
            }
        }

        #endregion

        /// <summary>
        /// 获取新的SM4密钥（16字节），规则：以appId(渠道id)作为Key，对appSecret加密，得到新秘钥串，取前16字节作为SM4密钥
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns>返回16字节密钥的Hex表示（32个十六进制字符）</returns>
        public static string GetSm4Key16(string appId, string appSecret)
        {
            return StringUtil.BytesToHex(GetSm4Key16Bytes(appId, appSecret), true);
        }

        private static byte[] GetSm4Key16Bytes(string appId, string appSecret)
        {
            // 以appId(渠道id)作为Key，对appSecret加密，得到新秘钥串
            if (appId.Length < 16)
            {
                throw new ArgumentException("appId长度不足16字节，无法派生SM4密钥", nameof(appId));
            }
            byte[] appIdBytes16 = Encoding.UTF8.GetBytes(appId.Substring(0, 16));
            byte[] appSecretBytes = Encoding.UTF8.GetBytes(appSecret);
            var sm4Crypto = new Sm4Crypto(Enums.SymmetricCipherMode.ECB);
            byte[] newEncryptedData = sm4Crypto.Encrypt(appSecretBytes, appIdBytes16);
            if (newEncryptedData.Length < 16)
            {
                throw new InvalidOperationException("派生SM4密钥失败，结果长度不足16字节");
            }

            var derived16 = new byte[16];
            Buffer.BlockCopy(newEncryptedData, 0, derived16, 0, 16);
            return derived16;
        }
    }
}
