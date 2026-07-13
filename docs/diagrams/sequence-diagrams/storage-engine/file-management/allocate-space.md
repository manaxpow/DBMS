# Sequence Diagram - Allocate Space

## Usecase Overview
**Actor**: `PageAllocator`
**Purpose**: Shows how the system allocates space dynamically in terms of extents (blocks of pages) for a database file, updating the tracking metadata and bitmaps.

---

## 1. Happy Path: Successful Extent Allocation
```mermaid
sequenceDiagram
    autonumber
    actor PA1 as PageAllocator
    participant EM1 as ExtentManager
    participant DF1 as DataFile
    participant BM1 as ExtentBitmap
    participant MD1 as AllocationMetadata

    PA1->>EM1: allocateExtent(DataFile)
    activate EM1
    
    EM1->>DF1: getExtentBitmap()
    activate DF1
    DF1-->>EM1: ExtentBitmap
    deactivate DF1
    
    EM1->>BM1: findFreeExtentBit()
    activate BM1
    BM1-->>EM1: extentIndex (e.g. 5)
    deactivate BM1
    
    EM1->>BM1: setAllocatedBit(extentIndex, true)
    activate BM1
    BM1-->>EM1: success
    deactivate BM1
    
    create participant EX1 as Extent
    EM1->>EX1: create(extentIndex, AllocationStatus.Allocated)
    
    EM1->>DF1: addExtent(Extent)
    activate DF1
    DF1-->>EM1: success
    deactivate DF1
    
    EM1->>DF1: getAllocationMetadata()
    activate DF1
    DF1-->>EM1: AllocationMetadata
    deactivate DF1
    
    EM1->>MD1: incrementAllocatedPages(pagesPerExtent)
    activate MD1
    MD1-->>EM1: success
    deactivate MD1
    
    EM1-->>PA1: Extent
    deactivate EM1
```

---

## 2. Failure Path: Out of Space (No Free Extents in Bitmap)
```mermaid
sequenceDiagram
    autonumber
    actor PA2 as PageAllocator
    participant EM2 as ExtentManager
    participant DF2 as DataFile
    participant BM2 as ExtentBitmap

    PA2->>EM2: allocateExtent(DataFile)
    activate EM2
    
    EM2->>DF2: getExtentBitmap()
    activate DF2
    DF2-->>EM2: ExtentBitmap
    deactivate DF2
    
    EM2->>BM2: findFreeExtentBit()
    activate BM2
    BM2-->>EM2: -1 (None Free)
    deactivate BM2
    
    EM2-->>PA2: throw NoFreeExtentsException
    deactivate EM2
```

---

## Discovered Candidates

### Method Candidates
- `ExtentManager.allocateExtent(file: DataFile) : Extent`
- `DataFile.getExtentBitmap() : ExtentBitmap`
- `DataFile.getAllocationMetadata() : AllocationMetadata`
- `DataFile.addExtent(extent: Extent) : void`
- `ExtentBitmap.findFreeExtentBit() : int`
- `ExtentBitmap.setAllocatedBit(index: int, allocated: boolean) : void`
- `AllocationMetadata.incrementAllocatedPages(count: int) : void`
- `Extent.create(index: int, status: AllocationStatus)`

### State Candidates
- `AllocationStatus` enum values (Free, Allocated, Reserved)
- Extent size metrics in `AllocationMetadata`.

