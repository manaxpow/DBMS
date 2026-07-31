using System;
using FluentAssertions;
using Xunit;
using DBMS.Exceptions;
using System.Collections.Generic;

public class ReferentialActionTests
{
    private (Schema, Table, Table) CreateSchema()
    {
        var schema = new Schema("TestSchema");

        var parentTable = new Table("ParentTable");
        parentTable.AddColumn(new Column("Id", DataTypeFactory.Create("INT")));
        schema.AddTable(parentTable);

        var childTable = new Table("ChildTable");
        childTable.AddColumn(new Column("ParentId", DataTypeFactory.Create("INT")));
        schema.AddTable(childTable);

        return (schema, parentTable, childTable);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ForeignKeyConstraint_OnParentRowDeleted_WhenRestrictStrategy_ShouldThrowReferentialIntegrityException()
    {
        // Arrange
        var (_, parentTable, childTable) = CreateSchema();
        var parentRow = new Row(parentTable, new List<object> { 1 });
        var childRow = new Row(childTable, new List<object> { 1 });

        var strategy = new RestrictAction();
        var fk = new ForeignKeyConstraint("FK", "ParentId", "ParentTable", "Id", strategy, strategy);

        childTable.AddConstraint(fk);
        parentTable.InsertRow(parentRow);
        childTable.InsertRow(childRow);

        // Act
        Action act = () => fk.OnParentRowDeleted(parentRow, childTable);

        // Assert
        childTable.Rows.Should().Contain(childRow);
        act.Should().Throw<ReferentialIntegrityException>();
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ForeignKeyConstraint_OnParentRowDeleted_WhenCascadeStrategy_ShouldDelegateToStrategy()
    {
        // Arrange
        var (_, parentTable, childTable) = CreateSchema();
        var parentRow = new Row(parentTable, new List<object> { 1 });
        var childRow = new Row(childTable, new List<object> { 1 });

        var strategy = new CascadeAction();
        var fk = new ForeignKeyConstraint("FK", "ParentId", "ParentTable", "Id", strategy, strategy);

        childTable.AddConstraint(fk);
        parentTable.InsertRow(parentRow);
        childTable.InsertRow(childRow);

        // Act
        fk.OnParentRowDeleted(parentRow, childTable);

        // Assert
        childTable.Rows.Should().NotContain(childRow);
    }

    [Trait("Category", "Important")]
    [Fact]
    public void ForeignKeyConstraint_OnParentRowDeleted_WhenSetNullStrategy_ShouldDelegateToStrategy()
    {
        // Arrange
        var (_, parentTable, childTable) = CreateSchema();
        var parentRow = new Row(parentTable, new List<object> { 1 });
        var childRow = new Row(childTable, new List<object> { 1 });

        var strategy = new SetNullAction();
        var fk = new ForeignKeyConstraint("FK", "ParentId", "ParentTable", "Id", strategy, strategy);

        childTable.AddConstraint(fk);
        parentTable.InsertRow(parentRow);
        childTable.InsertRow(childRow);

        // Act
        fk.OnParentRowDeleted(parentRow, childTable);

        // Assert
        childTable.Rows.Should().Contain(childRow);
        childRow.GetValue("ParentId").Should().BeNull();
    }
}

