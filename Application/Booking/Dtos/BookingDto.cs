using SharedKernel.Booking;

namespace Application.Booking.Dtos;

public class BookingDto
{
    public Guid Id { get; set; }
    public Guid ScreeningId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public string CustomerName { get; set; } = string.Empty;
    public List<SeatPosition> Seats { get; set; } = [];
    public decimal TotalPrice { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}
