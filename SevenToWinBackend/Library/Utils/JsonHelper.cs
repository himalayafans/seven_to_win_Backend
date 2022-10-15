using System.Text.Json;

namespace SevenToWinBackend.Library.Utils
{
    public static class JsonHelper
    {
	    /// <summary>
	    /// 尝试解析JSON，解析失败不抛出异常，而是返回默认值
	    /// </summary>
        public static T? TryParse<T>(string json)
        {
			try
			{
                return JsonSerializer.Deserialize<T>(json);
            }
			catch (Exception)
			{
                return default;
			}
        }
    }
}
