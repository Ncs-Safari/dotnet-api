namespace UserManagement.Application.Configuration
{
    public class DirectorySetting
    {
        public const string SectioName = "DirectorySetting";
        public static string BasePath { get; set; }
        public static string BaseUrl { get; set; }
        public static string BaseUrlProxy { get; set; }
        public static string PathUrl { get; set; }

        public static string FileUser { get; set; }
        public static string UrlFileUser { get; set; }
        public static string PathFileUser { get; set; }

        public static string FileData { get; set; }
        public static string UrlFileData { get; set; }
        public static string PathFileData { get; set; }

        public static string FileApp { get; set; }
        public static string UrlFileApp { get; set; }
        public static string PathFileApp { get; set; }
    }
}


