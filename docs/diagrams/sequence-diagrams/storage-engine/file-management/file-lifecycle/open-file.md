# Sequence Diagram - Open File

## Usecase Overview
**Actor**: `DatabaseManager`
**Purpose**: Shows how the system opens a physical database file, validates its format, initializes the in-memory `DataFile` representation, and registers an active `OpenFileEntry` to manage handle concurrency.

---

## 1. Happy Path: File Open (First Time)
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant PFS as IPhysicalFileSystem
    participant FR as FileReader
    participant FV as FileValidator

    DB->>LM: openFile(fileName, accessMode, lockMode)
    activate LM
    
    LM->>OM: getOpenFile(fileName)
    activate OM
    OM-->>LM: null
    deactivate OM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: true
    
    LM->>PFS: Open(fileName, accessMode)
    PFS-->>LM: fileHandle
    
    LM->>FR: readHeader(fileHandle)
    activate FR
    FR-->>LM: fileHeader
    deactivate FR
    
    LM->>FR: readAllocationMetadata(fileHandle, fileHeader)
    activate FR
    FR-->>LM: allocationMetadata
    deactivate FR
    
    LM->>FR: readExtentBitmap(fileHandle, fileHeader, allocationMetadata)
    activate FR
    FR-->>LM: extentBitmap
    deactivate FR
    
    LM->>PFS: GetSize(fileHandle)
    PFS-->>LM: physicalFileSize

    LM->>FV: Validate(fileHeader, allocationMetadata, extentBitmap, physicalFileSize)
    activate FV
    FV-->>LM: success
    deactivate FV

    note right of LM: Retrieve maximumSize and autoExtendEnabled<br/>from configuration or persistent defaults
    create participant DF as DataFile
    LM->>DF: Reconstruct(fileName, fileHeader, allocationMetadata, physicalFileSize, maximumSize, autoExtendEnabled)
    activate DF
    DF-->>LM: dataFile
    deactivate DF
    
    create participant OE as OpenFileEntry
    LM->>OE: create(fileHandle, dataFile, accessMode, lockMode)
    activate OE
    OE-->>LM: openFileEntry
    deactivate OE
    
    LM->>OM: registerOpenFile(fileName, openFileEntry)
    activate OM
    OM-->>LM: success
    deactivate OM
    
    LM-->>DB: openFileEntry
    deactivate LM
```

---

## 2. Happy Path: File Already Opened
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant OE as OpenFileEntry

    DB->>LM: openFile(fileName, accessMode, lockMode)
    activate LM
    
    LM->>OM: getOpenFile(fileName)
    activate OM
    OM-->>LM: openFileEntry
    deactivate OM
    
    LM->>OE: getAccessMode()
    activate OE
    OE-->>LM: existingAccessMode
    deactivate OE
    
    LM->>OE: getLockMode()
    activate OE
    OE-->>LM: existingLockMode
    deactivate OE
    
    note right of LM: Validate mode compatibility
    
    LM->>OE: incrementRefCount()
    activate OE
    OE-->>LM: newRefCount
    deactivate OE
    
    LM-->>DB: openFileEntry
    deactivate LM
```

---

## 3. Failure Path: File Not Found
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant PFS as IPhysicalFileSystem

    DB->>LM: openFile(fileName, accessMode, lockMode)
    activate LM
    
    LM->>OM: getOpenFile(fileName)
    activate OM
    OM-->>LM: null
    deactivate OM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: false
    
    LM-->>DB: throw FileNotFoundException
    deactivate LM
