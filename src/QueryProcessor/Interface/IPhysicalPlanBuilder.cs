using System;

public interface IPhysicalPlanBuilder
{
    PhysicalPlan Build(LogicalPlan logicalPlan);
}
