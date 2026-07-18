using System;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;

namespace CryptoTool.Algorithm.Algorithms.ECDSA
{
    /// <summary>
    /// ECDH 兼容模式
    /// </summary>
    public enum EcdhMode
    {
        /// <summary>
        /// CryptoTool 默认模式：HKDF-SHA256 + 12 字节 IV(Base64)
        /// </summary>
        CryptoTool,
        /// <summary>
        /// 8gwifi.org 兼容模式：SHA-256 KDF + 16 字节 IV(Hex)
        /// </summary>
        GwifiOrg
    }

    /// <summary>
    /// ECDH (Elliptic Curve Diffie-Hellman) 密钥协商算法
    /// </summary>
    public static class EcdhAlgorithm
    {
        /// <summary>
        /// 生成 ECDH 密钥对
        /// </summary>
        public static AsymmetricCipherKeyPair GenerateKeyPair(string curveName)
        {
            X9ECParameters curveParams = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"不支持的曲线: {curveName}");

            var domain = new ECDomainParameters(curveParams.Curve, curveParams.G, curveParams.N, curveParams.H, curveParams.GetSeed());
            var generator = new ECKeyPairGenerator("ECDH");
            generator.Init(new ECKeyGenerationParameters(domain, new SecureRandom()));
            return generator.GenerateKeyPair();
        }

        /// <summary>
        /// 使用 ECDH 协商共享密钥（返回原始 x 坐标字节）
        /// </summary>
        public static byte[] DeriveSharedSecret(ECPrivateKeyParameters privateKey, ECPublicKeyParameters publicKey)
        {
            var agreement = new ECDHBasicAgreement();
            agreement.Init(privateKey);
            BigInteger shared = agreement.CalculateAgreement(publicKey);

            // 将共享密钥的 x 坐标转为定长字节数组（左补零）
            int fieldSize = (publicKey.Parameters.Curve.FieldSize + 7) / 8;
            byte[] sharedBytes = shared.ToByteArrayUnsigned();
            if (sharedBytes.Length < fieldSize)
            {
                byte[] padded = new byte[fieldSize];
                Buffer.BlockCopy(sharedBytes, 0, padded, fieldSize - sharedBytes.Length, sharedBytes.Length);
                sharedBytes = padded;
            }
            return sharedBytes;
        }

        /// <summary>
        /// 使用 HKDF-SHA256 从共享密钥派生 AES 密钥
        /// </summary>
        public static byte[] DeriveAesKey(byte[] sharedSecret, byte[]? salt, byte[] info, int keySizeInBytes = 32)
        {
            var hkdf = new HkdfBytesGenerator(new Sha256Digest());
            hkdf.Init(new HkdfParameters(sharedSecret, salt, info));
            byte[] derivedKey = new byte[keySizeInBytes];
            hkdf.GenerateBytes(derivedKey, 0, derivedKey.Length);
            return derivedKey;
        }

        /// <summary>
        /// 根据兼容模式派生 AES 密钥
        /// </summary>
        public static byte[] DeriveAesKey(byte[] sharedSecret, EcdhMode mode)
        {
            return mode switch
            {
                EcdhMode.CryptoTool => DeriveAesKey(sharedSecret, null, System.Text.Encoding.UTF8.GetBytes("ECDH-AES-GCM")),
                EcdhMode.GwifiOrg => Sha256Kdf(sharedSecret, 32),
                _ => DeriveAesKey(sharedSecret, null, System.Text.Encoding.UTF8.GetBytes("ECDH-AES-GCM"))
            };
        }

        private static byte[] Sha256Kdf(byte[] sharedSecret, int length)
        {
            var digest = new Sha256Digest();
            byte[] hash = new byte[digest.GetDigestSize()];
            digest.BlockUpdate(sharedSecret, 0, sharedSecret.Length);
            digest.DoFinal(hash, 0);
            byte[] result = new byte[length];
            Buffer.BlockCopy(hash, 0, result, 0, Math.Min(length, hash.Length));
            return result;
        }

        /// <summary>
        /// 获取指定模式对应的 Nonce / IV 长度
        /// </summary>
        public static int GetNonceLength(EcdhMode mode)
        {
            return mode switch
            {
                EcdhMode.CryptoTool => 12,
                EcdhMode.GwifiOrg => 16,
                _ => 12
            };
        }

        /// <summary>
        /// 将 IV 按模式要求的格式编码为字符串
        /// </summary>
        public static string FormatIv(byte[] iv, EcdhMode mode)
        {
            return mode switch
            {
                EcdhMode.CryptoTool => Convert.ToBase64String(iv),
                EcdhMode.GwifiOrg => ToHexString(iv),
                _ => Convert.ToBase64String(iv)
            };
        }

        /// <summary>
        /// 按模式要求的格式将 IV 字符串解析为字节
        /// </summary>
        public static byte[] ParseIv(string ivText, EcdhMode mode)
        {
            return mode switch
            {
                EcdhMode.CryptoTool => Convert.FromBase64String(ivText.Trim()),
                EcdhMode.GwifiOrg => FromHexString(ivText.Trim()),
                _ => Convert.FromBase64String(ivText.Trim())
            };
        }

