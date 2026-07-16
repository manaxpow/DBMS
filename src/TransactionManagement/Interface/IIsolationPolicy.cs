using System;

public interface IIsolationPolicy
{
    void Enforce(TransactionContext context, LockResource resource, OperationAccess access);
}
