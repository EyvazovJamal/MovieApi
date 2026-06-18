using Application.Screening.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetNextSlotQueryHandler(IDocumentStore store)
    : IRequestHandler<GetNextSlotQuery, NextSlotDto>
{
    private static readonly TimeOnly DayOpenTime = new(9, 0);

    public async Task<NextSlotDto> Handle(GetNextSlotQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var hall = await session.LoadAsync<Domain.Hall.Hall>(request.HallId, cancellationToken);
        if (hall is null)
            throw new InvalidOperationException("Hall not found");

        var dayStart = request.Date.ToDateTime(DayOpenTime, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        var screenings = await session.Query<Domain.Screening.Screening>()
            .Where(s => s.HallId == request.HallId)
            .Where(s => s.StartTime >= dayStart && s.StartTime < dayEnd)
            .OrderBy(s => s.StartTime)
            .ToListAsync(cancellationToken);

        var suggestedStart = screenings.Count == 0
            ? dayStart
            : screenings.Last().EndTime.AddMinutes(Domain.Screening.Screening.BufferMinutes);

        return new NextSlotDto
        {
            HallId = request.HallId,
            SuggestedStartTime = suggestedStart
        };
    }
}
