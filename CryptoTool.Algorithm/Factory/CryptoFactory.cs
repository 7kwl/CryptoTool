using CryptoTool.Algorithm.Algorithms.AES;
using CryptoTool.Algorithm.Algorithms.DES;
using CryptoTool.Algorithm.Algorithms.MD5;
using CryptoTool.Algorithm.Algorithms.RSA;
using CryptoTool.Algorithm.Algorithms.SM2;
using CryptoTool.Algorithm.Algorithms.SM3;
using CryptoTool.Algorithm.Algorithms.SM4;
using CryptoTool.Algorithm.Enums;
using CryptoTool.Algorithm.Exceptions;
using CryptoTool.Algorithm.Interfaces;
using System;
using System.Collections.Generic;

namespace CryptoTool.Algorithm.Factory
{
    /// <summary>
    /// 加密算法工厂类
    /// </summary>
    public static class CryptoFactory
    {
        private static readonly Dictionary<string, Type> _algorithmTypes = new Dictionary<string, Type>
        {
            { "RSA", typeof(RsaCrypto) },
            { "AES", typeof(AesCrypto) },
            { "DES", typeof(DesCrypto) },
            { "MD5", typeof(Md5Hash) },
            { "SM2", typeof(Sm2Crypto) },
            { "SM3", typeof(Sm3Hash) },
            { "SM4", typeof(Sm4Crypto) }
        };

        /// <summary>
        /// 创建加密算法实例
        /// </summary>
        /// <param name="algorithmName">算法名称</param>
        /// <param name="parameters">算法参数</param>
        /// <returns>加密算法实例</returns>
        public static ICryptoAlgorithm CreateAlgorithm(string algorithmName, params object[] parameters)
        {
            if (string.IsNullOrEmpty(algorithmName))
                throw new ArgumentException("算法名称不能为空", nameof(algorithmName));

            var normalizedName = algorithmName.ToUpper();
            if (!_algorithmTypes.TryGetValue(normalizedName, out var algorithmType))
                throw new AlgorithmNotSupportedException($"不支持的算法: {algorithmName}");

            try
            {
                if (parameters == null || parameters.Length == 0)
                {
                    return normalizedName switch
                    {
                        "RSA" => new RsaCrypto(),
                        "AES" => new AesCrypto(),
                        "DES" => new DesCrypto(),
                        "MD5" => new Md5Hash(),
                        "SM2" => new Sm2Crypto(),
                        "SM3" => new Sm3Hash(),
                        "SM4" => new Sm4Crypto(),
                        _ => throw new AlgorithmNotSupportedException($"不支持的算法: {algorithmName}")
                    };
                }

                return (ICryptoAlgorithm)Activator.CreateInstance(algorithmType, parameters);
            }
            catch (Exception ex)
            {
                throw new CryptoException($"创建算法实例失败: {algorithmName}", ex);
            }
        }

        /// <summary>
        /// 创建RSA算法实例
        /// </summary>
        /// <param name="keySize">密钥长度</param>
        /// <returns>RSA算法实例</returns>
        public static RsaCrypto CreateRsa(int keySize = 2048)
        {
            return new RsaCrypto(keySize);
        }

        /// <summary>
        /// 创建AES算法实例
        /// </summary>
        /// <param name="keySize">密钥长度</param>
        /// <param name="mode">加密模式</param>
        /// <param name="padding">填充模式</param>
        /// <returns>AES算法实例</returns>
        public static AesCrypto CreateAes(int keySize = 256, System.Security.Cryptography.CipherMode mode = System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode padding = System.Security.Cryptography.PaddingMode.PKCS7)
        {
            return new AesCrypto(keySize, mode, padding);
        }

        /// <summary>
        /// 创建DES算法实例
        /// </summary>
        /// <param name="mode">加密模式</param>
        /// <param name="padding">填充模式</param>
        /// <returns>DES算法实例</returns>
        public static DesCrypto CreateDes(System.Security.Cryptography.CipherMode mode = System.Security.Cryptography.CipherMode.CBC, System.Security.Cryptography.PaddingMode padding = System.Security.Cryptography.PaddingMode.PKCS7)
        {
            return new DesCrypto(mode, padding);
        }

        /// <summary>
        /// 创建MD5算法实例
        /// </summary>
        /// <returns>MD5算法实例</returns>
        public static Md5Hash CreateMd5()
        {
            return new Md5Hash();
        }

        /// <summary>
        /// 创建SM2算法实例
        /// </summary>
        /// <returns>SM2算法实例</returns>
        public static Sm2Crypto CreateSm2()
        {
            return new Sm2Crypto();
        }

        /// <summary>
        /// 创建SM3算法实例
        /// </summary>
        /// <returns>SM3算法实例</returns>
        public static Sm3Hash CreateSm3()
        {
            return new Sm3Hash();
        }

        /// <summary>
        /// 创建SM4算法实例
        /// </summary>
        /// <param name="mode">加密模式</param>
        /// <param name="padding">填充模式</param>
        /// <returns>SM4算法实例</returns>
        public static Sm4Crypto CreateSm4(SymmetricCipherMode mode = SymmetricCipherMode.CBC, SymmetricPaddingMode padding = SymmetricPaddingMode.PKCS7)
        {
            return new Sm4Crypto(mode, padding);
        }

        /// <summary>
        /// 获取支持的算法列表
        /// </summary>
        /// <returns>支持的算法名称列表</returns>
        public static IEnumerable<string> GetSupportedAlgorithms()
        {
            return _algorithmTypes.Keys;
        }

        /// <summary>
        /// 检查是否支持指定算法
        /// </summary>
        /// <param name="algorithmName">算法名称</param>
        /// <returns>是否支持</returns>
        public static bool IsSupported(string algorithmName)
        {
            return !string.IsNullOrEmpty(algorithmName) && _algorithmTypes.ContainsKey(algorithmName.ToUpper());
        }

        /// <summary>
        /// 获取算法类型
        /// </summary>
        /// <param name="algorithmName">算法名称</param>
        /// <returns>算法类型</returns>
        public static CryptoAlgorithmType? GetAlgorithmType(string algorithmName)
        {
            if (!IsSupported(algorithmName))
                return null;

            try
            {
                var algorithm = CreateAlgorithm(algorithmName);
                return algorithm.AlgorithmType;
            }
            catch
            {
                return null;
            }
        }
    }
}
