using CSharpFunctionalExtensions;
using FluentValidation;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Application.Commands.SignOn;

public class SignOnUseCase : ICommandHandler<IIdentity, SignOnCommand>
{
    private readonly IValidator<SignOnCommand> validator;
    private readonly IPasswordManager passwordManager;
    private readonly ISignOnStorage storage;

    public SignOnUseCase(
        IValidator<SignOnCommand> validator,
        IPasswordManager passwordManager,
        ISignOnStorage storage)
    {
        this.validator = validator;
        this.passwordManager = passwordManager;
        this.storage = storage;
    }

    public async Task<Result<IIdentity, ErrorList>> Execute(SignOnCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);

        var result = new User(userId, Guid.Empty);;
        return result;
    }
}