using Discord.WebSocket;
using System.Net;
using Tesseract;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class ImageService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        //private readonly IWebHostEnvironment env;
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
        /// <summary>
        /// 将图片文件转换成文字
        /// </summary>
        public string? Ocr(FileInfo imageFile)
        {
            var path = Path.Combine(AppContext.BaseDirectory, "tessdata");
            imageFile.OpenRead();
            FileStream fs = imageFile.OpenRead();
            var ms = new MemoryStream();
            fs.CopyTo(ms);
            fs.Close();
            byte[] fileBytes = ms.ToArray();
            ms.Close();
            using (var engine = new TesseractEngine(path, "eng", EngineMode.Default))
            {
                using (var img = Pix.LoadFromMemory(fileBytes))
                {
                    using (var page = engine.Process(img))
                    {
                        var txt = page.GetText();
                        return txt;
                    }
                }
            }
        }
    }
}