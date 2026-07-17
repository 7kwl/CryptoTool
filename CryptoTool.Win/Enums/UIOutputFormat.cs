namespace CryptoTool.Win.Enums
{

    /// <summary>
    /// UI层输出格式枚举
    /// </summary>
    public enum UIOutputFormat
    {
        /// <summary>
        /// UTF-8字符串格式
        /// </summary>
        UTF8,
        /// <summary>
        /// Base64编码格式
        /// </summary>
        Base64,
        /// <summary>
        /// 十六进制字符串格式（大写）
        /// </summary>
        HexUpper,
        /// <summary>
        /// 十六进制字符串格式（小写）
        /// </summary>
        HexLower,
        /// <summary>
        /// 十六进制字符串格式（兼容旧版，小写）
        /// </summary>
        Hex,
        /// <summary>
        /// PEM格式
        /// </summary>
        PEM
    }
}
