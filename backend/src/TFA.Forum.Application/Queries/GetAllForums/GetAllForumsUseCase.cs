using CSharpFunctionalExtensions;
using FluentValidation;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Domain.DTO.Forum;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;

namespace TFA.Forum.Application.Queries.GetAllForums;

public class GetAllForumsUseCase(IGetAllForumsStorage storage, IValidator<GetAllForumsWithPaginationQuery> validator) : IQueryHandler<PagedList<ForumsDto>, GetAllForumsWithPaginationQuery>
{
    public async Task<Result<PagedList<ForumsDto>, ErrorList>> Execute(GetAllForumsWithPaginationQuery query, CancellationToken token)
    {
        var validationResult = await validator.ValidateAsync(query, token);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        var result = await storage.GetForumsWithPagination(query.SortBy, query.SortDirection, query.Page, query.PageSize, token);
        return result;
    }
}