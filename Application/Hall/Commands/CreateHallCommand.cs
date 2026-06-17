using MediatR;

namespace Application.Hall.Commands;

public sealed record CreateHallCommand(string Name,  int SeatCount):IRequest;