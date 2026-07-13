# Sequence Diagram - Resize File

## Use Case Overview

**Actor**: `ExtentManager`, `DatabaseManager`, or administrative operation  
**Purpose**: Changes the physical size of an existing data file while protecting allocated extents and keeping file metadata consistent.

### Preconditions

- The file is already open.
- The associated `FileHandle` is valid.
- The caller has sufficient permission to modify the file.
- Shrinking must not remove any extent that is still allocated.

---

## 1. Happy Path: Extend File

```mermaid
sequenceDiagram
    autonumber

    actor EM as ExtentManager
    participant FLM as FileLifecycleManager
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant FH as FileHandle
    participant FW as FileWriter
    participant FS as FileSynchronizer

    EM->>FLM: ResizeFile(openFileEntry, newSize)
    activate FLM

    FLM->>OE: Get Handle
    OE-->>FLM: fileHandle

    FLM->>OE: Get DataFile
    OE-->>FLM: dataFile

    FLM->>FH: GetLength()
    activate FH
    FH-->>FLM: currentSize
    deactivate FH

    FLM->>FLM: Validate newSize > currentSize

    FLM->>FH: SetLength(newSize)
    activate FH
    FH-->>FLM: Success
    deactivate FH

    FLM->>DF: Get AllocationMetadata
    DF-->>FLM: allocationMetadata

    FLM->>AM: AddFreeSpace(currentSize, newSize)
    activate AM
    note right of AM: Registers newly added extents<br/>as available free space.
    AM-->>FLM: Updated metadata
    deactivate AM

    FLM->>FW: WriteAllocationMetadata(fileHandle, allocationMetadata)
    activate FW
    FW-->>FLM: Success
    deactivate FW

    FLM->>FS: Sync(openFileEntry)
    activate FS
    FS-->>FLM: Success
    deactivate FS

    FLM-->>EM: Success
    deactivate FLM
```

---

## 2. Happy Path: Truncate File

```mermaid
sequenceDiagram
    autonumber

    actor DB as DatabaseManager
    participant FLM as FileLifecycleManager
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant FH as FileHandle
    participant FW as FileWriter
    participant FS as FileSynchronizer

    DB->>FLM: ResizeFile(openFileEntry, newSize)
    activate FLM

    FLM->>OE: Get Handle
    OE-->>FLM: fileHandle

    FLM->>OE: Get DataFile
    OE-->>FLM: dataFile

    FLM->>FH: GetLength()
    activate FH
    FH-->>FLM: currentSize
    deactivate FH

    FLM->>FLM: Validate newSize < currentSize

    FLM->>DF: Get AllocationMetadata
    DF-->>FLM: allocationMetadata

    FLM->>AM: CanTruncateTo(newSize)
    activate AM
    note right of AM: Verifies that no allocated extent<br/>exists beyond the new boundary.
    AM-->>FLM: true
    deactivate AM

    FLM->>AM: RemoveFreeSpace(newSize, currentSize)
    activate AM
    AM-->>FLM: Updated metadata
    deactivate AM

    FLM->>FW: WriteAllocationMetadata(fileHandle, allocationMetadata)
    activate FW
    FW-->>FLM: Success
    deactivate FW

    FLM->>FH: SetLength(newSize)
    activate FH
    FH-->>FLM: Success
    deactivate FH

    FLM->>FS: Sync(openFileEntry)
    activate FS
    FS-->>FLM: Success
    deactivate FS

    FLM-->>DB: Success
    deactivate FLM
```

---

## 3. Alternative Path: Requested Size Equals Current Size

```mermaid
sequenceDiagram
    autonumber

    actor Caller
    participant FLM as FileLifecycleManager
    participant OE as OpenFileEntry
    participant FH as FileHandle

    Caller->>FLM: ResizeFile(openFileEntry, newSize)
    activate FLM

    FLM->>OE: Get Handle
    OE-->>FLM: fileHandle

    FLM->>FH: GetLength()
    activate FH
    FH-->>FLM: currentSize
    deactivate FH

    alt newSize == currentSize
        FLM-->>Caller: Success without modification
    end

    deactivate FLM
```

