using CryptoTool.Algorithm.Enums;
using CryptoTool.Algorithm.Interfaces;
using CryptoTool.Algorithm.Utils;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Modes;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Parameters;
using System;
using System.Security.Cryptography;

namespace CryptoTool.Algorithm.Algorithms.SM4
{
    /// <summary>
    /// SM4国密对称加密算法实现
    /// </summary>
    public class Sm4Crypto : ISymmetricCrypto
    {
        public string AlgorithmName => "SM4";
        public CryptoAlgorithmType AlgorithmType => CryptoAlgorithmType.Symmetric;

        /// <summary>
        /// 密钥长度（字节）
        /// </summary>
        private const int SM4_KEY_SIZE_BYTES = 16; // 128位

        /// <summary>
        /// 块大小（字节）
        /// </summary>
        private const int SM4_BLOCK_SIZE_BYTES = 16; // 128位

        /// <summary>
        /// IV长度（字节）
        /// </summary>
        private const int SM4_IV_SIZE_BYTES = 16; // 128位

        private readonly SymmetricCipherMode _mode;
        private readonly SymmetricPaddingMode _padding;

        /// <summary>
        /// 初始化SM4加密算法
        /// </summary>
        /// <param name="mode">加密模式，默认CBC</param>
        /// <param name="padding">填充模式，默认PKCS7</param>
        public Sm4Crypto(SymmetricCipherMode mode = SymmetricCipherMode.CBC, SymmetricPaddingMode padding = SymmetricPaddingMode.PKCS7)
        {
            _mode = mode;
            _padding = padding;
        }


        /// <summary>
        /// 加密
        /// </summary>
        public byte[] Encrypt(byte[] data, byte[] key, byte[]? iv = null)
        {
            if (data == null || data.Length == 0)
                throw new Exceptions.DataException("待加密数据不能为空");

            if (key == null || key.Length == 0)
                throw new Exceptions.KeyException("密钥不能为空");

            if (key.Length != 16)
                throw new Exceptions.KeyException("SM4密钥长度必须为16字节");

            try
            {
                bool ivWasGenerated = false;

                // 生成IV（如果需要且未提供）
                if (iv == null && RequiresIV())
                {
                    iv = StringUtil.GenerateRandomBytes(SM4_IV_SIZE_BYTES);
                    ivWasGenerated = true;
                }

                // 创建SM4引擎
                var sm4Engine = new SM4Engine();

                // 创建密码器
                var cipher = CreateCipher(sm4Engine, true, key, iv);

                // 执行加密
                var encrypted = new byte[cipher.GetOutputSize(data.Length)];
                var length = cipher.ProcessBytes(data, 0, data.Length, encrypted, 0);
                length += cipher.DoFinal(encrypted, length);

                // 调整数组大小
                if (length < encrypted.Length)
                {
                    var result = new byte[length];
                    Array.Copy(encrypted, 0, result, 0, length);
                    encrypted = result;
                }

                // 如果IV是自动生成的，需要将IV和加密数据一起返回
                if (ivWasGenerated && RequiresIV() && iv != null)
                {
                    var result = new byte[iv.Length + encrypted.Length];
                    Array.Copy(iv, 0, result, 0, iv.Length);
                    Array.Copy(encrypted, 0, result, iv.Length, encrypted.Length);
                    return result;
                }

                return encrypted;
            }
            catch (Exception ex)
            {
                throw new Exceptions.CryptoException("SM4加密失败", ex);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[]? iv = null)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                throw new Exceptions.DataException("待解密数据不能为空");

            if (key == null || key.Length == 0)
                throw new Exceptions.KeyException("密钥不能为空");

            if (key.Length != 16)
                throw new Exceptions.KeyException("SM4密钥长度必须为16字节");

            try
            {
                // 如果IV为null，说明IV包含在加密数据的前面（自动生成的IV）
                if (iv == null && RequiresIV())
                {
                    if (encryptedData.Length < SM4_IV_SIZE_BYTES)
                        throw new Exceptions.DataException("加密数据长度不足，无法提取IV");

                    var extractedIV = new byte[16];
                    Array.Copy(encryptedData, 0, extractedIV, 0, 16);
                    iv = extractedIV;

                    var actualEncryptedData = new byte[encryptedData.Length - 16];
                    Array.Copy(encryptedData, 16, actualEncryptedData, 0, actualEncryptedData.Length);
                    encryptedData = actualEncryptedData;
                }

                // 创建SM4引擎
                var sm4Engine = new SM4Engine();

                // 创建密码器
                var cipher = CreateCipher(sm4Engine, false, key, iv);

                // 执行解密
                var decrypted = new byte[cipher.GetOutputSize(encryptedData.Length)];
                var length = cipher.ProcessBytes(encryptedData, 0, encryptedData.Length, decrypted, 0);
                length += cipher.DoFinal(decrypted, length);

                // 调整数组大小
                if (length < decrypted.Length)
                {
                    var result = new byte[length];
                    Array.Copy(decrypted, 0, result, 0, length);
                    decrypted = result;
                }

                return decrypted;
            }
            catch (Exception ex)
            {
                throw new Exceptions.CryptoException("SM4解密失败", ex);
            }
        }

