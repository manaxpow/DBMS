public class DatabaseServerTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void Start_WhenConfigurationIsValid_ShouldStartServer()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Stop_WhenServerIsRunning_ShouldStopAllComponents()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Start_WhenPortIsUnavailable_ShouldThrow()
    {
        throw new NotImplementedException();
    }


    [Trait("Category", "Important")]
    [Fact]
    public void Start_WhenServerIsAlreadyRunning_ShouldNotInitializeComponentsAgain()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Start_WhenConfigurationIsInvalid_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Start_WhenComponentInitializationFails_ShouldRemainStopped()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Stop_WhenServerIsNotRunning_ShouldRemainStopped()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Stop_WhenComponentShutdownFails_ShouldReportFailureAndRemainConsistent()
    {
        throw new NotImplementedException();
    }
}

