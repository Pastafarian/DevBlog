using SimpleInjector;
using SimpleInjector.Advanced;
using SimpleInjector.Lifestyles;
using DevBlog.Api.Ioc.Registrations;
using DevBlog.Application.Settings;
using System;
using System.Linq;
using System.Reflection;

namespace DevBlog.Api.Ioc
{
	public static class IocHelper
	{
		public static Container BuildContainer()
		{
			return BuildContainer(Lifestyle.Scoped);
		}

		public static Container BuildContainer(Lifestyle lifestyle)
		{
			return new Container
			{
				Options =
				{
					DefaultLifestyle = lifestyle,
					ResolveUnregisteredConcreteTypes = false,
					DefaultScopedLifestyle = new AsyncScopedLifestyle(),
					ConstructorResolutionBehavior = new LeastGreedyConstructorBehaviour()
				}
			};
		}

		public static void Register(this Container container, AppSettings appSettings, LoggerSettings loggerSettings)
		{
			MediatorRegistration.Register(container);
			AutoMapperRegistration.Register(container);
			FactoryRegistration.Register(container);
			ServiceRegistration.Register(container);
            ValidationRegistration.Register(container);
			SettingsRegistration.Register(container, appSettings, loggerSettings);
			LoggingRegistration.Register(container);
			GCPRegistrations.Register(container);
		}

		private class LeastGreedyConstructorBehaviour : IConstructorResolutionBehavior
		{
			public ConstructorInfo? TryGetConstructor(Type implementationType, out string? errorMessage)
			{
				errorMessage = $"{implementationType} has no public constructors.";
				return implementationType.GetConstructors().OrderBy(x => x.GetParameters().Length).FirstOrDefault();
			}
		}
	}
}
