using Application.Common.Repositories;
using Application.Features.Cars.List;
using Ardalis.Result;
using Domain.Entities.Car;
using Moq;

namespace Tests.Unit.Application.Features.ListCars;

public class ListCarsTests
{
    private readonly Mock<ICarRepository> _carsRepoMock;
    private readonly ListCarsQueryHandler _handler;

    public ListCarsTests()
    {
        _carsRepoMock = new Mock<ICarRepository>();

        _handler = new ListCarsQueryHandler(_carsRepoMock.Object);
    }

    [Fact]
    public async Task Handle_WhenNoCarsInRepo_ShouldReturnResultSuccess_WithEmptyList()
    {
        // Arrange
        _carsRepoMock.Setup(r => r.List())
            .Returns(Result<IEnumerable<Car>>.Success([]));

        var query = new ListCarsQuery();
        var cancellationToken = CancellationToken.None;
        
        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Empty(result.Value.Cars);
    }

    [Fact]
    public async Task Handler_WhenCarsInRepo_ShouldReturnResultSuccess_WithAllCars()
    {
        // Arrange
        IEnumerable<Car> cars =
        [
            Car.Create(Mark.Create("Ford"), Model.Create("Focus")),
            Car.Create(Mark.Create("Ford"), Model.Create("Raptor")),
            Car.Create(Mark.Create("Ford"), Model.Create("Mondeo")),
            Car.Create(Mark.Create("BMW"), Model.Create("X5 M")),
        ];

        _carsRepoMock.Setup(r => r.List())
            .Returns(Result<IEnumerable<Car>>.Success(cars));

        var query = new ListCarsQuery();
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        var actualCars = result.Value.Cars;

        Assert.Same(cars, actualCars);
    }
}
