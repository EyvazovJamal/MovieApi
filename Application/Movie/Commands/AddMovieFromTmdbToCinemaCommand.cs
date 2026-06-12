using MediatR;

namespace Application.Movie.Commands;

public sealed record AddMovieFromTmdbToCinemaCommand(int movieId) :IRequest;