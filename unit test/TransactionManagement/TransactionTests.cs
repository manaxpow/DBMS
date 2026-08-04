using FluentAssertions;

public class TransactionTests
{
    private Transaction _transaction;
    public TransactionTests()
    {
        _transaction = new Transaction(1);
    }
    [Trait("Category", "Important")]
    [Fact]
    public void Begin_WhenTransactionIsNew_ShouldBecomeActive()
    {
        // Act
        _transaction.Begin();

        // Assert
        _transaction.State.Should().Be(TransactionState.Active);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Commit_WhenTransactionIsActive_ShouldCommit()
    {
        // Act
        _transaction.Commit();

        // Assert
        _transaction.State.Should().Be(TransactionState.Committed);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Rollback_WhenTransactionIsActive_ShouldRollback()
    {
        // Act
        _transaction.Rollback();

        // Assert
        _transaction.State.Should().Be(TransactionState.Aborted);
    }


    [Fact]
    public void Begin_WhenTransactionIsAlreadyActive_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Commit_WhenTransactionIsNotActive_ShouldThrow()
    {
        // Arrange
        _transaction.State = TransactionState.New;
        // Act
        Action act = () => _transaction.Commit();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Rollback_WhenTransactionAlreadyCommitted_ShouldThrow()
    {
        // Arrange
        _transaction.State = TransactionState.Committed;

        // Act
        Action act = () => _transaction.Rollback();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Rollback_WhenTransactionAlreadyRolledBack_ShouldRemainRolledBack()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void MarkFailed_WhenTransactionIsActive_ShouldEnterFailedState()
    {
        // Arrange
        _transaction.State = TransactionState.Active;

        // Act
        _transaction.MarkFailed();

        // Assert
        _transaction.State.Should().Be(TransactionState.Failed);
    }
}
