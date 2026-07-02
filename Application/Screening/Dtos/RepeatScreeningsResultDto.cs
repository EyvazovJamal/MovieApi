namespace Application.Screening.Dtos;

public class RepeatScreeningsResultDto
{
    public int CreatedCount { get; set; }
    public DateOnly SourceDate { get; set; }
    public DateOnly TargetDate { get; set; }
}
