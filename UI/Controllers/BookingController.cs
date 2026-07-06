using Application.Booking.Commands;
using Application.Queries;
using Domain.Common;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel.Booking;
using UI.Requests;

namespace UI.Controllers;

[ApiController]
[Route("api/booking")]
public class BookingController(IMediator mediator) : ControllerBase
{
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        var result = await mediator.Send(new GetBookingByIdQuery(id));
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost("create")]
    public async Task<IActionResult> Create(CreateBookingRequest request)
    {
        try
        {
            var seats = request.Seats
                .Select(s => new SeatPosition(s.Row, s.Seat))
                .ToList();

            var result = await mediator.Send(new CreateBookingCommand(
                request.ScreeningId,
                request.CustomerName,
                seats));

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
