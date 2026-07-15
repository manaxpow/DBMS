# Sequence Diagram - Allocate Extent

## Use Case Overview

**Actor**: `PageAllocator`, `HeapManager`, `IndexManager`, or another storage component  
**Purpose**: Finds a free extent in a data file, marks it as allocated, and returns its identity and physical location to the caller.

### Preconditions

- The target `DataFile` exists and is open.
- The associated `FileHandle` is valid.
- Allocation metadata has been loaded.
- The requested extent size follows the file's configured extent size.

---

## 1. Happy Path: Allocate Existing Free Extent

```mermaid
sequenceDiagram
    autonumber

    actor PA as PageAllocator
    participant EM as ExtentManager
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant EB as ExtentBitmap
    participant FW as FileWriter

    PA->>EM: AllocateExtent(openFileEntry)
    activate EM

    EM->>OE: Get DataFile
    activate OE
    OE-->>EM: dataFile
    deactivate OE

    EM->>OE: Get Handle
    activate OE
    OE-->>EM: fileHandle
    deactivate OE

    EM->>DF: Get Header
    activate DF
    DF-->>EM: fileHeader
    deactivate DF

    EM->>DF: Get AllocationMetadata
    DF-->>EM: allocationMetadata

    EM->>AM: FindFreeExtent()
    activate AM

    AM->>EB: FindFirstFree()
    activate EB
    EB-->>AM: extentIndex
    deactivate EB

    AM-->>EM: extentId
    deactivate AM

    EM->>AM: MarkExtentAllocated(extentId)
    activate AM
    AM->>EB: MarkUsed(extentId)
    activate EB
    EB-->>AM: Success
    deactivate EB
    AM-->>EM: Success
    deactivate AM

    EM->>AM: CalculateFileOffset(extentId, fileHeader)
    activate AM
    AM-->>EM: diskAddress
    deactivate AM

    EM->>FW: WriteAllocationMetadata(fileHandle, fileHeader, allocationMetadata)
    activate FW
    note right of FW: Persists the updated bitmap<br/>and allocation counters.
    FW-->>EM: Success
    deactivate FW

    EM-->>PA: AllocatedExtent(extentId, diskAddress)
    deactivate EM
```

---

## 2. Alternative Path: No Free Extent — Extend File

```mermaid
sequenceDiagram
    autonumber

    actor PA as PageAllocator
    participant EM as ExtentManager
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant EB as ExtentBitmap
    participant FLM as FileLifecycleManager
    participant FW as FileWriter

    PA->>EM: AllocateExtent(dataFile)
    activate EM

    EM->>DF: Get AllocationMetadata
    DF-->>EM: allocationMetadata

    EM->>AM: FindFreeExtent()
    activate AM

    AM->>EB: FindFirstFree()
    activate EB
    EB-->>AM: Not found
    deactivate EB

    AM-->>EM: null
    deactivate AM

    EM->>DF: Get CurrentSize
    DF-->>EM: currentSize

    EM->>AM: Get ExtentSize
    AM-->>EM: extentSize

    EM->>EM: newSize = currentSize + extentSize

    EM->>FLM: ResizeFile(dataFile, newSize)
    activate FLM
    note right of FLM: Extends the physical file<br/>by one extent.
    FLM-->>EM: Success
    deactivate FLM

    note right of EM: Calculate count to add (e.g., 1)
    EM->>AM: AddExtents(1)
    activate AM

    AM->>EB: AppendFreeExtents(1)
    activate EB
    EB-->>AM: newExtentId
    deactivate EB

    AM-->>EM: newExtentId
    deactivate AM

    EM->>AM: MarkExtentAllocated(newExtentId)
    activate AM
    AM->>EB: MarkUsed(newExtentId)
    activate EB
    EB-->>AM: Success
    deactivate EB
    AM-->>EM: Success
    deactivate AM

    EM->>AM: CalculateFileOffset(newExtentId, fileHeader)
    activate AM
    AM-->>EM: diskAddress
    deactivate AM

    EM->>FW: WriteAllocationMetadata(fileHandle, fileHeader, allocationMetadata)
    activate FW
    FW-->>EM: Success
    deactivate FW

    EM-->>PA: AllocatedExtent(newExtentId, diskAddress)
    deactivate EM
```

