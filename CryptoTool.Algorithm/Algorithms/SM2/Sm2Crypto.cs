using CryptoTool.Algorithm.Enums;
using CryptoTool.Algorithm.Exceptions;
using CryptoTool.Algorithm.Interfaces;
using CryptoTool.Algorithm.Utils;
using Org.BouncyCastle.Asn1.X9;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using System;

namespace CryptoTool.Algorithm.Algorithms.SM2
{
    /// <summary>
    /// SM2国密算法实现 - 优化版
    /// </summary>
    public class Sm2Crypto : IAsymmetricCrypto
    {
        public string AlgorithmName => "SM2";
        public CryptoAlgorithmType AlgorithmType => CryptoAlgorithmType.Asymmetric;

        private const string SM2_CURVE_NAME = "SM2P256v1";
        private const int SM2_SIGNATURE_LENGTH = 64;
        private const int SM2_COMPONENT_LENGTH = 32;

        private static readonly X9ECParameters _sm2Parameters = ECNamedCurveTable.GetByName(SM2_CURVE_NAME);
        private static readonly ECDomainParameters _domainParameters = new ECDomainParameters(
            _sm2Parameters.Curve, _sm2Parameters.G, _sm2Parameters.N, _sm2Parameters.H, _sm2Parameters.GetSeed());

        /// <summary>
        /// 加密 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="publicKey"></param>
        /// <returns></returns>
        /// <exception cref="CryptoException"></exception>
        public byte[] Encrypt(byte[] data, byte[] publicKey)
        {
            return Encrypt(data, publicKey, SM2CipherFormat.C1C2C3);
        }

        /// <summary>
        /// 使用指定密文格式加密
        /// </summary>
        public byte[] Encrypt(byte[] data, byte[] publicKey, SM2CipherFormat cipherFormat)
        {
            ValidateEncryptInput(data, publicKey);

            try
            {
                var ecPublicKey = ParsePublicKey(publicKey);
                var sm2Engine = new SM2Engine(ToEngineMode(cipherFormat));
                sm2Engine.Init(true, new ParametersWithRandom(ecPublicKey, new SecureRandom()));

                return sm2Engine.ProcessBlock(data, 0, data.Length);
            }
            catch (Exception ex) when (!(ex is CryptoException))
            {
                throw new CryptoException("SM2加密失败", ex);
            }
        }

        /// <summary>
        /// 解密
        /// </summary>
        /// <param name="encryptedData"></param>
        /// <param name="privateKey"></param>
        /// <returns></returns>
        /// <exception cref="CryptoException"></exception>
        public byte[] Decrypt(byte[] encryptedData, byte[] privateKey)
        {
            return Decrypt(encryptedData, privateKey, SM2CipherFormat.C1C2C3);
        }

        /// <summary>
        /// 使用指定密文格式解密
        /// </summary>
        public byte[] Decrypt(byte[] encryptedData, byte[] privateKey, SM2CipherFormat cipherFormat)
        {
            ValidateDecryptInput(encryptedData, privateKey);

            try
            {
                var ecPrivateKey = ParsePrivateKey(privateKey);
                var sm2Engine = new SM2Engine(ToEngineMode(cipherFormat));
                sm2Engine.Init(false, ecPrivateKey);

                return sm2Engine.ProcessBlock(encryptedData, 0, encryptedData.Length);
            }
            catch (Exception ex) when (!(ex is CryptoException))
            {
                throw new CryptoException("SM2解密失败", ex);
            }
        }

        /// <summary>
        /// 生成密钥对
        /// </summary>
        public (byte[] PublicKey, byte[] PrivateKey) GenerateKeyPair()
        {
            try
            {
                var keyPairGenerator = new ECKeyPairGenerator();
                var keyGenParams = new ECKeyGenerationParameters(_domainParameters, new SecureRandom());
                keyPairGenerator.Init(keyGenParams);

                var keyPair = keyPairGenerator.GenerateKeyPair();
                var privateKey = (ECPrivateKeyParameters)keyPair.Private;
                var publicKey = (ECPublicKeyParameters)keyPair.Public;

                var privateKeyBytes = privateKey.D.ToByteArrayUnsigned();
                var publicKeyBytes = EncodePublicKey(publicKey);

                return (publicKeyBytes, privateKeyBytes);
            }
            catch (Exception ex)
            {
                throw new CryptoException("SM2密钥对生成失败", ex);
            }
        }

