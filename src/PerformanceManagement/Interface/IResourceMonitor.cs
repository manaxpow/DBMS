using System;

public interface IResourceMonitor
{
    double GetCpuUsage();
    double GetMemoryUsage();
    StorageStatistics GetDiskIO();
}
