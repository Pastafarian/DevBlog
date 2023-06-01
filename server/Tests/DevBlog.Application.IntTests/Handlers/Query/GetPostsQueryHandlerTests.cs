using System.Linq;
using DevBlog.Application.Auth;
using DevBlog.Application.Handlers.Query;
using DevBlog.Domain;
using DevBlog.Domain.Entities;
using DevBlog.TestHelper;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace DevBlog.Application.IntTests.Handlers.Query
{
    public class GetPostsQueryHandlerTests : IClassFixture<ContextBuilder>
	{
        private readonly ContextBuilder _contextBuilder;
		private readonly GetPosts.Handler _sut;

        public GetPostsQueryHandlerTests(ContextBuilder contextBuilder)
        {
            _contextBuilder = contextBuilder;
			_contextBuilder.Build();
            var mockLogger = new Mock<ILogger<GetPosts.Handler>>();
            _sut = new GetPosts.Handler(contextBuilder.Context, mockLogger.Object);
		}

		[Fact]
		public async void GivenNoPosts_WhenExecuted_ThenReturnEmptyList()
		{
			// Arrange
			ClearDb(_contextBuilder.Context);

			var query = new GetPosts.Query(new SiteUser());

			// Act
			var result = await _sut.Handle(query, default);

			// Assert
			Assert.Empty(result.Value);
		}

		[Fact]
		public async void GivenPublishedPostsPresent_WhenPublishedPostsSpecified_ThenNoUnpublishedPostsReturned()
		{
			// Arrange
			PopulatedDb(_contextBuilder.Context);
			var query = new GetPosts.Query(new SiteUser());

			// Act
			var postSummaries = (await _sut.Handle(query, default)).Value;

			// Assert
			Assert.Equal(2, postSummaries.Count);
			Assert.All(postSummaries, x => { Assert.True(x.IsPublished); });
		}

		[Fact]
		public async void Handle_WhenUnpublishedPostsSpecified_ThenOnlyPublishedPostsReturned()
		{
			// Arrange
			PopulatedDb(_contextBuilder.Context);

			var query = new GetPosts.Query(new SiteUser());

			// Act
			var postSummaries = (await _sut.Handle(query, default)).Value;

			// Assert
			Assert.Equal(2, postSummaries.Count);
		}

		private static void PopulatedDb(Context context)
		{
			ClearDb(context);

			context.Posts.Add(new Post { Id = 1, PublishDate = TestValues.DateInPast().ToUniversalTime() });
			context.Posts.Add(new Post { Id = 2, PublishDate = TestValues.DateInPast().ToUniversalTime() });
			context.Posts.Add(new Post { Id = 3, PublishDate = TestValues.DateInFuture().ToUniversalTime() });
			context.SaveChanges();
		}

		private static void ClearDb(Context context)
		{
			context.Posts.RemoveRange(context.Posts.ToList());
			context.SaveChanges();
		}
	}
}
