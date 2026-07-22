# Database Design Patterns

This document tracks the design patterns used across different modules in the DBMS and their current implementation status.

## 1. Database Objects

|  Priority | Status | Design Pattern      | Feature                 | Reason / Context                                                                                      |
| :-------: | :----: | :------------------ | :---------------------- | :---------------------------------------------------------------------------------------------------- |
|  🔴 High  |  `[x]` | **Template Method** | Constraint              | `Validate()` defines the workflow, while each concrete constraint only implements `Check()`.          |
|  🔴 High  |  `[x]` | **Factory Method**  | Constraint Creation     | Creates `PrimaryKey`, `ForeignKey`, `Unique`, and `CheckConstraint` objects from metadata.            |
|  🔴 High  |  `[x]` | **Strategy**        | Referential Action      | Selects Cascade, Restrict, SetNull, or SetDefault behavior when deleting or updating referenced rows. |
|  🔴 High  |  `[x]` | **Composite**       | Schema Objects          | Schema contains Tables, Views, and Stored Procedures and manages them uniformly as `ISchemaObject`.   |
|  🔴 High  |  `[x]` | **Command**         | DDL Command             | `CreateTable`, `DropTable`, and `AlterTable` operations are encapsulated into command objects.        |
| 🟡 Medium |  `[x]` | **Iterator**        | Schema Object Traversal | Provides sequential access to schema objects without exposing internal collections.                   |
| 🟡 Medium |  `[x]` | **Visitor**         | Schema Operations       | Backup, Export, and Validation can operate on all schema object types.                                |
| 🟡 Medium |  `[ ]` | **Builder**         | Table Definition        | Builds a Table step by step from columns, constraints, indexes, and partitions.                       |
|   🟢 Low  |  `[ ]` | **Prototype**       | Schema Object Cloning   | Clones schema objects for migration, temporary objects, or schema duplication.                        |
|   🟢 Low  |  `[ ]` | **Decorator**       | Constraint Extension    | Adds logging, metrics, or auditing without modifying existing constraints.                            |
|   🟢 Low  |  `[ ]` | **Mediator**        | Dependency Management   | Coordinates interactions among Tables, Views, Procedures, and Foreign Keys.                           |

---

## 2. Database Management

For Database Management patterns, please see [Database Management Patterns](./database-managment/README.md).

_Note: Update the status column to `[x]` when a pattern is implemented in the source code to manage progress._

## 3. Pattern Implementation Details

### 3.1. Template Method (Constraint)

The **Template Method** pattern is used in the `Constraint` class. The base class defines the skeletal workflow for validation in the `Validate()` method (e.g., checking if the constraint is enabled), and defers the specific logic to the `Check()` method which must be implemented by subclasses like `UniqueConstraint` or `PrimaryKeyConstraint`.

```mermaid
classDiagram
    class Client
    class Constraint {
        <<abstract>>
        +bool IsEnabled
        +Validate(row) validationResult
        #Check(row)* validationResult
    }
    class UniqueConstraint {
        #Check(row) validationResult
    }
    class PrimaryKeyConstraint {
        #Check(row) validationResult
    }

    Client --> Constraint
    Constraint <|-- UniqueConstraint
    Constraint <|-- PrimaryKeyConstraint
```

```mermaid
sequenceDiagram
    autonumber

    participant Client
    participant BaseConstraint as Constraint (Base)
    participant ConcreteConstraint as UniqueConstraint (Subclass)

    Client->>BaseConstraint: Validate(row)
    activate BaseConstraint

    Note over BaseConstraint: Common workflow step
    BaseConstraint->>BaseConstraint: Check if IsEnabled

    alt IsEnabled == false
        BaseConstraint-->>Client: true (Skip validation)
    else IsEnabled == true
        Note over BaseConstraint: Defers to subclass
        BaseConstraint->>ConcreteConstraint: Check(row)
        activate ConcreteConstraint

        Note over ConcreteConstraint: Subclass specific logic<br/>(e.g., duplicate check)
        ConcreteConstraint-->>BaseConstraint: validationResult
        deactivate ConcreteConstraint

        BaseConstraint-->>Client: validationResult
    end
    deactivate BaseConstraint
```

### 3.2. Factory Method (Constraint Creation)

The **Factory Method** pattern is used to encapsulate the creation logic of different types of constraints (`PrimaryKeyConstraint`, `ForeignKeyConstraint`, etc.) based on metadata. The abstract `ConstraintCreator` declares the factory method `CreateConstraint()` which is implemented by concrete creator subclasses to instantiate specific constraints.

