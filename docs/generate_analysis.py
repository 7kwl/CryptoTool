"""生成 CryptoTool.Algorithm 架构分析文档"""
from docx import Document
from docx.shared import Inches, Pt, Cm, RGBColor, Emu
from docx.enum.text import WD_ALIGN_PARAGRAPH
from docx.enum.table import WD_TABLE_ALIGNMENT
from docx.enum.style import WD_STYLE_TYPE
from docx.oxml.ns import qn, nsdecls
from docx.oxml import parse_xml
import datetime

doc = Document()

# ===== 样式设置 =====
style = doc.styles['Normal']
style.font.name = 'Arial'
style.font.size = Pt(11)
style.font.color.rgb = RGBColor(0x33, 0x33, 0x33)
style.paragraph_format.space_after = Pt(4)
style.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')

for level in [1, 2, 3]:
    h = doc.styles[f'Heading {level}']
    h.font.name = 'Arial'
    h.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')
    if level == 1:
        h.font.size = Pt(18)
        h.font.color.rgb = RGBColor(0x2E, 0x75, 0xB6)
        h.paragraph_format.space_before = Pt(24)
        h.paragraph_format.space_after = Pt(10)
    elif level == 2:
        h.font.size = Pt(15)
        h.font.color.rgb = RGBColor(0x2E, 0x75, 0xB6)
        h.paragraph_format.space_before = Pt(18)
        h.paragraph_format.space_after = Pt(8)
    else:
        h.font.size = Pt(13)
        h.font.color.rgb = RGBColor(0x40, 0x40, 0x40)
        h.paragraph_format.space_before = Pt(12)
        h.paragraph_format.space_after = Pt(6)


def add_para(text, bold=False, size=None, color=None, align=None, space_after=4, space_before=0):
    p = doc.add_paragraph()
    p.paragraph_format.space_after = Pt(space_after)
    p.paragraph_format.space_before = Pt(space_before)
    if align:
        p.alignment = align
    run = p.add_run(text)
    run.font.name = 'Arial'
    run.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')
    if bold:
        run.bold = True
    if size:
        run.font.size = Pt(size)
    if color:
        run.font.color.rgb = color
    return p


def add_rich_para(segments, space_after=4):
    """segments: list of (text, bold, size, color)"""
    p = doc.add_paragraph()
    p.paragraph_format.space_after = Pt(space_after)
    for seg in segments:
        text, bold, size, color = seg[0], seg[1] if len(seg) > 1 else False, seg[2] if len(seg) > 2 else None, seg[3] if len(seg) > 3 else None
        run = p.add_run(text)
        run.font.name = 'Arial'
        run.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')
        if bold:
            run.bold = True
        if size:
            run.font.size = Pt(size)
        if color:
            run.font.color.rgb = color
    return p


def add_bullet(text):
    p = doc.add_paragraph(style='List Bullet')
    p.paragraph_format.space_after = Pt(2)
    if text:
        p.clear()
        run = p.add_run(text)
        run.font.name = 'Arial'
        run.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')
        run.font.size = Pt(11)
    return p


def add_code(text):
    p = doc.add_paragraph()
    p.paragraph_format.space_after = Pt(6)
    p.paragraph_format.left_indent = Cm(1)
    p.paragraph_format.space_before = Pt(2)
    shading = parse_xml(f'<w:shd {nsdecls("w")} w:fill="F5F5F5" w:val="clear"/>')
    p.paragraph_format.element.get_or_add_pPr().append(shading)
    run = p.add_run(text)
    run.font.name = 'Consolas'
    run.font.size = Pt(9)
    return p


def set_cell_shading(cell, color):
    shading = parse_xml(f'<w:shd {nsdecls("w")} w:fill="{color}" w:val="clear"/>')
    cell._tc.get_or_add_tcPr().append(shading)


def set_cell_border(cell, **kwargs):
    tc = cell._tc
    tcPr = tc.get_or_add_tcPr()
    tcBorders = parse_xml(f'<w:tcBorders {nsdecls("w")}></w:tcBorders>')
    for edge, val in kwargs.items():
        element = parse_xml(
            f'<w:{edge} {nsdecls("w")} w:val="single" w:sz="4" w:space="0" w:color="999999"/>')
        tcBorders.append(element)
    tcPr.append(tcBorders)


