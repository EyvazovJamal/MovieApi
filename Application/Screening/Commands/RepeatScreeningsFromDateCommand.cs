using Application.Screening.Dtos;
using MediatR;

namespace Application.Screening.Commands;

public sealed record RepeatScreeningsFromDateCommand(DateOnly TargetDate) : IRequest<RepeatScreeningsResultDto>;
