# DBMS
Database management system

```mermaid
flowchart LR
    %% Left side
    QP_SP[SQL Parser] --- QP[Query Processor]
    QP_SA[Semantic Analyzer] --- QP
    QP_LP[Logical Planner] --- QP
    QP_QO[Query Optimizer] --- QP
    QP_QE[Query Executor] --- QP
    QP --- DBMS[Database Management System]
    
    SE_FM[File Management] --- SE[Storage Engine]
    SE_PM[Page Management] --- SE
    SE_BM[Buffer Management] --- SE
    SE_RM[Record Management] --- SE
    SE_IM[Index Management] --- SE
    SE --- DBMS
    
    TM_TM[Transaction Manager] --- TM[Transaction Management]
    TM_IM[Isolation Management] --- TM
    TM_LM[Lock Management] --- TM
    TM_DM[Deadlock Management] --- TM
    TM_CC[Concurrency Controller] --- TM
    TM --- DBMS
    
    LM_LM[Log Manager] --- LM[Logging Management]
    LM_WAL[WAL Protocol] --- LM
    LM_LRM[Log Record Manager] --- LM
    LM_LSNM[Log Sequence Number Manager] --- LM
    LM_LBM[Log Buffer Manager] --- LM
    LM_LW[Log Writer] --- LM
    LM_LBlkM[Log Block Manager] --- LM
    LM_LFM[Log File Manager] --- LM
    LM_LCC[Log Checkpoint Coordinator] --- LM
    LM --- DBMS
    
    RM_RM[Recovery Manager] --- RM[Recovery Management]
    RM_LBR[Log-Based Recovery] --- RM
    RM_CM[Checkpoint Management] --- RM
    RM_BR[Backup & Restore] --- RM
    RM --- DBMS

    %% Right side
    DBMS --- SecM[Security Management]
    SecM --- SecM_AuthN[Authentication]
    SecM --- SecM_AuthZ[Authorization]
    SecM --- SecM_EM[Encryption Management]
    SecM --- SecM_CM[Connection Management]
    SecM --- SecM_Aud[Auditing]
    SecM --- SecM_PM[Principal Management]
    
    DBMS --- DM[Database Manager]
    DM --- DM_DR[Database Registry]
    DM --- DM_DL[Database Lifecycle]
    DM --- DM_DM[Database Metadata]
    DM --- DM_DC[Database Configuration]
    DM --- DM_DS[Database State]
    DM --- DM_DFM[Database File Mapping]
    DM --- DM_DIG[Database ID Generator]
    
    DBMS --- DOM[Database Object Management]
    DOM --- DOM_SM[Schema Manager]
    DOM --- DOM_TM[Table Manager]
    DOM --- DOM_IM[Index Manager]
    DOM --- DOM_VM[View Manager]
    DOM --- DOM_SPM[Stored Procedure Manager]
    DOM --- DOM_FM[Function Manager]
    DOM --- DOM_TrM[Trigger Manager]
    DOM --- DOM_SC[System Catalog]
    
    DBMS --- PM[Performance Management]
    PM --- PM_PM[Performance Monitor]
    PM --- PM_QS[Query Statistics]
    PM --- PM_RM[Resource Monitor]
    PM --- PM_CM[Cache Monitor]
    PM --- PM_SM[Storage Monitor]
    PM --- PM_PA[Performance Advisor]
    
    DBMS --- SysM[System Management]
    SysM --- SysM_CM[Configuration Management]
    SysM --- SysM_SM[System Monitoring]
    SysM --- SysM_IE[Import & Export]
```

## Feature Class Diagrams

