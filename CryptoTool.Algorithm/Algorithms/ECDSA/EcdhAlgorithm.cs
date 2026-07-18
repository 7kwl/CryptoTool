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
    /// ECDH / ECIES 密钥派生模式
    /// </summary>
    public enum EcdhMode
    {
        /// <summary>
        /// CryptoTool 默认模式：HKDF-SHA256 + 12 字节 IV(Base64)
        /// </summary>
        CryptoTool,
        /// <summary>
        /// 8gwifi.org 兼容模式：原始共享密钥截断 + 16 字节 IV(Hex)
        /// </summary>
        GwifiOrg,
        /// <summary>
        /// ANSI X9.63：X9.63-KDF(SHA-256) + 12 字节 IV(Base64)，ECIES 临时密钥
        /// </summary>
        AnsiX963,
        /// <summary>
        /// IEEE 1363a：KDF2(SHA-256) + 12 字节 IV(Base64)，ECIES 临时密钥
        /// </summary>
        Ieee1363a,
        /// <summary>
        /// ISO/IEC 18033-2：KDF2(SHA-256) + 12 字节 IV(Base64)，ECIES 临时密钥
        /// </summary>
        Iso180332,
        /// <summary>
        /// SECG SEC 1 v2.0：X9.63-KDF(SHA-256) + 12 字节 IV(Base64)，ECIES 临时密钥
        /// </summary>
        SecgSec1
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
                // 8gwifi.org 直接使用原始 ECDH 共享密钥（X 坐标）作为 AES 密钥，
                // 长度超过 32 字节时取前 32 字节（兼容 P-384 / P-521）
                EcdhMode.GwifiOrg => TruncateToAesKey(sharedSecret, 32),
                // ECIES 标准模式：ANSI X9.63 / IEEE 1363a / ISO/IEC 18033-2 / SECG SEC 1
                // 均使用 ANSI X9.63-KDF (SHA-256, SharedInfo 为空)
                EcdhMode.AnsiX963 => X963Kdf(sharedSecret, 32),
                EcdhMode.Ieee1363a => X963Kdf(sharedSecret, 32),
                EcdhMode.Iso180332 => X963Kdf(sharedSecret, 32),
                EcdhMode.SecgSec1 => X963Kdf(sharedSecret, 32),
                _ => DeriveAesKey(sharedSecret, null, System.Text.Encoding.UTF8.GetBytes("ECDH-AES-GCM"))
            };
        }

        /// <summary>
        /// ANSI X9.63 KDF (等同 IEEE 1363a KDF2 / ISO/IEC 18033-2 KDF2 / SECG SEC 1)
        /// K = Hash(counter || Z || SharedInfo)
        /// counter 为 4 字节大端序，从 1 开始递增
        /// </summary>
        private static byte[] X963Kdf(byte[] sharedSecret, int keyLengthInBytes)
        {
            var digest = new Sha256Digest();
            int hashLen = digest.GetDigestSize(); // 32
            byte[] result = new byte[keyLengthInBytes];
            int offset = 0;
            uint counter = 1;

            while (offset < keyLengthInBytes)
            {
                digest.Reset();
                // counter (4 bytes, big-endian)
                byte[] counterBytes = new byte[4];
                counterBytes[0] = (byte)(counter >> 24);
                counterBytes[1] = (byte)(counter >> 16);
                counterBytes[2] = (byte)(counter >> 8);
                counterBytes[3] = (byte)(counter);
                digest.BlockUpdate(counterBytes, 0, 4);
                // Z (shared secret)
                digest.BlockUpdate(sharedSecret, 0, sharedSecret.Length);
                // SharedInfo (empty per ECIES defaults)
                byte[] hash = new byte[hashLen];
                digest.DoFinal(hash, 0);

                int copyLen = Math.Min(hashLen, keyLengthInBytes - offset);
                Buffer.BlockCopy(hash, 0, result, offset, copyLen);
                offset += copyLen;
                counter++;
            }
            return result;
        }

        /// <summary>
        /// 8gwifi.org 兼容：直接取共享密钥前 32 字节作为 AES-256 密钥
        /// </summary>
        private static byte[] TruncateToAesKey(byte[] sharedSecret, int length)
        {
            byte[] result = new byte[length];
            int copyLength = Math.Min(length, sharedSecret.Length);
            Buffer.BlockCopy(sharedSecret, 0, result, 0, copyLength);
            return result;
        }

        /// <summary>
        /// 获取指定模式对应的 Nonce / IV 长度
        /// </summary>
        public static int GetNonceLength(EcdhMode mode)
        {
            return mode switch
            {
                EcdhMode.GwifiOrg => 16,  // 8gwifi.org 使用 16 字节 IV
                _ => 12                    // CryptoTool 及所有 ECIES 标准模式使用 12 字节 IV
            };
        }

        /// <summary>
        /// 将 IV 按模式要求的格式编码为字符串
        /// </summary>
        public static string FormatIv(byte[] iv, EcdhMode mode)
        {
            return mode switch
            {
                EcdhMode.GwifiOrg => ToHexString(iv),     // 8gwifi.org 使用 Hex 编码
                _ => Convert.ToBase64String(iv)            // 其余模式使用 Base64
            };
        }

        /// <summary>
        /// 按模式要求的格式将 IV 字符串解析为字节
        /// </summary>
        public static byte[] ParseIv(string ivText, EcdhMode mode)
        {
            return mode switch
            {
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
        /// 获取指定曲线的临时公钥编码长度（未压缩格式：0x04 || X || Y）
        /// </summary>
        public static int GetEphemeralPubKeyLength(string curveName)
        {
            X9ECParameters curveParams = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"不支持的曲线: {curveName}");
            int fieldSize = (curveParams.Curve.FieldSize + 7) / 8;
            return 1 + 2 * fieldSize; // 0x04 || X || Y
        }

        /// <summary>
        /// ECIES 加密：生成临时密钥对，用接收方公钥加密
        /// 输出格式：ephemeral_pub(未压缩) || IV || cipher+tag
        /// </summary>
        public static byte[] EciesEncrypt(
            byte[] plain,
            ECPublicKeyParameters recipientPub,
            string curveName,
            EcdhMode mode,
            out byte[] sharedSecret,
            out byte[] iv)
        {
            // 1. 生成临时 EC 密钥对
            var ephemeralKp = GenerateKeyPair(curveName);
            var ephemeralPriv = (ECPrivateKeyParameters)ephemeralKp.Private;
            var ephemeralPub = (ECPublicKeyParameters)ephemeralKp.Public;

            // 2. 将临时公钥编码为未压缩格式（0x04 || X || Y）
            byte[] ephemeralPubEncoded = ephemeralPub.Q.GetEncoded(false);

            // 3. 计算共享密钥：临时私钥 × 接收方公钥
            sharedSecret = DeriveSharedSecret(ephemeralPriv, recipientPub);

            // 4. 派生 AES 密钥
            byte[] key = DeriveAesKey(sharedSecret, mode);

            // 5. 生成随机 IV
            iv = GenerateNonce(GetNonceLength(mode));

            // 6. AES-GCM 加密
            byte[] cipher = EncryptAesGcm(plain, key, iv);

            // 7. 拼接：ephemeral_pub_encoded || IV || cipher+tag
            byte[] combined = new byte[ephemeralPubEncoded.Length + iv.Length + cipher.Length];
            Buffer.BlockCopy(ephemeralPubEncoded, 0, combined, 0, ephemeralPubEncoded.Length);
            Buffer.BlockCopy(iv, 0, combined, ephemeralPubEncoded.Length, iv.Length);
            Buffer.BlockCopy(cipher, 0, combined, ephemeralPubEncoded.Length + iv.Length, cipher.Length);

            return combined;
        }

        /// <summary>
        /// ECIES 解密：从密文中提取临时公钥，用接收方私钥解密
        /// </summary>
        /// <param name="combined">ephemeral_pub_encoded || IV || cipher+tag</param>
        public static byte[] EciesDecrypt(
            byte[] combined,
            ECPrivateKeyParameters recipientPriv,
            string curveName,
            EcdhMode mode,
            out byte[] sharedSecret,
            out byte[] iv)
        {
            X9ECParameters curveParams = ECNamedCurveTable.GetByName(curveName)
                ?? throw new ArgumentException($"不支持的曲线: {curveName}");

            // 1. 计算临时公钥长度（未压缩格式：1 + 2*fieldSize）
            int fieldSize = (curveParams.Curve.FieldSize + 7) / 8;
            int pubKeyLength = 1 + 2 * fieldSize;

            int ivLength = GetNonceLength(mode);
            int minLen = pubKeyLength + ivLength + 16; // pub + IV + 至少 16 字节 Tag
            if (combined.Length < minLen)
                throw new FormatException($"ECIES 密文过短，需要至少 {minLen} 字节（含临时公钥 {pubKeyLength}B + IV {ivLength}B + Tag 16B）");

            // 2. 提取临时公钥
            byte[] ephemeralPubEncoded = new byte[pubKeyLength];
            Buffer.BlockCopy(combined, 0, ephemeralPubEncoded, 0, pubKeyLength);

            // 3. 解码临时公钥点
            var ephemeralPoint = curveParams.Curve.DecodePoint(ephemeralPubEncoded);
            var domain = new ECDomainParameters(curveParams.Curve, curveParams.G, curveParams.N, curveParams.H, curveParams.GetSeed());
            var ephemeralPub = new ECPublicKeyParameters("ECDH", ephemeralPoint, domain);

            // 4. 提取 IV
            iv = new byte[ivLength];
            Buffer.BlockCopy(combined, pubKeyLength, iv, 0, ivLength);

            // 5. 提取密文
            byte[] cipher = new byte[combined.Length - pubKeyLength - ivLength];
            Buffer.BlockCopy(combined, pubKeyLength + ivLength, cipher, 0, cipher.Length);

            // 6. 计算共享密钥：接收方私钥 × 临时公钥
            sharedSecret = DeriveSharedSecret(recipientPriv, ephemeralPub);

            // 7. 派生 AES 密钥
            byte[] key = DeriveAesKey(sharedSecret, mode);

            // 8. AES-GCM 解密
            return DecryptAesGcm(cipher, key, iv);
        }

        /// <summary>
        /// 静态 ECDH 加密（8gwifi.org 兼容模式）：使用 Alice 私钥 + Bob 公钥计算共享密钥
        /// 输出格式：Base64(IV + cipher+tag)，与 8gwifi.org 的解密流程兼容
        /// </summary>
        public static string StaticEcdhEncrypt(
            byte[] plain,
            ECPrivateKeyParameters alicePriv,
            ECPublicKeyParameters bobPub,
            EcdhMode mode,
            out byte[] sharedSecret,
            out byte[] iv)
        {
            sharedSecret = DeriveSharedSecret(alicePriv, bobPub);
            byte[] key = DeriveAesKey(sharedSecret, mode);
            iv = GenerateNonce(GetNonceLength(mode));
            byte[] cipher = EncryptAesGcm(plain, key, iv);
            return FormatCipher(iv, cipher, mode);
        }

        /// <summary>
        /// 静态 ECDH 解密（8gwifi.org 兼容模式）：使用 Bob 私钥 + Alice 公钥计算共享密钥
        /// 输入格式：Base64(IV + cipher+tag)
        /// </summary>
        public static byte[] StaticEcdhDecrypt(
            string cipherBase64,
            ECPrivateKeyParameters bobPriv,
            ECPublicKeyParameters alicePub,
            EcdhMode mode,
            out byte[] sharedSecret,
            out byte[] iv)
        {
            sharedSecret = DeriveSharedSecret(bobPriv, alicePub);
            byte[] key = DeriveAesKey(sharedSecret, mode);
            var ivCipher = ParseCipher(cipherBase64, mode);
            iv = ivCipher.iv;
            byte[] cipher = ivCipher.cipher;
            return DecryptAesGcm(cipher, key, iv);
        }

        /// <summary>
        /// 按模式将 IV 与密文打包成字符串（8gwifi.org 模式返回 Base64(IV + cipher+tag)）
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
