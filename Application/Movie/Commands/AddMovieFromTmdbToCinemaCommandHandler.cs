using MediatR;
using MovieApi.Application.Api;
using Domain.Movie;
using Marten;

namespace Application.Movie.Commands;

public class AddMovieFromTmdbToCinemaCommandHandler
    (ITmdbApi tmdbApi,
        IMovieRepository movieRepository,
        IDocumentStore store)
    : IRequestHandler<AddMovieFromTmdbToCinemaCommand>
{
    public async Task Handle(AddMovieFromTmdbToCinemaCommand request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var exists = await session.Query<Domain.Movie.Movie>()
            .AnyAsync(x => x.TmdbId == request.movieId, cancellationToken);
        if (exists)
            throw new InvalidOperationException("Movie already exists");
        var movieTmdb = await tmdbApi.GetMovieByIdAsync(request.movieId);
        var id=Guid.NewGuid();
        var movie = Domain.Movie.Movie.Create(
            id: id,
            tmdbId: request.movieId,
            adult: movieTmdb.adult,
            backdropPath: movieTmdb.backdrop_path,
            title: movieTmdb.title,
            originalLanguage: movieTmdb.original_language,
            originalTitle: movieTmdb.original_title,
            overview: movieTmdb.overview,
            posterPath: movieTmdb.poster_path,
            releaseDate: DateTimeOffset.UtcNow,
            voteAverage: movieTmdb.vote_average,
            voteCount: movieTmdb.vote_count,
            runtime: movieTmdb.runtime
            
        );
        await movieRepository.StoreAsync(movie);
    }
}