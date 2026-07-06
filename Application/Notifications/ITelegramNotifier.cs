namespace Application.Notifications;

public interface ITelegramNotifier
{
    Task NotifyMovieAddedToCinemaAsync(
        string title,
        string? posterPath,
        double voteAverage,
        int runtimeMinutes,
        CancellationToken ct = default);
}