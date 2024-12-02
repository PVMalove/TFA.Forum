using CSharpFunctionalExtensions;
using FluentValidation;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Domain.DTO.Topic;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;
using TFA.Forum.Persistence.Storage.Topic;

namespace TFA.Forum.Application.Commands.CreateTopic;

public class CreateTopicUseCase : ICommandHandler<TopicCreateDto, CreateTopicCommand>
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

    public async Task<Result<TopicCreateDto, ErrorList>> Execute(CreateTopicCommand command,
        CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();
        
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        await getForumsStorage.ThrowIfForumNotFound(command.ForumId, cancellationToken);

        var result = await storage.CreateTopic(command.ForumId, identityProvider.Current.UserId, command.Title, command.Content, cancellationToken);
        return result;
    }
}