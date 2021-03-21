using Microsoft.AspNetCore.Mvc;
using DevBlog.Api.Controllers;
using DevBlog.Application.Response;
using Xunit;

namespace DevBlog.Api.UnitTests.Controllers
{
	public class BaseControllerTests
	{
		private readonly TestController sut;

		public BaseControllerTests()
		{
			sut = new TestController();
		}

		[Fact]
		public void ToActionResult_GivenGenericConflictApiResult_WhenExecuted_ThenReturnsCorrectResponse()
		{
			// Arrange
			const string errorMessage = "error message";
			var apiResult = ApiResponse<EntityCreatedResponseDto>.Conflict(errorMessage);

			// Act
			var result = sut.ToActionResult(apiResult).Result;
			var conflictResult = result as ConflictObjectResult;

			// Assert
			Assert.IsType<ConflictObjectResult>(result);
			var response = conflictResult?.Value as ApiResponseError;
			Assert.IsType<ApiResponseError>(response);
			Assert.Equal(errorMessage, response.ErrorMessage);
		}

		[Fact]
		public void ToActionResult_GivenNotFoundApiResult_WhenExecuted_ThenReturnsCorrectResponse()
		{
			// Arrange
			const string errorMessage = "error message";
			var apiResult = ApiResponse.NotFound(errorMessage);

			// Act
			var result = sut.ToActionResult(apiResult).Result;
			var objectResult = result as NotFoundObjectResult;

			// Assert
			Assert.IsType<NotFoundObjectResult>(result);
			var response = objectResult?.Value as ApiResponseError;
			Assert.IsType<ApiResponseError>(response);
			Assert.Equal(errorMessage, response.ErrorMessage);
		}

		[Fact]
		public void ToActionResult_GivenForbiddenApiResult_WhenExecuted_ThenReturnsCorrectResponse()
		{
			// Arrange
			const string errorMessage = "error message";
			var apiResult = ApiResponse.Unauthorized(errorMessage);

			// Act
			var result = sut.ToActionResult(apiResult).Result;
			var objectResult = result as UnauthorizedObjectResult;
			
			// Assert
			Assert.IsType<UnauthorizedObjectResult>(result);
		    var response = objectResult?.Value as ApiResponseError;
		 	Assert.IsType<ApiResponseError>(response);
			Assert.Equal(errorMessage, response.ErrorMessage);
		}

		[Fact]
		public void ToActionResult_GivenBadRequestApiResult_WhenExecuted_ThenReturnsCorrectResponse()
		{
			// Arrange
			const string errorMessage = "error message";
			var apiResult = ApiResponse.BadRequest(errorMessage);

			// Act
			var result = sut.ToActionResult(apiResult).Result;
			var objectResult = result as BadRequestObjectResult;

			// Assert
			Assert.IsType<BadRequestObjectResult>(result);
			var response = objectResult?.Value as ApiResponseError;
			Assert.IsType<ApiResponseError>(response);
			Assert.Equal(errorMessage, response.ErrorMessage);
		}
	}

	public class TestController : BaseController
	{
		public new ActionResult<T> ToActionResult<T>(ApiResponse<T> apiResponse)
		{
			return base.ToActionResult(apiResponse);
		}

		public new ActionResult<ApiResponse> ToActionResult(ApiResponse apiResponse)
		{
			return base.ToActionResult(apiResponse);
		}
	}
}