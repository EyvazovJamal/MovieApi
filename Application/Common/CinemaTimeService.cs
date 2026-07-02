using Microsoft.Extensions.Options;

namespace Application.Common;

public class CinemaTimeService(IOptions<CinemaSettings> options) : ICinemaTime
{
    private readonly TimeZoneInfo _timeZone =
        TimeZoneInfo.FindSystemTimeZoneById(options.Value.TimeZoneId);

    public DateTimeOffset DayStart(DateOnly date, TimeOnly? openTime = null)
    {
        var time = openTime ?? TimeOnly.MinValue;
        var local = date.ToDateTime(time);
        return new DateTimeOffset(local, _timeZone.GetUtcOffset(local));
    }

    public DateTimeOffset DayEnd(DateOnly date, TimeOnly? openTime = null) =>
        DayStart(date, openTime).AddDays(1);

    public DateOnly Today()
    {
        var now = TimeZoneInfo.ConvertTime(DateTimeOffset.UtcNow, _timeZone);
        return DateOnly.FromDateTime(now.DateTime);
    }
}
