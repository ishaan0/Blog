using Blog.Domain.Enums;

namespace Blog.Api.Dtos.Common;

public class ResourcesQueryRequest
{
    private const int MaxPageSize = 20;
    private int _pageSize;
    public SortOrder? SortOrder { get; init; }
    public string? SortColumn { get; init; }
    public int PageNumber { get; init; } = 1;

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = Math.Min(value, MaxPageSize);
    }
}
