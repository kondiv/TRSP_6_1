using Application.Common.Repositories;
using Ardalis.Result;
using Domain.Entities.Car;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cars.Get;

public sealed class GetCarQueryHandler(
    ICarRepository cars,
    ILogger<GetCarQueryHandler> logger) 
    : IRequestHandler<GetCarQuery, Result<Car>>
{
    public Task<Result<Car>> Handle(GetCarQuery request, CancellationToken cancellationToken)
    {
        var id = CarId.FromValue(request.Id);

        var getResult = cars.Get(id);

        if (getResult.IsNotFound())
        {
            logger.LogWarning("Car with id {id} not found", id.Value);
        }

        return Task.FromResult(getResult);
    }
}
