using System.Globalization;
using SevenToWinBackend.Library.Extensions;
using SevenToWinBackend.Library.Ocr;

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
        Word? titleWord = null;
        Word? priceWord = null;
        var titleLine = parsedResult.TextOverlay.Lines.Find(line =>
        {
            return line.Words.Exists(item =>
            {
                var text = item.WordText;
                // 考虑到最后一个O可能被识别为数字0，或无法识别的问题
                var flag = text.StartsWith("HCN/HD") && text.Length is 6 or 7;
                if (flag)
                {
                    titleWord = item;
                }

                return flag;
            });
        });
        if (titleLine == null || titleWord == null)
        {
            result.AddMessage("截图没有包含HCN/HDO文字");
            return;
        }

        var priceLine = parsedResult.TextOverlay.Lines.Find(line =>
        {
            var item = line.Words.Find(word =>
            {
                var text = word.WordText;
                var flag = text.IsDecimal() && Math.Abs(titleWord.Left - word.Left) < 5;
                if (flag)
                {
                    priceWord = word;
                }

                return flag;
            });
            return item != null;
        });
        if (priceLine == null || priceWord == null)
        {
            result.AddMessage("截图没有包含喜币价格");
            return;
        }
        var price = Convert.ToDecimal(priceWord.WordText);
        var priceText = price.ToString(CultureInfo.CurrentCulture);
        var count = priceText.ToCharArray().Count(p => p == '7');
        // 如果价格包含7
        if (count >= 1)
        {
            var score = GetScore(count);
            result.TotalScore = result.TotalScore + score;
            result.AddMessage($"喜币价格{price}包含{count}个7，获得{score}个玉米");
        }
        else
        {
            result.AddMessage($"喜币价格{price},没有包含7");
        }

        this.Successor?.Handle(result);
    }
}