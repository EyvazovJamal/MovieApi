using Application.Booking.Dtos;
using MediatR;

namespace Application.Queries;

public sealed record GetBookingByIdQuery(Guid BookingId) : IRequest<BookingDto?>;
