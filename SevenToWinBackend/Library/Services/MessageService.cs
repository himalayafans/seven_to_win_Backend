using Discord;
using Discord.WebSocket;
using SevenToWinBackend.Library.Image;
using SevenToWinBackend.Library.Interfaces;
using SevenToWinBackend.Library.Strategy;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 策略服务
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly FileService _fileService;
        private readonly OcrSpaceService _ocrSpaceService;
        private readonly ImageService _imageService;
        private readonly ILogger<MessageService> _logger;
        private static readonly List<string> _imageTypes = new List<string>()
        {
            "image/jpeg",
            //"image/gif",
            "image/png",
            //"image/bmp"
        };

        public MessageService(FileService fileService, OcrSpaceService ocrSpaceService, ImageService imageService, ILogger<MessageService> logger)
        {
            this._fileService = fileService;
            _ocrSpaceService = ocrSpaceService;
            _imageService = imageService;
            this._logger = logger;
        }

        /// <summary>
        /// 获取消息中的图片，若不存在则返回null
        /// </summary>
        private static ImageInfo? GetImage(SocketMessage message)
        {
            var images = message.Attachments.Where(p => _imageTypes.Contains(p.ContentType)).ToList();
            if (images.Count == 0)
            {
                return null;
            }
            if (images.Count > 1)
            {
                throw new Exception("本次活动参与无效，一次只能上传一张图片");
            }
            var file = message.Attachments.First();
            return new ImageInfo(file.Url, new ImageSize(file.Width.GetValueOrDefault(), file.Height.GetValueOrDefault()));
        }
        private void WriteLog(SocketMessage msg,string catalog)
        {
            _logger.LogInformation($"[{catalog}]，[作者]:{msg.Author.Username},[时间]:{msg.CreatedAt},[消息内容]:{msg.CleanContent}");
        }

        public async Task Handle(SocketMessage arg)
        {
            this.WriteLog(arg, "接收到消息");
            var message = arg as SocketUserMessage;
            // 若是系统消息，不是用户发的，则忽略
            if (message == null)
            {
                this.WriteLog(arg, "忽略系统消息");
                return;
            }
            // 如果消息是机器人发送，则忽略
            if (message.Author.IsBot)
            {
                this.WriteLog(arg, "忽略机器人消息");
                return;
            }
            try
            {
                var imageInfo = GetImage(message);
                if(imageInfo == null)
                {
                    this.WriteLog(arg, "忽略未包含图片消息");
                    // 若消息中不包含图片，则忽略
                    return;
                }
                var file = await _imageService.DownloadAndResize(imageInfo);
                var ocrResponse = await _ocrSpaceService.Parse(file);
                var game = new SevenGame(ocrResponse, message);
                var playResult = game.Play();
                await message.ReplyAsync(playResult.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,"异常");
                await message.ReplyAsync(ex.Message);
            }
        }
    }
}
