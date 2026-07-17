using System;
using System.IO;
using System.Linq;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
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

        public static string ExportPublicKeyPem(ECPublicKeyParameters publicKey)
        {
            using var sw = new StringWriter();
            var pemWriter = new PemWriter(sw);
            pemWriter.WriteObject(publicKey);
            pemWriter.Writer.Flush();
            return sw.ToString();
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
                    return name;
                }
            }
            return "未知曲线";
        }
    }
}