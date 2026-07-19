using System;
using System.Collections.Generic;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配：该类被多个 Win 项目文件使用，保留 CryptoTool.Win.Helpers 命名空间
namespace CryptoTool.Win.Helpers
{
    /// <summary>
    /// ECDSA 曲线名称映射与分类（独立版，不依赖 Algorithm 项目）
    /// </summary>
    public static class EcdsaCurveNames
    {
        // ===== 所有曲线（扁平列表，保持向后兼容）=====
        private static readonly IReadOnlyList<string> _allCurves = new[]
        {
            // NIST / SECG prime 曲线
            "prime256v1", "secp256r1", "secp384r1", "secp521r1",
            "secp112r1", "secp112r2", "secp128r1", "secp128r2",
            "secp160k1", "secp160r1", "secp160r2",
            "secp192k1", "secp192r1", "secp224k1", "secp224r1",
            "secp256k1",
            // NIST B/P 曲线
            "P-192", "P-224", "P-256", "P-384", "P-521",
            "B-163", "B-233", "B-283", "B-409", "B-571",
            // Brainpool
            "brainpoolP160r1", "brainpoolP160t1",
            "brainpoolP192r1", "brainpoolP192t1",
            "brainpoolP224r1", "brainpoolP224t1",
            "brainpoolP256r1", "brainpoolP256t1",
            "brainpoolP320r1", "brainpoolP320t1",
            "brainpoolP384r1", "brainpoolP384t1",
            "brainpoolP512r1", "brainpoolP512t1",
            // GOST
            "GostR3410-2001-CryptoPro-A", "GostR3410-2001-CryptoPro-B",
            "GostR3410-2001-CryptoPro-C", "GostR3410-2001-CryptoPro-D",
            // SM2
            "sm2p256v1",
            // WTLS
            "wtls1", "wtls3", "wtls4", "wtls5", "wtls6", "wtls7", "wtls8", "wtls9", "wtls10",
            // X9.62
            "c2pnb163v1", "c2pnb163v2", "c2pnb163v3",
            "c2pnb176v1", "c2pnb208w1", "c2pnb272w1",
            "c2pnb304w1", "c2pnb368w1",
            "c2tnb191v1", "c2tnb191v2", "c2tnb191v3",
            "c2tnb239v1", "c2tnb239v2", "c2tnb239v3",
            "c2tnb431r1",
            // ANSI X9.62 prime
            "prime192v1", "prime192v2", "prime192v3",
            "prime239v1", "prime239v2", "prime239v3", "prime256v1"
        };

        /// <summary>
        /// 曲线Key → 所属分类Key的反向映射（静态构造初始化，O(1)查询）
        /// </summary>
        private static readonly Dictionary<string, string> _curveToCategory;

        /// <summary>
        /// 静态构造函数：构建反向映射 + 校验分类合法性
        /// </summary>
        static EcdsaCurveNames()
        {
            _curveToCategory = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            var allCategories = GetAllCurvesByCategory();

            foreach (var category in allCategories)
            {
                var categoryKey = category.Key;
                foreach (var curve in category.Value.Curves)
                {
                    var curveKey = curve.Key;
                    // 校验：禁止同一个曲线Key出现在多个分类中（分类设计原则）
                    if (_curveToCategory.TryGetValue(curveKey, out var existingCategory))
                    {
                        throw new InvalidOperationException(
                            $"曲线分类配置错误：曲线Key '{curveKey}' 同时存在于分类 '{existingCategory}' 和 '{categoryKey}' 中，" +
                            $"请检查GetAllCurvesByCategory的分类配置。");
                    }
                    _curveToCategory.Add(curveKey, categoryKey);
                }
            }
        }

        /// <summary>
        /// 获取所有曲线名称（扁平列表，保持原有接口）
        /// </summary>
        public static IReadOnlyList<string> GetAllCurves()
        {
            return _allCurves;
        }