```

---

## 4. Failure Path: Invalid or Corrupted File
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant PFS as IPhysicalFileSystem
    participant FR as FileReader
    participant FV as FileValidator

    DB->>LM: openFile(fileName, accessMode, lockMode)
    activate LM
    
    LM->>OM: getOpenFile(fileName)
    activate OM
    OM-->>LM: null
    deactivate OM
    
    LM->>PFS: Exists(fileName)
    PFS-->>LM: true
    
    LM->>PFS: Open(fileName, accessMode)
    PFS-->>LM: fileHandle
    
    LM->>FR: readHeader(fileHandle)
    activate FR
    FR-->>LM: fileHeader
    deactivate FR

    LM->>FR: readAllocationMetadata(fileHandle, fileHeader)
    activate FR
    FR-->>LM: allocationMetadata
    deactivate FR
    
    LM->>FR: readExtentBitmap(fileHandle, fileHeader, allocationMetadata)
    activate FR
    FR-->>LM: extentBitmap
    deactivate FR

    LM->>PFS: GetSize(fileHandle)
    PFS-->>LM: physicalFileSize
    
    LM->>FV: Validate(fileHeader, allocationMetadata, extentBitmap, physicalFileSize)
    activate FV
    FV-->>LM: throw CorruptedFileException
    deactivate FV
    
    LM->>PFS: Close(fileHandle)
    
    LM-->>DB: throw CorruptedFileException
    deactivate LM
```
> [!NOTE] 
> If any step fails after `PFS.Open` succeeds (e.g., `ReadHeader`, `ReadAllocationMetadata`, `GetSize`, `Validate`), `PFS.Close(handle)` is immediately called, the entry is NOT registered, and the exception is propagated.

---

## 5. Failure Path: Mode Mismatch or Lock Conflict
```mermaid
sequenceDiagram
    autonumber
    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant OM as OpenFileManager
    participant OE as OpenFileEntry

    DB->>LM: openFile(fileName, accessMode, lockMode)
    activate LM
    
    LM->>OM: getOpenFile(fileName)
    activate OM
    OM-->>LM: openFileEntry
    deactivate OM
    
    LM->>OE: getAccessMode()
    activate OE
    OE-->>LM: existingAccessMode
    deactivate OE
    
    LM->>OE: getLockMode()
    activate OE
    OE-->>LM: existingLockMode
    deactivate OE
    
    note right of LM: Validate mode compatibility<br/>Validation fails (e.g. exclusive lock conflict).
    
    LM-->>DB: throw LockConflictException
    deactivate LM
```

---

## Concurrency & Implementation Requirements
1. **Atomic Check-and-Register**: To prevent race conditions where Request A and Request B concurrently check for the open file in `OpenFileManager` and both register different entries, the check-and-register block must be atomic or locked.
2. **Atomic Reference Counting**: In `OpenFileEntry.incrementRefCount()`, the update to the reference count must be thread-safe or atomic (e.g. using atomic integers or locks).
3. **No Redundant Open/Read**: When a file is already opened, the physical file is not reopened or reread; the existing in-memory `OpenFileEntry` is reused directly.

---

## Discovered Candidates

### Method Candidates
- `FileLifecycleManager.openFile(fileName: String, accessMode: FileAccessMode, lockMode: FileLockMode) : OpenFileEntry`
- `IPhysicalFileSystem.Exists(fileName: String) : boolean`
- `IPhysicalFileSystem.Open(fileName: String, accessMode: FileAccessMode) : FileHandle`
- `IPhysicalFileSystem.Close(handle: FileHandle) : void`
- `IPhysicalFileSystem.GetSize(handle: FileHandle) : long`
- `OpenFileManager.getOpenFile(fileName: String) : OpenFileEntry`
- `OpenFileManager.registerOpenFile(fileName: String, entry: OpenFileEntry) : void`
- `FileReader.readHeader(handle: FileHandle) : FileHeader`
- `FileReader.readAllocationMetadata(handle: FileHandle, header: FileHeader) : AllocationMetadata`
- `FileReader.readExtentBitmap(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) : ExtentBitmap`
- `DataFile.Reconstruct(fileName: String, header: FileHeader, metadata: AllocationMetadata, currentSize: long, maximumSize: long?, autoExtendEnabled: bool) : DataFile`
- `OpenFileEntry.create(handle: FileHandle, file: DataFile, accessMode: FileAccessMode, lockMode: FileLockMode) : OpenFileEntry`
- `OpenFileEntry.getAccessMode() : FileAccessMode`
- `OpenFileEntry.getLockMode() : FileLockMode`
- `OpenFileEntry.incrementRefCount() : int`
- `FileValidator.Validate(header: FileHeader, metadata: AllocationMetadata, bitmap: ExtentBitmap, physicalFileSize: long) : void`

### State Candidates
- `FileAccessMode` enum (ReadOnly, ReadWrite)
- `FileLockMode` enum (Shared, Exclusive, None)
- `OpenFileEntry` attributes:
  - `Handle: FileHandle`
  - `DataFile: DataFile`
  - `AccessMode: FileAccessMode`
  - `LockMode: FileLockMode`
  - `ReferenceCount: int`
