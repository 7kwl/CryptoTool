using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Asn1;
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
        /// 将 EC 私钥导出为 PKCS#8 (RFC 5958) 格式 PEM
        /// </summary>
        public static string ExportPrivateKeyPemPkcs8(ECPrivateKeyParameters privateKey)
        {
            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            var pkcs8Info = PrivateKeyInfoFactory.CreatePrivateKeyInfo(privateKey);
            pemWriter.WriteObject(new Org.BouncyCastle.Utilities.IO.Pem.PemObject("PRIVATE KEY", pkcs8Info.GetEncoded()));
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

            var algorithmIdentifier = new AlgorithmIdentifier(X9ObjectIdentifiers.IdECPublicKey, namedCurveOid);
            var namedCurveSpi = new SubjectPublicKeyInfo(algorithmIdentifier, publicKey.Q.GetEncoded(false));

            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(new Org.BouncyCastle.Utilities.IO.Pem.PemObject("PUBLIC KEY", namedCurveSpi.GetEncoded()));
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