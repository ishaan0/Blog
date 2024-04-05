using Blog.Domain.Enums;
using System.Linq.Expressions;

namespace Blog.Domain.Models;

public record PaginationQuery<TEntity>(
    Expression<Func<TEntity, bool>> Filter,
    SortOrder SortOrder,
    string? SortColumn,
    int PageNumber,
    int PageSize);