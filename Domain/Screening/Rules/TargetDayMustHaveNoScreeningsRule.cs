using Domain.Common;

namespace Domain.Screening.Rules;

public sealed class TargetDayMustHaveNoScreeningsRule(int existingCount) : IBusinessRule
{
    public string Message => "Target day already has screenings";

    public bool IsBroken() => existingCount > 0;
}
