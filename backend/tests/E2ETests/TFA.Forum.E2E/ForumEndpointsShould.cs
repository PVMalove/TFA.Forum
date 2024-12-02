using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TFA.Forum.Domain.DTO.Forum;

namespace TFA.Forum.E2E;

public class ForumEndpointsShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly HttpClient httpClient;
    private readonly JsonSerializerOptions jsonOptions;

    public ForumEndpointsShould(ForumApiApplicationFactory factory)
    {
        httpClient = factory.CreateClient();
        jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [Fact]
    public async Task CreateNewForum_ShouldCreateForum_WhenValidRequest()
    {
        const string forumTitle = "DDAB3629-0BD9-4842-9C70-310A51694ACC";

        await VerifyForumDoesNotExist(forumTitle);
        await CreateForum(forumTitle);
        await VerifyForumExists(forumTitle);
    }

    private async Task VerifyForumDoesNotExist(string forumTitle)
    {
        var response = await httpClient.GetAsync("api/v1.0/Forum");
        response.EnsureSuccessStatusCode();

        var initialForums = await response.Content.ReadFromJsonAsync<ForumGetDto[]>();
        initialForums.Should().NotBeNull()
            .And.NotContain(f => f.Title.Equals(forumTitle));
    }

    private async Task CreateForum(string forumTitle)
    {
        var createForumContent = JsonContent.Create(new { Title = forumTitle });
        var response = await httpClient.PostAsync("api/v1.0/Forum/create", createForumContent);
        
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<ForumCreateDto>>(jsonResponse, jsonOptions);

        result.Should().NotBeNull();
        result!.Errors.Should().BeNull();
        result.Result.Should().NotBeNull()
            .And.Subject.As<ForumCreateDto>().Title.Should().Be(forumTitle);
    }

    private async Task VerifyForumExists(string forumTitle)
    {
        var response = await httpClient.GetAsync("api/v1.0/Forum");
        response.EnsureSuccessStatusCode();

        var forums = await response.Content.ReadFromJsonAsync<ForumCreateDto[]>();
        forums.Should().NotBeNull()
            .And.Contain(f => f.Title!.Equals(forumTitle));
    }
}