using FluentValidation;

namespace TFA.Forum.Application.Queries.CreateTopic;

public class CreateTopicValidator : AbstractValidator<CreateTopicQuery>
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