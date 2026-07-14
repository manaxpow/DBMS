# Unit Test Specification — `FileLifecycleManager.CreateFile`

## 1. Scope

This document covers unit test specifications for `FileLifecycleManager.CreateFile`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileLifecycleManager`
* **Method:**

```csharp
DataFile CreateFile(
    string fileName,
    FileType fileType,
    int pageSize,
    long initialFileSize);
```

## 3. Unit Under Test

`FileLifecycleManager.CreateFile` is responsible for:

* Validating the input parameters.
* Checking whether the file already exists.
* Creating the physical file.
* Initializing the `FileHeader`.
* Initializing the `AllocationMetadata`.
* Initializing the `ExtentBitmap`.
* Writing the initialization structures to the physical file.
* Closing the temporary file handle after initialization.
* Rolling back the physical file when initialization fails.
* Returning the initialized `DataFile` after successful creation.

## 4. Mocked Dependencies

The following dependencies must be mocked in the unit tests:

* `IPhysicalFileSystem`
* `IFileWriter`

The unit tests must not create or modify real files on disk.

---

# Test Cases

## Case 1: Successful File Creation

### Input

```text
fileName        = "test.db"
fileType        = FileType.Data
pageSize        = 4096
initialFileSize = 1048576
```

### Preconditions

* The file `"test.db"` does not exist.
* All physical file and write operations succeed.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

* Returns a valid `DataFile`.
* The returned `DataFile` contains the correct:

  * File name.
  * File type.
  * Page size.
  * Initial file size.
  * File metadata.

### Expected State

* The physical file is created successfully.
* The `FileHeader` is initialized and written to the file.
* The `AllocationMetadata` is initialized and written to the file.
* The `ExtentBitmap` is initialized and written to the file.
* The temporary file handle is closed after initialization.
* The physical file is not deleted.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns false

IPhysicalFileSystem.Create("test.db", 1048576)
    -> returns fileHandle

FileWriter.WriteHeader(
    fileHandle,
    FileHeader)
    -> succeeds

FileWriter.WriteAllocationMetadata(
    fileHandle,
    FileHeader,
    AllocationMetadata)
    -> succeeds

FileWriter.WriteExtentBitmap(
    fileHandle,
    FileHeader,
    AllocationMetadata,
    ExtentBitmap)
    -> succeeds

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds
```

### Suggested Test Name

```csharp
CreateFile_ValidInput_ReturnsInitializedDataFile()
```

---

## Case 2: File Already Exists

### Input

```text
fileName        = "test.db"
fileType        = FileType.Data
pageSize        = 4096
initialFileSize = 1048576
```

### Preconditions

* The file `"test.db"` already exists.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

```text
Throws FileAlreadyExistsException
```

### Expected State

* No new physical file is created.
* No header is written.
* No allocation metadata is written.
* No extent bitmap is written.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns true
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Create(...)
FileWriter.WriteHeader(...)
FileWriter.WriteAllocationMetadata(...)
FileWriter.WriteExtentBitmap(...)
IPhysicalFileSystem.Close(...)
IPhysicalFileSystem.Delete(...)
```

### Suggested Test Name

```csharp
CreateFile_FileAlreadyExists_ThrowsFileAlreadyExistsException()
```

---

## Case 3: Physical File Creation Fails

### Input

```text
fileName        = "test.db"
fileType        = FileType.Data
pageSize        = 4096
initialFileSize = 1048576
```

### Preconditions

* The file does not exist.
* The operating system cannot create the physical file.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

```text
Throws FileCreationException
```

### Expected Exception

* The `FileCreationException` wraps the original `IOException`.

### Expected State

* No header is written.
* No allocation metadata is written.
* No extent bitmap is written.
* No rollback deletion is performed because the physical file was not created successfully.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns false

IPhysicalFileSystem.Create("test.db", 1048576)
    -> throws IOException
```

### Suggested Test Name

