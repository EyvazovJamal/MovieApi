using Application.Screening.Dtos;
using MediatR;

namespace Application.Queries;

public sealed record GetNextSlotQuery(Guid HallId, DateOnly Date) : IRequest<NextSlotDto>;
