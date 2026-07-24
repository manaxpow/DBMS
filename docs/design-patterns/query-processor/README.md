# Query Processor Design Patterns

This document tracks the design patterns used across the Query Processor module and their current implementation status.

| Priority  | Status | Design Pattern              | Feature                     | Reason / Context                                                                                                                                                                             |
| :-------: | :----: | :-------------------------- | :-------------------------- | :------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- |
|  🔴 High  | `[x]`  | **Interpreter**             | SQL / AST Evaluation        | Represents SQL grammar as AST expression nodes such as SelectNode, WhereNode, and BinaryExpression, allowing the parsed query structure to be interpreted or translated into a logical plan. |
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

---

## 2.2. Visitor (AST Processing)

The **Visitor** pattern is used in the `Expression` class hierarchy to decouple operations from the AST nodes they operate on.

- Define an `Accept` method on each expression node that takes an `IExpressionVisitor`.
- Implement specific visitors (like `SemanticAnalysisVisitor` or `LogicalPlanVisitor`) that encapsulate the logic for processing the entire expression tree.

#### Structure Diagram

```mermaid
classDiagram
    direction TB

    class Visitor {
        <<interface>>
        +VisitElementA(ElementA)
        +VisitElementB(ElementB)
    }

    class ConcreteVisitor1 {
        +VisitElementA(ElementA)
        +VisitElementB(ElementB)
    }

    class Element {
        <<interface>>
        +Accept(Visitor)
    }

    class ConcreteElementA {
        +Accept(Visitor)
    }

    class ConcreteElementB {
        +Accept(Visitor)
    }

    Visitor <|.. ConcreteVisitor1
    Element <|.. ConcreteElementA
    Element <|.. ConcreteElementB
    ConcreteElementA ..> Visitor : calls VisitElementA(this)
    ConcreteElementB ..> Visitor : calls VisitElementB(this)
```

#### Example code

```csharp
// Abstract Element
public interface Expression
{
    T Accept<T>(IExpressionVisitor<T> visitor);
}

// Concrete Element
public class ColumnExpression : Expression
{
    public string ColumnName { get; set; }

    public T Accept<T>(IExpressionVisitor<T> visitor)
    {
        return visitor.Visit(this);
    }
}

// Visitor Interface
public interface IExpressionVisitor<T>
{
    T Visit(ColumnExpression expression);
    // other visit methods...
}

// Concrete Visitor
public class LogicalPlanVisitor : IExpressionVisitor<LogicalNode>
{
    public LogicalNode Visit(ColumnExpression expression)
    {
        // Generate logical node for column
        return new LogicalNode();
    }
}
```

#### Class diagram

```mermaid
classDiagram
    direction LR

    class Expression {
        <<interface>>
        +Accept~T~(IExpressionVisitor~T~ visitor) T
    }

    class ColumnExpression {
        +string ColumnName
        +Accept(visitor) T
    }

    class LiteralExpression {
        +object Value
        +Accept(visitor) T
    }

    class BinaryExpression {
        +Expression Left
        +BinaryOperator Operator
        +Expression Right
        +Accept(visitor) T
    }

    class WhereExpression {
        +Expression Condition
        +Accept(visitor) T
    }

    class SelectExpression {
        +List~Expression~ Columns
        +Expression Source
        +WhereExpression Where
        +Accept(visitor) T
    }

    class IExpressionVisitor~T~ {
        <<interface>>
        +Visit(ColumnExpression expression) T
        +Visit(LiteralExpression expression) T
        +Visit(BinaryExpression expression) T
        +Visit(WhereExpression expression) T
        +Visit(SelectExpression expression) T
    }

    class SemanticAnalysisVisitor {
        +Visit(ColumnExpression expression)
        +Visit(LiteralExpression expression)
        +Visit(BinaryExpression expression)
        +Visit(WhereExpression expression)
        +Visit(SelectExpression expression)
    }

    class LogicalPlanVisitor {
        +Visit(ColumnExpression expression) LogicalNode
        +Visit(LiteralExpression expression) LogicalNode
        +Visit(BinaryExpression expression) LogicalNode
        +Visit(WhereExpression expression) LogicalNode
        +Visit(SelectExpression expression) LogicalNode
    }

    Expression <|.. ColumnExpression
    Expression <|.. LiteralExpression
    Expression <|.. BinaryExpression
    Expression <|.. WhereExpression
    Expression <|.. SelectExpression

    BinaryExpression o-- Expression
    WhereExpression o-- Expression
    SelectExpression o-- Expression

    IExpressionVisitor~T~ <|.. SemanticAnalysisVisitor
    IExpressionVisitor~T~ <|.. LogicalPlanVisitor

    Expression --> IExpressionVisitor~T~ : Accept(visitor)
```

#### Sequence Diagram: AST Processing Workflow

```mermaid
sequenceDiagram
    autonumber

    participant Client
    participant Visitor as LogicalPlanVisitor
    participant Expr as BinaryExpression
    participant ColExpr as ColumnExpression

    Client->>Expr: Accept(Visitor)
    activate Expr
    
    Expr->>Visitor: Visit(BinaryExpression)
    activate Visitor

    Note over Visitor: Process left child
    Visitor->>ColExpr: Accept(Visitor)
    activate ColExpr
    ColExpr->>Visitor: Visit(ColumnExpression)
    activate Visitor
    Visitor-->>ColExpr: LogicalNode
    deactivate Visitor
    ColExpr-->>Visitor: LogicalNode
    deactivate ColExpr

    Note over Visitor: Combine results
    Visitor-->>Expr: LogicalNode
    deactivate Visitor
    
    Expr-->>Client: LogicalNode
    deactivate Expr
```
