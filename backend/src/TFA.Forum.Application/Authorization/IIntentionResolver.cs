using TFA.Forum.Application.Authentication;

namespace TFA.Forum.Application.Authorization;

public interface IIntentionResolver { }

public interface IIntentionResolver<in TIntention> : IIntentionResolver
{
    bool IsAllowed(IIdentity subject, TIntention intention);
}
