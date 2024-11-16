using AutoMapper;
using FluentValidation;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Commands.CreateForum;

public class CreateForumUseCase : ICreateForumUseCase
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


    public async Task<ForumCreateDto> Execute(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        var result = await storage.Create(command.Title, cancellationToken);
        return mapper.Map<ForumCreateDto>(result);
    }
}