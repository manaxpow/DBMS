using System;
using Xunit;

public class DatabaseTests
{
    [Fact]
    public void Open_WhenDatabaseIsClosed_ShouldOpenDatabase()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Close_WhenDatabaseIsOpen_ShouldCloseDatabase()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AddSchema_WhenNameAlreadyExists_ShouldThrow()
    {
        throw new NotImplementedException();
    }


    [Fact]
    public void Open_WhenDatabaseIsAlreadyOpen_ShouldRemainOpen()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Open_WhenStorageInitializationFails_ShouldRemainClosed()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void Close_WhenDatabaseIsAlreadyClosed_ShouldRemainClosed()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AddSchema_WhenSchemaIsValid_ShouldRegisterSchema()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropSchema_WhenSchemaExists_ShouldRemoveSchema()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void DropSchema_WhenSchemaDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AlterSchema_WhenSchemaExists_ShouldUpdateSchema()
    {
        throw new NotImplementedException();
    }

    [Fact]
    public void AlterSchema_WhenSchemaDoesNotExist_ShouldThrow()
    {
        throw new NotImplementedException();
    }
}
