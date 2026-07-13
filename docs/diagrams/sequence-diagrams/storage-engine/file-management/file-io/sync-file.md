# Sequence Diagram - Sync File

## Usecase Overview
**Actor**: `BufferManager` (or `CheckpointCoordinator`)
**Purpose**: Shows how the system flushes OS-level write buffers and triggers a hardware-level flush to ensure durability (ACID Properties).

---

## 1. Happy Path: Successful File Sync
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FS as FileSynchronizer
    participant OE as OpenFileEntry
    participant FH as FileHandle

    BM->>FS: sync(OpenFileEntry)
    activate FS
    
    FS->>OE: getState()
    activate OE
    OE-->>FS: FileState.Open
    deactivate OE
    
    FS->>OE: getFileHandle()
    activate OE
    OE-->>FS: FileHandle
    deactivate OE
    
    FS->>FH: flush()
    activate FH
    note right of FH: Flushes internal software buffers.
    FH-->>FS: success
    deactivate FH
    
    FS->>FH: sync()
    activate FH
    note right of FH: OS fsync system call to commit to persistent physical drive.
    FH-->>FS: success
    deactivate FH
    
    FS-->>BM: success
    deactivate FS
```

---

## 2. Failure Path: File State is Invalid
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FS as FileSynchronizer
    participant OE as OpenFileEntry

    BM->>FS: sync(OpenFileEntry)
    activate FS
    
    FS->>OE: getState()
    activate OE
    OE-->>FS: FileState.Closed
    deactivate OE
    
    FS-->>BM: throw FileNotOpenException
    deactivate FS
```

---

## 3. Failure Path: OS-level fsync Error
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FS as FileSynchronizer
    participant OE as OpenFileEntry
    participant FH as FileHandle

    BM->>FS: sync(OpenFileEntry)
    activate FS
    
    FS->>OE: getState()
    activate OE
    OE-->>FS: FileState.Open
    deactivate OE
    
    FS->>OE: getFileHandle()
    activate OE
    OE-->>FS: FileHandle
    deactivate OE
    
    FS->>FH: flush()
    activate FH
    FH-->>FS: success
    deactivate FH
    
    FS->>FH: sync()
    activate FH
    FH-->>FS: throw IOException
    deactivate FH
    
    FS-->>BM: throw SyncFailureException
    deactivate FS
```

---

## Discovered Candidates

### Method Candidates
- `FileSynchronizer.sync(entry: OpenFileEntry) : void`
- `OpenFileEntry.getState() : FileState`
- `OpenFileEntry.getFileHandle() : FileHandle`
- `FileHandle.flush() : void`
- `FileHandle.sync() : void`

### State Candidates
- `FileState` enum values (Open, Closed)
