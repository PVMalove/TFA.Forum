using FluentValidation;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.Shared;

namespace TFA.Forum.Application.Queries.GetAllForums;

public class GetAllForumsWithPaginationValidator : AbstractValidator<GetAllForumsWithPaginationQuery>
{
    public GetAllForumsWithPaginationValidator()
    {
        RuleFor(v => v.Page)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("Page"));
        
        RuleFor(v => v.PageSize)
            .GreaterThanOrEqualTo(1)
            .WithError(Errors.General.ValueIsInvalid("PageSize"));
    }
}