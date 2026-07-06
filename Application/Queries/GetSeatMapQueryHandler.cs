using Application.Booking.Dtos;
using Domain.Common;
using Marten;
using MediatR;
using SharedKernel.Booking;

namespace Application.Queries;

public class GetSeatMapQueryHandler(IDocumentStore store)
    : IRequestHandler<GetSeatMapQuery, SeatMapDto?>
{
    public async Task<SeatMapDto?> Handle(GetSeatMapQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var screening = await session.LoadAsync<Domain.Screening.Screening>(
            request.ScreeningId, cancellationToken);

        if (screening is null)
            return null;

        var movie = await session.LoadAsync<Domain.Movie.Movie>(screening.MovieId, cancellationToken);
        var hall = await session.LoadAsync<Domain.Hall.Hall>(screening.HallId, cancellationToken);

        var bookings = await session.Query<Domain.Booking.Booking>()
            .Where(b => b.ScreeningId == request.ScreeningId)
            .ToListAsync(cancellationToken);

        var occupied = bookings
            .SelectMany(b => b.Seats)
            .ToList();

        return new SeatMapDto
        {
            ScreeningId = screening.Id,
            MovieId = screening.MovieId,
            MovieTitle = movie?.Title ?? string.Empty,
            HallName = hall?.Name ?? string.Empty,
            StartTime = screening.StartTime,
            TicketPrice = HallLayout.DefaultTicketPrice,
            Rows = HallLayout.DefaultRows.ToList(),
            OccupiedSeats = occupied
        };
    }
}
