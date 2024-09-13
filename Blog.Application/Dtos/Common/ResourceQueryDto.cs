using Blog.Domain.Enums;

namespace Blog.Application.Dtos.Common;

public class ResourceQueryDto
{
    private const int MaxPageSize = 20;
    private const int MinValue = 1;
    private int _pageSize;
    private int _pageNumber;
    public SortOrder? SortOrder { get; init; }
    public string? SortColumn { get; init; }
    public int PageNumber
    {
        get => _pageNumber;
        set => _pageNumber = Math.Max(value, MinValue);
    }
    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : Math.Max(value, MinValue);
    }
}
