using Application.Screening;
using Domain.Common;
using Infrastructure.Repositories;
using Marten;

namespace Infrastructure.Screening;

public sealed class ScreeningRepository(IDocumentSession ctx, IDocumentStore documentStore)
    : AggregateRootRepository<Domain.Screening.Screening>(ctx, documentStore), IScreeningRepository;
