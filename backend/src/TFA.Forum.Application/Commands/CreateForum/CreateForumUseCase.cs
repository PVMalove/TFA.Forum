﻿using FluentValidation;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Application.Storage.Forum;

namespace TFA.Forum.Application.Commands.CreateForum;

public class CreateForumUseCase : ICreateForumUseCase
{
    readonly IValidator<CreateForumCommand> validator;
    private readonly IIntentionManager intentionManager;
    private readonly ICreateForumStorage storage;

    public CreateForumUseCase(IValidator<CreateForumCommand> validator, IIntentionManager intentionManager,
        ICreateForumStorage storage)
    {
        this.validator = validator;
        this.intentionManager = intentionManager;
        this.storage = storage;
    }


    public async Task<Domain.Entities.Forum> Execute(CreateForumCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        intentionManager.ThrowIfForbidden(ForumIntention.Create);

        return await storage.Create(command.Title, cancellationToken);
    }
}