### 1. Storage Engine
```mermaid
classDiagram
    direction LR

    class IFileLifecycleManager {
        <<interface>>
        +CreateFile(string path) FileId
        +DeleteFile(FileId fileId) void
        +OpenFile(FileId fileId) FileHandle
        +CloseFile(FileHandle handle) void
    }
    class FileLifecycleManager

    class IPhysicalFileSystem {
        <<interface>>
        +ReadBlock(DiskAddress address, byte[] buffer) void
        +WriteBlock(DiskAddress address, byte[] buffer) void
    }
    class PhysicalFileSystem

    class IBufferPoolManager {
        <<interface>>
        +FetchPage(PageId pageId) Page
        +UnpinPage(PageId pageId, bool isDirty) void
        +FlushPage(PageId pageId) void
        +NewPage(FileId fileId) Page
        +DeletePage(PageId pageId) void
    }
    class BufferPoolManager

    class IPageReplacementPolicy {
        <<interface>>
        +Pin(FrameId frameId) void
        +Unpin(FrameId frameId) void
        +Victim() FrameId
    }
    class ClockReplacementPolicy

    class IRecordManager {
        <<interface>>
        +InsertRecord(Record record) RecordId
        +GetRecord(RecordId recordId) Record
        +UpdateRecord(RecordId recordId, Record record) void
        +DeleteRecord(RecordId recordId) void
    }
    class RecordManager

    class IIndex {
        <<interface>>
        +Insert(IndexKey key, RecordPointer ptr) void
        +Delete(IndexKey key) void
        +Search(IndexKey key) RecordPointer
    }
    class BPlusTreeIndex

    IFileLifecycleManager <|-- FileLifecycleManager
    IPhysicalFileSystem <|-- PhysicalFileSystem
    IBufferPoolManager <|-- BufferPoolManager
    IPageReplacementPolicy <|-- ClockReplacementPolicy
    IRecordManager <|-- RecordManager
    IIndex <|-- BPlusTreeIndex

    BufferPoolManager --> IPageReplacementPolicy
```

### 2. Query Processor
```mermaid
classDiagram
    direction LR

    class ISqlParser {
        <<interface>>
        +Parse(string sql) SqlStatement
    }
    class SqlParser

    class ISemanticAnalyzer {
        <<interface>>
        +Analyze(SqlStatement statement, SemanticContext ctx) BoundStatement
    }
    class SemanticAnalyzer

    class ILogicalPlanBuilder {
        <<interface>>
        +Build(BoundStatement statement) LogicalPlan
    }
    class LogicalPlanBuilder

    class IQueryOptimizer {
        <<interface>>
        +Optimize(LogicalPlan plan) LogicalPlan
    }
    class QueryOptimizer

    class IPhysicalPlanBuilder {
        <<interface>>
        +Build(LogicalPlan logicalPlan) PhysicalPlan
    }
    class PhysicalPlanBuilder

    class IQueryExecutor {
        <<interface>>
        +Execute(PhysicalPlan plan, ExecutionContext ctx) QueryResult
    }
    class QueryExecutor

    ISqlParser <|-- SqlParser
    ISemanticAnalyzer <|-- SemanticAnalyzer
    ILogicalPlanBuilder <|-- LogicalPlanBuilder
    IQueryOptimizer <|-- QueryOptimizer
    IPhysicalPlanBuilder <|-- PhysicalPlanBuilder
    IQueryExecutor <|-- QueryExecutor

    QueryExecutor --> PhysicalPlanBuilder
    QueryOptimizer --> LogicalPlanBuilder
```

### 3. Transaction Management
```mermaid
classDiagram
    direction LR

    class ITransactionManager {
        <<interface>>
        +BeginTransaction(IsolationLevel level) TransactionId
        +CommitTransaction(TransactionId txId) void
        +RollbackTransaction(TransactionId txId) void
        +GetTransactionState(TransactionId txId) TransactionState
    }
    class TransactionManager

    class IIsolationPolicy {
        <<interface>>
        +EnforceReadRules(TransactionId txId, LockResource res) void
        +EnforceWriteRules(TransactionId txId, LockResource res) void
    }
    class RepeatableReadPolicy

    class ILockManager {
        <<interface>>
        +AcquireLock(TransactionId txId, LockResource res, LockMode mode) bool
        +ReleaseLock(TransactionId txId, LockResource res) void
        +UpgradeLock(TransactionId txId, LockResource res, LockMode newMode) bool
    }
    class LockManager

    class IDeadlockDetector {
        <<interface>>
        +DetectDeadlock() DeadlockCycle
        +ResolveDeadlock(DeadlockCycle cycle) TransactionId
    }
    class DeadlockDetector

    class IConcurrencyController {
        <<interface>>
        +CheckAccess(TransactionId txId, OperationAccess access) bool
    }
    class ConcurrencyController

    ITransactionManager <|-- TransactionManager
    IIsolationPolicy <|-- RepeatableReadPolicy
    ILockManager <|-- LockManager
    IDeadlockDetector <|-- DeadlockDetector
    IConcurrencyController <|-- ConcurrencyController

    TransactionManager --> ILockManager
    LockManager --> IDeadlockDetector
```

