namespace Application.Movie.Dtos;

public class MovieDto
{
    public Guid Id { get; set; }
    public bool Adult { get; set; }
    public string? BackdropPath { get; set; }
    public string Title { get; set; } = string.Empty;
    public string OriginalLanguage { get; set; } = string.Empty;
    public string OriginalTitle { get; set; } = string.Empty;
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public DateTimeOffset? ReleaseDate { get; set; }
    public double VoteAverage { get; set; }
    public int VoteCount { get; set; }
    public int Runtime { get; set; }
}