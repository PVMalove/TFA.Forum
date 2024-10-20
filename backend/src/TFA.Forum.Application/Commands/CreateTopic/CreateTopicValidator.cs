using FluentValidation;

namespace TFA.Forum.Application.Commands.CreateTopic;

public class CreateTopicValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode("Empty");
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(100).WithErrorCode("Too long");
        RuleFor(c => c.Content)
            .NotEmpty().WithErrorCode("Empty")
            .MaximumLength(100).WithErrorCode("Too long");
    }
}