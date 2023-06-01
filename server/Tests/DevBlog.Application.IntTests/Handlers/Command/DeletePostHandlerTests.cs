using System.Linq;
using MediatR;
using DevBlog.Application.Handlers.Command;
using DevBlog.Application.Response;
using DevBlog.Domain.Entities;
using FluentAssertions;
using Xunit;

namespace DevBlog.Application.IntTests.Handlers.Command
{
    public class DeletePostHandlerTests : IClassFixture<ContextBuilder>
	{
		private readonly ContextBuilder _contextBuilder;
        private readonly DeletePost.Handler _sut;

		public DeletePostHandlerTests(ContextBuilder contextBuilder)
        {
            _contextBuilder = contextBuilder;
            contextBuilder.Build();
            _sut = new DeletePost.Handler(contextBuilder.Context);
		}


		[Fact]
		public async void GivenPostNotFound_WhenExecuted_ThenThrows()
		{
			// Arrange
			var command = new DeletePost.Command(76);

            // Act
            var result = await _sut.Handle(command, default);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.Error.ErrorType.Should().Be(ApiResponseErrorType.NotFound);
            result.Error.ErrorMessage.Should().Be($"Error: Unable to find post with id {command.PostId} to delete.");
        }


		[Fact]
		public async void GivenPostPresent_WhenExecuted_ThenPostRemoved()
		{
			// Arrange
			await _contextBuilder.Context.Posts.AddAsync(new Post { Id = 76 });
			await _contextBuilder.Context.SaveChangesAsync();

			var command = new DeletePost.Command(76);

			// Act
			await ((IRequestHandler<DeletePost.Command, ApiResponse>)_sut).Handle(command, default);

			// Assert
			Assert.True(_contextBuilder.Context.Posts.All(x => x.Id != 76));
		}
	}
}