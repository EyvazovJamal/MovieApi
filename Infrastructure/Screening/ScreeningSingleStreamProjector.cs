using Marten.Events.Aggregation;

namespace Infrastructure.Screening;

public class ScreeningSingleStreamProjector : SingleStreamProjection<Domain.Screening.Screening>
{
    public ScreeningSingleStreamProjector()
    {
        IncludeType<ScreeningCreatedEvent>();
    }

    public static Domain.Screening.Screening Create(ScreeningCreatedEvent e)
    {
        return Domain.Screening.Screening.Create(
            e.ScreeningId,
            e.MovieId,
            e.HallId,
            e.StartTime,
            e.EndTime,
            e.Runtime);
    }
}
