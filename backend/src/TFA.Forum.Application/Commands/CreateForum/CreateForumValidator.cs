using FluentValidation;
using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Persistence.Configurations;


namespace TFA.Forum.Application.Commands.CreateForum;

public class CreateForumValidator : AbstractValidator<CreateForumCommand>
{
    public CreateForumValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithErrorCode(ValidationErrorCode.Empty)
            .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH_50).WithErrorCode(ValidationErrorCode.TooLong);
    }
}