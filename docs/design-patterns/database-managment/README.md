# Database Management Patterns

## 2. Database Management

|  Priority | Status | Design Pattern       | Feature                    | Reason / Context                                                                                         |
| :-------: | :----: | :------------------- | :------------------------- | :------------------------------------------------------------------------------------------------------- |
|  🔴 High  |  `[x]` | **Facade**           | DatabaseServer             | Provides a single unified API to start, stop, configure, and access the database server.                 |
|  🔴 High  |  `[ ]` | **Command**          | Database Operations        | Encapsulates `CreateDatabase`, `DropDatabase`, and `RenameDatabase` into command objects.                |
|  🔴 High  |  `[x]` | **Observer**         | Database Events            | Monitoring, Logging, and Replication receive Create, Drop, Backup, Restore, and State events.            |
| 🟡 Medium |  `[ ]` | **Factory Method**   | Database Creation          | Allows different Database implementations to be instantiated by subclasses or providers.                 |
| 🟡 Medium |  `[ ]` | **State**            | Database Lifecycle         | Database transitions between Offline, Online, ReadOnly, Recovering, and Dropped states.                  |
| 🟡 Medium |  `[ ]` | **Template Method**  | Backup/Restore             | Defines a common workflow while allowing Full and Incremental implementations to differ.                 |
| 🟡 Medium |  `[ ]` | **Adapter**          | External Storage           | Adapts operating-system or cloud-storage APIs to DBMS storage interfaces.                                |
|   🟢 Low  |  `[ ]` | **Builder**          | Database Configuration     | Builds database configuration (page size, logging, storage, security) step by step.                      |
|   🟢 Low  |  `[ ]` | **Proxy**            | Database Access            | Adds authorization, lazy opening, remote access, or logging around database access.                      |
|   🟢 Low  |  `[ ]` | **Bridge**           | Database Storage           | Separates Database abstraction from different storage implementations.                                   |
|   🟢 Low  |  `[ ]` | **Mediator**         | Subsystem Coordination     | Coordinates Storage, Catalog, Transaction, Recovery, Security, and Monitoring modules.                   |
|   🟢 Low  |  `[ ]` | **Decorator**        | Database Service Extension | Adds metrics, tracing, caching, or auditing without changing the core service.                           |

## 3. Pattern Implementation Details

### 3.1. Facade (DatabaseServer)

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


### 3.2. Observer (Database Events)

The **Observer** pattern is used to notify various subsystems (like Monitoring, Logging, and Replication) about database lifecycle events. When an event occurs (e.g., `DatabaseCreated`), the `DatabaseEventPublisher` notifies all registered `IDatabaseEventObserver` instances.

```mermaid
classDiagram
    direction TB

    class DatabaseEventPublisher {
        -List~IDatabaseEventObserver~ _observers
        +Subscribe(IDatabaseEventObserver observer) void
        +Unsubscribe(IDatabaseEventObserver observer) void
        +Notify(DatabaseEvent event) void
    }

    class IDatabaseEventObserver {
        <<interface>>
        +OnDatabaseEvent(DatabaseEvent event) void
    }

    class MonitoringObserver {
        +OnDatabaseEvent(DatabaseEvent event) void
    }

    class LoggingObserver {
        +OnDatabaseEvent(DatabaseEvent event) void
    }

    class ReplicationObserver {
        +OnDatabaseEvent(DatabaseEvent event) void
    }

    class DatabaseEvent {
        +DatabaseEventType Type
        +string DatabaseName
        +DateTime Timestamp
    }

    class DatabaseEventType {
        <<enumeration>>
        Created
        Dropped
        BackupCompleted
        Restored
        StateChanged
    }

    DatabaseEventPublisher o-- IDatabaseEventObserver : observers

    IDatabaseEventObserver <|.. MonitoringObserver
    IDatabaseEventObserver <|.. LoggingObserver
    IDatabaseEventObserver <|.. ReplicationObserver

    DatabaseEventPublisher ..> DatabaseEvent : publishes
    DatabaseEvent --> DatabaseEventType
```

#### Sequence Diagram: Database Event Notification

```mermaid
sequenceDiagram
    autonumber

    actor Client
    participant DM as DatabaseManager
    participant EP as DatabaseEventPublisher
    participant LO as LoggingObserver
    participant MO as MonitoringObserver
    participant RO as ReplicationObserver

    Client->>DM: CreateDatabase("ShopDB")

    DM->>DM: Create database

    DM->>EP: Notify(DatabaseCreated)

    EP->>LO: OnDatabaseEvent(DatabaseCreated)
    LO->>LO: Write log
    LO-->>EP: Completed

    EP->>MO: OnDatabaseEvent(DatabaseCreated)
    MO->>MO: Update metrics
    MO-->>EP: Completed

    EP->>RO: OnDatabaseEvent(DatabaseCreated)
    RO->>RO: Replicate metadata
    RO-->>EP: Completed

    EP-->>DM: Notification completed

    DM-->>Client: Database created
```
