using SharedKernel.Contracts;

public sealed record ScreeningCreatedEvent(
    Guid ScreeningId,
    Guid MovieId,
    Guid HallId,
    DateTimeOffset StartTime,
    DateTimeOffset EndTime,
    int Runtime
) : IDomainEvent;
