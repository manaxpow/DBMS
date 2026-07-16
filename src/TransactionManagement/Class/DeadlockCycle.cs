using System;
using System.Collections.Generic;

public class DeadlockCycle
{
    public List<TransactionId> Transactions { get; set; } = new List<TransactionId>();
}
