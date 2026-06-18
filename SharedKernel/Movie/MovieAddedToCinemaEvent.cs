using SharedKernel.Contracts;

namespace SharedKernel.Movie;

public record MovieAddedToCinemaEvent(
    Guid MovieId,
    int TmdbId,
    bool Adult,
    string? BackdropPath,
    string Title,
    string OriginalLanguage,
    string OriginalTitle,
    string? Overview,
    string? PosterPath,
    DateTimeOffset? ReleaseDate,
    double VoteAverage,
    int VoteCount,
    int Runtime):IDomainEvent;

