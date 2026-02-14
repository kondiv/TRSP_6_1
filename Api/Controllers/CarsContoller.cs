using Application.Features.Cars.Create;
using Application.Features.Cars.Get;
using Application.Features.Cars.List;
using Ardalis.Result.AspNetCore;
using Domain.Entities.Car;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/items/")]
public sealed class CarsContoller(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<CarId>> CreateAsync(CreateCarCommand request,
        CancellationToken cancellationToken = default)
    {
        var result = await mediator.Send(request, cancellationToken);

        return result.ToActionResult(this);
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<Car>> GetAsync(Guid id,
        CancellationToken cancellationToken = default)
    {
        var query = new GetCarQuery(id);

        var getResult = await mediator.Send(query, cancellationToken);

        return getResult.ToActionResult(this);
    }

    [HttpGet]
    public async Task<ActionResult<ListCarsQueryResponse>> ListAsync(CancellationToken cancellationToken = default)
    {
        var listResult = await mediator.Send(new ListCarsQuery(), cancellationToken);

        return listResult.ToActionResult(this);
    }

}
