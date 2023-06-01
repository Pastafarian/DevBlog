using DevBlog.Application.Requests.Validators;
using Microsoft.Extensions.DependencyInjection;

namespace DevBlog.Api.Ioc.Registrations
{
    public static class ValidationRegistration
    {
		public static void RegisterValidation(this IServiceCollection services)
        {
            services.AddTransient<SubmitContactRequestValidator, SubmitContactRequestValidator>();
            services.AddTransient<UpdatePostRequestValidator, UpdatePostRequestValidator>();
        }
	}
}