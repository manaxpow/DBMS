# Runtime File Management Diagram - File Management

### Purpose
Details how active, open files and their OS-level handles are managed at runtime.

### Mermaid classDiagram
```mermaid
classDiagram
    class OpenFileManager
    class OpenFileEntry
    class FileHandle
    class DataFile

    class FileState {
        <<enumeration>>
    }
    class FileLockMode {
        <<enumeration>>
    }

    %% Aggregations
    OpenFileManager "1" o-- "0..*" OpenFileEntry : aggregates

    %% Compositions
    OpenFileEntry "1" *-- "1" FileHandle : composes

    %% Associations
    OpenFileEntry "0..*" --> "1" DataFile : references
    OpenFileEntry "0..*" --> "1" FileState : references
    OpenFileEntry "0..*" --> "1" FileLockMode : references
```

### Relationship Explanation
- **Composition (`*--`)**:
  - **`OpenFileEntry` composes `FileHandle`**: An open file entry holds exclusive ownership of the raw OS-level file handle. The file handle's lifetime is bound to the `OpenFileEntry`; when the entry is closed/destroyed, the handle is also closed.
- **Aggregation (`o--`)**:
  - **`OpenFileManager` aggregates `OpenFileEntry`**: The manager tracks active open file entries. It stores them in a collection, but does not own their logical database lifetime. The entry can be open or closed independently of the manager's existence.
