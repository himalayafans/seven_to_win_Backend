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
        private readonly FileService fileService;
        private readonly ImageService imageService;

        public MessageService(FileService fileService, ImageService imageService)
        {
            this.fileService = fileService;
            this.imageService = imageService;
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
            var url = imageService.GetImageUrl(message);
            if (String.IsNullOrWhiteSpace(url))
            {
                // 如果消息不包含图片，则忽略
                return;
            }
            FileInfo file = await fileService.DownloadFile(url);
            await message.ReplyAsync(file.FullName);
        }
    }
}
