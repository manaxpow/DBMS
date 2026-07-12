# File Domain Model Diagram - File Management

### Purpose
Illustrates the static structural composition of a physical data file representation.

### Mermaid classDiagram
```mermaid
classDiagram
    class DataFile
    class FileHeader
    class AllocationMetadata
    class ExtentBitmap
    class Extent

    class FileType {
        <<enumeration>>
    }
    class AllocationStatus {
        <<enumeration>>
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
