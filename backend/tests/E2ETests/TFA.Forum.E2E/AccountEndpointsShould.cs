using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using TFA.Forum.Application.Authentication;
using Xunit.Abstractions;

namespace TFA.Forum.E2E;

public class AccountEndpointsShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly ForumApiApplicationFactory factory;
    private readonly ITestOutputHelper testOutputHelper;
    private readonly JsonSerializerOptions jsonOptions;
    
    public AccountEndpointsShould(
        ForumApiApplicationFactory factory,
        ITestOutputHelper testOutputHelper)
    {
        this.factory = factory;
        this.testOutputHelper = testOutputHelper;
        jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
    }

    [Fact]
    public async Task SignInAfterSignOn()
    {
        using var httpClient = factory.CreateClient();

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
        
        signInResponse.Headers.Should().ContainKey("TFA-Auth-Token");
        testOutputHelper.WriteLine(string.Join(Environment.NewLine,
            signInResponse.Headers.Select(h => $"{h.Key} = {string.Join(", ", h.Value)}")));
        
        jsonResponse = await signOnResponse.Content.ReadAsStringAsync();
        var signedInUser = JsonSerializer.Deserialize<ApiResponse<User>>(jsonResponse, jsonOptions);
        
        signedInUser.Should().NotBeNull();
        signedInUser!.Errors.Should().BeNull();

        signedInUser.Result.Should()
            .NotBeNull().And
            .BeEquivalentTo(createdUser.Result);
    }
}