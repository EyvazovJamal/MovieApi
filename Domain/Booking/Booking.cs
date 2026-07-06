using Domain.Common;
using Marten.Events.CodeGeneration;
using Newtonsoft.Json;
using SharedKernel.Booking;
using SharedKernel.Contracts;

namespace Domain.Booking;

public class Booking : AggregateRoot
{
    public Guid ScreeningId { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public List<SeatPosition> Seats { get; private set; } = [];
    public decimal TotalPrice { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    [JsonConstructor]
    internal Booking(
        Guid id,
        Guid screeningId,
        string customerName,
        List<SeatPosition> seats,
        decimal totalPrice,
        DateTimeOffset createdAt)
    {
        Id = id;
        ScreeningId = screeningId;
        CustomerName = customerName;
        Seats = seats;
        TotalPrice = totalPrice;
        CreatedAt = createdAt;
    }

    private Booking(
        Guid id,
        Guid screeningId,
        string customerName,
        List<SeatPosition> seats,
        decimal totalPrice,
        DateTimeOffset createdAt,
        bool _)
    {
        Id = id;
        ScreeningId = screeningId;
        CustomerName = customerName;
        Seats = seats;
        TotalPrice = totalPrice;
        CreatedAt = createdAt;

        AddDomainEvent(new BookingCreatedEvent(
            id,
            screeningId,
            customerName,
            seats,
            totalPrice,
            createdAt));
    }

    [MartenIgnore]
    public static Booking Create(
        Guid id,
        Guid screeningId,
        string customerName,
        List<SeatPosition> seats,
        decimal totalPrice,
        DateTimeOffset createdAt)
    {
        return new Booking(
            id,
            screeningId,
            customerName,
            seats,
            totalPrice,
            createdAt,
            true);
    }

    public void Apply(BookingCreatedEvent @event) => InternalApply(@event);

    public override void ApplyEvent(IDomainEvent domainEvent)
    {
        if (domainEvent is BookingCreatedEvent e)
            InternalApply(e);
    }

    private void InternalApply(BookingCreatedEvent e)
    {
        Id = e.BookingId;
        ScreeningId = e.ScreeningId;
        CustomerName = e.CustomerName;
        Seats = e.Seats.ToList();
        TotalPrice = e.TotalPrice;
        CreatedAt = e.CreatedAt;
    }
}
