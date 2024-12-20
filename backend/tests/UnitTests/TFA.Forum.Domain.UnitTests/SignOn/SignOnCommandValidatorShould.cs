﻿using FluentAssertions;
using TFA.Forum.Application.Commands.SignOn;

namespace TFA.Forum.Domain.UnitTests.SignOn;

public class SignOnCommandValidatorShould
{
    private readonly SignOnCommandValidator sut = new();

    [Fact]
    public void ReturnSuccess_WhenCommandValid()
    {
        var validCommand = new SignOnCommand("Test", "qwerty");
        sut.Validate(validCommand).IsValid.Should().BeTrue();
    }

    public static IEnumerable<object[]> GetInvalidCommands()
    {
        var validCommand = new SignOnCommand("Test", "qwerty");
        yield return new object[] { validCommand with { Login = string.Empty } };
        yield return new object[] { validCommand with { Login = "12345678901234567890123456" } };
        yield return new object[] { validCommand with { Login = "   " } };
        yield return new object[] { validCommand with { Password = string.Empty } };
        yield return new object[] { validCommand with { Password = "     " } };
    }

    [Theory]
    [MemberData(nameof(GetInvalidCommands))]
    public void ReturnFailure_WhenCommandInvalid(SignOnCommand command)
    {
        sut.Validate(command).IsValid.Should().BeFalse();
    }
}