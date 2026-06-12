using Application.Movie;
using Infrastructure.Repositories;
using Marten;

namespace Infrastructure.Movie;

public sealed class MovieRepository(IDocumentSession ctx, IDocumentStore documentStore) 
    :AggregateRootRepository<Domain.Movie.Movie>(ctx, documentStore),IMovieRepository;