        /// <summary>
        /// 获取曲线的友好显示名称
        /// </summary>
        public static string GetDisplayName(string curveName)
        {
            if (string.IsNullOrEmpty(curveName)) return curveName;

            var friendlyNames = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                { "prime256v1", "prime256v1 (NIST P-256)" },
                { "secp256r1", "P-256 (secp256r1) - 128 位安全" },
                { "secp384r1", "P-384 (secp384r1) - 192 位安全" },
                { "secp521r1", "P-521 (secp521r1) - 256 位安全" },
                { "secp112r1", "secp112r1 (SECG 112-bit)" },
                { "secp112r2", "secp112r2 (SECG 112-bit)" },
                { "secp128r1", "secp128r1 (SECG 128-bit)" },
                { "secp128r2", "secp128r2 (SECG 128-bit)" },
                { "secp160k1", "secp160k1 (SECG 160-bit Koblitz)" },
                { "secp160r1", "secp160r1 (SECG 160-bit)" },
                { "secp160r2", "secp160r2 (SECG 160-bit)" },
                { "secp192k1", "secp192k1 (NIST K-192)" },
                { "secp192r1", "secp192r1 (NIST P-192)" },
                { "secp224k1", "secp224k1 (NIST K-224)" },
                { "secp224r1", "secp224r1 (NIST P-224)" },
                { "secp256k1", "secp256k1 (Bitcoin/secp256k1)" },
                { "P-192", "P-192 (NIST)" },
                { "P-224", "P-224 (NIST)" },
                { "P-256", "P-256 (NIST)" },
                { "P-384", "P-384 (NIST)" },
                { "P-521", "P-521 (NIST)" },
                { "B-163", "B-163 (NIST Binary)" },
                { "B-233", "B-233 (NIST Binary)" },
                { "B-283", "B-283 (NIST Binary)" },
                { "B-409", "B-409 (NIST Binary)" },
                { "B-571", "B-571 (NIST Binary)" },
                { "brainpoolP160r1", "brainpoolP160r1" },
                { "brainpoolP160t1", "brainpoolP160t1" },
                { "brainpoolP192r1", "brainpoolP192r1" },
                { "brainpoolP192t1", "brainpoolP192t1" },
                { "brainpoolP224r1", "brainpoolP224r1" },
                { "brainpoolP224t1", "brainpoolP224t1" },
                { "brainpoolP256r1", "brainpoolP256r1" },
                { "brainpoolP256t1", "brainpoolP256t1" },
                { "brainpoolP320r1", "brainpoolP320r1" },
                { "brainpoolP320t1", "brainpoolP320t1" },
                { "brainpoolP384r1", "brainpoolP384r1" },
                { "brainpoolP384t1", "brainpoolP384t1" },
                { "brainpoolP512r1", "brainpoolP512r1" },
                { "brainpoolP512t1", "brainpoolP512t1" },
                { "GostR3410-2001-CryptoPro-A", "GOST R 34.10-2001 (CryptoPro-A)" },
                { "GostR3410-2001-CryptoPro-B", "GOST R 34.10-2001 (CryptoPro-B)" },
                { "GostR3410-2001-CryptoPro-C", "GOST R 34.10-2001 (CryptoPro-C)" },
                { "GostR3410-2001-CryptoPro-D", "GOST R 34.10-2001 (CryptoPro-D)" },
                { "sm2p256v1", "sm2p256v1 (国密 SM2)" },
                { "wtls1", "WTLS-1" },
                { "wtls3", "WTLS-3" },
                { "wtls4", "WTLS-4" },
                { "wtls5", "WTLS-5" },
                { "wtls6", "WTLS-6" },
                { "wtls7", "WTLS-7" },
                { "wtls8", "WTLS-8" },
                { "wtls9", "WTLS-9" },
                { "wtls10", "WTLS-10" },
                { "c2pnb163v1", "X9.62 c2pnb163v1" },
                { "c2pnb163v2", "X9.62 c2pnb163v2" },
                { "c2pnb163v3", "X9.62 c2pnb163v3" },
                { "c2pnb176v1", "X9.62 c2pnb176v1" },
                { "c2pnb208w1", "X9.62 c2pnb208w1" },
                { "c2pnb272w1", "X9.62 c2pnb272w1" },
                { "c2pnb304w1", "X9.62 c2pnb304w1" },
                { "c2pnb368w1", "X9.62 c2pnb368w1" },
                { "c2tnb191v1", "X9.62 c2tnb191v1" },
                { "c2tnb191v2", "X9.62 c2tnb191v2" },
                { "c2tnb191v3", "X9.62 c2tnb191v3" },
                { "c2tnb239v1", "X9.62 c2tnb239v1" },
                { "c2tnb239v2", "X9.62 c2tnb239v2" },
                { "c2tnb239v3", "X9.62 c2tnb239v3" },
                { "c2tnb431r1", "X9.62 c2tnb431r1" },
                { "prime192v1", "prime192v1 (NIST P-192)" },
                { "prime192v2", "prime192v2" },
                { "prime192v3", "prime192v3" },
                { "prime239v1", "prime239v1" },
                { "prime239v2", "prime239v2" },
                { "prime239v3", "prime239v3" },
            };

