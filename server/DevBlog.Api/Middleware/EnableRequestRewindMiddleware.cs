using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace DevBlog.Api.Middleware
{
	
	/// <summary>
	/// Enable request body for multiple reads
	/// https://devblogs.microsoft.com/aspnet/re-reading-asp-net-core-request-bodies-with-enablebuffering/
	/// </summary>
    public class EnableRequestRewindMiddleware : IMiddleware
	{
		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			context.Request.EnableBuffering();
			await next(context);
		}
	}
}