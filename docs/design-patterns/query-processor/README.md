# Query Processor Design Patterns

This document tracks the design patterns used across the Query Processor module and their current implementation status.

| Priority  | Status | Design Pattern              | Feature                     | Reason / Context                                                                                                                                                                             |
| :-------: | :----: | :-------------------------- | :-------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|  🔴 High  | `[ ]`  | **Interpreter**             | SQL / AST Evaluation        | Represents SQL grammar as AST expression nodes such as SelectNode, WhereNode, and BinaryExpression, allowing the parsed query structure to be interpreted or translated into a logical plan. |
|  🔴 High  | `[ ]`  | **Visitor**                 | AST Processing              | Allows validation, semantic analysis, logical-plan generation, or expression evaluation to operate on different AST node types without putting every operation inside the AST classes.       |
|  🔴 High  | `[ ]`  | **Strategy**                | Query Optimization          | Allows QueryOptimizer to switch between optimization algorithms such as predicate pushdown, join reordering, index selection, or cost-based optimization.                                    |
|  🔴 High  | `[ ]`  | **Factory Method**          | Physical Operator Creation  | Creates physical operators such as TableScan, IndexScan, HashJoin, NestedLoopJoin, and Sort from logical-plan nodes selected by the optimizer.                                               |
| 🟡 Medium | `[ ]`  | **Composite**               | Query Plan Tree             | Treats leaf operators such as scans and composite operators such as joins, filters, and projections uniformly as plan nodes, naturally representing LogicalPlan and PhysicalPlan as trees.   |
| 🟡 Medium | `[ ]`  | **Iterator**                | Query Result Execution      | Lets physical operators expose rows one at a time through a common Next()/MoveNext() interface, enabling pipelined query execution without materializing every intermediate result.          |
| 🟡 Medium | `[ ]`  | **Command**                 | SQL Statement Execution     | Encapsulates parsed statements such as SELECT, INSERT, UPDATE, and DELETE as executable command objects and decouples statement dispatch from QueryExecutor.                                 |
|  🟢 Low   | `[ ]`  | **Chain of Responsibility** | Optimization Pipeline       | Passes a query plan through independent optimization rules such as constant folding, predicate pushdown, projection pruning, and join optimization. Each rule transforms or passes the plan onward. |
|  🟢 Low   | `[ ]`  | **Builder**                 | Query Plan Construction     | Builds complex LogicalPlan or PhysicalPlan objects step by step from AST nodes, especially useful when plans contain scans, filters, joins, projections, grouping, sorting, and limits.      |
|  🟢 Low   | `[ ]`  | **Template Method**         | Physical Operator Execution | Defines a common execution lifecycle such as Open() → Next() → Close() while concrete operators implement operator-specific behavior.                                                        |

---

## 2. Pattern Implementation Details

### 2.1. Interpreter (SQL / AST Evaluation)

The **Interpreter** pattern is used in the `Expression` class hierarchy to evaluate parsed SQL structures.

- Define a common `Interpret` method for all expression nodes.
- Delegate interpretation to subclass implementations for terminal and non-terminal operations.

#### Structure Diagram

```mermaid
classDiagram
    direction TB

    class Context

    class AbstractExpression {
        <<abstract>>
        +Interpret(context)* void
    }

    class TerminalExpression {
        +Interpret(context) void
    }

    class NonterminalExpression {
        +Interpret(context) void
    }

    Client --> Context
    Client --> AbstractExpression
    AbstractExpression <|-- TerminalExpression
    AbstractExpression <|-- NonterminalExpression
    NonterminalExpression o-- AbstractExpression
```

#### Example code

```csharp
// Abstract class
public abstract class Expression
{
    public abstract LogicalNode Interpret(InterpretationContext context);
}

// Terminal Expression
public class ColumnExpression : TerminalExpression
{
    public string ColumnName { get; set; }

    public override LogicalNode Interpret(InterpretationContext context)
    {
        return context.ResolveColumn(ColumnName);
    }
}

// Non-Terminal Expression
public class BinaryExpression : NonTerminalExpression
{
    public Expression Left { get; set; }
    public string Operator { get; set; }
    public Expression Right { get; set; }

    public override LogicalNode Interpret(InterpretationContext context)
    {
        var leftNode = Left.Interpret(context);
        var rightNode = Right.Interpret(context);
        
        // Return combined LogicalNode
        return new LogicalNode();
    }
}
```

#### Class diagram

```mermaid
classDiagram
    direction LR

    class QueryProcessor {
        <<Client>>
        +Process(Expression expression) LogicalPlan
    }

    class InterpretationContext {
        <<Context>>
        +ResolveTable(string name)
        +ResolveColumn(string name)
    }

    class Expression {
        <<interface>>
        +Interpret(InterpretationContext context) LogicalNode
    }

    class TerminalExpression {
        <<abstract>>
        +Interpret(InterpretationContext context) LogicalNode
    }

    class ColumnExpression {
        +string ColumnName
        +Interpret(InterpretationContext context) LogicalNode
    }

    class LiteralExpression {
        +object Value
        +Interpret(InterpretationContext context) LogicalNode
    }

    class TableExpression {
        +string TableName
        +Interpret(InterpretationContext context) LogicalNode
    }

    class NonTerminalExpression {
        <<abstract>>
        +Interpret(InterpretationContext context) LogicalNode
    }

    class SelectExpression {
        +List~Expression~ Columns
        +Expression Source
        +Expression Where
        +Interpret(InterpretationContext context) LogicalNode
    }

    class WhereExpression {
        +Expression Condition
        +Interpret(InterpretationContext context) LogicalNode
    }

    class BinaryExpression {
        +Expression Left
        +string Operator
        +Expression Right
        +Interpret(InterpretationContext context) LogicalNode
    }

    QueryProcessor --> InterpretationContext : uses
    QueryProcessor --> Expression : calls Interpret()

    Expression <|.. TerminalExpression
    Expression <|.. NonTerminalExpression

    TerminalExpression <|-- ColumnExpression
    TerminalExpression <|-- LiteralExpression
    TerminalExpression <|-- TableExpression

    NonTerminalExpression <|-- SelectExpression
    NonTerminalExpression <|-- WhereExpression
    NonTerminalExpression <|-- BinaryExpression

    NonTerminalExpression o-- Expression : contains / interprets
```

#### Sequence Diagram: Expression Interpretation Workflow

```mermaid
sequenceDiagram
    autonumber

    participant Client as QueryProcessor
    participant Context as InterpretationContext
    participant BinaryExpr as BinaryExpression (Non-Terminal)
    participant ColExpr as ColumnExpression (Terminal)

    Client->>BinaryExpr: Interpret(context)
    activate BinaryExpr

    Note over BinaryExpr: Interpret left child
    BinaryExpr->>ColExpr: Interpret(context)
    activate ColExpr
    
    Note over ColExpr: Resolve column from context
    ColExpr->>Context: ResolveColumn(ColumnName)
    Context-->>ColExpr: LogicalNode
    ColExpr-->>BinaryExpr: LogicalNode
    deactivate ColExpr

    Note over BinaryExpr: Combine results and return
    BinaryExpr-->>Client: LogicalNode
    deactivate BinaryExpr
```