### 4. Logging Management
```mermaid
classDiagram
    direction LR

    class ILogManager {
        <<interface>>
        +AppendLog(LogRecord record) LogSequenceNumber
        +FlushToLSN(LogSequenceNumber lsn) void
        +GetLogRecord(LogSequenceNumber lsn) LogRecord
    }
    class LogManager

    class IWALProtocol {
        <<interface>>
        +EnsureWAL(LogSequenceNumber pageLsn) void
    }
    class WALProtocol

    class ILogBufferManager {
        <<interface>>
        +WriteToBuffer(LogRecord record) void
        +FlushBuffer() void
    }
    class LogBufferManager

    class ILogWriter {
        <<interface>>
        +WriteBlock(LogBlock block) void
        +Sync() void
    }
    class LogWriter

    ILogManager <|-- LogManager
    IWALProtocol <|-- WALProtocol
    ILogBufferManager <|-- LogBufferManager
    ILogWriter <|-- LogWriter

    LogManager --> IWALProtocol
    LogManager --> ILogBufferManager
    LogBufferManager --> ILogWriter
```

### 5. Recovery Management
```mermaid
classDiagram
    direction LR

    class IRecoveryManager {
        <<interface>>
        +RecoverDatabase() void
        +UndoTransaction(TransactionId txId) void
    }
    class RecoveryManager

    class ILogBasedRecovery {
        <<interface>>
        +PerformAnalysis() RecoveryAnalysisPhase
        +PerformRedo() void
        +PerformUndo() void
    }
    class AriesRecoveryAlgorithm

    class ICheckpointCoordinator {
        <<interface>>
        +CreateCheckpoint() CheckpointId
        +GetLatestCheckpoint() CheckpointMetadata
    }
    class CheckpointCoordinator

    class IBackupManager {
        <<interface>>
        +CreateFullBackup(string destination) BackupId
        +CreateIncrementalBackup(string destination) BackupId
    }
    class BackupManager

    class IRestoreManager {
        <<interface>>
        +RestoreFromBackup(BackupId backupId) void
    }
    class RestoreManager

    IRecoveryManager <|-- RecoveryManager
    ILogBasedRecovery <|-- AriesRecoveryAlgorithm
    ICheckpointCoordinator <|-- CheckpointCoordinator
    IBackupManager <|-- BackupManager
    IRestoreManager <|-- RestoreManager

    RecoveryManager --> ILogBasedRecovery
    RecoveryManager --> ICheckpointCoordinator
```

### 6. Security Management
```mermaid
classDiagram
    direction LR

    class IAuthenticationManager {
        <<interface>>
        +Authenticate(Credential cred) AuthenticationResult
        +CreateLoginSession(UserId userId) SessionId
    }
    class AuthenticationManager

    class IAuthorizationManager {
        <<interface>>
        +CheckPermission(UserId userId, SecuredResource res, Privilege priv) bool
        +GrantPermission(UserId userId, SecuredResource res, Privilege priv) void
    }
    class AuthorizationManager

    class IPrincipalManager {
        <<interface>>
        +CreateUser(string username, string password) UserId
        +AssignRole(UserId userId, RoleId roleId) void
    }
    class PrincipalManager

    class IConnectionManager {
        <<interface>>
        +OpenConnection(ConnectionContext ctx) ConnectionId
        +CloseConnection(ConnectionId connId) void
        +GetActiveConnections() List~ConnectionId~
    }
    class ConnectionManager

    IAuthenticationManager <|-- AuthenticationManager
    IAuthorizationManager <|-- AuthorizationManager
    IPrincipalManager <|-- PrincipalManager
    IConnectionManager <|-- ConnectionManager
```

### 7. Database Manager
```mermaid
classDiagram
    direction LR

    class IDatabaseRegistry {
        <<interface>>
        +RegisterDatabase(DatabaseDescriptor desc) void
        +UnregisterDatabase(DatabaseId dbId) void
        +GetDatabase(DatabaseId dbId) DatabaseDescriptor
    }
    class DatabaseRegistry

    class IDatabaseLifecycleManager {
        <<interface>>
        +CreateDatabase(string name) DatabaseId
        +DropDatabase(DatabaseId dbId) void
        +StartDatabase(DatabaseId dbId) void
        +StopDatabase(DatabaseId dbId) void
    }
    class DatabaseLifecycleManager

    class IDatabaseMetadataManager {
        <<interface>>
        +GetMetadata(DatabaseId dbId) DatabaseMetadata
        +UpdateMetadata(DatabaseId dbId, DatabaseMetadata meta) void
    }
    class DatabaseMetadataManager

    class IDatabaseConfigurationManager {
        <<interface>>
        +LoadConfiguration(DatabaseId dbId) DatabaseConfiguration
        +SaveConfiguration(DatabaseId dbId, DatabaseConfiguration config) void
    }
    class DatabaseConfigurationManager

    IDatabaseRegistry <|-- DatabaseRegistry
    IDatabaseLifecycleManager <|-- DatabaseLifecycleManager
    IDatabaseMetadataManager <|-- DatabaseMetadataManager
    IDatabaseConfigurationManager <|-- DatabaseConfigurationManager

    DatabaseLifecycleManager --> IDatabaseRegistry
```