```csharp
CreateFile_PhysicalCreationFails_ThrowsFileCreationException()
```

---

## Case 4: Header Write Fails

### Input

```text
fileName        = "test.db"
fileType        = FileType.Data
pageSize        = 4096
initialFileSize = 1048576
```

### Preconditions

* The file does not exist.
* The physical file is created successfully.
* Writing the `FileHeader` fails.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

```text
Throws FileCreationException
```

### Expected Exception

* The `FileCreationException` wraps the `IOException` thrown by `WriteHeader`.

### Expected State

* The file handle is closed.
* The incomplete physical file is deleted.
* Allocation metadata is not written.
* The extent bitmap is not written.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns false

IPhysicalFileSystem.Create("test.db", 1048576)
    -> returns fileHandle

FileWriter.WriteHeader(
    fileHandle,
    FileHeader)
    -> throws IOException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds

IPhysicalFileSystem.Delete("test.db")
    -> succeeds
```

### Required Call Order

```text
IPhysicalFileSystem.Close(fileHandle)
    before
IPhysicalFileSystem.Delete("test.db")
```

### Suggested Test Name

```csharp
CreateFile_HeaderWriteFails_RollsBackPhysicalFile()
```

---

## Case 5: Allocation Metadata Write Fails

### Input

```text
fileName        = "test.db"
fileType        = FileType.Data
pageSize        = 4096
initialFileSize = 1048576
```

### Preconditions

* The file does not exist.
* The physical file is created successfully.
* The `FileHeader` is written successfully.
* Writing the `AllocationMetadata` fails.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

```text
Throws FileCreationException
```

### Expected Exception

* The `FileCreationException` wraps the `IOException` thrown by `WriteAllocationMetadata`.

### Expected State

* The file handle is closed.
* The incomplete physical file is deleted.
* The extent bitmap is not written.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns false

IPhysicalFileSystem.Create("test.db", 1048576)
    -> returns fileHandle

FileWriter.WriteHeader(
    fileHandle,
    FileHeader)
    -> succeeds

FileWriter.WriteAllocationMetadata(
    fileHandle,
    FileHeader,
    AllocationMetadata)
    -> throws IOException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds

IPhysicalFileSystem.Delete("test.db")
    -> succeeds
```

### Required Call Order

```text
IPhysicalFileSystem.Close(fileHandle)
    before
IPhysicalFileSystem.Delete("test.db")
```

### Suggested Test Name

```csharp
CreateFile_MetadataWriteFails_RollsBackPhysicalFile()
```

---

## Case 6: Extent Bitmap Write Fails

### Input

```text
fileName        = "test.db"
fileType        = FileType.Data
pageSize        = 4096
initialFileSize = 1048576
```

### Preconditions

* The file does not exist.
* The physical file is created successfully.
* The file header and allocation metadata are written successfully.
* Writing the `ExtentBitmap` fails.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

```text
Throws FileCreationException
```

### Expected Exception

* The `FileCreationException` wraps the `IOException` thrown by `WriteExtentBitmap`.

### Expected State

* The file handle is closed.
* The incomplete physical file is deleted.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns false

IPhysicalFileSystem.Create("test.db", 1048576)
    -> returns fileHandle

FileWriter.WriteHeader(...)
    -> succeeds

FileWriter.WriteAllocationMetadata(...)
    -> succeeds

FileWriter.WriteExtentBitmap(...)
    -> throws IOException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds

IPhysicalFileSystem.Delete("test.db")
    -> succeeds
```

### Required Call Order

```text
IPhysicalFileSystem.Close(fileHandle)
    before
IPhysicalFileSystem.Delete("test.db")
```

### Suggested Test Name

```csharp
CreateFile_BitmapWriteFails_RollsBackPhysicalFile()
```

---

## Case 7: Rollback Deletion Also Fails

### Description

Verifies that an exception thrown during rollback does not replace the original file creation exception.

### Preconditions

* The physical file is created successfully.
* The file header is written successfully.
* Writing the allocation metadata fails.
* Closing the file handle succeeds.
* Deleting the physical file during rollback fails.

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    1048576);
```

