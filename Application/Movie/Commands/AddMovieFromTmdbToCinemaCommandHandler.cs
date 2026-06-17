using MediatR;
using MovieApi.Application.Api;
using Domain.Movie;
namespace Application.Movie.Commands;

public class AddMovieFromTmdbToCinemaCommandHandler
    (ITmdbApi tmdbApi,
        IMovieRepository movieRepository)
    : IRequestHandler<AddMovieFromTmdbToCinemaCommand>
{
    public async Task Handle(AddMovieFromTmdbToCinemaCommand request, CancellationToken cancellationToken)
    {
        var movieTmdb = await tmdbApi.GetMovieByIdAsync(request.movieId);
        var id=Guid.NewGuid();
        var movie = Domain.Movie.Movie.Create(
            id:id,
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