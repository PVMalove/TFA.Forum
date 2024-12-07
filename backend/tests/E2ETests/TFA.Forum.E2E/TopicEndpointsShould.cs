using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.E2E;

public class TopicEndpointsShould(ForumApiApplicationFactory factory)
    : IClassFixture<ForumApiApplicationFactory>, IAsyncLifetime
{
    private readonly HttpClient httpClient = factory.CreateClient();
    private readonly JsonSerializerOptions jsonOptions = new() { PropertyNameCaseInsensitive = true };

    public Task InitializeAsync() => Task.CompletedTask;

    public Task DisposeAsync() => Task.CompletedTask;

    [Fact]
    public async Task ReturnForbidden_WhenNotAuthenticated()
    {
        var forumTitle = Guid.NewGuid().ToString();
        var forumId = await CreateForumAsync(forumTitle);

        var responseMessage = await httpClient.PostAsync(
            $"api/v1.0/Forums/{forumId}/topics",
            JsonContent.Create(new { title = "Hello world", content = "Hello world" })
        );

        responseMessage.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    private async Task SignInAfterSignOn()
    {
        using var signOnResponse = await httpClient.PostAsync(
            "api/v1.0/Account/sign_on", JsonContent.Create(new { login = "Test", password = "qwerty" }));
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();

        var jsonResponse = await signOnResponse.Content.ReadAsStringAsync();
        var createdUser = JsonSerializer.Deserialize<ApiResponse<User>>(jsonResponse, jsonOptions);
        createdUser.Should().NotBeNull();
        createdUser!.Errors.Should().BeNull();
        
        using var signInResponse = await httpClient.PostAsync(
            "api/v1.0/Account/sign_in", JsonContent.Create(new { login = "Test", password = "qwerty" }));
        signInResponse.IsSuccessStatusCode.Should().BeTrue();
        
        jsonResponse = await signOnResponse.Content.ReadAsStringAsync();
        var signedInUser = JsonSerializer.Deserialize<ApiResponse<User>>(jsonResponse, jsonOptions);
        
        signedInUser.Should().NotBeNull();
        signedInUser!.Errors.Should().BeNull();

        signedInUser.Result.Should()
            .NotBeNull().And
            .BeEquivalentTo(createdUser.Result);
    }
    
    private async Task<Guid> CreateForumAsync(string title)
    {
        await SignInAfterSignOn();
        var createForumContent = JsonContent.Create(new { Title = title });
        using var response = await httpClient.PostAsync("api/v1.0/Forums", createForumContent);

        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var createdForumResult = JsonSerializer.Deserialize<ApiResponse<ForumCreateDto>>(jsonResponse, jsonOptions);

        createdForumResult.Should().NotBeNull();
        createdForumResult!.Errors.Should().BeNull();
        
        using var signOnResponse = await httpClient.PostAsync("api/v1.0/Account/sign_out", null);
        signOnResponse.IsSuccessStatusCode.Should().BeTrue();
        
        return createdForumResult.Result!.Id;
    }
}