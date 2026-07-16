using System;

public interface IDeadlockDetector
{
    DeadlockCycle Detect();
    void Resolve(DeadlockCycle cycle);
}
