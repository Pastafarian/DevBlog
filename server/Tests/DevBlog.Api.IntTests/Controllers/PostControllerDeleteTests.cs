using DevBlog.Api.IntTests.Framework;
using DevBlog.Application.Response;
using FluentAssertions;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;

namespace DevBlog.Api.IntTests.Controllers
{
    public class PostControllerDeleteTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public PostControllerDeleteTests(CustomWebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }

        [Fact]
        public async Task DeleteAsync_InvalidClaim_ReturnsUnauthorizedResult()
        {
            // Arrange + Act
            var result = await _client.DeleteAsync("Post/34");

            result.IsSuccessStatusCode.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
        }

        [Fact]
        public async Task DeleteAsync_ValidClaim_PostPresent_ReturnsAuthorizedResult()
        {
            // Arrange
            var token =
                new BearerTokenBuilder()
                    .DefaultToken()
                    .WithClaim("http://stephenadam.api.io/isAdmin", "true").BuildToken();

            var request = new HttpRequestMessage(HttpMethod.Delete, "Post/1");

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

            // Act
            var result = await _client.SendAsync(request);

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
            result.StatusCode.Should().Be(HttpStatusCode.OK);
        }

        [Fact]
        public async Task DeleteAsync_ValidClaim_PostNotPresent_ReturnsNotFoundError()
        {
            // Arrange
            const int postId = 35324;

            var token =
                new BearerTokenBuilder()
                    .DefaultToken()
                    .WithClaim("http://stephenadam.api.io/isAdmin", "true").BuildToken();

            var request = new HttpRequestMessage(HttpMethod.Delete, $"Post/{postId}");

            request.Headers.Authorization = new AuthenticationHeaderValue("bearer", token);

            // Act
            var result = await _client.SendAsync(request);

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true,
                Converters ={
                    new JsonStringEnumConverter()
                }
            };

            var apiResultError = await result.Content.ReadFromJsonAsync<ApiResponseError>(serializeOptions);

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.NotFound);
          
            apiResultError!.ErrorMessage.Should().Be($"Error: Unable to find post with id {postId} to delete.");
            apiResultError.ErrorType.Should().Be(ApiResponseErrorType.NotFound);
        }
    }
}
