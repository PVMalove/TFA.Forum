using FluentValidation;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Domain.Entities;
using TFA.Forum.Persistence.Storage.Forum;
using TFA.Forum.Persistence.Storage.Topic;

namespace TFA.Forum.Application.Commands.CreateTopic;

public class CreateTopicUseCase : ICreateTopicUseCase
{
    private readonly IValidator<CreateTopicCommand> validator;
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ICreateTopicStorage storage;
    private readonly IGetAllForumsStorage getForumsStorage;

    public CreateTopicUseCase(
        IValidator<CreateTopicCommand> validator,
        IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ICreateTopicStorage storage,
        IGetAllForumsStorage getForumsStorage)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
        this.getForumsStorage = getForumsStorage;
    }

    public async Task<Topic> Execute(CreateTopicCommand command,
        CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        await getForumsStorage.ThrowIfForumNotFound(command.ForumId, cancellationToken);

        return await storage.CreateTopic(command.ForumId, identityProvider.Current.UserId, command.Title, command.Content, cancellationToken);
    }
}