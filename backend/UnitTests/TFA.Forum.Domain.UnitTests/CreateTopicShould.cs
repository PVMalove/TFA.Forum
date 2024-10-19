using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Queries.CreateTopic;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;


namespace TFA.Forum.Domain.UnitTests;

public class CreateTopicShould
{
    private readonly ICreateTopicUseCase sut;
    private readonly Mock<ICreateTopicStorage> storage;
    private readonly ISetup<ICreateTopicStorage, Task<bool>> forumExistsSetup;
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
    private readonly ISetup<IIdentity,Guid> getCurrentUserIdSetup;
    private readonly ISetup<IIntentionManager,bool> intentionIsAllowedSetup;

    public CreateTopicShould()
    {
        storage = new Mock<ICreateTopicStorage>();
        forumExistsSetup = storage.Setup(s => s.ForumExists(It.IsAny<Guid>(), It.IsAny<CancellationToken>()));
        createTopicSetup = storage.Setup(s =>
            s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(),It.IsAny<string>(), It.IsAny<CancellationToken>()));
        
        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        getCurrentUserIdSetup = identity.Setup(s => s.UserId);

        Mock<IIntentionManager> intentionManager = new();
        intentionIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));
        
        var validator = new Mock<IValidator<CreateTopicQuery>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<CreateTopicQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        sut = new CreateTopicUseCase(validator.Object, intentionManager.Object, identityProvider.Object, storage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("5E1DCF96-E8F3-41C9-BD59-6479140933B3");

        intentionIsAllowedSetup.Returns(true);
        forumExistsSetup.ReturnsAsync(false);

        await sut.Invoking(s => s.Execute(new CreateTopicQuery(forumId, "Some title","Some content"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
        storage.Verify(s => s.ForumExists(forumId, It.IsAny<CancellationToken>()));
    }
    
    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");
        var userId = Guid.Parse("91B714CC-BDFF-47A1-A6DC-E71DDE8C25F7");

        intentionIsAllowedSetup.Returns(true);
        forumExistsSetup.ReturnsAsync(true);
        getCurrentUserIdSetup.Returns(userId);
        var expected = new Topic();
        createTopicSetup.ReturnsAsync(expected);

        var actual = await sut.Execute(new CreateTopicQuery(forumId, "Some title","Some content"), CancellationToken.None);
        actual.Should().Be(expected);

        storage.Verify(s => s.CreateTopic(forumId, userId, "Some title","Some content", It.IsAny<CancellationToken>()), Times.Once);
    }
}