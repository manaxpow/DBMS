using System;

public interface ILockManager
{
    bool AcquireLock(TransactionId transactionId, LockResource resource, LockMode mode);
    void ReleaseLock(TransactionId transactionId, LockResource resource);
    void ReleaseAllLocks(TransactionId transactionId);
}
