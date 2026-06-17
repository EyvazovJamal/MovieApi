using Application.Hall.Dtos;
using Application.Movie.Dtos;
using MediatR;

namespace Application.Queries;

public class GetHallsQuery : IRequest<List<HallDto>>
{
    public GetMoviesFilter Filter { get; }
    public GetHallsQuery(GetMoviesFilter filter)
    {
        Filter = filter;
    }
}