using MediatR;
using MovieApi.Application.Api.Response;

namespace Application.Queries;

public record GetPopularMoviesQuery : IRequest<List<PopularMoviesResponse>>;
    
