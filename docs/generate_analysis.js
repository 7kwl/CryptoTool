const fs = require("fs");
const {
  Document, Packer, Paragraph, TextRun, Table, TableRow, TableCell,
  HeadingLevel, BorderStyle, WidthType, ShadingType,
  AlignmentType, LevelFormat, Header, Footer, PageNumber, PageBreak
} = require("docx");

const border = { style: BorderStyle.SINGLE, size: 1, color: "999999" };
const borders = { top: border, bottom: border, left: border, right: border };
const cellMargins = { top: 60, bottom: 60, left: 100, right: 100 };
const headerFill = { fill: "D5E8F0", type: ShadingType.CLEAR };

function cell(text, opts = {}) {
  const runs = Array.isArray(text) ? text.map(t => new TextRun(t)) : [new TextRun(typeof text === "string" ? text : "")];
  return new TableCell({
    borders,
    width: { size: opts.width || 2340, type: WidthType.DXA },
    shading: opts.shading || undefined,
    margins: cellMargins,
    children: [new Paragraph({ children: runs, alignment: opts.align || undefined })]
  });
}

function headerCell(text, width) {
  return cell(text, { width, shading: headerFill });
}

function heading(level, text) {
  return new Paragraph({
    heading: level,
    spacing: { before: level === HeadingLevel.HEADING_1 ? 360 : 240, after: 120 },
    children: [new TextRun(text)]
  });
}

function para(text, opts = {}) {
  return new Paragraph({
    spacing: { before: 0, after: 80 },
    children: Array.isArray(text) ? text : [new TextRun(typeof text === "string" ? text : "")],
    ...opts
  });
}

function bullet(text) {
  return new Paragraph({
    numbering: { reference: "bullets", level: 0 },
    spacing: { before: 0, after: 40 },
    children: [new TextRun(text)]
  });
}

function code(text) {
  return new Paragraph({
    spacing: { before: 0, after: 80 },
    indent: { left: 360 },
    children: [new TextRun({ text, font: "Consolas", size: 18 })],
    shading: { type: ShadingType.CLEAR, fill: "F5F5F5" }
  });
}

function spacer() {
  return new Paragraph({ spacing: { before: 0, after: 0 }, children: [] });
}

