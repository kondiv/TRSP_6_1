using Application.Common.Repositories;
using Application.Features.Cars.Create;
using Ardalis.Result;
using Domain.Entities.Car;
using Domain.Exceptions;
using Microsoft.Extensions.Logging;
using Moq;

namespace Tests.Unit.Application.Features.CreateCar;

public class CreateCarTests
{
    private readonly CreateCarCommandHandler _handler;
    private readonly Mock<ICarRepository> _carRepoMock;

    public CreateCarTests()
    {
        _carRepoMock = new Mock<ICarRepository>();
        _handler = new CreateCarCommandHandler(
            _carRepoMock.Object,
            new LoggerFactory().CreateLogger<CreateCarCommandHandler>());
    }

    public static TheoryData<CreateCarCommand> InvalidCommands()
    {
        return
        [
            new CreateCarCommand("", null!),
            new CreateCarCommand(null!, ""),
            new CreateCarCommand(null!, null!),
            new CreateCarCommand("", "")
        ];
    }

    [Fact]
    public async Task Handle_WhenEverythingIsOk_ShouldReturn_ResultCreated_WithCarId()
    {
        // Arrange
        var carId = CarId.New();
        string mark = "Ford", model = "Focus";
        var validCommand = new CreateCarCommand(mark, model);
        var cancellationToken = CancellationToken.None;

        _carRepoMock.Setup(r => r.Add(It.IsAny<Car>()))
            .Returns(Result<CarId>.Created(carId));

        // Act
        var result = await _handler.Handle(validCommand, cancellationToken);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.Equal(carId, result.Value);
    }

    [Fact]
    public async Task Handle_WhenIdCollision_ShouldReturn_ResultFailure()
    {
        // Arrange
        var carId = CarId.New();
        string mark = "Ford", model = "Focus";
        var command = new CreateCarCommand(mark, model);
        var cancellationToken = CancellationToken.None;

        _carRepoMock.Setup(r => r.Add(It.IsAny<Car>()))
            .Returns(Result<CarId>.Conflict());

        // Act
        var result = await _handler.Handle(command, cancellationToken);

        // Assert
        Assert.False(result.IsSuccess);
        Assert.True(result.IsConflict());
    }

    [Theory]
    [MemberData(nameof(InvalidCommands))]
    public async Task Handle_WhenDomainRulesViolate_ShouldThrow_DomainException(CreateCarCommand command)
    {
        // Arrange
        var cancellationToken = CancellationToken.None;

        // Act
        var act = () => _handler.Handle(command, cancellationToken);

        // Assert
        await Assert.ThrowsAsync<DomainException>(act);
    }
}
