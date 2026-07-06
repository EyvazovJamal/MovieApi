using Marten.Events.Aggregation;

namespace Infrastructure.Booking;

public class BookingSingleStreamProjector : SingleStreamProjection<Domain.Booking.Booking>
{
    public BookingSingleStreamProjector()
    {
        IncludeType<SharedKernel.Booking.BookingCreatedEvent>();
    }

    public static Domain.Booking.Booking Create(SharedKernel.Booking.BookingCreatedEvent e)
    {
        return Domain.Booking.Booking.Create(
            e.BookingId,
            e.ScreeningId,
            e.CustomerName,
            e.Seats.ToList(),
            e.TotalPrice,
            e.CreatedAt);
    }
}
