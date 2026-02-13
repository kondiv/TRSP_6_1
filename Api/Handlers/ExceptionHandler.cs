using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Api.Handlers;

public sealed class ExceptionHandler(IProblemDetailsService problemDetailsService)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext, Exception exception, CancellationToken cancellationToken)
    {
        var problemDetails = new ProblemDetails
        {
            Status = StatusCodes.Status400BadRequest,
            Title = "An error occurred",
            Type = exception.GetType().Name,
            Detail = exception.Message
        };

        httpContext.Response.StatusCode = StatusCodes.Status400BadRequest;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            Exception = exception,
            HttpContext = httpContext,
            ProblemDetails = problemDetails
        });
    }
}
