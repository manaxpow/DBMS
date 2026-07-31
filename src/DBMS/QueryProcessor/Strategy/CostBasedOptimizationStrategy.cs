using System;
using System.Collections.Generic;
using System.Linq;

public class CostBasedOptimizationStrategy : IOptimizationStrategy
{
    public PhysicalPlan Optimize(LogicalPlan plan)
    {
        var candidates = this.GenerateCandidatePlans(plan);
        this.EstimateCost(candidates);
        return this.SelectBestPlan(candidates);
    }

    public virtual List<PhysicalPlan> GenerateCandidatePlans(LogicalPlan plan)
    {
        throw new NotImplementedException();
    }

    public virtual void EstimateCost(List<PhysicalPlan> candidates)
    {
        throw new NotImplementedException();
    }

    public virtual PhysicalPlan SelectBestPlan(List<PhysicalPlan> candidates)
    {
        throw new NotImplementedException();
    }
}