---

## 4. Failure Path: Truncate Removes Used Extent

```mermaid
sequenceDiagram
    autonumber

    actor DB as DatabaseManager
    participant FLM as FileLifecycleManager
    participant OE as OpenFileEntry
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant FH as FileHandle

    DB->>FLM: ResizeFile(openFileEntry, newSize)
    activate FLM

    FLM->>OE: Get Handle
    OE-->>FLM: fileHandle

    FLM->>OE: Get DataFile
    OE-->>FLM: dataFile

    FLM->>FH: GetLength()
    FH-->>FLM: currentSize

    FLM->>DF: Get AllocationMetadata
    DF-->>FLM: allocationMetadata

    FLM->>AM: CanTruncateTo(newSize)
    activate AM
    AM-->>FLM: false
    deactivate AM

    FLM-->>DB: throw FileTruncationException
    deactivate FLM
```

---

## 5. Failure Path: Operating-System Resize Error

```mermaid
sequenceDiagram
    autonumber

    actor Caller
    participant FLM as FileLifecycleManager
    participant OE as OpenFileEntry
    participant FH as FileHandle

    Caller->>FLM: ResizeFile(openFileEntry, newSize)
    activate FLM

    FLM->>OE: Get Handle
    OE-->>FLM: fileHandle

    FLM->>FH: SetLength(newSize)
    activate FH

    FH-->>FLM: throw IOException
    deactivate FH

    FLM->>FLM: Wrap exception with file context

    FLM-->>Caller: throw FileResizeException
    deactivate FLM
```

---

## Discovered Candidates

### Method Candidates

```text
FileLifecycleManager.ResizeFile(
    openFileEntry: OpenFileEntry,
    newSize: long
) : void

FileHandle.GetLength() : long

FileHandle.SetLength(
    newSize: long
) : void

AllocationMetadata.CanTruncateTo(
    newSize: long
) : bool

AllocationMetadata.AddFreeSpace(
    previousSize: long,
    newSize: long
) : void

AllocationMetadata.RemoveFreeSpace(
    newSize: long,
    previousSize: long
) : void

FileWriter.WriteAllocationMetadata(
    handle: FileHandle,
    metadata: AllocationMetadata
) : void
```

### Property Candidates

```text
DataFile.Size : long

DataFile.AllocationMetadata : AllocationMetadata

AllocationMetadata.ExtentSize : int

AllocationMetadata.TotalExtentCount : int

AllocationMetadata.FreeExtentCount : int
```

### Exception Candidates

```text
InvalidFileSizeException

FileResizeException

FileTruncationException

AllocatedExtentTruncationException
```

### State Candidates

```text
DataFile.Size

AllocationMetadata.TotalExtentCount

AllocationMetadata.FreeExtentCount
```

---

## Responsibility Assignment

### `FileLifecycleManager`

- Coordinates the resize operation.
- Validates the requested size.
- Prevents unsafe file truncation.
- Updates physical file size.
- Coordinates metadata persistence.

### `AllocationMetadata`

- Determines whether truncation is safe.
- Registers newly added extents.
- Removes free extents outside the new file boundary.
- Maintains extent counters.

### `FileHandle`

- Reads the current physical file length.
- Requests the operating system to change the physical file length.
- Converts low-level resize failures into `IOException`.

---

## Design Notes

`FileLifecycleManager` should not decide which extent will be allocated.

It only changes the physical file size and coordinates metadata updates.

Extent selection remains the responsibility of `ExtentManager`.

```text
ExtentManager
    → determines additional capacity is required
    → requests FileLifecycleManager.ResizeFile()
    → allocates an extent from the newly added free space
```
