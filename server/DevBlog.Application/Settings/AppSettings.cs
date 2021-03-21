namespace DevBlog.Application.Settings
{
	public class AppSettings
	{
		public string GcpProjectId { get; set; }
		public string BucketName { get; set; }
		public string StorageAccountUri { get; set; }
        public string Environment { get; set; }

        public bool IsProduction => !string.IsNullOrWhiteSpace(Environment) && Environment.ToLower() == "production";
    }
}