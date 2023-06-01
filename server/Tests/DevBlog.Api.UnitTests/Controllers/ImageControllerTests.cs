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
		private readonly Mock<IFormFile> _mockFormFile;
		private readonly Mock<IMediator> _mockMediator;
		private readonly ImageController _sut;

		public ImageControllerTests()
		{
			_mockMediator = new Mock<IMediator>();
			_mockFormFile = new Mock<IFormFile>();
			_sut = new ImageController(_mockMediator.Object);
		}

		[Fact]
		public async void Upload_GivenValidFormFile_WhenApiSuccessResponseReturned_OkResultReturned()
		{
			// Arrange
			const string fileName = "foo.jpg";
			_mockFormFile.Setup(x => x.FileName).Returns(fileName);
			_mockFormFile.Setup(x => x.OpenReadStream()).Returns(TestHelpers.GetMemoryStream());
			
			var imageUploadedResponse = new ImageUploadedResponseDto("http://domain/image.jpg");

			_mockMediator.Setup(m => m.Send(It.Is<UploadImage.Command>(c => c.FileName == fileName), default))
				.ReturnsAsync(ApiResponse<ImageUploadedResponseDto>.Ok(imageUploadedResponse));

			// Act
			var result = (OkObjectResult)(await _sut.Upload(_mockFormFile.Object)).Result;
            Assert.NotNull(result);

			var response = result.Value as ImageUploadedResponseDto;
			
			// Assert
			_mockMediator.Verify(x => x.Send(It.Is<UploadImage.Command>(y => y.FileName == fileName), default));
			Assert.IsType<ImageUploadedResponseDto>(result.Value);
			Assert.Equal(imageUploadedResponse.ImageUrl, response?.ImageUrl);
		}
	}
}