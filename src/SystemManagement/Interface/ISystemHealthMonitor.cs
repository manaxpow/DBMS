using System;

public interface ISystemHealthMonitor
{
    SystemHealthReport PerformHealthCheck();
    void RegisterHealthCheck(IHealthCheck check);
}
