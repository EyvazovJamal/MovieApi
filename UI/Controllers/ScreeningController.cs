using Application.Queries;
using Application.Screening.Commands;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using UI.Requests;

namespace UI.Controllers;

[ApiController]
[Route("api/screening")]
public class ScreeningController(IMediator mediator) : ControllerBase
{
    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateScreeningRequest request)
    {
        try
        {
            await mediator.Send(new CreateScreeningCommand(
                request.MovieId,
                request.HallId,
                request.StartTime));

            return Ok();
        }
        catch (BusinessRuleValidationException ex)
        {
            return Conflict(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetByDate([FromQuery] DateOnly date)
    {
        var result = await mediator.Send(new GetScreeningsByDateQuery(date));
        return Ok(result);
    }

    [HttpGet("next-slot")]
    public async Task<IActionResult> GetNextSlot([FromQuery] Guid hallId, [FromQuery] DateOnly date)
    {
        try
        {
            var result = await mediator.Send(new GetNextSlotQuery(hallId, date));
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
