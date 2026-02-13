using MediatR;
using Microsoft.Extensions.Logging;
using System.Diagnostics;

namespace Application.Behaviors;

public sealed class PerformanceBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : IRequest
{
    private readonly ILogger<PerformanceBehavior<TRequest, TResponse>> _logger;

    public PerformanceBehavior(ILogger<PerformanceBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        var sw = Stopwatch.StartNew();

        var response = await next();

        sw.Stop();

        _logger.LogInformation(
            "Request {RequestName} executed in {ElapsedMilliseconds} ms",
            typeof(TRequest).Name,
            sw.ElapsedMilliseconds);

        return response;
    }
}
