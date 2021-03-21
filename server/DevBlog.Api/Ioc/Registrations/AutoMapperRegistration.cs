using AutoMapper;
using AutoMapper.Configuration;
using SimpleInjector;
using DevBlog.Application.MappingConfiguration;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class AutoMapperRegistration
	{
		public static void Register(Container container)
		{
			container.RegisterSingleton(() => GetMapper(container));
		}

		private static IMapper GetMapper(Container container)
		{
			var mce = new MapperConfigurationExpression();
			mce.ConstructServicesUsing(container.GetInstance);

			mce.AddMaps(typeof(AutoMapperProfile).Assembly);

			var mc = new MapperConfiguration(mce);
			mc.AssertConfigurationIsValid();

			return new Mapper(mc, container.GetInstance);
		}
	}
}