const fs = require('fs');

const newContent = `### 2. Query Processor
\`\`\`mermaid
classDiagram
    direction LR

    class QueryProcessor {
        +Initialize() void
        +ProcessQuery(string sql) QueryResult
    }

    class ISqlParser {
        <<interface>>
        +Parse(string sql) SqlStatement
    }
    class SqlParser {
        -Lexer lexer
        +BuildAST() ASTNode
    }

    class ISemanticAnalyzer {
        <<interface>>
        +Analyze(SqlStatement statement, SemanticContext ctx) BoundStatement
    }
    class SemanticAnalyzer {
        -Catalog catalog
        +ValidateTypes() void
    }

    class ILogicalPlanBuilder {
        <<interface>>
        +Build(BoundStatement statement) LogicalPlan
    }
    class LogicalPlanBuilder {
        +CreateOperators() void
    }

    class IQueryOptimizer {
        <<interface>>
        +Optimize(LogicalPlan plan) LogicalPlan
    }
    class QueryOptimizer {
        -OptimizationRuleSet rules
        +ApplyRules() void
    }

    class IPhysicalPlanBuilder {
        <<interface>>
        +Build(LogicalPlan logicalPlan) PhysicalPlan
    }
    class PhysicalPlanBuilder {
        +SelectAccessPath() void
    }

    class IQueryExecutor {
        <<interface>>
        +Execute(PhysicalPlan plan, ExecutionContext ctx) QueryResult
    }
    class QueryExecutor {
        -OperatorExecutorFactory factory
        +RunPipeline() void
    }

    ISqlParser <|-- SqlParser
    ISemanticAnalyzer <|-- SemanticAnalyzer
    ILogicalPlanBuilder <|-- LogicalPlanBuilder
    IQueryOptimizer <|-- QueryOptimizer
    IPhysicalPlanBuilder <|-- PhysicalPlanBuilder
    IQueryExecutor <|-- QueryExecutor

    QueryProcessor *-- ISqlParser
    QueryProcessor *-- ISemanticAnalyzer
    QueryProcessor *-- ILogicalPlanBuilder
    QueryProcessor *-- IQueryOptimizer
    QueryProcessor *-- IPhysicalPlanBuilder
    QueryProcessor *-- IQueryExecutor

    SemanticAnalyzer --> ISqlParser : Uses (Implicit)
    LogicalPlanBuilder --> ISemanticAnalyzer : Uses
    QueryOptimizer --> ILogicalPlanBuilder : Uses
    PhysicalPlanBuilder --> IQueryOptimizer : Uses
    QueryExecutor --> IPhysicalPlanBuilder : Uses
\`\`\`

### 3. Transaction Management
\`\`\`mermaid
classDiagram
    direction LR

    class TransactionManagement {
        +Initialize() void
        +Shutdown() void
    }

    class ITransactionManager {
        <<interface>>
        +BeginTransaction(IsolationLevel level) TransactionId
        +CommitTransaction(TransactionId txId) void
        +RollbackTransaction(TransactionId txId) void
        +GetTransactionState(TransactionId txId) TransactionState
    }
    class TransactionManager {
        -TransactionRegistry registry
        +CreateContext() TransactionContext
    }

    class IIsolationPolicy {
        <<interface>>
        +EnforceReadRules(TransactionId txId, LockResource res) void
        +EnforceWriteRules(TransactionId txId, LockResource res) void
    }
    class RepeatableReadPolicy {
        +CheckSnapshot() void
    }

    class ILockManager {
        <<interface>>
        +AcquireLock(TransactionId txId, LockResource res, LockMode mode) bool
        +ReleaseLock(TransactionId txId, LockResource res) void
        +UpgradeLock(TransactionId txId, LockResource res, LockMode newMode) bool
    }
    class LockManager {
        -LockTable lockTable
        +DetectWaiters() void
    }

    class IDeadlockDetector {
        <<interface>>
        +DetectDeadlock() DeadlockCycle
        +ResolveDeadlock(DeadlockCycle cycle) TransactionId
    }
    class DeadlockDetector {
        -WaitForGraph wfg
        +SelectVictim() void
    }

    class IConcurrencyController {
        <<interface>>
        +CheckAccess(TransactionId txId, OperationAccess access) bool
    }
    class ConcurrencyController {
        +ResolveLocks() void
    }

    ITransactionManager <|-- TransactionManager
    IIsolationPolicy <|-- RepeatableReadPolicy
    ILockManager <|-- LockManager
    IDeadlockDetector <|-- DeadlockDetector
    IConcurrencyController <|-- ConcurrencyController

    TransactionManagement *-- ITransactionManager
    TransactionManagement *-- ILockManager
    TransactionManagement *-- IDeadlockDetector
    TransactionManagement *-- IConcurrencyController

    TransactionManager --> ILockManager : Uses
    TransactionManager --> IIsolationPolicy : Uses
    LockManager --> IDeadlockDetector : Triggers
    ConcurrencyController --> ILockManager : Uses
\`\`\`

### 4. Logging Management
\`\`\`mermaid
classDiagram
    direction LR

    class LoggingManagement {
        +Initialize() void
        +Shutdown() void
    }

    class ILogManager {
        <<interface>>
        +AppendLog(LogRecord record) LogSequenceNumber
        +FlushToLSN(LogSequenceNumber lsn) void
        +GetLogRecord(LogSequenceNumber lsn) LogRecord
    }
    class LogManager {
        -LogSequenceNumberGenerator lsnGen
        +CreateAppendRequest() void
    }

    class IWALProtocol {
        <<interface>>
        +EnsureWAL(LogSequenceNumber pageLsn) void
    }
    class WALProtocol {
        +ValidatePageLSN() void
    }

    class ILogBufferManager {
        <<interface>>
        +WriteToBuffer(LogRecord record) void
        +FlushBuffer() void
    }
    class LogBufferManager {
        -LogBuffer buffer
        +RotateBuffer() void
    }

    class ILogWriter {
        <<interface>>
        +WriteBlock(LogBlock block) void
        +Sync() void
    }
    class LogWriter {
        -DurableLSNTracker tracker
        +PerformIO() void
    }

    ILogManager <|-- LogManager
    IWALProtocol <|-- WALProtocol
    ILogBufferManager <|-- LogBufferManager
    ILogWriter <|-- LogWriter

    LoggingManagement *-- ILogManager
    LoggingManagement *-- IWALProtocol
    LoggingManagement *-- ILogBufferManager
    LoggingManagement *-- ILogWriter

    LogManager --> IWALProtocol : Uses
    LogManager --> ILogBufferManager : Uses
    LogBufferManager --> ILogWriter : Uses
\`\`\`

### 5. Recovery Management
\`\`\`mermaid
classDiagram
    direction LR

    class RecoveryManagement {
        +Initialize() void
        +StartRecovery() void
    }

    class IRecoveryManager {
        <<interface>>
        +RecoverDatabase() void
        +UndoTransaction(TransactionId txId) void
    }
    class RecoveryManager {
        -RecoveryContext ctx
        +AnalyzeState() void
    }

    class ILogBasedRecovery {
        <<interface>>
        +PerformAnalysis() RecoveryAnalysisPhase
        +PerformRedo() void
        +PerformUndo() void
    }
    class AriesRecoveryAlgorithm {
        -TransactionRecoveryTable trt
        -DirtyPageTable dpt
    }

    class ICheckpointCoordinator {
        <<interface>>
        +CreateCheckpoint() CheckpointId
        +GetLatestCheckpoint() CheckpointMetadata
    }
    class CheckpointCoordinator {
        -CheckpointWriter writer
        +FlushDirtyPages() void
    }

    class IBackupManager {
        <<interface>>
        +CreateFullBackup(string destination) BackupId
        +CreateIncrementalBackup(string destination) BackupId
    }
    class BackupManager {
        -BackupPlanner planner
        +WriteManifest() void
    }

    class IRestoreManager {
        <<interface>>
        +RestoreFromBackup(BackupId backupId) void
    }
    class RestoreManager {
        -RestoreValidator validator
        +ApplyLogs() void
    }

    IRecoveryManager <|-- RecoveryManager
    ILogBasedRecovery <|-- AriesRecoveryAlgorithm
    ICheckpointCoordinator <|-- CheckpointCoordinator
    IBackupManager <|-- BackupManager
    IRestoreManager <|-- RestoreManager

    RecoveryManagement *-- IRecoveryManager
    RecoveryManagement *-- ILogBasedRecovery
    RecoveryManagement *-- ICheckpointCoordinator
    RecoveryManagement *-- IBackupManager
    RecoveryManagement *-- IRestoreManager

    RecoveryManager --> ILogBasedRecovery : Uses
    RecoveryManager --> ICheckpointCoordinator : Uses
    RestoreManager --> ILogBasedRecovery : Uses
\`\`\`

### 6. Security Management
\`\`\`mermaid
classDiagram
    direction LR

    class SecurityManagement {
        +Initialize() void
        +EnforceSecurity() void
    }

    class IAuthenticationManager {
        <<interface>>
        +Authenticate(Credential cred) AuthenticationResult
        +CreateLoginSession(UserId userId) SessionId
    }
    class AuthenticationManager {
        -PasswordHasher hasher
        +ValidateToken() void
    }

    class IAuthorizationManager {
        <<interface>>
        +CheckPermission(UserId userId, SecuredResource res, Privilege priv) bool
        +GrantPermission(UserId userId, SecuredResource res, Privilege priv) void
    }
    class AuthorizationManager {
        -PermissionEvaluator evaluator
        +ResolveRoles() void
    }

    class IPrincipalManager {
        <<interface>>
        +CreateUser(string username, string password) UserId
        +AssignRole(UserId userId, RoleId roleId) void
    }
    class PrincipalManager {
        -PrincipalRepository repo
        +UpdateStatus() void
    }

    class IConnectionManager {
        <<interface>>
        +OpenConnection(ConnectionContext ctx) ConnectionId
        +CloseConnection(ConnectionId connId) void
        +GetActiveConnections() List~ConnectionId~
    }
    class ConnectionManager {
        -ConnectionRegistry registry
        +LimitConnections() void
    }

    IAuthenticationManager <|-- AuthenticationManager
    IAuthorizationManager <|-- AuthorizationManager
    IPrincipalManager <|-- PrincipalManager
    IConnectionManager <|-- ConnectionManager

    SecurityManagement *-- IAuthenticationManager
    SecurityManagement *-- IAuthorizationManager
    SecurityManagement *-- IPrincipalManager
    SecurityManagement *-- IConnectionManager

    AuthenticationManager --> IPrincipalManager : Uses
    AuthorizationManager --> IPrincipalManager : Uses
    ConnectionManager --> IAuthenticationManager : Uses
\`\`\`

### 7. Database Manager
\`\`\`mermaid
classDiagram
    direction LR

    class DatabaseManagerSystem {
        +Initialize() void
        +GetDatabases() List~DatabaseId~
    }

    class IDatabaseRegistry {
        <<interface>>
        +RegisterDatabase(DatabaseDescriptor desc) void
        +UnregisterDatabase(DatabaseId dbId) void
        +GetDatabase(DatabaseId dbId) DatabaseDescriptor
    }
    class DatabaseRegistry {
        -DatabaseLookupService lookup
        +ResolveName() void
    }

    class IDatabaseLifecycleManager {
        <<interface>>
        +CreateDatabase(string name) DatabaseId
        +DropDatabase(DatabaseId dbId) void
        +StartDatabase(DatabaseId dbId) void
        +StopDatabase(DatabaseId dbId) void
    }
    class DatabaseLifecycleManager {
        -DatabaseBootstrapper bootstrapper
        +CoordinateStartup() void
    }

    class IDatabaseMetadataManager {
        <<interface>>
        +GetMetadata(DatabaseId dbId) DatabaseMetadata
        +UpdateMetadata(DatabaseId dbId, DatabaseMetadata meta) void
    }
    class DatabaseMetadataManager {
        -DatabaseMetadataRepository repo
        +SyncVersion() void
    }

    class IDatabaseConfigurationManager {
        <<interface>>
        +LoadConfiguration(DatabaseId dbId) DatabaseConfiguration
        +SaveConfiguration(DatabaseId dbId, DatabaseConfiguration config) void
    }
    class DatabaseConfigurationManager {
        -DatabaseConfigurationLoader loader
        +ValidateConfig() void
    }

    IDatabaseRegistry <|-- DatabaseRegistry
    IDatabaseLifecycleManager <|-- DatabaseLifecycleManager
    IDatabaseMetadataManager <|-- DatabaseMetadataManager
    IDatabaseConfigurationManager <|-- DatabaseConfigurationManager

    DatabaseManagerSystem *-- IDatabaseRegistry
    DatabaseManagerSystem *-- IDatabaseLifecycleManager
    DatabaseManagerSystem *-- IDatabaseMetadataManager
    DatabaseManagerSystem *-- IDatabaseConfigurationManager

    DatabaseLifecycleManager --> IDatabaseRegistry : Uses
    DatabaseLifecycleManager --> IDatabaseMetadataManager : Uses
    DatabaseLifecycleManager --> IDatabaseConfigurationManager : Uses
\`\`\`

### 8. Database Object Management
\`\`\`mermaid
classDiagram
    direction LR

    class DatabaseObjectManagement {
        +Initialize() void
        +ResolveObject() void
    }

    class ISchemaManager {
        <<interface>>
        +CreateSchema(string name, UserId ownerId) SchemaId
        +DropSchema(SchemaId schemaId) void
    }
    class SchemaManager {
        -SystemCatalog catalog
        +ValidateSchema() void
    }

    class ITableManager {
        <<interface>>
        +CreateTable(SchemaId schemaId, TableDefinition def) TableId
        +DropTable(TableId tableId) void
        +AlterTable(TableId tableId, TableDefinition newDef) void
    }
    class TableManager {
        -SystemCatalog catalog
        +CheckConstraints() void
    }

    class IIndexDefinitionManager {
        <<interface>>
        +CreateIndex(TableId tableId, IndexDefinition def) IndexId
        +DropIndex(IndexId indexId) void
    }
    class IndexDefinitionManager {
        +ValidateIndexColumns() void
    }

    class IViewManager {
        <<interface>>
        +CreateView(SchemaId schemaId, ViewDefinition def) ViewId
        +DropView(ViewId viewId) void
    }
    class ViewManager {
        +CompileView() void
    }

    class ISystemCatalog {
        <<interface>>
        +GetTableDefinition(TableId tableId) TableDefinition
        +GetIndexDefinition(IndexId indexId) IndexDefinition
        +InvalidateCache(CatalogObjectId id) void
    }
    class SystemCatalog {
        -CatalogCache cache
        +FlushToDisk() void
    }

    ISchemaManager <|-- SchemaManager
    ITableManager <|-- TableManager
    IIndexDefinitionManager <|-- IndexDefinitionManager
    IViewManager <|-- ViewManager
    ISystemCatalog <|-- SystemCatalog

    DatabaseObjectManagement *-- ISchemaManager
    DatabaseObjectManagement *-- ITableManager
    DatabaseObjectManagement *-- IIndexDefinitionManager
    DatabaseObjectManagement *-- IViewManager
    DatabaseObjectManagement *-- ISystemCatalog

    SchemaManager --> ISystemCatalog : Uses
    TableManager --> ISystemCatalog : Uses
    IndexDefinitionManager --> ISystemCatalog : Uses
    ViewManager --> ISystemCatalog : Uses
\`\`\`

### 9. Performance Management
\`\`\`mermaid
classDiagram
    direction LR

    class PerformanceManagement {
        +Initialize() void
        +GenerateReport() void
    }

    class IPerformanceMonitor {
        <<interface>>
        +StartMonitoring() void
        +StopMonitoring() void
        +GetSnapshot() PerformanceSnapshot
    }
    class PerformanceMonitor {
        -PerformanceMonitorScheduler scheduler
        +AggregateMetrics() void
    }

    class IQueryStatisticsCollector {
        <<interface>>
        +RecordQueryExecution(QueryExecutionStatistics stats) void
        +GetSlowQueries(TimeSpan threshold) List~QueryExecutionStatistics~
    }
    class QueryStatisticsCollector {
        -QueryStatisticsRepository repo
        +DetectSlowQueries() void
    }

    class IResourceMonitor {
        <<interface>>
        +GetCpuUsage() double
        +GetMemoryUsage() double
        +GetDiskIO() StorageStatistics
    }
    class ResourceMonitor {
        -CpuMonitor cpu
        -MemoryMonitor memory
        +SampleResources() void
    }

    class IPerformanceAdvisor {
        <<interface>>
        +AnalyzeWorkload() List~PerformanceRecommendation~
        +SuggestIndexes() List~MissingIndexRecommendationRule~
    }
    class PerformanceAdvisor {
        -RecommendationEngine engine
        +EvaluateRules() void
    }

    IPerformanceMonitor <|-- PerformanceMonitor
    IQueryStatisticsCollector <|-- QueryStatisticsCollector
    IResourceMonitor <|-- ResourceMonitor
    IPerformanceAdvisor <|-- PerformanceAdvisor

    PerformanceManagement *-- IPerformanceMonitor
    PerformanceManagement *-- IQueryStatisticsCollector
    PerformanceManagement *-- IResourceMonitor
    PerformanceManagement *-- IPerformanceAdvisor

    PerformanceMonitor --> IResourceMonitor : Uses
    PerformanceAdvisor --> IQueryStatisticsCollector : Uses
    PerformanceAdvisor --> IPerformanceMonitor : Uses
\`\`\`

### 10. System Management
\`\`\`mermaid
classDiagram
    direction LR

    class SystemManagement {
        +Initialize() void
        +ShutdownSystem() void
    }

    class ISystemConfigurationManager {
        <<interface>>
        +GetSetting(string key) ConfigurationValue
        +UpdateSetting(string key, ConfigurationValue val) void
        +LoadGlobalConfig() ConfigurationSnapshot
    }
    class SystemConfigurationManager {
        -ConfigurationLoader loader
        +MergeConfigs() void
    }

    class ISystemHealthMonitor {
        <<interface>>
        +PerformHealthCheck() SystemHealthReport
        +RegisterHealthCheck(IHealthCheck check) void
    }
    class SystemHealthMonitor {
        -List~IHealthCheck~ checks
        +EvaluateStatus() void
    }

    class IImportManager {
        <<interface>>
        +ImportData(ImportRequest request) ImportResult
        +ValidateImportPlan(ImportPlan plan) bool
    }
    class ImportManager {
        -ImportPlanner planner
        +ParseFormat() void
    }

    class IExportManager {
        <<interface>>
        +ExportData(ExportRequest request) ExportResult
    }
    class ExportManager {
        -ExportPlanner planner
        +FormatOutput() void
    }

    ISystemConfigurationManager <|-- SystemConfigurationManager
    ISystemHealthMonitor <|-- SystemHealthMonitor
    IImportManager <|-- ImportManager
    IExportManager <|-- ExportManager

    SystemManagement *-- ISystemConfigurationManager
    SystemManagement *-- ISystemHealthMonitor
    SystemManagement *-- IImportManager
    SystemManagement *-- IExportManager

    SystemHealthMonitor --> ISystemConfigurationManager : Uses
    ImportManager --> ISystemConfigurationManager : Uses
\`\`\`
`;

const readmePath = 'README.md';
let content = fs.readFileSync(readmePath, 'utf8');

const marker = '### 2. Query Processor';
const idx = content.indexOf(marker);

if (idx !== -1) {
    content = content.substring(0, idx) + newContent;
    fs.writeFileSync(readmePath, content, 'utf8');
    console.log('Successfully updated README.md');
} else {
    console.error('Marker not found in README.md');
}