```mermaid
classDiagram
    direction LR

    class ConstraintCreatorRegistry {
        +GetCreator(metadataType) ConstraintCreator
    }

    %% Constraint Creators
    class ConstraintCreator {
        <<abstract>>
        +CreateConstraint(ConstraintMetadata metadata) Constraint
    }
    class PrimaryKeyConstraintCreator {
        +CreateConstraint(ConstraintMetadata metadata) Constraint
    }
    class ForeignKeyConstraintCreator {
        +CreateConstraint(ConstraintMetadata metadata) Constraint
    }
    class UniqueConstraintCreator {
        +CreateConstraint(ConstraintMetadata metadata) Constraint
    }
    class CheckConstraintCreator {
        +CreateConstraint(ConstraintMetadata metadata) Constraint
    }

    ConstraintCreatorRegistry ..> ConstraintCreator : returns

    ConstraintCreator <|-- PrimaryKeyConstraintCreator
    ConstraintCreator <|-- ForeignKeyConstraintCreator
    ConstraintCreator <|-- UniqueConstraintCreator
    ConstraintCreator <|-- CheckConstraintCreator

    %% Constraints
    class Constraint {
        <<abstract>>
        +string Name
        +bool IsEnabled
    }
    class PrimaryKeyConstraint
    class ForeignKeyConstraint
    class UniqueConstraint
    class CheckConstraint

    Constraint <|-- PrimaryKeyConstraint
    Constraint <|-- ForeignKeyConstraint
    Constraint <|-- UniqueConstraint
    Constraint <|-- CheckConstraint

    %% Factory Relationships
    PrimaryKeyConstraintCreator ..> PrimaryKeyConstraint : creates
    ForeignKeyConstraintCreator ..> ForeignKeyConstraint : creates
    UniqueConstraintCreator ..> UniqueConstraint : creates
    CheckConstraintCreator ..> CheckConstraint : creates
```

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant Registry as ConstraintCreatorRegistry
    participant Creator as PrimaryKeyConstraintCreator
    participant Constraint as PrimaryKeyConstraint

    Client->>Registry: GetCreator(metadata.Type)
    Registry-->>Client: creator

    Client->>Creator: CreateConstraint(metadata)
    activate Creator

    Note right of Creator: Factory Method
    Creator->>Constraint: new PrimaryKeyConstraint(...)
    Constraint-->>Creator: constraint

    Creator-->>Client: constraint
    deactivate Creator
```

### 3.3. Strategy (Referential Action)

The **Strategy** pattern is used to handle foreign key referential actions (`ON DELETE`, `ON UPDATE`). Instead of writing hardcoded `switch` statements inside the `ForeignKeyConstraint` class, the behavior is delegated to a strategy interface `IReferentialAction`. Concrete strategies like `CascadeAction`, `RestrictAction`, `SetNullAction`, and `SetDefaultAction` implement the specific execution logic dynamically based on table metadata.

```mermaid
classDiagram
    class Client
    class ForeignKeyConstraint {
        -IReferentialAction _strategy
        +OnParentRowDeleted(parentRow) result
    }
    class IReferentialAction {
        <<interface>>
        +Execute(parentRow, childTable) result
    }
    class CascadeAction {
        +Execute(parentRow, childTable) result
    }
    class RestrictAction {
        +Execute(parentRow, childTable) result
    }
    class SetNullAction {
        +Execute(parentRow, childTable) result
    }
    class SetDefaultAction {
        +Execute(parentRow, childTable) result
    }

    Client --> ForeignKeyConstraint
    ForeignKeyConstraint o--> IReferentialAction : delegates to
    IReferentialAction <|.. CascadeAction
    IReferentialAction <|.. RestrictAction
    IReferentialAction <|.. SetNullAction
    IReferentialAction <|.. SetDefaultAction
```

```mermaid
sequenceDiagram
    autonumber

    participant Client
    participant FK as ForeignKeyConstraint (Context)
    participant Strategy as IReferentialAction (Strategy)
    participant ChildTable as Table (Child)

    Client->>FK: OnParentRowDeleted(parentRow)
    activate FK

    Note over FK: Context delegates the behavior<br/>to the configured strategy
    FK->>Strategy: Execute(parentRow, childTable)
    activate Strategy

    alt is CascadeAction
        Strategy->>ChildTable: DeleteRow(childRow)
    else is SetNullAction
        Strategy->>ChildTable: UpdateRow(childRow, null)
    else is RestrictAction
        Strategy-->>FK: throws ReferentialIntegrityException
    end

    Strategy-->>FK: result
    deactivate Strategy

    FK-->>Client: result
    deactivate FK