def add_table(headers, rows, col_widths=None):
    total_width = sum(col_widths) if col_widths else 9360
    table = doc.add_table(rows=1 + len(rows), cols=len(headers))
    table.style = 'Table Grid'
    table.alignment = WD_TABLE_ALIGNMENT.CENTER

    # Header
    for i, h in enumerate(headers):
        cell = table.rows[0].cells[i]
        cell.text = ''
        run = cell.paragraphs[0].add_run(h)
        run.bold = True
        run.font.name = 'Arial'
        run.font.size = Pt(10)
        run.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')
        set_cell_shading(cell, 'D5E8F0')
        if col_widths:
            cell.width = Cm(col_widths[i] * 0.035)

    for r, row in enumerate(rows):
        for c, val in enumerate(row):
            cell = table.rows[r + 1].cells[c]
            cell.text = ''
            run = cell.paragraphs[0].add_run(str(val))
            run.font.name = 'Arial'
            run.font.size = Pt(10)
            run.element.rPr.rFonts.set(qn('w:eastAsia'), '微软雅黑')
            if col_widths:
                cell.width = Cm(col_widths[c] * 0.035)

    doc.add_paragraph()  # spacing after table
    return table


# ===== 封面 =====
for _ in range(8):
    doc.add_paragraph()

add_para('CryptoTool.Algorithm', bold=True, size=28, color=RGBColor(0x2E, 0x75, 0xB6),
         align=WD_ALIGN_PARAGRAPH.CENTER, space_after=6)
add_para('项目架构与代码归类分析文档', size=18, color=RGBColor(0x66, 0x66, 0x66),
         align=WD_ALIGN_PARAGRAPH.CENTER, space_after=24)
add_para('版本: 1.0.0.0  |  目标框架: .NET Standard 2.1  |  语言: C# 8.0',
         size=11, color=RGBColor(0x99, 0x99, 0x99), align=WD_ALIGN_PARAGRAPH.CENTER, space_after=4)
add_para('依赖: BouncyCastle.Cryptography 2.6.2 + Newtonsoft.Json 13.0.3',
         size=11, color=RGBColor(0x99, 0x99, 0x99), align=WD_ALIGN_PARAGRAPH.CENTER)
add_para(f'文档生成日期: {datetime.date.today().isoformat()}',
         size=11, color=RGBColor(0x99, 0x99, 0x99), align=WD_ALIGN_PARAGRAPH.CENTER)

doc.add_page_break()

# ===== 1. 项目概述 =====
doc.add_heading('1. 项目概述', level=1)
add_para('CryptoTool.Algorithm 是一个全面的 .NET 加密算法类库，基于 .NET Standard 2.1，使用 C# 8.0 编写。该库封装了国际标准算法与中国国家密码标准（国密）算法，为上层应用提供统一的加解密、签名验签、哈希计算接口。')
add_rich_para([('目标框架: ', True), ('netstandard2.1（兼容 .NET Core 3.0+ / .NET 5-9 / .NET Framework 4.8+）', False)])
add_rich_para([('核心依赖: ', True), ('BouncyCastle.Cryptography 2.6.2（提供 ECDSA/SM2/SM3/SM4 的底层密码学原语）', False)])

doc.add_heading('1.1 支持的算法', level=2)
add_table(
    ['算法', '类型', '功能', '标准', '实现类'],
    [
        ['RSA', '非对称', '加/解密、签名/验签、密钥生成', 'PKCS#1', 'RsaCrypto'],
        ['ECDSA', '非对称', '签名/验签、ECDH密钥协商、PEM转换', 'RFC 5480/5915', 'EcdsaAlgorithm'],
        ['AES', '对称', 'ECB/CBC 加解密', 'FIPS 197', 'AesCrypto'],
        ['DES', '对称', 'ECB/CBC 加解密', 'FIPS 46-3', 'DesCrypto'],
        ['SM2', '非对称', '加/解密、签名/验签、密钥生成、密文格式转换', 'GM/T 0003', 'Sm2Crypto'],
        ['SM3', '哈希', '256位国密哈希摘要', 'GM/T 0004', 'Sm3Hash'],
        ['SM4', '对称', 'ECB/CBC 加解密', 'GM/T 0002', 'Sm4Crypto'],
        ['MD5', '哈希', '128位哈希摘要', 'RFC 1321', 'Md5Hash'],
    ]
)

