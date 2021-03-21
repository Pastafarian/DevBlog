using System.Linq;
using DevBlog.Application.Auth;
using DevBlog.Application.Handlers.Query;
using DevBlog.Domain;
using DevBlog.Domain.Entities;
using DevBlog.TestHelper;
using Xunit;
using Xunit.Abstractions;

namespace DevBlog.Application.IntTests.Handlers.Query
{
	public class GetPostsQueryHandlerTests
	{
		private readonly ITestOutputHelper _output;
		private readonly ContextHelper th;
		private readonly GetPosts.Handler sut;

		public GetPostsQueryHandlerTests(ITestOutputHelper output)
		{
			_output = output;
			th = new ContextHelper().BuildPgHelper();
			sut = new GetPosts.Handler(th.Context);
		}

		[Fact]
		public async void GivenNoPosts_WhenExecuted_ThenReturnEmptyList()
		{
			// Arrange
			ClearDb(th.Context);

			var query = new GetPosts.Query(new SiteUser());

			// Act
			var result = await sut.Handle(query, default);

			// Assert
			Assert.Empty(result.Value);
		}

		[Fact]
		public async void GivenPublishedPostsPresent_WhenPublishedPostsSpecified_ThenNoUnpublishedPostsReturned()
		{
			// Arrange
			PopulatedDb(th.Context);
			var query = new GetPosts.Query(new SiteUser());

			// Act
			var postSummaries = (await sut.Handle(query, default)).Value;

			// Assert
			Assert.Equal(2, postSummaries.Count);
			Assert.All(postSummaries, x => { Assert.True(x.IsPublished); });
		}

		[Fact]
		public async void Handle_WhenUnpublishedPostsSpecified_ThenOnlyPublishedPostsReturned()
		{
			// Arrange
			PopulatedDb(th.Context);

			var query = new GetPosts.Query(new SiteUser());

			// Act
			var postSummaries = (await sut.Handle(query, default)).Value;

			// Assert
			Assert.Equal(2, postSummaries.Count);
		}

		private static void PopulatedDb(Context context)
		{
			ClearDb(context);

			context.Posts.Add(new Post { Id = 1, PublishDate = TestValues.DateInPast() });
			context.Posts.Add(new Post { Id = 2, PublishDate = TestValues.DateInPast() });
			context.Posts.Add(new Post { Id = 3, PublishDate = TestValues.DateInFuture() });
			context.SaveChanges();
		}

		private static void ClearDb(Context context)
		{
			context.Posts.RemoveRange(context.Posts.ToList());
			context.SaveChanges();
		}
	}
}
