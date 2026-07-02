using SharedKernel.Contracts;

namespace SharedKernel.Screening;

public sealed record ScreeningDeletedEvent(Guid ScreeningId) : IDomainEvent;
