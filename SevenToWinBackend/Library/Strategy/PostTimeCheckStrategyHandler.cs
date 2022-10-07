namespace SevenToWinBackend.Library.Strategy;

/// <summary>
/// 发帖时间验证处理器
/// </summary>
public class PostTimeCheckStrategyHandler: BaseStrategyHandler
{
    /// <summary>
    /// 检查截图时间是否与Discord发帖时间一致（考虑到时区，只判断分和秒）
    /// </summary>
    private static bool IsTimeSame(PlayResult result)
    {
        // Discord发帖的分钟
        var minute = result.SocketUserMessage.CreatedAt.Minute;
        // Discord发帖的秒数
        var second = result.SocketUserMessage.CreatedAt.Second;
        var time1 = $"{minute}:{second}";
        // 补充前导0， 例如 3:59 -> 03:59
        var time2 = time1.PadLeft(5, '0');
        foreach (var parsedResult in result.OcrResponse.ParsedResults)
        {
            foreach (var line in parsedResult.TextOverlay.Lines)
            {
                if (line.LineText == time1)
                {
                    return true;
                }
            }
        }
        return false;
    }
    public override void Handle(PlayResult result)
    {
        // 如果发帖时间与截图时间一致，则继续执行，否则短路
        if (IsTimeSame(result))
        {
            this.Successor?.Handle(result);
        }
        else
        {
            result.Tips.Add("发帖时间与截图时间不一致");
        }
    }
}