using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Application.Queries.GetTopics;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Domain.ValueObjects;
using TFA.Forum.Persistence.Storage.Forum;
using TFA.Forum.Persistence.Storage.Topic;

namespace TFA.Forum.Domain.UnitTests.GetTopics;

public class GetTopicsUseCaseShould
{
    private readonly GetTopicsUseCase sut;
    private readonly Mock<IGetTopicsStorage> storage;
    private readonly ISetup<IGetTopicsStorage,Task<(IEnumerable<Topic> resources, int totalCount)>> getTopicsSetup;
    private readonly ISetup<IGetAllForumsStorage,Task<IEnumerable<Forum.Domain.Entities.Forum>>> getForumsSetup;

    public GetTopicsUseCaseShould()
    {
        var validator = new Mock<IValidator<GetTopicsQuery>>();
        validator
            .Setup(v => v.ValidateAsync(It.IsAny<GetTopicsQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());

        var getForumsStorage = new Mock<IGetAllForumsStorage>();
        getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));

        storage = new Mock<IGetTopicsStorage>();
        getTopicsSetup = storage.Setup(s =>
            s.GetTopics(It.IsAny<Guid>(), It.IsAny<int>(), It.IsAny<int>(), It.IsAny<CancellationToken>()));

        sut = new GetTopicsUseCase(validator.Object, getForumsStorage.Object, storage.Object);
    }

    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoForum()
    {
        var forumId = Guid.Parse("64C3B227-8D4A-4A0E-A161-04F19C2ABBC4");

        getForumsSetup.ReturnsAsync(new Entities.Forum[] { new( Guid.Parse("01B1C554-184B-4B32-913E-F7031AAD3BAC"), Title.Create("Some title").Value, DateTimeOffset.Now)});

        var query = new GetTopicsQuery(forumId, 0, 1);
        await sut.Invoking(s => s.Execute(query, CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }

    [Fact]
    public async Task ReturnTopics_ExtractedFromStorage_WhenForumExists()
    {
        var forumId = Guid.Parse("845D0972-0E11-4BD2-A102-299E99590267");

        getForumsSetup.ReturnsAsync(new Entities.Forum[] { new( Guid.Parse("845D0972-0E11-4BD2-A102-299E99590267"), Title.Create("Some title").Value, DateTimeOffset.Now)});
        var expectedResources = new Topic[] { new() };
        var expectedTotalCount = 6;
        getTopicsSetup.ReturnsAsync((expectedResources, expectedTotalCount));

        var (actualResources, actualTotalCount) = await sut.Execute(
            new GetTopicsQuery(forumId, 5, 10), CancellationToken.None);

        actualResources.Should().BeEquivalentTo(expectedResources);
        actualTotalCount.Should().Be(expectedTotalCount);
        storage.Verify(s => s.GetTopics(forumId, 5, 10, It.IsAny<CancellationToken>()), Times.Once);
    }
}