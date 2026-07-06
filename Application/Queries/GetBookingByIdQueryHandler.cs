using Application.Booking.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetBookingByIdQueryHandler(IDocumentStore store)
    : IRequestHandler<GetBookingByIdQuery, BookingDto?>
{
    public async Task<BookingDto?> Handle(GetBookingByIdQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var booking = await session.LoadAsync<Domain.Booking.Booking>(
            request.BookingId, cancellationToken);

        if (booking is null)
            return null;

        var screening = await session.LoadAsync<Domain.Screening.Screening>(
            booking.ScreeningId, cancellationToken);

        if (screening is null)
            return null;

        var movie = await session.LoadAsync<Domain.Movie.Movie>(screening.MovieId, cancellationToken);
        var hall = await session.LoadAsync<Domain.Hall.Hall>(screening.HallId, cancellationToken);

        return new BookingDto
        {
            Id = booking.Id,
            ScreeningId = booking.ScreeningId,
            MovieTitle = movie?.Title ?? string.Empty,
            HallName = hall?.Name ?? string.Empty,
            StartTime = screening.StartTime,
            CustomerName = booking.CustomerName,
            Seats = booking.Seats,
            TotalPrice = booking.TotalPrice,
            CreatedAt = booking.CreatedAt
        };
    }
}
