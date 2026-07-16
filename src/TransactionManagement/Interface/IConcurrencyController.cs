using System;

public interface IConcurrencyController
{
    bool CanAccess(TransactionContext context, LockResource resource, OperationAccess access);
}
