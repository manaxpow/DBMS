# Sequence Diagram - Write File

## Usecase Overview
**Actor**: `BufferManager`
**Purpose**: Shows how the system writes modified pages/blocks from memory to the open physical data file.

---

## 1. Happy Path: Successful Page Write
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FW as FileWriter
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant FH as FileHandle

    BM->>FW: writePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    FW->>FW: validateAccessMode(accessMode)
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    FW->>FW: validatePageId(pageId, dataFile)
    
    FW->>OE: get Handle
    OE-->>FW: fileHandle
    
    FW->>DF: Header
    activate DF
    DF-->>FW: fileHeader
    deactivate DF
    
    FW->>FW: calculatePageOffset(pageId, fileHeader)
    
    FW->>FH: write(offset, pageSize, source)
    activate FH
    FH-->>FW: bytesWritten
    deactivate FH
    
    FW->>FW: validateBytesWritten(bytesWritten, pageSize)
    
    FW-->>BM: success
    deactivate FW
```

---

## 2. Failure Path: Write Lock / Read-Only Violation
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FW as FileWriter
    participant OE as OpenFileEntry

    BM->>FW: writePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    FW->>FW: validateAccessMode(accessMode)
    note right of FW: AccessMode is ReadOnly.
    FW-->>FW: throw ReadOnlyFileException
    
    FW-->>BM: throw ReadOnlyFileException
    deactivate FW
```

---

## 3. Failure Path: Invalid PageId
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FW as FileWriter
    participant OE as OpenFileEntry

    BM->>FW: writePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    FW->>FW: validateAccessMode(accessMode)
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    FW->>FW: validatePageId(pageId, dataFile)
    note right of FW: PageId is negative, exceeds total pages, or falls outside data region.
    FW-->>FW: throw InvalidPageIdException
    
    FW-->>BM: throw InvalidPageIdException
    deactivate FW
```

---

## 4. Failure Path: Incomplete Page Write
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FW as FileWriter
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant FH as FileHandle

    BM->>FW: writePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    FW->>FW: validateAccessMode(accessMode)
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    FW->>FW: validatePageId(pageId, dataFile)
    
    FW->>OE: get Handle
    OE-->>FW: fileHandle
    
    FW->>DF: Header
    activate DF
    DF-->>FW: fileHeader
    deactivate DF
    
    FW->>FW: calculatePageOffset(pageId, fileHeader)
    
    FW->>FH: write(offset, pageSize, source)
    activate FH
    FH-->>FW: bytesWritten (bytesWritten < pageSize)
    deactivate FH
    
    FW->>FW: validateBytesWritten(bytesWritten, pageSize)
    FW-->>FW: throw IncompletePageWriteException
    
    FW-->>BM: throw IncompletePageWriteException
    deactivate FW
```

---

## 5. Failure Path: OS I/O Error
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FW as FileWriter
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant FH as FileHandle

    BM->>FW: writePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    FW->>FW: validateAccessMode(accessMode)
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    FW->>FW: validatePageId(pageId, dataFile)
    
    FW->>OE: get Handle
    OE-->>FW: fileHandle
    
    FW->>DF: Header
    activate DF
    DF-->>FW: fileHeader
    deactivate DF
    
    FW->>FW: calculatePageOffset(pageId, fileHeader)
    
    FW->>FH: write(offset, pageSize, source)
    activate FH
    FH-->>FW: throw IOException
    deactivate FH
    
    FW-->>BM: throw WriteFailureException
    deactivate FW
```

---

## Discovered Candidates

### Method Candidates
- `FileWriter.writePage(entry: OpenFileEntry, pageId: PageId, source: ByteBuffer) : void`
- `FileWriter.validateAccessMode(accessMode: FileAccessMode) : void`
- `FileWriter.validatePageId(pageId: PageId, file: DataFile) : void`
- `FileWriter.calculatePageOffset(pageId: PageId, header: FileHeader) : long`
- `FileWriter.validateBytesWritten(bytesWritten: int, expectedBytes: int) : void`
- `FileHandle.write(offset: long, length: int, source: ByteBuffer) : int`

### State Candidates
- `PageId` type (representing the unique identifier of a database page).
- `ByteBuffer` representing the source stream buffer.
- `FileAccessMode` enum values (ReadOnly, ReadWrite).
