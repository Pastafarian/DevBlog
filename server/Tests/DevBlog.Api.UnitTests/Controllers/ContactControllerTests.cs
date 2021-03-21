using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DevBlog.Api.Controllers;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Requests;
using DevBlog.Application.Response;
using DevBlog.Application.Settings;
using Xunit;

namespace DevBlog.Api.UnitTests.Controllers
{
	public class ContactControllerTests
	{

		private readonly ContactController sut;
		private readonly Mock<IMediator> mockMediator;

		public ContactControllerTests()
		{
			mockMediator = new Mock<IMediator>();
			sut = new ContactController(mockMediator.Object, new AppSettings());
		}

		[Fact]
		public async Task Post_GivenContactPosted_WhenCommandSuccess_ThenEntityCreatedResponseReturned()
		{
			// Arrange
			var request = new SubmitContactRequestDto();
			var entityCreatedResponse = EntityCreatedResponseDto.Create(23);

			mockMediator.Setup(x => x.Send(It.Is<SubmitContact.Command>(y => y.Request == request), default))
				.ReturnsAsync(ApiResponse<EntityCreatedResponseDto>.Ok(entityCreatedResponse));

			// Act
			var response = (OkObjectResult)(await sut.Post(request)).Result;

			// Assert
			var result = response.Value as EntityCreatedResponseDto;
			Assert.Equal(entityCreatedResponse.Id, result?.Id);
		}
	}
}