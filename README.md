# DBMS 2 layer

Database management system

```mermaid
mindmap
  root((DatabaseServer))
    StorageEngine
      FileManager
      BufferPool
        Page
    QueryProcessor
      SQLParser
        Lexer
        AST
      QueryOptimizer
        LogicalPlan
        PhysicalPlan
      QueryExecutor
    TransactionManager
      Transaction
      LockManager
      MVCCManager
    RecoveryManager
      WALManager
      BackupManager
    DatabaseManager
      Database
        Schema
          Table
            Column
            Row
            Constraint
            ForeignKey
            Index
            Partition
          View
          StoredProcedure
      CatalogManager
      StatisticsManager
    SecurityManager
      User
      Role
      Permission
    ReplicationManager
      ClusterNode
    MonitoringManager
```

## Feature Class Diagrams

### 1. Storage Engine

```mermaid
classDiagram
    direction TB
    class StorageEngine {
        +Initialize() void
    }
    class BufferPool {
        +GetPage() Page
    }
    class Page {
        +Read() byte[]
    }
    class FileManager {
        +OpenFile() void
    }
    StorageEngine *-- BufferPool
    StorageEngine *-- FileManager
    BufferPool *-- Page
```

### 2. Query Processor

```mermaid
classDiagram
    direction TB
    class QueryExecutor {
        +Execute() void
    }
    class SQLParser {
        +Parse() AST
    }
    class Lexer {
        +Tokenize() void
    }
    class AST {
        +GetRoot() void
    }
    class QueryOptimizer {
        +Optimize() PhysicalPlan
    }
    class LogicalPlan {
    }
    class PhysicalPlan {
    }

    QueryExecutor *-- SQLParser
    QueryExecutor *-- QueryOptimizer
    SQLParser *-- Lexer
    SQLParser --> AST : Creates
    QueryOptimizer --> LogicalPlan : Uses
    QueryOptimizer --> PhysicalPlan : Creates
```

### 3. Transaction Management

```mermaid
classDiagram
    direction TB
    class TransactionManager {
        +BeginTransaction() Transaction
    }
    class Transaction {
        +Commit() void
        +Rollback() void
    }
    class LockManager {
        +AcquireLock() void
    }
    class MVCCManager {
        +GetSnapshot() void
    }

    TransactionManager *-- LockManager
    TransactionManager *-- MVCCManager
    TransactionManager --> Transaction : Manages
```

### 4. Recovery Management

```mermaid
classDiagram
    direction TB
    class RecoveryManager {
        +Recover() void
    }
    class WALManager {
        +WriteLog() void
    }
    class BackupManager {
        +CreateBackup() void
    }

    RecoveryManager *-- WALManager
    RecoveryManager *-- BackupManager
```

### 5. Security Management

```mermaid
classDiagram
    direction TB
    class SecurityManager {
        +Authenticate() void
    }
    class User {
        +string Username
    }
    class Role {
        +string RoleName
    }
    class Permission {
        +string Action
    }

    SecurityManager *-- User
    SecurityManager *-- Role
    SecurityManager *-- Permission
    User *-- Role
    Role *-- Permission
```

### 6. Database Manager

```mermaid
classDiagram
    direction TB
    class DatabaseServer {
        +Start() void
    }
    class DatabaseManager {
        +CreateDatabase() void
    }
    class Database {
        +string Name
    }
    class CatalogManager {
        +GetMetadata() void
    }
    class StatisticsManager {
        +UpdateStats() void
    }

    DatabaseServer *-- DatabaseManager
    DatabaseServer *-- CatalogManager
    DatabaseManager *-- Database
    CatalogManager *-- StatisticsManager
```

### 7. Database Objects

```mermaid
classDiagram
    direction TB
    class Schema {
        +string Name
    }
    class Table {
        +string Name
    }
    class Column {
        +string Name
        +string Type
    }
    class Row {
        +object[] Values
    }
    class Constraint {
        +Check() bool
    }
    class ForeignKey {
        +string RefTable
    }
    class Index {
        +Scan() void
    }
    class Partition {
        +string Range
    }
    class View {
        +string Query
    }
    class StoredProcedure {
        +Execute() void
    }

    Schema *-- Table
    Schema *-- View
    Schema *-- StoredProcedure
    Table *-- Column
    Table *-- Row
    Table *-- Constraint
    Constraint <|-- ForeignKey
    Table *-- Index
    Table *-- Partition
```

### 8. Replication and Cluster

```mermaid
classDiagram
    direction TB
    class ReplicationManager {
        +Sync() void
    }
    class ClusterNode {
        +string NodeId
    }

    ReplicationManager *-- ClusterNode
```

### 9. Monitoring

```mermaid
classDiagram
    direction TB
    class MonitoringManager {
        +CollectMetrics() void
    }
```

## Unit Tests Architecture

### 1. Database Manager Unit Tests

