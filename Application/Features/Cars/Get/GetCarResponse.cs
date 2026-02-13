using Domain.Entities.Car;

namespace Application.Features.Cars.Get;

public sealed record GetCarResponse
{
    public Car Car { get; private init; }

    public GetCarResponse(Car car) => Car = car;
}
