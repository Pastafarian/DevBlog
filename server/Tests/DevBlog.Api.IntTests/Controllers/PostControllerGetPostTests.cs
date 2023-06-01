using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using DevBlog.Api.IntTests.Framework;
using DevBlog.Application.Dtos;
using DevBlog.TestHelper;
using FluentAssertions;
using Xunit;

namespace DevBlog.Api.IntTests.Controllers;

public class PostControllerIndexTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public PostControllerIndexTests(CustomWebApplicationFactory<Program> application)
    {
        _client = application.CreateClient();
    }

    [Fact]
    public async Task Given10PostsExist_WhenIndexCalled_Then10PreviewsReturned()
    {
        // Arrange + Act
        var result = await _client.GetAsync("Post");

        result.IsSuccessStatusCode.Should().BeTrue();
        var posts = await result.Content.ReadFromJsonAsync<List<PostDto>>(TestHelpers.SerializeOptions);

        // Assert
        posts.Should().NotBeNull();
        posts.Should().HaveCount(10);

        // Check we are truncating the body
        posts.Should().AllSatisfy(x => x.Body.Length.Should().BeLessThan(250));
    }
}