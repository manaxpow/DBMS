public interface IQueryExecutor
{
    public ResultSet Execute(PhysicalPlan plan);
}
