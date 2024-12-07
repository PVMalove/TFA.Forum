using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TFA.Forum.Application.Authentication;
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

        await SignInAfterSignOn();
        await VerifyForumDoesNotExist(forumTitle);
        await CreateForum(forumTitle);
        await VerifyForumExists(forumTitle);
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

    private async Task VerifyForumDoesNotExist(string forumTitle)
    {
        var response = await httpClient.GetAsync("api/v1.0/Forums");
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<ForumGetDto[]>>(jsonResponse, jsonOptions);
        result.Should().NotBeNull();
        result!.Errors.Should().BeNull();
        result.Result.Should().NotBeNull()
            .And.NotContain(f => f.Title.Equals(forumTitle));
    }

    private async Task CreateForum(string forumTitle)
    {
        var createForumContent = JsonContent.Create(new { Title = forumTitle });
        var response = await httpClient.PostAsync("api/v1.0/Forums", createForumContent);
        
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
        var response = await httpClient.GetAsync("api/v1.0/Forums");
        response.EnsureSuccessStatusCode();

        var jsonResponse = await response.Content.ReadAsStringAsync();
        var result = JsonSerializer.Deserialize<ApiResponse<ForumCreateDto[]>>(jsonResponse, jsonOptions);
        
        result.Should().NotBeNull();
        result!.Errors.Should().BeNull();
        result.Result.Should().NotBeNull()
            .And.Contain(f => f.Title!.Equals(forumTitle));
    }
}