using Discord;
using Discord.Net.Rest;
using Discord.WebSocket;

namespace SevenToWinBackend.Library.Core
{
    public class DiscordClient : DiscordSocketClient
    {
        private readonly ILogger<DiscordClient> _logger;
        private static DiscordSocketConfig config = new DiscordSocketConfig()
        {
            RestClientProvider = DefaultRestClientProvider.Create(useProxy: true),
            LogLevel = LogSeverity.Verbose,
            GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
        };

        public DiscordClient(ILogger<DiscordClient> logger) : base(config)
        {
            _logger = logger;
        }

        public override async Task StopAsync()
        {
            _logger.LogInformation("Robot is stopping......");
            await base.StopAsync();
            _logger.LogInformation("Robot has stopped running.");
        }
    }
}
