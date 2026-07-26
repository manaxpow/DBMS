using System;
using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using Xunit;

namespace DBMS.UnitTests.QueryProcessor
{
    public class InterpreterTests
    {
        [Trait("Category", "Important")]
        [Fact]
        public void ColumnExpression_Interpret_ShouldResolveColumnFromContext()
        {
            // Arrange
            var context = Substitute.For<InterpretationContext>();
            var expectedNode = new LogicalNode();
            context.ResolveColumn("id").Returns(expectedNode);

            var expr = new ColumnExpression { ColumnName = "id" };

            // Act
            var result = expr.Interpret(context);

            // Assert
            result.Should().Be(expectedNode);
            context.Received(1).ResolveColumn("id");
        }

        [Trait("Category", "Important")]
        [Fact]
        public void TableExpression_Interpret_ShouldResolveTableFromContext()
        {
            // Arrange
            var context = Substitute.For<InterpretationContext>();
            var expectedNode = new LogicalNode();
            context.ResolveTable("users").Returns(expectedNode);

            var expr = new TableExpression { TableName = "users" };

            // Act
            var result = expr.Interpret(context);

            // Assert
            result.Should().Be(expectedNode);
            context.Received(1).ResolveTable("users");
        }

        [Trait("Category", "Important")]
        [Fact]
        public void LiteralExpression_Interpret_ShouldReturnLiteralLogicalNode()
        {
            // Arrange
            var context = Substitute.For<InterpretationContext>();
            var expr = new LiteralExpression { Value = 42 };

            // Act
            var result = expr.Interpret(context);

            // Assert
            result.Should().NotBeNull();
        }

        [Trait("Category", "Important")]
        [Fact]
        public void BinaryExpression_Interpret_ShouldInterpretLeftAndRightAndReturnNode()
        {
            // Arrange
            var context = Substitute.For<InterpretationContext>();

            var leftExpr = Substitute.For<Expression>();
            var leftNode = new LogicalNode();
            leftExpr.Interpret(context).Returns(leftNode);

            var rightExpr = Substitute.For<Expression>();
            var rightNode = new LogicalNode();
            rightExpr.Interpret(context).Returns(rightNode);

            var expr = new BinaryExpression
            {
                Left = leftExpr,
                Right = rightExpr,
                Operator = "="
            };

            // Act
            var result = expr.Interpret(context);

            // Assert
            result.Should().NotBeNull();
            leftExpr.Received(1).Interpret(context);
            rightExpr.Received(1).Interpret(context);
        }

        [Trait("Category", "Important")]
        [Fact]
        public void WhereExpression_Interpret_ShouldInterpretConditionAndReturnNode()
        {
            // Arrange
            var context = Substitute.For<InterpretationContext>();
            var conditionExpr = Substitute.For<Expression>();
            var conditionNode = new LogicalNode();
            conditionExpr.Interpret(context).Returns(conditionNode);

            var expr = new WhereExpression { Condition = conditionExpr };

            // Act
            var result = expr.Interpret(context);

            // Assert
            result.Should().NotBeNull();
            conditionExpr.Received(1).Interpret(context);
        }

        [Trait("Category", "Important")]
        [Fact]
        public void SelectExpression_Interpret_ShouldInterpretFromWhereAndColumns()
        {
            // Arrange
            var context = Substitute.For<InterpretationContext>();

            var fromExpr = Substitute.For<Expression>();
            fromExpr.Interpret(context).Returns(new LogicalNode());

            var whereExpr = Substitute.For<Expression>();
            whereExpr.Interpret(context).Returns(new LogicalNode());

            var colExpr = Substitute.For<Expression>();
            colExpr.Interpret(context).Returns(new LogicalNode());

            var expr = new SelectExpression
            {
                From = fromExpr,
                Where = whereExpr,
                Columns = new List<Expression> { colExpr }
            };

            // Act
            var result = expr.Interpret(context);

            // Assert
            result.Should().NotBeNull();
            fromExpr.Received(1).Interpret(context);
            whereExpr.Received(1).Interpret(context);
            colExpr.Received(1).Interpret(context);
        }
    }
}

