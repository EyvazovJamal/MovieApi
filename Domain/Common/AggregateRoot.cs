using System.Runtime.Serialization;
using SharedKernel.Contracts;

namespace Domain.Common;

public abstract class AggregateRoot : Entity, IAggregateRoot
{
    private readonly List<IDomainEvent> events = new();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        events.Add(domainEvent);
    }

    [IgnoreDataMember]
    public IReadOnlyList<IDomainEvent> DomainEvents => events.AsReadOnly();

    public void ClearDomainEvents()
    {
        events.Clear();
    }

    public abstract void ApplyEvent(IDomainEvent domainEvent);
}
