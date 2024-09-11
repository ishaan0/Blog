using Blog.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Helpers;

public class ApiResponseHelper
{
    public static IActionResult Success<T>(T data, string? message = null)
    {
        var response = new ApiResponse<T>(data, message);
        return new OkObjectResult(response);
    }

    public static IActionResult Created<T>(string actionName, T data, string message = "Resource created successfully")
    {
        var response = new ApiResponse<T>(data, message);
        return new CreatedAtActionResult(actionName, null, null, response);

    }

    public static IActionResult NoContent(string message = "No content")
    {
        return new NoContentResult();
    }

    public static IActionResult Error(string message, int statusCode, List<string> errors = null)
    {
        var errorResponse = new ApiErrorResponse(message, statusCode, errors);
        return new ObjectResult(errorResponse) { StatusCode = statusCode };
    }

    public static IActionResult BadRequest(string message = "Invalid request", List<string> errors = null)
    {
        return Error(message, StatusCodes.Status400BadRequest, errors);
    }

    public static IActionResult NotFound(string message = "Resource not found")
    {
        return Error(message, StatusCodes.Status404NotFound);
    }
}
