using CSharpFunctionalExtensions;
using MediatR;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Application.Commands.SignOn;

public class SignOnUseCase(
    IPasswordManager passwordManager,
    ISignOnStorage storage)
    : IRequestHandler<SignOnCommand, Result<IIdentity, ErrorList>>
{
    public async Task<Result<IIdentity, ErrorList>> Handle(SignOnCommand command, CancellationToken cancellationToken)
    {
        var (salt, hash) = passwordManager.GeneratePasswordParts(command.Password);
        var userId = await storage.CreateUser(command.Login, salt, hash, cancellationToken);

        var result = new User(userId, Guid.Empty);;
        return result;
    }
}