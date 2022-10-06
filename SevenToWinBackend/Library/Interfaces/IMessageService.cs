using Discord.WebSocket;

namespace SevenToWinBackend.Library.Interfaces
{
    /// <summary>
    /// 消息服务接口
    /// </summary>
    public interface IMessageService
    {
        /// <summary>
        /// 处理接收到的Discord消息
        /// </summary>
        Task Handle(SocketMessage message);
    }
}
