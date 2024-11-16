using FluentValidation;
using TFA.Forum.Persistence.Configurations;
using TFA.Forum.Persistence.Shared;


namespace TFA.Forum.Application.Commands.CreateTopic;

public class CreateTopicValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode("Empty");
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH_100).WithErrorCode("Too long");
        RuleFor(c => c.Content)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(Constants.MAX_LOW_TEXT_LENGTH_100).WithErrorCode("Too long");
    }
}