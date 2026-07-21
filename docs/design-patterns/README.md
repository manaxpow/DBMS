# Database Design Patterns

This document tracks the design patterns used across different modules in the DBMS and their current implementation status.

## 1. Database Objects

| Status | Design Pattern      | Feature            | Reason / Context                                                                        |
| :----: | :------------------ | :----------------- | :-------------------------------------------------------------------------------------- |
| `[x]`  | **Template Method** | Constraint         | `Validate()` defines the workflow, each constraint only implements `Check()`.           |
| `[x]`  | **Strategy**        | Referential Action | Selects Cascade, Restrict, SetNull, or SetDefault behavior when deleting/updating.      |
| `[x]`  | **Composite**       | Schema Objects     | Schema contains Tables, Views, Procedures and manages them uniformly.                   |
| `[x]`  | **Command**         | DDL Command        | `CreateTable`, `DropTable`, and `AlterTable` operations are encapsulated into commands. |
| `[ ]`  | **State**           | Object Status      | Table transitions between states like Creating, Available, Dropping, Dropped.           |

## 2. Database Management

| Status | Design Pattern      | Feature             | Reason / Context                                                                                             |
| :----: | :------------------ | :------------------ | :----------------------------------------------------------------------------------------------------------- |
| `[ ]`  | **Facade**          | DatabaseManager     | Provides a single unified API to create, open, close, and drop databases.                                    |
| `[ ]`  | **Factory Method**  | Database Creation   | Creates a Database along with its dependencies like SystemCatalog, Schema, and Storage.                      |
| `[ ]`  | **Command**         | Database Operations | `CreateDatabase`, `DropDatabase`, and `RenameDatabase` are encapsulated as commands.                         |
| `[ ]`  | **State**           | Database Lifecycle  | Database transitions between states such as Offline, Online, ReadOnly, and Recovering.                       |
| `[ ]`  | **Observer**        | Database Events     | Monitoring systems receive events for Create, Drop, Backup, and Restore.                                     |
| `[ ]`  | **Template Method** | Backup/Restore      | Provides a fixed backup workflow, while differentiating between Full and Incremental backup implementations. |

_Note: Update the status column to `[x]` when a pattern is implemented in the source code to manage progress._

## 3. Pattern Implementation Details

### 3.1. Template Method (Constraint)

The **Template Method** pattern is used in the `Constraint` class. The base class defines the skeletal workflow for validation in the `Validate()` method (e.g., checking if the constraint is enabled), and defers the specific logic to the `Check()` method which must be implemented by subclasses like `UniqueConstraint` or `PrimaryKeyConstraint`.

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

### 3.2. Strategy (Referential Action)

The **Strategy** pattern is used to handle foreign key referential actions (`ON DELETE`, `ON UPDATE`). Instead of writing hardcoded `switch` statements inside the `ForeignKeyConstraint` class, the behavior is delegated to a strategy interface `IReferentialAction`. Concrete strategies like `CascadeAction`, `RestrictAction`, `SetNullAction`, and `SetDefaultAction` implement the specific execution logic dynamically based on table metadata.

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

### 3.3. Composite (Schema Objects)

The **Composite** pattern is used to treat individual database objects (`Table`, `View`, `StoredProcedure`) and groups of objects uniformly. The `Schema` class acts as the composite node that manages collections of these leaf objects. When a high-level lifecycle operation such as `Drop()` is performed on the `Schema`, it delegates the operation to all of its child components.

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

### 3.4. Command (DDL Command)

The **Command** pattern is used to encapsulate DDL operations (like `CreateTable`, `DropTable`, and `AlterTable`) into standalone command objects. This allows the system to parameterize clients with different requests, queue or log requests, and support undoable operations. The `DDLCommandExecutor` acts as the invoker that executes the concrete `IDDLCommand`.

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
