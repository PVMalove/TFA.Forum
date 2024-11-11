using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;

namespace TFA.Forum.Application.Commands.CreateForum;

public class ForumIntentionResolver : IIntentionResolver<ForumIntention>
{
    public bool IsAllowed(IIdentity subject, ForumIntention intention) => intention switch
    {
        ForumIntention.Create => subject.IsAuthenticated(),
        _ => false
    };
}