using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Api.Filters;

public class AddCustomFieldsToProblemDetailsFilter : IResultFilter
{
    public void OnResultExecuted(ResultExecutedContext context) { }

    public void OnResultExecuting(ResultExecutingContext context)
    {
        if (context.Result is ObjectResult objectResult && 
            objectResult.Value is ProblemDetails problemDetails)
        {
            var requestId = context.HttpContext.TraceIdentifier;

            problemDetails.Extensions.TryAdd("requestId", requestId);

            var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;

            problemDetails.Extensions.TryAdd("traceId", activity?.Id);

            problemDetails.Instance =
                $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";
        }
    }
}
