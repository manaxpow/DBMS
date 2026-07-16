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

## Architecture Class Diagram

```mermaid
classDiagram
    class DatabaseManagementSystem {
        <<System>>
    }
    class QueryProcessor {
        <<Component>>
    }
    class StorageEngine {
        <<Component>>
    }
    class TransactionManagement {
        <<Component>>
    }
    class LoggingManagement {
        <<Component>>
    }
    class RecoveryManagement {
        <<Component>>
    }
    class SecurityManagement {
        <<Component>>
    }
    class DatabaseManager {
        <<Component>>
    }
    class DatabaseObjectManagement {
        <<Component>>
    }
    class PerformanceManagement {
        <<Component>>
    }
    class SystemManagement {
        <<Component>>
    }

    DatabaseManagementSystem *-- QueryProcessor
    DatabaseManagementSystem *-- StorageEngine
    DatabaseManagementSystem *-- TransactionManagement
    DatabaseManagementSystem *-- LoggingManagement
    DatabaseManagementSystem *-- RecoveryManagement
    DatabaseManagementSystem *-- SecurityManagement
    DatabaseManagementSystem *-- DatabaseManager
    DatabaseManagementSystem *-- DatabaseObjectManagement
    DatabaseManagementSystem *-- PerformanceManagement
    DatabaseManagementSystem *-- SystemManagement
```

## Component Level Flowcharts

### 1. Query Processor Flowchart

```mermaid
flowchart TD
    QP[QueryProcessor]
    
    QP_Parse[ParseSQL]
    QP_Analyze[AnalyzeSemantics]
    QP_Plan[CreateLogicalPlan]
    QP_Opt[OptimizeQuery]
    QP_Exec[ExecuteQuery]

    QP --> QP_Parse
    QP --> QP_Analyze
    QP --> QP_Plan
    QP --> QP_Opt
    QP --> QP_Exec

    QP_Parse_Test1([ParseSQL_ValidSyntax_ReturnsAST])
    QP_Parse_Test2([ParseSQL_InvalidSyntax_ThrowsException])
    QP_Parse --> QP_Parse_Test1
    QP_Parse --> QP_Parse_Test2

    QP_Analyze_Test1([AnalyzeSemantics_ValidObjects_Passes])
    QP_Analyze_Test2([AnalyzeSemantics_MissingTable_ThrowsException])
    QP_Analyze --> QP_Analyze_Test1
    QP_Analyze --> QP_Analyze_Test2

    QP_Plan_Test1([CreateLogicalPlan_ValidAST_ReturnsPlan])
    QP_Plan --> QP_Plan_Test1

    QP_Opt_Test1([OptimizeQuery_GivenPlan_ReturnsOptimizedPlan])
    QP_Opt --> QP_Opt_Test1

    QP_Exec_Test1([ExecuteQuery_ValidPlan_ReturnsResult])
    QP_Exec --> QP_Exec_Test1
```

### 2. Storage Engine Flowchart

```mermaid
flowchart TD
    SE[StorageEngine]

    SE_CreateFile[CreateDataFile]
    SE_OpenFile[OpenDataFile]
    SE_ReadPage[ReadPageData]
    SE_WritePage[WritePageData]
    SE_AllocExtent[AllocateExtent]

    SE --> SE_CreateFile
    SE --> SE_OpenFile
    SE --> SE_ReadPage
    SE --> SE_WritePage
    SE --> SE_AllocExtent

    SE_CreateFile_Test1([CreateDataFile_ValidParams_Success])
    SE_CreateFile_Test2([CreateDataFile_AlreadyExists_Throws])
    SE_CreateFile --> SE_CreateFile_Test1
    SE_CreateFile --> SE_CreateFile_Test2

    SE_OpenFile_Test1([OpenDataFile_ExistingFile_Success])
    SE_OpenFile_Test2([OpenDataFile_NotFound_Throws])
    SE_OpenFile --> SE_OpenFile_Test1
    SE_OpenFile --> SE_OpenFile_Test2

    SE_ReadPage_Test1([ReadPageData_ValidId_ReturnsData])
    SE_ReadPage_Test2([ReadPageData_InvalidId_Throws])
    SE_ReadPage --> SE_ReadPage_Test1
    SE_ReadPage --> SE_ReadPage_Test2

    SE_WritePage_Test1([WritePageData_ValidId_Success])
    SE_WritePage --> SE_WritePage_Test1

    SE_AllocExtent_Test1([AllocateExtent_HasSpace_ReturnsExtent])
    SE_AllocExtent --> SE_AllocExtent_Test1
```

