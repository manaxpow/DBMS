$components = [ordered]@{
    "1. Storage Engine" = @("StorageEngine", "FileLifecycleManager", "PhysicalFileSystem", "BufferPoolManager", "ClockReplacementPolicy", "RecordManager", "BPlusTreeIndex")
    "2. Query Processor" = @("QueryProcessor", "SqlParser", "SemanticAnalyzer", "LogicalPlanBuilder", "QueryOptimizer", "PhysicalPlanBuilder", "QueryExecutor")
    "3. Transaction Management" = @("TransactionManagement", "TransactionManager", "RepeatableReadPolicy", "LockManager", "DeadlockDetector", "ConcurrencyController")
    "4. Logging Management" = @("LoggingManagement", "LogManager", "WALProtocol", "LogBufferManager", "LogWriter")
    "5. Recovery Management" = @("RecoveryManagement", "RecoveryManager", "AriesRecoveryAlgorithm", "CheckpointCoordinator", "BackupManager", "RestoreManager")
    "6. Security Management" = @("SecurityManagement", "AuthenticationManager", "AuthorizationManager", "PrincipalManager", "ConnectionManager")
    "7. Database Manager" = @("DatabaseManagerSystem", "DatabaseRegistry", "DatabaseLifecycleManager", "DatabaseMetadataManager", "DatabaseConfigurationManager")
    "8. Database Object Management" = @("DatabaseObjectManagement", "SchemaManager", "TableManager", "IndexDefinitionManager", "ViewManager", "ConstraintManager", "SystemCatalog")
    "9. Performance Management" = @("PerformanceManagement", "PerformanceMonitor", "QueryStatisticsCollector", "ResourceMonitor")
    "10. System Management" = @("SystemManagement", "SystemConfigurationManager", "SystemHealthMonitor")
}

$content = Get-Content c:\Users\ADMIN\Desktop\DBMS\README.md -Raw

foreach ($key in $components.Keys) {
    $header = "### " + $key
    $idxHeader = $content.IndexOf($header)
    if ($idxHeader -ge 0) {
        $idxFirstMermaid = $content.IndexOf("```mermaid", $idxHeader)
        if ($idxFirstMermaid -ge 0) {
            $idxEndMermaid = $content.IndexOf("```", $idxFirstMermaid + 10)
            if ($idxEndMermaid -ge 0) {
                $idxInsert = $idxEndMermaid + 3
                
                $classes = $components[$key]
                $flowchart = "`r`n`r`n#### Unit Tests Mapping`r`n`r`n```mermaid`r`nflowchart LR`r`n"
                foreach ($cls in $classes) {
                    $flowchart += "    $cls --> $($cls)Tests`r`n"
                }
                $flowchart += "```"

                $content = $content.Insert($idxInsert, $flowchart)
            }
        }
    }
}

Set-Content c:\Users\ADMIN\Desktop\DBMS\README.md -Value $content -NoNewline
