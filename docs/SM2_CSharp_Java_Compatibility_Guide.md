# SM2 C#与Java互操作性指南

## 概述

本文档说明了如何在C#和Java之间实现SM2加密算法的完全兼容，特别是解决密文格式兼容性和签名格式转换的关键问题。

## 当前桌面版行为

当前 `CryptoTool.Win` 中的 SM2 页已与算法层对齐，界面行为如下：

- 密文格式下拉框只保留 `C1C3C2` 和 `C1C2C3`
- 加密时会按当前下拉框显式生成对应格式的密文
- 解密时需要用户选择与密文一致的格式
- 签名格式支持 `ASN1` 与 `RS` 两种输出
- 验签时会按当前签名格式选择解析签名内容
- SM2 密钥导出文件可直接再次导入

如果你是在桌面工具里和 Java 端联调，建议先统一两件事：

1. 密文使用 `C1C3C2` 还是 `C1C2C3`
2. 签名使用 `ASN1 DER` 还是 `RS(r||s)`

## 核心兼容性问题

### 1. 密文格式差异 (0x04前缀问题)

这是C#和Java BouncyCastle库之间最重要的兼容性问题：

**问题描述：**
- **.NET BouncyCastle**: 生成的密文C1部分包含0x04前缀（65字节未压缩点）
- **Java BouncyCastle**: 生成的密文C1部分不包含0x04前缀（64字节坐标）

**密文结构对比：**
```
.NET密文格式：  0x04 + X(32字节) + Y(32字节) + C2(变长) + C3(32字节)
Java密文格式：       X(32字节) + Y(32字节) + C2(变长) + C3(32字节)
```

### 2. 解决方案

#### C#端解决方案

```csharp
// 生成Java兼容密文
string javaCompatibleCiphertext = SM2Util.EncryptForJava(plainText, publicKey);

// 解密Java密文
string decryptedText = SM2Util.DecryptFromJavaToString(javaCiphertext, privateKey);

// 智能解密（自动检测格式）
string smartDecryptText = SM2Util.SmartDecryptToString(ciphertext, privateKey);

// 格式检测
bool isJavaFormat = SM2Util.IsJavaFormat(ciphertextBytes, SM2CipherFormat.C1C3C2);
```

#### Java端解决方案

```java
// 生成C#兼容密文
String dotNetCompatibleCiphertext = encryptForDotNet(plainText, publicKey);

// 解密C#密文
String decryptedText = decryptFromDotNet(csharpCiphertext, privateKey);

// 智能解密（自动检测格式）
String smartDecryptText = smartDecrypt(ciphertext, privateKey);

// 格式检测
boolean isDotNetFormat = isDotNetFormat(ciphertextBytes);
```

## 签名格式说明

### 1. ASN.1 DER格式
- **用途**: BouncyCastle默认格式，标准的数字签名格式
- **结构**: `SEQUENCE { r INTEGER, s INTEGER }`
- **特点**: 变长编码，包含类型和长度信息
- **兼容性**: 两端完全兼容

### 2. RS格式  
- **用途**: 固定长度格式，便于存储和传输
- **结构**: `r || s` (r和s各32字节)
- **特点**: 固定64字节长度，无额外编码信息
- **兼容性**: 需要正确处理BigInteger的符号位

## 完整兼容性实现

### C#端完整示例

```csharp
public static void TestJavaCompatibility()
{
    // 1. 生成密钥对
    var keyPair = SM2Util.GenerateKeyPair();
    var publicKey = (ECPublicKeyParameters)keyPair.Public;
    var privateKey = (ECPrivateKeyParameters)keyPair.Private;
    
    string plainText = "测试数据";
    
    // 2. 加密兼容性测试
    
    // 标准C#加密
    string csharpCiphertext = SM2Util.Encrypt(plainText, publicKey);
    
    // Java兼容加密（移除0x04前缀）
    string javaCompatibleCiphertext = SM2Util.EncryptForJava(plainText, publicKey);
    
    // 3. 解密兼容性测试
    
    // 解密标准C#密文
    string decrypted1 = SM2Util.DecryptToString(csharpCiphertext, privateKey);
    
    // 解密Java格式密文
    string decrypted2 = SM2Util.DecryptFromJavaToString(javaCompatibleCiphertext, privateKey);
    
    // 智能解密（自动检测格式）
    string decrypted3 = SM2Util.SmartDecryptToString(csharpCiphertext, privateKey);
    string decrypted4 = SM2Util.SmartDecryptToString(javaCompatibleCiphertext, privateKey);
    
    // 4. 签名兼容性测试
    byte[] data = Encoding.UTF8.GetBytes(plainText);
    
    // ASN.1格式签名
    string asn1Signature = SM2Util.SignSm3WithSm2(data, privateKey, SM2SignatureFormat.ASN1);
    
    // RS格式签名
    string rsSignature = SM2Util.SignSm3WithSm2(data, privateKey, SM2SignatureFormat.RS);
    
    // 格式转换
    string convertedRs = SM2Util.ConvertHexAsn1ToHexRs(asn1Signature);
    string convertedAsn1 = SM2Util.ConvertHexRsToHexAsn1(rsSignature);
    
    // 验证签名
    bool valid1 = SM2Util.VerifySm3WithSm2(data, asn1Signature, publicKey, SM2SignatureFormat.ASN1);
    bool valid2 = SM2Util.VerifySm3WithSm2(data, rsSignature, publicKey, SM2SignatureFormat.RS);
}
```

### Java端完整示例

