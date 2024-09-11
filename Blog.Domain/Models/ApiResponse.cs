namespace Blog.Domain.Models;

public class ApiResponse<T>
{
    public T Data { get; set; }
    public string Message { get; set; }
    public int StatusCode { get; set; }
    public List<string> Errors { get; set; }

    public ApiResponse(T data, string message = "Request successful")
    {
        Data = data;
        Message = message;
        StatusCode = 200;
        Errors = new List<string>();
    }
}