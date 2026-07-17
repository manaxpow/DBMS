using System;

public class TransactionExecutionException : Exception
{
    public TransactionExecutionException() : base() { }
    public TransactionExecutionException(string message) : base(message) { }
    public TransactionExecutionException(string message, Exception inner) : base(message, inner) { }
}
