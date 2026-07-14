# Unit Test Specification — `FileLifecycleManager.OpenFile`

## 1. Scope

This document covers unit test specifications for `FileLifecycleManager.OpenFile`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileLifecycleManager`
* **Method:**

```csharp
OpenFileEntry OpenFile(
    string fileName,
    FileAccessMode accessMode,
    FileLockMode lockMode);
```

## 3. Unit Under Test

`FileLifecycleManager.OpenFile` is responsible for:

* Checking whether the file is already open.
* Reusing the existing handle and incrementing reference counts if locks are compatible.
* Rejecting openings if lock mode conflicts or access mode mismatches are detected.
* Verifying physical file existence on disk.
* Opening the physical file handle.
* Reading header, allocation metadata, and extent bitmap segments.
* Invoking validator checks.
* Registering and returning the newly initialized `OpenFileEntry`.
* Closing active temporary handles on any step failure.

## 4. Mocked Dependencies

* `IOpenFileManager`
* `IPhysicalFileSystem`
* `IFileReader`
* `IFileValidator`

---

# Test Cases

## Case 1: First-Time Open Success

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadWrite
lockMode   = FileLockMode.Exclusive
```

### Preconditions

* File exists on disk.
* File is not registered in `OpenFileManager`.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
```

### Expected Output

* Returns a valid `OpenFileEntry` wrapping the reconstructed structures and handles.

### Expected State

* Entry is registered with `ReferenceCount == 1`.

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("test.db")
    -> returns null

IPhysicalFileSystem.Exists("test.db")
    -> returns true

IPhysicalFileSystem.Open("test.db", FileAccessMode.ReadWrite)
    -> returns fileHandle

FileReader.ReadHeader(fileHandle)
    -> returns fileHeader

FileReader.ReadAllocationMetadata(fileHandle, fileHeader)
    -> returns allocationMetadata

FileReader.ReadExtentBitmap(fileHandle, fileHeader, allocationMetadata)
    -> returns extentBitmap

FileValidator.Validate(fileHeader, allocationMetadata, extentBitmap, physicalSize)
    -> succeeds

OpenFileManager.RegisterOpenFile("test.db", openFileEntry)
    -> succeeds
```

### Suggested Test Name

```csharp
OpenFile_FirstTime_ReturnsRegisteredEntry()
```

---

## Case 2: Already Open Compatible RefCount Increment

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadOnly
lockMode   = FileLockMode.Shared
```

### Preconditions

* File is already registered in `OpenFileManager` with `AccessMode = ReadOnly` and `LockMode = Shared`.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadOnly, FileLockMode.Shared);
```

### Expected Output

* Returns the existing `OpenFileEntry`.

### Expected State

* ReferenceCount increments to `2`.

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("test.db")
    -> returns existingOpenFileEntry

existingOpenFileEntry.IncrementRefCount()
    -> returns 2
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Open(...)
FileReader.ReadHeader(...)
FileValidator.Validate(...)
```

### Suggested Test Name

```csharp
OpenFile_AlreadyOpenCompatible_IncrementsRefCount()
```

---


## Case 3: Exclusive Lock Conflict

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadWrite
lockMode   = FileLockMode.Exclusive
```

### Preconditions

* File is already registered in `OpenFileManager` with `LockMode = Shared`.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
```

### Expected Output

```text
Throws LockConflictException
```

### Suggested Test Name

```csharp
OpenFile_ExclusiveLockConflict_ThrowsLockConflictException()
```

---

## Case 4: Access Mode Conflict

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadWrite
lockMode   = FileLockMode.Shared
```

### Preconditions

