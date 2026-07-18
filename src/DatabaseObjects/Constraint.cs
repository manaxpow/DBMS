using System;

public abstract class Constraint
{
    public string Name { get; set; }
    public bool IsEnabled { get; set; }

    protected Constraint()
    {
    }

    protected Constraint(string name)
    {
        Name = name;
    }

    protected abstract bool Check(object value);
    public bool Validate(object value) => throw new NotImplementedException();
    public void Apply(object value) => throw new NotImplementedException();
    public void Enable() => throw new NotImplementedException();
    public void Disable() => throw new NotImplementedException();
    protected virtual void OnApply(object value) => throw new NotImplementedException();
}
