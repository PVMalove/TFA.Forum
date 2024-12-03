using CSharpFunctionalExtensions;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.User;

namespace TFA.Forum.Application.Commands.SingOut;

public class SignOutUseCase : ICommandHandler<SignOutCommand>
{
    private readonly IIntentionManager intentionManager;
    private readonly IIdentityProvider identityProvider;
    private readonly ISignOutStorage storage;
    
    public SignOutUseCase(IIntentionManager intentionManager,
        IIdentityProvider identityProvider,
        ISignOutStorage storage)
    {
        this.intentionManager = intentionManager;
        this.identityProvider = identityProvider;
        this.storage = storage;
    }

    public async Task<UnitResult<ErrorList>> Execute(SignOutCommand command, CancellationToken cancellationToken)
    {
        intentionManager.ThrowIfForbidden(AccountIntention.SignOut);
        var sessionId = identityProvider.Current.SessionId;
        
        await storage.RemoveSession(sessionId, cancellationToken);
        return UnitResult.Success<ErrorList>();
    }
}