const doc = new Document({
  numbering: {
    config: [
      {
        reference: "bullets",
        levels: [{
          level: 0, format: LevelFormat.BULLET, text: "\u2022", alignment: AlignmentType.LEFT,
          style: { paragraph: { indent: { left: 720, hanging: 360 } } }
        }]
      }
    ]
  },
  styles: {
    default: { document: { run: { font: "Arial", size: 22 } } },
    paragraphStyles: [
      {
        id: "Heading1", name: "Heading 1", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 36, bold: true, font: "Arial", color: "2E75B6" },
        paragraph: { spacing: { before: 480, after: 200 }, outlineLevel: 0 }
      },
      {
        id: "Heading2", name: "Heading 2", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 30, bold: true, font: "Arial", color: "2E75B6" },
        paragraph: { spacing: { before: 360, after: 160 }, outlineLevel: 1 }
      },
      {
        id: "Heading3", name: "Heading 3", basedOn: "Normal", next: "Normal", quickFormat: true,
        run: { size: 26, bold: true, font: "Arial", color: "404040" },
        paragraph: { spacing: { before: 240, after: 120 }, outlineLevel: 2 }
      }
    ]
  },
  sections: [
    // ====== 封面 ======
    {
      properties: {
        page: {
          size: { width: 12240, height: 15840 },
          margin: { top: 1440, right: 1440, bottom: 1440, left: 1440 }
        }
      },
      children: [
        new Paragraph({ spacing: { before: 3600 }, children: [] }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { after: 200 },
          children: [new TextRun({ text: "CryptoTool.Algorithm", size: 56, bold: true, color: "2E75B6", font: "Arial" })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { after: 200 },
          children: [new TextRun({ text: "项目架构与代码归类分析文档", size: 36, color: "666666", font: "Arial" })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { before: 600, after: 100 },
          children: [new TextRun({ text: "版本: 1.0.0.0  |  目标框架: .NET Standard 2.1  |  语言: C# 8.0", size: 22, color: "999999", font: "Arial" })]
        }),
        new Paragraph({
          alignment: AlignmentType.CENTER,
          spacing: { after: 100 },
          children: [new TextRun({ text: "依赖: BouncyCastle.Cryptography 2.6.2 + Newtonsoft.Json 13.0.3", size: 22, color: "999999", font: "Arial" })]
        }),
        new Paragraph({ spacing: { before: 2400 }, children: [new PageBreak()] })
      ]
    },
    // ====== 正文 ======
    {
      properties: {
        page: {
          size: { width: 12240, height: 15840 },
          margin: { top: 1440, right: 1440, bottom: 1440, left: 1440 }
        }
      },
      headers: {
        default: new Header({
          children: [new Paragraph({
            alignment: AlignmentType.RIGHT,
            border: { bottom: { style: BorderStyle.SINGLE, size: 6, color: "2E75B6", space: 4 } },
            children: [new TextRun({ text: "CryptoTool.Algorithm \u2014 架构分析", size: 18, color: "999999", font: "Arial" })]
          })]
        })
      },
      footers: {
        default: new Footer({
          children: [new Paragraph({
            alignment: AlignmentType.CENTER,
            children: [
              new TextRun({ text: "Page ", size: 18, color: "999999", font: "Arial" }),
              new TextRun({ children: [PageNumber.CURRENT], size: 18, color: "999999", font: "Arial" })
            ]
          })]
        })
      },
      children: [
        // === 第1章 ===
        heading(HeadingLevel.HEADING_1, "1. 项目概述"),
        para("CryptoTool.Algorithm 是一个全面的 .NET 加密算法类库，基于 .NET Standard 2.1，使用 C# 8.0 编写。该库封装了国际标准算法与中国国家密码标准（国密）算法，为上层应用提供统一的加解密、签名验签、哈希计算接口。"),
        para([
          new TextRun({ text: "目标框架: ", bold: true }), new TextRun("netstandard2.1（兼容 .NET Core 3.0+ / .NET 5-9 / .NET Framework 4.8+）")
        ]),
        para([
          new TextRun({ text: "核心依赖: ", bold: true }), new TextRun("BouncyCastle.Cryptography 2.6.2（提供 ECDSA/SM2/SM3/SM4 的底层密码学原语）")
        ]),

        heading(HeadingLevel.HEADING_2, "1.1 支持的算法"),
        new Table({
          width: { size: 9360, type: WidthType.DXA },
          columnWidths: [1560, 1560, 3120, 1560, 1560],
          rows: [
            new TableRow({ children: [
              headerCell("算法", 1560), headerCell("类型", 1560),
              headerCell("功能", 3120), headerCell("标准", 1560), headerCell("实现类", 1560)
            ]}),
            new TableRow({ children: [
              cell("RSA", 1560), cell("非对称", 1560),
              cell("加/解密、签名/验签、密钥生成", 3120), cell("PKCS#1", 1560), cell("RsaCrypto", 1560)
            ]}),
            new TableRow({ children: [
              cell("ECDSA", 1560), cell("非对称", 1560),
              cell("签名/验签、ECDH密钥协商、PEM转换", 3120), cell("RFC 5480/5915", 1560), cell("EcdsaAlgorithm", 1560)
            ]}),
            new TableRow({ children: [
              cell("AES", 1560), cell("对称", 1560),
              cell("ECB/CBC 加解密", 3120), cell("FIPS 197", 1560), cell("AesCrypto", 1560)
            ]}),
            new TableRow({ children: [
              cell("DES", 1560), cell("对称", 1560),
              cell("ECB/CBC 加解密", 3120), cell("FIPS 46-3", 1560), cell("DesCrypto", 1560)
            ]}),
            new TableRow({ children: [
              cell("SM2", 1560), cell("非对称", 1560),
              cell("加/解密、签名/验签、密钥生成、密文格式转换", 3120), cell("GM/T 0003", 1560), cell("Sm2Crypto", 1560)
            ]}),
            new TableRow({ children: [
              cell("SM3", 1560), cell("哈希", 1560),
              cell("256位国密哈希摘要", 3120), cell("GM/T 0004", 1560), cell("Sm3Hash", 1560)
            ]}),
            new TableRow({ children: [
              cell("SM4", 1560), cell("对称", 1560),
              cell("ECB/CBC 加解密", 3120), cell("GM/T 0002", 1560), cell("Sm4Crypto", 1560)
            ]}),
            new TableRow({ children: [
              cell("MD5", 1560), cell("哈希", 1560),
              cell("128位哈希摘要", 3120), cell("RFC 1321", 1560), cell("Md5Hash", 1560)
            ]})
          ]
        }),
        spacer(),

        // === 第2章 ===
        heading(HeadingLevel.HEADING_1, "2. 目录结构"),
        new Table({
          width: { size: 9360, type: WidthType.DXA },
          columnWidths: [3120, 6240],
          rows: [
            new TableRow({ children: [headerCell("目录/文件", 3120), headerCell("职责说明", 6240)] }),
            new TableRow({ children: [cell("CryptoTool.cs", 3120), cell("主入口静态类，提供所有算法的统一访问入口", 6240)] }),
            new TableRow({ children: [cell("CryptoTool.Algorithm.csproj", 3120), cell("项目文件，定义框架版本、NuGet依赖等", 6240)] }),
            new TableRow({ children: [cell("README.md", 3120), cell("使用文档，含代码示例", 6240)] }),
            new TableRow({ children: [cell("Algorithms/", 3120), cell("所有算法具体实现（14个.cs文件），按算法类型分子目录", 6240)] }),
            new TableRow({ children: [cell("Interfaces/", 3120), cell("核心接口定义：ICryptoAlgorithm、IAsymmetricCrypto、ISymmetricCrypto、IHashAlgorithm", 6240)] }),
            new TableRow({ children: [cell("Enums/", 3120), cell("枚举定义：填充模式、加密模式、签名算法等", 6240)] }),
            new TableRow({ children: [cell("Factory/", 3120), cell("工厂类 CryptoFactory，统一创建算法实例", 6240)] }),
            new TableRow({ children: [cell("Exceptions/", 3120), cell("自定义异常体系 CryptoException", 6240)] }),
            new TableRow({ children: [cell("Utils/", 3120), cell("工具类：医保加解密、阿里云CSB签名、字节/字符串转换", 6240)] })
          ]
        }),
        spacer(),

        // === 第3章 ===
        heading(HeadingLevel.HEADING_1, "3. 架构设计与归类"),
        para("项目采用经典的", [new TextRun({ text: "接口-实现-工厂", bold: true })]),
        para([new TextRun({ text: "三层架构", bold: true }), new TextRun("：")]),
        code("Interfaces (抽象层) --> Algorithms (实现层) --> CryptoTool / CryptoFactory (接入层)"),
        spacer(),

        heading(HeadingLevel.HEADING_2, "3.1 接口层 (Interfaces)"),
        para("定义了算法的最小契约，所有算法实现必须遵循："),
        bullet([
          new TextRun({ text: "ICryptoAlgorithm: ", bold: true }),
          new TextRun("基础接口，定义 AlgorithmName 属性和 Encrypt/Decrypt 方法")
        ]),
        bullet([
          new TextRun({ text: "IAsymmetricCrypto: ", bold: true }),
          new TextRun("非对称算法接口，扩展 GenerateKeyPair、SignData、VerifySignature")
        ]),
        bullet([
          new TextRun({ text: "ISymmetricCrypto: ", bold: true }),
          new TextRun("对称算法接口，要求提供 CipherMode 和 PaddingMode 属性")
        ]),
        bullet([
          new TextRun({ text: "IHashAlgorithm: ", bold: true }),
          new TextRun("哈希算法接口，定义 ComputeHash 方法")
        ]),
        para("同时包含 ", [new TextRun({ text: "CryptoAlgorithmType", bold: true }), new TextRun(" 枚举，用于标识算法类型（Aes、Des、Rsa、Sm2、Sm3、Sm4、Md5）。")]),

        heading(HeadingLevel.HEADING_2, "3.2 实现层 (Algorithms)"),
        para("按算法分子目录，每种子目录维护该算法的全部相关代码："),

        heading(HeadingLevel.HEADING_3, "AES/ 目录"),
        bullet("AesCrypto.cs \u2014 实现 ISymmetricCrypto，支持 ECB/CBC 模式 + PKCS7/Zero/None 填充"),
        bullet("密钥支持 128/192/256 位"),
        bullet("基于 BouncyCastle IBufferedCipher 实现"),

        heading(HeadingLevel.HEADING_3, "DES/ 目录"),
        bullet("DesCrypto.cs \u2014 实现 ISymmetricCrypto，支持 ECB/CBC 模式"),
        bullet("基于 BouncyCastle IBufferedCipher 实现"),

        heading(HeadingLevel.HEADING_3, "ECDSA/ 目录（4 个文件）"),
        bullet([
          new TextRun({ text: "EcdsaAlgorithm.cs: ", bold: true }),
          new TextRun("ECDSA 签名/验证核心实现，支持 NIST P-256/P-384/P-521 和 secp256k1")
        ]),
        bullet([
          new TextRun({ text: "EcdhAlgorithm.cs: ", bold: true }),
          new TextRun("ECDH 密钥协商算法，支持双方各自使用自己的私钥生成共享密钥")
        ]),
        bullet([
          new TextRun({ text: "EcdsaKeyHelper.cs: ", bold: true }),
          new TextRun("密钥 PEM/DER 格式导入导出，支持 SEC1(PKCS#8)/X.509 SubjectPublicKeyInfo 标准")
        ]),
        bullet([
          new TextRun({ text: "EcdsaCurveNames.cs: ", bold: true }),
          new TextRun("椭圆曲线名称映射与分类，提供 OID <-> 名称互查")
        ]),

        heading(HeadingLevel.HEADING_3, "RSA/ 目录"),
        bullet("RsaCrypto.cs \u2014 实现 IAsymmetricCrypto，支持 OAEP/PKCS1 填充的加解密和签名验签"),
        bullet("密钥生成支持 1024/2048/4096 位，基于 BouncyCastle RsaEngine 和 PssSigner"),

        heading(HeadingLevel.HEADING_3, "SM2/ 目录（4 个文件）"),
        bullet([
          new TextRun({ text: "Sm2Crypto.cs: ", bold: true }),
          new TextRun("SM2 国密非对称加密实现，支持加解密、签名验签、密钥对生成")
        ]),
        bullet([
          new TextRun({ text: "SM2CipherFormat.cs: ", bold: true }),
          new TextRun("SM2 密文格式枚举（C1C2C3 / C1C3C2），两种格式互转")
        ]),
        bullet([
          new TextRun({ text: "Sm2CipherFormatConverter.cs: ", bold: true }),
          new TextRun("密文格式转换器，自动检测源格式并转为目标格式")
        ]),
        bullet([
          new TextRun({ text: "SM2CipherComponentInfo.cs: ", bold: true }),
          new TextRun("SM2 密文组件信息模型（C1/C2/C3 三段式结构）")
        ]),

        heading(HeadingLevel.HEADING_3, "SM3/ 目录"),
        bullet("Sm3Hash.cs \u2014 实现 IHashAlgorithm，256位国密哈希，基于 BouncyCastle SM3Digest"),

        heading(HeadingLevel.HEADING_3, "SM4/ 目录"),
        bullet("Sm4Crypto.cs \u2014 实现 ISymmetricCrypto，支持 CBC/ECB 模式，128位固定密钥"),

        heading(HeadingLevel.HEADING_3, "MD5/ 目录"),
        bullet("Md5Hash.cs \u2014 实现 IHashAlgorithm，128位哈希，基于 System.Security.Cryptography.MD5"),
        spacer(),

        heading(HeadingLevel.HEADING_2, "3.3 接入层"),
        bullet([
          new TextRun({ text: "CryptoTool.cs (门面模式/Facade): ", bold: true }),
          new TextRun("提供所有算法的静态方法入口，如 CryptoTool.AesEncrypt()、CryptoTool.Sm2Sign() 等")
        ]),
        bullet([
          new TextRun({ text: "CryptoFactory.cs (工厂模式): ", bold: true }),
          new TextRun("根据 CryptoAlgorithmType 枚举动态创建算法实例，支持对称/非对称/哈希三种类型的方法重载")
        ]),

        // === 第4章 ===
        heading(HeadingLevel.HEADING_1, "4. 枚举层 (Enums)"),
        para("统一管理所有算法的配置选项，避免参数散落在各实现类中："),

        new Table({
          width: { size: 9360, type: WidthType.DXA },
          columnWidths: [2800, 4640, 1920],
          rows: [
            new TableRow({ children: [headerCell("枚举", 2800), headerCell("成员", 4640), headerCell("作用域", 1920)] }),
            new TableRow({ children: [
              cell("SymmetricCipherMode", 2800),
              cell("ECB、CBC、CFB、OFB、CTR", 4640),
              cell("对称加密", 1920)
            ]}),
            new TableRow({ children: [
              cell("SymmetricPaddingMode", 2800),
              cell("PKCS7、ISO10126、Zeros、None", 4640),
              cell("对称加密", 1920)
            ]}),
            new TableRow({ children: [
              cell("AsymmetricPaddingMode", 2800),
              cell("PKCS1、OAEP_SHA1、OAEP_SHA256、NoPadding", 4640),
              cell("非对称加密", 1920)
            ]}),
            new TableRow({ children: [
              cell("SignatureAlgorithm", 2800),
              cell("ECDSA、SM2Signature、MD5withRSA、SHA1withECDSA等", 4640),
              cell("签名验签", 1920)
            ]})
          ]
        }),
        spacer(),

        // === 第5章 ===
        heading(HeadingLevel.HEADING_1, "5. 工具层 (Utils)"),
        para("提供业务级工具和通用辅助方法："),

        new Table({
          width: { size: 9360, type: WidthType.DXA },
          columnWidths: [2340, 7020],
          rows: [
            new TableRow({ children: [headerCell("工具类", 2340), headerCell("功能", 7020)] }),
            new TableRow({ children: [
              cell("StringUtil.cs", 2340),
              cell("字节数组 <-> Base64/Hex字符串互转、随机字节生成等通用工具", 7020)
            ]}),
            new TableRow({ children: [
              cell("CryptoPaddingUtil.cs", 2340),
              cell("对称/非对称填充模式从自定义枚举到 BouncyCastle 枚举的转换", 7020)
            ]}),
            new TableRow({ children: [
              cell("MedicareUtil.cs", 2340),
              cell("医保业务专用工具：SM4密钥生成、SM2签名/验签、医保报文加解密一体化", 7020)
            ]}),
            new TableRow({ children: [
              cell("AliyunCSBUtil.cs", 2340),
              cell("阿里云CSB网关 HMAC-SHA256 签名工具，用于API请求鉴权", 7020)
            ]})
          ]
        }),
        spacer(),

        // === 第6章 ===
        heading(HeadingLevel.HEADING_1, "6. 异常体系 (Exceptions)"),
        para("自定义异常类 ", [new TextRun({ text: "CryptoException", bold: true }), new TextRun("，包含：")]),
        bullet([new TextRun({ text: "ErrorCode (int): ", bold: true }), new TextRun("错误码，便于定位问题")]),
        bullet([new TextRun({ text: "AlgorithmName (string?): ", bold: true }), new TextRun("出错的算法名称")]),
        bullet([new TextRun({ text: "Operation (string?): ", bold: true }), new TextRun("出错的操作用途（如\"加密\"、\"解密\"、\"签名\"）")]),
        spacer(),

        // === 第7章 ===
        heading(HeadingLevel.HEADING_1, "7. 代码写法与设计模式"),
        para([new TextRun({ text: "7.1 门面模式 (Facade / Gateway)", bold: true, size: 24 })]),
        para("CryptoTool.cs 作为统一门面，对外暴露简洁的静态方法。调用者无需了解内部实现，例如："),
        code("CryptoTool.Sm2Encrypt(data, publicKey);  // 一行代码完成SM2加密"),
        code("CryptoTool.RsaGenerateKeyPair(2048);     // 一行代码生成RSA密钥对"),
        spacer(),

        para([new TextRun({ text: "7.2 工厂方法模式 (Factory Method)", bold: true, size: 24 })]),
        para("CryptoFactory 根据 CryptoAlgorithmType 枚举动态创建实例，支持扩展新算法而不修改调用方："),
        code("var aes = CryptoFactory.CreateSymmetric(CryptoAlgorithmType.Aes, key, iv, mode, padding);"),
        code("var rsa = CryptoFactory.CreateAsymmetric(CryptoAlgorithmType.Rsa);"),
        spacer(),

        para([new TextRun({ text: "7.3 策略模式 (Strategy)", bold: true, size: 24 })]),
        para("所有算法实现同一接口（ICryptoAlgorithm / IAsymmetricCrypto / ISymmetricCrypto / IHashAlgorithm），可以在运行时替换算法实现："),
        code("IAsymmetricCrypto algo = useSM2 ? new Sm2Crypto() : new RsaCrypto();"),
        code("algo.SignData(data, privateKey);  // 统一接口，无需关心底层算法"),
        spacer(),

        para([new TextRun({ text: "7.4 命名空间归类规范", bold: true, size: 24 })]),
        bullet([
          new TextRun({ text: "CryptoTool.Algorithm.Algorithms.*: ", bold: true }),
          new TextRun("具体算法实现，如 Algorithms.Aes、Algorithms.SM2")
        ]),
        bullet([
          new TextRun({ text: "CryptoTool.Algorithm.Interfaces: ", bold: true }),
          new TextRun("接口契约定义")
        ]),
        bullet([
          new TextRun({ text: "CryptoTool.Algorithm.Enums: ", bold: true }),
          new TextRun("配置枚举")
        ]),
        bullet([
          new TextRun({ text: "CryptoTool.Algorithm.Utils: ", bold: true }),
          new TextRun("通用与业务工具")
        ]),
        bullet([
          new TextRun({ text: "CryptoTool.Algorithm.Factory: ", bold: true }),
          new TextRun("工厂创建")
        ]),
        bullet([
          new TextRun({ text: "CryptoTool.Algorithm.Exceptions: ", bold: true }),
          new TextRun("异常定义")
        ]),
        spacer(),

        // === 第8章 ===
        heading(HeadingLevel.HEADING_1, "8. 技术栈与依赖关系"),
        new Table({
          width: { size: 9360, type: WidthType.DXA },
          columnWidths: [3120, 1560, 4680],
          rows: [
            new TableRow({ children: [headerCell("依赖包", 3120), headerCell("版本", 1560), headerCell("用途", 4680)] }),
            new TableRow({ children: [
              cell("BouncyCastle.Cryptography", 3120),
              cell("2.6.2", 1560),
              cell("核心密码学库：ECDSA/NIST曲线、SM2/SM3/SM4国密、RSA/AES/DES底层引擎", 4680)
            ]}),
            new TableRow({ children: [
              cell("Newtonsoft.Json", 3120),
              cell("13.0.3", 1560),
              cell("JSON序列化：医保报文格式化、密钥导出JSON格式", 4680)
            ]}),
            new TableRow({ children: [
              cell("System.Security.Cryptography.Algorithms", 3120),
              cell("4.3.1", 1560),
              cell(".NET原生加密基类（MD5等依赖）", 4680)
            ]}),
            new TableRow({ children: [
              cell("System.Security.Cryptography.Cng", 3120),
              cell("4.3.0", 1560),
              cell("Windows Cryptography Next Generation 支持", 4680)
            ]}),
            new TableRow({ children: [
              cell("System.Security.Cryptography.OpenSsl", 3120),
              cell("4.3.0", 1560),
              cell("Linux/macOS OpenSSL 原生加密支持", 4680)
            ]})
          ]
        }),
        spacer(),

        // === 第9章 ===
        heading(HeadingLevel.HEADING_1, "9. 总结"),
        para("CryptoTool.Algorithm 是一个设计规范、层次清晰的 .NET 加密类库。其核心优势："),
        bullet([
          new TextRun("统一入口: ", bold: true),
          new TextRun("CryptoTool.cs 静态门面提供一行式调用体验")
        ]),
        bullet([
          new TextRun("接口驱动: ", bold: true),
          new TextRun("所有算法实现标准化接口，支持运行时策略切换")
        ]),
        bullet([
          new TextRun("按算法归类: ", bold: true),
          new TextRun("每个算法独立子目录（AES/DES/ECDSA/RSA/SM2/SM3/SM4/MD5），职责清晰")
        ]),
        bullet([
          new TextRun("国密全覆盖: ", bold: true),
          new TextRun("SM2/SM3/SM4 完整实现，支持 C1C2C3/C1C3C2 密文格式转换")
        ]),
        bullet([
          new TextRun("企业级工具: ", bold: true),
          new TextRun("医保报文加解密、阿里云CSB网关签名等业务工具开箱即用")
        ]),
        bullet([
          new TextRun("国际化: ", bold: true),
          new TextRun("同时支持国际标准（RSA/AES/ECDSA）和中国国密标准（SM系列）")
        ]),
        spacer(),
        spacer(),
        para([
          new TextRun({ text: "文档生成日期: ", color: "999999" }),
          new TextRun({ text: "2026-07-30", color: "999999" })
        ])
      ]
    }
  ]
});

Packer.toBuffer(doc).then(buffer => {
  fs.writeFileSync("E:/Users/Documents/GitHub/CryptoTool/docs/CryptoTool.Algorithm_Analysis.docx", buffer);
  console.log("Document generated: docs/CryptoTool.Algorithm_Analysis.docx");
});
