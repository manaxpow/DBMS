using System;

public class ResourceMonitor : IResourceMonitor
{
    private object _cpu;
    private object _memory;

    public double GetCpuUsage()
    {
        return default;
    }

    public double GetMemoryUsage()
    {
        return default;
    }

    public StorageStatistics GetDiskIO()
    {
        return default;
    }

    public void SampleResources()
    {
    }
}
