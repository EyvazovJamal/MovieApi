using Marten.Events.Aggregation;

namespace Infrastructure.Hall;

public class HallSingleStreamProjector :SingleStreamProjection<Domain.Hall.Hall>
{
    public HallSingleStreamProjector()
    {
        IncludeType<HallCreatedEvent>();
    }

    public static Domain.Hall.Hall Create(HallCreatedEvent e)
    {
        return Domain.Hall.Hall.Create(e.HallId,e.Name,e.SeatCount);
    }
}