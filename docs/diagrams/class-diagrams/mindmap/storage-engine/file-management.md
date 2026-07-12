Interfaces
- IFileLifecycleManager
- IFileReader
- IFileWriter
- IFileSynchronizer
- IExtentManager
- IOpenFileManager

Classes
- FileLifecycleManager
- FileReader
- FileWriter
- FileSynchronizer
- ExtentManager
- OpenFileManager

Domain Classes
- DataFile
- FileHeader
- AllocationMetadata
- ExtentBitmap
- Extent
- OpenFileEntry
- FileHandle

Enums
- FileType
- FileState
- FileAccessMode
- FileLockMode
- AllocationStatus

Relationships

FileLifecycleManager ..|> IFileLifecycleManager
FileReader ..|> IFileReader
FileWriter ..|> IFileWriter
FileSynchronizer ..|> IFileSynchronizer
ExtentManager ..|> IExtentManager
OpenFileManager ..|> IOpenFileManager

FileLifecycleManager --> FileReader
FileLifecycleManager --> FileWriter
FileLifecycleManager --> FileSynchronizer
FileLifecycleManager --> OpenFileManager
FileLifecycleManager --> DataFile

ExtentManager --> DataFile
ExtentManager --> AllocationMetadata
ExtentManager --> ExtentBitmap
ExtentManager --> Extent

FileReader --> OpenFileEntry
FileWriter --> OpenFileEntry
FileSynchronizer --> OpenFileEntry

OpenFileManager --> OpenFileEntry

DataFile *-- FileHeader
DataFile *-- AllocationMetadata
DataFile *-- ExtentBitmap
DataFile *-- Extent

OpenFileEntry *-- FileHandle

OpenFileEntry --> DataFile

DataFile --> FileType
OpenFileEntry --> FileState
OpenFileEntry --> FileLockMode
FileReader --> FileAccessMode
FileWriter --> FileAccessMode
Extent --> AllocationStatus