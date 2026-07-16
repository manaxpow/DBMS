const fs = require('fs');
let content = fs.readFileSync('c:\\Users\\ADMIN\\Desktop\\DBMS\\README.md', 'utf8');

// Remove the previously added Unit Tests Mapping sections
content = content.replace(/\n\n#### Unit Tests Mapping\n\n```mermaid\nflowchart LR\n[\s\S]*?```/g, '');

const components = {
    'Storage Engine': ['StorageEngine', 'FileLifecycleManager', 'PhysicalFileSystem', 'BufferPoolManager', 'ClockReplacementPolicy', 'RecordManager', 'BPlusTreeIndex'],
    'Query Processor': ['QueryProcessor', 'SqlParser', 'SemanticAnalyzer', 'LogicalPlanBuilder', 'QueryOptimizer', 'PhysicalPlanBuilder', 'QueryExecutor'],
    'Transaction Management': ['TransactionManagement', 'TransactionManager', 'RepeatableReadPolicy', 'LockManager', 'DeadlockDetector', 'ConcurrencyController'],
    'Logging Management': ['LoggingManagement', 'LogManager', 'WALProtocol', 'LogBufferManager', 'LogWriter'],
    'Recovery Management': ['RecoveryManagement', 'RecoveryManager', 'AriesRecoveryAlgorithm', 'CheckpointCoordinator', 'BackupManager', 'RestoreManager'],
    'Security Management': ['SecurityManagement', 'AuthenticationManager', 'AuthorizationManager', 'PrincipalManager', 'ConnectionManager'],
    'Database Manager': ['DatabaseManagerSystem', 'DatabaseRegistry', 'DatabaseLifecycleManager', 'DatabaseMetadataManager', 'DatabaseConfigurationManager'],
    'Database Object Management': ['DatabaseObjectManagement', 'SchemaManager', 'TableManager', 'IndexDefinitionManager', 'ViewManager', 'ConstraintManager', 'SystemCatalog'],
    'Performance Management': ['PerformanceManagement', 'PerformanceMonitor', 'QueryStatisticsCollector', 'ResourceMonitor'],
    'System Management': ['SystemManagement', 'SystemConfigurationManager', 'SystemHealthMonitor']
};

let newSection = '\n\n## Unit Tests Architecture\n';

let i = 1;
for (const [subsystem, classes] of Object.entries(components)) {
    newSection += `\n### ${i}. ${subsystem} Unit Tests\n\n`;
    newSection += '```mermaid\nflowchart LR\n';
    
    // Subsystem node
    const subNode = `Subsystem${i}`;
    newSection += `    ${subNode}["${subsystem}"]\n\n`;
    
    // Classes and Tests
    let classIndex = 1;
    for (const cls of classes) {
        const clsNode = `Class_${i}_${classIndex}`;
        const testNode = `Test_${i}_${classIndex}`;
        
        newSection += `    ${clsNode}["${cls}"]\n`;
        newSection += `    ${testNode}["${cls}Tests"]\n`;
        
        newSection += `    ${subNode} --> ${clsNode}\n`;
        newSection += `    ${clsNode} -.-> ${testNode}\n\n`;
        
        classIndex++;
    }
    
    newSection += '```\n';
    i++;
}

content += newSection;

fs.writeFileSync('c:\\Users\\ADMIN\\Desktop\\DBMS\\README.md', content, 'utf8');
