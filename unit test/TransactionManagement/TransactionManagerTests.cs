using System;
using FluentAssertions;
using Xunit;

public class TransactionManagerTests
{
    private TransactionManager _transactionManager = new TransactionManager();
    [Trait("Category", "Important")]
    [Fact]
    public void BeginTransaction_ShouldReturnActiveTransaction()
    {
        // Arrange

        // Act
        var transaction = _transactionManager.BeginTransaction();

        // Assert
        transaction.State.Should().Be(TransactionState.Active);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Commit_WhenTransactionExists_ShouldCommitTransaction()
    {
        // Arrange
        var transaction = _transactionManager.BeginTransaction();

        // Act
        _transactionManager.Commit(transaction);

        // Assert
        transaction.State.Should().Be(TransactionState.Committed);
    }

    [Fact]
    public void Commit_WhenTransactionDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }


    [Trait("Category", "Important")]
    [Fact]
    public void BeginTransaction_ShouldAssignUniqueTransactionId()
    {
        // Arrange

        // Act
        var transaction1 = _transactionManager.BeginTransaction();
        var transaction2 = _transactionManager.BeginTransaction();

        // Assert
        transaction1.Id.Should().NotBe(transaction2.Id);
    }

    [Fact]
    public void GetTransaction_WhenTransactionExists_ShouldReturnTransaction()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void GetTransaction_WhenTransactionDoesNotExist_ShouldReturnNull()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Rollback_WhenTransactionExists_ShouldAbortTransaction()
    {
        // Arrange
        var transaction = _transactionManager.BeginTransaction();

        // Act
        _transactionManager.Rollback(transaction);

        // Assert
        transaction.State.Should().Be(TransactionState.Aborted);
    }

    [Fact]
    public void Rollback_WhenTransactionDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Complete_WhenTransactionFinishes_ShouldRemoveFromActiveTransactions()
    {
        // Arrange
        var transaction = _transactionManager.BeginTransaction();

        // Act
        _transactionManager.Complete(transaction);

        // Assert
        _transactionManager.Contains(transaction).Should().BeFalse();
    }
}

