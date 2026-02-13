namespace Domain.Entities.Car;

public sealed record CarId
{
    public Guid Value { get; private init; }

    private CarId(Guid value) => Value = value;

    public CarId New() => new(Guid.NewGuid());

    public CarId FromValue(Guid value) => new(value);
}
