using SharedKernel.Contracts;

namespace SharedKernel.Booking;

public sealed record BookingCreatedEvent(
    Guid BookingId,
    Guid ScreeningId,
    string CustomerName,
    IReadOnlyList<SeatPosition> Seats,
    decimal TotalPrice,
    DateTimeOffset CreatedAt) : IDomainEvent;
