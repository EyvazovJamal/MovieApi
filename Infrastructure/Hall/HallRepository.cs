using Application.Hall;
using Domain.Common;
using Infrastructure.Repositories;
using Marten;

namespace Infrastructure.Hall;

public sealed class HallRepository(IDocumentSession ctx, IDocumentStore documentStore)
    :AggregateRootRepository<Domain.Hall.Hall>(ctx, documentStore),IHallRepository;