using MediatR;

namespace Application.Screening.Commands;

public sealed record DeleteScreeningCommand(Guid id) : IRequest;
