using System;

public class PerformanceMonitor : IPerformanceMonitor
{
    private object _scheduler;

    public void StartMonitoring()
    {
    }

    public void StopMonitoring()
    {
    }

    public PerformanceSnapshot GetSnapshot()
    {
        return default;
    }

    public void AggregateMetrics()
    {
    }
}
