# Database Design Patterns

This document tracks the design patterns used across different modules in the DBMS and their current implementation status.

## 1. Database Objects

|  Priority | Status | Design Pattern      | Feature                 | Reason / Context                                                                                      |
| :-------: | :----: | :------------------ | :---------------------- | :---------------------------------------------------------------------------------------------------- |
|  🔴 High  |  `[x]` | **Template Method** | Constraint              | `Validate()` defines the workflow, while each concrete constraint only implements `Check()`.          |
|  🔴 High  |  `[ ]` | **Factory Method**  | Constraint Creation     | Creates `PrimaryKey`, `ForeignKey`, `Unique`, and `CheckConstraint` objects from metadata.            |
|  🔴 High  |  `[x]` | **Strategy**        | Referential Action      | Selects Cascade, Restrict, SetNull, or SetDefault behavior when deleting or updating referenced rows. |
|  🔴 High  |  `[x]` | **Composite**       | Schema Objects          | Schema contains Tables, Views, and Stored Procedures and manages them uniformly as `ISchemaObject`.   |
|  🔴 High  |  `[x]` | **Command**         | DDL Command             | `CreateTable`, `DropTable`, and `AlterTable` operations are encapsulated into command objects.        |
| 🟡 Medium |  `[ ]` | **Iterator**        | Schema Object Traversal | Provides sequential access to schema objects without exposing internal collections.                   |
| 🟡 Medium |  `[ ]` | **Visitor**         | Schema Operations       | Backup, Export, Validation, and Dependency Analysis can operate on all schema object types.           |
| 🟡 Medium |  `[ ]` | **State**           | Object Status           | Table transitions between `Creating`, `Available`, `Dropping`, and `Dropped`.                         |
| 🟡 Medium |  `[ ]` | **Builder**         | Table Definition        | Builds a Table step by step from columns, constraints, indexes, and partitions.                       |
|   🟢 Low  |  `[ ]` | **Prototype**       | Schema Object Cloning   | Clones schema objects for migration, temporary objects, or schema duplication.                        |
|   🟢 Low  |  `[ ]` | **Decorator**       | Constraint Extension    | Adds logging, metrics, or auditing without modifying existing constraints.                            |
|   🟢 Low  |  `[ ]` | **Mediator**        | Dependency Management   | Coordinates interactions among Tables, Views, Procedures, and Foreign Keys.                           |

---

## 2. Database Management

