using Application.Common;
using Application.Screening.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetNextSlotQueryHandler(IDocumentStore store, ICinemaTime cinemaTime)
    : IRequestHandler<GetNextSlotQuery, NextSlotDto>
{
    private static readonly TimeOnly DayOpenTime = new(9, 0);

    public async Task<NextSlotDto> Handle(GetNextSlotQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var hall = await session.LoadAsync<Domain.Hall.Hall>(request.HallId, cancellationToken);
        if (hall is null)
            throw new InvalidOperationException("Hall not found");

        var dayStart = cinemaTime.DayStart(request.Date, DayOpenTime);
        var dayEnd = cinemaTime.DayEnd(request.Date, DayOpenTime);

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
