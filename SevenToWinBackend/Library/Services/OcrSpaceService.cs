/****************************************
 * OcrSpace是图片识别的服务提供商 https://ocr.space/
 ***************************************/

using System.Text.Json;
using SevenToWinBackend.Library.Ocr;
using SevenToWinBackend.Library.Utils;

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

        public OcrSpaceService(OptionSettings option, IHttpClientFactory httpClientFactory,
            ILogger<OcrSpaceService> logger)
        {
            _option = option;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
        }

        /// <summary>
        /// 解析JSON，解析失败将抛出异常
        /// </summary>
        private OcrResponse TryParse(string json)
        {
            var responseWithError = JsonHelper.TryParse<OcrResponseWithError>(json);
            if (responseWithError != null)
            {
                return responseWithError;
            }

            var response = JsonHelper.TryParse<OcrResponse>(json);
            if (response == null)
            {
                _logger.LogError($"JSON解析错误：${json}");
                throw new Exception("JSON解析失败");
            }

            return response;
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
                _logger.LogInformation(jsonString);
                var response = TryParse(jsonString);
                if (response.IsErroredOnProcessing)
                {
                    var responseWithError = response as OcrResponseWithError;
                    if (responseWithError != null && responseWithError.ErrorMessage.Count > 0)
                    {
                        throw new Exception("图片识别错误："+responseWithError.ErrorMessage.First());
                    }
                    throw new Exception("图片识别错误：" + response.ErrorDetails);
                }

                return response;
            }
            catch (Exception e)
            {
                _logger.LogError(e.ToString());
                throw;
            }
        }
    }
}