using System;
using FluentAssertions;
using Xunit;

public class LockManagerTests
{
    private readonly LockManager _lockManager;

    public LockManagerTests()
    {
        _lockManager = new LockManager();
    }
    [Fact]
    public void Acquire_WhenLocksAreCompatible_ShouldGrantLock()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Acquire_WhenLocksConflict_ShouldRejectOrWait()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Shared);

        var tx2 = new Transaction(2);
        // Act
        _lockManager.Acquire(tx2, 1, LockMode.Exclusive);

        // Assert
        tx2.State.Should().Be(TransactionState.Aborted);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Release_WhenLockExists_ShouldRemoveLock()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Shared);

        // Act
        _lockManager.Release(tx1, 1);

        // Assert
        _lockManager.Contains(tx1, 1).Should().BeFalse();
    }


    [Fact]
    public void Acquire_WhenSharedLockAlreadyExists_ShouldGrantSharedLock()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Acquire_WhenExclusiveLockExists_ShouldRejectOtherTransactions()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Exclusive);

        // Act
        var tx2 = new Transaction(2);
        _lockManager.Acquire(tx2, 1, LockMode.Shared);

        // Assert
        tx2.State.Should().Be(TransactionState.Aborted);
    }

    [Fact]
    public void Acquire_WhenTransactionAlreadyOwnsLock_ShouldReuseLock()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Upgrade_WhenTransactionIsSoleReader_ShouldGrantExclusiveLock()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Shared);

        // Act
        var upgraded = _lockManager.Upgrade(tx1, 1);

        // Assert
        upgraded.Should().BeTrue();
        _lockManager.Contains(tx1, 1).Should().BeTrue();
        _lockManager.HasLock(tx1, LockMode.Exclusive).Should().BeTrue();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Upgrade_WhenOtherReadersExist_ShouldRejectOrWait()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Shared);

        var tx2 = new Transaction(2);
        _lockManager.Acquire(tx2, 1, LockMode.Shared);
        // Act
        var upgraded = _lockManager.Upgrade(tx2, 1);

        // Assert
        upgraded.Should().BeFalse();
        _lockManager.Contains(tx2, 1).Should().BeTrue();
        _lockManager.HasLock(tx2, LockMode.Shared).Should().BeTrue();
    }

    [Fact]
    public void Release_WhenLockDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ReleaseAll_WhenTransactionHasLocks_ShouldRemoveAllLocks()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Shared);

        // Act
        _lockManager.ReleaseAll(tx1);

        // Assert
        _lockManager.Contains(tx1, 1).Should().BeFalse();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void DetectDeadlock_WhenCycleExists_ShouldAbortVictimTransaction()
    {
        // Arrange
        var tx1 = new Transaction(1);
        _lockManager.Acquire(tx1, 1, LockMode.Shared);

        var tx2 = new Transaction(2);
        _lockManager.Acquire(tx2, 2, LockMode.Shared);

        var tx3 = new Transaction(3);
        _lockManager.Acquire(tx3, 3, LockMode.Shared);

        // Act
        _lockManager.DetectDeadlock();

        // Assert
        tx2.State.Should().Be(TransactionState.Aborted);
    }

    [Fact]
    public void DetectDeadlock_WhenNoCycleExists_ShouldNotAbortTransaction()
    {
        throw new NotImplementedException();
    }
}

