using SevenToWinBackend.Library.Image;
using SixLabors.ImageSharp;
using ImageSharp = SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 图片服务
    /// </summary>
    public class ImageService
    {
        private readonly IWebHostEnvironment _env;
        private readonly IHttpClientFactory _httpClientFactory;
        private const int MaxWidth = 1500;
        private const int MaxHeight = 3000;

        public ImageService(IWebHostEnvironment env, IHttpClientFactory httpClientFactory)
        {
            _env = env;
            _httpClientFactory = httpClientFactory;
        }

        /// <summary>
        /// 下载并调整文件尺寸
        /// </summary>
        public async Task<FileInfo> DownloadAndResize(ImageInfo imageInfo)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var fileResponse = await httpClient.GetAsync(imageInfo.Url);
            var bytes = await fileResponse.Content.ReadAsByteArrayAsync();
            using var image = ImageSharp.Image.Load(bytes);
            // 调整文件尺寸
            if (imageInfo.Size.Height > MaxHeight || imageInfo.Size.Width > MaxWidth)
            {
                var newSize = ImageSize.GetSameRateSize(imageInfo.Size, new ImageSize(MaxWidth, MaxHeight));
                image.Mutate(x => x.Resize(newSize.Width, newSize.Height, KnownResamplers.Lanczos3));
            }
            // 保存文件
            var directoryPath = Path.Combine(_env.WebRootPath, "images");
            if (!Directory.Exists(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }
            var newFilePath = Path.Combine(directoryPath, $"{Guid.NewGuid()}.jpg");
            var newFile = new FileInfo(newFilePath);
            await image.SaveAsJpegAsync(newFile.FullName);
            return newFile;
        }
    }
}