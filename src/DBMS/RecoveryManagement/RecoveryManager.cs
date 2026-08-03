public class RecoveryManager : IServerComponent
{
    public void Start(object config)
    {
        throw new NotImplementedException();
    }

    public void Stop()
    {
        throw new NotImplementedException();
    }

    public void Recover()
    {
        throw new NotImplementedException();
    }

    private List<Transaction> IdentifyCommittedTransactions(object logRecords)
    {
        throw new NotImplementedException();
    }

    private List<Transaction> IdentifyUncommittedTransactions(object logRecords)
    {
        throw new NotImplementedException();
    }

    private void ApplyRedo(object record)
    {
        throw new NotImplementedException();
    }

    private void ApplyUndo(object record)
    {
        throw new NotImplementedException();
    }
}
