using Discord;
using Discord.WebSocket;
using SevenToWinBackend.Library.Interfaces;

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 策略服务
    /// </summary>
    public class MessageService : IMessageService
    {
        private readonly FileService _fileService;
        private readonly OcrSpaceService _ocrSpaceService;
        private readonly List<string> _imageTypes = new List<string>()
        {
            "image/jpeg",
            "image/gif",
            "image/png",
            "image/bmp"
        };

        public MessageService(FileService fileService, OcrSpaceService ocrSpaceService)
        {
            this._fileService = fileService;
            _ocrSpaceService = ocrSpaceService;
        }
        
        /// <summary>
        /// 获取消息中图片的URL，若不存在则返回null
        /// </summary>
        private string? GetImageUrl(SocketMessage message)
        {
            if (message.Attachments.Count == 0)
            {
                return null;
            }
            var file = message.Attachments.First();
            if (file == null)
            {
                return null;
            }
            return _imageTypes.Contains(file.ContentType) ? file.Url : null;
        }

        public async Task Handle(SocketMessage arg)
        {
            var message = arg as SocketUserMessage;
            // 若是系统消息，不是用户发的，则忽略
            if (message == null)
            {
                return;
            }
            // 如果消息是机器人发送，则忽略
            if (message.Author.IsBot)
            {
                return;
            }
            var url = GetImageUrl(message);
            if (string.IsNullOrWhiteSpace(url))
            {
                // 如果消息不包含图片，则忽略
                return;
            }
            var file = await _fileService.DownloadFile(url);
            await _ocrSpaceService.Parse(file);
            await message.ReplyAsync("图片解析成功");
        }
    }
}
