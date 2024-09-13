using Blog.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Extensions;

public static class PaginationExtensions
{
    public static async Task<PaginatedList<TDestination>> ToPaginatedList<TSource, TDestination>(
        this IQueryable<TSource> queryable,
        int pageNumber, int pageSize,
        Func<TSource, TDestination> mapFunction)
    {
        var count = await queryable.CountAsync();
        var items = await queryable.GetPage(pageNumber, pageSize).ToListAsync();

        var mappedItems = items.Select(mapFunction).ToList();

        return new PaginatedList<TDestination>(
            mappedItems,
            new PaginationMetadata(count, pageNumber, pageSize));
    }

    public static IQueryable<TItem> GetPage<TItem>(
        this IQueryable<TItem> queryable,
        int pageNumber, int pageSize)
    {
        return queryable
            .Skip(pageSize * (pageNumber - 1))
            .Take(pageSize);
    }

    public static async Task<PaginationMetadata> GetPaginationMetadataAsync<TItem>(
        this IQueryable<TItem> queryable,
        int pageNumber, int pageSize)
    {
        return new PaginationMetadata(
          await queryable.CountAsync(),
          pageNumber,
          pageSize);
    }
}
