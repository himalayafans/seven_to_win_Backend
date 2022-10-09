namespace SevenToWinBackend.Library.Image
{
    public class ImageInfo
    {
        public string Url { get;}
        public ImageSize Size { get;}

        public ImageInfo(string url, ImageSize size)
        {
            Url = url;
            Size = size;
        }
    }
}
