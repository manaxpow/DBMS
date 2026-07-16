using System;

public class LockManager : ILockManager
{
    public bool AcquireLock(TransactionId transactionId, LockResource resource, LockMode mode)
    {
        return default;
    }

    public void ReleaseLock(TransactionId transactionId, LockResource resource)
    {
    }

    public void ReleaseAllLocks(TransactionId transactionId)
    {
    }
}
