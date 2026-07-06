using Application.Booking.Dtos;
using SharedKernel.Booking;
using MediatR;

namespace Application.Booking.Commands;

public sealed record CreateBookingCommand(
    Guid ScreeningId,
    string CustomerName,
    IReadOnlyList<SeatPosition> Seats) : IRequest<BookingDto>;
