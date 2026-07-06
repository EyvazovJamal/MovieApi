using Domain.Common;
using SharedKernel.Booking;

namespace Domain.Booking.Rules;

public sealed class SeatsMustBeAvailableRule(
    IEnumerable<Booking> existingBookings,
    IReadOnlyList<SeatPosition> requestedSeats) : IBusinessRule
{
    public string Message => "One or more seats are already taken";

    public bool IsBroken()
    {
        var occupied = existingBookings
            .SelectMany(b => b.Seats)
            .Select(s => HallLayout.SeatKey(s.Row, s.Seat))
            .ToHashSet();

        return requestedSeats
            .Select(s => HallLayout.SeatKey(s.Row, s.Seat))
            .Any(occupied.Contains);
    }
}
