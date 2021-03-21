using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using Microsoft.Net.Http.Headers;
using System;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace DevBlog.Api.Middleware
{
	// ReSharper disable once ClassNeverInstantiated.Global
	public class ExceptionHandlerMiddleware : IMiddleware
	{
		private readonly IActionResultExecutor<ObjectResult> executor;
		private readonly ILogger logger;
		private static readonly ActionDescriptor EmptyActionDescriptor = new ActionDescriptor();

		public ExceptionHandlerMiddleware(IActionResultExecutor<ObjectResult> executor, ILoggerFactory loggerFactory)
		{
			this.executor = executor;
			logger = loggerFactory.CreateLogger<ExceptionHandlerMiddleware>();
		}

		public async Task InvokeAsync(HttpContext context, RequestDelegate next)
		{
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				logger.LogError(ex, $"An unhandled exception has occurred while executing the request. Url: {context.Request.GetDisplayUrl()}. Request Data: " + GetRequestData(context));

				if (context.Response.HasStarted) throw;
				
				var routeData = context.GetRouteData() ?? new RouteData();

				ClearCacheHeaders(context.Response);

				var actionContext = new ActionContext(context, routeData, EmptyActionDescriptor);

				var result = new ObjectResult(new ErrorResponse($"Error processing request. Server error. {ex.Message} {ex.InnerException?.Message} {ex.StackTrace}"))
				{
					StatusCode = (int)HttpStatusCode.InternalServerError,
				};

				await executor.ExecuteAsync(actionContext, result);
			}
		}

		private static string GetRequestData(HttpContext context)
		{
			var sb = new StringBuilder();

			if (context.Request.HasFormContentType && context.Request.Form.Any())
			{
				sb.Append("Form variables:");
				foreach (var (key, value) in context.Request.Form)
				{
					sb.AppendFormat("Key={0}, Value={1}<br/>", key, value);
				}
			}

			sb.AppendLine("Method: " + context.Request.Method);

			return sb.ToString();
		}

		private static void ClearCacheHeaders(HttpResponse response)
		{
			response.Headers[HeaderNames.CacheControl] = "no-cache";
			response.Headers[HeaderNames.Pragma] = "no-cache";
			response.Headers[HeaderNames.Expires] = "-1";
			response.Headers.Remove(HeaderNames.ETag);
		}

		[DataContract(Name = "ErrorResponse")]
		public class ErrorResponse
		{
			[DataMember(Name = "Message")]
			public string Message { get; set; }

			public ErrorResponse(string message)
			{
				Message = message;
			}
		}
	}
}