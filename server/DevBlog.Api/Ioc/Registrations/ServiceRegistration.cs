using DevBlog.Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class ServiceRegistration
	{
		public static void RegisterServices(this IServiceCollection services)
        {
            services.AddTransient<IFileSystemService, FileSystemService>();
			services.AddTransient<IFileSystemService, FileSystemService>();
            services.AddTransient<IImageService, ImageService>();
			services.AddTransient<IFileNamingService, FileNamingService>();
            services.AddTransient<IImageStorageService, ImageStorageService>();
            services.AddTransient<ISendGridService, SendGridService>();
        }
	}
}