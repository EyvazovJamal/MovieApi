using Application.Screening.Dtos;
using MediatR;

namespace Application.Queries;

public sealed record GetScreeningsByDateQuery(DateOnly Date) : IRequest<List<ScreeningDto>>;
