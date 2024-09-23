namespace UserManagement.Infrastructure.Services.Configurations
{
    public class JwtSettings
    {
        public const string SectioName = "JwtSettings";
        public static string AppSecret { get; set; }
        public static int AppExpireMinutes { get; set; }
        public static string AppIssuer { get; set; }
        public static string AppAudience { get; set; }
        public static string UserSecret { get; set; }
        public static int UserExpireSeconds { get; set; }
        public static string UserIssuer { get; set; }
        public static string UserAudience { get; set; }
    }
}