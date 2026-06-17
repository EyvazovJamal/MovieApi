using Application.Movie.Dtos;
using MediatR;

namespace Application.Queries;

public class GetMyMoviesQuery : IRequest<List<MovieDto>>
{
    public GetMoviesFilter Filter { get; }
    public GetMyMoviesQuery(GetMoviesFilter filter)
    {
        Filter = filter;
    }
}