using Domain.Common;
using SharedKernel.Booking;

namespace Application.Booking.Dtos;

public class SeatMapDto
{
    public Guid ScreeningId { get; set; }
    public Guid MovieId { get; set; }
    public string MovieTitle { get; set; } = string.Empty;
    public string HallName { get; set; } = string.Empty;
    public DateTimeOffset StartTime { get; set; }
    public decimal TicketPrice { get; set; } = HallLayout.DefaultTicketPrice;
    public List<HallRowLayout> Rows { get; set; } = [];
    public List<SeatPosition> OccupiedSeats { get; set; } = [];
}
