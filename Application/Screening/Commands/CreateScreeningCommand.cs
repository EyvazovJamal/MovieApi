using MediatR;

namespace Application.Screening.Commands;

public sealed record CreateScreeningCommand(
    Guid MovieId,
    Guid HallId,
    DateTimeOffset StartTime) : IRequest;