```mermaid
mindmap
  root((Database Manager))
    DatabaseServer
      Start_WhenConfigurationIsValid_ShouldStart
      Stop_WhenServerIsRunning_ShouldStop
      Start_WhenPortIsUnavailable_ShouldThrow
    DatabaseManager
      CreateDatabase_WhenNameIsValid_ShouldCreateDatabase
      CreateDatabase_WhenNameAlreadyExists_ShouldThrow
      DropDatabase_WhenDatabaseExists_ShouldRemoveDatabase
    Database
      Open_WhenDatabaseIsClosed_ShouldOpenDatabase
      Close_WhenDatabaseIsOpen_ShouldCloseDatabase
      AddSchema_WhenNameAlreadyExists_ShouldThrow
    CatalogManager
      Register_WhenObjectIsValid_ShouldAddToCatalog
      Register_WhenObjectAlreadyExists_ShouldThrow
      Find_WhenObjectDoesNotExist_ShouldReturnNull
    StatisticsManager
      UpdateStatistics_WhenDataChanges_ShouldRefreshStatistics
      EstimateSelectivity_WhenStatisticsExist_ShouldReturnEstimate
      EstimateSelectivity_WhenStatisticsAreMissing_ShouldUseFallback
```

### 2. Database Objects Unit Tests

```mermaid
mindmap
  root((Database Objects))
    Schema
      AddTable_WhenTableIsValid_ShouldRegisterTable
      AddTable_WhenNameAlreadyExists_ShouldThrow
      RemoveTable_WhenTableExists_ShouldRemoveTable
    Table
      InsertRow_WhenRowIsValid_ShouldInsertRow
      InsertRow_WhenSchemaDoesNotMatch_ShouldThrow
      AddColumn_WhenNameAlreadyExists_ShouldThrow
    Column
      Create_WhenDefinitionIsValid_ShouldCreateColumn
      Create_WhenNameIsInvalid_ShouldThrow
      ValidateValue_WhenTypeDoesNotMatch_ShouldReturnFalse
    Row
      GetValue_WhenColumnExists_ShouldReturnValue
      SetValue_WhenValueIsValid_ShouldUpdateValue
      SetValue_WhenTypeDoesNotMatch_ShouldThrow
    Constraint
      Validate_WhenValueSatisfiesConstraint_ShouldSucceed
      Validate_WhenValueViolatesConstraint_ShouldFail
      Apply_WhenConstraintIsDisabled_ShouldSkipValidation
    ForeignKey
      Validate_WhenParentRecordExists_ShouldSucceed
      Validate_WhenParentRecordDoesNotExist_ShouldFail
      DeleteParent_WhenRestricted_ShouldRejectDeletion
    Index
      Insert_WhenKeyIsValid_ShouldAddEntry
      Search_WhenKeyExists_ShouldReturnRecordPointer
      Insert_WhenUniqueKeyAlreadyExists_ShouldThrow
    Partition
      RouteRow_WhenKeyMatchesRange_ShouldReturnPartition
      RouteRow_WhenKeyIsOutsideRange_ShouldFail
      AddRange_WhenRangesOverlap_ShouldThrow
    View
      Create_WhenQueryIsValid_ShouldCreateView
      Resolve_WhenDependenciesExist_ShouldReturnDefinition
      Resolve_WhenDependencyIsMissing_ShouldThrow
    StoredProcedure
      Execute_WhenParametersAreValid_ShouldReturnResult
      Execute_WhenRequiredParameterIsMissing_ShouldThrow
      Execute_WhenTransactionFails_ShouldPropagateFailure
```

### 3. Transaction Management Unit Tests

```mermaid
mindmap
  root((Transaction Management))
    Transaction
      Begin_WhenTransactionIsNew_ShouldBecomeActive
      Commit_WhenTransactionIsActive_ShouldCommit
      Rollback_WhenTransactionIsActive_ShouldRollback
    TransactionManager
      BeginTransaction_ShouldReturnActiveTransaction
      Commit_WhenTransactionExists_ShouldCommitTransaction
      Commit_WhenTransactionDoesNotExist_ShouldThrow
    LockManager
      Acquire_WhenLocksAreCompatible_ShouldGrantLock
      Acquire_WhenLocksConflict_ShouldRejectOrWait
      Release_WhenLockExists_ShouldRemoveLock
    MVCCManager
      CreateVersion_WhenRowChanges_ShouldCreateNewVersion
      ReadVersion_WhenVersionIsVisible_ShouldReturnVersion
      Cleanup_WhenVersionIsObsolete_ShouldRemoveVersion
```

### 4. Storage Engine Unit Tests

```mermaid
mindmap
  root((Storage Engine))
    BufferPool
      FetchPage_WhenPageIsBuffered_ShouldReturnExistingFrame
      FetchPage_WhenSpaceIsAvailable_ShouldLoadPage
      FetchPage_WhenAllFramesArePinned_ShouldThrow
    Page
      InsertRecord_WhenSpaceIsAvailable_ShouldInsertRecord
      InsertRecord_WhenSpaceIsInsufficient_ShouldFail
      DeleteRecord_WhenRecordExists_ShouldUpdateSlotDirectory
    StorageEngine
      Initialize_WhenConfigurationIsValid_ShouldInitializeComponents
      ReadPage_ShouldDelegateToBufferPool
      Shutdown_ShouldFlushDirtyPagesAndCloseFiles
    FileManager
      CreateFile_WhenPathIsValid_ShouldCreateFile
      OpenFile_WhenFileExists_ShouldReturnHandle
      DeleteFile_WhenFileIsInUse_ShouldThrow
```

