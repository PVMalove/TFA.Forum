using FluentValidation;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Application.Commands.CreateTopic;

public class CreateTopicValidator : AbstractValidator<CreateTopicCommand>
{
    public CreateTopicValidator()
    {
        RuleFor(c => c.ForumId).NotEmpty().WithErrorCode("Empty");
        RuleFor(c => c.Title).Cascade(CascadeMode.Stop)
            .MustBeValueObject(Title.Create);
        RuleFor(c => c.Content)
            .MustBeValueObject(Content.Create);
    }
}