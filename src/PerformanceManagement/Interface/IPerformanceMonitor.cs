using System;

public interface IPerformanceMonitor
{
    void StartMonitoring();
    void StopMonitoring();
    PerformanceSnapshot GetSnapshot();
}
