using Domain.Common;

namespace Domain.Screening.Rules;

public sealed class ScreeningMustNotOverlapRule(
    DateTimeOffset newStart,
    DateTimeOffset newEnd,
    IEnumerable<Screening> existingInHall) : IBusinessRule
{
    public string Message => "Screening overlaps with an existing session in this hall";

    public bool IsBroken()
    {
        return existingInHall.Any(existing =>
            Screening.Overlaps(newStart, newEnd, existing.StartTime, existing.EndTime));
    }
}
