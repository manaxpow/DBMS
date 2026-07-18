using System;
using Xunit;

public class MonitoringManagerTests
{
    [Fact]
    public void CollectMetrics_WhenSourcesAreAvailable_ShouldReturnMetrics()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Evaluate_WhenThresholdIsExceeded_ShouldRaiseAlert()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CollectMetrics_WhenSourceFails_ShouldRecordFailure()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void RegisterSource_ShouldAddMetricSource()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void RegisterSource_WhenSourceAlreadyExists_ShouldNotDuplicate()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void RegisterSource_WhenSourceIsNull_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CollectMetrics_WhenNoSourcesExist_ShouldReturnEmptyCollection()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void CollectMetrics_WhenOneSourceFails_ShouldContinueCollectingOtherSources()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Evaluate_WhenThresholdIsNotExceeded_ShouldNotRaiseAlert()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Evaluate_WhenAlertIsAlreadyActive_ShouldNotDuplicateAlert()
    {
        throw new NotImplementedException();
    }
}
