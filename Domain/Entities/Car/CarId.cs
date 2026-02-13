namespace Domain.Entities.Car;

public sealed record CarId
{
    public Guid Value { get; private init; }

    private CarId(Guid value) => Value = value;

    public static CarId New() => new(Guid.NewGuid());

    public static CarId FromValue(Guid value) => new(value);
}
