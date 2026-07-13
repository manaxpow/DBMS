# File Domain Model Diagram - File Management

### Purpose
Illustrates the static structural composition of a physical data file representation.

### Mermaid classDiagram
```mermaid
classDiagram
    class DataFile {
        +FileName : String
        +FileType : FileType
        +Header : FileHeader
        +AllocationMetadata : AllocationMetadata
        +ExtentBitmap : ExtentBitmap
        +Extents : List~Extent~
        +create(fileName: String, fileType: FileType, header: FileHeader, metadata: AllocationMetadata, extentBitmap: ExtentBitmap) DataFile
        +reconstruct(fileName: String, fileType: FileType, header: FileHeader, metadata: AllocationMetadata, extentBitmap: ExtentBitmap) DataFile
        +addExtent(extent: Extent) void
    }

    class FileHeader {
        +FileType : FileType
        +PageSize : int
        +FormatVersion : int
        +MetadataOffset : long
        +ExtentBitmapOffset : long
        +create(fileType: FileType, pageSize: int, formatVersion: int) FileHeader
    }

    class AllocationMetadata {
        +InitialFileSize : long
        +PageSize : int
        +TotalExtents : int
        +AllocatedPages : int
        +create(initialFileSize: long, pageSize: int) AllocationMetadata
        +incrementAllocatedPages(count: int) void
    }

    class ExtentBitmap {
        +TotalExtents : int
        +BitmapBytes : byte[]
        +create(totalExtents: int) ExtentBitmap
        +findFreeExtentBit() int
        +setAllocatedBit(index: int, allocated: boolean) void
    }

    class Extent {
        +Index : int
        +Status : AllocationStatus
        +create(index: int, status: AllocationStatus) Extent
    }

    class FileType {
        <<enumeration>>
        Table
        Log
        Temporary
    }
    class AllocationStatus {
        <<enumeration>>
        Free
        Allocated
        Reserved
    }

    %% Compositions
    DataFile "1" *-- "1" FileHeader : composes
    DataFile "1" *-- "1" AllocationMetadata : composes
    DataFile "1" *-- "1" ExtentBitmap : composes
    DataFile "1" *-- "0..*" Extent : composes

    %% Associations
    DataFile "0..*" --> "1" FileType : references
    Extent "0..*" --> "1" AllocationStatus : references
```

### Relationship Explanation
- **Composition (`*--`)**:
  - **`DataFile` composes `FileHeader`, `AllocationMetadata`, `ExtentBitmap`, and `Extent`**: The physical header, metadata tracker, extent occupancy bitmap, and individual extents are structural parts of a single `DataFile`. They are created together with the `DataFile` and their lifetimes are bound to the `DataFile`. They cannot exist or be shared outside of it.