### 5. Recovery Management Unit Tests

```mermaid
mindmap
  root((Recovery Management))
    WALManager
      Append_WhenRecordIsValid_ShouldAssignLSN
      Flush_WhenTargetLSNExists_ShouldPersistRecords
      Append_WhenSequenceIsInvalid_ShouldThrow
    RecoveryManager
      Recover_ShouldRedoCommittedTransactions
      Recover_ShouldUndoUncommittedTransactions
      Recover_WhenCheckpointExists_ShouldStartFromCheckpoint
    BackupManager
      CreateBackup_WhenDatabaseIsOnline_ShouldCreateBackup
      Restore_WhenBackupIsValid_ShouldRestoreDatabase
      CreateBackup_WhenWriteFails_ShouldCleanPartialBackup
```

### 6. Query Processor Unit Tests

```mermaid
mindmap
  root((Query Processor))
    Lexer
      Tokenize_WhenSQLIsValid_ShouldReturnTokens
      Tokenize_WhenInputContainsWhitespace_ShouldIgnoreWhitespace
      Tokenize_WhenTokenIsInvalid_ShouldThrow
    SQLParser
      Parse_WhenSelectStatementIsValid_ShouldReturnAST
      Parse_WhenStatementIsIncomplete_ShouldThrowSyntaxError
      Parse_WhenTokensAreEmpty_ShouldRejectInput
    AST
      Accept_WhenVisitorIsProvided_ShouldDispatchVisitor
      Build_WhenChildrenAreValid_ShouldPreserveTreeStructure
      Build_WhenRequiredNodeIsMissing_ShouldFail
    QueryOptimizer
      Optimize_WhenMultiplePlansExist_ShouldChooseLowestCostPlan
      Optimize_ShouldPreserveLogicalSemantics
      Optimize_WhenNoAlternativeExists_ShouldReturnOriginalPlan
    LogicalPlan
      AddOperator_WhenOperatorIsValid_ShouldUpdatePlan
      Validate_WhenOperatorInputsMatch_ShouldSucceed
      Validate_WhenSchemaDoesNotMatch_ShouldFail
    PhysicalPlan
      Build_WhenLogicalPlanIsValid_ShouldCreatePhysicalOperators
      CalculateCost_ShouldReturnEstimatedExecutionCost
      Validate_WhenOperatorIsUnsupported_ShouldFail
    QueryExecutor
      Execute_WhenPlanIsValid_ShouldReturnRows
      Execute_WhenStorageFails_ShouldPropagateFailure
      Execute_WhenTransactionFails_ShouldRollback
```

### 7. Security Management Unit Tests

```mermaid
mindmap
  root((Security Management))
    SecurityManager
      Authenticate_WhenCredentialsAreValid_ShouldReturnUser
      Authenticate_WhenCredentialsAreInvalid_ShouldFail
      Authorize_WhenPermissionIsMissing_ShouldDenyAccess
    User
      AssignRole_WhenRoleIsValid_ShouldAddRole
      AssignRole_WhenRoleAlreadyAssigned_ShouldNotDuplicate
      Disable_WhenUserIsActive_ShouldDisableUser
    Role
      AddPermission_WhenPermissionIsValid_ShouldAddPermission
      AddPermission_WhenPermissionExists_ShouldNotDuplicate
      RemovePermission_WhenPermissionExists_ShouldRemovePermission
    Permission
      Allows_WhenActionAndResourceMatch_ShouldReturnTrue
      Allows_WhenActionDoesNotMatch_ShouldReturnFalse
      Allows_WhenScopeDoesNotMatch_ShouldReturnFalse
```

### 8. Replication & Cluster Unit Tests

```mermaid
mindmap
  root((Replication & Cluster))
    ReplicationManager
      Replicate_WhenFollowerIsAvailable_ShouldSendLogRecords
      Replicate_WhenFollowerFails_ShouldRetry
      Commit_WhenQuorumIsNotReached_ShouldFail
    ClusterNode
      ReceiveHeartbeat_ShouldUpdateLastSeenTime
      MarkUnavailable_WhenHeartbeatExpires_ShouldChangeState
      Create_WhenEndpointIsInvalid_ShouldThrow
```

### 9. Monitoring Unit Tests

```mermaid
mindmap
  root((Monitoring))
    MonitoringManager
      CollectMetrics_WhenSourcesAreAvailable_ShouldReturnMetrics
      Evaluate_WhenThresholdIsExceeded_ShouldRaiseAlert
      CollectMetrics_WhenSourceFails_ShouldRecordFailure
```