### 3. Transaction Management Flowchart

```mermaid
flowchart TD
    TM[TransactionManagement]

    TM_Begin[BeginTransaction]
    TM_Commit[CommitTransaction]
    TM_Roll[RollbackTransaction]
    TM_Lock[AcquireLock]

    TM --> TM_Begin
    TM --> TM_Commit
    TM --> TM_Roll
    TM --> TM_Lock

    TM_Begin_Test1([BeginTransaction_ReturnsNewId])
    TM_Begin --> TM_Begin_Test1

    TM_Commit_Test1([CommitTransaction_ActiveTx_SavesChanges])
    TM_Commit_Test2([CommitTransaction_InactiveTx_Throws])
    TM_Commit --> TM_Commit_Test1
    TM_Commit --> TM_Commit_Test2

    TM_Roll_Test1([RollbackTransaction_ActiveTx_RevertsChanges])
    TM_Roll --> TM_Roll_Test1

    TM_Lock_Test1([AcquireLock_ResourceFree_GrantsLock])
    TM_Lock_Test2([AcquireLock_ResourceBusy_WaitsOrTimesOut])
    TM_Lock --> TM_Lock_Test1
    TM_Lock --> TM_Lock_Test2
```

### 4. Logging Management Flowchart

```mermaid
flowchart TD
    LM[LoggingManagement]

    LM_Write[WriteLogRecord]
    LM_Flush[FlushLogBuffer]
    LM_Check[PerformCheckpoint]

    LM --> LM_Write
    LM --> LM_Flush
    LM --> LM_Check

    LM_Write_Test1([WriteLogRecord_ValidData_AppendsToBuffer])
    LM_Write --> LM_Write_Test1

    LM_Flush_Test1([FlushLogBuffer_HasData_WritesToDisk])
    LM_Flush --> LM_Flush_Test1

    LM_Check_Test1([PerformCheckpoint_SavesState_Success])
    LM_Check --> LM_Check_Test1
```

### 5. Recovery Management Flowchart

```mermaid
flowchart TD
    RM[RecoveryManagement]

    RM_Recover[RecoverFromLog]
    RM_Undo[UndoTransaction]
    RM_Redo[RedoTransaction]

    RM --> RM_Recover
    RM --> RM_Undo
    RM --> RM_Redo

    RM_Recover_Test1([RecoverFromLog_ValidLog_RestoresState])
    RM_Recover_Test2([RecoverFromLog_CorruptedLog_ThrowsException])
    RM_Recover --> RM_Recover_Test1
    RM_Recover --> RM_Recover_Test2

    RM_Undo_Test1([UndoTransaction_ActiveTx_RevertsActions])
    RM_Undo --> RM_Undo_Test1

    RM_Redo_Test1([RedoTransaction_CommittedTx_ReappliesActions])
    RM_Redo --> RM_Redo_Test1
```

### 6. Security Management Flowchart

