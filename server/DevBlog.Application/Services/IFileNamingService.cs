namespace DevBlog.Application.Services
{
	public interface IFileNamingService
	{
		string GenerateFileName(string name, string extension);
	}
}