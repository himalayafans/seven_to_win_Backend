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
            foreach (var parsedResult in result.OcrResponse.ParsedResults)
            {
                foreach (var line in parsedResult.TextOverlay.Lines)
                {
                    foreach (var word in line.Words)
                    {
                        var text = word.WordText;
                        if (text.ContainsIgnoreCase("himalaya") && text.ContainsIgnoreCase("exchange"))
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }
        public override void Handle(PlayResult result)
        {
            if (IsEnabled(result))
            {
                this.Successor?.Handle(result);
            }
            else
            {
                result.Tips.Add("截图没有喜交所的LOGO或网址");
            }
        }
    }
}
