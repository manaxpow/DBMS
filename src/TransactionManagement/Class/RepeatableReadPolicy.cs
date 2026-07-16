using System;

public class RepeatableReadPolicy : IIsolationPolicy
{
    private ILockManager _lockManager;

    public RepeatableReadPolicy(ILockManager lockManager)
    {
        _lockManager = lockManager;
    }

    public void Enforce(TransactionContext context, LockResource resource, OperationAccess access)
    {
    }
}
