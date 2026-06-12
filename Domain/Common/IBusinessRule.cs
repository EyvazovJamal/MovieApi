namespace Domain.Common;

public interface IBusinessRule
{
    public string Message { get; }
    public bool IsBroken();
}