public abstract class ConstraintMetadata
{
    public string Name { get; set; }
    public abstract ConstraintType Type { get; }
    public bool IsEnabled { get; set; }

    protected ConstraintMetadata(string name, bool isEnabled = true)
    {
        Name = name;
        IsEnabled = isEnabled;
    }
}
