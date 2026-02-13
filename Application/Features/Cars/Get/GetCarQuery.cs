using Ardalis.Result;
using Domain.Entities.Car;
using MediatR;

namespace Application.Features.Cars.Get;

public sealed record GetCarQuery(Guid Id) : IRequest<Result<Car>>;
