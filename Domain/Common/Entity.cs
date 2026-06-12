namespace Domain.Common;

public abstract class Entity : IEntity
{
    public Guid Id { get; protected set; }

    protected Entity(Guid id)
    {
        Id = id;
    }

    protected Entity() 
    { }

    protected void CheckRule(IBusinessRule rule)
    {
        if (rule.IsBroken())
        {
            throw new BusinessRuleValidationException(rule);
        }
    }

    public override bool Equals(object? obj)
    {
        return obj is Entity entity && Id.Equals(entity.Id);
    }

    public override int GetHashCode()
    {
        return Id.GetHashCode();
    }

    public static bool operator ==(Entity a, Entity b)
    {
        return Equals(a, b);
    }

    public static bool operator !=(Entity a, Entity b)
    {
        return !Equals(a, b);
    }
}