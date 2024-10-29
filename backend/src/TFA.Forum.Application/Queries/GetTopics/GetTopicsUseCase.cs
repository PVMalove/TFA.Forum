using FluentValidation;
using TFA.Forum.Application.Queries.GetAllForums;
using TFA.Forum.Application.Storage.Forum;
using TFA.Forum.Application.Storage.Topic;
using TFA.Forum.Domain.Entities;

namespace TFA.Forum.Application.Queries.GetTopics;


public class GetTopicsUseCase : IGetForumsUseCase
{
    private readonly IValidator<GetTopicsQuery> validator;
    private readonly IGetAllForumsStorage getForumsStorage;
    private readonly IGetTopicsStorage getTopicsStorage;

    public GetTopicsUseCase(IValidator<GetTopicsQuery> validator, IGetAllForumsStorage getForumsStorage, 
        IGetTopicsStorage getTopicsStorage)
    {
        this.validator = validator;
        this.getForumsStorage = getForumsStorage;
        this.getTopicsStorage = getTopicsStorage;
    }

    public async Task<(IEnumerable<Topic> resources, int totalCount)> Execute(GetTopicsQuery query, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(query, cancellationToken);
        await getForumsStorage.ThrowIfForumNotFound(query.ForumId, cancellationToken);
        return await getTopicsStorage.GetTopics(query.ForumId, query.Skip, query.Take, cancellationToken);
    }
}