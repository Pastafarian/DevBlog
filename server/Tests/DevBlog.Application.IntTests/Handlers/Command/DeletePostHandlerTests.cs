using System;
using System.Linq;
using MediatR;
using DevBlog.Application.Handlers.Command;
using DevBlog.Domain.Entities;
using DevBlog.TestHelper;
using Xunit;

namespace DevBlog.Application.IntTests.Handlers.Command
{
    public class DeletePostHandlerTests
	{
		private readonly ContextHelper th;
		private readonly DeletePost.Handler sut;

		public DeletePostHandlerTests()
		{
			th = new ContextHelper().BuildInMemoryHelper();
			sut = new DeletePost.Handler(th.Context);
		}


		[Fact]
		public async void GivenPostNotFound_WhenExecuted_ThenThrows()
		{
			// Arrange
			var command = new DeletePost.Command(76);

			// Act + Assert
			await Assert.ThrowsAsync<InvalidOperationException>(() => ((IRequestHandler<DeletePost.Command>)sut).Handle(command, default));
		}


		[Fact]
		public async void GivenPostPresent_WhenExecuted_ThenPostRemoved()
		{
			// Arrange
			await th.Context.Posts.AddAsync(new Post { Id = 76 });
			await th.Context.SaveChangesAsync();

			var command = new DeletePost.Command(76);

			// Act
			await ((IRequestHandler<DeletePost.Command>)sut).Handle(command, default);

			// Assert
			Assert.True(th.Context.Posts.All(x => x.Id != 76));
		}
	}
}