|  Priority | Status | Design Pattern       | Feature                    | Reason / Context                                                                                         |
| :-------: | :----: | :------------------- | :------------------------- | :------------------------------------------------------------------------------------------------------- |
|  🔴 High  |  `[x]` | **Facade**           | DatabaseServer             | Provides a single unified API to start, stop, configure, and access the database server.                 |
|  🔴 High  |  `[ ]` | **Abstract Factory** | Database Components        | Creates compatible families of Database, Catalog, Schema, Storage, Transaction, and Recovery components. |
|  🔴 High  |  `[ ]` | **Command**          | Database Operations        | Encapsulates `CreateDatabase`, `DropDatabase`, and `RenameDatabase` into command objects.                |
|  🔴 High  |  `[ ]` | **Observer**         | Database Events            | Monitoring, Logging, and Replication receive Create, Drop, Backup, Restore, and State events.            |
| 🟡 Medium |  `[ ]` | **Factory Method**   | Database Creation          | Allows different Database implementations to be instantiated by subclasses or providers.                 |
| 🟡 Medium |  `[ ]` | **State**            | Database Lifecycle         | Database transitions between Offline, Online, ReadOnly, Recovering, and Dropped states.                  |
| 🟡 Medium |  `[ ]` | **Template Method**  | Backup/Restore             | Defines a common workflow while allowing Full and Incremental implementations to differ.                 |
| 🟡 Medium |  `[ ]` | **Adapter**          | External Storage           | Adapts operating-system or cloud-storage APIs to DBMS storage interfaces.                                |
|   🟢 Low  |  `[ ]` | **Builder**          | Database Configuration     | Builds database configuration (page size, logging, storage, security) step by step.                      |
|   🟢 Low  |  `[ ]` | **Proxy**            | Database Access            | Adds authorization, lazy opening, remote access, or logging around database access.                      |
|   🟢 Low  |  `[ ]` | **Bridge**           | Database Storage           | Separates Database abstraction from different storage implementations.                                   |
|   🟢 Low  |  `[ ]` | **Mediator**         | Subsystem Coordination     | Coordinates Storage, Catalog, Transaction, Recovery, Security, and Monitoring modules.                   |
|   🟢 Low  |  `[ ]` | **Decorator**        | Database Service Extension | Adds metrics, tracing, caching, or auditing without changing the core service.                           |

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

    participant Client
    participant Manager as SchemaManager
    participant Schema
    participant Catalog as SystemCatalog
    participant Storage as StorageEngine

    Client->>Manager: DropSchema(schema, cascade)
    activate Manager

    Manager->>Schema: Objects
    activate Schema
    Schema-->>Manager: schemaObjects
    deactivate Schema

    alt Schema contains objects and cascade = false
        Manager-->>Client: throw SchemaNotEmptyException
    else Schema is empty or cascade = true

        opt cascade = true
            loop for each schemaObject
                Manager->>Manager: DropObject(schema, schemaObject)

                alt schemaObject is Table
                    Manager->>Manager: Check table dependencies
                    Manager->>Catalog: UnregisterTable(schemaObject.Name)
                    Catalog-->>Manager: success
                    Manager->>Storage: DropTableStorage(schemaObject.Id)
                    Storage-->>Manager: success

                else schemaObject is View
                    Manager->>Manager: Check view dependencies
                    Manager->>Catalog: UnregisterView(schemaObject.Name)
                    Catalog-->>Manager: success

                else schemaObject is StoredProcedure
                    Manager->>Catalog: UnregisterProcedure(schemaObject.Name)
                    Catalog-->>Manager: success
                end

                Manager->>Schema: UnregisterObject(schemaObject.Name)
                Schema-->>Manager: removedObject
            end
        end

        Manager->>Catalog: UnregisterSchema(schema.Name)
        Catalog-->>Manager: success

        Manager-->>Client: success
    end

    deactivate Manager
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

For updating an existing table, the flow for `AlterTableCommand` works similarly:

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant Executor as DDLCommandExecutor
    participant Command as IDDLCommand
    participant Concrete as AlterTableCommand
    participant Schema

    Client->>Executor: Execute(alterTableCommand)
    activate Executor

    Executor->>Command: Execute()
    Command->>Concrete: Execute()
    activate Concrete

    Concrete->>Schema: ContainsTable(tableName)
    Schema-->>Concrete: true

    Concrete->>Schema: AlterTable(tableName, newTable)
    Schema-->>Concrete: success

    Concrete-->>Command: DDLResult.Success
    deactivate Concrete

    Command-->>Executor: DDLResult.Success
    Executor-->>Client: DDLResult.Success

    deactivate Executor
```

### 3.6. Facade (DatabaseServer)

The **Facade** pattern is used in `DatabaseServer` to provide a single, unified interface for starting and stopping the database system. Instead of the client interacting with multiple complex subsystems (such as `StorageEngine`, `TransactionManager`, `QueryProcessor`, and `NetworkServer`), the `DatabaseServer` coordinates their initialization and startup sequences in the correct order.

```mermaid
classDiagram
    class Client
    class DatabaseServer {
        -bool _isRunning
        +Start(config) success
    }
    class StorageEngine {
        +Start(config) success
    }
    class TransactionManager {
        +Start(config) success
    }
    class QueryProcessor {
        +Start(config) success
    }
    class NetworkServer {
        +Start(config) success
    }

    Client --> DatabaseServer
    DatabaseServer --> StorageEngine
    DatabaseServer --> TransactionManager
    DatabaseServer --> QueryProcessor
    DatabaseServer --> NetworkServer
```

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant Server as DatabaseServer
    participant Storage as StorageEngine
    participant Transaction as TransactionManager
    participant Query as QueryProcessor
    participant Network as NetworkServer

    Client->>Server: Start(config)
    activate Server

    Server->>Server: Check IsRunning

    Server->>Storage: Start(config)
    Storage-->>Server: success

    Server->>Transaction: Start(config)
    Transaction-->>Server: success

    Server->>Query: Start(config)
    Query-->>Server: success

    Server->>Network: Start(config)
    Network-->>Server: success

    Server->>Server: _isRunning = true
    Server-->>Client: success

    deactivate Server 
```
