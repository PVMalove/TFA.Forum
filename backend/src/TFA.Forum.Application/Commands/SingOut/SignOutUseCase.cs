using CSharpFunctionalExtensions;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Application.Commands.SingOut;

public class SignOutUseCase(
    IIntentionManager intentionManager,
    IIdentityProvider identityProvider,
    ISignOutStorage storage)
    : ICommandHandler<SignOutCommand>
{
    public async Task<UnitResult<ErrorList>> Execute(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntention.SignOut);
        var sessionId = identityProvider.Current.SessionId;
        
        await storage.RemoveSession(sessionId, cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}