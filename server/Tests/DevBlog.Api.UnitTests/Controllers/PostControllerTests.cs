using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;
using DevBlog.Api.Controllers;
using DevBlog.Application.Dtos;
using DevBlog.Application.Response;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Handlers.Query;
using Xunit;

namespace DevBlog.Api.UnitTests.Controllers
{
	public class PostControllerTests
	{
		private readonly Mock<IMediator> mockMediator;
		private readonly PostController sut;

		public PostControllerTests()
		{
			mockMediator = new Mock<IMediator>();
			sut = new PostController(mockMediator.Object);
		}

		[Fact]
		public async void Index_GivenCalled_WhenUserIsNotAdmin_ThenMapsResultsToModels()
		{
			// Arrange
			var summaryDtos = new List<PostDto> { new PostDto { Body = "foo" } };
			var response = new ApiResponse<List<PostDto>>
			{
				IsSuccess = true,
				Value = summaryDtos
			};

			mockMediator.Setup(x => x.Send(It.Is<GetPosts.Query>(y => !y.SiteUser.IsAdmin), CancellationToken.None))
				.ReturnsAsync(response);

			// Act
			var result = (OkObjectResult)(await sut.Index()).Result;
			var value = result.Value as List<PostDto>;

			// Assert
			Assert.NotNull(value);
			Assert.Equal(summaryDtos.First().Body, value.Single().Body);
		}

		[Fact]
		public async void GetArticle_GivenCalled_WhenRequestsPublishedPost_ThenMapsResultToModel()
		{
			// Arrange
			var postDto = new PostDto { Id = 99 };
			var response = ApiResponse<PostDto>.Ok(postDto);
			const string slug = "slug";

			mockMediator.Setup(x => x.Send(It.Is<GetPost.Query>(y => y.Slug == slug), CancellationToken.None))
				.ReturnsAsync(response);

			// Act
			var result = (OkObjectResult)(await sut.GetPost(slug)).Result;
			var value = result.Value as PostDto;

			// Assert
			Assert.NotNull(value);
			Assert.Equal(postDto.Id, value.Id);
		}

		[Fact]
		public async void CreatePost_GivenValidPostPresent_WhenApiSuccessResponseReturned_OkResultReturned()
		{
			// Arrange
			const int postId = 23;
			var createPostDto = new CreatePostDto
			{
				Title = "test title"
			};

			mockMediator.Setup(m => m.Send(It.Is<CreatePost.Command>(c => c.Post == createPostDto), CancellationToken.None))
				.ReturnsAsync(ApiResponse<PostDto>.Ok(new PostDto{Id = postId }));

			// Act
			var result = (OkObjectResult)(await sut.CreatePost(createPostDto)).Result;

			// Assert
			mockMediator.Verify(x => x.Send(It.Is<CreatePost.Command>(y => y.Post.Title == createPostDto.Title), CancellationToken.None));
			Assert.IsType<PostDto>(result.Value);
		}

		[Fact]
		public async Task UpdatePost_GivenValidPostRequest_WhenApiSuccessResponseReturned_OkResultReturned()
		{
			// Arrange
			var postDto = new PostDto { Id = 23 };
			
			mockMediator.Setup(m => m.Send(It.Is<UpdatePost.Command>(c => c.Post == postDto && c.Post.Id == postDto.Id), CancellationToken.None))
				.ReturnsAsync(ApiResponse<PostDto>.Ok(postDto));

			// Act
			var response = (OkObjectResult)(await sut.UpdatePost(postDto)).Result;

			// Assert
			mockMediator.Verify(x => x.Send(It.Is<UpdatePost.Command>(y => y.Post.Id == postDto.Id && y.Post.Id == postDto.Id), CancellationToken.None));
			Assert.IsType<OkObjectResult>(response);
		}
	}
}