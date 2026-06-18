namespace Application.Screening.Dtos;

public class NextSlotDto
{
    public Guid HallId { get; set; }
    public DateTimeOffset SuggestedStartTime { get; set; }
}
