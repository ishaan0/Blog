namespace Blog.Domain.Models;

public class ApiErrorResponse
{
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; }

    public ApiErrorResponse(string message, int statusCode, List<string>? errors = null)
    {
        Message = message;
        StatusCode = statusCode;
        Errors = errors ?? new List<string>();
    }
}

