using SimpleInjector;
using DevBlog.Application.Factories;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class GCPRegistrations
	{
		public static void Register(Container container)
		{
			container.Register(() => container.GetInstance<IStorageClientFactory>().Build());
		}
	}
}
