namespace UserManagement.Infrastructure.Services.Configurations
{
    public class SmtpSettings
    {
        public const string SectioName = "SmtpSettings";
        public static string Host { get; set; }
        public static int Port { get; set; }
        public static string Username { get; set; }
        public static string Password { get; set; }
        public static string EmailSender { get; set; }
    }
}