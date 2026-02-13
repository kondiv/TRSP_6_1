using Application.Common.Repositories;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cars.List;

public sealed class ListCarsQueryHandler(
    ICarRepository cars,
    ILogger<ListCarsQueryHandler> logger) 
    : IRequestHandler<ListCarsQuery, Result<ListCarsQueryResponse>>
{
    public Task<Result<ListCarsQueryResponse>> Handle(ListCarsQuery request, CancellationToken cancellationToken)
    {
        using var loggerScope = logger.BeginScope(new Dictionary<string, string>
        {
            ["Operation"] = "ListCars"
        });

        var carsList = cars.List();

        return Task.FromResult(Result.Success(new ListCarsQueryResponse(carsList.Value)));
    }
}