```

### 3.4. Composite (Schema Objects)

The **Composite** pattern is used to treat individual database objects (`Table`, `View`, `StoredProcedure`) and groups of objects uniformly. The `Schema` class acts as the composite node that manages collections of these leaf objects. When a high-level lifecycle operation such as `Drop()` is performed on the `Schema`, it delegates the operation to all of its child components.

```mermaid
classDiagram
    class ISchemaObject {
        <<interface>>
        +Drop()
    }
    class Schema {
        -List~ISchemaObject~ _objects
        +Drop()
        +AddObject(ISchemaObject)
        +RemoveObject(ISchemaObject)
    }
    class Table {
        +Drop()
    }
    class View {
        +Drop()
    }
    class StoredProcedure {
        +Drop()
    }

    ISchemaObject <|.. Schema
    ISchemaObject <|.. Table
    ISchemaObject <|.. View
    ISchemaObject <|.. StoredProcedure
    Schema o--> ISchemaObject : children
```

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant Schema
    participant Child as ISchemaObject

    Client->>Schema: Drop()
    activate Schema

    loop for each child in _objects
        Schema->>Child: Drop()
        activate Child
        Note over Child: Concrete objects (Table, View) <br/> handle their own drop logic.
        Child-->>Schema: success
        deactivate Child
    end

    Schema-->>Client: success
    deactivate Schema
```

### 3.5. Command (DDL Command)

The **Command** pattern is used to encapsulate DDL operations (like `CreateTable`, `DropTable`, and `AlterTable`) into standalone command objects. This allows the system to parameterize clients with different requests, queue or log requests, and support undoable operations. The `DDLCommandExecutor` acts as the invoker that executes the concrete `IDDLCommand`.

```mermaid
classDiagram
    class Client
    class DDLCommandExecutor {
        +Execute(IDDLCommand) DDLResult
    }
    class IDDLCommand {
        <<interface>>
        +Execute() DDLResult
    }
    class CreateTableCommand {
        +Execute() DDLResult
    }
    class AlterTableCommand {
        +Execute() DDLResult
    }
    class DropTableCommand {
        +Execute() DDLResult
    }
    class Schema {
        +ContainsTable(tableName)
        +AddTable(table)
        +AlterTable(tableName, newTable)
    }
    class Table

    Client --> DDLCommandExecutor
    Client ..> CreateTableCommand : creates
    DDLCommandExecutor o--> IDDLCommand : invokes
    IDDLCommand <|.. CreateTableCommand
    IDDLCommand <|.. AlterTableCommand
    IDDLCommand <|.. DropTableCommand
    CreateTableCommand --> Schema : receiver
    CreateTableCommand --> Table : creates
    AlterTableCommand --> Schema : receiver
```

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant Executor as DDLCommandExecutor
    participant Command as IDDLCommand
    participant Concrete as CreateTableCommand
    participant Schema
    participant Table

    Client->>Executor: Execute(createTableCommand)
    activate Executor

    Executor->>Command: Execute()
    Command->>Concrete: Execute()
    activate Concrete

    Concrete->>Schema: ContainsTable(tableName)
    Schema-->>Concrete: false

    Concrete->>Table: new Table(tableName)
    Table-->>Concrete: table

    Concrete->>Schema: AddTable(table)
    Schema-->>Concrete: success

    Concrete-->>Command: DDLResult.Success
    deactivate Concrete

    Command-->>Executor: DDLResult.Success
    Executor-->>Client: DDLResult.Success

    deactivate Executor
```

### 3.6. Iterator (Schema Object Traversal)

The **Iterator** pattern provides sequential access to schema objects without exposing the internal collection structures. The `Schema` class acts as the aggregate, providing a `CreateIterator()` method that returns an `ISchemaObjectIterator`. The client uses `HasNext()` and `Next()` to traverse through all `ISchemaObject` elements (like `Table`, `View`, and `StoredProcedure`).

```mermaid
classDiagram
    direction LR

    class Client

    class Schema {
        +CreateIterator() ISchemaObjectIterator
    }

    class ISchemaObjectIterator {
        <<interface>>
        +HasNext() bool
        +Next() ISchemaObject
    }

    class SchemaObjectIterator {
        -IReadOnlyList~ISchemaObject~ _objects
        -int _position
        +HasNext() bool
        +Next() ISchemaObject
    }

    class ISchemaObject {
        <<interface>>
        +Name
    }

    class Table
    class View
    class StoredProcedure

    Client --> Schema
    Client --> ISchemaObjectIterator

    Schema --> SchemaObjectIterator : creates
    ISchemaObjectIterator <|.. SchemaObjectIterator

    ISchemaObject <|.. Table
    ISchemaObject <|.. View
    ISchemaObject <|.. StoredProcedure

    Schema o-- Table
    Schema o-- View
    Schema o-- StoredProcedure

    SchemaObjectIterator --> ISchemaObject
