using FluentAssertions;
using TFA.Forum.Application.Commands.SignIn;

namespace TFA.Forum.Domain.UnitTests.SignIn;

public class SignInCommandValidatorShould
{
    private readonly SignInCommandValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var command = new SignInCommand("Test", "qwerty", false);
        sut.Validate(command).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var command = new SignInCommand("Test", "qwerty", false);
        yield return [command with { Login = string.Empty }];
        yield return [command with { Login = "  " }];
        yield return [command with { Login = "12345678901234567890123456" }];
        yield return [command with { Password = "      " }];
        yield return [command with { Password = string.Empty }];
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void ReturnFailure_WhenCommandInvalid(SignInCommand command)
    {
        sut.Validate(command).IsValid.Should().BeFalse();
    }
}