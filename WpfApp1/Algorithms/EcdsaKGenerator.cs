// ============================================================================
// EcdsaKGenerator.cs — ECDSA "k 值(临时随机数)" 生成策略集合
// ----------------------------------------------------------------------------
// 为什么 k 重要:
//   ECDSA 签名中 k 是每次签名生成的一次性临时随机数(本质是椭圆曲线上的一个随机
//   标量)。k 必须满足:
//     1) 真随机且不重复(重复或可预测会导致私钥泄露,见 RFC 6979 引言)
//     2) 均匀落在 [1, n-1] 区间
//     3) 绝不能泄露给攻击者
//   k 的性质与 ECDH 中的"临时私钥 ePriv"同源:都是 EC 标量,但用途不同。
//
// 三种策略:
//   - 混合熵(默认,推荐):RFC 6979 确定性派生 + 随机盐,兼有确定性兜底和不可预测性
//   - RFC 6979 确定性:同一私钥同一消息签名恒定,便于测试与去重
//   - 纯 CSPRNG:完全交给 BouncyCastle 默认 SecureRandom,最快但随机源偏置即崩溃
//
// 本文件无任何 UI 依赖,可被任意界面(签名验签、文件签名、批量任务、命令行等)复用。
// ============================================================================

using System;
using System.Collections.Generic;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace CryptoTool.Algorithm.Algorithms.ECDSA
{
    /// <summary>
    /// ECDSA k 值生成策略 + 相关签名器工厂。
    /// 所有方法均为静态、无 UI 依赖,可被任意签名入口调用。
    /// </summary>
    public static class EcdsaKGenerator
    {
        // ---- k 模式常量(供 UI 下拉框 SelectedText 与 CreateSigner 判定共享) ----
        /// <summary>混合熵:RFC 6979 派生 + 随机盐(默认,推荐)</summary>
        public const string ModeHybridEntropy = "混合熵随机 k(私钥派生 + 随机盐)(默认)";

        /// <summary>RFC 6979 纯确定性:同一私钥同一消息签名恒定</summary>
        public const string ModeRfc6979 = "RFC 6979 确定性 k";

        /// <summary>纯 CSPRNG:完全交给 BouncyCastle 默认 SecureRandom</summary>
        public const string ModeCsprng = "纯 CSPRNG 随机 k";

        /// <summary>默认随机盐长度(字节),与 RFC 6979 附录 A.1 推荐一致</summary>
        public const int DefaultSaltLength = 32;

        // ====================================================================
        //  公开工厂
        // ====================================================================

        /// <summary>
        /// UI Hash 名称 -> BouncyCastle 摘要算法对象。
        /// 默认 SHA-256。未知名称不抛异常,回退到 SHA-256。
        /// </summary>
        public static IDigest CreateDigest(string hashAlg) => hashAlg switch
        {
            "SHA-224" => new Sha224Digest(),
            "SHA-256" => new Sha256Digest(),
            "SHA-384" => new Sha384Digest(),
            "SHA-512" => new Sha512Digest(),
            "SHA3-224" => new Sha3Digest(224),
            "SHA3-256" => new Sha3Digest(256),
            "SHA3-384" => new Sha3Digest(384),
            "SHA3-512" => new Sha3Digest(512),
            _ => new Sha256Digest()
        };

        /// <summary>
        /// UI Hash 名称 -> BouncyCastle 签名算法名(SHA-256withECDSA 等)。
        /// 默认 SHA-256withECDSA。
        /// </summary>
        public static string GetSignerAlgorithm(string uiHash) => uiHash switch
        {
            "SHA-224" => "SHA-224withECDSA",
            "SHA-256" => "SHA-256withECDSA",
            "SHA-384" => "SHA-384withECDSA",
            "SHA-512" => "SHA-512withECDSA",
            "SHA3-224" => "SHA3-224withECDSA",
            "SHA3-256" => "SHA3-256withECDSA",
            "SHA3-384" => "SHA3-384withECDSA",
            "SHA3-512" => "SHA3-512withECDSA",
            _ => "SHA-256withECDSA"
        };

        /// <summary>
        /// 按 k 生成方式构造签名器:
        ///   混合熵(默认):DsaDigestSigner(HybridEntropyEcdsaSigner, digest),k 由 RFC 6979+随机盐派生;
        ///   RFC 6979 确定性:DsaDigestSigner(ECDsaSigner(HMacDsaKCalculator(digest)), digest);
        ///   纯 CSPRNG:SignerUtilities.GetSigner("SHA-*withECDSA"),内部 SecureRandom。
        /// </summary>
        public static ISigner CreateSigner(string kMode, string hashAlg)
        {
            if (kMode.StartsWith("混合熵"))
                return new DsaDigestSigner(new HybridEntropyEcdsaSigner(CreateDigest(hashAlg)), CreateDigest(hashAlg));

            if (kMode.StartsWith("RFC 6979"))
            {
                IDigest digest = CreateDigest(hashAlg);
                return new DsaDigestSigner(new ECDsaSigner(new HMacDsaKCalculator(digest)), digest);
            }

            // 纯 CSPRNG / 兜底
            return SignerUtilities.GetSigner(GetSignerAlgorithm(hashAlg));
        }

        // ====================================================================
        //  核心:RFC 6979 + 随机盐 k 派生(纯函数,可复用)
        // ====================================================================

        /// <summary>
        /// RFC 6979 附录 A.1 的 k 派生 + 额外熵(随机盐)。
        /// 步骤:HMAC 初始化 V/K,int2octets(x)/bits2octets(h1)/salt 拼入两轮 HMAC,
        ///       循环拼接 T=V 直至长度 ≥ rolen,k = bits2int(T),
        ///       若 k 不在 [1, n-1] 则 HMAC(V||0x00) 重生 K/V 后继续。
        ///
        /// 纯函数:不持任何状态,无 UI 依赖。输入仅:
        ///   - privateKeyD:长期私钥 d(BigInteger)
        ///   - groupOrderN:曲线阶 n(BigInteger)
        ///   - messageHash:已 hash 后的消息摘要字节(注意:不是原文,是 Hash(message))
        ///   - digest:与签名一致的 Hash 算法
        ///   - salt:额外熵;传 null 则本函数自动生成 32 字节随机盐
        /// </summary>
        public static BigInteger GenerateRfc6979KWithSalt(
            BigInteger privateKeyD,
            BigInteger groupOrderN,
            byte[] messageHash,
            IDigest digest,
            byte[]? salt = null)
        {
            if (messageHash == null) throw new ArgumentNullException(nameof(messageHash));
            if (digest == null) throw new ArgumentNullException(nameof(digest));

            int hlen = digest.GetDigestSize();
            int qlen = groupOrderN.BitLength;
            int rolen = (qlen + 7) / 8;

            byte[] effectiveSalt = salt ?? new byte[DefaultSaltLength];
            if (salt == null)
                new SecureRandom().NextBytes(effectiveSalt);

            byte[] v = new byte[hlen];
            byte[] k = new byte[hlen];
            Arrays.Fill(v, (byte)0x01);

            byte[] bx = Int2Octets(privateKeyD, rolen);
            byte[] z1 = Bits2Octets(messageHash, groupOrderN, rolen);

            var hmac = new HMac(digest);

            // K = HMAC_K(V || 0x00 || int2octets(x) || bits2octets(h1) || salt)
            hmac.Init(new KeyParameter(k));
            hmac.BlockUpdate(v, 0, v.Length);
            hmac.Update(0x00);
            hmac.BlockUpdate(bx, 0, bx.Length);
            hmac.BlockUpdate(z1, 0, z1.Length);
            hmac.BlockUpdate(effectiveSalt, 0, effectiveSalt.Length);
            k = DoFinal(hmac);

            // V = HMAC_K(V)
            v = HmacOnce(v, k, digest);

            // K = HMAC_K(V || 0x01 || int2octets(x) || bits2octets(h1) || salt)
            hmac.Init(new KeyParameter(k));
            hmac.BlockUpdate(v, 0, v.Length);
            hmac.Update(0x01);
            hmac.BlockUpdate(bx, 0, bx.Length);
            hmac.BlockUpdate(z1, 0, z1.Length);
            hmac.BlockUpdate(effectiveSalt, 0, effectiveSalt.Length);
            k = DoFinal(hmac);

            // V = HMAC_K(V)
            v = HmacOnce(v, k, digest);

            // 循环产出 k,直到落在 [1, n-1]
            while (true)
            {
                var t = new List<byte>();
                while (t.Count < rolen)
                {
                    v = HmacOnce(v, k, digest);
                    t.AddRange(v);
                }

                BigInteger candidate = Bits2Int([.. t], qlen);
                if (candidate.SignValue >= 1 && candidate.CompareTo(groupOrderN) < 0)
                    return candidate;

                // K = HMAC_K(V || 0x00);V = HMAC_K(V)
                hmac.Init(new KeyParameter(k));
                hmac.BlockUpdate(v, 0, v.Length);
                hmac.Update(0x00);
                k = DoFinal(hmac);
                v = HmacOnce(v, k, digest);
            }
        }

        // ====================================================================
        //  内部辅助:RFC 6979 派生所需的位/字节转换
        // ====================================================================

        private static byte[] HmacOnce(byte[] data, byte[] key, IDigest digest)
        {
            var hmac = new HMac(digest);
            hmac.Init(new KeyParameter(key));
            hmac.BlockUpdate(data, 0, data.Length);
            byte[] result = new byte[hmac.GetMacSize()];
            hmac.DoFinal(result, 0);
            return result;
        }

        private static byte[] DoFinal(HMac hmac)
        {
            byte[] result = new byte[hmac.GetMacSize()];
            hmac.DoFinal(result, 0);
            return result;
        }

        /// <summary>int2octets:大整数转无符号大端字节,补齐到 rolen。</summary>
        private static byte[] Int2Octets(BigInteger x, int rolen)
        {
            byte[] bytes = x.ToByteArrayUnsigned();
            if (bytes.Length < rolen)
            {
                byte[] padded = new byte[rolen];
                Array.Copy(bytes, 0, padded, rolen - bytes.Length, bytes.Length);
                return padded;
            }
            return bytes;
        }

        /// <summary>bits2octets:哈希截断 + mod n,再 int2octets 到 rolen。</summary>
        private static byte[] Bits2Octets(byte[] hash, BigInteger n, int rolen)
        {
            BigInteger z = Bits2Int(hash, n.BitLength);
            BigInteger z1 = z.Subtract(n);
            if (z1.SignValue < 0)
                z1 = z;
            return Int2Octets(z1, rolen);
        }

        /// <summary>bits2int:大端字节转大整数,超出 qlen 的位右移截断。</summary>
        private static BigInteger Bits2Int(byte[] bytes, int qlen)
        {
            BigInteger x = new(1, bytes);
            int blen = bytes.Length * 8;
            if (blen > qlen)
                x = x.ShiftRight(blen - qlen);
            return x;
        }
    }

    /// <summary>
    /// 混合熵 ECDSA 签名器:确定性熵(私钥+消息哈希)+ 随机盐(每次签名不同)。
    /// 等价于 RFC 6979 附录 A.1 的"标准确定性 + 额外熵"扩展实现。
    /// 可被任意需要自定义 k 派生策略的签名流程直接复用。
    /// </summary>
    public sealed class HybridEntropyEcdsaSigner : IDsa
    {
        private readonly IDigest _digest;
        private ECPrivateKeyParameters? _privKey;

        public HybridEntropyEcdsaSigner(IDigest digest)
        {
            _digest = digest ?? throw new ArgumentNullException(nameof(digest));
        }

        public string AlgorithmName => "ECDSA";

        public BigInteger Order => _privKey?.Parameters.N
            ?? throw new InvalidOperationException("混合熵签名器未初始化");

        public void Init(bool forSigning, ICipherParameters parameters)
        {
            if (!forSigning)
                throw new InvalidOperationException("混合熵签名器仅支持签名");
            _privKey = parameters switch
            {
                ECPrivateKeyParameters k => k,
                ParametersWithRandom r => (ECPrivateKeyParameters)r.Parameters,
                _ => throw new InvalidKeyException("混合熵签名器需要 EC 私钥参数")
            };
        }

        public BigInteger[] GenerateSignature(byte[] message)
        {
            if (_privKey == null)
                throw new InvalidOperationException("混合熵签名器未初始化");

            var n = _privKey.Parameters.N;

            // k = RFC6979(priv, h1) + 随机盐 —— 委托给 EcdsaKGenerator 的纯函数
            BigInteger k = EcdsaKGenerator.GenerateRfc6979KWithSalt(_privKey.D, n, message, _digest);

            // R = k × G,r = R.x mod n
            ECPoint point = _privKey.Parameters.G.Multiply(k).Normalize();
            BigInteger r = point.AffineXCoord.ToBigInteger().Mod(n);
            if (r.SignValue == 0)
                throw new InvalidOperationException("r 为零,请重试");

            // s = k⁻¹(z + r·d) mod n
            BigInteger e = CalculateE(n, message);
            BigInteger s = k.ModInverse(n).Multiply(e.Add(r.Multiply(_privKey.D))).Mod(n);
            if (s.SignValue == 0)
                throw new InvalidOperationException("s 为零,请重试");

            return [r, s];
        }

        public bool VerifySignature(byte[] message, BigInteger r, BigInteger s)
            => throw new NotSupportedException("混合熵签名器仅支持签名");

        /// <summary>把消息哈希截断到曲线序 n 的比特长度(FIPS 186-4 / SEC1 的 z 计算)。</summary>
        private static BigInteger CalculateE(BigInteger n, byte[] message)
        {
            int nBits = n.BitLength;
            int msgBits = message.Length * 8;
            BigInteger e = new(1, message);
            if (nBits < msgBits)
                e = e.ShiftRight(msgBits - nBits);
            return e;
        }
    }
}