using System;
using Xunit;

public class TransactionTests
{
    [Trait("Category", "Important")]
    [Fact]
    public void Begin_WhenTransactionIsNew_ShouldBecomeActive()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Commit_WhenTransactionIsActive_ShouldCommit()
    {
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Rollback_WhenTransactionIsActive_ShouldRollback()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void Rollback_WhenTransactionAlreadyCommitted_ShouldThrow()
    {
        throw new NotImplementedException();
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
        throw new NotImplementedException();
    }
}

