using Ardalis.Result;
using Domain.Entities.Car;
using MediatR;

namespace Application.Features.Cars.Create;

public sealed record CreateCarRequest(string Mark, string Model)
    : IRequest<Result<CarId>>;
