namespace DevBlog.Application.Settings
{
    public class AppSettings
	{
        public string Environment { get; set; }
        public string FileStoragePath { get; set; }
        public string ImagePath { get; set; }
        public bool RunMigrations { get; set; }
        public string SendGridApiKey { get; set; }
        public bool IsProduction => !string.IsNullOrWhiteSpace(Environment) && Environment.ToLower() == "production";
    }
}