namespace SevenToWinBackend.Library.Services
{
    /// <summary>
    /// 选项设置
    /// </summary>
    public class OptionSettings
    {
        /// <summary>
        /// Discord Token
        /// </summary>
        public string DiscordToken { get; }
        /// <summary>
        /// 是否启用机器人
        /// </summary>
        public bool BotEnabled { get; }
        public string OcrSpaceKey { get; }

        public OptionSettings(IConfiguration config)
        {
            this.DiscordToken = config.GetValue<string>("Discord:Token");
            this.OcrSpaceKey = config.GetValue<string>("OcrSpaceKey");
            BotEnabled = true;
        }
    }
}