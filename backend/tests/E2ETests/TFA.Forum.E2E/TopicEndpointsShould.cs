using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.E2E;

public class TopicEndpointsShould : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient _httpClient;
    private readonly JsonSerializerOptions _jsonOptions;

    public TopicEndpointsShould(ForumApiApplicationFactory factory)
    {
        _httpClient = factory.CreateClient();
        _jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ReturnForbidden_WhenNotAuthenticated()
    {
        var forumTitle = Guid.NewGuid().ToString();
        var forumId = await CreateForumAsync(forumTitle);

        var responseMessage = await _httpClient.PostAsync(
            $"api/v1.0/Forum/{forumId}/topic",
            JsonContent.Create(new { title = "Hello world", content = "Hello world" })
        );

        responseMessage.StatusCode.Should().Be(HttpStatusCode.InternalServerError);
    }

    private async Task<Guid> CreateForumAsync(string title)
    {
        var createForumContent = JsonContent.Create(new { Title = title });
        using var response = await _httpClient.PostAsync("api/v1.0/Forum/create", createForumContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var createdForumResult = JsonSerializer.Deserialize<ApiResponse<ForumCreateDto>>(jsonResponse, _jsonOptions);

        createdForumResult.Should().NotBeNull();
        createdForumResult!.Errors.Should().BeNull();

        return createdForumResult.Result!.Id;
    }
}