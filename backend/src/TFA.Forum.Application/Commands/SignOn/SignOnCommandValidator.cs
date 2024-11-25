using FluentValidation;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Commands.SignOn;

public class SignOnCommandValidator : AbstractValidator<SignOnCommand>
{
    public SignOnCommandValidator()
    {
        RuleFor(c => c.Login)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("login"))
            .MinimumLength(Constants.MIN_LOW_TEXT_LENGTH_3).WithError(Errors.General.ValueIsInvalid("login"))
            .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH_25).WithError(Errors.General.ValueIsInvalid("login"));
        RuleFor(c => c.Password)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("password"))
            .MinimumLength(Constants.MIN_LOW_TEXT_LENGTH_6).WithError(Errors.General.ValueIsInvalid("password"));
    }
}