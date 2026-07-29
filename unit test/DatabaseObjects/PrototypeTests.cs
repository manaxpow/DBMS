using FluentAssertions;
using System;
using Xunit;
using System.Linq;

public class PrototypeTests
{
    [Fact]
    public void Clone_Schema_ShouldReturnDeepCopy()
    {
        // Arrange
        var original = new Schema("TestSchema");

        // Act
        var clone = (Schema)original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Name.Should().Be("TestSchema_Clone");
        clone.Objects.Should().NotBeSameAs(original.Objects);
    }

    [Fact]
    public void Clone_Table_ShouldReturnDeepCopy()
    {
        // Arrange
        var original = new Table("TestTable");
        original.AddColumn(new Column("Id", typeof(int), false));

        // Act
        var clone = (Table)original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Name.Should().Be("TestTable_Clone");
        clone.Columns.Should().NotBeSameAs(original.Columns);
        clone.Columns.Should().HaveCount(original.Columns.Count);

        // Deep copy check for Columns
        if (original.Columns.Count > 0)
        {
            clone.Columns[0].Should().NotBeSameAs(original.Columns[0]);
            clone.Columns[0].Name.Should().Be(original.Columns[0].Name);
        }
    }

    [Fact]
    public void Clone_View_ShouldReturnDeepCopy()
    {
        // Arrange
        var original = new View("TestView", "SELECT * FROM Table");

        // Act
        var clone = (View)original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Name.Should().Be("TestView_Clone");
        clone.Query.Should().Be(original.Query);
    }

    [Fact]
    public void Clone_StoredProcedure_ShouldReturnDeepCopy()
    {
        // Arrange
        var original = new StoredProcedure("TestSp", "BEGIN END;");

        // Act
        var clone = (StoredProcedure)original.Clone();

        // Assert
        clone.Should().NotBeSameAs(original);
        clone.Name.Should().Be("TestSp_Clone");
        clone.Body.Should().NotBeSameAs(original.Body);
    }
}
