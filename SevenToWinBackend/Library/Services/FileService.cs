using System;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 文件服务
    /// </summary>
    public class FileService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public FileService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 下载文件，并保存为临时文件
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>返回临时文件的磁盘路径</returns>
        public async Task<FileInfo> DownloadFile(string url)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var fileResponse = await httpClient.GetAsync(url);
            byte[] bytes = await fileResponse.Content.ReadAsByteArrayAsync();
            var filePath = Path.GetTempFileName();
            using (var stream = File.Create(filePath))
            {
                await stream.WriteAsync(bytes);
            }
            return new FileInfo(filePath);
        }
    }
}
