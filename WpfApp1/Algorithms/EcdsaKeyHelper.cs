using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Pkcs;
using Org.BouncyCastle.Security;

namespace CryptoTool.Algorithm.Algorithms.ECDSA 
{
    public static class EcdsaKeyHelper
    {
        public static string ExportPrivateKeyPem(ECPrivateKeyParameters privateKey)
        {
            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(privateKey);
            pemWriter.Writer.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// 将 EC 私钥导出为 SEC1/RFC 5915 namedCurve 格式 PEM（曲线用 OID 引用，体积小，与 OpenSSL 默认输出一致）
        /// </summary>
        public static string ExportPrivateKeyPemNamedCurve(ECPrivateKeyParameters privateKey)
        {
            var namedCurveOid = privateKey.PublicKeyParamSet
                ?? FindNamedCurveOid(privateKey.Parameters)
                ?? throw new ArgumentException("无法将私钥转换为 SEC1 namedCurve 格式：未找到匹配的命名曲线");

            var namedParams = new ECNamedDomainParameters(namedCurveOid, privateKey.Parameters);
            var namedPriv = new ECPrivateKeyParameters(privateKey.D, namedParams);

            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(namedPriv);
            pemWriter.Writer.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// 将 EC 私钥导出为 PKCS#8（RFC 5958）格式 PEM（BEGIN PRIVATE KEY，现代应用推荐）
        /// 注意：不能直接用 PemWriter.WriteObject(PrivateKeyInfo)——
        /// BouncyCastle 的 MiscPemGenerator 会把 EC 的 PrivateKeyInfo 自动转回 SEC1 短编码，
        /// 因此必须用 PemObject("PRIVATE KEY", der) 手工构造 PKCS#8 PEM 外壳。
        /// </summary>
        public static string ExportPrivateKeyPemPkcs8(ECPrivateKeyParameters privateKey)
        {
            var info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            byte[] der = info.GetDerEncoded();

            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            // PemObject 全限定名，避免与 OpenSsl.PemWriter 冲突
            pemWriter.WriteObject(new Org.BouncyCastle.Utilities.IO.Pem.PemObject("PRIVATE KEY", der));
            pemWriter.Writer.Flush();
            return sw.ToString();
        }

        public static string ExportPublicKeyPem(ECPublicKeyParameters publicKey)
        {
            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(publicKey);
            pemWriter.Writer.Flush();
            return sw.ToString();
        }

        /// <summary>
        /// 将 EC 公钥导出为 RFC 5480/namedCurve 格式 PEM（使用曲线 OID，体积更小，与 OpenSSL 默认输出一致）
        /// </summary>
        public static string ExportPublicKeyPemNamedCurve(ECPublicKeyParameters publicKey)
        {
            var namedCurveOid = publicKey.PublicKeyParamSet
                ?? FindNamedCurveOid(publicKey.Parameters)
                ?? throw new ArgumentException("无法将公钥转换为 namedCurve 格式：未找到匹配的命名曲线");

            var namedParams = new ECNamedDomainParameters(namedCurveOid, publicKey.Parameters);
            var namedPub = new ECPublicKeyParameters(publicKey.Q, namedParams);

            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(namedPub);
            pemWriter.Writer.Flush();
            return sw.ToString();
        }

        private static DerObjectIdentifier? FindNamedCurveOid(ECDomainParameters parameters)
        {
            foreach (string name in ECNamedCurveTable.Names)
            {
                var x9 = ECNamedCurveTable.GetByName(name);
                if (x9 == null) continue;

                var namedDomain = new ECDomainParameters(x9.Curve, x9.G, x9.N, x9.H, x9.GetSeed());
                if (CurveParametersEqual(parameters, namedDomain))
                    return ECNamedCurveTable.GetOid(name);
            }
            return null;
        }

        private static bool CurveParametersEqual(ECDomainParameters a, ECDomainParameters b)
        {
            if (a.Curve.FieldSize != b.Curve.FieldSize)
                return false;

            if (a.Curve is FpCurve fpA && b.Curve is FpCurve fpB && !fpA.Q.Equals(fpB.Q))
                return false;

            return a.Curve.A.ToBigInteger().Equals(b.Curve.A.ToBigInteger()) &&
                   a.Curve.B.ToBigInteger().Equals(b.Curve.B.ToBigInteger()) &&
                   a.N.Equals(b.N) &&
                   a.H.Equals(b.H) &&
                   a.G.AffineXCoord.ToBigInteger().Equals(b.G.AffineXCoord.ToBigInteger()) &&
                   a.G.AffineYCoord.ToBigInteger().Equals(b.G.AffineYCoord.ToBigInteger());
        }

        public static ECPrivateKeyParameters ImportPrivateKeyPem(string pem)
        {
            using var sr = new StringReader(pem);
            var pemReader = new PemReader(sr);
            var obj = pemReader.ReadObject();

            if (obj is ECPrivateKeyParameters ecPriv)
                return ecPriv;
            if (obj is AsymmetricCipherKeyPair kp)
                return (ECPrivateKeyParameters)kp.Private;

            // PKCS#8 (RFC 5958)：PrivateKeyInfo
            if (obj is PrivateKeyInfo pkcs8Info)
            {
                var privateKey = PrivateKeyFactory.CreateKey(pkcs8Info) as ECPrivateKeyParameters
                    ?? throw new ArgumentException("PKCS#8 私钥不是有效的 EC 私钥");
                return privateKey;
            }

            // 兼容某些 PEM 直接读到 AsymmetricKeyParameter 的场景
            if (obj is AsymmetricKeyParameter asymmetric && asymmetric.IsPrivate)
            {
                if (asymmetric is ECPrivateKeyParameters ecPriv2)
                    return ecPriv2;
            }

            throw new ArgumentException("无效的 EC 私钥 PEM");
        }

        public static ECPublicKeyParameters ImportPublicKeyPem(string pem)
        {
            using var sr = new StringReader(pem);
            var pemReader = new PemReader(sr);
            var obj = pemReader.ReadObject();

            if (obj is ECPublicKeyParameters ecPub)
                return ecPub;
            if (obj is AsymmetricCipherKeyPair kp)
                return (ECPublicKeyParameters)kp.Public;

            // X.509 SubjectPublicKeyInfo (RFC 5280)
            if (obj is SubjectPublicKeyInfo pubInfo)
            {
                var publicKey = PublicKeyFactory.CreateKey(pubInfo) as ECPublicKeyParameters
                    ?? throw new ArgumentException("SubjectPublicKeyInfo 公钥不是有效的 EC 公钥");
                return publicKey;
            }

            throw new ArgumentException("无效的 EC 公钥 PEM");
        }

        public static bool VerifyKeyPair(string privateKeyPem, string publicKeyPem)
        {
            var priv = ImportPrivateKeyPem(privateKeyPem);
            var pub = ImportPublicKeyPem(publicKeyPem);

            var derivedPub = EcdsaAlgorithm.GetPublicKey(priv);

            var pubBytes = pub.Q.GetEncoded(true);
            var derivedBytes = derivedPub.Q.GetEncoded(true);

            return pubBytes.SequenceEqual(derivedBytes);
        }

        /// <summary>
        /// BouncyCastle 曲线名称别名 → 规范化名称（用于统一的 GUI 显示）
        /// </summary>
        private static readonly Dictionary<string, string> CurveAliasMap = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            { "prime256v1", "secp256r1" },
            { "P-256", "secp256r1" },
        };

        public static string GetCurveName(ECPrivateKeyParameters privateKey)
        {
            foreach (string name in ECNamedCurveTable.Names)
            {
                var ecParams = ECNamedCurveTable.GetByName(name);
                if (ecParams != null
                    && ecParams.Curve?.Equals(privateKey.Parameters.Curve) == true
                    && ecParams.G?.Equals(privateKey.Parameters.G) == true
                    && ecParams.N?.Equals(privateKey.Parameters.N) == true)
                {
                    return CurveAliasMap.TryGetValue(name, out var canonical) ? canonical : name;
                }
            }
            return "未知曲线";
        }

        public static string GetCurveName(ECPublicKeyParameters publicKey)
        {
            foreach (string name in ECNamedCurveTable.Names)
            {
                var ecParams = ECNamedCurveTable.GetByName(name);
                if (ecParams != null
                    && ecParams.Curve?.Equals(publicKey.Parameters.Curve) == true
                    && ecParams.G?.Equals(publicKey.Parameters.G) == true
                    && ecParams.N?.Equals(publicKey.Parameters.N) == true)
                {
                    return CurveAliasMap.TryGetValue(name, out var canonical) ? canonical : name;
                }
            }
            return "未知曲线";
        }
    }
}
