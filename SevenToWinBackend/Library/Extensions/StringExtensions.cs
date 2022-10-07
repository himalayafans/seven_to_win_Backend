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
    }
}
