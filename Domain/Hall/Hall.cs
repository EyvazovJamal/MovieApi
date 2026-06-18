using Newtonsoft.Json;
using Domain.Common;
using Marten.Events.CodeGeneration;
using SharedKernel.Contracts;

namespace Domain.Hall;

public class Hall : AggregateRoot
{
    public string Name { get; private set; } = string.Empty;

    public int SeatCount { get; private set; }

    [JsonConstructor]
    internal Hall(
        Guid id,
        string name,
        int seatCount)
    {
        Id = id;
        Name = name;
        SeatCount = seatCount;
    }

    private Hall(
        Guid id,
        string name,
        int seatCount,
        bool _)
    {
        Id = id;
        Name = name;
        SeatCount = seatCount;

        AddDomainEvent(new HallCreatedEvent(
            id,
            name,
            seatCount));
    }

    [MartenIgnore]
    public static Hall Create(
        Guid id,
        string name,
        int seatCount)
    {
        return new Hall(
            id,
            name,
            seatCount,
            true);
    }   

    public void Apply(HallCreatedEvent @event)
    {
        InternalApply(@event);
    }

    public override void ApplyEvent(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case HallCreatedEvent e:
                InternalApply(e);
                break;
        }
    }

    private void InternalApply(HallCreatedEvent e)
    {
        Id = e.HallId;
        Name = e.Name;
        SeatCount = e.SeatCount;
    }
}