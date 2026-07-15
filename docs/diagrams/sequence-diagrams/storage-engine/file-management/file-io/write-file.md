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

    BM->>FW: WritePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    note right of FW: Validate AccessMode
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    note right of FW: Validate PageId against DataFile
    
    FW->>OE: get Handle
    OE-->>FW: fileHandle
    
    FW->>DF: Header
    activate DF
    DF-->>FW: fileHeader
    deactivate DF
    
    note right of FW: Calculate physical page offset
    
    FW->>FH: WriteAtOffset(offset, pageSize, source)
    activate FH
    FH-->>FW: bytesWritten
    deactivate FH
    
    note right of FW: Validate bytes written
    
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

    BM->>FW: WritePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    note right of FW: Validate AccessMode
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

    BM->>FW: WritePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    note right of FW: Validate AccessMode
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    note right of FW: Validate PageId against DataFile
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

    BM->>FW: WritePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    note right of FW: Validate AccessMode
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    note right of FW: Validate PageId against DataFile
    
    FW->>OE: get Handle
    OE-->>FW: fileHandle
    
    FW->>DF: Header
    activate DF
    DF-->>FW: fileHeader
    deactivate DF
    
    note right of FW: Calculate physical page offset
    
    FW->>FH: WriteAtOffset(offset, pageSize, source)
    activate FH
    FH-->>FW: bytesWritten (bytesWritten < pageSize)
    deactivate FH
    
    note right of FW: Validate bytes written
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

    BM->>FW: WritePage(openFileEntry, pageId, source)
    activate FW
    
    FW->>OE: get AccessMode
    OE-->>FW: accessMode
    
    note right of FW: Validate AccessMode
    
    FW->>OE: DataFile
    activate OE
    OE-->>FW: dataFile
    deactivate OE
    
    note right of FW: Validate PageId against DataFile
    
    FW->>OE: get Handle
    OE-->>FW: fileHandle
    
    FW->>DF: Header
    activate DF
    DF-->>FW: fileHeader
    deactivate DF
    
    note right of FW: Calculate physical page offset
    
    FW->>FH: WriteAtOffset(offset, pageSize, source)
    activate FH
    FH-->>FW: throw IOException
    deactivate FH
    
    FW-->>BM: throw WriteFailureException
    deactivate FW
```

---

## Discovered Candidates

### Method Candidates
- `FileWriter.WritePage(entry: OpenFileEntry, pageId: PageId, source: ReadOnlyMemory~byte~) : void`
- `FileHandle.WriteAtOffset(offset: long, source: ReadOnlyMemory~byte~) : void`

### State Candidates
- `PageId` type (representing the unique identifier of a database page).
- `ByteBuffer` representing the source stream buffer.
- `FileAccessMode` enum values (ReadOnly, ReadWrite).
