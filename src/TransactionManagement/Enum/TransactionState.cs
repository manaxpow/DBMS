using System;

public enum TransactionState { 
    Active, 
    PartiallyCommitted, 
    Committed, 
    Failed, 
    Aborted 
}
