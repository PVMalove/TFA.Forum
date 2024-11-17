using System.Net.Http.Json;
using FluentAssertions;

namespace TFA.Forum.E2E;

public class ForumEndpointsShould : IClassFixture<ForumApiApplicationFactory>
{
    private readonly ForumApiApplicationFactory factory;

    public ForumEndpointsShould(ForumApiApplicationFactory factory)
    {
        this.factory = factory;
    }

    [Fact]
    public async Task CreateNewForum()
    {
        using var httpClient = factory.CreateClient();

        using var getInitialForumsResponse = await httpClient.GetAsync("api/v1.0/Forum");
        var initialForums = await getInitialForumsResponse.Content.ReadFromJsonAsync<Forum.Domain.Entities.Forum[]>();
        initialForums
            .Should().NotBeNull().And
            .Subject.As<Forum.Domain.Entities.Forum[]>().Should().BeEmpty();

        using var response = await httpClient.PostAsync("api/v1.0/Forum/create_forum",
            JsonContent.Create(new { title = "New forum" }));
        response.Invoking(r => r.EnsureSuccessStatusCode()).Should().NotThrow();

        // var forum = await response.Content.ReadFromJsonAsync<Forum.Domain.Entities.Forum>();
        // forum
        //     .Should().NotBeNull().And
        //     .Subject.As<Forum.Domain.Entities.Forum>().Title.Should().Be("New forum");
        
        using var getForumsResponse = await httpClient.GetAsync("api/v1.0/Forum");
        var forums = await getForumsResponse.Content.ReadFromJsonAsync<Forum.Domain.Entities.Forum[]>();
        forums
            .Should().NotBeNull().And
            .Subject.As<Forum.Domain.Entities.Forum[]>().Should().Contain(f => f.Title.Value == "New forum");
    }
}