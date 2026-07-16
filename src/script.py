import re

components = {
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
}

with open(r'c:\Users\ADMIN\Desktop\DBMS\README.md', 'r', encoding='utf-8') as f:
    content = f.read()

for key, classes in components.items():
    header_pattern = r'(### ' + re.escape(key) + r'[\s\S]*?`mermaid[\s\S]*?`)'
    
    flowchart = "\n\n#### Unit Tests Mapping\n\n`mermaid\nflowchart LR\n"
    for cls in classes:
        flowchart += f"    {cls} --> {cls}Tests\n"
    flowchart += "`"
    
    content = re.sub(header_pattern, r'\1' + flowchart, content, count=1)

with open(r'c:\Users\ADMIN\Desktop\DBMS\README.md', 'w', encoding='utf-8', newline='') as f:
    f.write(content)