### Expected Output

```text
Throws FileCreationException
```

### Expected Exception

* The `FileCreationException` wraps the original metadata write exception.
* The exception thrown by `IPhysicalFileSystem.Delete` does not replace the original exception.

### Expected Dependency Calls

```text
FileWriter.WriteHeader(...)
    -> succeeds

FileWriter.WriteAllocationMetadata(...)
    -> throws metadataWriteException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds

IPhysicalFileSystem.Delete("test.db")
    -> throws rollbackDeleteException
```

### Expected Assertion

```text
FileCreationException.InnerException
    == metadataWriteException
```

### Suggested Test Name

```csharp
CreateFile_RollbackDeleteFails_PreservesOriginalException()
```

---

## Case 8: Non-Positive Page Size

### Input Examples

```text
pageSize = 0
pageSize = -512
```

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    pageSize,
    1048576);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Expected State

No physical file system or file writer dependency is called.

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Exists(...)
IPhysicalFileSystem.Create(...)
FileWriter.WriteHeader(...)
FileWriter.WriteAllocationMetadata(...)
FileWriter.WriteExtentBitmap(...)
IPhysicalFileSystem.Close(...)
IPhysicalFileSystem.Delete(...)
```

### Suggested Test Name

```csharp
CreateFile_NonPositivePageSize_ThrowsArgumentOutOfRangeException()
```

> [!NOTE]
> Both `pageSize = 0` and `pageSize = -512` exercise the same behavioral path: non-positive page size.
> These may be implemented as a single parameterized test case during test implementation.

---

## Case 9: Page Size Is Not a Power of Two

### Input

```text
pageSize = 4097
```

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4097,
    1048576);
```

### Expected Output

```text
Throws ArgumentException
```

### Expected State

No physical file system or file writer dependency is called.

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Exists(...)
IPhysicalFileSystem.Create(...)
FileWriter.WriteHeader(...)
FileWriter.WriteAllocationMetadata(...)
FileWriter.WriteExtentBitmap(...)
IPhysicalFileSystem.Close(...)
IPhysicalFileSystem.Delete(...)
```

### Suggested Test Name

```csharp
CreateFile_PageSizeNotPowerOfTwo_ThrowsArgumentException()
```

---

## Case 10: Invalid Initial File Size

### Input

```text
initialFileSize = -1
```

### Execution

```csharp
CreateFile(
    "test.db",
    FileType.Data,
    4096,
    -1);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Expected State

No physical file system or file writer dependency is called.

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Exists(...)
IPhysicalFileSystem.Create(...)
FileWriter.WriteHeader(...)
FileWriter.WriteAllocationMetadata(...)
FileWriter.WriteExtentBitmap(...)
IPhysicalFileSystem.Close(...)
IPhysicalFileSystem.Delete(...)
```

### Suggested Test Name

```csharp
CreateFile_NonPositiveInitialFileSize_ThrowsArgumentOutOfRangeException()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Valid input                     | Returns an initialized `DataFile`    |
|  2 | File already exists             | Throws `FileAlreadyExistsException`  |
|  3 | Physical file creation fails    | Throws `FileCreationException`       |
|  4 | Header write fails              | Rolls back and throws                |
|  5 | Metadata write fails            | Rolls back and throws                |
|  6 | Bitmap write fails              | Rolls back and throws                |
|  7 | Rollback deletion fails         | Preserves the original exception     |
|  8 | Page size is non-positive       | Throws `ArgumentOutOfRangeException` |
|  9 | Page size is not a power of two | Throws `ArgumentException`           |
| 10 | Initial file size is invalid    | Throws `ArgumentOutOfRangeException` |

## Total

```text
10 independent unit test behaviors
```
