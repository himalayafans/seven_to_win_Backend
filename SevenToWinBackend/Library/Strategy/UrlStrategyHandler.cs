namespace SevenToWinBackend.Library.Strategy;

/// <summary>
/// URL策略处理器
/// </summary>
public class UrlStrategyHandler : BaseStrategyHandler
{
    private static bool IsEnabled(PlayResult result)
    {
        // 判断截图是否包含喜交所网址
        var text = result.OcrResponse.ParsedResults.First().ParsedText;
        // 忽略大小写查找喜交所的网址
        return text.IndexOf("himalaya.exchange", StringComparison.OrdinalIgnoreCase) > 0;
    }

    public override void Handle(PlayResult result)
    {
        // 如果不满足条件则短路
        if (IsEnabled(result))
        {
            this.Successor?.Handle(result);
        }
        else
        {
            result.Tips.Add("截图没有包含喜交所网址");
        }
    }
}