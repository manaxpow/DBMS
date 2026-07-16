using System;

public interface ITransactionManager
{
    TransactionContext BeginTransaction(IsolationLevel isolationLevel);
    void Commit(TransactionId transactionId);
    void Abort(TransactionId transactionId);
    TransactionState GetState(TransactionId transactionId);
}
