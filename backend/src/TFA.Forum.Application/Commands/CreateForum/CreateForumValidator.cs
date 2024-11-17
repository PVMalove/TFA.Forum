using FluentValidation;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.ValueObjects;

namespace TFA.Forum.Application.Commands.CreateForum;

public class CreateForumValidator : AbstractValidator<CreateForumCommand>
{
    public CreateForumValidator()
    {
        RuleFor(c => c.Title)
            .MustBeValueObject(Title.Create);
    }
}