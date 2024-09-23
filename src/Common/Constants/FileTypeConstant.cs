namespace Common.Constants
{
    public class FileTypeConstant
    {
        public static Dictionary<string, string> Dict = new()
        {
            {Image,"File Image" },
            {Audio,"File Audio" },
            {Video,"File Video" },
            {Pdf,"File Pdf" },
            {Word,"File Word" },
            {Sheet,"File Excel" },
        };
        public const string Image = "image";
        public const string Audio = "audio";
        public const string Video = "video";
        public const string Pdf = "pdf";
        public const string Word = "word";
        public const string Sheet = "sheet";
    }
}
