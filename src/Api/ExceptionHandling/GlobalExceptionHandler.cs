using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

public sealed class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) :
    IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        logger.LogError(
            exception,
            "Unhandled exception occurred: {Message}",
            exception.Message);

        (int statusCode, string title) = exception switch
        {
            NotFoundException =>
                (StatusCodes.Status404NotFound, "Resource not found"),

            KeyNotFoundException =>
                (StatusCodes.Status404NotFound, "Resource not found"),

            ConflictException =>
                (StatusCodes.Status409Conflict, "Resource conflict"),

            ValidationException =>
                (StatusCodes.Status400BadRequest, "Validation failed"),

            ArgumentException =>
                (StatusCodes.Status400BadRequest, "Invalid request"),

            NotSupportedException =>
                (StatusCodes.Status400BadRequest, "Unsupported operation"),

            OperationCanceledException =>
                (StatusCodes.Status499ClientClosedRequest, "Request cancelled"),

            _ =>
                (StatusCodes.Status500InternalServerError,
                 "Internal server error")
        };

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = statusCode == StatusCodes.Status500InternalServerError
                ? "An unexpected error occurred."
                : exception.Message,
            Instance = httpContext.Request.Path
        };

        problemDetails.Extensions["traceId"] =
            httpContext.TraceIdentifier;

        httpContext.Response.StatusCode = statusCode;

        await httpContext.Response.WriteAsJsonAsync(
            problemDetails,
            cancellationToken);

        return true;
    }
}
