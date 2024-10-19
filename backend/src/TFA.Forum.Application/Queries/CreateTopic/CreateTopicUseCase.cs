using FluentValidation;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.Application.Queries.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicQuery> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICreateTopicStorage storage;

    public CreateTopicUseCase(
        IValidator<CreateTopicQuery> validator,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
    }

    public async Task<Topic> Execute(CreateTopicQuery query,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        var forumExists = await storage.ForumExists(query.ForumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(query.ForumId);
        }

        return await storage.CreateTopic(query.ForumId, identityProvider.Current.UserId, query.Title, query.Content, cancellationToken);
    }
}