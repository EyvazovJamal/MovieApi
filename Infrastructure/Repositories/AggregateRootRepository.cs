using Application.Common.Contracts;
using Domain.Common;
using Marten;
using Marten.Linq;

namespace Infrastructure.Repositories;

public abstract class AggregateRootRepository<T>(
    IDocumentSession ctx,
    IDocumentStore documentStore)
    : IAggregateRootRepository<T>
    where T : class, IAggregateRoot
{
    protected readonly IDocumentSession Ctx = ctx;

    public async Task StoreAsync(T aggregate, CancellationToken ct = default)
    {
        var events = aggregate.DomainEvents;

        if (events is null || events.Count == 0)
            return;

        Ctx.Events.Append(aggregate.Id, events);

        await Ctx.SaveChangesAsync(ct);

        aggregate.ClearDomainEvents();
    }

    public async Task<T?> AggregateAsync(
        Guid id,
        int? version = null,
        CancellationToken ct = default)
    {
        var aggregate = await Ctx.Events.AggregateStreamAsync<T>(
            id,
            version ?? 0,
            token: ct);

        return aggregate;
    }
}