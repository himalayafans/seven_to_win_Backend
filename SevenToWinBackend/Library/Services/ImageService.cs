using Discord.WebSocket;
using System.Net;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class ImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly List<string> _imageTypes = new List<string>()
        {
            "image/jpeg",
            "image/gif",
            "image/png",
            "image/bmp"
        };

        public ImageService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }
        /// <summary>
        /// 获取图片URL，若不存在则返回null
        /// </summary>
        public string? GetImageUrl(SocketMessage message)
        {
            if (message == null)
            {
                return null;
            }
            if (message.Attachments.Count == 0)
            {
                return null;
            }
            var file = message.Attachments.First();
            if (file == null)
            {
                return null;
            }
            if (_imageTypes.Contains(file.ContentType))
            {
                return file.Url;
            }
            else
            {
                return null;
            }
        }
    }
}