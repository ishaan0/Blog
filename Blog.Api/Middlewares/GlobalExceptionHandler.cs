using Blog.Api.Helpers;
using Blog.Domain.Exceptions;
using Blog.Domain.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Blog.Api.Middlewares
{
    public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
    {
        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            LogException(exception);

            var (statusCode, title, detail) = MapExceptionToProblemInformation(exception);

            var responseObjectResult = ApiResponseHelper.Error(
                    title, statusCode, new List<string>() { detail }) as ObjectResult;

            var errorResponse = responseObjectResult.Value as ApiErrorResponse;

            httpContext.Response.ContentType = "application/json";
            httpContext.Response.StatusCode = statusCode;

            await httpContext.Response.WriteAsJsonAsync(errorResponse, cancellationToken);

            return true;
        }

        private void LogException(Exception exception)
        {
            if (exception is CustomException)
            {
                logger.LogWarning(exception, exception.Message);
            }
            else
            {
                logger.LogError(exception, exception.Message);
            }
        }

        private static (int statusCode, string title, string detail)
            MapExceptionToProblemInformation(Exception exception)
        {
            if (exception is not CustomException customException)
            {
                return (
                    StatusCodes.Status500InternalServerError,
                    "Internal server error",
                    "Some internal error on the server occured."
                );
            }

            return (
              customException switch
              {
                  NotFoundException => StatusCodes.Status404NotFound,
                  UnauthorizedException => StatusCodes.Status401Unauthorized,
                  BadRequestException => StatusCodes.Status400BadRequest,
                  _ => StatusCodes.Status500InternalServerError
              },
              customException.Title,
              customException.Message
            );
        }
    }
}

