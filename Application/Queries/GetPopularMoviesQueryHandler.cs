using MediatR;
using MovieApi.Application.Api;
using MovieApi.Application.Api.Response;

namespace Application.Queries;

public class GetPopularMoviesQueryHandler(
    ITmdbApi tmdbApi) 
    : IRequestHandler<GetPopularMoviesQuery,List<PopularMoviesResponse>>
{
    public async Task<List<PopularMoviesResponse>> Handle(GetPopularMoviesQuery request, CancellationToken cancellationToken)
    {
        var movies = await tmdbApi.GetPopularMoviesAsync();
        if (movies==null)
            throw new NullReferenceException();
        return movies.Results;
    }
}