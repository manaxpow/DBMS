# Sequence Diagram - Create File

## Usecase Overview
**Actor**: `DatabaseManager`
**Purpose**: Shows how the system creates a new physical database file, initializes its structure, and persists the metadata.

---

## 1. Happy Path: Successful Creation
```mermaid
sequenceDiagram
    autonumber

    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant PFS as IPhysicalFileSystem
    participant FW as FileWriter

    DB->>LM: createFile(fileName, fileType, pageSize, initialFileSize)
    activate LM

    LM->>PFS: Exists(fileName)
    PFS-->>LM: false

    note right of LM: Generate fileId<br/>extentSize = pagesPerExtent * pageSize<br/>totalExtentCount = (initialFileSize - headerSize) / extentSize
    create participant FH as FileHeader
    LM->>FH: Create(fileId, fileType, pageSize, extentSize, formatVersion)
    activate FH
    FH-->>LM: fileHeader
    deactivate FH

    create participant AM as AllocationMetadata
    LM->>AM: Create(totalExtentCount)
    activate AM
    AM-->>LM: allocationMetadata
    deactivate AM

    create participant EB as ExtentBitmap
    LM->>EB: Create(totalExtentCount)
    activate EB
    EB-->>LM: extentBitmap
    deactivate EB

    create participant DF as DataFile
    LM->>DF: Create(fileName, fileHeader, allocationMetadata, initialFileSize, maximumSize, autoExtendEnabled)
    activate DF
    DF-->>LM: dataFile
    deactivate DF

    LM->>PFS: Create(fileName, initialFileSize)
    PFS-->>LM: fileHandle

    LM->>FW: WriteHeader(fileHandle, dataFile.Header)
    activate FW
    FW-->>LM: success
    deactivate FW

    LM->>FW: WriteAllocationMetadata(fileHandle, dataFile.Header, dataFile.AllocationMetadata)
    activate FW
    FW-->>LM: success
    deactivate FW

    LM->>FW: WriteExtentBitmap(fileHandle, dataFile.Header, dataFile.AllocationMetadata)
    activate FW
    FW-->>LM: success
    deactivate FW

    LM->>PFS: Close(fileHandle)
    LM-->>DB: dataFile
    deactivate LM
```

---

## 2. Failure Path: File Already Exists
```mermaid
sequenceDiagram
    autonumber

    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant PFS as IPhysicalFileSystem

    DB->>LM: createFile(fileName, fileType, pageSize, initialFileSize)
    activate LM

    LM->>PFS: Exists(fileName)
    PFS-->>LM: true

    LM-->>DB: FileAlreadyExistsError
    deactivate LM
```

---

## 3. Failure Path: Write Operation Failed (Rollback)
```mermaid
sequenceDiagram
    autonumber

    actor DB as DatabaseManager
    participant LM as FileLifecycleManager
    participant PFS as IPhysicalFileSystem
    participant FW as FileWriter

    DB->>LM: createFile(fileName, fileType, pageSize, initialFileSize)
    activate LM

    LM->>PFS: Exists(fileName)
    PFS-->>LM: false

    note right of LM: Generate fileId<br/>extentSize = pagesPerExtent * pageSize<br/>totalExtentCount = (initialFileSize - headerSize) / extentSize
    create participant FH as FileHeader
    LM->>FH: Create(fileId, fileType, pageSize, extentSize, formatVersion)
    activate FH
    FH-->>LM: fileHeader
    deactivate FH

    create participant AM as AllocationMetadata
    LM->>AM: Create(totalExtentCount)
    activate AM
    AM-->>LM: allocationMetadata
    deactivate AM

    create participant EB as ExtentBitmap
    LM->>EB: Create(totalExtentCount)
    activate EB
    EB-->>LM: extentBitmap
    deactivate EB

    create participant DF as DataFile
    LM->>DF: Create(fileName, fileHeader, allocationMetadata, initialFileSize, maximumSize, autoExtendEnabled)
    activate DF
    DF-->>LM: dataFile
    deactivate DF

    alt Physical creation failed
        LM->>PFS: Create(fileName, initialFileSize)
        PFS-->>LM: exception
        LM-->>DB: propagate exception
    else Initialization failed after creation
        LM->>PFS: Create(fileName, initialFileSize)
        PFS-->>LM: fileHandle

        LM->>FW: WriteHeader(fileHandle, dataFile.Header)
        activate FW
        FW-->>LM: failure
        deactivate FW

        LM->>PFS: Close(fileHandle)
        LM->>PFS: Delete(fileName)
        
        LM-->>DB: propagate original initialization exception
    end

    deactivate LM
```
> [!NOTE] 
> If `PFS: Close` or `PFS: Delete` fail during rollback, the original initialization exception is preserved as the primary failure. The cleanup failure is recorded/attached according to project exception policy. Note that an `OpenFileEntry` is NEVER registered during any failure path.

## Discovered Candidates

### Method Candidates
- `FileLifecycleManager.CreateFile(fileName: String, fileType: FileType, pageSize: int, initialFileSize: long) : DataFile`
- `IPhysicalFileSystem.Exists(fileName: String) : boolean`
- `IPhysicalFileSystem.Create(fileName: String, initialFileSize: long) : FileHandle`
- `IPhysicalFileSystem.Close(handle: FileHandle) : void`
- `IPhysicalFileSystem.Delete(fileName: String) : void`
- `FileHeader.Create(fileId: FileId, fileType: FileType, pageSize: int, extentSize: int, formatVersion: int) : FileHeader`
- `AllocationMetadata.Create(totalExtentCount: int) : AllocationMetadata`
- `ExtentBitmap.Create(totalExtents: int) : ExtentBitmap`
- `DataFile.Create(fileName: String, header: FileHeader, metadata: AllocationMetadata, currentSize: long, maximumSize: long?, autoExtendEnabled: bool) : DataFile`
- `FileWriter.WriteHeader(handle: FileHandle, header: FileHeader) : void`
- `FileWriter.WriteAllocationMetadata(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) : void`
- `FileWriter.WriteExtentBitmap(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) : void`

### State Candidates
- `FileType` enum (e.g., Table, Log, Temporary)
- `pageSize` (int)
- `initialFileSize` (long)
- `formatVersion` (int)
- `totalExtents` (int)
