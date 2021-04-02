using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DevBlog.Api.Controllers;
using DevBlog.Api.IntTests.Framework;
using DevBlog.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace DevBlog.Api.IntTests.Controllers
{
    public class PostControllerTests
    {
        private readonly PostController sut;
        private readonly ControllerFixture fixture;

        public PostControllerTests()
        {
            fixture = new ControllerFixture();
            sut = new PostController(fixture.Mediator);
        }

        [Fact]
        public async Task Given10PostsExist_WhenIndexCalled_Then10PreviewsReturned()
        {
            // Arrange
            fixture.Context.Posts.RemoveRange(fixture.Context.Posts.ToList());
            await fixture.Context.Posts.AddRangeAsync(TestData.GetRandomPosts(10));
            await fixture.Context.SaveChangesAsync();

            // Act
            var actionResult = await sut.Index();

            var posts = (List<PostDto>)((OkObjectResult)actionResult.Result).Value;
            
            // Assert
            Assert.Equal(10, posts.Count);

            // Check we are truncating the body
            Assert.All(posts, postDto => Assert.True(postDto.Body.Length < 250));

            // Clean up
            fixture.Context.Posts.RemoveRange(fixture.Context.Posts.ToList());
        }
    }
}