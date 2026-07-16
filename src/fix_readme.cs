using System;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

class Program
{
    static void Main()
    {
        var components = new Dictionary<string, string[]>
        {
            { "1. Storage Engine", new[] { "StorageEngine", "FileLifecycleManager", "PhysicalFileSystem", "BufferPoolManager", "ClockReplacementPolicy", "RecordManager", "BPlusTreeIndex" } },
            { "2. Query Processor", new[] { "QueryProcessor", "SqlParser", "SemanticAnalyzer", "LogicalPlanBuilder", "QueryOptimizer", "PhysicalPlanBuilder", "QueryExecutor" } },
            { "3. Transaction Management", new[] { "TransactionManagement", "TransactionManager", "RepeatableReadPolicy", "LockManager", "DeadlockDetector", "ConcurrencyController" } },
            { "4. Logging Management", new[] { "LoggingManagement", "LogManager", "WALProtocol", "LogBufferManager", "LogWriter" } },
            { "5. Recovery Management", new[] { "RecoveryManagement", "RecoveryManager", "AriesRecoveryAlgorithm", "CheckpointCoordinator", "BackupManager", "RestoreManager" } },
            { "6. Security Management", new[] { "SecurityManagement", "AuthenticationManager", "AuthorizationManager", "PrincipalManager", "ConnectionManager" } },
            { "7. Database Manager", new[] { "DatabaseManagerSystem", "DatabaseRegistry", "DatabaseLifecycleManager", "DatabaseMetadataManager", "DatabaseConfigurationManager" } },
            { "8. Database Object Management", new[] { "DatabaseObjectManagement", "SchemaManager", "TableManager", "IndexDefinitionManager", "ViewManager", "ConstraintManager", "SystemCatalog" } },
            { "9. Performance Management", new[] { "PerformanceManagement", "PerformanceMonitor", "QueryStatisticsCollector", "ResourceMonitor" } },
            { "10. System Management", new[] { "SystemManagement", "SystemConfigurationManager", "SystemHealthMonitor" } }
        };

        string path = @"c:\Users\ADMIN\Desktop\DBMS\README.md";
        string content = File.ReadAllText(path);

        foreach (var kvp in components)
        {
            string header = Regex.Escape(kvp.Key);
            string pattern = @"(?s)(### " + header + @".*?`mermaid.*?`)";
            
            string flowchart = "\n\n#### Unit Tests Mapping\n\n`mermaid\nflowchart LR\n";
            foreach (var cls in kvp.Value)
            {
                flowchart += $"    {cls} --> {cls}Tests\n";
            }
            flowchart += "``";
            
            content = Regex.Replace(content, pattern, m => m.Groups[1].Value + flowchart);
        }

        File.WriteAllText(path, content);
    }
}