        /// <summary>
        /// 签名 - 简化版
        /// </summary>
        public byte[] Sign(byte[] data, byte[] privateKey)
        {
            ValidateSignInput(data, privateKey);

            try
            {
                var ecPrivateKey = ParsePrivateKey(privateKey);
                var signer = new SM2Signer();
                signer.Init(true, new ParametersWithRandom(ecPrivateKey, new SecureRandom()));

                signer.BlockUpdate(data, 0, data.Length);
                var signature = signer.GenerateSignature();

                return ConvertSignatureToStandardFormat(signature);
            }
            catch (Exception ex) when (!(ex is CryptoException))
            {
                throw new CryptoException("SM2签名失败", ex);
            }
        }

        /// <summary>
        /// 验证签名
        /// </summary>
        public bool VerifySign(byte[] data, byte[] signature, byte[] publicKey)
        {
            ValidateVerifyInput(data, signature, publicKey);

            try
            {
                var ecPublicKey = ParsePublicKey(publicKey);

                if (signature.Length != SM2_SIGNATURE_LENGTH)
                    return false;

                var derSignature = ConvertRsToDer(signature);
                var signer = new SM2Signer();
                signer.Init(false, ecPublicKey);

                signer.BlockUpdate(data, 0, data.Length);
                return signer.VerifySignature(derSignature);
            }
            catch (Exception ex) when (!(ex is CryptoException))
            {
                throw new CryptoException("SM2签名验证失败", ex);
            }
        }


        #region 密文格式转换功能 - 保持不变

        public byte[] C1C2C3ToC1C3C2(byte[] c1c2c3Data)
        {
            return Sm2CipherFormatConverter.C1C2C3ToC1C3C2(c1c2c3Data);
        }

        public byte[] C1C3C2ToC1C2C3(byte[] c1c3c2Data)
        {
            return Sm2CipherFormatConverter.C1C3C2ToC1C2C3(c1c3c2Data);
        }

        public SM2CipherFormat DetectCipherFormat(byte[] cipherData)
        {
            return Sm2CipherFormatConverter.DetectFormat(cipherData);
        }

        public bool ValidateCipherData(byte[] cipherData, SM2CipherFormat expectedFormat)
        {
            return Sm2CipherFormatConverter.ValidateCipherData(cipherData, expectedFormat);
        }

        public SM2CipherComponentInfo GetCipherComponentInfo(byte[] cipherData)
        {
            return Sm2CipherFormatConverter.GetComponentInfo(cipherData);
        }

        #endregion

        #region 扩展填充模式和签名算法支持

        /// <summary>
        /// 使用指定填充模式加密 - SM2默认不支持多种填充模式，但保持接口一致性
        /// </summary>
        public byte[] Encrypt(byte[] data, byte[] publicKey, AsymmetricPaddingMode paddingMode)
        {
            // SM2算法本身不支持多种填充模式，但为了接口一致性，我们忽略填充模式参数
            if (paddingMode != AsymmetricPaddingMode.PKCS1)
            {
                throw new NotSupportedException($"SM2算法不支持填充模式: {paddingMode}，SM2使用固定的填充方式");
            }

            return Encrypt(data, publicKey);
        }

        /// <summary>
        /// 使用指定填充模式解密 - SM2默认不支持多种填充模式，但保持接口一致性
        /// </summary>
        public byte[] Decrypt(byte[] encryptedData, byte[] privateKey, AsymmetricPaddingMode paddingMode)
        {
            // SM2算法本身不支持多种填充模式，但为了接口一致性，我们忽略填充模式参数
            if (paddingMode != AsymmetricPaddingMode.PKCS1)
            {
                throw new NotSupportedException($"SM2算法不支持填充模式: {paddingMode}，SM2使用固定的填充方式");
            }

            return Decrypt(encryptedData, privateKey);
        }

        /// <summary>
        /// 使用指定签名算法签名
        /// </summary>
        public byte[] Sign(byte[] data, byte[] privateKey, SignatureAlgorithm signatureAlgorithm)
        {
            ValidateSignInput(data, privateKey);

            if (!CryptoPaddingUtil.IsSM2Compatible(signatureAlgorithm))
            {
                throw new CryptoException($"SM2不支持签名算法: {signatureAlgorithm}，SM2只支持SM3withSM2");
            }

            // SM2目前只支持SM3withSM2，直接调用原有方法
            return Sign(data, privateKey);
        }

