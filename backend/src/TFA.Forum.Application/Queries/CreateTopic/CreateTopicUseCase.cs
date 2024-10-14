using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.Application.Queries.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICreateTopicStorage storage;

    public CreateTopicUseCase(
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
    }

    public async Task<Topic> Execute(Guid forumId, string title, string content,
        CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        var forumExists = await storage.ForumExists(forumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(forumId);
        }

        return await storage.CreateTopic(forumId, identityProvider.Current.UserId, title, content, cancellationToken);
    }
}