using AutoMapper;
using Moq;
using DevBlog.Application.Dtos;
using DevBlog.Application.Handlers.Query;
using DevBlog.Domain.Entities;
using Xunit;

namespace DevBlog.Application.IntTests.Handlers.Query
{
	public class GetPostTests : IClassFixture<ContextBuilder>
	{
		private readonly ContextBuilder _contextBuilder;
		private readonly GetPost.Handler _sut;
		private readonly Mock<IMapper> _mapper;

		public GetPostTests(ContextBuilder contextBuilder)
        {
            contextBuilder.Build();
            _contextBuilder = contextBuilder;
			_mapper = new Mock<IMapper>();
			_sut = new GetPost.Handler(contextBuilder.Context, _mapper.Object);
		}

		[Fact]
		public async void Handle_ShouldReturnNotFound_IfPostNotFound()
		{
			// Arrange
			var query = new GetPost.Query("unknown-slug");

			// Act
			var result = await _sut.Handle(query, default);

			// Assert
			Assert.True(result.IsNotFound);
		}

		[Fact]
		public async void Handle_ShouldReturnPostAndIgnoreCase_IfValidSlug()
		{
			// Arrange
			const string slug = "A-POST-SLUG";

			var post = new Post { Slug = slug };
            await _contextBuilder.Context.Posts.AddAsync(post);
            await _contextBuilder.Context.SaveChangesAsync();

			var query = new GetPost.Query(slug.ToLower());
			var postDto = new PostDto();
			_mapper.Setup(x => x.Map<Post, PostDto>(It.IsAny<Post>())).Returns(postDto);

			// Act
			var result = await _sut.Handle(query, default);

			// Assert
			Assert.Equal(postDto, result.Value);
		}
	}
}