# ===== 2. 目录结构 =====
doc.add_heading('2. 目录结构', level=1)
add_table(
    ['目录/文件', '职责说明'],
    [
        ['CryptoTool.cs', '主入口静态类，提供所有算法的统一访问入口（门面模式）'],
        ['CryptoTool.Algorithm.csproj', '项目文件，定义框架版本、NuGet依赖等'],
        ['README.md', '使用文档，含代码示例'],
        ['Algorithms/', '所有算法具体实现（14个.cs文件），按算法类型分子目录'],
        ['Interfaces/', '核心接口定义：ICryptoAlgorithm、IAsymmetricCrypto、ISymmetricCrypto、IHashAlgorithm'],
        ['Enums/', '枚举定义：填充模式、加密模式、签名算法等'],
        ['Factory/', '工厂类 CryptoFactory，统一创建算法实例'],
        ['Exceptions/', '自定义异常体系 CryptoException'],
        ['Utils/', '工具类：医保加解密、阿里云CSB签名、字节/字符串转换'],
    ]
)

# ===== 3. 架构设计与归类 =====
doc.add_heading('3. 架构设计与归类', level=1)
add_para('项目采用经典的三层架构：')
add_code('Interfaces (抽象层)  -->  Algorithms (实现层)  -->  CryptoTool / CryptoFactory (接入层)')

doc.add_heading('3.1 接口层 (Interfaces)', level=2)
add_para('定义了算法的最小契约，所有算法实现必须遵循：')
add_rich_para([('ICryptoAlgorithm: ', True), ('基础接口，定义 AlgorithmName 属性和 Encrypt/Decrypt 方法', False)])
add_rich_para([('IAsymmetricCrypto: ', True), ('非对称算法接口，扩展 GenerateKeyPair、SignData、VerifySignature', False)])
add_rich_para([('ISymmetricCrypto: ', True), ('对称算法接口，要求提供 CipherMode 和 PaddingMode 属性', False)])
add_rich_para([('IHashAlgorithm: ', True), ('哈希算法接口，定义 ComputeHash 方法', False)])
add_rich_para([('同时包含 CryptoAlgorithmType 枚举，用于标识算法类型（Aes、Des、Rsa、Sm2、Sm3、Sm4、Md5）。', False)])

doc.add_heading('3.2 实现层 (Algorithms)', level=2)
add_para('按算法分子目录，每种子目录维护该算法的全部相关代码：')

doc.add_heading('AES/ 目录', level=3)
add_bullet('AesCrypto.cs — 实现 ISymmetricCrypto，支持 ECB/CBC 模式 + PKCS7/Zero/None 填充')
add_bullet('密钥支持 128/192/256 位')
add_bullet('基于 BouncyCastle IBufferedCipher 实现')

doc.add_heading('DES/ 目录', level=3)
add_bullet('DesCrypto.cs — 实现 ISymmetricCrypto，支持 ECB/CBC 模式')
add_bullet('基于 BouncyCastle IBufferedCipher 实现')

doc.add_heading('ECDSA/ 目录（4 个文件）', level=3)
add_rich_para([('EcdsaAlgorithm.cs: ', True), ('ECDSA 签名/验证核心实现，支持 NIST P-256/P-384/P-521 和 secp256k1', False)])
add_rich_para([('EcdhAlgorithm.cs: ', True), ('ECDH 密钥协商算法，支持双方各自使用自己的私钥生成共享密钥', False)])
add_rich_para([('EcdsaKeyHelper.cs: ', True), ('密钥 PEM/DER 格式导入导出，支持 SEC1(PKCS#8)/X.509 SubjectPublicKeyInfo 标准', False)])
add_rich_para([('EcdsaCurveNames.cs: ', True), ('椭圆曲线名称映射与分类，提供 OID <-> 名称互查', False)])

doc.add_heading('RSA/ 目录', level=3)
add_bullet('RsaCrypto.cs — 实现 IAsymmetricCrypto，支持 OAEP/PKCS1 填充的加解密和签名验签')
add_bullet('密钥生成支持 1024/2048/4096 位，基于 BouncyCastle RsaEngine 和 PssSigner')

