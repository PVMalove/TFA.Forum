using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Moq;
using Moq.Language.Flow;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Commands.CreateTopic;
using TFA.Forum.Application.Storage.Forum;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.Domain.UnitTests.CreateTopic;

public class CreateTopicShould
{
    private readonly ICreateTopicUseCase sut;
    private readonly Mock<ICreateTopicStorage> storage;
    private readonly ISetup<ICreateTopicStorage, Task<Topic>> createTopicSetup;
    private readonly ISetup<IIdentity,Guid> getCurrentUserIdSetup;
    private readonly ISetup<IIntentionManager,bool> intentionIsAllowedSetup;
    private readonly Mock<IIntentionManager> intentionManager;
    private readonly Mock<IGetAllForumsStorage> getForumsStorage;
    public readonly ISetup<IGetAllForumsStorage, Task<IEnumerable<Entities.Forum>?>> getForumsSetup;


    public CreateTopicShould()
    {
        getForumsStorage = new Mock<IGetAllForumsStorage>();
        getForumsSetup = getForumsStorage.Setup(s => s.GetForums(It.IsAny<CancellationToken>()));
        
        storage = new Mock<ICreateTopicStorage>();
        createTopicSetup = storage.Setup(s =>
            s.CreateTopic(It.IsAny<Guid>(), It.IsAny<Guid>(), It.IsAny<string>(),It.IsAny<string>(), It.IsAny<CancellationToken>()));
        
        var identity = new Mock<IIdentity>();
        var identityProvider = new Mock<IIdentityProvider>();
        identityProvider.Setup(p => p.Current).Returns(identity.Object);
        getCurrentUserIdSetup = identity.Setup(s => s.UserId);

        intentionManager = new Mock<IIntentionManager>();
        intentionIsAllowedSetup = intentionManager.Setup(m => m.IsAllowed(It.IsAny<TopicIntention>()));
        
        var validator = new Mock<IValidator<CreateTopicCommand>>();
        validator.Setup(v => v.ValidateAsync(It.IsAny<CreateTopicCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(new ValidationResult());
        
        sut = new CreateTopicUseCase(validator.Object, intentionManager.Object, identityProvider.Object,storage.Object, getForumsStorage.Object);
    }

    [Fact]
    public async Task ThrowIntentionManagerException_WhenTopicCreationIsNotAllowed()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");

        intentionIsAllowedSetup.Returns(false);

        await sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Some title","Some content"), CancellationToken.None))
            .Should().ThrowAsync<IntentionManagerException>();
        intentionManager.Verify(m => m.IsAllowed(TopicIntention.Create));
    }
    
    [Fact]
    public async Task ReturnNewlyCreatedTopic_WhenMatchingForumExists()
    {
        var forumId = Guid.Parse("E20A64A3-47E3-4076-96D0-7EF226EAF5F2");
        var userId = Guid.Parse("91B714CC-BDFF-47A1-A6DC-E71DDE8C25F7");

        intentionIsAllowedSetup.Returns(true);
        getForumsSetup.ReturnsAsync(new Entities.Forum[] { new() { Id = forumId } });
        getCurrentUserIdSetup.Returns(userId);
        var expected = new Topic();
        createTopicSetup.ReturnsAsync(expected);

        var actual = await sut.Execute(new CreateTopicCommand(forumId, "Some title","Some content"), CancellationToken.None);
        actual.Should().Be(expected);

        storage.Verify(s => s.CreateTopic(forumId, userId, "Some title","Some content", It.IsAny<CancellationToken>()), Times.Once);
    }
    
    [Fact]
    public async Task ThrowForumNotFoundException_WhenNoMatchingForum()
    {
        var forumId = Guid.Parse("1D85F69E-6F37-4A2D-A5B0-12CE45F4DCD7");

        intentionIsAllowedSetup.Returns(true);
        getForumsSetup.ReturnsAsync(Array.Empty<Entities.Forum>());

        await sut.Invoking(s => s.Execute(new CreateTopicCommand(forumId, "Some title","Some content"), CancellationToken.None))
            .Should().ThrowAsync<ForumNotFoundException>();
    }
}