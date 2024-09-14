namespace Blog.Domain.Models;

public record PaginationMetadata(int TotalCount, int CurrentPage, int PageSize)
{
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / Math.Max(PageSize, 1));
}
