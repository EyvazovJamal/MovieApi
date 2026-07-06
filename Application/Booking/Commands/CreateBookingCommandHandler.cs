using Application.Booking.Dtos;
using Domain.Booking.Rules;
using Domain.Common;
using Marten;
using MediatR;
using SharedKernel.Booking;

namespace Application.Booking.Commands;

public class CreateBookingCommandHandler(
    IDocumentStore store,
    IBookingRepository bookingRepository) : IRequestHandler<CreateBookingCommand, BookingDto>
{
    public async Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var screening = await session.LoadAsync<Domain.Screening.Screening>(
            request.ScreeningId, cancellationToken);

        if (screening is null)
            throw new InvalidOperationException("Screening not found");

        var movie = await session.LoadAsync<Domain.Movie.Movie>(screening.MovieId, cancellationToken);
        var hall = await session.LoadAsync<Domain.Hall.Hall>(screening.HallId, cancellationToken);

        var seats = request.Seats.ToList();

        var validRule = new SeatsMustBeValidRule(seats);
        if (validRule.IsBroken())
            throw new BusinessRuleValidationException(validRule);

        var existingBookings = await session.Query<Domain.Booking.Booking>()
            .Where(b => b.ScreeningId == request.ScreeningId)
            .ToListAsync(cancellationToken);

        var availableRule = new SeatsMustBeAvailableRule(existingBookings, seats);
        if (availableRule.IsBroken())
            throw new BusinessRuleValidationException(availableRule);

        var totalPrice = seats.Count * HallLayout.DefaultTicketPrice;
        var bookingId = Guid.NewGuid();

        var booking = Domain.Booking.Booking.Create(
            bookingId,
            request.ScreeningId,
            request.CustomerName.Trim(),
            seats,
            totalPrice,
            DateTimeOffset.UtcNow);

        await bookingRepository.StoreAsync(booking, cancellationToken);

        return new BookingDto
        {
            Id = bookingId,
            ScreeningId = request.ScreeningId,
            MovieTitle = movie?.Title ?? string.Empty,
            HallName = hall?.Name ?? string.Empty,
            StartTime = screening.StartTime,
            CustomerName = request.CustomerName.Trim(),
            Seats = seats,
            TotalPrice = totalPrice,
            CreatedAt = booking.CreatedAt
        };
    }
}
