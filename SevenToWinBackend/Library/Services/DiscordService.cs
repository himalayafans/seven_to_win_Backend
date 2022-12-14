using Discord;
using Discord.Commands;
using Discord.WebSocket;
using System.Reflection;

/****************************************
 * IHostedServices接口用于定义.Net中的后台任务或计划作业
 * 如果不使用该接口，而是使用多线程，则主应用程序退出后，线程并没有被杀死，仍然在运行
 * 当在.Net中注册IHostedService服务后，.Net将分别在应用程序启动和停止期间调用该接口的实现方法：StartAsync()
 ***************************************/

namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// Discord后台服务
    /// </summary>
    public class DiscordService : IHostedService, IDisposable
    {
        private readonly DiscordSocketClient _client;
        /// <summary>
        /// 该服务提供了一套框架，用于构建discord命令
        /// </summary>
        private readonly CommandService _commandService;
        private readonly IServiceProvider _services;
        private readonly OptionSettings _settings;
        private readonly ILogger<DiscordService> _logger;
        private readonly MessageService _messageService;

        public DiscordService(DiscordClientFactory factory, CommandService commandService, IServiceProvider services, OptionSettings settings, ILogger<DiscordService> logger, MessageService messageService)
        {
            _client = factory.Create();
            _commandService = commandService;
            _services = services;
            _settings = settings;
            _logger = logger;
            _messageService = messageService;
        }

        async Task IHostedService.StartAsync(CancellationToken cancellationToken)
        {
            _client.Ready += _client_Ready;
            _client.MessageReceived += _client_MessageReceived;
            _client.Log += _log;
            _commandService.Log += _log;
            _commandService.CommandExecuted += _commandService_CommandExecuted;
            // 查找并加载程序集中所有继承自ModuleBase的命令类
            await _commandService.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            // 使用配置文件中的token登录discord
            await _client.LoginAsync(TokenType.Bot, _settings.DiscordToken);
            // 启动机器人
            await _client.StartAsync();
        }
        /// <summary>
        /// 当一个命令被执行后触发，无论该命令是否成功或失败
        /// </summary>
        private Task _commandService_CommandExecuted(Optional<CommandInfo> arg1, ICommandContext arg2, Discord.Commands.IResult arg3)
        {
            return Task.CompletedTask;
        }

        /// <summary>
        /// 处理日志事件
        /// </summary>
        private Task _log(Discord.LogMessage arg)
        {
            _logger.LogInformation(arg.ToString());
            return Task.CompletedTask;
        }

        /// <summary>
        /// 收到消息时会触发该事件。
        /// 该消息可能是系统消息（SocketSystemMessage）或用户消息（SocketUserMessage）
        /// </summary>
        private async Task _client_MessageReceived(SocketMessage arg)
        {
            await this._messageService.Handle(arg);
        }
        /// <summary>
        /// 当discord服务器数据下载完成时触发。
        /// 此时机器人已经成功连接到discord服务器，并可以开始工作
        /// </summary>
        private Task _client_Ready()
        {
            return Task.CompletedTask;
            // 设置为在线状态
            //await _client.SetStatusAsync(UserStatus.Online);
        }

        async Task IHostedService.StopAsync(CancellationToken cancellationToken)
        {
            await _client.SetGameAsync(null);
            await _client.StopAsync();
            _client.Log -= _log;
            _client.Ready -= _client_Ready;
            _client.MessageReceived -= _client_MessageReceived;
            _commandService.Log -= _log;
            _commandService.CommandExecuted -= _commandService_CommandExecuted;
        }
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
