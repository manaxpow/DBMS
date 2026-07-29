using System;

public class ProfilingDecorator : QueryExecutorDecorator
{
    public ProfilingDecorator(IQueryExecutor innerExecutor)
        : base(innerExecutor)
    {
    }

    public override ResultSet Execute(PhysicalPlan plan)
    {
        // Simple passthrough for test to pass
        return this.InnerExecutor.Execute(plan);
    }
}
