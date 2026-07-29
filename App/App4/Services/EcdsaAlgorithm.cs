using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Security;
using System;

namespace App4.Services;

/// <summary>
/// ECDSA 签名/验签/密钥生成核心算法
/// 代码来源：BouncyCastle 官方文档 + CryptoTool.Algorithm
/// </summary>
public static class EcdsaAlgorithm
{
    private static readonly SecureRandom Random = new();

    /// <summary>
    /// 生成 ECDSA 密钥对
    /// </summary>
    /// <param name="curveName">曲线名：secp256r1, secp384r1, secp521r1 等</param>
    public static AsymmetricCipherKeyPair GenerateKeyPair(string curveName)
    {
        var ecParams = ECNamedCurveTable.GetByName(curveName)
            ?? throw new ArgumentException($"不支持的曲线: {curveName}");

        var oid = X962NamedCurves.GetOid(curveName);
        var domain = new ECDomainParameters(ecParams.Curve, ecParams.G,
            ecParams.N, ecParams.H, ecParams.GetSeed());

        var generator = new ECKeyPairGenerator("ECDSA");
        generator.Init(new ECKeyGenerationParameters(domain, Random));
        return generator.GenerateKeyPair();
    }

    /// <summary>
    /// ECDSA 签名
    /// </summary>
    /// <param name="data">待签名数据</param>
    /// <param name="privateKey">ECDSA 私钥</param>
    /// <param name="digest">哈希算法：SHA-256, SHA-384, SHA-512</param>
    /// <returns>DER 格式签名</returns>
    public static byte[] Sign(byte[] data, ECPrivateKeyParameters privateKey, string digest = "SHA-256")
    {
        ISigner signer = SignerUtilities.GetSigner($"{digest}withECDSA");
        signer.Init(true, privateKey);
        signer.BlockUpdate(data, 0, data.Length);
        return signer.GenerateSignature();
    }

    /// <summary>
    /// ECDSA 验签
    /// </summary>
    /// <param name="data">原始数据</param>
    /// <param name="signature">DER 格式签名</param>
    /// <param name="publicKey">ECDSA 公钥</param>
    /// <param name="digest">哈希算法：SHA-256, SHA-384, SHA-512</param>
    /// <returns>true = 验签通过</returns>
    public static bool Verify(byte[] data, byte[] signature, ECPublicKeyParameters publicKey, string digest = "SHA-256")
    {
        ISigner signer = SignerUtilities.GetSigner($"{digest}withECDSA");
        signer.Init(false, publicKey);
        signer.BlockUpdate(data, 0, data.Length);
        return signer.VerifySignature(signature);
    }
}
