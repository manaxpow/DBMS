using System.ComponentModel;
using DBMS.Exceptions;
using FluentAssertions;
using NSubstitute;

public class DatabaseServerTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void Start_WhenConfigurationIsValid_ShouldStartServer()
    {
        // Arrange
        var components = Substitute.For<IServerComponent>();
        var server = new DatabaseServer(new List<IServerComponent> { components });
        var config = new { Port = 5432, MaxConnections = 100 };

        // Act
        server.Start(config);

        // Assert
        server.IsRunning.Should().Be(true);
    }


    [Trait("Category", "Important")]
    [Fact]
    public void Start_WhenServerIsAlreadyRunning_ShouldNotInitializeComponentsAgain()
    {
        // Arrange
        var components = Substitute.For<IServerComponent>();
        var server = new DatabaseServer(new List<IServerComponent> { components });
        var config = new { Port = 5432, MaxConnections = 100 };

        server.Start(config);

        // Act
        server.Start(config);

        // Assert
        components.Received(1).Start(config);
        server.IsRunning.Should().Be(true);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Start_WhenComponentInitializationFails_ShouldRemainStopped()
    {
        // Arrange
        var components = Substitute.For<IServerComponent>();
        components
            .When(c => c.Start(Arg.Any<object>()))
            .Do(x => throw new ComponentInitializationException());

        var server = new DatabaseServer(new List<IServerComponent> { components });
        var config = new { Port = 5432, MaxConnections = 100 };

        // Act
        Action act = () => server.Start(config);

        // Assert
        act.Should().Throw<ComponentInitializationException>();
        server.IsRunning.Should().Be(false);
    }


    [Trait("Category", "Important")]
    [Fact]
    public void Stop_WhenServerIsRunning_ShouldStopAllComponents()
    {
        // Arrange
        var components = Substitute.For<IServerComponent>();
        var config = new { Port = 5432, MaxConnections = 100 };
        var server = new DatabaseServer(new List<IServerComponent> { components });
        server.Start(config);

        // Act
        server.Stop();

        // Assert
        components.Received(1).Stop();
        server.IsRunning.Should().Be(false);
    }

    [Fact]
    public void Start_WhenPortIsUnavailable_ShouldThrow()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void Start_WhenConfigurationIsInvalid_ShouldThrow()
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
    public void Stop_WhenComponentStopFails_ShouldReportFailureAndRemainConsistent()
    {
        // Arrange
        var components = Substitute.For<IServerComponent>();
        components
            .When(c => c.Stop())
            .Do(x => throw new ComponentShutdownException());

        var config = new { Port = 5432, MaxConnections = 100 };
        var server = new DatabaseServer(new List<IServerComponent> { components });
        server.Start(config);

        // Act
        Action act = () => server.Stop();

        // Assert
        components.Received(1).Stop();
        act.Should().Throw<ComponentShutdownException>();
    }
}
