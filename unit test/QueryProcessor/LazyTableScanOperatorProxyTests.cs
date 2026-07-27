using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

public class LazyTableScanOperatorProxyTests
{
    private readonly CatalogManager _catalog;
    private readonly Table _table;

    public LazyTableScanOperatorProxyTests()
    {
        _catalog = new CatalogManager();
        _table = new Table("TestTable");
        _catalog.Register(_table);
    }

    [Fact]
    public void Open_WhenCalled_ShouldInitializeRealOperatorAndOpen()
    {
        // Arrange
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);

        // Act
        proxy.Open();

        // Assert
        var hasNext = proxy.Next();
        hasNext.Should().BeFalse();
    }

    [Fact]
    public void Next_WhenRealOperatorNotInitialized_ShouldThrow()
    {
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);

        // Act
        Action act = () => proxy.Next();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void GetCurrent_WhenRealOperatorNotInitialized_ShouldThrow()
    {
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);

        // Act
        Action act = () => proxy.GetCurrent();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Next_WhenRealOperatorInitialized_ShouldDelegate()
    {
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);
        proxy.Open();

        // Act
        var result = proxy.Next();

        // Assert
        result.Should().BeFalse();
    }

    [Fact]
    public void GetCurrent_WhenRealOperatorInitialized_ShouldDelegate()
    {
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);
        proxy.Open();

        // Act
        Action act = () => proxy.GetCurrent();

        // Assert
        act.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void Close_WhenRealOperatorInitialized_ShouldDelegate()
    {
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);
        proxy.Open();

        // Act
        proxy.Close();

        // Assert
        var result = proxy.Next();
        result.Should().BeFalse();
    }

    [Fact]
    public void Close_WhenRealOperatorNotInitialized_ShouldNotThrow()
    {
        var proxy = new LazyTableScanOperatorProxy("TestTable", _catalog);

        // Act
        Action act = () => proxy.Close();

        // Assert
        act.Should().NotThrow();
    }
}
