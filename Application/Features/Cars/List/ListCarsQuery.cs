using Ardalis.Result;
using MediatR;

namespace Application.Features.Cars.List;

public sealed record ListCarsQuery : IRequest<Result<ListCarsQueryResponse>>;