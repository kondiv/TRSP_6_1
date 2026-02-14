using Application.Common.Repositories;
using Ardalis.Result;
using Domain.Entities.Car;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cars.Create;

public sealed class CreateCarCommandHandler(
    ICarRepository cars,
    ILogger<CreateCarCommandHandler> logger)
    : IRequestHandler<CreateCarCommand, Result<CarId>>
{
    public Task<Result<CarId>> Handle(CreateCarCommand request, CancellationToken cancellationToken)
    {
        var mark = Mark.Create(request.Mark);
        var model = Model.Create(request.Model);

        var car = Car.Create(mark, model);

        var result = cars.Add(car);

        if (!result.IsSuccess)
        {
            logger.LogWarning("Some errors occurred during creating the entity.\nErrors {errors}",
                result.Errors);
        }
        else
        {
            logger.LogInformation("Entity created successfully");
        }

        return Task.FromResult(result);
    }
}
