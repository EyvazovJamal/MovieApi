using Marten.Events.Aggregation;
using SharedKernel.Movie;

namespace Infrastructure.Movie;

public class MovieSingleStreamProjector : SingleStreamProjection<Domain.Movie.Movie>
{
    public MovieSingleStreamProjector()
    {
        IncludeType<MovieAddedToCinemaEvent>();
    }

    public static Domain.Movie.Movie Create(MovieAddedToCinemaEvent e)
    {
        return Domain.Movie.Movie.Create(e.MovieId,e.Adult,e.BackdropPath,e.Title,e.OriginalLanguage,
            e.OriginalTitle,e.Overview,e.PosterPath,e.ReleaseDate,e.VoteAverage,e.VoteCount);
    }
}