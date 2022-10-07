namespace SevenToWinBackend.Library.Ocr;

public class Line
{
    public string LineText { get; set; }
    public List<Word> Words { get; set; }
    public double MaxHeight { get; set; }
    public double MinTop { get; set; }
}