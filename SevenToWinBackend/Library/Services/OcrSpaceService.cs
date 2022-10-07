﻿/****************************************
 * OcrSpace是图片识别的服务提供商 https://ocr.space/
 ***************************************/

using System.Text.Json;
using SevenToWinBackend.Library.Ocr;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// OcrSpace服务
    /// </summary>
    public class OcrSpaceService
    {
        private readonly OptionSettings _option;
        private const string Api = "https://api.ocr.space/parse/image";
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<OcrSpaceService> _logger;

        public OcrSpaceService(OptionSettings option, IHttpClientFactory httpClientFactory, ILogger<OcrSpaceService> logger)
        {
            _option = option;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        public async Task<OcrResponse> Parse(FileInfo file)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient();
                _logger.LogInformation("Reading file" + file.FullName);
                var bytes = await File.ReadAllBytesAsync(file.FullName);
                var base64 = "data:image/jpeg;base64," + Convert.ToBase64String(bytes);
                var data = new Dictionary<string, string>
                {
                    { "language", "eng" },
                    { "isOverlayRequired", "true" },
                    { "scale", "true" },
                    { "OCREngine", "2" },
                    { "apikey", _option.OcrSpaceKey },
                    { "base64Image", base64 }
                };
                _logger.LogInformation($"Start analyzing image files {file.FullName}...");
                var res = await httpClient.PostAsync(Api, new FormUrlEncodedContent(data));
                var jsonString = await res.Content.ReadAsStringAsync();
                _logger.LogInformation($"Finish analyzing the image file {file.FullName}");
                return JsonSerializer.Deserialize<OcrResponse>(jsonString)!;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}