using Application.Common.Repositories;
using Ardalis.Result;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Features.Cars.List;

public sealed class ListCarsQueryHandler(
    ICarRepository cars) 
    : IRequestHandler<ListCarsQuery, Result<ListCarsQueryResponse>>
{
    public Task<Result<ListCarsQueryResponse>> Handle(ListCarsQuery request, CancellationToken cancellationToken)
    {
        var carsList = cars.List();

        return Task.FromResult(Result.Success(new ListCarsQueryResponse(carsList.Value)));
    }
}
