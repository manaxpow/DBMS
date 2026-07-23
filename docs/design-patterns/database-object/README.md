# Database Design Patterns

This document tracks the design patterns used across different modules in the DBMS and their current implementation status.

## 1. Database Objects

| Priority  | Status | Design Pattern      | Feature                 | Reason / Context                                                                                      |
| :-------: | :----: | :------------------ | :---------------------- | :---------------------------------------------------------------------------------------------------- |
|  🔴 High  | `[x]`  | **Template Method** | Constraint              | `Validate()` defines the workflow, while each concrete constraint only implements `Check()`.          |
|  🔴 High  | `[x]`  | **Factory Method**  | Constraint Creation     | Creates `PrimaryKey`, `ForeignKey`, `Unique`, and `CheckConstraint` objects from metadata.            |
|  🔴 High  | `[x]`  | **Strategy**        | Referential Action      | Selects Cascade, Restrict, SetNull, or SetDefault behavior when deleting or updating referenced rows. |
|  🔴 High  | `[x]`  | **Composite**       | Schema Objects          | Schema contains Tables, Views, and Stored Procedures and manages them uniformly as `ISchemaObject`.   |
|  🔴 High  | `[x]`  | **Command**         | DDL Command             | `CreateTable`, `DropTable`, and `AlterTable` operations are encapsulated into command objects.        |
| 🟡 Medium | `[x]`  | **Iterator**        | Schema Object Traversal | Provides sequential access to schema objects without exposing internal collections.                   |
| 🟡 Medium | `[x]`  | **Visitor**         | Schema Operations       | Backup, Export, and Validation can operate on all schema object types.                                |
| 🟡 Medium | `[x]`  | **Builder**         | Table Definition        | Builds a Table step by step from columns, constraints, indexes, and partitions.                       |
|  🟢 Low   | `[ ]`  | **Prototype**       | Schema Object Cloning   | Clones schema objects for migration, temporary objects, or schema duplication.                        |
|  🟢 Low   | `[ ]`  | **Decorator**       | Constraint Extension    | Adds logging, metrics, or auditing without modifying existing constraints.                            |
|  🟢 Low   | `[ ]`  | **Mediator**        | Dependency Management   | Coordinates interactions among Tables, Views, Procedures, and Foreign Keys.                           |

---

## 2. Database Management

For Database Management patterns, please see [Database Management Patterns](./database-managment/README.md).

_Note: Update the status column to `[x]` when a pattern is implemented in the source code to manage progress._

## 3. Pattern Implementation Details

### 3.1. Template Method (Constraint)

The **Template Method** pattern is used in the `Constraint` class.

- Define a template method with **multiple steps**.

- Delegate subclass implement how each step work.

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

#### Sequence Diagram: Constraint Validation Workflow

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

The **Factory Method** pattern uses for creating `Constraint`.

- Define a abstract **Factory Method**
- Delegate object creations to **Concrete Creator** through Polymorphism.

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

#### Sequence Diagram: Constraint Instantiation

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

The **Strategy** pattern is used to implemnt Referential Action of FK.

- **Using Polymorphism to dispatch appropriate algorithm** (runtime).

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

#### Sequence Diagram: Referential Action Execution

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

The **Composite** pattern is used to treat individual database objects (`Table`, `View`, `StoredProcedure`) and groups of objects uniformly.
The `Schema` class acts as the composite node that manages collections of these leaf objects. When a high-level lifecycle operation such as `Drop()` is performed on the `Schema`, it delegates the operation to all of its child components.

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

#### Sequence Diagram: Recursive Drop Operation

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

The **Command** pattern is used to encapsulate DDL operations (like `CreateTable`, `DropTable`, and `AlterTable`) into standalone command objects.
This allows the system to parameterize clients with different requests, queue or log requests, and support undoable operations. The `DDLCommandExecutor` acts as the invoker that executes the concrete `IDDLCommand`.

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

#### Sequence Diagram: DDL Command Execution

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

The **Iterator** pattern provides sequential access to schema objects without exposing the internal collection structures

- Encapsulates the traversal logic inside an Iterator.
- Allow client traverse a collection without knowing about how it's stored.

