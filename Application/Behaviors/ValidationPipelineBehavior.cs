using Ardalis.Result;
using FluentValidation;
using MediatR;
using System.Reflection;

namespace Application.Behaviors;

public sealed class ValidationPipelineBehavior<TRequest, TResponse>(
    IEnumerable<IValidator<TRequest>> validators)
    : IPipelineBehavior<TRequest, TResponse>
    where TRequest : notnull
{
    public async Task<TResponse> Handle(TRequest request, RequestHandlerDelegate<TResponse> next, CancellationToken cancellationToken)
    {
        if (!validators.Any())
        {
            return await next();
        }

        var validationResults = await Task.WhenAll(validators.Select(v => v.ValidateAsync(request, cancellationToken)));

        var errors = validationResults
            .SelectMany(f => f.Errors)
            .Where(f => f is not null)
            .Select(e => new ValidationError
            {
                ErrorMessage = e.ErrorMessage,
                Identifier = e.PropertyName,
                ErrorCode = e.ErrorCode
            })
            .Distinct()
            .ToList();

        if (errors.Count > 0)
        {
            return ToInvalidResult<TResponse>(errors);
        }

        return await next();
    }

    private static TResult ToInvalidResult<TResult>(List<ValidationError> errors)
    {
        var type = typeof(TResult);

        if (type == typeof(Result))
        {
            return (TResult)(object)Result.Invalid(errors);
        }

        var invalidMethods = type.GetMethods(BindingFlags.Public | BindingFlags.Static)
        .Where(m => m.Name == "Invalid")
        .ToList();

        var targetMethod = invalidMethods.FirstOrDefault(m =>
        {
            var parameters = m.GetParameters();
            return parameters.Length == 1 &&
                    parameters[0].ParameterType == typeof(IEnumerable<ValidationError>);
        });

        if (targetMethod is not null)
        {
            return (TResult)targetMethod.Invoke(null, [errors])!;
        }
        else
        {
            throw new InvalidOperationException("TResult is not Result type");
        }
    }
}
