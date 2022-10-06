using System;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class FileService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IWebHostEnvironment _env;

        public FileService(IHttpClientFactory httpClientFactory, IWebHostEnvironment env)
        {
            _httpClientFactory = httpClientFactory;
            _env = env;
        }

        /// <summary>
        /// 下载文件，并保存为临时文件
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>返回临时文件的磁盘路径</returns>
        public async Task<FileInfo> DownloadFile(string url)
        {
            var ext = Path.GetExtension(url);
            var httpClient = _httpClientFactory.CreateClient();
            var fileResponse = await httpClient.GetAsync(url);
            var bytes = await fileResponse.Content.ReadAsByteArrayAsync();
            var filePath = Path.Combine(_env.WebRootPath, "images", $"{Guid.NewGuid()}{ext}");
            await using (var stream = File.Create(filePath))
            {
                await stream.WriteAsync(bytes);
            }
            return new FileInfo(filePath);
        }
    }
}
