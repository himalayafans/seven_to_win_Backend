using Discord.WebSocket;
using SevenToWinBackend.Library.Ocr;

namespace SevenToWinBackend.Library.Strategy;

/// <summary>
/// 出7制胜游戏
/// </summary>
public class SevenGame
{
    /// <summary>
    /// 图片解析结果
    /// </summary>
    private OcrResponse OcrResponse { get; }
    /// <summary>
    /// 用户发送的Discord消息
    /// </summary>
    private SocketUserMessage SocketUserMessage { get; }

    public SevenGame(OcrResponse ocrResponse, SocketUserMessage socketUserMessage)
    {
        OcrResponse = ocrResponse;
        SocketUserMessage = socketUserMessage;
    }
    public PlayResult Play()
    {
        var result = new PlayResult(OcrResponse, SocketUserMessage);
        var h0 = new EmptyTextStrategyHandler();
        var h1 = new LogoStrategyHandler();
        h0.SetSuccessor(h1);
        var h2 = new PostTimeCheckStrategyHandler();
        h1.SetSuccessor(h2);
        var h3 = new TimeContainSevenStrategyHandler();
        h2.SetSuccessor(h3);
        var h4 = new PriceStrategyHandler();
        h3.SetSuccessor(h4);
        h1.Handle(result);
        return result;

        // 以下是测试代码
        //var result = new PlayResult(OcrResponse, SocketUserMessage);
        //var h = new PriceStrategyHandler();
        //h.Handle(result);
        //return result;
    }
}