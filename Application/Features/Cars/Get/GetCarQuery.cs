using Ardalis.Result;
using MediatR;

namespace Application.Features.Cars.Get;

public sealed record GetCarQuery(Guid Id) : IRequest<Result<GetCarResponse>>;
