namespace Application.Common;

public interface ICinemaTime
{
    DateTimeOffset DayStart(DateOnly date, TimeOnly? openTime = null);

    DateTimeOffset DayEnd(DateOnly date, TimeOnly? openTime = null);

    DateOnly Today();
}
