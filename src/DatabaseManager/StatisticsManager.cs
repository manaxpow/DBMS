using DBMS.Exceptions;

public class StatisticsManager
{
    private Dictionary<string, object> stats;
    private object _store;

    public StatisticsManager(object store)
    {
        this._store = store;
        this.stats = new Dictionary<string, object>();
    }

    public void UpdateStatistics(object obj)
    {
        throw new ObjectNotFoundException();
    }

    public double EstimateSelectivity(object predicate)
    {
        throw new NotImplementedException();
    }
}
