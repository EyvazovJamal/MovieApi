namespace Domain.Common;

public sealed record HallRowLayout(int RowNumber, int SeatCount);

public static class HallLayout
{
    public const decimal DefaultTicketPrice = 10m;

    public static IReadOnlyList<HallRowLayout> DefaultRows { get; } =
        Enumerable.Range(1, 10)
            .Select(r => new HallRowLayout(r, r == 10 ? 14 : 12))
            .OrderByDescending(r => r.RowNumber)
            .ToList();

    public static bool IsValidSeat(int row, int seat)
    {
        var layout = DefaultRows.FirstOrDefault(r => r.RowNumber == row);
        return layout is not null && seat >= 1 && seat <= layout.SeatCount;
    }

    public static string SeatKey(int row, int seat) => $"{row}-{seat}";
}
