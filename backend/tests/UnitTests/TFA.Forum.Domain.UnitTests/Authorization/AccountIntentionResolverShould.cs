using FluentAssertions;
using Moq;
using TFA.Forum.Application.Authentication;
using TFA.Forum.Application.Commands.SingOut;

namespace TFA.Forum.Domain.UnitTests.Authorization;

public class AccountIntentionResolverShould
{
    private readonly AccountIntentionResolver sut = new();
    
    [Fact]
    public void ReturnFalse_WhenIntentionNotInEnum()
    {
        var intention = (AccountIntention) (-1);
        sut.IsAllowed(new Mock<IIdentity>().Object, intention).Should().BeFalse();
    }

    [Fact]
    public void ReturnFalse_WhenCheckingForumCreateIntention_AndUserIsGuest()
    {
        sut.IsAllowed(User.Guest, AccountIntention.SignOut).Should().BeFalse();
    }

    [Fact]
    public void ReturnTrue_WhenCheckingForumCreateIntention_AndUserIsAuthenticated()
    {
        sut.IsAllowed(new User(Guid.Parse("6F5C56BD-25EB-4BDC-9604-F19DAE2963A4"), Guid.Empty), AccountIntention.SignOut)
            .Should().BeTrue();
    }
}