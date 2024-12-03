using FluentValidation;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Exceptions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Queries.GetTopics;

public class GetTopicsWithPaginationValidator : AbstractValidator<GetTopicsWithPaginationQuery>
{
    public GetTopicsWithPaginationValidator()
    {
        RuleFor(q => q.ForumId).NotEmpty().WithErrorCode(ValidationErrorCode.Empty);
        
        RuleFor(v => v.Page).Cascade(CascadeMode.Stop)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("page"));
        
        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("pageSize"));
    }
}