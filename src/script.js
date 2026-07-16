const fs = require('fs');

const components = {
    '1. Storage Engine': ['StorageEngine', 'FileLifecycleManager', 'PhysicalFileSystem', 'BufferPoolManager', 'ClockReplacementPolicy', 'RecordManager', 'BPlusTreeIndex'],
    '2. Query Processor': ['QueryProcessor', 'SqlParser', 'SemanticAnalyzer', 'LogicalPlanBuilder', 'QueryOptimizer', 'PhysicalPlanBuilder', 'QueryExecutor'],
    '3. Transaction Management': ['TransactionManagement', 'TransactionManager', 'RepeatableReadPolicy', 'LockManager', 'DeadlockDetector', 'ConcurrencyController'],
    '4. Logging Management': ['LoggingManagement', 'LogManager', 'WALProtocol', 'LogBufferManager', 'LogWriter'],
    '5. Recovery Management': ['RecoveryManagement', 'RecoveryManager', 'AriesRecoveryAlgorithm', 'CheckpointCoordinator', 'BackupManager', 'RestoreManager'],
    '6. Security Management': ['SecurityManagement', 'AuthenticationManager', 'AuthorizationManager', 'PrincipalManager', 'ConnectionManager'],
    '7. Database Manager': ['DatabaseManagerSystem', 'DatabaseRegistry', 'DatabaseLifecycleManager', 'DatabaseMetadataManager', 'DatabaseConfigurationManager'],
    '8. Database Object Management': ['DatabaseObjectManagement', 'SchemaManager', 'TableManager', 'IndexDefinitionManager', 'ViewManager', 'ConstraintManager', 'SystemCatalog'],
    '9. Performance Management': ['PerformanceManagement', 'PerformanceMonitor', 'QueryStatisticsCollector', 'ResourceMonitor'],
    '10. System Management': ['SystemManagement', 'SystemConfigurationManager', 'SystemHealthMonitor']
};

let content = fs.readFileSync('c:\\Users\\ADMIN\\Desktop\\DBMS\\README.md', 'utf8');

for (const [key, classes] of Object.entries(components)) {
    const escapedKey = key.replace(/[-\/\\^$*+?.()|[\]{}]/g, '\\$&');
    const regex = new RegExp('(### ' + escapedKey + '[\\s\\S]*?```mermaid[\\s\\S]*?```)');
    
    let flowchart = '\n\n#### Unit Tests Mapping\n\n```mermaid\nflowchart LR\n';
    for (const cls of classes) {
        flowchart += `    ${cls} --> ${cls}Tests\n`;
    }
    flowchart += '```';
    
    content = content.replace(regex, `$1${flowchart}`);
}

fs.writeFileSync('c:\\Users\\ADMIN\\Desktop\\DBMS\\README.md', content, 'utf8');