        /// <summary>
        /// 使用指定签名算法验证签名
        /// </summary>
        public bool VerifySign(byte[] data, byte[] signature, byte[] publicKey, SignatureAlgorithm signatureAlgorithm)
        {
            ValidateVerifyInput(data, signature, publicKey);

            if (!CryptoPaddingUtil.IsSM2Compatible(signatureAlgorithm))
            {
                throw new CryptoException($"SM2不支持签名算法: {signatureAlgorithm}，SM2只支持SM3withSM2");
            }

            // SM2目前只支持SM3withSM2，直接调用原有方法
            return VerifySign(data, signature, publicKey);
        }

        /// <summary>
        /// 获取SM2支持的签名算法列表
        /// </summary>
        /// <returns>支持的签名算法列表</returns>
        public SignatureAlgorithm[] GetSupportedSignatureAlgorithms()
        {
            return new[] { SignatureAlgorithm.SM3withSM2 };
        }

        /// <summary>
        /// 获取SM2支持的填充模式列表
        /// </summary>
        /// <returns>支持的填充模式列表</returns>
        public AsymmetricPaddingMode[] GetSupportedPaddingModes()
        {
            return new[] { AsymmetricPaddingMode.PKCS1 };
        }

        #endregion

        #region 扩展功能

        /// <summary>
        /// 从私钥获取公钥
        /// </summary>
        public byte[] GetPublicKeyFromPrivateKey(byte[] privateKey)
        {
            if (privateKey == null || privateKey.Length == 0)
                throw new Exceptions.KeyException("私钥不能为空");

            try
            {
                var ecPrivateKey = ParsePrivateKey(privateKey);
                var publicPoint = _domainParameters.G.Multiply(ecPrivateKey.D);
                var publicKey = new ECPublicKeyParameters(publicPoint, _domainParameters);

                return EncodePublicKey(publicKey);
            }
            catch (Exception ex) when (!(ex is CryptoException))
            {
                throw new CryptoException("从私钥获取公钥失败", ex);
            }
        }

        /// <summary>
        /// 验证密钥格式
        /// </summary>
        public bool ValidateKey(byte[] key, bool isPrivateKey)
        {
            if (key == null || key.Length == 0)
                return false;

            try
            {
                if (isPrivateKey)
                {
                    var privateKey = ParsePrivateKey(key);
                    return privateKey != null &&
                           privateKey.D.CompareTo(BigInteger.Zero) > 0 &&
                           privateKey.D.CompareTo(_domainParameters.N) < 0;
                }
                else
                {
                    var publicKey = ParsePublicKey(key);
                    return publicKey != null && publicKey.Q.IsValid();
                }
            }
            catch
            {
                return false;
            }
        }

        #endregion

        #region 私有辅助方法

        private static void ValidateEncryptInput(byte[] data, byte[] publicKey)
        {
            if (data == null || data.Length == 0)
                throw new DataException("待加密数据不能为空");
            if (publicKey == null || publicKey.Length == 0)
                throw new Exceptions.KeyException("公钥不能为空");
        }

        private static void ValidateDecryptInput(byte[] encryptedData, byte[] privateKey)
        {
            if (encryptedData == null || encryptedData.Length == 0)
                throw new DataException("待解密数据不能为空");
            if (privateKey == null || privateKey.Length == 0)
                throw new Exceptions.KeyException("私钥不能为空");
        }

        private static void ValidateSignInput(byte[] data, byte[] privateKey)
        {
            if (data == null || data.Length == 0)
                throw new DataException("待签名数据不能为空");
            if (privateKey == null || privateKey.Length == 0)
                throw new Exceptions.KeyException("私钥不能为空");
        }

        private static void ValidateVerifyInput(byte[] data, byte[] signature, byte[] publicKey)
        {
            if (data == null || data.Length == 0)
                throw new DataException("原始数据不能为空");
            if (signature == null || signature.Length == 0)
                throw new DataException("签名数据不能为空");
            if (publicKey == null || publicKey.Length == 0)
                throw new Exceptions.KeyException("公钥不能为空");
        }

        /// <summary>
        /// 简化的签名格式转换
        /// </summary>
        private byte[] ConvertSignatureToStandardFormat(byte[] signature)
        {
            if (signature.Length == SM2_SIGNATURE_LENGTH)
            {
                return signature;
            }

            try
            {
                // 尝试解析DER格式
                var derSequence = Org.BouncyCastle.Asn1.Asn1Object.FromByteArray(signature)
                    as Org.BouncyCastle.Asn1.DerSequence;

                if (derSequence?.Count == 2)
                {
                    var rDer = derSequence[0] as Org.BouncyCastle.Asn1.DerInteger;
                    var sDer = derSequence[1] as Org.BouncyCastle.Asn1.DerInteger;

                    if (rDer != null && sDer != null)
                    {
                        return CreateStandardSignature(rDer.Value, sDer.Value);
                    }
                }
            }
            catch
            {
                // 如果DER解析失败，抛出异常
            }

            throw new CryptoException($"不支持的签名格式，长度: {signature.Length}字节");
        }

