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
    public List<string> Tips { get; }

    /// <summary>
    /// 获取的总分数
    /// </summary>
    public int TotalScore { get; set; }

    public PlayResult(OcrResponse ocrResponse, SocketUserMessage socketUserMessage)
    {
        OcrResponse = ocrResponse ?? throw new ArgumentNullException(nameof(ocrResponse));
        SocketUserMessage = socketUserMessage ?? throw new ArgumentNullException(nameof(socketUserMessage));
        Tips = new List<string>();
        TotalScore = 0;
    }

    public override string ToString()
    {
        var msg = string.Join(';', Tips);
        return $"本次获得玉米总数:{TotalScore}   {msg}";
    }
}