using System;

public interface ILogicalPlanBuilder
{
    LogicalPlan Build(BoundStatement statement);
}
