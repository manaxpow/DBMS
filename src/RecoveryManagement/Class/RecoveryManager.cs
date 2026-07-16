using System;

public class RecoveryManager : IRecoveryManager
{
    private object _ctx;

    public void RecoverDatabase()
    {
    }

    public void UndoTransaction(TransactionId txId)
    {
    }

    public void AnalyzeState()
    {
    }
}
