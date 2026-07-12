# Query Processor

```text
Query Processor
├── SQL Parser
│   ├── Lexer
│   ├── Syntax Parser
│   ├── AST Builder
│   └── Syntax Validation
│
├── Semantic Analyzer
│   ├── Object Resolution
│   ├── Type Checking
│   ├── Name Resolution
│   ├── Permission Validation
│   └── Semantic Validation
│
├── Logical Planner
│   ├── Logical Plan Generation
│   ├── Relational Algebra Tree
│   ├── Operator Selection
│   └── Query Rewrite
│
├── Query Optimizer
│   ├── Cost Estimation
│   ├── Join Optimization
│   ├── Index Selection
│   ├── Predicate Pushdown
│   ├── Projection Pruning
│   ├── Statistics
│   └── Physical Plan Selection
│
└── Query Executor
    ├── Operator Execution
    ├── Transaction Coordination
    ├── Storage Access
    ├── Result Generation
    └── Result Streaming
```
