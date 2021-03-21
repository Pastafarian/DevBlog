using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DevBlog.Api.Controllers;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Response;
using DevBlog.TestHelper;
using Xunit;

namespace DevBlog.Api.UnitTests.Controllers
{
	public class ImageControllerTests
	{
		private readonly Mock<IFormFile> mockFormFile;
		private readonly Mock<IMediator> mockMediator;
		private readonly ImageController sut;

		public ImageControllerTests()
		{
			mockMediator = new Mock<IMediator>();
			mockFormFile = new Mock<IFormFile>();
			sut = new ImageController(mockMediator.Object);
		}

		[Fact]
		public async void Upload_GivenValidFormFile_WhenApiSuccessResponseReturned_OkResultReturned()
		{
			// Arrange
			const string fileName = "foo.jpg";
			mockFormFile.Setup(x => x.FileName).Returns(fileName);
			mockFormFile.Setup(x => x.OpenReadStream()).Returns(TestHelpers.GetMemoryStream());
			
			var imageUploadedResponse = new ImageUploadedResponseDto("http://domain/image.jpg");

			mockMediator.Setup(m => m.Send(It.Is<UploadImage.Command>(c => c.FileName == fileName), default))
				.ReturnsAsync(ApiResponse<ImageUploadedResponseDto>.Ok(imageUploadedResponse));

			// Act
			var result = (OkObjectResult)(await sut.Upload(mockFormFile.Object)).Result;
			var response = result.Value as ImageUploadedResponseDto;
			// Assert
			mockMediator.Verify(x => x.Send(It.Is<UploadImage.Command>(y => y.FileName == fileName), default));
			Assert.IsType<ImageUploadedResponseDto>(result.Value);
			Assert.Equal(imageUploadedResponse.ImageUrl, response?.ImageUrl);
		}
	}
}