using System;

public class TransactionContext
{
    public TransactionId Id { get; set; }
    public TransactionState State { get; set; }
    public IsolationLevel Isolation { get; set; }
}
