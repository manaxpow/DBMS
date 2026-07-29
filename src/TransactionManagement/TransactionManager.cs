using System;

public class TransactionManager : IServerComponent
{
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
}