        /// <summary>
        /// 创建标准64字节签名格式
        /// </summary>
        private byte[] CreateStandardSignature(BigInteger r, BigInteger s)
        {
            var rBytes = PadToLength(r.ToByteArrayUnsigned(), SM2_COMPONENT_LENGTH);
            var sBytes = PadToLength(s.ToByteArrayUnsigned(), SM2_COMPONENT_LENGTH);

            var result = new byte[SM2_SIGNATURE_LENGTH];
            Buffer.BlockCopy(rBytes, 0, result, 0, SM2_COMPONENT_LENGTH);
            Buffer.BlockCopy(sBytes, 0, result, SM2_COMPONENT_LENGTH, SM2_COMPONENT_LENGTH);

            return result;
        }

        /// <summary>
        /// 填充字节数组到指定长度
        /// </summary>
        private byte[] PadToLength(byte[] input, int targetLength)
        {
            if (input.Length == targetLength)
                return input;

            var result = new byte[targetLength];
            if (input.Length <= targetLength)
            {
                Buffer.BlockCopy(input, 0, result, targetLength - input.Length, input.Length);
            }
            else
            {
                Buffer.BlockCopy(input, input.Length - targetLength, result, 0, targetLength);
            }

            return result;
        }

        private ECPublicKeyParameters ParsePublicKey(byte[] publicKeyBytes)
        {
            try
            {
                if (publicKeyBytes.Length == 33 || publicKeyBytes.Length == 65)
                {
                    var point = _domainParameters.Curve.DecodePoint(publicKeyBytes);
                    return new ECPublicKeyParameters(point, _domainParameters);
                }

                throw new Exceptions.KeyException($"不支持的公钥长度: {publicKeyBytes.Length}");
            }
            catch (Exception ex) when (!(ex is Exceptions.KeyException))
            {
                throw new Exceptions.KeyException("公钥解析失败", ex);
            }
        }

        private ECPrivateKeyParameters ParsePrivateKey(byte[] privateKeyBytes)
        {
            try
            {
                var d = new BigInteger(1, privateKeyBytes);
                return new ECPrivateKeyParameters(d, _domainParameters);
            }
            catch (Exception ex)
            {
                throw new Exceptions.KeyException("私钥解析失败", ex);
            }
        }

        private byte[] EncodePublicKey(ECPublicKeyParameters publicKey)
        {
            try
            {
                return publicKey.Q.GetEncoded(false);
            }
            catch (Exception ex)
            {
                throw new Exceptions.KeyException("公钥编码失败", ex);
            }
        }

        private byte[] ConvertRsToDer(byte[] rsSignature)
        {
            if (rsSignature?.Length != SM2_SIGNATURE_LENGTH)
                throw new ArgumentException("签名必须是64字节的r||s格式");

            try
            {
                var rBytes = new byte[SM2_COMPONENT_LENGTH];
                var sBytes = new byte[SM2_COMPONENT_LENGTH];
                Buffer.BlockCopy(rsSignature, 0, rBytes, 0, SM2_COMPONENT_LENGTH);
                Buffer.BlockCopy(rsSignature, SM2_COMPONENT_LENGTH, sBytes, 0, SM2_COMPONENT_LENGTH);

                var r = new BigInteger(1, rBytes);
                var s = new BigInteger(1, sBytes);

                var rDer = new Org.BouncyCastle.Asn1.DerInteger(r);
                var sDer = new Org.BouncyCastle.Asn1.DerInteger(s);
                var sequence = new Org.BouncyCastle.Asn1.DerSequence(rDer, sDer);

                return sequence.GetEncoded();
            }
            catch (Exception ex)
            {
                throw new CryptoException("签名格式转换失败", ex);
            }
        }

        private static SM2Engine.Mode ToEngineMode(SM2CipherFormat cipherFormat)
        {
            return cipherFormat switch
            {
                SM2CipherFormat.C1C2C3 => SM2Engine.Mode.C1C2C3,
                SM2CipherFormat.C1C3C2 => SM2Engine.Mode.C1C3C2,
                _ => throw new CryptoException($"不支持的SM2密文格式: {cipherFormat}")
            };
        }

        #endregion
    }
}
