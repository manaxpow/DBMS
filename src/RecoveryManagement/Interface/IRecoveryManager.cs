using System;

public interface IRecoveryManager
{
    void RecoverDatabase();
    void UndoTransaction(TransactionId txId);
}
