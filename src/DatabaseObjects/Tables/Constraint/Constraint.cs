public abstract class Constraint
{
    public int Id { get; set; }

    public string Name { get; set; }
    public bool IsEnabled { get; private set; }

    protected Constraint()
    {
        IsEnabled = true;
    }

    protected Constraint(string name)
    {
        Name = name;
        IsEnabled = true;
    }

    protected abstract bool Check(ConstraintContext context);

    public bool Validate(ConstraintContext context)
    {
        if (!IsEnabled)
        {
            return true;
        }
        return Check(context);
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }
}
