namespace SevenToWinBackend.Library.Ocr;

public class OcrResponse
{
    public List<ParsedResult> ParsedResults { get; set; } = new List<ParsedResult>();
    public bool IsErroredOnProcessing { get; set; }
    /// <summary>
    /// 错误详情
    /// </summary>
    public string? ErrorDetails { get; set; }
}