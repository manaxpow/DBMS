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