            return friendlyNames.TryGetValue(curveName, out var display) ? display : curveName;
        }

        /// <summary>
        /// ✅ 按分类获取曲线（级联下拉框用）
        /// Key = 分类名, Value = (图标, 曲线列表)
        /// </summary>
        public static Dictionary<string, (string Icon, List<KeyValuePair<string, string>> Curves)> GetAllCurvesByCategory()
        {
            var result = new Dictionary<string, (string, List<KeyValuePair<string, string>>)>
            {
                // 1. NIST 推荐曲线（置顶，与参考网站一致）
                ["NIST Curves (Recommended)"] = ("🏆", new List<KeyValuePair<string, string>>
                {
                    KVP("secp256r1", "P-256 (secp256r1) - 128 位安全"),
                    KVP("secp384r1", "P-384 (secp384r1) - 192 位安全"),
                    KVP("secp521r1", "P-521 (secp521r1) - 256 位安全"),
                }),

                // 2. NIST Prime 曲线
                ["prime"] = ("⭐", new List<KeyValuePair<string, string>>
                {
                    KVP("prime256v1", "prime256v1 (NIST P-256)"),
                    KVP("secp192r1", "secp192r1 (NIST P-192)"),
                    KVP("secp224r1", "secp224r1 (NIST P-224)"),
                    KVP("prime192v1", "prime192v1 (NIST P-192)"),
                    KVP("prime192v2", "prime192v2"),
                    KVP("prime192v3", "prime192v3"),
                    KVP("prime239v1", "prime239v1"),
                    KVP("prime239v2", "prime239v2"),
                    KVP("prime239v3", "prime239v3"),
                }),

                // 3. SECG Koblitz 曲线
                ["secp"] = ("🔹", new List<KeyValuePair<string, string>>
                {
                    KVP("secp112r1", "secp112r1 (SECG 112-bit)"),
                    KVP("secp112r2", "secp112r2 (SECG 112-bit)"),
                    KVP("secp128r1", "secp128r1 (SECG 128-bit)"),
                    KVP("secp128r2", "secp128r2 (SECG 128-bit)"),
                    KVP("secp160k1", "secp160k1 (Koblitz 160)"),
                    KVP("secp160r1", "secp160r1 (SECG 160-bit)"),
                    KVP("secp160r2", "secp160r2 (SECG 160-bit)"),
                    KVP("secp192k1", "secp192k1 (Koblitz 192)"),
                    KVP("secp224k1", "secp224k1 (Koblitz 224)"),
                    KVP("secp256k1", "secp256k1 (Bitcoin)"),
                }),

                // 4. NIST B 二进制曲线
                ["nist-b"] = ("🔸", new List<KeyValuePair<string, string>>
                {
                    KVP("B-163", "B-163 (NIST Binary 163)"),
                    KVP("B-233", "B-233 (NIST Binary 233)"),
                    KVP("B-283", "B-283 (NIST Binary 283)"),
                    KVP("B-409", "B-409 (NIST Binary 409)"),
                    KVP("B-571", "B-571 (NIST Binary 571)"),
                }),

                // 5. Brainpool 曲线
                ["brainpool"] = ("🧠", new List<KeyValuePair<string, string>>
                {
                    KVP("brainpoolP160r1", "brainpoolP160r1"),
                    KVP("brainpoolP160t1", "brainpoolP160t1"),
                    KVP("brainpoolP192r1", "brainpoolP192r1"),
                    KVP("brainpoolP192t1", "brainpoolP192t1"),
                    KVP("brainpoolP224r1", "brainpoolP224r1"),
                    KVP("brainpoolP224t1", "brainpoolP224t1"),
                    KVP("brainpoolP256r1", "brainpoolP256r1"),
                    KVP("brainpoolP256t1", "brainpoolP256t1"),
                    KVP("brainpoolP320r1", "brainpoolP320r1"),
                    KVP("brainpoolP320t1", "brainpoolP320t1"),
                    KVP("brainpoolP384r1", "brainpoolP384r1"),
                    KVP("brainpoolP384t1", "brainpoolP384t1"),
                    KVP("brainpoolP512r1", "brainpoolP512r1"),
                    KVP("brainpoolP512t1", "brainpoolP512t1"),
                }),

                // 6. GOST 曲线
                ["gost"] = ("🇷🇺", new List<KeyValuePair<string, string>>
                {
                    KVP("GostR3410-2001-CryptoPro-A", "GOST CP-A"),
                    KVP("GostR3410-2001-CryptoPro-B", "GOST CP-B"),
                    KVP("GostR3410-2001-CryptoPro-C", "GOST CP-C"),
                    KVP("GostR3410-2001-CryptoPro-D", "GOST CP-D"),
                }),

                // 7. X9.62 c2pnb 曲线
                ["c2pnb"] = ("📶", new List<KeyValuePair<string, string>>
                {
                    KVP("c2pnb163v1", "c2pnb163v1"),
                    KVP("c2pnb163v2", "c2pnb163v2"),
                    KVP("c2pnb163v3", "c2pnb163v3"),
                    KVP("c2pnb176v1", "c2pnb176v1"),
                    KVP("c2pnb208w1", "c2pnb208w1"),
                    KVP("c2pnb272w1", "c2pnb272w1"),
                    KVP("c2pnb304w1", "c2pnb304w1"),
                    KVP("c2pnb368w1", "c2pnb368w1"),
                }),

                // 8. X9.62 c2tnb 曲线
                ["c2tnb"] = ("📡", new List<KeyValuePair<string, string>>
                {
                    KVP("c2tnb191v1", "c2tnb191v1"),
                    KVP("c2tnb191v2", "c2tnb191v2"),
                    KVP("c2tnb191v3", "c2tnb191v3"),
                    KVP("c2tnb239v1", "c2tnb239v1"),
                    KVP("c2tnb239v2", "c2tnb239v2"),
                    KVP("c2tnb239v3", "c2tnb239v3"),
                    KVP("c2tnb431r1", "c2tnb431r1"),
                }),

                // 9. WTLS 曲线
                ["wtls"] = ("📱", new List<KeyValuePair<string, string>>
                {
                    KVP("wtls1", "WTLS-1"),
                    KVP("wtls3", "WTLS-3"),
                    KVP("wtls4", "WTLS-4"),
                    KVP("wtls5", "WTLS-5"),
                    KVP("wtls6", "WTLS-6"),
                    KVP("wtls7", "WTLS-7"),
                    KVP("wtls8", "WTLS-8"),
                    KVP("wtls9", "WTLS-9"),
                    KVP("wtls10", "WTLS-10"),
                }),

                // 10. SM2 国密
                ["sm2"] = ("🇨🇳", new List<KeyValuePair<string, string>>
                {
                    KVP("sm2p256v1", "sm2p256v1 (国密 SM2)"),
                }),

                // 11. NIST P 别名
                ["nist-p"] = ("🇺🇸", new List<KeyValuePair<string, string>>
                {
                    KVP("P-192", "P-192 (NIST)"),
                    KVP("P-224", "P-224 (NIST)"),
                    KVP("P-256", "P-256 (NIST)"),
                    KVP("P-384", "P-384 (NIST)"),
                    KVP("P-521", "P-521 (NIST)"),
                }),
            };

            return result;
        }

        /// <summary>
        /// ✅ 新增：根据曲线Key获取所属分类Key（用于导入私钥后自动同步分类下拉框）
        /// </summary>
        /// <param name="curveKey">曲线Key（大小写不敏感）</param>
        /// <returns>分类Key（如prime/secp/sm2等）</returns>
        /// <exception cref="ArgumentException">曲线Key为空或不存在时抛出</exception>
        public static string GetCategoryByCurveKey(string curveKey)
        {
            if (string.IsNullOrWhiteSpace(curveKey))
                throw new ArgumentException("曲线Key不能为空", nameof(curveKey));

            if (_curveToCategory.TryGetValue(curveKey, out var categoryKey))
                return categoryKey;

            throw new ArgumentException(
                $"未找到曲线 '{curveKey}' 对应的分类，请确认曲线Key是否正确，或补充到EcdsaCurveNames的分类配置中。",
                nameof(curveKey));
        }

        private static KeyValuePair<string, string> KVP(string key, string value)
        {
            return new KeyValuePair<string, string>(key, value);
        }
    }
}