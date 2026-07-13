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
    participant FW as FileWriter

    DB->>LM: createFile(fileName, fileType, pageSize, initialFileSize)
    activate LM

    LM->>LM: checkFileExists(fileName)
    LM-->>LM: false

    create participant FH as FileHeader
    LM->>FH: create(fileType, pageSize, formatVersion)
    activate FH
    FH-->>LM: fileHeader
    deactivate FH

    create participant AM as AllocationMetadata
    LM->>AM: create(initialFileSize, pageSize)
    activate AM
    AM-->>LM: allocationMetadata
    deactivate AM

    create participant EB as ExtentBitmap
    LM->>EB: create(allocationMetadata.totalExtents)
    activate EB
    EB-->>LM: extentBitmap
    deactivate EB

    create participant DF as DataFile
    LM->>DF: create(fileName, fileType, fileHeader, allocationMetadata, extentBitmap)
    activate DF
    DF-->>LM: dataFile
    deactivate DF

    LM->>LM: createPhysicalFile(fileName, initialFileSize)
    LM-->>LM: fileHandle

    LM->>FW: writeHeader(fileHandle, dataFile.header)
    activate FW
    FW-->>LM: success
    deactivate FW

    LM->>FW: writeAllocationMetadata(fileHandle, dataFile.allocationMetadata)
    activate FW
    FW-->>LM: success
    deactivate FW

    LM->>FW: writeExtentBitmap(fileHandle, dataFile.extentBitmap)
    activate FW
    FW-->>LM: success
    deactivate FW

    LM->>LM: closePhysicalFile(fileHandle)
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

    DB->>LM: createFile(fileName, fileType, pageSize, initialFileSize)
    activate LM

    LM->>LM: checkFileExists(fileName)
    LM-->>LM: true

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
    participant FW as FileWriter

    DB->>LM: createFile(fileName, fileType, pageSize, initialFileSize)
    activate LM

    LM->>LM: checkFileExists(fileName)
    LM-->>LM: false

    create participant FH as FileHeader
    LM->>FH: create(fileType, pageSize, formatVersion)
    activate FH
    FH-->>LM: fileHeader
    deactivate FH

    create participant AM as AllocationMetadata
    LM->>AM: create(initialFileSize, pageSize)
    activate AM
    AM-->>LM: allocationMetadata
    deactivate AM

    create participant EB as ExtentBitmap
    LM->>EB: create(allocationMetadata.totalExtents)
    activate EB
    EB-->>LM: extentBitmap
    deactivate EB

    create participant DF as DataFile
    LM->>DF: create(fileName, fileType, fileHeader, allocationMetadata, extentBitmap)
    activate DF
    DF-->>LM: dataFile
    deactivate DF

    LM->>LM: createPhysicalFile(fileName, initialFileSize)
    LM-->>LM: fileHandle

    LM->>FW: writeHeader(fileHandle, dataFile.header)
    activate FW
    FW-->>LM: failure
    deactivate FW

    LM->>LM: closePhysicalFile(fileHandle)
    LM->>LM: deletePhysicalFile(fileName)
    
    LM-->>DB: FileCreationError
    deactivate LM
```

## Discovered Candidates

### Method Candidates
- `FileLifecycleManager.createFile(fileName: String, fileType: FileType, pageSize: int, initialFileSize: long) : DataFile`
- `FileLifecycleManager.checkFileExists(fileName: String) : boolean`
- `FileLifecycleManager.createPhysicalFile(fileName: String, initialFileSize: long) : FileHandle`
- `FileLifecycleManager.closePhysicalFile(handle: FileHandle) : void`
- `FileLifecycleManager.deletePhysicalFile(fileName: String) : void`
- `FileHeader.create(fileType: FileType, pageSize: int, formatVersion: int) : FileHeader`
- `AllocationMetadata.create(initialFileSize: long, pageSize: int) : AllocationMetadata`
- `ExtentBitmap.create(totalExtents: int) : ExtentBitmap`
- `DataFile.create(fileName: String, fileType: FileType, header: FileHeader, metadata: AllocationMetadata, extentBitmap: ExtentBitmap) : DataFile`
- `FileWriter.writeHeader(handle: FileHandle, header: FileHeader) : void`
- `FileWriter.writeAllocationMetadata(handle: FileHandle, metadata: AllocationMetadata) : void`
- `FileWriter.writeExtentBitmap(handle: FileHandle, bitmap: ExtentBitmap) : void`
- `FileWriter.flush(handle: FileHandle) : void`

### State Candidates
- `FileType` enum (e.g., Table, Log, Temporary)
- `pageSize` (int)
- `initialFileSize` (long)
- `formatVersion` (int)
- `totalExtents` (int)

