using SimpleInjector;
using DevBlog.Application.Factories;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class FactoryRegistration
	{
		public static void Register(Container container)
		{
			container.Register<IStorageClientFactory, StorageClientFactory>();
			container.Register<IGoogleCredentialFactory, GoogleCredentialFactory>();
		}
	}
}
