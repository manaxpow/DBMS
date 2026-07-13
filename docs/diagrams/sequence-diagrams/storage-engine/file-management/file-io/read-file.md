# Sequence Diagram - Read File

## Usecase Overview
**Actor**: `BufferManager`
**Purpose**: Shows how the system reads physical blocks/pages from an open database file using the active descriptor and handle.

---

## 1. Happy Path: Successful Page Read
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry
    participant FH as FileHandle

    BM->>FR: readPage(OpenFileEntry, pageId, outBuffer)
    activate FR
    
    FR->>OE: getState()
    activate OE
    OE-->>FR: FileState.Open
    deactivate OE
    
    FR->>OE: getFileHandle()
    activate OE
    OE-->>FR: FileHandle
    deactivate OE
    
    FR->>FR: calculateOffset(pageId)
    
    FR->>FH: read(offset, pageSize, outBuffer)
    activate FH
    note right of FH: OS read system call.
    FH-->>FR: bytesRead
    deactivate FH
    
    FR-->>BM: success
    deactivate FR
```

---

## 2. Failure Path: Read Access Lock Violation
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry

    BM->>FR: readPage(OpenFileEntry, pageId, outBuffer)
    activate FR
    
    FR->>OE: getState()
    activate OE
    OE-->>FR: FileState.Corrupted
    deactivate OE
    
    FR-->>BM: throw InvalidFileStateException
    deactivate FR
```

---

## 3. Failure Path: OS Level Read Error (Bad Sector/Disk Failure)
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FR as FileReader
    participant OE as OpenFileEntry
    participant FH as FileHandle

    BM->>FR: readPage(OpenFileEntry, pageId, outBuffer)
    activate FR
    
    FR->>OE: getState()
    activate OE
    OE-->>FR: FileState.Open
    deactivate OE
    
    FR->>OE: getFileHandle()
    activate OE
    OE-->>FR: FileHandle
    deactivate OE
    
    FR->>FR: calculateOffset(pageId)
    
    FR->>FH: read(offset, pageSize, outBuffer)
    activate FH
    FH-->>FR: throw IOException
    deactivate FH
    
    FR-->>BM: throw ReadFailureException
    deactivate FR
```

---

## Discovered Candidates

### Method Candidates
- `FileReader.readPage(entry: OpenFileEntry, pageId: int, buffer: ByteBuffer) : void`
- `FileReader.calculateOffset(pageId: int) : long`
- `OpenFileEntry.getState() : FileState`
- `OpenFileEntry.getFileHandle() : FileHandle`
- `FileHandle.read(offset: long, length: int, destination: ByteBuffer) : int`

### State Candidates
- `FileState` enum values (e.g. Open, Corrupted)
- `FileAccessMode` enum values
