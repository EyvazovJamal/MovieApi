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
    [HttpPost("delete")]
    public async Task<IActionResult> Delete([FromBody]Guid screeningId)
    {
        try
        {
            await mediator.Send(new DeleteScreeningCommand(screeningId));
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("repeat-preview")]
    public async Task<IActionResult> GetRepeatPreview([FromQuery] DateOnly targetDate)
    {
        var result = await mediator.Send(new GetRepeatScreeningsPreviewQuery(targetDate));
        return Ok(result);
    }

    [HttpPost("repeat-from-date")]
    public async Task<IActionResult> RepeatFromDate(RepeatScreeningsRequest request)
    {
        try
        {
            var result = await mediator.Send(new RepeatScreeningsFromDateCommand(request.TargetDate));
            return Ok(result);
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
}
