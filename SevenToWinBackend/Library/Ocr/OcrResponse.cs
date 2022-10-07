namespace SevenToWinBackend.Library.Ocr;

public class OcrResponse
{
    public List<ParsedResult> ParsedResults { get; set; }
    public int OCRExitCode { get; set; }
    public bool IsErroredOnProcessing { get; set; }
    public string ProcessingTimeInMilliseconds { get; set; }
    public string SearchablePDFURL { get; set; }
}