using Application.Screening.Dtos;
using MediatR;

namespace Application.Queries;

public sealed record GetRepeatScreeningsPreviewQuery(DateOnly TargetDate) : IRequest<RepeatScreeningsPreviewDto>;