        /// <summary>
        /// 生成随机密钥
        /// </summary>
        /// <returns>随机密钥</returns>
        public byte[] GenerateKey()
        {
            return StringUtil.GenerateRandomKey(SM4_KEY_SIZE_BYTES * 8); // SM4密钥长度为128位
        }

        /// <summary>
        /// 生成随机IV
        /// </summary>
        /// <returns>随机IV</returns>
        public byte[] GenerateIV()
        {
            return StringUtil.GenerateRandomIV(SM4_KEY_SIZE_BYTES * 8); // SM4向量长度为128位
        }

        /// <summary>
        /// 从密码生成密钥（使用PBKDF2）
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="salt">盐值</param>
        /// <param name="iterations">迭代次数</param>
        /// <returns>生成的密钥</returns>
        public byte[] DeriveKeyFromPassword(string password, byte[] salt, int iterations = 10000)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            if (salt == null || salt.Length == 0)
                throw new ArgumentException("盐值不能为空", nameof(salt));

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            return pbkdf2.GetBytes(SM4_KEY_SIZE_BYTES); // SM4密钥为16字节
        }

        /// <summary>
        /// 从密码生成密钥（使用PBKDF2）并返回盐值
        /// </summary>
        /// <param name="password">密码</param>
        /// <param name="iterations">迭代次数</param>
        /// <returns>生成的密钥和盐值</returns>
        public (byte[] Key, byte[] Salt) DeriveKeyFromPassword(string password, int iterations = 10000)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("密码不能为空", nameof(password));

            var salt = StringUtil.GenerateRandomBytes(16); // 128位盐值
            var key = DeriveKeyFromPassword(password, salt, iterations);
            return (key, salt);
        }

        /// <summary>
        /// 创建填充器
        /// </summary>
        private IBlockCipherPadding CreatePadding()
        {
            return _padding switch
            {
                SymmetricPaddingMode.PKCS5 => new Pkcs7Padding(),  // PKCS5等同于PKCS7
                SymmetricPaddingMode.PKCS7 => new Pkcs7Padding(),
                SymmetricPaddingMode.Zeros => new ZeroBytePadding(),
                _ => new Pkcs7Padding()
            };
        }

        /// <summary>
        /// 创建密码器
        /// </summary>
        private IBufferedCipher CreateCipher(IBlockCipher engine, bool forEncryption, byte[] key, byte[]? iv)
        {
            IBufferedCipher cipher;

            // 根据模式创建密码器
            switch (_mode)
            {
                case SymmetricCipherMode.CBC:
                    cipher = _padding == SymmetricPaddingMode.None
                        ? new BufferedBlockCipher(new CbcBlockCipher(engine))
                        : new PaddedBufferedBlockCipher(new CbcBlockCipher(engine), CreatePadding());
                    break;
                case SymmetricCipherMode.ECB:
                    cipher = _padding == SymmetricPaddingMode.None
                        ? new BufferedBlockCipher(new EcbBlockCipher(engine))
                        : new PaddedBufferedBlockCipher(new EcbBlockCipher(engine), CreatePadding());
                    break;
                case SymmetricCipherMode.CFB:
                    cipher = new BufferedBlockCipher(new CfbBlockCipher(engine, SM4_BLOCK_SIZE_BYTES * 8)); // 128位
                    break;
                case SymmetricCipherMode.OFB:
                    cipher = new BufferedBlockCipher(new OfbBlockCipher(engine, SM4_BLOCK_SIZE_BYTES * 8)); // 128位
                    break;
                case SymmetricCipherMode.CTR:
                    cipher = new BufferedBlockCipher(new SicBlockCipher(engine));
                    break;
                default:
                    throw new ArgumentException($"不支持的加密模式: {_mode}");
            }

            // 创建密钥参数
            KeyParameter keyParam = new KeyParameter(key);

            if (RequiresIV() && iv != null)
            {
                if (iv.Length != SM4_IV_SIZE_BYTES)
                    throw new Exceptions.KeyException("SM4 IV长度必须为16字节");

                var parameters = new ParametersWithIV(keyParam, iv);
                cipher.Init(forEncryption, parameters);
            }
            else if (RequiresIV() && iv == null)
            {
                // 生成随机IV
                var randomIV = StringUtil.GenerateRandomBytes(SM4_IV_SIZE_BYTES);
                var parameters = new ParametersWithIV(keyParam, randomIV);
                cipher.Init(forEncryption, parameters);
            }
            else
            {
                cipher.Init(forEncryption, keyParam);
            }

            return cipher;
        }

        /// <summary>
        /// 判断当前模式是否需要IV
        /// </summary>
        private bool RequiresIV()
        {
            return _mode != SymmetricCipherMode.ECB;
        }
    }
}
