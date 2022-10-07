/****************************************
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

        public OcrSpaceService(OptionSettings option, IHttpClientFactory httpClientFactory)
        {
            _option = option;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<OcrResponse> Parse(FileInfo file)
        {
            var httpClient = _httpClientFactory.CreateClient();
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
            var res = await httpClient.PostAsync(Api, new FormUrlEncodedContent(data));
            var jsonString = await res.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<OcrResponse>(jsonString)!;
        }
    }
}