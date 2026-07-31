public abstract class ConstraintMetadata(string name, bool isEnabled = true)
{
    public string Name { get; set; } = name;

    public abstract ConstraintType Type { get; }

    public bool IsEnabled { get; set; } = isEnabled;
}
