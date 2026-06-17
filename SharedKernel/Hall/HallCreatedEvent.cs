using SharedKernel.Contracts;

public sealed record HallCreatedEvent(
    Guid HallId,
    string Name,
    int SeatCount
) : IDomainEvent;