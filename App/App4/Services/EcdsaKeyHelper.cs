using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using System;
using System.IO;

namespace App4.Services;

/// <summary>
/// ECDSA 密钥 PEM 格式导入/导出
/// 代码来源：CryptoTool.Algorithm
/// </summary>
public static class EcdsaKeyHelper
{
    /// <summary>
    /// 导出私钥为 PEM（PKCS#8 格式，BEGIN PRIVATE KEY）
    /// </summary>
    public static string ExportPrivateKeyPem(ECPrivateKeyParameters privateKey)
    {
        using var sw = new StringWriter();
        var pemWriter = new PemWriter(sw);
        pemWriter.WriteObject(privateKey);
        pemWriter.Writer.Flush();
        return sw.ToString();
    }

    /// <summary>
    /// 导出公钥为 PEM（SubjectPublicKeyInfo 格式，BEGIN PUBLIC KEY）
    /// </summary>
    public static string ExportPublicKeyPem(ECPublicKeyParameters publicKey)
    {
        using var sw = new StringWriter();
        var pemWriter = new PemWriter(sw);
        pemWriter.WriteObject(publicKey);
        pemWriter.Writer.Flush();
        return sw.ToString();
    }

    /// <summary>
    /// 从 PEM 导入私钥
    /// </summary>
    public static ECPrivateKeyParameters ImportPrivateKeyPem(string pem)
    {
        using var sr = new StringReader(pem);
        var pemReader = new PemReader(sr);
        var obj = pemReader.ReadObject();

        if (obj is ECPrivateKeyParameters ecPriv)
            return ecPriv;
        if (obj is AsymmetricCipherKeyPair kp)
            return (ECPrivateKeyParameters)kp.Private;

        throw new ArgumentException("无效的 EC 私钥 PEM 格式");
    }

    /// <summary>
    /// 从 PEM 导入公钥
    /// </summary>
    public static ECPublicKeyParameters ImportPublicKeyPem(string pem)
    {
        using var sr = new StringReader(pem);
        var pemReader = new PemReader(sr);
        var obj = pemReader.ReadObject();

        if (obj is ECPublicKeyParameters ecPub)
            return ecPub;
        if (obj is AsymmetricCipherKeyPair kp)
            return (ECPublicKeyParameters)kp.Public;

        throw new ArgumentException("无效的 EC 公钥 PEM 格式");
    }
}
