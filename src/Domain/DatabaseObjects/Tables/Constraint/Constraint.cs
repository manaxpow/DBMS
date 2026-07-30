public abstract class Constraint(string name)
{
    protected Constraint()
        : this(string.Empty)
    {
    }

    public int Id { get; set; }

    public string Name { get; set; } = name;

    public bool IsEnabled { get; private set; } = true;

    public bool Validate(ConstraintContext context)
    {
        if (!this.IsEnabled)
        {
            throw new NotImplementedException();
        }

        throw new NotImplementedException();
    }

    public void Enable()
    {
        throw new NotImplementedException();
    }

    public void Disable()
    {
        throw new NotImplementedException();
    }

    protected abstract bool Check(ConstraintContext context);
}
