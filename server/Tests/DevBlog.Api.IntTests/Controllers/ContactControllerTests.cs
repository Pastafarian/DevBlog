using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using DevBlog.Api.IntTests.Framework;
using DevBlog.Application.Requests;
using FluentAssertions;
using Xunit;

namespace DevBlog.Api.IntTests.Controllers
{
    public class ContactControllerTests : IClassFixture<CustomWebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public ContactControllerTests(CustomWebApplicationFactory<Program> application)
        {
            _client = application.CreateClient();
        }


        [Fact]
        public async Task GivenInvalidContact_WhenExecuted_ThenErrorReturned()
        {
            // Arrange
            var invalidRequest = new SubmitContactRequestDto();

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var invalidRequestJson = JsonSerializer.Serialize(invalidRequest, serializeOptions);

            // Act
            var result = await _client.PostAsync("Contact", new StringContent(invalidRequestJson, Encoding.UTF8, "application/json"));

            // Assert
            result.IsSuccessStatusCode.Should().BeFalse();
            result.StatusCode.Should().Be(HttpStatusCode.BadRequest);
        }

        [Fact]
        public async Task GivenValidContact_WhenExecuted_ThenOkResponseReturned()
        {
            // Arrange
            var validRequest = new SubmitContactRequestDto
            {
                Name = TestData.RandomString(10),
                Email = TestData.RandomEmail(),
                Message = TestData.RandomString(20)
            };

            var serializeOptions = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };

            var invalidRequestJson = JsonSerializer.Serialize(validRequest, serializeOptions);

            // Act
            var result = await _client.PostAsync("Contact", new StringContent(invalidRequestJson, Encoding.UTF8, "application/json"));

            // Assert
            result.IsSuccessStatusCode.Should().BeTrue();
        }

    }
}