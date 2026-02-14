using Application.Common.Repositories;
using Application.Features.Cars.Get;
using Ardalis.Result;
using Domain.Entities.Car;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Unit.Application.Features.GetCar;

public class GetCarTests
{
    private readonly Mock<ICarRepository> _carRepoMock;
    private readonly GetCarQueryHandler _handler;

    public GetCarTests()
    {
        _carRepoMock = new Mock<ICarRepository>();
        _handler = new GetCarQueryHandler(
            _carRepoMock.Object,
            new LoggerFactory().CreateLogger<GetCarQueryHandler>());
    }

    [Fact]
    public async Task Handle_WhenCarFound_ShouldReturnResultSuccess_WithCarValue()
    {
        // Arrange
        var mark = Mark.Create("Ford");
        var model = Model.Create("Focus");
        var car = Car.Create(mark, model);

        var carId = car.Id;

        _carRepoMock.Setup(r => r.Get(carId))
            .Returns(Result<Car>.Success(car));

        var query = new GetCarQuery(carId.Value);
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);

        var actualCar = result.Value;
        Assert.Equal(mark, actualCar.Mark);
        Assert.Equal(model, actualCar.Model);
    }

    [Fact]
    public async Task Handle_WhenCarNotFound_ShouldReturnResultNotFound()
    {
        // Arrange
        _carRepoMock.Setup(r => r.Get(It.IsAny<CarId>()))
            .Returns(Result<Car>.NotFound());

        var query = new GetCarQuery(Guid.NewGuid());
        var cancellationToken = CancellationToken.None;

        // Act
        var result = await _handler.Handle(query, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsNotFound());
    }
}
