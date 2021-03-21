using System;
using Microsoft.AspNetCore.Mvc;
using DevBlog.Application.Auth;
using DevBlog.Application.Extensions;
using DevBlog.Application.Response;

namespace DevBlog.Api.Controllers
{
	public class BaseController : Controller
	{
		protected SiteUser SiteUser => LazySiteUser.Value;
		private Lazy<SiteUser> LazySiteUser => new Lazy<SiteUser>(User.BuildSiteUser());

		protected ActionResult<T> ToActionResult<T>(ApiResponse<T> apiResponse)
		{
			if (apiResponse.IsSuccess) return Ok(apiResponse.Value);
			
			return ToErrorResult(apiResponse);
		}

		protected ActionResult<ApiResponse> ToActionResult(ApiResponse apiResponse)
		{
			if (apiResponse.IsSuccess) return Ok();
			
			return ToErrorResult(apiResponse);
		}

		private ActionResult ToErrorResult(ApiResponse serviceResult)
		{
			if (serviceResult.IsConflict)
				return Conflict(serviceResult.Error);

			if (serviceResult.IsNotFound)
				return NotFound(serviceResult.Error);

			if (serviceResult.IsUnauthorized)
				return Unauthorized(serviceResult.Error);
			
			return BadRequest(serviceResult.Error);
		}
	}
}