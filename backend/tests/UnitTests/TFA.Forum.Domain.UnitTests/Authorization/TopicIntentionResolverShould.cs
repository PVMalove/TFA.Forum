﻿using FluentAssertions;
using Moq;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Commands.CreateTopic;

namespace TFA.Forum.Domain.UnitTests.Authorization;

public class TopicIntentionResolverShould
{
    private readonly TopicIntentionResolver sut = new();
    
    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum()
    {
        var intention = (TopicIntention) (-1);
        sut.IsAllowed(new Mock<IIdentity>().Object, intention).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WhenCheckingTopicCreateIntention_AndUserIsGuest()
    {
        sut.IsAllowed(User.Guest, TopicIntention.Create).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrue_WhenCheckingTopicCreateIntention_AndUserIsAuthenticated()
    {
        sut.IsAllowed(new User(Guid.Parse("6F5C56BD-25EB-4BDC-9604-F19DAE2963A4"), Guid.Empty), TopicIntention.Create)
            .Should().BeTrue();
    }
}