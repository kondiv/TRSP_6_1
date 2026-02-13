using Api.Filters;
using Api.Handlers;
using Application.Behaviors;
using Application.Common.Repositories;
using Application.Features.Cars.Create;
using FluentValidation;
using Infrastructure.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.Features;

namespace Api.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddSingleton<ICarRepository, InMemoryCarRepository>();

        return services;
    }

    public static IServiceCollection AddMediatR(this IServiceCollection services)
    {
        services.AddMediatR(cfg =>
        {
            cfg.RegisterServicesFromAssembly(typeof(CreateCarCommand).Assembly);

            cfg.AddOpenBehavior(typeof(LoggingBehavior<,>));
            cfg.AddOpenBehavior(typeof(PerformanceBehavior<,>));
        });

        return services;
    }

    public static IServiceCollection AddValidators(this IServiceCollection services)
    {
        services.AddValidatorsFromAssembly(typeof(CreateCarCommandValidator).Assembly);

        return services;
    }

    public static IServiceCollection AddCustomProblemDetails(this IServiceCollection services)
    {
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                context.ProblemDetails.Instance = 
                    $"{context.HttpContext.Request.Method} {context.HttpContext.Request.Path}";

                context.ProblemDetails.Extensions.TryAdd("requestId", context.HttpContext.TraceIdentifier);

                var activity = context.HttpContext.Features.Get<IHttpActivityFeature>()?.Activity;
                context.ProblemDetails.Extensions.TryAdd("traceId", activity?.Id);
            };
        });

        return services;
    }

    public static IServiceCollection AddControllersWithFilters(this IServiceCollection services)
    {
        services.AddControllers(options =>
        {
            options.Filters.Add<AddCustomFieldsToProblemDetailsFilter>();
        });

        return services;
    }

    public static IServiceCollection AddExceptionHandlers(this IServiceCollection services)
    {
        services.AddExceptionHandler<ExceptionHandler>();

        return services;
    }
}
