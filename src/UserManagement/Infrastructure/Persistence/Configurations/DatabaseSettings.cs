namespace UserManagement.Infrastructure.Persistence.Configurations
{
    public class DatabaseSettings
    {
        public const string SectioName = "DatabaseSettings";
        public static string Server { get; set; }
        public static int Port { get; set; }
        public static string UserId { get; set; }
        public static string Password { get; set; }
        public static string Database { get; set; }
        public static int CommandTimeout { get; set; }   
    }
}