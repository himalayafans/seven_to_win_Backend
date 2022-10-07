using Discord;
using Discord.Commands;
using Discord.Rest;
using Discord.WebSocket;
using Serilog;
using SevenToWinBackend.Library.Core;
using SevenToWinBackend.Library.Services;

namespace SevenToWinBackend.Library.Extensions
{
    public static class WebHostBuilderExtensions
    {
        public static WebApplicationBuilder AddDiscordBot(this WebApplicationBuilder builder)
        {
            builder.Services.AddSingleton<OptionSettings>();
            builder.Services.AddSingleton<DiscordClient>();
            builder.Services.AddSingleton<CommandService>();
            builder.Services.AddSingleton<DiscordService>();
            builder.Services.AddSingleton<MessageService>();
            builder.Services.AddSingleton<FileService>();
            builder.Services.AddSingleton<OcrSpaceService>();
            builder.Services.AddHostedService(provider => provider.GetRequiredService<DiscordService>());
            return builder;
        }
        /// <summary>
        /// 增加日志服务
        /// </summary>
        public static void AddLogger(this WebApplicationBuilder builder)
        {
            var logger = new LoggerConfiguration().ReadFrom.Configuration(builder.Configuration).Enrich.FromLogContext().CreateLogger();
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog(logger);
        }
    }
}