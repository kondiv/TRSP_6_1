using Domain.Exceptions;

namespace Domain.Entities.Car;

public sealed record Model
{
    public string Name { get; private init; }

    private Model(string name) => Name = name;

    public static Model Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Model name is missing");
        }

        return new(name);
    }
}
