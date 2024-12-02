using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using TFA.Forum.Domain.Models;

namespace TFA.Forum.Persistence.Extensions;

public static class QueriesExtensions
{
    public static async Task<PagedList<T>> GetObjectsWithPagination<T>(
        this IQueryable<T> source,
        int page,
        int pageSize,
        CancellationToken token = default)
    {
        var countObjects = await source.CountAsync(token);

        var result = await source
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync(token);

        return new PagedList<T>
        {
            Items = result,
            PageSize = pageSize,
            Page = page,
            TotalCount = countObjects
        };
    }

    public static IQueryable<T> WhereIf<T>(
        this IQueryable<T> source,
        bool condition,
        Expression<Func<T, bool>> predicate)
    {
        return condition ? source.Where(predicate) : source;
    }
}