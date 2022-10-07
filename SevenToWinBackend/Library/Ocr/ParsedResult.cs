namespace SevenToWinBackend.Library.Ocr;

public class ParsedResult
{
    public TextOverlay TextOverlay { get; set; }
    public string TextOrientation { get; set; }
    public int FileParseExitCode { get; set; }
    public string ParsedText { get; set; }
    public string ErrorMessage { get; set; }
    public string ErrorDetails { get; set; }
}