```java
public static void testCSharpCompatibility() throws Exception {
    // 1. 创建密钥对（使用与C#相同的密钥）
    String hexPublicKey = "04..."; // 从C#端获取
    String hexPrivateKey = "...";  // 从C#端获取
    
    ECPublicKeyParameters publicKey = createPublicKeyFromHex(hexPublicKey);
    ECPrivateKeyParameters privateKey = createPrivateKeyFromHex(hexPrivateKey);
    
    String plainText = "测试数据";
    
    // 2. 加密兼容性测试
    
    // Java标准加密
    SM2Engine engine = new SM2Engine();
    engine.init(true, new ParametersWithRandom(publicKey, new SecureRandom()));
    byte[] javaStandardCiphertext = engine.processBlock(plainText.getBytes("UTF-8"), 0, plainText.length());
    
    // C#兼容加密（添加0x04前缀）
    String csharpCompatibleCiphertext = encryptForDotNet(plainText, publicKey);
    
    // 3. 解密兼容性测试
    
    // 解密Java标准密文
    engine.init(false, privateKey);
    byte[] decrypted1 = engine.processBlock(javaStandardCiphertext, 0, javaStandardCiphertext.length);
    
    // 解密C#格式密文
    String decrypted2 = decryptFromDotNet(csharpCompatibleCiphertext, privateKey);
    
    // 智能解密
    String decrypted3 = smartDecrypt(Base64.toBase64String(javaStandardCiphertext), privateKey);
    String decrypted4 = smartDecrypt(csharpCompatibleCiphertext, privateKey);
    
    // 4. 签名兼容性测试
    byte[] data = plainText.getBytes("UTF-8");
    
    // ASN.1格式签名
    String asn1Signature = signSM2Asn1(data, privateKey);
    
    // RS格式签名
    String rsSignature = signSM2Rs(data, privateKey);
    
    // 格式转换
    byte[] convertedRs = convertAsn1ToRs(Hex.decode(asn1Signature));
    byte[] convertedAsn1 = convertRsToAsn1(Hex.decode(rsSignature));
    
    // 验证签名
    boolean valid1 = verifySM2(data, asn1Signature, publicKey, false);
    boolean valid2 = verifySM2(data, rsSignature, publicKey, true);
}
```

## 最佳实践

### 1. 开发建议

1. **统一接口设计**：
   ```csharp
   // C#端：提供Java兼容方法
   string EncryptForJava(string plainText, ECPublicKeyParameters publicKey);
   string DecryptFromJava(string ciphertext, ECPrivateKeyParameters privateKey);
   ```

   ```java
   // Java端：提供C#兼容方法
   String encryptForDotNet(String plainText, ECPublicKeyParameters publicKey);
   String decryptFromDotNet(String ciphertext, ECPrivateKeyParameters privateKey);
   ```

2. **智能处理**：
   ```csharp
   // 自动检测密文格式
   string SmartDecrypt(string ciphertext, ECPrivateKeyParameters privateKey);
   ```

3. **格式验证**：
   ```csharp
   bool IsJavaFormat(byte[] ciphertext, SM2CipherFormat format);
   ```

### 2. 错误处理

```csharp
try
{
    string result = SM2Util.SmartDecryptToString(ciphertext, privateKey);
}
catch (ArgumentException ex)
{
    // 处理格式错误
    Console.WriteLine($"密文格式错误: {ex.Message}");
}
catch (CryptographicException ex)
{
    // 处理解密错误
    Console.WriteLine($"解密失败: {ex.Message}");
}
```

### 3. 性能优化

1. **避免重复转换**：在接口层统一格式，减少运行时转换
2. **缓存密钥对象**：避免重复解析密钥
3. **批量处理**：对于大量数据，考虑批量格式转换

## 测试验证

### 1. 单元测试

```csharp
[Test]
public void TestCrossPlatformCompatibility()
{
    var keyPair = SM2Util.GenerateKeyPair();
    var publicKey = (ECPublicKeyParameters)keyPair.Public;
    var privateKey = (ECPrivateKeyParameters)keyPair.Private;
    
    string plainText = "跨平台兼容性测试";
    
    // C# -> Java 兼容性
    string javaCompatibleCiphertext = SM2Util.EncryptForJava(plainText, publicKey);
    string decrypted1 = SM2Util.DecryptFromJavaToString(javaCompatibleCiphertext, privateKey);
    Assert.AreEqual(plainText, decrypted1);
    
    // 智能解密测试
    string smartDecrypted = SM2Util.SmartDecryptToString(javaCompatibleCiphertext, privateKey);
    Assert.AreEqual(plainText, smartDecrypted);
}
```

### 2. 集成测试

1. **密钥一致性**：确保两端使用相同的密钥
2. **数据一致性**：确保两端使用相同的测试数据
3. **格式验证**：验证转换后的格式正确性
4. **功能验证**：验证加密解密功能正常

## 常见问题

### Q1: 为什么需要处理0x04前缀？
A1: 这是椭圆曲线点的编码标准差异。0x04表示未压缩点格式，不同的BouncyCastle实现对此处理不同。

### Q2: 如何确保跨平台兼容性？
A2: 
- 使用专门的兼容性方法（EncryptForJava/DecryptFromJava）
- 实现智能检测和自动转换
- 进行充分的集成测试

### Q3: 性能影响如何？
A3: 格式转换的性能开销很小（主要是数组复制），但建议在架构设计时统一格式标准。

### Q4: 如何调试兼容性问题？
A4: 
- 检查密文长度差异（相差1字节）
- 验证第一个字节是否为0x04
- 使用十六进制工具比对密文结构

## 版本历史

- v1.0: 初始版本，基本的RS和ASN.1互转
- v1.1: 增加Java兼容性优化
- v1.2: 完善错误处理和验证机制
- v2.0: **添加密文格式兼容性处理（0x04前缀问题）**
- v2.1: 增加智能检测和自动转换功能
