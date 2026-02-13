using Application.Common.Repositories;
using Ardalis.Result;
using Domain.Entities.Car;
using System.Collections.Concurrent;

namespace Infrastructure.Repositories;

public sealed class InMemoryCarRepository : ICarRepository
{
    private readonly ConcurrentDictionary<CarId, Car> _cars = [];

    public Result<CarId> Add(Car car)
    {
        if (!_cars.TryAdd(car.Id, car))
        {
            return Result.Conflict("Unique violation");
        }

        return Result.Created(car.Id);
    }

    public Result<Car> Get(CarId id)
    {
        if(!_cars.TryGetValue(id, out var car))
        {
            return Result.NotFound();
        }

        return car;
    }

    public Result<IEnumerable<Car>> List()
    {
        return _cars.Values.ToArray();
    }
}
