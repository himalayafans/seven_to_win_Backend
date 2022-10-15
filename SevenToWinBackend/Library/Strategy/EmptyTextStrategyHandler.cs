namespace SevenToWinBackend.Library.Strategy
{
    /// <summary>
    /// 空文本检查策略
    /// </summary>
    public class EmptyTextStrategyHandler : BaseStrategyHandler
    {
        public override void Handle(PlayResult result)
        {
            var obj = result.OcrResponse.ParsedResults;
            if (obj == null || obj.Count == 0 || string.IsNullOrWhiteSpace(obj.First().ParsedText))
            {
                result.AddMessage("该截图没有包含任何文字");
            }
            else
            {
                this.Successor?.Handle(result);
            }
        }
    }
}
