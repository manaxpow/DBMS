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
    participant FH as FileHandle

    BM->>FW: writePage(OpenFileEntry, pageId, inBuffer)
    activate FW
    
    FW->>OE: getState()
    activate OE
    OE-->>FW: FileState.Open
    deactivate OE
    
    FW->>OE: getAccessMode()
    activate OE
    OE-->>FW: FileAccessMode.ReadWrite
    deactivate OE
    
    FW->>OE: getFileHandle()
    activate OE
    OE-->>FW: FileHandle
    deactivate OE
    
    FW->>FW: calculateOffset(pageId)
    
    FW->>FH: write(offset, pageSize, inBuffer)
    activate FH
    note right of FH: OS write system call.
    FH-->>FW: bytesWritten
    deactivate FH
    
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

    BM->>FW: writePage(OpenFileEntry, pageId, inBuffer)
    activate FW
    
    FW->>OE: getState()
    activate OE
    OE-->>FW: FileState.Open
    deactivate OE
    
    FW->>OE: getAccessMode()
    activate OE
    OE-->>FW: FileAccessMode.ReadOnly
    deactivate OE
    
    FW-->>BM: throw ReadOnlyFileException
    deactivate FW
```

---

## 3. Failure Path: OS Level Disk Write Error (Disk Failure)
```mermaid
sequenceDiagram
    autonumber
    actor BM as BufferManager
    participant FW as FileWriter
    participant OE as OpenFileEntry
    participant FH as FileHandle

    BM->>FW: writePage(OpenFileEntry, pageId, inBuffer)
    activate FW
    
    FW->>OE: getState()
    activate OE
    OE-->>FW: FileState.Open
    deactivate OE
    
    FW->>OE: getAccessMode()
    activate OE
    OE-->>FW: FileAccessMode.ReadWrite
    deactivate OE
    
    FW->>OE: getFileHandle()
    activate OE
    OE-->>FW: FileHandle
    deactivate OE
    
    FW->>FW: calculateOffset(pageId)
    
    FW->>FH: write(offset, pageSize, inBuffer)
    activate FH
    FH-->>FW: throw IOException
    deactivate FH
    
    FW-->>BM: throw WriteFailureException
    deactivate FW
```

---

## Discovered Candidates

### Method Candidates
- `FileWriter.writePage(entry: OpenFileEntry, pageId: int, buffer: ByteBuffer) : void`
- `FileWriter.calculateOffset(pageId: int) : long`
- `OpenFileEntry.getState() : FileState`
- `OpenFileEntry.getAccessMode() : FileAccessMode`
- `OpenFileEntry.getFileHandle() : FileHandle`
- `FileHandle.write(offset: long, length: int, source: ByteBuffer) : int`

### State Candidates
- `FileAccessMode` enum values (ReadOnly, ReadWrite)
- `FileState` enum values (Open, Closed, Corrupted)
