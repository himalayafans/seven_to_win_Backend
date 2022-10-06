namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// Discord配置
    /// </summary>
    public class DiscordSettings
    {      
        /// <summary>
        /// 授权Token
        /// </summary>
        public string Token { get; }
        /// <summary>
        /// 是否启用机器人
        /// </summary>
        public bool BotEnabled { get; set; }
        public DiscordSettings(IConfiguration config)
        {
            Token = config.GetValue<string>("Discord:Token");
            BotEnabled = true;
        }
    }
}
