namespace Domain.Common;

public sealed class BusinessRuleValidationException : Exception
{
    public IBusinessRule BrokenRule { get; }

    public string Details { get; }

    public BusinessRuleValidationException(IBusinessRule brokenRule) : base(brokenRule.Message)
    {
        this.BrokenRule = brokenRule;
        this.Details = brokenRule.Message;
    }

    public override string ToString()
    {
        return $"{BrokenRule.GetType().FullName}: {BrokenRule.Message}";
    }
}