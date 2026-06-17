using Application.Hall.Commands;
using Application.Movie.Dtos;
using Application.Queries;
using Infrastructure.Hall;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.Requests;

namespace UI.Controllers;
[ApiController]
[Route("api/hall")]
public class HallController(
    IMediator mediator) :ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> CreateHall(CreateHallRequest request)
    {
        var command= new CreateHallCommand(request.Name, request.SeatCount);
        await mediator.Send(command);
        return Ok();
    }
    [HttpPost("filter")]
    public async Task<IActionResult> Filter(GetMoviesFilter? filter)
    {
        if (filter==null)
        {
            filter = new GetMoviesFilter
            {
                Skip = 0,
                Take = 10,
            };
        }
        var command = new GetHallsQuery(filter);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}