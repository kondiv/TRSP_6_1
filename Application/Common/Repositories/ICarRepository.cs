using Ardalis.Result;
using Domain.Entities.Car;

namespace Application.Common.Repositories;

public interface ICarRepository
{
    Result<CarId> Add(Car car);
    
    Result<Car> Get(CarId id);
    
    Result<IEnumerable<Car>> List();
}
