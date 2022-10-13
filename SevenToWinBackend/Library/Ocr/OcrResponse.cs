namespace SevenToWinBackend.Library.Ocr;

public class OcrResponse
{
    public List<ParsedResult> ParsedResults { get; set; } = new List<ParsedResult>();
    public bool IsErroredOnProcessing { get; set; }
    /// <summary>
    /// 错误详情(即便IsErroredOnProcessing=true时，有时也为空)
    /// </summary>
    public string? ErrorDetails { get; set; }
    /// <summary>
    /// 错误信息(即便IsErroredOnProcessing=true时，有时也为空)
    /// </summary>
    public List<string>? ErrorMessage { get; set; }
}