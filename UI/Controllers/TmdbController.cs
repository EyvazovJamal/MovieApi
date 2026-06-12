using Application.Movie.Commands;
using Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace UI.Controllers;
[ApiController]
[Route("api/tmdb")]
public class TmdbController(IMediator mediator) : ControllerBase
{
    [HttpGet("popular")]
    public async Task<IActionResult> GetPopularMoviesAsync()
    {
        var command = new GetPopularMoviesQuery();
        var result = await mediator.Send(command);
        return Ok(result);
    }

    [HttpPost("addToCinema")]
    public async Task<IActionResult> AddMovieFromTmdbToCinema([FromBody] int id)
    {
        var command = new AddMovieFromTmdbToCinemaCommand(id);
        await mediator.Send(command);
        return Ok();
    }
    
    
}