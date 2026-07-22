using System;
using System.Reflection;
using DBMS.Exceptions;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class DatabaseStateTests
{
    private Database _database;

    public DatabaseStateTests()
    {
        _database = new Database();
    }

    private IDatabaseState GetCurrentState(Database db)
    {
        var field = typeof(Database).GetField("_state", BindingFlags.NonPublic | BindingFlags.Instance);
        return (IDatabaseState)field.GetValue(db);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ChangeState_WhenStateIsValid_ShouldUpdateCurrentState()
    {
        // Arrange
        var newState = new OnlineState(_database);

        // Act
        _database.ChangeState(newState);

        // Assert
        GetCurrentState(_database).Should().BeOfType<OnlineState>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void OfflineState_Open_ShouldTransitionToOnlineState()
    {
        // Arrange
        var offlineState = new OfflineState(_database);
        _database.ChangeState(offlineState);

        // Act
        offlineState.Open();

        // Assert
        GetCurrentState(_database).Should().BeOfType<OnlineState>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void OnlineState_SetReadOnly_ShouldTransitionToReadOnlyState()
    {
        // Arrange
        var onlineState = new OnlineState(_database);
        _database.ChangeState(onlineState);

        // Act
        onlineState.SetReadOnly();

        // Assert
        GetCurrentState(_database).Should().BeOfType<ReadOnlyState>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void OnlineState_Drop_ShouldTransitionToDroppedState()
    {
        // Arrange
        var onlineState = new OnlineState(_database);
        _database.ChangeState(onlineState);

        // Act
        onlineState.Drop();

        // Assert
        GetCurrentState(_database).Should().BeOfType<DroppedState>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ReadOnlyState_Open_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var readOnlyState = new ReadOnlyState(_database);
        _database.ChangeState(readOnlyState);

        // Act
        Action act = () => readOnlyState.Open();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }
}
