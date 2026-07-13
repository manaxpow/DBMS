# File Domain Model Diagram - File Management

### Purpose
Illustrates the static structural composition of a physical data file representation.

### Mermaid classDiagram
```mermaid
classDiagram
    direction LR

    class DataFile {
        +FileName: string
        +Header: FileHeader
        +AllocationMetadata: AllocationMetadata
        +CurrentSize: long
        +MaximumSize: long?
        +AutoExtendEnabled: bool

        +Create(fileName: string, header: FileHeader, metadata: AllocationMetadata, currentSize: long, maximumSize: long?, autoExtendEnabled: bool) DataFile
        +Reconstruct(fileName: string, header: FileHeader, metadata: AllocationMetadata, currentSize: long, maximumSize: long?, autoExtendEnabled: bool) DataFile
    }

    class FileHeader {
        +MagicNumber: uint
        +FormatVersion: int
        +HeaderSize: int
        +FileId: FileId
        +FileType: FileType
        +PageSize: int
        +ExtentSize: int
        +AllocationMetadataOffset: long
        +ExtentBitmapOffset: long

        +Create(fileId: FileId, fileType: FileType, pageSize: int, extentSize: int, formatVersion: int) FileHeader
    }

    class AllocationMetadata {
        +TotalExtentCount: int
        +FreeExtentCount: int
        +ExtentBitmap: ExtentBitmap

        +Create(totalExtentCount: int) AllocationMetadata
        +FindFreeExtent() ExtentId?
        +ContainsExtent(extentId: ExtentId) bool
        +GetExtentState(extentId: ExtentId) ExtentState
        +MarkExtentAllocated(extentId: ExtentId) void
        +MarkExtentFree(extentId: ExtentId) void
        +AddExtents(count: int) void
        +CanTruncateTo(newExtentCount: int) bool
        +TruncateTo(newExtentCount: int) void
        +CalculateFileOffset(extentId: ExtentId, header: FileHeader) FileOffset
    }

    class ExtentBitmap {
        +TotalExtents: int
        -bitmapBytes: byte[]

        +Create(totalExtents: int) ExtentBitmap
        +FindFirstFree() int?
        +IsAllocated(index: int) bool
        +MarkUsed(index: int) void
        +MarkFree(index: int) void
        +AppendFreeExtents(count: int) void
        +Truncate(newTotalExtents: int) void
    }

    class FileId {
        +Value: int
    }

    class ExtentId {
        +Value: long
    }

    class FileOffset {
        +Value: long
    }

    class FileType {
        <<enumeration>>
        Data
        Index
        Log
        Temporary
    }

    class ExtentState {
        <<enumeration>>
        Free
        Allocated
    }

    DataFile "1" *-- "1" FileHeader
    DataFile "1" *-- "1" AllocationMetadata
    AllocationMetadata "1" *-- "1" ExtentBitmap

    FileHeader "1" *-- "1" FileId

    DataFile ..> FileType
    AllocationMetadata ..> ExtentId
    AllocationMetadata ..> FileOffset
    AllocationMetadata ..> ExtentState
```

### Relationship Explanation
- **Composition (`*--`)**:
  - `DataFile` strictly composes the structural `FileHeader` and `AllocationMetadata`.
- **Association (`-->`)**:
  - `AllocationMetadata` holds direct references to enums (`ExtentState`) and structural IDs (`ExtentId`) for allocation lookups.
