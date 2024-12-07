using CSharpFunctionalExtensions;
using FluentValidation;
using TFA.Forum.Application.Abstractions;
using TFA.Forum.Application.Extensions;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Domain.DTO.Topic;
using TFA.Forum.Domain.Models;
using TFA.Forum.Domain.Shared;
using TFA.Forum.Persistence.Storage.Forum;
using TFA.Forum.Persistence.Storage.Topic;

namespace TFA.Forum.Application.Queries.GetTopics;

public class GetTopicsUseCase(
    IValidator<GetTopicsWithPaginationQuery> validator,
    IGetAllForumsStorage getForumsStorage,
    IGetTopicsStorage getTopicsStorage)
    : IQueryHandler<PagedList<TopicGetDto>, GetTopicsWithPaginationQuery>
{
    public async Task<Result<PagedList<TopicGetDto>, ErrorList>> Execute(GetTopicsWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (!validationResult.IsValid)
            return validationResult.ToList();
        
        var forumResult = await getForumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);
        if (forumResult.IsFailure)
            return forumResult.Error.ToErrorList();
        
        var result = await getTopicsStorage.GetTopicsWithPagination(query.ForumId, query.SortBy, query.SortDirection, query.Page, query.PageSize, cancellationToken);
        return result;
    }
}