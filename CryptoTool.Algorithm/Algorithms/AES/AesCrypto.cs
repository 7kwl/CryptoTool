using CryptoTool.Algorithm.Exceptions;
using CryptoTool.Algorithm.Interfaces;
using CryptoTool.Algorithm.Utils;
using System;
using System.Security.Cryptography;

namespace CryptoTool.Algorithm.Algorithms.AES
{
    /// <summary>
    /// AES加密算法实现
    /// </summary>
    public class AesCrypto : ISymmetricCrypto
    {
        public string AlgorithmName => "AES";
        public CryptoAlgorithmType AlgorithmType => CryptoAlgorithmType.Symmetric;

        // AES块大小常量（128位 = 16字节）
        private const int AES_BLOCK_SIZE_BYTES = 16;
        
        private readonly int _keySize;
        private readonly CipherMode _mode;
        private readonly PaddingMode _padding;

        /// <summary>
        /// 初始化AES加密算法
        /// </summary>
        /// <param name="keySize">密钥长度，默认256位</param>
        /// <param name="mode">加密模式，默认CBC</param>
        /// <param name="padding">填充模式，默认PKCS7</param>
        public AesCrypto(int keySize = 256, CipherMode mode = CipherMode.CBC, PaddingMode padding = PaddingMode.PKCS7)
        {
            // 验证密钥长度
            if (keySize != 128 && keySize != 192 && keySize != 256)
                throw new ArgumentException("密钥长度必须为128、192或256位", nameof(keySize));

            _keySize = keySize;
            _mode = mode;
            _padding = padding;
        }

        /// <summary>
        /// 加密
        /// </summary>
        public byte[] Encrypt(byte[] data, byte[] key, byte[]? iv = null)
        {
            // 验证输入参数
            if (data == null || data.Length == 0)
                throw new ArgumentNullException(nameof(data), "待加密数据不能为空");
            
            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key), "密钥不能为空");
            
            if (key.Length != _keySize / 8)
                throw new KeyException($"密钥长度必须为{_keySize / 8}字节");

            // 对于非ECB模式，验证IV参数
            if (_mode != CipherMode.ECB)
            {
                if (iv != null && iv.Length != AES_BLOCK_SIZE_BYTES)
                    throw new KeyException($"IV长度必须为{AES_BLOCK_SIZE_BYTES}字节");
            }
            else
            {
                // ECB模式不需要IV
                if (iv != null)
                    throw new ArgumentException("ECB模式不需要IV参数", nameof(iv));
            }

            try
            {
                using var aes = Aes.Create();
                aes.KeySize = _keySize;
                aes.Mode = _mode;
                aes.Padding = _padding;
                aes.Key = key;
                bool shouldEmbedIv = _mode != CipherMode.ECB && iv == null;

                if (iv != null)
                {
                    if (iv.Length != AES_BLOCK_SIZE_BYTES)
                        throw new KeyException($"IV长度必须为{AES_BLOCK_SIZE_BYTES}字节");
                    aes.IV = iv;
                }
                else if (_mode != CipherMode.ECB)
                {
                    aes.GenerateIV();
                }

                using var encryptor = aes.CreateEncryptor();
                var encrypted = encryptor.TransformFinalBlock(data, 0, data.Length);

                // 如果IV是自动生成的，需要将IV和加密数据一起返回
                if (shouldEmbedIv)
                {
                    var result = new byte[aes.IV.Length + encrypted.Length];
                    Array.Copy(aes.IV, 0, result, 0, aes.IV.Length);
                    Array.Copy(encrypted, 0, result, aes.IV.Length, encrypted.Length);
                    return result;
                }

                return encrypted;
            }
            catch (Exception ex)
            {
                throw new CryptoException("AES加密失败", ex);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        public byte[] Decrypt(byte[] encryptedData, byte[] key, byte[]? iv = null)
        {
            // 验证输入参数
            if (encryptedData == null || encryptedData.Length == 0)
                throw new ArgumentNullException(nameof(encryptedData), "待解密数据不能为空");

            if (key == null || key.Length == 0)
                throw new ArgumentNullException(nameof(key), "密钥不能为空");
          
            if (key.Length != _keySize / 8)
                throw new KeyException($"密钥长度必须为{_keySize / 8}字节");

            // 对于非ECB模式，验证IV参数
            if (_mode != CipherMode.ECB)
            {
                if (iv != null && iv.Length != AES_BLOCK_SIZE_BYTES)
                    throw new KeyException($"IV长度必须为{AES_BLOCK_SIZE_BYTES}字节");
            }
            else
            {
                // ECB模式不需要IV
                if (iv != null)
                    throw new ArgumentException("ECB模式不需要IV参数", nameof(iv));
            }

            try
            {
                using var aes = Aes.Create();
                aes.KeySize = _keySize;
                aes.Mode = _mode;
                aes.Padding = _padding;
                aes.Key = key;

                // 如果IV为null，说明IV包含在加密数据的前面
                if (_mode != CipherMode.ECB && iv == null)
                {
                    if (encryptedData.Length < AES_BLOCK_SIZE_BYTES)
                        throw new DataException("加密数据长度不足，无法提取IV");

                    var extractedIV = new byte[AES_BLOCK_SIZE_BYTES];
                    Array.Copy(encryptedData, 0, extractedIV, 0, extractedIV.Length);
                    aes.IV = extractedIV;

                    var actualEncryptedData = new byte[encryptedData.Length - extractedIV.Length];
                    Array.Copy(encryptedData, extractedIV.Length, actualEncryptedData, 0, actualEncryptedData.Length);
                    encryptedData = actualEncryptedData;
                }
                else if (_mode != CipherMode.ECB)
                {
                    if (iv.Length != AES_BLOCK_SIZE_BYTES)
                        throw new KeyException($"IV长度必须为{AES_BLOCK_SIZE_BYTES}字节");
                    aes.IV = iv;
                }

                using var decryptor = aes.CreateDecryptor();
                return decryptor.TransformFinalBlock(encryptedData, 0, encryptedData.Length);
            }
            catch (Exception ex)
            {
                throw new CryptoException("AES解密失败", ex);
            }
        }

        /// <summary>
        /// 生成随机密钥
        /// </summary>
        /// <returns>随机密钥</returns>
        public byte[] GenerateKey()
        {
            return StringUtil.GenerateRandomKey(_keySize);
        }

        /// <summary>
        /// 生成随机IV
        /// </summary>
        /// <returns>随机IV</returns>
        public byte[] GenerateIV()
        {
            return StringUtil.GenerateRandomIV(AES_BLOCK_SIZE_BYTES * 8); // AES块大小为128位
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

            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256))
            {
                return pbkdf2.GetBytes(_keySize / 8);
            }
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

            var salt = StringUtil.GenerateRandomBytes(32); // 256位盐值
            var key = DeriveKeyFromPassword(password, salt, iterations);
            return (key, salt);
        }
    }
}
