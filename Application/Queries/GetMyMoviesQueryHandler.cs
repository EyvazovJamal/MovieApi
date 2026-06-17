using Application.Common;
using Application.Movie;
using Application.Movie.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetMyMoviesQueryHandler(IDocumentStore store)
    : IRequestHandler<GetMyMoviesQuery, List<MovieDto>>
{
    public async Task<List<MovieDto>> Handle(GetMyMoviesQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();
        var filter = request.Filter ?? new GetMoviesFilter();
        IQueryable<Domain.Movie.Movie> query = session.Query<Domain.Movie.Movie>();
        if (filter.Skip.HasValue && filter.Skip.Value > 0)
            query = query.Skip(filter.Skip.Value);
        if (filter.Take.HasValue && filter.Take.Value > 0)
            query = query.Take(filter.Take.Value);
        var movies = await query.ToListAsync(cancellationToken);
        return movies.Select(m => new MovieDto
        {
            Id = m.Id,
            Adult = m.Adult,
            BackdropPath = m.BackdropPath,
            Title = m.Title,
            OriginalLanguage = m.OriginalLanguage,
            OriginalTitle = m.OriginalTitle,
            Overview = m.Overview,
            PosterPath = m.PosterPath,
            ReleaseDate = m.ReleaseDate,
            VoteAverage = m.VoteAverage,
            VoteCount = m.VoteCount,
            Runtime = m.Runtime
        }).ToList();
    }
}