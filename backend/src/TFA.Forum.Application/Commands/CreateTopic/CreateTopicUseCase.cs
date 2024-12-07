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

public class CreateTopicUseCase(
    IValidator<CreateTopicCommand> validator,
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ICreateTopicStorage storage,
    IGetAllForumsStorage getForumsStorage)
    : ICommandHandler<TopicCreateDto, CreateTopicCommand>
{
    public async Task<Result<TopicCreateDto, ErrorList>> Execute(CreateTopicCommand command,
        CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();
        
        intentionManager.ThrowIfForbidden(TopicIntention.Create);
        
        var forumResult = await getForumsStorage.ThrowIfForumNotFound(command.ForumId, cancellationToken);
        if (forumResult.IsFailure)
            return forumResult.Error.ToErrorList();
        
        var result = await storage.CreateTopic(command.ForumId, identityProvider.Current.UserId, command.Title, command.Content, cancellationToken);
        return result;
    }
}