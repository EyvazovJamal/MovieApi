using Application.Screening.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetScreeningsByDateQueryHandler(IDocumentStore store)
    : IRequestHandler<GetScreeningsByDateQuery, List<ScreeningDto>>
{
    public async Task<List<ScreeningDto>> Handle(
        GetScreeningsByDateQuery request,
        CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var dayStart = request.Date.ToDateTime(TimeOnly.MinValue, DateTimeKind.Utc);
        var dayEnd = dayStart.AddDays(1);

        var screenings = await session.Query<Domain.Screening.Screening>()
            .Where(s => s.StartTime >= dayStart && s.StartTime < dayEnd)
            .OrderBy(s => s.HallId)
            .ThenBy(s => s.StartTime)
            .ToListAsync(cancellationToken);

        var movies = await session.Query<Domain.Movie.Movie>().ToListAsync(cancellationToken);
        var halls = await session.Query<Domain.Hall.Hall>().ToListAsync(cancellationToken);

        var moviesById = movies.ToDictionary(m => m.Id);
        var hallsById = halls.ToDictionary(h => h.Id);

        return screenings.Select(s =>
        {
            moviesById.TryGetValue(s.MovieId, out var movie);
            hallsById.TryGetValue(s.HallId, out var hall);

            return new ScreeningDto
            {
                Id = s.Id,
                MovieId = s.MovieId,
                MovieTitle = movie?.Title ?? string.Empty,
                HallId = s.HallId,
                HallName = hall?.Name ?? string.Empty,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                Runtime = s.Runtime,
                BufferMinutes = Domain.Screening.Screening.BufferMinutes
            };
        }).ToList();
    }
}
