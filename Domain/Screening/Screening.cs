using Newtonsoft.Json;
using Domain.Common;
using Marten.Events.CodeGeneration;
using SharedKernel.Contracts;
using SharedKernel.Screening;

namespace Domain.Screening;

public class Screening : AggregateRoot
{
    public const int BufferMinutes = 10;

    public Guid MovieId { get; private set; }
    public Guid HallId { get; private set; }
    public DateTimeOffset StartTime { get; private set; }
    public DateTimeOffset EndTime { get; private set; }
    public int Runtime { get; private set; }

    [JsonIgnore]
    public DateTimeOffset OccupiedUntil => EndTime.AddMinutes(BufferMinutes);

    [JsonConstructor]
    internal Screening(
        Guid id,
        Guid movieId,
        Guid hallId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int runtime)
    {
        Id = id;
        MovieId = movieId;
        HallId = hallId;
        StartTime = startTime;
        EndTime = endTime;
        Runtime = runtime;
    }

    private Screening(
        Guid id,
        Guid movieId,
        Guid hallId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int runtime,
        bool _)
    {
        Id = id;
        MovieId = movieId;
        HallId = hallId;
        StartTime = startTime;
        EndTime = endTime;
        Runtime = runtime;

        AddDomainEvent(new ScreeningCreatedEvent(
            id,
            movieId,
            hallId,
            startTime,
            endTime,
            runtime));
    }

    [MartenIgnore]
    public static Screening Create(
        Guid id,
        Guid movieId,
        Guid hallId,
        DateTimeOffset startTime,
        DateTimeOffset endTime,
        int runtime)
    {
        return new Screening(
            id,
            movieId,
            hallId,
            startTime,
            endTime,
            runtime,
            true);
    }

    public void Apply(ScreeningCreatedEvent @event)
    {
        InternalApply(@event);
    }

    public override void ApplyEvent(IDomainEvent domainEvent)
    {
        switch (domainEvent)
        {
            case ScreeningCreatedEvent e:
                InternalApply(e);
                break;
        }
    }

    private void InternalApply(ScreeningCreatedEvent e)
    {
        Id = e.ScreeningId;
        MovieId = e.MovieId;
        HallId = e.HallId;
        StartTime = e.StartTime;
        EndTime = e.EndTime;
        Runtime = e.Runtime;
    }

    public static bool Overlaps(
        DateTimeOffset startA,
        DateTimeOffset endA,
        DateTimeOffset startB,
        DateTimeOffset endB)
    {
        var occupiedEndA = endA.AddMinutes(BufferMinutes);
        var occupiedEndB = endB.AddMinutes(BufferMinutes);
        return startA < occupiedEndB && occupiedEndA > startB;
    }

    public void Delete()
    {
        AddDomainEvent(new ScreeningDeletedEvent(Id));
    }
}
