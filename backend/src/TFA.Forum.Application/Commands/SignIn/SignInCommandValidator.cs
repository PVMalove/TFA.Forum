using FluentValidation;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Commands.SignIn;

public class SignInCommandValidator : AbstractValidator<SignInCommand>
{
    public SignInCommandValidator()
    {
        RuleFor(c => c.Login).Cascade(CascadeMode.Stop)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("login"));
        RuleFor(c => c.Password)
            .NotEmpty().WithError(Errors.General.ValueIsRequired("password"));
    }
}