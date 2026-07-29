using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math.EC;
using Org.BouncyCastle.OpenSsl;

namespace App2.Services;

/// <summary>
/// ECDSA 密钥 PEM 导入/导出
/// </summary>
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
}
