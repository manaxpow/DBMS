using System;
using FluentAssertions;
using Xunit;

public class DataTypeFactoryTests
{
    [Fact]
    public void GetDataType_WhenValidType_ShouldReturnSharedInstance()
    {
        // Arrange
        var factory = new DataTypeFactory();

        // Act
        var intType1 = factory.GetDataType("INT");
        var intType2 = factory.GetDataType("int");
        var varcharType1 = factory.GetDataType("VARCHAR");
        var varcharType2 = factory.GetDataType("VARCHAR");

        // Assert
        intType1.Should().NotBeNull();
        intType1.Should().BeOfType<IntegerType>();
        intType1.Should().BeSameAs(intType2, "Flyweight pattern should return the shared instance.");

        varcharType1.Should().NotBeNull();
        varcharType1.Should().BeOfType<VarcharType>();
        varcharType1.Should().BeSameAs(varcharType2, "Flyweight pattern should return the shared instance.");
    }

    [Fact]
    public void GetDataType_WhenInvalidType_ShouldThrow()
    {
        // Arrange
        var factory = new DataTypeFactory();

        // Act
        Action act = () => factory.GetDataType("INVALID_TYPE");

        // Assert
        act.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Validate_ShouldDelegateToConcreteFlyweight()
    {
        // Arrange
        var factory = new DataTypeFactory();
        var intType = factory.GetDataType("INT");
        var column = new Column("Age", intType, isNullable: false);

        // Act
        Action validAct = () => intType.Validate(column, 25);
        Action invalidNullAct = () => intType.Validate(column, null);
        Action invalidTypeAct = () => intType.Validate(column, "not an int");

        // Assert
        validAct.Should().NotThrow();
        invalidNullAct.Should().Throw<InvalidOperationException>().WithMessage("Age cannot be null.");
        invalidTypeAct.Should().Throw<InvalidOperationException>().WithMessage("Age must contain an integer.");
    }
}
