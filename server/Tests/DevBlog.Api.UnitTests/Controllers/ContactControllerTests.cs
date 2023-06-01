using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DevBlog.Api.Controllers;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Requests;
using DevBlog.Application.Response;
using FluentAssertions;
using Xunit;

namespace DevBlog.Api.UnitTests.Controllers
{
	public class ContactControllerTests
	{

		private readonly ContactController _sut;
		private readonly Mock<IMediator> _mockMediator;

		public ContactControllerTests()
		{
			_mockMediator = new Mock<IMediator>();
			_sut = new ContactController(_mockMediator.Object);
		}

		[Fact]
		public async Task Post_GivenContactPosted_WhenCommandSuccess_ThenEntityCreatedResponseReturned()
		{
			// Arrange
			var request = new SubmitContactRequestDto();
			var entityCreatedResponse = EntityCreatedResponseDto.Create(23);

			_mockMediator.Setup(x => x.Send(It.Is<SubmitContact.Command>(y => y.Request == request), default))
				.ReturnsAsync(ApiResponse<EntityCreatedResponseDto>.Ok(entityCreatedResponse));

			// Act
			var response = (OkObjectResult)(await _sut.Post(request)).Result;

			// Assert
            response.Should().NotBeNull();
			var result = response.Value as EntityCreatedResponseDto;
			Assert.Equal(entityCreatedResponse.Id, result?.Id);
		}
	}
}