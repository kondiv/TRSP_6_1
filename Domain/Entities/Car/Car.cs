using Domain.Exceptions;

namespace Domain.Entities.Car;

public sealed class Car
{
    public CarId Id { get; private init; }

    public Mark Mark { get; private init; }

    public Model Model { get; private init; }

    private Car(Mark mark, Model model)
    {
        Id = CarId.New();
        Mark = mark;
        Model = model;
    }

    public static Car Create(Mark mark, Model model)
    {
        if (mark is null)
        {
            throw new DomainException("Mark is missing");
        }

        if (model is null)
        {
            throw new DomainException("Model is missing");
        }

        return new Car(mark, model);
    }
}
