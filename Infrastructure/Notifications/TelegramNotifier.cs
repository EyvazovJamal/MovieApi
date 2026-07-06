using Application.Common;

namespace Infrastructure.Notifications;

using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Application.Notifications;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

public class TelegramNotifier(
    HttpClient http,
    IOptions<TelegramSettings> options,
    ILogger<TelegramNotifier> logger) : ITelegramNotifier
{
    public async Task NotifyMovieAddedToCinemaAsync(
        string title,
        string? posterPath,
        double voteAverage,
        int runtimeMinutes,
        CancellationToken ct = default)
    {
        var settings = options.Value;

        if (!settings.Enabled)
            return;

        var hours = runtimeMinutes / 60;
        var mins = runtimeMinutes % 60;

        var caption =
            $"🍿 Новый фильм в прокате!\n\n" +
            $"Название: {title}\n" +
            $"⭐ Рейтинг: {voteAverage:0.0}\n" +
            $"⏱ Длительность: {hours}ч {mins}м";

        var url = $"https://api.telegram.org/bot{settings.BotToken}/sendPhoto";

        var photoUrl = $"https://image.tmdb.org/t/p/w500{posterPath}";

        var form = new FormUrlEncodedContent(new Dictionary<string, string>
        {
            ["chat_id"] = settings.ChatId,
            ["photo"] = photoUrl,
            ["caption"] = caption
        });

        try
        {
            var response = await http.PostAsync(url, form, ct);

            var responseBody = await response.Content.ReadAsStringAsync(ct);

            logger.LogInformation(
                "Telegram response ({StatusCode}): {Response}",
                response.StatusCode,
                responseBody);

            if (!response.IsSuccessStatusCode)
            {
                logger.LogWarning(
                    "Telegram failed ({StatusCode}): {Response}",
                    response.StatusCode,
                    responseBody);
            }
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Telegram notification failed");
        }
    }
}