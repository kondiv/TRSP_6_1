using Domain.Entities.Car;

namespace Application.Features.Cars.List;

public sealed record ListCarsQueryResponse
{
    public IEnumerable<Car> Cars { get; private init; } = [];

    public ListCarsQueryResponse(IEnumerable<Car> cars) => Cars = cars;
}
