# Extent Management Diagram - File Management

### Purpose
Details the service interface and implementation for allocating space in terms of extents (blocks of pages) for database files.

### Mermaid classDiagram
```mermaid
classDiagram
    direction LR

    class IExtentManager {
        <<interface>>
        +AllocateExtent(entry: OpenFileEntry) AllocatedExtent
        +FreeExtent(entry: OpenFileEntry, extentId: ExtentId) void
    }

    class ExtentManager {
        -fileLifecycleManager: IFileLifecycleManager
        -fileWriter: IFileWriter

        +AllocateExtent(entry: OpenFileEntry) AllocatedExtent
        +FreeExtent(entry: OpenFileEntry, extentId: ExtentId) void
    }

    class IFileLifecycleManager {
        <<interface>>
    }

    class IFileWriter {
        <<interface>>
    }

    class AllocatedExtent {
        +ExtentId: ExtentId
        +DiskAddress: DiskAddress
        +Size: int
    }

    class ExtentId {
        +Value: long
    }

    class DiskAddress {
        +Offset: long
    }

    IExtentManager <|.. ExtentManager

    ExtentManager ..> IFileLifecycleManager : requests resize
    ExtentManager ..> IFileWriter : persists metadata

    IExtentManager ..> AllocatedExtent : returns

    AllocatedExtent *-- ExtentId
    AllocatedExtent *-- DiskAddress
```

### Relationship Explanation
- **Interface Realization (`..|>`)**:
  - `ExtentManager` implements `IExtentManager` to isolate space allocation operations.
  - `ExtentUsageTracker` implements `IExtentUsageTracker` to track page allocation statuses independently.
- **Association (`-->`)**:
  - `ExtentManager` references the `IExtentUsageTracker` interface to determine if extents can be safely released.
  - `AllocatedExtent` references `ExtentId` and `DiskAddress` value objects.
