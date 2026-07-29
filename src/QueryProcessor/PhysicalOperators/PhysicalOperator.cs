public abstract class PhysicalOperator
{
    public abstract void Open();

    public abstract bool Next();

    public abstract Row GetCurrent();

    public abstract void Close();
}
