using System;
using System.Collections.Generic;
using DBMS.Exceptions;

public class StatisticsManager
{
    private Dictionary<string, object> _stats;
    private object _store;

    public void UpdateStatistics(object obj)
    {
        throw new ObjectNotFoundException();
    }

    public double EstimateSelectivity(object predicate)
    {
        throw new NotImplementedException();
    }
}
