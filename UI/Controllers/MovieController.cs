using Application.Hall;
using Application.Movie.Dtos;
using Application.Queries;
using Domain.Hall;
using Infrastructure.Hall;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;
[ApiController]
[Route("api/movies")]
public class MovieController(
    IMediator mediator
    ):ControllerBase
{
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
        var command = new GetMyMoviesQuery(filter);
        var result = await mediator.Send(command);
        return Ok(result);
    }
}