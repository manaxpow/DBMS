using System;

public class ConcurrencyController : IConcurrencyController
{
    private IIsolationPolicy _isolationPolicy;

    public ConcurrencyController(IIsolationPolicy isolationPolicy)
    {
        _isolationPolicy = isolationPolicy;
    }

    public bool CanAccess(TransactionContext context, LockResource resource, OperationAccess access)
    {
        return default;
    }
}
