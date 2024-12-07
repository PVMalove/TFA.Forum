using CSharpFunctionalExtensions;
using FluentValidation;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Commands.CreateForum;

public class CreateForumUseCase(
    IValidator<CreateForumCommand> validator,
    IIntentionManager intentionManager,
    ICreateForumStorage storage)
    : ICommandHandler<ForumCreateDto, CreateForumCommand>
{
    public async Task<Result<ForumCreateDto, ErrorList>> Execute(CreateForumCommand command,
        CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);
        if (!validateResult.IsValid)
            return validateResult.ToList();

        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        var result = await storage.Create(command.Title, cancellationToken);
        
        return result;
    }
}