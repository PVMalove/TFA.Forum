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


public class GetTopicsUseCase : IQueryHandler<PagedList<TopicGetDto>, GetTopicsWithPaginationQuery>
{
    private readonly IValidator<GetTopicsWithPaginationQuery> validator;
    private readonly IGetAllForumsStorage getForumsStorage;
    private readonly IGetTopicsStorage getTopicsStorage;

    public GetTopicsUseCase(IValidator<GetTopicsWithPaginationQuery> validator, IGetAllForumsStorage getForumsStorage, 
        IGetTopicsStorage getTopicsStorage)
    {
        this.validator = validator;
        this.getForumsStorage = getForumsStorage;
        this.getTopicsStorage = getTopicsStorage;
    }

    public async Task<Result<PagedList<TopicGetDto>, ErrorList>> Execute(GetTopicsWithPaginationQuery query, CancellationToken cancellationToken = default)
    {
        var validationResult = await validator.ValidateAsync(query, cancellationToken);
        if (validationResult.IsValid == false)
            return validationResult.ToList();
        
        await getForumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);
        
        var result = await getTopicsStorage.GetTopicsWithPagination(query.ForumId, query.SortBy, query.SortDirection, query.Page, query.PageSize, cancellationToken);
        return result;
    }
}