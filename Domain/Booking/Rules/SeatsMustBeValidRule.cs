using Domain.Common;
using SharedKernel.Booking;

namespace Domain.Booking.Rules;

public sealed class SeatsMustBeValidRule(IReadOnlyList<SeatPosition> seats) : IBusinessRule
{
    public string Message => "One or more seats are invalid for this hall";

    public bool IsBroken() =>
        seats.Count == 0 || seats.Any(s => !HallLayout.IsValidSeat(s.Row, s.Seat));
}
