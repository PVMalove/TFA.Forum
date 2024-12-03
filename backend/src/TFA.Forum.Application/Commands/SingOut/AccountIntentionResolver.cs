using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;

namespace TFA.Forum.Application.Commands.SingOut;

public class AccountIntentionResolver : IIntentionResolver<AccountIntention>
{
    public bool IsAllowed(IIdentity subject, AccountIntention intention) => intention switch
    {
        AccountIntention.SignOut => subject.IsAuthenticated(),
        _ => false,
    };
}