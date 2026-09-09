using System;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;

#pragma warning disable IDE0130 // 命名空间与文件夹结构不匹配：保留 CryptoTool.Algorithm.Algorithms.ECDSA 以兼容历史引用
namespace CryptoTool.Algorithm.Algorithms.ECDSA 
{
    public class EcdsaAlgorithm
    {
        private static readonly SecureRandom Random = new();

        public static AsymmetricCipherKeyPair GenerateKeyPair(string curveName)
        {
            X9ECParameters ecParams = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"不支持的曲线: {curveName}");

            var oid = ECNamedCurveTable.GetOid(curveName);
            var domain = new ECNamedDomainParameters(oid,
                ecParams.Curve, ecParams.G, ecParams.N, ecParams.H, ecParams.GetSeed());

            var generator = new ECKeyPairGenerator("ECDSA");
            generator.Init(new ECKeyGenerationParameters(domain, Random));

            return generator.GenerateKeyPair();
        }

        public static byte[] Sign(byte[] data, ECPrivateKeyParameters privateKey, string digest = "SHA-256")
        {
            ISigner signer = SignerUtilities.GetSigner($"{digest}withECDSA");
            signer.Init(true, privateKey);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.GenerateSignature();
        }

        public static bool Verify(byte[] data, byte[] signature, ECPublicKeyParameters publicKey, string digest = "SHA-256")
        {
            ISigner signer = SignerUtilities.GetSigner($"{digest}withECDSA");
            signer.Init(false, publicKey);
            signer.BlockUpdate(data, 0, data.Length);
            return signer.VerifySignature(signature);
        }

        public static ECPublicKeyParameters GetPublicKey(ECPrivateKeyParameters privateKey)
        {
            var q = privateKey.Parameters.G.Multiply(privateKey.D);
            return new ECPublicKeyParameters(q, privateKey.Parameters);
        }
    }
}
