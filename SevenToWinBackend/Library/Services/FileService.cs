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
        /// 下载文件，并保存为文件
        /// </summary>
        /// <param name="url">URL</param>
        /// <returns>返回文件的磁盘路径</returns>
        public async Task<FileInfo> DownloadFile(string url)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new Exception("待下载的图片URL不能为空");
            }

            var ext = Path.GetExtension(url);
            var httpClient = _httpClientFactory.CreateClient();
            var fileResponse = await httpClient.GetAsync(url);
            var bytes = await fileResponse.Content.ReadAsByteArrayAsync();
            var directoryPath = Path.Combine(_env.WebRootPath, "original");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var directory = new DirectoryInfo(directoryPath);
            var filePath = Path.Combine(directory.FullName, $"{Guid.NewGuid()}{ext}");
            await File.WriteAllBytesAsync(filePath, bytes);
            return new FileInfo(filePath);
        }
    }
}