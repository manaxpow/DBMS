# Runtime File Management Diagram - File Management

### Purpose

Details how active, open files and their OS-level handles are managed at runtime.

### Mermaid Class Diagram

```mermaid
classDiagram
    direction LR

    class OpenFileManager {
        -openFiles: ConcurrentDictionary~string, OpenFileEntry~

        +GetOpenFile(fileName: string) OpenFileEntry?
        +RegisterOpenFile(fileName: string, entry: OpenFileEntry) void
        +UnregisterOpenFile(fileName: string) void
        +TryBeginDelete(fileName: string) bool
        +CompleteDelete(fileName: string) void
        +CancelDelete(fileName: string) void
    }

    class OpenFileEntry {
        +Handle: FileHandle
        +DataFile: DataFile
        +AccessMode: FileAccessMode
        +LockMode: FileLockMode
        +ReferenceCount: int

        +Create(handle: FileHandle, file: DataFile, accessMode: FileAccessMode, lockMode: FileLockMode) OpenFileEntry
        +IncrementRefCount() int
        +DecrementRefCount() int
    }

    class FileHandle {
        -descriptor: int

        +ReadAtOffset(destination: Memory~byte~, offset: long) int
        +WriteAtOffset(source: ReadOnlyMemory~byte~, offset: long) void
        +FlushToDisk() void
        +GetLength() long
        +SetLength(newSize: long) void
    }

    class DataFile

    class FileAccessMode {
        <<enumeration>>
        ReadOnly
        ReadWrite
    }

    class FileLockMode {
        <<enumeration>>
        Shared
        Exclusive
        None
    }

    OpenFileManager "1" *-- "0..*" OpenFileEntry : tracks
    OpenFileEntry "1" *-- "1" FileHandle : owns

    OpenFileEntry ..> DataFile
    OpenFileEntry ..> FileAccessMode
    OpenFileEntry ..> FileLockMode
```

### Relationship Explanation

- **Composition (`*--`)**:
  - `OpenFileEntry` owns and composes the lifetime of its low-level operating-system `FileHandle`.
  - `OpenFileManager` composes and locks its collection of active open entries.
- **Dependency/Association (`..>`)**:
  - `OpenFileEntry` refers to `DataFile` structure, access flags, and file lock options.