doc.add_heading('SM2/ 目录（4 个文件）', level=3)
add_rich_para([('Sm2Crypto.cs: ', True), ('SM2 国密非对称加密实现，支持加解密、签名验签、密钥对生成', False)])
add_rich_para([('SM2CipherFormat.cs: ', True), ('SM2 密文格式枚举（C1C2C3 / C1C3C2），两种格式互转', False)])
add_rich_para([('Sm2CipherFormatConverter.cs: ', True), ('密文格式转换器，自动检测源格式并转为目标格式', False)])
add_rich_para([('SM2CipherComponentInfo.cs: ', True), ('SM2 密文组件信息模型（C1/C2/C3 三段式结构）', False)])

doc.add_heading('SM3/ 目录', level=3)
add_bullet('Sm3Hash.cs — 实现 IHashAlgorithm，256位国密哈希，基于 BouncyCastle SM3Digest')

doc.add_heading('SM4/ 目录', level=3)
add_bullet('Sm4Crypto.cs — 实现 ISymmetricCrypto，支持 CBC/ECB 模式，128位固定密钥')

doc.add_heading('MD5/ 目录', level=3)
add_bullet('Md5Hash.cs — 实现 IHashAlgorithm，128位哈希，基于 System.Security.Cryptography.MD5')

doc.add_heading('3.3 接入层', level=2)
add_rich_para([('CryptoTool.cs (门面模式/Facade): ', True), ('提供所有算法的静态方法入口，如 CryptoTool.AesEncrypt()、CryptoTool.Sm2Sign() 等', False)])
add_rich_para([('CryptoFactory.cs (工厂模式): ', True), ('根据 CryptoAlgorithmType 枚举动态创建算法实例，支持对称/非对称/哈希三种类型的方法重载', False)])

# ===== 4. 枚举层 =====
doc.add_heading('4. 枚举层 (Enums)', level=1)
add_para('统一管理所有算法的配置选项，避免参数散落在各实现类中：')
add_table(
    ['枚举', '成员', '作用域'],
    [
        ['SymmetricCipherMode', 'ECB、CBC、CFB、OFB、CTR', '对称加密'],
        ['SymmetricPaddingMode', 'PKCS7、ISO10126、Zeros、None', '对称加密'],
        ['AsymmetricPaddingMode', 'PKCS1、OAEP_SHA1、OAEP_SHA256、NoPadding', '非对称加密'],
        ['SignatureAlgorithm', 'ECDSA、SM2Signature、MD5withRSA、SHA1withECDSA 等', '签名验签'],
    ]
)

# ===== 5. 工具层 =====
doc.add_heading('5. 工具层 (Utils)', level=1)
add_para('提供业务级工具和通用辅助方法：')
add_table(
    ['工具类', '功能'],
    [
        ['StringUtil.cs', '字节数组 <-> Base64/Hex字符串互转、随机字节生成等通用工具'],
        ['CryptoPaddingUtil.cs', '对称/非对称填充模式从自定义枚举到 BouncyCastle 枚举的转换'],
        ['MedicareUtil.cs', '医保业务专用工具：SM4密钥生成、SM2签名/验签、医保报文加解密一体化'],
        ['AliyunCSBUtil.cs', '阿里云CSB网关 HMAC-SHA256 签名工具，用于API请求鉴权'],
    ]
)

# ===== 6. 异常体系 =====
doc.add_heading('6. 异常体系 (Exceptions)', level=1)
add_para('自定义异常类 CryptoException，包含：')
add_rich_para([('ErrorCode (int): ', True), ('错误码，便于定位问题', False)])
add_rich_para([('AlgorithmName (string?): ', True), ('出错的算法名称', False)])
add_rich_para([('Operation (string?): ', True), ('出错的操作用途（如"加密"、"解密"、"签名"）', False)])

# ===== 7. 代码写法与设计模式 =====
doc.add_heading('7. 代码写法与设计模式', level=1)

