using System;

public class TransactionManager : ITransactionManager
{
    public TransactionContext BeginTransaction(IsolationLevel isolationLevel)
    {
        return default;
    }

    public void Commit(TransactionId transactionId)
    {
    }

    public void Abort(TransactionId transactionId)
    {
    }

    public TransactionState GetState(TransactionId transactionId)
    {
        return default;
    }
}