```mermaid
flowchart TD
    SecM[SecurityManagement]

    SecM_Auth[AuthenticateUser]
    SecM_Authz[AuthorizeAction]
    SecM_Audit[LogAuditTrail]

    SecM --> SecM_Auth
    SecM --> SecM_Authz
    SecM --> SecM_Audit

    SecM_Auth_Test1([AuthenticateUser_ValidCreds_ReturnsToken])
    SecM_Auth_Test2([AuthenticateUser_InvalidCreds_Throws])
    SecM_Auth --> SecM_Auth_Test1
    SecM_Auth --> SecM_Auth_Test2

    SecM_Authz_Test1([AuthorizeAction_HasPermission_ReturnsTrue])
    SecM_Authz_Test2([AuthorizeAction_NoPermission_ReturnsFalse])
    SecM_Authz --> SecM_Authz_Test1
    SecM_Authz --> SecM_Authz_Test2

    SecM_Audit_Test1([LogAuditTrail_ActionLogged_Success])
    SecM_Audit --> SecM_Audit_Test1
```

### 7. Database Manager Flowchart

```mermaid
flowchart TD
    DM[DatabaseManager]

    DM_Create[CreateDatabase]
    DM_Drop[DropDatabase]
    DM_Start[StartDatabase]
    DM_Stop[StopDatabase]

    DM --> DM_Create
    DM --> DM_Drop
    DM --> DM_Start
    DM --> DM_Stop

    DM_Create_Test1([CreateDatabase_ValidName_Success])
    DM_Create_Test2([CreateDatabase_Exists_Throws])
    DM_Create --> DM_Create_Test1
    DM_Create --> DM_Create_Test2

    DM_Drop_Test1([DropDatabase_Existing_Success])
    DM_Drop --> DM_Drop_Test1

    DM_Start_Test1([StartDatabase_StoppedDb_Success])
    DM_Start --> DM_Start_Test1

    DM_Stop_Test1([StopDatabase_RunningDb_Success])
    DM_Stop --> DM_Stop_Test1
```

### 8. Database Object Management Flowchart

```mermaid
flowchart TD
    DOM[DatabaseObjectManagement]

    DOM_CreateTable[CreateTable]
    DOM_DropTable[DropTable]
    DOM_CreateIndex[CreateIndex]

    DOM --> DOM_CreateTable
    DOM --> DOM_DropTable
    DOM --> DOM_CreateIndex

    DOM_CreateTable_Test1([CreateTable_ValidSchema_Success])
    DOM_CreateTable_Test2([CreateTable_InvalidSchema_ThrowsException])
    DOM_CreateTable --> DOM_CreateTable_Test1
    DOM_CreateTable --> DOM_CreateTable_Test2

    DOM_DropTable_Test1([DropTable_ExistingTable_Success])
    DOM_DropTable --> DOM_DropTable_Test1

    DOM_CreateIndex_Test1([CreateIndex_ValidColumns_Success])
    DOM_CreateIndex --> DOM_CreateIndex_Test1
```

### 9. Performance Management Flowchart

```mermaid
flowchart TD
    PM[PerformanceManagement]

    PM_Collect[CollectMetrics]
    PM_Analyze[AnalyzeQueryStats]

    PM --> PM_Collect
    PM --> PM_Analyze

    PM_Collect_Test1([CollectMetrics_UpdatesStats_Success])
    PM_Collect --> PM_Collect_Test1

    PM_Analyze_Test1([AnalyzeQueryStats_ReturnsReport_Success])
    PM_Analyze --> PM_Analyze_Test1
```

### 10. System Management Flowchart

```mermaid
flowchart TD
    SysM[SystemManagement]

    SysM_Update[UpdateConfig]
    SysM_Export[ExportData]

    SysM --> SysM_Update
    SysM --> SysM_Export

    SysM_Update_Test1([UpdateConfig_ValidConfig_AppliesChanges])
    SysM_Update_Test2([UpdateConfig_InvalidConfig_ThrowsException])
    SysM_Update --> SysM_Update_Test1
    SysM_Update --> SysM_Update_Test2

    SysM_Export_Test1([ExportData_ValidPath_CreatesFile])
    SysM_Export --> SysM_Export_Test1
```