```mermaid
classDiagram
    direction TB

    %% =========================
    %% SCHEMA OBJECTS
    %% =========================

    class ISchemaObject {
        <<interface>>
        +int Id
        +string Name
        +Drop() void
    }

    class Schema {
        +int Id
        +string Name
        +IEnumerable~ISchemaObject~ Objects

        +CreateTableIterator() ISchemaObjectIterator
        +CreateViewIterator() ISchemaObjectIterator
        +CreateStoredProcedureIterator() ISchemaObjectIterator
        +CreateAllObjectsIterator() ISchemaObjectIterator

        +Drop() void
    }

    class Table {
        +int Id
        +string Name
        +Drop() void
    }

    class View {
        +int Id
        +string Name
        +Drop() void
    }

    class StoredProcedure {
        +int Id
        +string Name
        +Drop() void
    }

    ISchemaObject <|.. Schema
    ISchemaObject <|.. Table
    ISchemaObject <|.. View
    ISchemaObject <|.. StoredProcedure

    Schema *-- Table : contains
    Schema *-- View : contains
    Schema *-- StoredProcedure : contains


    %% =========================
    %% ITERATOR
    %% =========================

    class ISchemaObjectIterator {
        <<interface>>
        +HasNext() bool
        +Next() ISchemaObject
        +Reset() void
    }


    %% =========================
    %% CONCRETE ITERATORS
    %% =========================

    class TableIterator {
        -IReadOnlyList~ISchemaObject~ _objects
        -int _position
        +HasNext() bool
        +Next() ISchemaObject
        +Reset() void
    }

    class ViewIterator {
        -IReadOnlyList~ISchemaObject~ _objects
        -int _position
        +HasNext() bool
        +Next() ISchemaObject
        +Reset() void
    }

    class StoredProcedureIterator {
        -IReadOnlyList~ISchemaObject~ _objects
        -int _position
        +HasNext() bool
        +Next() ISchemaObject
        +Reset() void
    }

    class SchemaObjectsIterator {
        -IReadOnlyList~ISchemaObject~ _objects
        -int _position
        +HasNext() bool
        +Next() ISchemaObject
        +Reset() void
    }


    %% =========================
    %% ITERATOR IMPLEMENTATIONS
    %% =========================

    ISchemaObjectIterator <|.. TableIterator
    ISchemaObjectIterator <|.. ViewIterator
    ISchemaObjectIterator <|.. StoredProcedureIterator
    ISchemaObjectIterator <|.. SchemaObjectsIterator


    %% =========================
    %% ITERATOR TARGETS
    %% =========================

    TableIterator --> Table : returns only
    ViewIterator --> View : returns only
    StoredProcedureIterator --> StoredProcedure : returns only

    SchemaObjectsIterator --> ISchemaObject : returns all

    Schema --> ISchemaObjectIterator : creates
```

#### Sequence Diagram: Schema Object Traversal

```mermaid
sequenceDiagram
    autonumber

    participant Test as IteratorTests
    participant Schema as Schema
    participant TI as TableIterator

    Test->>Schema: CreateTableIterator()
    activate Schema

    Schema->>Schema: Get Objects
    Schema->>TI: new TableIterator(Objects)
    TI-->>Schema: iterator
    Schema-->>Test: iterator

    deactivate Schema

    loop while HasNext()
        Test->>TI: HasNext()
        activate TI

        TI->>TI: Find next Table from _position
        TI-->>Test: true

        deactivate TI

        Test->>TI: Next()
        activate TI

        TI->>TI: Get next Table
        TI->>TI: Advance _position
        TI-->>Test: Table as ISchemaObject

        deactivate TI
    end

    Test->>TI: HasNext()
    TI-->>Test: false
```

### 3.7. Visitor (Schema Operations)

The **Visitor** pattern is used for schema operations like backup, export, and validation.

- It allows **defining new operations** on schema objects (Table, View, StoredProcedure, Schema) **without modifying their classes**.

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

#### Sequence Diagram: Visitor Dispatch Workflow

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

### 3.8. Builder (Table Definition)

The **Builder** pattern is used to construct complex `Table` objects step by step. This encapsulates the construction logic of columns, constraints, indexes, and partitions, keeping the `Table` constructor clean and preventing partially initialized tables.

```mermaid
classDiagram
    direction LR

    class Client {
        +CreateTable() Table
    }

    class ITableBuilder {
        <<interface>>
        +SetName(string name) ITableBuilder
        +AddColumn(Column column) ITableBuilder
        +AddConstraint(Constraint constraint) ITableBuilder
        +AddIndex(Index index) ITableBuilder
        +AddPartition(Partition partition) ITableBuilder
        +Build() Table
    }

    class TableBuilder {
        +SetName(string name) ITableBuilder
        +AddColumn(Column column) ITableBuilder
        +AddConstraint(Constraint constraint) ITableBuilder
        +AddIndex(Index index) ITableBuilder
        +AddPartition(Partition partition) ITableBuilder
        +Build() Table
    }

    class Table {
        +string Name
        +Columns
        +Constraints
        +Indexes
        +Partitions
    }

    Client --> ITableBuilder : uses
    ITableBuilder <|.. TableBuilder
    TableBuilder ..> Table : builds
```

#### Sequence Diagram: Step-by-Step Table Construction

```mermaid
sequenceDiagram
    autonumber
    
    actor Client
    participant TB as TableBuilder
    participant T as Table
    
    Client->>TB: SetName("Users")
    TB-->>Client: ITableBuilder
    
    loop For each Column
        Client->>TB: AddColumn(col)
        TB-->>Client: ITableBuilder
    end
    
    loop For each Constraint
        Client->>TB: AddConstraint(const)
        TB-->>Client: ITableBuilder
    end
    
    loop For each Index
        Client->>TB: AddIndex(idx)
        TB-->>Client: ITableBuilder
    end
    
    loop For each Partition
        Client->>TB: AddPartition(part)
        TB-->>Client: ITableBuilder
    end
    
    Client->>TB: Build()
    activate TB
    
    TB->>T: new Table(...)
    T-->>TB: Table
    
    TB-->>Client: Table
    deactivate TB
```