### 8. Database Object Management
```mermaid
classDiagram
    direction LR

    class ISchemaManager {
        <<interface>>
        +CreateSchema(string name, UserId ownerId) SchemaId
        +DropSchema(SchemaId schemaId) void
    }
    class SchemaManager

    class ITableManager {
        <<interface>>
        +CreateTable(SchemaId schemaId, TableDefinition def) TableId
        +DropTable(TableId tableId) void
        +AlterTable(TableId tableId, TableDefinition newDef) void
    }
    class TableManager

    class IIndexDefinitionManager {
        <<interface>>
        +CreateIndex(TableId tableId, IndexDefinition def) IndexId
        +DropIndex(IndexId indexId) void
    }
    class IndexDefinitionManager

    class IViewManager {
        <<interface>>
        +CreateView(SchemaId schemaId, ViewDefinition def) ViewId
        +DropView(ViewId viewId) void
    }
    class ViewManager

    class ISystemCatalog {
        <<interface>>
        +GetTableDefinition(TableId tableId) TableDefinition
        +GetIndexDefinition(IndexId indexId) IndexDefinition
        +InvalidateCache(CatalogObjectId id) void
    }
    class SystemCatalog

    ISchemaManager <|-- SchemaManager
    ITableManager <|-- TableManager
    IIndexDefinitionManager <|-- IndexDefinitionManager
    IViewManager <|-- ViewManager
    ISystemCatalog <|-- SystemCatalog

    TableManager --> ISystemCatalog
    IndexDefinitionManager --> ISystemCatalog
```

### 9. Performance Management
```mermaid
classDiagram
    direction LR

    class IPerformanceMonitor {
        <<interface>>
        +StartMonitoring() void
        +StopMonitoring() void
        +GetSnapshot() PerformanceSnapshot
    }
    class PerformanceMonitor

    class IQueryStatisticsCollector {
        <<interface>>
        +RecordQueryExecution(QueryExecutionStatistics stats) void
        +GetSlowQueries(TimeSpan threshold) List~QueryExecutionStatistics~
    }
    class QueryStatisticsCollector

    class IResourceMonitor {
        <<interface>>
        +GetCpuUsage() double
        +GetMemoryUsage() double
        +GetDiskIO() StorageStatistics
    }
    class ResourceMonitor

    class IPerformanceAdvisor {
        <<interface>>
        +AnalyzeWorkload() List~PerformanceRecommendation~
        +SuggestIndexes() List~MissingIndexRecommendationRule~
    }
    class PerformanceAdvisor

    IPerformanceMonitor <|-- PerformanceMonitor
    IQueryStatisticsCollector <|-- QueryStatisticsCollector
    IResourceMonitor <|-- ResourceMonitor
    IPerformanceAdvisor <|-- PerformanceAdvisor

    PerformanceMonitor --> IResourceMonitor
    PerformanceAdvisor --> IQueryStatisticsCollector
```

### 10. System Management
```mermaid
classDiagram
    direction LR

    class ISystemConfigurationManager {
        <<interface>>
        +GetSetting(string key) ConfigurationValue
        +UpdateSetting(string key, ConfigurationValue val) void
        +LoadGlobalConfig() ConfigurationSnapshot
    }
    class SystemConfigurationManager

    class ISystemHealthMonitor {
        <<interface>>
        +PerformHealthCheck() SystemHealthReport
        +RegisterHealthCheck(IHealthCheck check) void
    }
    class SystemHealthMonitor

    class IImportManager {
        <<interface>>
        +ImportData(ImportRequest request) ImportResult
        +ValidateImportPlan(ImportPlan plan) bool
    }
    class ImportManager

    class IExportManager {
        <<interface>>
        +ExportData(ExportRequest request) ExportResult
    }
    class ExportManager

    ISystemConfigurationManager <|-- SystemConfigurationManager
    ISystemHealthMonitor <|-- SystemHealthMonitor
    IImportManager <|-- ImportManager
    IExportManager <|-- ExportManager
```
