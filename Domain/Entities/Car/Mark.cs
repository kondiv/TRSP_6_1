using Domain.Exceptions;

namespace Domain.Entities.Car;

public sealed record Mark
{
    public string Name { get; private init; }

    private Mark(string name) => Name = name;

    public static Mark Create(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new DomainException("Mark name is missing");
        }

        return new Mark(name);
    }
}
