using Marten.Events.Aggregation;
using SharedKernel.Screening;

namespace Infrastructure.Screening;

public class ScreeningSingleStreamProjector : SingleStreamProjection<Domain.Screening.Screening>
{
    public ScreeningSingleStreamProjector()
    {
        IncludeType<ScreeningCreatedEvent>();
        IncludeType<ScreeningDeletedEvent>();
        DeleteEvent<ScreeningDeletedEvent>();
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
