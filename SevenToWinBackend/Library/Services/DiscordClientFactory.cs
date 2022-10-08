using Discord;
using Discord.Net.Rest;
using Discord.WebSocket;

namespace SevenToWinBackend.Library.Services
{
    public class DiscordClientFactory
    {
        private readonly OptionSettings _settings;

        public DiscordClientFactory(OptionSettings settings)
        {
            _settings = settings;
        }

        public DiscordSocketClient Create()
        {
            DiscordSocketConfig config = new DiscordSocketConfig()
            {
                RestClientProvider = DefaultRestClientProvider.Create(useProxy: _settings.EnableDiscordProxy),
                LogLevel = LogSeverity.Verbose,
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.MessageContent
            };
            return new DiscordSocketClient(config);
        }
    }
}
