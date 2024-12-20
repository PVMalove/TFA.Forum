﻿using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Authorization;

namespace TFA.Forum.Application.Commands.CreateTopic;

public class TopicIntentionResolver : IIntentionResolver<TopicIntention>
{
    public bool IsAllowed(IIdentity subject, TopicIntention intention) => intention switch
    {
        TopicIntention.Create => subject.IsAuthenticated(),
        _ => false
    };
}