doc.add_heading('7.1 门面模式 (Facade / Gateway)', level=2)
add_para('CryptoTool.cs 作为统一门面，对外暴露简洁的静态方法。调用者无需了解内部实现，例如：')
add_code('CryptoTool.Sm2Encrypt(data, publicKey);  // 一行代码完成SM2加密')
add_code('CryptoTool.RsaGenerateKeyPair(2048);     // 一行代码生成RSA密钥对')

doc.add_heading('7.2 工厂方法模式 (Factory Method)', level=2)
add_para('CryptoFactory 根据 CryptoAlgorithmType 枚举动态创建实例，支持扩展新算法而不修改调用方：')
add_code('var aes = CryptoFactory.CreateSymmetric(CryptoAlgorithmType.Aes, key, iv, mode, padding);')
add_code('var rsa = CryptoFactory.CreateAsymmetric(CryptoAlgorithmType.Rsa);')

doc.add_heading('7.3 策略模式 (Strategy)', level=2)
add_para('所有算法实现同一接口，可以在运行时替换算法实现：')
add_code('IAsymmetricCrypto algo = useSM2 ? new Sm2Crypto() : new RsaCrypto();')
add_code('algo.SignData(data, privateKey);  // 统一接口，无需关心底层算法')

doc.add_heading('7.4 命名空间归类规范', level=2)
add_rich_para([('CryptoTool.Algorithm.Algorithms.*: ', True), ('具体算法实现，如 Algorithms.Aes、Algorithms.SM2', False)])
add_rich_para([('CryptoTool.Algorithm.Interfaces: ', True), ('接口契约定义', False)])
add_rich_para([('CryptoTool.Algorithm.Enums: ', True), ('配置枚举', False)])
add_rich_para([('CryptoTool.Algorithm.Utils: ', True), ('通用与业务工具', False)])
add_rich_para([('CryptoTool.Algorithm.Factory: ', True), ('工厂创建', False)])
add_rich_para([('CryptoTool.Algorithm.Exceptions: ', True), ('异常定义', False)])

# ===== 8. 依赖关系 =====
doc.add_heading('8. 技术栈与依赖关系', level=1)
add_table(
    ['依赖包', '版本', '用途'],
    [
        ['BouncyCastle.Cryptography', '2.6.2', '核心密码学库：ECDSA/NIST曲线、SM2/SM3/SM4国密、RSA/AES/DES底层引擎'],
        ['Newtonsoft.Json', '13.0.3', 'JSON序列化：医保报文格式化、密钥导出JSON格式'],
        ['System.Security.Cryptography.Algorithms', '4.3.1', '.NET原生加密基类（MD5等依赖）'],
        ['System.Security.Cryptography.Cng', '4.3.0', 'Windows Cryptography Next Generation 支持'],
        ['System.Security.Cryptography.OpenSsl', '4.3.0', 'Linux/macOS OpenSSL 原生加密支持'],
    ]
)

# ===== 9. 总结 =====
doc.add_heading('9. 总结', level=1)
add_para('CryptoTool.Algorithm 是一个设计规范、层次清晰的 .NET 加密类库。其核心优势：')
add_rich_para([('统一入口: ', True), ('CryptoTool.cs 静态门面提供一行式调用体验', False)])
add_rich_para([('接口驱动: ', True), ('所有算法实现标准化接口，支持运行时策略切换', False)])
add_rich_para([('按算法归类: ', True), ('每个算法独立子目录（AES/DES/ECDSA/RSA/SM2/SM3/SM4/MD5），职责清晰', False)])
add_rich_para([('国密全覆盖: ', True), ('SM2/SM3/SM4 完整实现，支持 C1C2C3/C1C3C2 密文格式转换', False)])
add_rich_para([('企业级工具: ', True), ('医保报文加解密、阿里云CSB网关签名等业务工具开箱即用', False)])
add_rich_para([('国际化: ', True), ('同时支持国际标准（RSA/AES/ECDSA）和中国国密标准（SM系列）', False)])

# 页脚
doc.add_paragraph()
add_para(f'文档生成日期: {datetime.date.today().isoformat()}', size=9, color=RGBColor(0x99, 0x99, 0x99))

# ===== 保存 =====
output_path = r'E:\Users\Documents\GitHub\CryptoTool\docs\CryptoTool.Algorithm_Analysis.docx'
doc.save(output_path)
print(f'Document saved: {output_path}')