```

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant Schema
    participant Iterator as SchemaObjectIterator
    participant Object as ISchemaObject

    Client->>Schema: CreateIterator()
    Schema-->>Client: iterator

    loop For each object
        Client->>Iterator: HasNext()
        Iterator-->>Client: true

        Client->>Iterator: Next()
        Iterator-->>Client: schemaObject

        Client->>Object: Process object
    end

    Client->>Iterator: HasNext()
    Iterator-->>Client: false
```

### 3.7. Visitor (Schema Operations)

The **Visitor** pattern is used for schema operations like backup, export, and validation. It allows defining new operations on schema objects (Table, View, StoredProcedure, Schema) without modifying their classes.

```mermaid
classDiagram
    direction TB

    %% =========================
    %% VISITOR
    %% =========================

    class ISchemaVisitor {
        <<interface>>
        +Visit(Schema schema) void
        +Visit(Table table) void
        +Visit(View view) void
        +Visit(StoredProcedure procedure) void
    }

    class BackupVisitor {
        +Visit(Schema schema) void
        +Visit(Table table) void
        +Visit(View view) void
        +Visit(StoredProcedure procedure) void
    }

    class ExportVisitor {
        +Visit(Schema schema) void
        +Visit(Table table) void
        +Visit(View view) void
        +Visit(StoredProcedure procedure) void
    }

    class ValidationVisitor {
        +Visit(Schema schema) void
        +Visit(Table table) void
        +Visit(View view) void
        +Visit(StoredProcedure procedure) void
    }

    %% =========================
    %% ELEMENT
    %% =========================

    class ISchemaObject {
        <<interface>>
        +int Id
        +string Name
        +Accept(ISchemaVisitor visitor) void
    }

    class Schema {
        +string Name
        +IEnumerable~ISchemaObject~ Objects
        +Accept(ISchemaVisitor visitor) void
    }

    class Table {
        +string Name
        +IReadOnlyList~Column~ Columns
        +IReadOnlyList~Constraint~ Constraints
        +IReadOnlyList~Index~ Indexes
        +Accept(ISchemaVisitor visitor) void
    }

    class View {
        +string Name
        +string Query
        +IReadOnlyList~string~ Dependencies
        +Accept(ISchemaVisitor visitor) void
    }

    class StoredProcedure {
        +string Name
        +ProcedureBody Body
        +Accept(ISchemaVisitor visitor) void
    }

    %% =========================
    %% CLIENT / MANAGERS
    %% =========================

    class SchemaManager {
        +Validate(Schema schema) void
    }

    %% Visitor implementations
    ISchemaVisitor <|.. BackupVisitor
    ISchemaVisitor <|.. ExportVisitor
    ISchemaVisitor <|.. ValidationVisitor

    %% Element implementations
    ISchemaObject <|.. Schema
    ISchemaObject <|.. Table
    ISchemaObject <|.. View
    ISchemaObject <|.. StoredProcedure

    %% Schema contains schema objects
    Schema *-- ISchemaObject : contains

    %% Clients create/use visitors
    SchemaManager ..> ValidationVisitor : creates

    %% Visitors operate on elements
    BackupVisitor ..> ISchemaObject : visits
    ExportVisitor ..> ISchemaObject : visits
    ValidationVisitor ..> ISchemaObject : visits
```

```mermaid
sequenceDiagram
    actor Client
    participant BV as BackupVisitor
    participant T as Table

    Client->>BV: new BackupVisitor()
    Client->>T: Accept(backupVisitor)

    T->>BV: Visit(this)

    Note over T,BV: this = Table<br/>selects Visit(Table)

    BV->>T: Get Columns
    T-->>BV: Columns

    BV->>T: Get Constraints
    T-->>BV: Constraints

    BV->>T: Get Indexes
    T-->>BV: Indexes

    BV->>BV: Backup table structure and data

    BV-->>T: Completed
    T-->>Client: Completed
```