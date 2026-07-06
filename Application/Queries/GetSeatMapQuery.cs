using Application.Booking.Dtos;
using MediatR;

namespace Application.Queries;

public sealed record GetSeatMapQuery(Guid ScreeningId) : IRequest<SeatMapDto?>;
