using System.Text;
using Discord.WebSocket;
using SevenToWinBackend.Library.Ocr;

namespace SevenToWinBackend.Library.Strategy;

/// <summary>
/// 游戏结果
/// </summary>
public class PlayResult
{
    /// <summary>
    /// 图片解析结果
    /// </summary>
    public OcrResponse OcrResponse { get; }

    /// <summary>
    /// 用户发送的Discord消息
    /// </summary>
    public SocketUserMessage SocketUserMessage { get; }

    /// <summary>
    /// 提示信息
    /// </summary>
    private StringBuilder Tips { get; }

    /// <summary>
    /// 获取的总分数
    /// </summary>
    public int TotalScore { get; set; }

    /// <summary>
    /// 价格中7出现的次数
    /// </summary>
    public int SevenTimes { get; set; } = 0;

    public PlayResult(OcrResponse ocrResponse, SocketUserMessage socketUserMessage)
    {
        OcrResponse = ocrResponse ?? throw new ArgumentNullException(nameof(ocrResponse));
        SocketUserMessage = socketUserMessage ?? throw new ArgumentNullException(nameof(socketUserMessage));
        Tips = new StringBuilder();
        TotalScore = 0;
    }

    /// <summary>
    /// 增加消息
    /// </summary>
    public void AddMessage(string msg)
    {
        if (!string.IsNullOrWhiteSpace(msg))
        {
            this.Tips.AppendLine(msg.Trim());
        }
    }
    
    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendLine($"本次获得玉米总数:{TotalScore}");
        sb.Append(Tips);
        sb.AppendLine("您的玉米总数请咨询“出7致胜”管理员");
        if (this.SevenTimes > 0)
        {
            //礼花字符串
            var fire = new StringBuilder();
            for (int i = 0; i < this.SevenTimes; i++)
            {
                fire.Append("🎉");
            }
            sb.Append($"感谢您关注喜马拉雅交易所，用喜元、玩喜币、跟着喜支付一起飞…… {fire.ToString()}");
        }
        else
        {
            sb.Append($"感谢您关注喜马拉雅交易所，用喜元、玩喜币、跟着喜支付一起飞……");           
        }       
        var result = sb.ToString();
        return result;
    }
}