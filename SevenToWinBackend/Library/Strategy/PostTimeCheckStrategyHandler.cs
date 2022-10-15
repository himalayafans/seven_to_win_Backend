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
        // 补充前导0， 例如 1 -> 01
        var minute2 = minute.ToString().PadLeft(2, '0');
        var text = result.OcrResponse.ParsedResults.First().ParsedText;
        // 只检查分钟，因为小时涉及到时区
        return text.Contains($":{minute2}");
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
            result.AddMessage("发帖时间与截图时间不一致");
        }
    }
}
