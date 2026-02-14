using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Behaviors;

public sealed class LoggingPipelineBehavior<TRequest, TResponse>
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    private readonly ILogger<LoggingPipelineBehavior<TRequest, TResponse>> _logger;

    public LoggingPipelineBehavior(ILogger<LoggingPipelineBehavior<TRequest, TResponse>> logger) => _logger = logger;

    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        _logger.LogInformation(
            "Handling {RequestName} {@Request}",
            typeof(TRequest).Name,
            request);

        TResponse response;

        try
        {
            response = await next();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Unhandled exception in {RequestName} {@Request}",
                typeof(TRequest).Name,
                request);
            throw;
        }

        if (response is IResult result)
        {
            if (result.Status != ResultStatus.Ok ||
                result.Status != ResultStatus.NoContent ||
                result.Status != ResultStatus.Created)
            {
                _logger.LogWarning("{RequestName} {@Request} completed with errors: {Errors}",
                    typeof(TRequest).Name,
                    request,
                    result.Errors.Any() ? 
                    result.Errors.Select(e => e) : 
                    result.ValidationErrors.Select(e => e.ErrorMessage));
            }
        }

        _logger.LogInformation(
            "Handled {RequestName} {@Response}",
            typeof(TRequest).Name,
            response);

        return response;
    }
}
