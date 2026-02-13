using Api.Extensions;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Enrichers.Span;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, services, configuration) =>
{
    configuration
        .Enrich.FromLogContext()
        .Enrich.WithSpan()
        .WriteTo.Console(outputTemplate:
            "[{Timestamp:HH:mm:ss} {Level:u3}] " +
            "[TraceId: {TraceId}] " +
            "[SpanId: {SpanId}] " +
            "{Message:lj}{NewLine}{Exception}");
});

builder.Services.AddExceptionHandlers();

builder.Services.AddInfrastructure();

builder.Services.AddValidators();

builder.Services.AddMediatR();

builder.Services.AddCustomProblemDetails();

builder.Services.AddControllersWithFilters();

builder.Services.AddOpenApi();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseExceptionHandler();

app.UseHttpsRedirection();

app.UseAuthorization();

app.UseStatusCodePages();

app.MapControllers();

app.Run();
