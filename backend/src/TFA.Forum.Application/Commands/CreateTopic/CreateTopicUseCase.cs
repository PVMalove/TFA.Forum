using FluentValidation;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Domain.Exceptions;

namespace TFA.Forum.Application.Commands.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICreateTopicStorage storage;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
    }

    public async Task<Topic> Execute(CreateTopicCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        
        intentionManager.ThrowIfForbidden(TopicIntention.Create);

        var forumExists = await storage.ForumExists(command.ForumId, cancellationToken);
        if (!forumExists)
        {
            throw new ForumNotFoundException(command.ForumId);
        }

        return await storage.CreateTopic(command.ForumId, identityProvider.Current.UserId, command.Title, command.Content, cancellationToken);
    }
}