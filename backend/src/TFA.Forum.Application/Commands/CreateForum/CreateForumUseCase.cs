using CSharpFunctionalExtensions;
using AutoMapper;
using FluentValidation;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Commands.CreateForum;

public class CreateForumUseCase : ICommandHandler<ForumCreateDto, CreateForumCommand>
{
    readonly IValidator<CreateForumCommand> validator;
    private readonly IIntentionManager intentionManager;
    private readonly ICreateForumStorage storage;
    private readonly IMapper mapper;

    public CreateForumUseCase(IValidator<CreateForumCommand> validator, IIntentionManager intentionManager,
        ICreateForumStorage storage, IMapper mapper)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.storage = storage;
        this.mapper = mapper;
    }

    public async Task<Result<ForumCreateDto, ErrorList>> Execute(CreateForumCommand command,
        CancellationToken cancellationToken)
    {
        var validateResult = await validator.ValidateAsync(command, cancellationToken);

        if (!validateResult.IsValid)
            return validateResult.ToList();

        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        var result = await storage.Create(command.Title, cancellationToken);
        return result;
        //ForumCreateDto forumCreateDto = mapper.Map<ForumCreateDto>(result);
        //return forumCreateDto;
    }
}