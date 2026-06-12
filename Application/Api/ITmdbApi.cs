using MovieApi.Application.Api.Response;
using Refit;

namespace MovieApi.Application.Api;

public interface ITmdbApi
{ 
    [Get("/movie/popular")]
    Task<TmdbEnvelope>  GetPopularMoviesAsync();
    [Get("/movie/{id}")]
    Task<MovieByIdResponse> GetMovieByIdAsync(int id);
}