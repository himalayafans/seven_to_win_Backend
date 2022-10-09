using SevenToWinBackend.Library.Extensions;

namespace SevenToWinBackend.Library.Strategy
{
    /// <summary>
    /// LOGO检查策略
    /// </summary>
    public class LogoStrategyHandler : BaseStrategyHandler
    {
        private bool IsEnabled(PlayResult result)
        {
            string text = result.OcrResponse.ParsedResults.First().ParsedText;
            // 偶尔会被识别为 HIMALA AYA  EEXCHANGE
            // return text.ContainsIgnoreCase("himalaya") && text.ContainsIgnoreCase("exchange");
            return text.ContainsIgnoreCase("himala") && text.ContainsIgnoreCase("exchange");
        }
        public override void Handle(PlayResult result)
        {
            if (IsEnabled(result))
            {
                this.Successor?.Handle(result);
            }
            else
            {
                result.AddMessage("截图没有喜交所的LOGO或网址");
            }
        }
    }
}
