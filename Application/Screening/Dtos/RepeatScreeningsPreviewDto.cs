namespace Application.Screening.Dtos;

public class RepeatScreeningsPreviewDto
{
    public DateOnly SourceDate { get; set; }
    public DateOnly TargetDate { get; set; }
    public int SourceScreeningCount { get; set; }
    public bool TargetHasScreenings { get; set; }
    public bool CanRepeat => SourceScreeningCount > 0 && !TargetHasScreenings;
}