---

## 3. Failure Path: Automatic File Extension Is Disabled

```mermaid
sequenceDiagram
    autonumber

    actor PA as PageAllocator
    participant EM as ExtentManager
    participant DF as DataFile
    participant AM as AllocationMetadata

    PA->>EM: AllocateExtent(dataFile)
    activate EM

    EM->>DF: Get AllocationMetadata
    DF-->>EM: allocationMetadata

    EM->>AM: FindFreeExtent()
    activate AM
    AM-->>EM: null
    deactivate AM

    EM->>DF: Get AutoExtendEnabled
    activate DF
    DF-->>EM: false
    deactivate DF

    EM-->>PA: throw NoFreeExtentException
    deactivate EM
```

---

## 4. Failure Path: Maximum File Size Reached

```mermaid
sequenceDiagram
    autonumber

    actor PA as PageAllocator
    participant EM as ExtentManager
    participant DF as DataFile
    participant AM as AllocationMetadata

    PA->>EM: AllocateExtent(dataFile)
    activate EM

    EM->>DF: Get AllocationMetadata
    DF-->>EM: allocationMetadata

    EM->>AM: FindFreeExtent()
    AM-->>EM: null

    EM->>DF: Get CurrentSize
    DF-->>EM: currentSize

    EM->>DF: Get MaximumSize
    DF-->>EM: maximumSize

    EM->>AM: Get ExtentSize
    AM-->>EM: extentSize

    EM->>EM: Calculate requiredSize

    alt requiredSize > maximumSize
        EM-->>PA: throw MaximumFileSizeExceededException
    end

    deactivate EM
```

---

## 5. Failure Path: Physical File Extension Failed

```mermaid
sequenceDiagram
    autonumber

    actor PA as PageAllocator
    participant EM as ExtentManager
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant FLM as FileLifecycleManager

    PA->>EM: AllocateExtent(dataFile)
    activate EM

    EM->>DF: Get AllocationMetadata
    DF-->>EM: allocationMetadata

    EM->>AM: FindFreeExtent()
    AM-->>EM: null

    EM->>DF: Get CurrentSize
    DF-->>EM: currentSize

    EM->>AM: Get ExtentSize
    AM-->>EM: extentSize

    EM->>FLM: ResizeFile(dataFile, currentSize + extentSize)
    activate FLM

    FLM-->>EM: throw FileResizeException
    deactivate FLM

    EM-->>PA: throw ExtentAllocationException
    deactivate EM
```

---

## 6. Failure Path: Allocation Metadata Persistence Failed

```mermaid
sequenceDiagram
    autonumber

    actor PA as PageAllocator
    participant EM as ExtentManager
    participant DF as DataFile
    participant AM as AllocationMetadata
    participant EB as ExtentBitmap
    participant FW as FileWriter

    PA->>EM: AllocateExtent(dataFile)
    activate EM

    EM->>DF: Get AllocationMetadata
    DF-->>EM: allocationMetadata

    EM->>AM: FindFreeExtent()
    activate AM
    AM-->>EM: extentId
    deactivate AM

    EM->>AM: MarkExtentAllocated(extentId)
    activate AM
    AM->>EB: MarkUsed(extentId)
    activate EB
    EB-->>AM: Success
    deactivate EB
    AM-->>EM: Success
    deactivate AM

    EM->>FW: WriteAllocationMetadata(fileHandle, fileHeader, allocationMetadata)
    activate FW

    FW-->>EM: throw IOException
    deactivate FW

    EM->>AM: MarkExtentFree(extentId)
    activate AM
    AM->>EB: MarkFree(extentId)
    activate EB
    note right of EB: Restores the in-memory bitmap<br/>after persistence failure.
    EB-->>AM: Restored
    deactivate EB
    AM-->>EM: Restored count
    deactivate AM

    EM-->>PA: throw ExtentAllocationException
    deactivate EM
```

---

## Discovered Candidates

### Method Candidates

