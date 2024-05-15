namespace Blog.Domain.Models;

public record ApiResponse<TItem>(
    bool Success,
    int StatusCode,
    string? Message,
    IEnumerable<TItem>? Data);