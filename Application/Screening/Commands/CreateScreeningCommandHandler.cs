using Domain.Screening.Rules;
using Marten;
using MediatR;

namespace Application.Screening.Commands;

public class CreateScreeningCommandHandler(
    IDocumentStore store,
    IScreeningRepository screeningRepository)
    : IRequestHandler<CreateScreeningCommand>
{
    public async Task Handle(CreateScreeningCommand request, CancellationToken cancellationToken)
    {
        await using var session = store.QuerySession();

        var movie = await session.LoadAsync<Domain.Movie.Movie>(request.MovieId, cancellationToken);
        if (movie is null)
            throw new InvalidOperationException("Movie not found");

        var hall = await session.LoadAsync<Domain.Hall.Hall>(request.HallId, cancellationToken);
        if (hall is null)
            throw new InvalidOperationException("Hall not found");

        if (movie.Runtime <= 0)
            throw new InvalidOperationException("Movie runtime is not set");

        var endTime = request.StartTime.AddMinutes(movie.Runtime);

        var existingInHall = await session.Query<Domain.Screening.Screening>()
            .Where(s => s.HallId == request.HallId)
            .ToListAsync(cancellationToken);

        var overlapRule = new ScreeningMustNotOverlapRule(
            request.StartTime,
            endTime,
            existingInHall);

        if (overlapRule.IsBroken())
            throw new Domain.Common.BusinessRuleValidationException(overlapRule);

        var id = Guid.NewGuid();
        var screening = Domain.Screening.Screening.Create(
            id,
            request.MovieId,
            request.HallId,
            request.StartTime,
            endTime,
            movie.Runtime);

        await screeningRepository.StoreAsync(screening, cancellationToken);
    }
}
