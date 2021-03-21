using AutoMapper;
using Moq;
using DevBlog.Application.Dtos;
using DevBlog.Application.Handlers.Query;
using DevBlog.Domain.Entities;
using DevBlog.TestHelper;
using Xunit;

namespace DevBlog.Application.IntTests.Handlers.Query
{
	public class GetPostTests
	{
		private readonly ContextHelper th;
		private readonly GetPost.Handler sut;
		private readonly Mock<IMapper> mapper;

		public GetPostTests()
		{
			th = new ContextHelper().BuildInMemoryHelper();
			mapper = new Mock<IMapper>();
			sut = new GetPost.Handler(th.Context, mapper.Object);
		}

		[Fact]
		public async void Handle_ShouldReturnNotFound_IfPostNotFound()
		{
			// Arrange
			var query = new GetPost.Query("unknown-slug");

			// Act
			var result = await sut.Handle(query, default);

			// Assert
			Assert.True(result.IsNotFound);
		}

		[Fact]
		public async void Handle_ShouldReturnPostAndIgnoreCase_IfValidSlug()
		{
			// Arrange
			const string slug = "A-POST-SLUG";
			await th.CreatePost(new Post { Slug = slug });
			var query = new GetPost.Query(slug.ToLower());
			var postDto = new PostDto();
			mapper.Setup(x => x.Map<Post, PostDto>(It.IsAny<Post>())).Returns(postDto);

			// Act
			var result = await sut.Handle(query, default);

			// Assert
			Assert.Equal(postDto, result.Value);
		}
	}
}