```text
ExtentManager.AllocateExtent(
    entry: OpenFileEntry
) : AllocatedExtent

AllocationMetadata.FindFreeExtent() : ExtentId?

AllocationMetadata.CalculateFileOffset(
    extentId: ExtentId,
    header: FileHeader
) : FileOffset

AllocationMetadata.AddExtents(
    count: int
) : void

AllocationMetadata.MarkExtentAllocated(
    extentId: ExtentId
) : void

AllocationMetadata.MarkExtentFree(
    extentId: ExtentId
) : void

ExtentBitmap.FindFirstFree() : int?

ExtentBitmap.AppendFreeExtents(
    count: int
) : void

ExtentBitmap.MarkUsed(
    extentId: ExtentId
) : void

ExtentBitmap.MarkFree(
    extentId: ExtentId
) : void

FileLifecycleManager.ResizeFile(
    entry: OpenFileEntry,
    newSize: long
) : void

FileWriter.WriteAllocationMetadata(
    handle: FileHandle,
    header: FileHeader,
    metadata: AllocationMetadata
) : void
```

### Property Candidates

```text
DataFile.AllocationMetadata : AllocationMetadata

DataFile.CurrentSize : long

DataFile.MaximumSize : long?

DataFile.AutoExtendEnabled : bool

AllocationMetadata.ExtentSize : int

AllocationMetadata.TotalExtentCount : int

AllocationMetadata.FreeExtentCount : int

AllocationMetadata.ExtentBitmap : ExtentBitmap
```

### Result Candidates

```text
AllocatedExtent
├── ExtentId : ExtentId
├── DiskAddress : DiskAddress
└── Size : int
```

### Exception Candidates

```text
NoFreeExtentException

MaximumFileSizeExceededException

ExtentAllocationException

FileResizeException

AllocationMetadataWriteException
```

### State Candidates

```text
ExtentBitmap[extentId]

AllocationMetadata.TotalExtentCount

AllocationMetadata.FreeExtentCount

DataFile.CurrentSize
```

---

## Responsibility Assignment

### `ExtentManager`

- Coordinates extent allocation.
- Searches for an available extent.
- Requests file extension when no free extent exists.
- Updates allocation state.
- Returns the allocated extent location.

### `AllocationMetadata`

- Owns extent-related counters.
- Finds free extents through the bitmap.
- Calculates the disk address of an extent.
- Registers new extents after physical file extension.

### `ExtentBitmap`

- Stores the allocation state of every extent.
- Finds a free extent.
- Marks an extent as used or free.
- Does not perform physical file operations.

### `FileLifecycleManager`

- Extends the physical data file.
- Does not select or allocate an extent.
- Does not modify extent allocation state.

### `FileWriter`

- Persists allocation metadata.
- Does not decide which extent should be allocated.
---

## Allocation Flow

```text
AllocateExtent
    → Find free extent
        → Found
            → Mark extent used
            → Update counters
            → Persist metadata
            → Return allocated extent

        → Not found
            → Check auto-extend
            → Check maximum file size
            → Extend physical file
            → Register new extent
            → Mark new extent used
            → Persist metadata
            → Return allocated extent
```

---

## Design Decision

`ExtentManager` should not write user records or pages.

Its responsibility ends after returning the allocated storage range:

```text
ExtentManager.AllocateExtent()
    → returns ExtentId and DiskAddress

PageAllocator
    → initializes pages inside the allocated extent

FileWriter
    → writes page data at the provided offsets
```

`FindFreeExtent()`, `MarkUsed()` and `ExtendFile()` are internal steps of the allocation use case, so they do not require separate sequence-diagram files unless their logic later becomes complex.

---

## Durability Note

Extent allocation metadata does not need to trigger `FlushToDisk()` after every allocation.

A full DBMS normally follows:

```text
Write allocation WAL record
    → update allocation metadata page
    → mark metadata page dirty
    → flush during checkpoint or forced durability boundary
```

For the simplified implementation, `WriteAllocationMetadata()` may write immediately, while `BufferManager` or `CheckpointCoordinator` controls the durable sync operation.
