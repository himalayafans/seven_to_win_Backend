namespace SevenToWinBackend.Library.Extensions
{
    public static class StringExtensions
    {
        /// <summary>
        /// 判断是否包含子字符串（忽略大小写）
        /// </summary>
        public static bool ContainsIgnoreCase(this string source, string childString)
        {
            return source?.IndexOf(childString, StringComparison.OrdinalIgnoreCase) >= 0;
        }

        /// <summary>
        /// 检查一个字符串是否是数字
        /// </summary>
        public static bool IsDecimal(this string source)
        {
            // https://www.c-sharpcorner.com/blogs/c-sharp-hidden-gems-sharp1-discards-variable
            return decimal.TryParse(source, out _);
        }
    }
}
