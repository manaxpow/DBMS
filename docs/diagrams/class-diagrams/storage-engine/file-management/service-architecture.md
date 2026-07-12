# Service Architecture Diagram - File Management

### Purpose
Shows the service classes, their corresponding interfaces, and how the manager references helper services.

### Mermaid classDiagram
```mermaid
classDiagram
    %% Interfaces
    class IFileLifecycleManager {
        <<interface>>
    }
    class IFileReader {
        <<interface>>
    }
    class IFileWriter {
        <<interface>>
    }
    class IFileSynchronizer {
        <<interface>>
    }
    class IOpenFileManager {
        <<interface>>
    }
    class IExtentManager {
        <<interface>>
    }

    %% Classes
    class FileLifecycleManager
    class FileReader
    class FileWriter
    class FileSynchronizer
    class OpenFileManager
    class ExtentManager

    %% Interface Realizations
    FileLifecycleManager ..|> IFileLifecycleManager
    FileReader ..|> IFileReader
    FileWriter ..|> IFileWriter
    FileSynchronizer ..|> IFileSynchronizer
    OpenFileManager ..|> IOpenFileManager
    ExtentManager ..|> IExtentManager

    %% Structural Aggregations
    FileLifecycleManager "1" o-- "1" IFileReader : references
    FileLifecycleManager "1" o-- "1" IFileWriter : references
    FileLifecycleManager "1" o-- "1" IFileSynchronizer : references
    FileLifecycleManager "1" o-- "1" IOpenFileManager : references
```

### Relationship Explanation
- **Interface Realization (`..|>`)**:
  - Used for all service components (`FileLifecycleManager`, `FileReader`, `FileWriter`, `FileSynchronizer`, `OpenFileManager`, `ExtentManager`) to isolate implementation details from other database subsystems.
- **Aggregation (`o--`)**:
  - **`FileLifecycleManager` aggregates helper services**: It holds persistent references to `IFileReader`, `IFileWriter`, `IFileSynchronizer`, and `IOpenFileManager` to delegate sub-tasks (e.g. format on creation, close entries on deletion). These helper services can exist independently of the lifecycle manager and are typically injected via dependency injection.
