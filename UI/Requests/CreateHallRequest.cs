namespace UI.Requests;

public class CreateHallRequest
{
    public required string Name { get; set; }

    public int SeatCount { get; set; }
}