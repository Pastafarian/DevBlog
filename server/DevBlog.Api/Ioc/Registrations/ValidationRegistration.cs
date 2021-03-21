using SimpleInjector;
using DevBlog.Application.Requests.Validators;

namespace DevBlog.Api.Ioc.Registrations
{
    public class ValidationRegistration
    {
		public static void Register(Container container)
        {
            container.Register<SubmitContactRequestValidator, SubmitContactRequestValidator>();
        }
	}
}