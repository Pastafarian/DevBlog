using SimpleInjector;
using DevBlog.Application.Services;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class ServiceRegistration
	{
		public static void Register(Container container)
		{
			container.Register<IFileService, FileService>();
			container.Register<IImageService, ImageService>();
			container.Register<IFileNamingService, FileNamingService>();
			container.Register<IImageStorageService, ImageStorageService>();
		}
	}
}