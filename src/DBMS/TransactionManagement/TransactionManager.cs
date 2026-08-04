public class TransactionManager : IServerComponent
{
    private List<Transaction> _activeTransactions = new List<Transaction>();

    public void Start(object config)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public Transaction BeginTransaction()
    {
        throw new NotImplementedException();
    }

    public void Commit(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public void Rollback(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public void Complete(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    public bool Contains(Transaction transaction)
    {
        throw new NotImplementedException();
    }

    private int NextTransactionId()
    {
        throw new NotImplementedException();
    }
}
