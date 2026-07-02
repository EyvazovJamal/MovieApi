using Application.Common;
using Application.Screening.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetRepeatScreeningsPreviewQueryHandler(IDocumentStore store, ICinemaTime cinemaTime)
    : IRequestHandler<GetRepeatScreeningsPreviewQuery, RepeatScreeningsPreviewDto>
{
    public async Task<RepeatScreeningsPreviewDto> Handle(
        GetRepeatScreeningsPreviewQuery request,
        CancellationToken cancellationToken)
    {
        var sourceDate = request.TargetDate.AddDays(-1);

        await using var session = store.QuerySession();

        var dayStartSource = cinemaTime.DayStart(sourceDate);
        var dayEndSource = cinemaTime.DayEnd(sourceDate);
        var dayStartTarget = cinemaTime.DayStart(request.TargetDate);
        var dayEndTarget = cinemaTime.DayEnd(request.TargetDate);

        var sourceCount = await session.Query<Domain.Screening.Screening>()
            .Where(s => s.StartTime >= dayStartSource && s.StartTime < dayEndSource)
            .CountAsync(cancellationToken);

        var targetHasScreenings = await session.Query<Domain.Screening.Screening>()
            .AnyAsync(s => s.StartTime >= dayStartTarget && s.StartTime < dayEndTarget, cancellationToken);

        return new RepeatScreeningsPreviewDto
        {
            SourceDate = sourceDate,
            TargetDate = request.TargetDate,
            SourceScreeningCount = sourceCount,
            TargetHasScreenings = targetHasScreenings
        };
    }
}
