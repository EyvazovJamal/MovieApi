using Application.Common.Contracts;
using Domain.Common;

namespace Application.Movie;

public interface IMovieRepository : IAggregateRootRepository<Domain.Movie.Movie>;
