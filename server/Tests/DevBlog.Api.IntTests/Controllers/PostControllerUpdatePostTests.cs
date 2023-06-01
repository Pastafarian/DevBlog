using System;
using DevBlog.Api.IntTests.Framework;
using DevBlog.Application.Dtos;
using DevBlog.TestHelper;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using Xunit;
using System.Net;

namespace DevBlog.Api.IntTests.Controllers
{
    public class PostControllerUpdatePostTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PostControllerUpdatePostTests(CustomWebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task GivenValidPostAndValidClaim_WhenUpdateCalled_ThenPostUpdated()
        {
            // Arrange
            var token = new BearerTokenBuilder().DefaultToken().WithClaim("http://stephenadam.api.io/isAdmin", "true").BuildToken();

            var fakePost = TestData.FakePost;
            var post = fakePost.Generate();
            post.Id = 1;
            var byteContent = TestValues.SerializeObjectToByteArrayContent(post);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);


            var result = await _client.PutAsync("Post", byteContent);

            result.IsSuccessStatusCode.Should().BeTrue();
            var postResult = await result.Content.ReadFromJsonAsync<PostDto>(TestHelpers.SerializeOptions);

            // Assert
            postResult!.Slug.Should().Be(post.Slug);
            postResult.Title.Should().Be(post.Title);
            postResult.Body.Should().Be(post.Body);
            postResult.HeaderImage.Should().NotBeNullOrWhiteSpace();
            postResult.PublishDate.Should().Be(post.PublishDate);
            postResult.IsPublished.Should().Be(post.PublishDate < DateTime.UtcNow);
        }

        [Fact]
        public async Task GivenValidPostAndInvalidClaim_WhenUpdateCalled_ReturnsUnauthorizedResult()
        {
            // Arrange
            var fakePost = TestData.FakePost;
            var post = fakePost.Generate();
            post.Id = 1;
            var byteContent = TestValues.SerializeObjectToByteArrayContent(post);

            // Act
            var result = await _client.PutAsync("Post", byteContent);

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }
    }
}