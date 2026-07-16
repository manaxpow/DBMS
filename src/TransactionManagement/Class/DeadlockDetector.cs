using System;

public class DeadlockDetector : IDeadlockDetector
{
    public DeadlockCycle Detect()
    {
        return default;
    }

    public void Resolve(DeadlockCycle cycle)
    {
    }
}
