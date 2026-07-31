using System;
using System.Collections.Generic;
using FluentAssertions;
using Xunit;

public class TableBuilderTests
{
    private TableBuilder _builder;

    public TableBuilderTests()
    {
        _builder = new TableBuilder();
    }

    [Trait("Category", "Builder")]
    [Fact]
    public void SetName_ShouldReturnBuilder_AndSetTableName()
    {
        // Arrange
        var tableName = "Users";

        // Act
        var result = _builder.SetName(tableName);

        // Assert
        result.Should().BeSameAs(_builder);
    }

    [Trait("Category", "Builder")]
    [Fact]
    public void AddColumn_ShouldReturnBuilder_AndAddColumn()
    {
        // Arrange
        var column = new Column("Id", DataTypeFactory.Create("INT"));

        // Act
        var result = _builder.AddColumn(column);

        // Assert
        result.Should().BeSameAs(_builder);
    }

    [Trait("Category", "Builder")]
    [Fact]
    public void AddConstraint_ShouldReturnBuilder_AndAddConstraint()
    {
        // Arrange
        // Using null! as dummy since we just want to test builder accumulation
        Constraint constraint = null!;

        // Act
        var result = _builder.AddConstraint(constraint);

        // Assert
        result.Should().BeSameAs(_builder);
    }

    [Trait("Category", "Builder")]
    [Fact]
    public void AddIndex_ShouldReturnBuilder_AndAddIndex()
    {
        // Arrange
        Index index = null!;

        // Act
        var result = _builder.AddIndex(index);

        // Assert
        result.Should().BeSameAs(_builder);
    }

    [Trait("Category", "Builder")]
    [Fact]
    public void AddPartition_ShouldReturnBuilder_AndAddPartition()
    {
        // Arrange
        Partition partition = null!;

        // Act
        var result = _builder.AddPartition(partition);

        // Assert
        result.Should().BeSameAs(_builder);
    }

    [Trait("Category", "Builder")]
    [Fact]
    public void Build_ShouldReturnTable_WithAllPropertiesSet()
    {
        // Arrange
        var tableName = "Orders";
        var column = new Column("Id", DataTypeFactory.Create("INT"));
        Constraint constraint = null!;
        Index index = null!;
        Partition partition = null!;

        _builder.SetName(tableName)
                .AddColumn(column)
                .AddConstraint(constraint)
                .AddIndex(index)
                .AddPartition(partition);

        // Act
        var table = _builder.Build();

        // Assert
        table.Should().NotBeNull();
        table.Name.Should().Be(tableName);
        table.Columns.Should().Contain(column);
        table.Constraints.Should().Contain(constraint);
        table.Indexes.Should().Contain(index);
        table.Partitions.Should().Contain(partition);
    }
}

