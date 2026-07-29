public class HashJoinOperator : CompositePhysicalOperator
{
    public override void Open()
    {
    }

    public override bool Next()
    {
        return false;
    }

    public override Row GetCurrent()
    {
        throw new System.NotImplementedException();
    }

    public override void Close()
    {
    }
}
