public class LockManager
{
    private Dictionary<object, LockQueue> _lockTable = new Dictionary<object, LockQueue>();

    public bool Acquire(Transaction tx, object resource, LockMode mode)
    {
        throw new NotImplementedException();
    }

    public bool Upgrade(Transaction tx, object resource)
    {
        throw new NotImplementedException();
    }

    public bool HasLock(Transaction tx, LockMode mode) => throw new NotImplementedException();

    public void Release(Transaction tx, object resource)
    {
        throw new NotImplementedException();
    }

    public void ReleaseAll(Transaction tx)
    {
        throw new NotImplementedException();
    }

    public bool DetectDeadlock()
    {
        throw new NotImplementedException();
    }

    public bool Contains(Transaction tx, object resource)
    {
        throw new NotImplementedException();
    }

    private LockQueue GetLockQueue(object resource)
    {
        throw new NotImplementedException();
    }

    private object BuildWaitsForGraph()
    {
        throw new NotImplementedException();
    }

    private object FindCycles(object graph)
    {
        throw new NotImplementedException();
    }

    private Transaction SelectVictim(object cycle)
    {
        throw new NotImplementedException();
    }
}
