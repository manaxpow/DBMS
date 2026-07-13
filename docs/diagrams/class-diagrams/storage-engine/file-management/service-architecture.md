# Service Architecture Diagram - File Management

### Purpose

Shows detailed File Management service interfaces, concrete implementations, and dependencies, grouped by responsibility.

### Mermaid Class Diagram

```mermaid
classDiagram
    direction LR

    namespace FileLifecycle {
        class IFileLifecycleManager {
            <<interface>>
            +CreateFile(fileName: string, fileType: FileType, pageSize: int, initialFileSize: long) DataFile
            +OpenFile(fileName: string, accessMode: FileAccessMode, lockMode: FileLockMode) OpenFileEntry
            +CloseFile(fileName: string) void
            +DeleteFile(fileName: string) void
            +ResizeFile(entry: OpenFileEntry, newSize: long) void
        }

        class FileLifecycleManager {
            -fileReader: IFileReader
            -fileWriter: IFileWriter
            -fileSynchronizer: IFileSynchronizer
            -openFileManager: IOpenFileManager
            -fileValidator: IFileValidator

            +CreateFile(fileName: string, fileType: FileType, pageSize: int, initialFileSize: long) DataFile
            +OpenFile(fileName: string, accessMode: FileAccessMode, lockMode: FileLockMode) OpenFileEntry
            +CloseFile(fileName: string) void
            +DeleteFile(fileName: string) void
            +ResizeFile(entry: OpenFileEntry, newSize: long) void

            -CheckFileExists(fileName: string) bool
            -CreatePhysicalFile(fileName: string, initialFileSize: long) FileHandle
            -OpenPhysicalFile(fileName: string, accessMode: FileAccessMode) FileHandle
            -ClosePhysicalFile(handle: FileHandle) void
            -DeletePhysicalFile(fileName: string) void
            -GetPhysicalFileSize(handle: FileHandle) long
        }

        class IFileValidator {
            <<interface>>
            +Validate(header: FileHeader, metadata: AllocationMetadata, bitmap: ExtentBitmap, physicalFileSize: long) void
        }

        class FileValidator {
            +Validate(header: FileHeader, metadata: AllocationMetadata, bitmap: ExtentBitmap, physicalFileSize: long) void

            -ValidateMagicNumber(header: FileHeader) void
            -ValidateFormatVersion(header: FileHeader) void
            -ValidateFileBoundary(header: FileHeader, physicalFileSize: long) void
            -ValidateAllocationMetadata(metadata: AllocationMetadata, bitmap: ExtentBitmap) void
        }
    }

    namespace FileIO {
        class IFileReader {
            <<interface>>
            +ReadAtOffset(entry: OpenFileEntry, offset: long, destination: Memory~byte~) int
            +ReadHeader(handle: FileHandle) FileHeader
            +ReadAllocationMetadata(handle: FileHandle, header: FileHeader) AllocationMetadata
            +ReadExtentBitmap(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) ExtentBitmap
        }

        class FileReader {
            +ReadAtOffset(entry: OpenFileEntry, offset: long, destination: Memory~byte~) int
            +ReadHeader(handle: FileHandle) FileHeader
            +ReadAllocationMetadata(handle: FileHandle, header: FileHeader) AllocationMetadata
            +ReadExtentBitmap(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) ExtentBitmap

            -ValidateReadRange(entry: OpenFileEntry, offset: long, length: int) void
            -ValidateBytesRead(bytesRead: int, expectedBytes: int) void
        }

        class IFileWriter {
            <<interface>>
            +WriteAtOffset(entry: OpenFileEntry, offset: long, source: ReadOnlyMemory~byte~) void
            +WriteHeader(handle: FileHandle, header: FileHeader) void
            +WriteAllocationMetadata(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) void
            +WriteExtentBitmap(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata, bitmap: ExtentBitmap) void
        }

        class FileWriter {
            +WriteAtOffset(entry: OpenFileEntry, offset: long, source: ReadOnlyMemory~byte~) void
            +WriteHeader(handle: FileHandle, header: FileHeader) void
            +WriteAllocationMetadata(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata) void
            +WriteExtentBitmap(handle: FileHandle, header: FileHeader, metadata: AllocationMetadata, bitmap: ExtentBitmap) void

            -ValidateAccessMode(accessMode: FileAccessMode) void
            -ValidateWriteRange(entry: OpenFileEntry, offset: long, length: int) void
            -ValidateBytesWritten(bytesWritten: int, expectedBytes: int) void
        }

        class IFileSynchronizer {
            <<interface>>
            +Sync(entry: OpenFileEntry) void
        }

        class FileSynchronizer {
            +Sync(entry: OpenFileEntry) void
        }
    }

    namespace RuntimeFileManagement {
        class IOpenFileManager {
            <<interface>>
            +GetOpenFile(fileName: string) OpenFileEntry?
            +RegisterOpenFile(fileName: string, entry: OpenFileEntry) void
            +UnregisterOpenFile(fileName: string) void
            +TryBeginDelete(fileName: string) bool
            +CompleteDelete(fileName: string) void
            +CancelDelete(fileName: string) void
        }

        class OpenFileManager {
            -openFiles: ConcurrentDictionary~string, OpenFileEntry~

            +GetOpenFile(fileName: string) OpenFileEntry?
            +RegisterOpenFile(fileName: string, entry: OpenFileEntry) void
            +UnregisterOpenFile(fileName: string) void
            +TryBeginDelete(fileName: string) bool
            +CompleteDelete(fileName: string) void
            +CancelDelete(fileName: string) void
        }
    }

    namespace ExtentManagement {
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

        class AllocatedExtent {
            +ExtentId: ExtentId
            +DiskAddress: DiskAddress
            +Size: int
        }
    }

    IFileLifecycleManager <|.. FileLifecycleManager
    IFileValidator <|.. FileValidator

    IFileReader <|.. FileReader
    IFileWriter <|.. FileWriter
    IFileSynchronizer <|.. FileSynchronizer

    IOpenFileManager <|.. OpenFileManager
    IExtentManager <|.. ExtentManager

    FileLifecycleManager ..> IFileReader
    FileLifecycleManager ..> IFileWriter
    FileLifecycleManager ..> IFileSynchronizer
    FileLifecycleManager ..> IOpenFileManager
    FileLifecycleManager ..> IFileValidator

    ExtentManager ..> IFileLifecycleManager
    ExtentManager ..> IFileWriter

    IExtentManager ..> AllocatedExtent : returns