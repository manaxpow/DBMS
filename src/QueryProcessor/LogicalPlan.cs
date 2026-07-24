using System;
using System.Collections.Generic;

public class LogicalPlan
{
    public List<LogicalNode> Nodes { get; set; } = new List<LogicalNode>();
    public bool IsValidated { get; set; }

    public void AddOperator()
    {
        throw new NotImplementedException();
    }

    public void Validate()
    {
        throw new NotImplementedException();
    }
}
