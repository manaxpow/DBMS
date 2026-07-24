# Query Processor Sequence Diagrams

This document contains the sequence diagrams for the Query Processor components and their associated unit tests.

## 1. Lexer

```mermaid
sequenceDiagram
    participant Test as LexerTests
    participant Lexer as Lexer

    Test->>Lexer: Tokenize("SELECT * FROM table")
    alt Valid SQL
        Lexer-->>Test: List<Token>
    else Invalid Token
        Lexer-->>Test: throws Exception
    end
    Test->>Test: Assert Output / Exception
```

## 2. Parser

```mermaid
sequenceDiagram
    participant Test as SQLParserTests
    participant Parser as SQLParser

    Test->>Parser: Parse(tokens)
    alt Valid Syntax
        Parser-->>Test: AST (Abstract Syntax Tree)
    else Invalid Syntax / Empty Input
        Parser-->>Test: throws Exception
    end
    Test->>Test: Assert Output / Exception
```

## 3. Semantic Analyzer

```mermaid
sequenceDiagram
    participant Test as SemanticAnalyzerTests
    participant SA as SemanticAnalyzer

    Test->>SA: Analyze(AST)
    alt Valid Semantic
        SA-->>Test: LogicalPlan
    else Invalid Semantic (e.g., Table Not Found)
        SA-->>Test: throws Exception
    end
    Test->>Test: Assert Output / Exception
```

## 4. Optimizer

```mermaid
sequenceDiagram
    participant Test as QueryOptimizerTests
    participant Optimizer as QueryOptimizer

    Test->>Optimizer: Optimize(LogicalPlan)
    alt Valid Logical Plan
        Optimizer-->>Test: PhysicalPlan
    else Invalid Plan
        Optimizer-->>Test: throws Exception
    end
    Test->>Test: Assert Output / Exception
```

## 5. Executor

```mermaid
sequenceDiagram
    participant Test as QueryExecutorTests
    participant Executor as QueryExecutor

    Test->>Executor: Execute(PhysicalPlan)
    alt Valid Plan
        Executor-->>Test: ResultSet (Rows)
    else Storage/Transaction Failure
        Executor-->>Test: throws Exception
    end
    Test->>Test: Assert Output / Exception
```

## 6. Main Query Execution Flow

This demonstrates the end-to-end orchestration by the `QueryProcessor` Facade during integration testing or execution.

```mermaid
sequenceDiagram
    participant Test as QueryProcessorTests
    participant QP as QueryProcessor
    participant Lexer as Lexer
    participant Parser as SQLParser
    participant SA as SemanticAnalyzer
    participant Optimizer as QueryOptimizer
    participant Exec as QueryExecutor

    Test->>QP: ExecuteQuery(sqlString)
    QP->>Lexer: Tokenize(sqlString)
    Lexer-->>QP: List<Token>
    
    QP->>Parser: Parse(tokens)
    Parser-->>QP: AST
    
    QP->>SA: Analyze(AST)
    SA-->>QP: LogicalPlan
    
    QP->>Optimizer: Optimize(LogicalPlan)
    Optimizer-->>QP: PhysicalPlan
    
    QP->>Exec: Execute(PhysicalPlan)
    Exec-->>QP: ResultSet
    
    QP-->>Test: ResultSet
    Test->>Test: Assert result / state
```

## 7. Cost-Based Optimization Strategy

```mermaid
sequenceDiagram
    autonumber

    participant Test as CostBasedOptimizationStrategyTests
    participant Strategy as CostBasedOptimizationStrategy
    participant Plan as PhysicalPlan

    Note over Test, Plan: Arrange
    Test->>Strategy: new CostBasedOptimizationStrategy()
    Test->>Plan: Create Candidate Plans (e.g., TableScan, IndexScan)

    Note over Test, Strategy: Act
    Test->>Strategy: SelectBestPlan(candidates)
    activate Strategy
    Strategy->>Strategy: Find plan with minimum cost
    Strategy-->>Test: PhysicalPlan (bestPlan)
    deactivate Strategy

    Note over Test: Assert
    Test->>Test: result.Should().Be(expectedPlan)
    Test->>Test: result.Cost.Should().Be(expectedCost)
```

## 8. Rule-Based Optimization Strategy

```mermaid
sequenceDiagram
    autonumber

    participant Test as RuleBasedOptimizationStrategyTests
    participant Strategy as RuleBasedOptimizationStrategy
    participant Plan as LogicalPlan

    Note over Test, Plan: Arrange
    Test->>Strategy: new RuleBasedOptimizationStrategy()
    Test->>Plan: new LogicalPlan()
    Test->>Plan: Add(LogicalNode)

    Note over Test, Strategy: Act
    Test->>Strategy: ApplyPredicatePushdown(plan)
    activate Strategy
    Strategy-->>Test: LogicalPlan (result)
    deactivate Strategy

    Note over Test: Assert
    Test->>Test: result.Should().NotBeNull()
    Test->>Test: result.Nodes.Should().ContainSingle(...)
```