* File is already registered in `OpenFileManager` with `AccessMode = ReadOnly`.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Shared);
```

### Expected Output

```text
Throws LockConflictException
```

### Suggested Test Name

```csharp
OpenFile_AccessModeConflict_ThrowsLockConflictException()
```

---

## Case 5: File Does Not Exist

### Input

```text
fileName   = "missing.db"
accessMode = FileAccessMode.ReadOnly
lockMode   = FileLockMode.Shared
```

### Preconditions

* File does not exist on disk.

### Execution

```csharp
OpenFile("missing.db", FileAccessMode.ReadOnly, FileLockMode.Shared);
```

### Expected Output

```text
Throws FileNotFoundException
```

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("missing.db")
    -> returns null

IPhysicalFileSystem.Exists("missing.db")
    -> returns false
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Open(...)
```

### Suggested Test Name

```csharp
OpenFile_FileDoesNotExist_ThrowsFileNotFoundException()
```

---

## Case 6: OS Open Physical Handle Failure

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadWrite
lockMode   = FileLockMode.Exclusive
```

### Preconditions

* File exists but physical OS call returns access error.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
```

### Expected Output

```text
Throws FileOpenException
```

### Expected Exception

* Wraps the root `IOException`.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Exists("test.db")
    -> returns true

IPhysicalFileSystem.Open("test.db", FileAccessMode.ReadWrite)
    -> throws IOException
```

### Suggested Test Name

```csharp
OpenFile_PhysicalOpenFails_ThrowsFileOpenException()
```

---

## Case 7: Read Header Failure Recovery

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadWrite
lockMode   = FileLockMode.Exclusive
```

### Preconditions

* OS open succeeds, but reading header block throws.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
```

### Expected Output

```text
Throws FileOpenException
```

### Expected State

* File handle is closed to prevent resource leaks.

### Expected Dependency Calls

```text
IPhysicalFileSystem.Open("test.db", FileAccessMode.ReadWrite)
    -> returns fileHandle

FileReader.ReadHeader(fileHandle)
    -> throws IOException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds
```

### Suggested Test Name

```csharp
OpenFile_HeaderReadFails_ClosesHandleAndThrows()
```

---

## Case 8: Read Allocation Metadata Failure Recovery

### Input

```text
fileName   = "test.db"
accessMode = FileAccessMode.ReadWrite
lockMode   = FileLockMode.Exclusive
```

### Preconditions

* Header reads succeed, but metadata reads throw.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
```

### Expected Output

```text
Throws FileOpenException
```

### Expected State

* Handle is closed.

### Expected Dependency Calls

```text
FileReader.ReadHeader(...)
    -> returns fileHeader

FileReader.ReadAllocationMetadata(fileHandle, fileHeader)
    -> throws IOException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds
```

### Suggested Test Name

```csharp
OpenFile_MetadataReadFails_ClosesHandleAndThrows()
```

---

## Case 9: Validation Failure

### Preconditions

* Reads succeed, but file validator fails checks.

### Execution

```csharp
OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
```

### Expected Output

```text
Throws InvalidFileFormatException
```

### Expected State

* Handle is closed.
* Entry is not registered.

### Expected Dependency Calls

```text
FileValidator.Validate(...)
    -> throws InvalidFileFormatException

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds
```

### Forbidden Dependency Calls

```text
OpenFileManager.RegisterOpenFile(...)
```

### Suggested Test Name

```csharp
OpenFile_ValidationFails_ClosesHandleAndThrows()
```

---

# Summary

| ID | Scenario                           | Expected Result                       |
| -: | ---------------------------------- | ------------------------------------- |
|  1 | First-time open success            | Returns a valid `OpenFileEntry`       |
|  2 | Reopen compatible                  | Increments `ReferenceCount`           |
|  3 | Exclusive lock conflict            | Throws `LockConflictException`        |
|  4 | Access mode conflict               | Throws `LockConflictException`        |
|  5 | File does not exist                | Throws `FileNotFoundException`        |
|  6 | OS open handle fails               | Throws `FileOpenException`            |
|  7 | Read header fails                  | Closes handle and throws              |
|  8 | Read metadata fails                | Closes handle and throws              |
|  9 | Validation fails                   | Closes handle and throws              |

## Total

```text
9 independent unit test behaviors
```
