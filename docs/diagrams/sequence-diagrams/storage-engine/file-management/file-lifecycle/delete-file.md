# Sequence Diagram - Delete File

## Usecase Overview
**Actor**: `DatabaseManager`
**Purpose**: Shows how the system safely deletes a physical database file, ensuring it is not currently open or locked by the system.

---

## 1. Happy Path: Successful Deletion
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant PFS as IPhysicalFileSystem

    DB->>LM: deleteFile(fileName)
    activate LM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: true

    LM->>OM: tryBeginDelete(fileName)
    activate OM
    OM-->>LM: true
    deactivate OM
    
    LM->>PFS: Delete(fileName)
    PFS-->>LM: success
    
    LM->>OM: completeDelete(fileName)
    activate OM
    OM-->>LM: success
    deactivate OM
    
    LM-->>DB: success
    deactivate LM
```
> [!NOTE] 
> A race condition where `Exists` returns true but the file disappears before `Delete` is called is acceptable. `Exists` acts as a friendly check, but the physical `Delete` operation defines the final outcome.

---

## 2. Failure Path: File is Open / In Use
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant PFS as IPhysicalFileSystem

    DB->>LM: deleteFile(fileName)
    activate LM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: true

    LM->>OM: tryBeginDelete(fileName)
    activate OM
    OM-->>LM: false
    deactivate OM
    
    LM-->>DB: throw FileInUseException
    deactivate LM
```
> [!IMPORTANT]
> If `tryBeginDelete` returns `false`, no deletion reservation is acquired. Therefore, `PFS.Delete`, `completeDelete`, and `cancelDelete` MUST NOT be called.

---

## 3. Failure Path: File Does Not Exist
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant PFS as IPhysicalFileSystem

    DB->>LM: deleteFile(fileName)
    activate LM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: false
    
    LM-->>DB: throw FileNotFoundException
    deactivate LM
```

---

## 4. Failure Path: Physical Deletion Fails
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant PFS as IPhysicalFileSystem

    DB->>LM: deleteFile(fileName)
    activate LM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: true

    LM->>OM: tryBeginDelete(fileName)
    activate OM
    OM-->>LM: true
    deactivate OM
    
    LM->>PFS: Delete(fileName)
    PFS-->>LM: throw Exception
    
    LM->>OM: cancelDelete(fileName)
    activate OM
    OM-->>LM: success
    deactivate OM
    
    LM-->>DB: throw Exception
    deactivate LM
```

---

## Concurrency & Implementation Requirements
1. **Atomic Deletion Coordination**: To prevent race conditions where a thread deletes a file while another concurrent thread is attempting to open it, `OpenFileManager` coordinates deletions atomically. Once `tryBeginDelete` returns `true`, the file transitions to a locking `Deleting` state internally. Any incoming `openFile()` request is rejected until the deletion is either completed or canceled.
2. **Encapsulated Manager State**: The temporary `Deleting` state remains an implementation detail internal to `OpenFileManager`. It is not exposed to the public domain model.

---

## Discovered Candidates

### Method Candidates
- `FileLifecycleManager.deleteFile(fileName: String) : void`
- `IPhysicalFileSystem.Exists(fileName: String) : boolean`
- `IPhysicalFileSystem.Delete(fileName: String) : void`
- `OpenFileManager.tryBeginDelete(fileName: String) : boolean`
- `OpenFileManager.completeDelete(fileName: String) : void`
- `OpenFileManager.cancelDelete(fileName: String) : void`

### State Candidates
- None (When deleted, the physical file is removed and there is no active domain state to maintain).