        private static string ToHexString(byte[] bytes)
        {
            if (bytes == null || bytes.Length == 0) return string.Empty;
            char[] chars = new char[bytes.Length * 2];
            for (int i = 0; i < bytes.Length; i++)
            {
                byte b = bytes[i];
                chars[i * 2] = GetHexValue(b / 16);
                chars[i * 2 + 1] = GetHexValue(b % 16);
            }
            return new string(chars);
        }

        private static char GetHexValue(int i) => (char)(i < 10 ? i + 48 : i + 87);

        private static byte[] FromHexString(string hex)
        {
            if (string.IsNullOrWhiteSpace(hex)) return Array.Empty<byte>();
            hex = hex.Trim();
            if (hex.Length % 2 != 0)
                throw new FormatException("Hex 字符串长度必须是偶数");
            byte[] bytes = new byte[hex.Length / 2];
            for (int i = 0; i < hex.Length; i += 2)
            {
                bytes[i / 2] = (byte)((GetHexDigit(hex[i]) << 4) | GetHexDigit(hex[i + 1]));
            }
            return bytes;
        }

        private static int GetHexDigit(char c)
        {
            if (c >= '0' && c <= '9') return c - '0';
            if (c >= 'a' && c <= 'f') return c - 'a' + 10;
            if (c >= 'A' && c <= 'F') return c - 'A' + 10;
            throw new FormatException($"无效的 Hex 字符: {c}");
        }

        /// <summary>
        /// 按模式将 IV 与密文打包成字符串（CryptoTool 只返回密文，8gwifi.org 返回 IV + 密文）
        /// </summary>
        public static string FormatCipher(byte[] iv, byte[] cipher, EcdhMode mode)
        {
            if (mode == EcdhMode.GwifiOrg)
            {
                byte[] combined = new byte[iv.Length + cipher.Length];
                Buffer.BlockCopy(iv, 0, combined, 0, iv.Length);
                Buffer.BlockCopy(cipher, 0, combined, iv.Length, cipher.Length);
                return Convert.ToBase64String(combined);
            }
            return Convert.ToBase64String(cipher);
        }

        /// <summary>
        /// 按模式从字符串中拆分 IV 与密文
        /// </summary>
        public static (byte[] iv, byte[] cipher) ParseCipher(string cipherText, EcdhMode mode)
        {
            if (mode == EcdhMode.GwifiOrg)
            {
                byte[] combined = Convert.FromBase64String(cipherText.Trim());
                int ivLength = GetNonceLength(mode);
                int minLen = ivLength + 16; // 至少 IV + 16 字节 Tag
                if (combined.Length < minLen)
                    throw new FormatException($"密文过短，8gwifi.org 模式需要至少 {minLen} 字节（含 IV + Tag）");
                byte[] iv = new byte[ivLength];
                byte[] cipher = new byte[combined.Length - ivLength];
                Buffer.BlockCopy(combined, 0, iv, 0, ivLength);
                Buffer.BlockCopy(combined, ivLength, cipher, 0, cipher.Length);
                return (iv, cipher);
            }
            return (Array.Empty<byte>(), Convert.FromBase64String(cipherText.Trim()));
        }

        /// <summary>
        /// 生成随机 Nonce / IV
        /// </summary>
        public static byte[] GenerateNonce(int length)
        {
            return SecureRandom.GetNextBytes(new SecureRandom(), length);
        }

        /// <summary>
        /// AES-256-GCM 加密（密文 + 16字节 Tag 拼接输出）
        /// </summary>
        public static byte[] EncryptAesGcm(byte[] plain, byte[] key, byte[] nonce)
        {
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), 128, nonce);
            cipher.Init(true, parameters);

            byte[] output = new byte[cipher.GetOutputSize(plain.Length)];
            int len = cipher.ProcessBytes(plain, 0, plain.Length, output, 0);
            cipher.DoFinal(output, len);

            return output;
        }

        /// <summary>
        /// AES-256-GCM 解密（输入为密文 + 16字节 Tag 拼接）
        /// </summary>
        public static byte[] DecryptAesGcm(byte[] cipherWithTag, byte[] key, byte[] nonce)
        {
            var cipher = new GcmBlockCipher(new AesEngine());
            var parameters = new AeadParameters(new KeyParameter(key), 128, nonce);
            cipher.Init(false, parameters);

            byte[] output = new byte[cipher.GetOutputSize(cipherWithTag.Length)];
            int len = cipher.ProcessBytes(cipherWithTag, 0, cipherWithTag.Length, output, 0);
            int finalLen = cipher.DoFinal(output, len);

            byte[] plain = new byte[len + finalLen];
            Buffer.BlockCopy(output, 0, plain, 0, plain.Length);
            return plain;
        }
    }
}
