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
        try
        {
            await mediator.Send(new AddMovieFromTmdbToCinemaCommand(id));
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(ex.Message);
        }
    }
    
    
}