namespace SevenToWinBackend.Library.Strategy
{
    /// <summary>
    /// LOGO检查策略
    /// </summary>
    public class LogoStrategyHandler : BaseStrategyHandler
    {
        public override void Handle(PlayResult result)
        {
            var text = result.OcrResponse.ParsedResults.First().ParsedText;
            if(text.Contains("himalaya".ToUpper()) && text.Contains("exchange".ToUpper()))
            {
                this.Successor?.Handle(result);
            }
            else
            {
                result.Tips.Add("截图没有喜交所的LOGO");
            }
        }
    }
}
