using MediatR;
using MediatR.Pipeline;
using SimpleInjector;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using DevBlog.Application.Handlers.Query;

namespace DevBlog.Api.Ioc.Registrations
{
	public static class MediatorRegistration
	{
		public static void Register(Container container)
		{
			var assemblies = GetAssemblies().ToArray();

			container.RegisterSingleton<IMediator, Mediator>();
			container.Register(typeof(IRequestHandler<,>), assemblies);
			container.Register(typeof(IRequestHandler<>), assemblies);
			container.Collection.Register(typeof(IPipelineBehavior<,>), new[]
			{
				typeof(RequestPreProcessorBehavior<,>),
				typeof(RequestPostProcessorBehavior<,>)
			});

			container.Collection.Register(typeof(IRequestPreProcessor<>), assemblies);
			container.Collection.Register(typeof(IRequestPostProcessor<,>), assemblies);

			var notificationHandlerTypes = container.GetTypesToRegister(typeof(INotificationHandler<>), assemblies, new TypesToRegisterOptions
			{
				IncludeGenericTypeDefinitions = true,
				IncludeComposites = false,
			});

			container.Register(typeof(INotificationHandler<>), notificationHandlerTypes);
			container.Register(() => new ServiceFactory(container.GetInstance), Lifestyle.Singleton);
		}

		private static IEnumerable<Assembly> GetAssemblies()
		{
			yield return typeof(IMediator).GetTypeInfo().Assembly;
			yield return typeof(GetPost.Query).GetTypeInfo().Assembly;
		}
	}
}


