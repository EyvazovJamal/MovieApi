using Application.Common;
using Application.Screening.Dtos;
using Domain.Common;
using Domain.Screening.Rules;
using Marten;
using MediatR;

namespace Application.Screening.Commands;

public class RepeatScreeningsFromDateCommandHandler(
    IDocumentStore store,
    IDocumentSession session,
    ICinemaTime cinemaTime)
    : IRequestHandler<RepeatScreeningsFromDateCommand, RepeatScreeningsResultDto>
{
    public async Task<RepeatScreeningsResultDto> Handle(
        RepeatScreeningsFromDateCommand request,
        CancellationToken cancellationToken)
    {
        var sourceDate = request.TargetDate.AddDays(-1);

        await using var querySession = store.QuerySession();

        var sourceScreenings = await LoadScreeningsForDateAsync(querySession, sourceDate, cancellationToken);
        var targetScreenings = await LoadScreeningsForDateAsync(querySession, request.TargetDate, cancellationToken);

        var targetEmptyRule = new TargetDayMustHaveNoScreeningsRule(targetScreenings.Count);
        if (targetEmptyRule.IsBroken())
            throw new BusinessRuleValidationException(targetEmptyRule);

        if (sourceScreenings.Count == 0)
        {
            return new RepeatScreeningsResultDto
            {
                CreatedCount = 0,
                SourceDate = sourceDate,
                TargetDate = request.TargetDate
            };
        }

        var dayOffset = request.TargetDate.DayNumber - sourceDate.DayNumber;
        var movies = await querySession.Query<Domain.Movie.Movie>().ToListAsync(cancellationToken);
        var halls = await querySession.Query<Domain.Hall.Hall>().ToListAsync(cancellationToken);
        var moviesById = movies.ToDictionary(m => m.Id);
        var hallsById = halls.ToDictionary(h => h.Id);

        var toCreate = new List<Domain.Screening.Screening>();

        foreach (var source in sourceScreenings)
        {
            if (!moviesById.TryGetValue(source.MovieId, out var movie))
                throw new InvalidOperationException("Movie not found");

            if (!hallsById.TryGetValue(source.HallId, out _))
                throw new InvalidOperationException("Hall not found");

            if (movie.Runtime <= 0)
                throw new InvalidOperationException("Movie runtime is not set");

            var newStart = source.StartTime.AddDays(dayOffset);
            var newEnd = source.EndTime.AddDays(dayOffset);

            var existingInHall = targetScreenings
                .Where(s => s.HallId == source.HallId)
                .Concat(toCreate.Where(s => s.HallId == source.HallId))
                .ToList();

            var overlapRule = new ScreeningMustNotOverlapRule(newStart, newEnd, existingInHall);
            if (overlapRule.IsBroken())
                throw new BusinessRuleValidationException(overlapRule);

            toCreate.Add(Domain.Screening.Screening.Create(
                Guid.NewGuid(),
                source.MovieId,
                source.HallId,
                newStart,
                newEnd,
                source.Runtime));
        }

        foreach (var screening in toCreate)
        {
            var events = screening.DomainEvents;
            if (events is { Count: > 0 })
            {
                session.Events.Append(screening.Id, events);
                screening.ClearDomainEvents();
            }
        }

        await session.SaveChangesAsync(cancellationToken);

        return new RepeatScreeningsResultDto
        {
            CreatedCount = toCreate.Count,
            SourceDate = sourceDate,
            TargetDate = request.TargetDate
        };
    }

    private async Task<IReadOnlyList<Domain.Screening.Screening>> LoadScreeningsForDateAsync(
        IQuerySession querySession,
        DateOnly date,
        CancellationToken cancellationToken)
    {
        var dayStart = cinemaTime.DayStart(date);
        var dayEnd = cinemaTime.DayEnd(date);

        return await querySession.Query<Domain.Screening.Screening>()
            .Where(s => s.StartTime >= dayStart && s.StartTime < dayEnd)
            .OrderBy(s => s.HallId)
            .ThenBy(s => s.StartTime)
            .ToListAsync(cancellationToken);
    }
}
