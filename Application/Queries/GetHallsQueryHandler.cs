using Application.Hall.Dtos;
using Application.Movie.Dtos;
using Marten;
using MediatR;

namespace Application.Queries;

public class GetHallsQueryHandler(IDocumentStore store):IRequestHandler<GetHallsQuery,List<HallDto>>
{
    public async Task<List<HallDto>> Handle(GetHallsQuery request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();
        var filter = request.Filter ?? new GetMoviesFilter();
        IQueryable<Domain.Hall.Hall> query = session.Query<Domain.Hall.Hall>();
        if (filter.Skip.HasValue && filter.Skip.Value > 0)
            query = query.Skip(filter.Skip.Value);
        if (filter.Take.HasValue && filter.Take.Value > 0)
            query = query.Take(filter.Take.Value);
        var halls = await query.ToListAsync(cancellationToken);
        return halls.Select(h => new HallDto
        {
            Id = h.Id,
            Name=h.Name,
            SeatCount=h.SeatCount,
        }).ToList();
        
    }
}