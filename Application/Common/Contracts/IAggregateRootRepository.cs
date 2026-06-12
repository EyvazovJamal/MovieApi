using Domain.Common;
using Marten.Linq;

namespace Application.Common.Contracts;

public interface IAggregateRootRepository<T>
    where T : IAggregateRoot
{
    Task StoreAsync(T aggregate, CancellationToken ct = default);
    Task<T?> AggregateAsync(Guid id, int? version = null, CancellationToken ct = default);
}
