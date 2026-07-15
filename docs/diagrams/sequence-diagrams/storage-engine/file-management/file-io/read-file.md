# Sequence Diagram - Read File

## Usecase Overview
**Actor**: `BufferManager`
**Purpose**: Shows how the system reads physical blocks/pages from an open database file using the active file handle.

---

## 1. Happy Path: Successful Page Read
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant FH as FileHandle

    BM->>FR: ReadPage(openFileEntry, pageId, destination)
    activate FR
    
    FR->>OE: DataFile
    activate OE
    OE-->>FR: dataFile
    deactivate OE
    
    note right of FR: Validate PageId against DataFile
    
    FR->>OE: get Handle
    OE-->>FR: fileHandle
    
    FR->>DF: Header
    activate DF
    DF-->>FR: fileHeader
    deactivate DF
    
    note right of FR: Calculate physical page offset
    
    FR->>FH: ReadAtOffset(offset, pageSize, destination)
    activate FH
    FH-->>FR: bytesRead
    deactivate FH
    
    note right of FR: Validate bytes read
    
    FR-->>BM: success
    deactivate FR
```

---

## 2. Failure Path: Invalid PageId
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry

    BM->>FR: ReadPage(openFileEntry, pageId, destination)
    activate FR
    
    FR->>OE: DataFile
    activate OE
    OE-->>FR: dataFile
    deactivate OE
    
    note right of FR: Validate PageId
    note right of FR: PageId is negative, exceeds total pages, or falls outside data region.
    FR-->>FR: throw InvalidPageIdException
    
    FR-->>BM: throw InvalidPageIdException
    deactivate FR
```

---

## 3. Failure Path: Incomplete Page Read
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant FH as FileHandle

    BM->>FR: ReadPage(openFileEntry, pageId, destination)
    activate FR
    
    FR->>OE: DataFile
    activate OE
    OE-->>FR: dataFile
    deactivate OE
    
    note right of FR: Validate PageId against DataFile
    
    FR->>OE: get Handle
    OE-->>FR: fileHandle
    
    FR->>DF: Header
    activate DF
    DF-->>FR: fileHeader
    deactivate DF
    
    note right of FR: Calculate physical page offset
    
    FR->>FH: ReadAtOffset(offset, pageSize, destination)
    activate FH
    FH-->>FR: bytesRead (bytesRead < pageSize)
    deactivate FH
    
    note right of FR: Validate bytes read
    FR-->>FR: throw IncompletePageReadException
    
    FR-->>BM: throw IncompletePageReadException
    deactivate FR
```

---

## 4. Failure Path: OS I/O Error
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant FH as FileHandle

    BM->>FR: ReadPage(openFileEntry, pageId, destination)
    activate FR
    
    FR->>OE: DataFile
    activate OE
    OE-->>FR: dataFile
    deactivate OE
    
    note right of FR: Validate PageId against DataFile
    
    FR->>OE: Handle
    activate OE
    OE-->>FR: fileHandle
    deactivate OE
    
    FR->>DF: Header
    activate DF
    DF-->>FR: fileHeader
    deactivate DF
    
    note right of FR: Calculate physical page offset
    
    FR->>FH: ReadAtOffset(offset, pageSize, destination)
    activate FH
    FH-->>FR: throw IOException
    deactivate FH
    
    FR-->>BM: throw ReadFailureException
    deactivate FR
```

---

## Discovered Candidates

### Method Candidates
- `FileReader.ReadPage(entry: OpenFileEntry, pageId: PageId, destination: Memory~byte~) : void`
- `FileHandle.ReadAtOffset(offset: long, destination: Memory~byte~) : int`

### State Candidates
- `PageId` type (representing the unique identifier of a database page).
- `ByteBuffer` representing the destination stream buffer.
