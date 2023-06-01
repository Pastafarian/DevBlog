using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using DevBlog.Application.Dtos;
using Xunit;
using DevBlog.Api.IntTests.Framework;
using FluentAssertions;
using System.Net;
using DevBlog.Application.Response;
using System.Net.Http.Json;
using DevBlog.TestHelper;

namespace DevBlog.Api.IntTests.Controllers
{
    public class PostControllerGetPostTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PostControllerGetPostTests(CustomWebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task GivenPostExists_WhenGetPostIsCalled_ReturnsPost()
        {
            // Arrange
            var postsResults = await _client.GetAsync("Post");
            var posts = await postsResults.Content.ReadFromJsonAsync<List<PostDto>>(TestHelpers.SerializeOptions);
            posts.Should().NotBeNull();

            // Act
            // ReSharper disable once AssignNullToNotNullAttribute
            var result = await _client.GetAsync("Post/" + posts.First().Slug);

            result.IsSuccessStatusCode.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);

            var post = await result.Content.ReadFromJsonAsync<PostDto>(TestHelpers.SerializeOptions);

            post.Should().NotBeNull();
            // ReSharper disable once PossibleNullReferenceException
            post.Slug.Should().NotBeNullOrWhiteSpace();
            post.Body.Should().NotBeNullOrWhiteSpace();
        }

        [Fact]
        public async Task GivenPostDoesNotExist_WhenGetPostIsCalled_ReturnsNotFoundResult()
        {
            // Arrange + Act
            var slug = TestValues.UniqueString(10);
            var result = await _client.GetAsync("Post/" + slug);

            result.IsSuccessStatusCode.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);

            var apiResponseError = await result.Content.ReadFromJsonAsync<ApiResponseError>(TestHelpers.SerializeOptions);
            // ReSharper disable once PossibleNullReferenceException
            apiResponseError.ErrorMessage.Should().Be($"Unable to find post with slug '{slug}'.");
            apiResponseError.ErrorType.Should().Be(ApiResponseErrorType.NotFound);
        }
    }
}