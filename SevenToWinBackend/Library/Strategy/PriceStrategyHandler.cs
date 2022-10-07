namespace SevenToWinBackend.Library.Strategy;

/// <summary>
/// 价格策略处理器
/// </summary>
public class PriceStrategyHandler : BaseStrategyHandler
{
    /// <summary>
    /// 获取奖励玉米
    /// </summary>
    private static int GetScore(int count)
    {
        return count switch
        {
            1 => 77,
            2 => 777,
            3 => 7777,
            4 => 77777,
            5 => 777777,
            6 => 7777777,
            _ => throw new Exception($"规则不支持价格含{count}个7")
        };
    }

    public override void Handle(PlayResult result)
    {
        var parsedResult = result.OcrResponse.ParsedResults.First();
        // 从截图中获取喜币价格，价格与HCN/HDO文字的左边距相同
        double? titleLeft = null;
        double? price = null;
        foreach (var line in parsedResult.TextOverlay.Lines)
        {
            if (line.LineText == "HCN/HDO" && !titleLeft.HasValue)
            {
                titleLeft = line.Words.First().Left;
            }

            if (titleLeft.HasValue && !price.HasValue && Math.Abs(titleLeft.Value - line.Words.First().Left) < 0.0005 && line.LineText != "HCN/HDO")
            {
                price = Convert.ToDouble(line.LineText);
            }
        }
        if (titleLeft == null)
        {
            result.Tips.Add("截图没有包含HCN/HDO文字");
        }
        else if (price == null)
        {
            result.Tips.Add("截图没有包含喜币价格");
        }
        else
        {
            var count = price?.ToString().ToCharArray().Count(p => p == '7');
            // 如果价格包含7
            if (count >= 1)
            {
                var score = GetScore(count.Value);
                result.TotalScore = result.TotalScore + score;
                result.Tips.Add($"价格包含{count}个7，获得{score}个玉米");
            }
            this.Successor?.Handle(result);
        }
    }
}