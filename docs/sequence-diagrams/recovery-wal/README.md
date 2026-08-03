# Recovery & WAL Unit Test Sequence Diagrams

## 1. WAL Tests

### 1.1 Append_WhenRecordIsValid_ShouldAssignLSN

```mermaid
sequenceDiagram
    autonumber
    participant Test as WALTests
    participant WAL as WAL

    Test->>WAL: Append(record)
    activate WAL
    WAL->>WAL: GenerateNextLSN()
    WAL-->>WAL: newLSN
    WAL->>WAL: record.LSN = newLSN
    WAL->>WAL: _logBuffer.Add(record)
    WAL-->>Test: newLSN
    deactivate WAL
```

### 1.2 Append_WhenWriteFails_ShouldNotAdvanceDurableLSN

```mermaid
sequenceDiagram
    autonumber
    participant Test as WALTests
    participant WAL as WAL
    participant FM as FileManager

    Test->>WAL: Append(record)
    activate WAL
    WAL->>WAL: GenerateNextLSN()
    WAL-->>WAL: newLSN
    WAL->>WAL: record.LSN = newLSN
    WAL->>FM: Write(record)
    activate FM
    FM-->>WAL: throws IOException
    deactivate FM
    WAL->>WAL: RevertNextLSN(newLSN)
    WAL-->>Test: throws IOException
    deactivate WAL
```

### 1.3 Flush_WhenTargetLSNExists_ShouldPersistRecords

```mermaid
sequenceDiagram
    autonumber
    participant Test as WALTests
    participant WAL as WAL
    participant FM as FileManager

    Test->>WAL: Flush(targetLSN)
    activate WAL
    WAL->>WAL: Check targetLSN <= _flushedLSN
    WAL-->>WAL: false
    WAL->>FM: Flush(_logBuffer.GetRecordsUpTo(targetLSN))
    activate FM
    FM-->>WAL: success
    deactivate FM
    WAL->>WAL: _flushedLSN = targetLSN
    WAL-->>Test: success
    deactivate WAL
```

### 1.4 Flush_WhenTargetLSNIsAlreadyDurable_ShouldDoNothing

```mermaid
sequenceDiagram
    autonumber
    participant Test as WALTests
    participant WAL as WAL

    Test->>WAL: Flush(targetLSN)
    activate WAL
    WAL->>WAL: Check targetLSN <= _flushedLSN
    WAL-->>WAL: true
    WAL-->>Test: success (No-op)
    deactivate WAL
```

### 1.5 Flush_WhenTargetLSNDoesNotExist_ShouldThrow

```mermaid
sequenceDiagram
    autonumber
    participant Test as WALTests
    participant WAL as WAL

    Test->>WAL: Flush(targetLSN)
    activate WAL
    WAL->>WAL: Check targetLSN > _currentLSN
    WAL-->>WAL: true
    WAL-->>Test: throws InvalidLSNException
    deactivate WAL
```

## 2. Recovery Tests

### 2.1 Recover_ShouldRedoCommittedTransactions

```mermaid
sequenceDiagram
    autonumber
    participant Test as RecoveryTests
    participant Rec as Recovery
    participant WAL as WAL
    participant SE as StorageEngine

    Test->>Rec: Recover()
    activate Rec
    Rec->>WAL: ReadAllRecords()
    WAL-->>Rec: logRecords
    Rec->>Rec: Identify committed transactions
    Rec-->>Rec: [tx1, tx2]
    loop For each REDO record of committed transactions
        Rec->>SE: ApplyRedo(record)
        activate SE
        SE-->>Rec: success
        deactivate SE
    end
    Rec-->>Test: success
    deactivate Rec
```

### 2.2 Recover_ShouldUndoUncommittedTransactions

```mermaid
sequenceDiagram
    autonumber
    participant Test as RecoveryTests
    participant Rec as Recovery
    participant WAL as WAL
    participant SE as StorageEngine

    Test->>Rec: Recover()
    activate Rec
    Rec->>WAL: ReadAllRecords()
    WAL-->>Rec: logRecords
    Rec->>Rec: Identify uncommitted transactions
    Rec-->>Rec: [tx3]
    loop Reverse order of UNDO records of uncommitted transactions
        Rec->>SE: ApplyUndo(record)
        activate SE
        SE-->>Rec: success
        deactivate SE
    end
    Rec-->>Test: success
    deactivate Rec
```

### 2.3 Recover_WhenLogIsEmpty_ShouldCompleteSuccessfully

```mermaid
sequenceDiagram
    autonumber
    participant Test as RecoveryTests
    participant Rec as Recovery
    participant WAL as WAL

    Test->>Rec: Recover()
    activate Rec
    Rec->>WAL: ReadAllRecords()
    WAL-->>Rec: empty list
    Rec-->>Test: success
    deactivate Rec
```

### 2.4 Recover_WhenLogRecordIsCorrupted_ShouldFailSafely

```mermaid
sequenceDiagram
    autonumber
    participant Test as RecoveryTests
    participant Rec as Recovery
    participant WAL as WAL

    Test->>Rec: Recover()
    activate Rec
    Rec->>WAL: ReadAllRecords()
    WAL-->>Rec: throws ChecksumMismatchException (Corrupted Log)
    Rec-->>Test: throws RecoveryFailureException
    deactivate Rec
```

### 2.5 Recover_WhenRedoFails_ShouldNotReportSuccessfulRecovery

```mermaid
sequenceDiagram
    autonumber
    participant Test as RecoveryTests
    participant Rec as Recovery
    participant WAL as WAL
    participant SE as StorageEngine

    Test->>Rec: Recover()
    activate Rec
    Rec->>WAL: ReadAllRecords()
    WAL-->>Rec: logRecords
    Rec->>Rec: Identify committed transactions
    loop For each REDO record
        Rec->>SE: ApplyRedo(record)
        activate SE
        SE-->>Rec: throws StorageFailureException
        deactivate SE
    end
    Rec-->>Test: throws RecoveryFailureException
    deactivate Rec
```
