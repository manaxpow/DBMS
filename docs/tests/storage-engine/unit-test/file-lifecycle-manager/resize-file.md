# Unit Test Specification — `FileLifecycleManager.ResizeFile`

## 1. Scope

This document covers unit test specifications for `FileLifecycleManager.ResizeFile`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileLifecycleManager`
* **Method:**

```csharp
void ResizeFile(
    OpenFileEntry entry,
    long newSize);
```

## 3. Unit Under Test

`FileLifecycleManager.ResizeFile` is responsible for:

* Validating arguments (rejecting non-aligned or negative sizes).
* Checking configuration limits like maximum database capacities.
* Distinguishing between extensions (size increases) and truncations (size decreases).
* Verifying whether target truncate ranges contain active allocations before shrinking.
* Changing physical lengths using OS file handlers.
* Re-computing and adding new extent mappings in the metadata representation.
* Committing structural updates (such as allocation metadata headers) and force committing flushes.
* Updating current state memory descriptors.

## 4. Mocked Dependencies

* `IFileWriter`
* `IFileSynchronizer`
* `IFileHandle`

---

# Test Cases

## Case 1: Extend File Successfully

### Input

```text
entry   = openFileEntry (CurrentSize = 1MB)
newSize = 2097152 (2MB)
```

### Preconditions

* Physical disk operations, metadata writes, and sync calls succeed.

### Execution

```csharp
ResizeFile(openFileEntry, 2097152);
```

### Expected Output

* Void return (Success).

### Expected State

* Physical length is set to 2MB.
* New extents are added to allocation maps.
* `openFileEntry.DataFile.CurrentSize` is updated to 2MB.

### Expected Dependency Calls

```text
dataFile.Size
    -> returns 1048576

IPhysicalFileSystem.Resize(fileHandle, 2097152)
    -> succeeds

AllocationMetadata.AddExtents(16) // (2MB - 1MB) / 65536 bytes
    -> succeeds

FileWriter.WriteAllocationMetadata(fileHandle, header, metadata)
    -> succeeds

FileSynchronizer.Sync(openFileEntry)
    -> succeeds
```

### Suggested Test Name

```csharp
ResizeFile_IncreaseSize_PhysicallyExtendsAndUpdatesMetadata()
```

---

## Case 2: Truncate File Safely

### Input

```text
entry   = openFileEntry (CurrentSize = 2MB)
newSize = 1048576 (1MB)
```

### Preconditions

* The upper 1MB range contains only unallocated (free) extents.

### Execution

```csharp
ResizeFile(openFileEntry, 1048576);
```

### Expected Output

* Void return (Success).

### Expected State

* Physical length is shrunk to 1MB.
* In-memory metadata removes upper extents.

### Expected Dependency Calls

```text
dataFile.Size
    -> returns 2097152

AllocationMetadata.CanTruncateTo(16)
    -> returns true

AllocationMetadata.TruncateTo(16)
    -> succeeds

FileWriter.WriteAllocationMetadata(fileHandle, header, metadata)
    -> succeeds

IPhysicalFileSystem.Resize(fileHandle, 1048576)
    -> succeeds

FileSynchronizer.Sync(openFileEntry)
    -> succeeds
```

### Suggested Test Name

```csharp
ResizeFile_DecreaseSizeSafeRange_ShrinksAndTruncatesMetadata()
```

---

## Case 3: Resize No-Op

### Input

```text
entry   = openFileEntry (CurrentSize = 1MB)
newSize = 1048576
```

### Execution

```csharp
ResizeFile(openFileEntry, 1048576);
```

### Expected Output

* Void return (Success).

### Expected Dependency Calls

```text
dataFile.Size
    -> returns 1048576
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Resize(fileHandle, ...)
FileWriter.WriteAllocationMetadata(...)
FileSynchronizer.Sync(...)
```

### Suggested Test Name

```csharp
ResizeFile_SizeUnchanged_NoOperationsExecuted()
```

---

## Case 4: Invalid Size Arguments Rejection

### Input Examples

```text
newSize = -500
newSize = 1044481 (Not page/extent size aligned)
```

### Execution

```csharp
ResizeFile(openFileEntry, newSize);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Forbidden Dependency Calls

```text
fileHandle.GetLength(...)
IPhysicalFileSystem.Resize(fileHandle, ...)
```

### Suggested Test Name

```csharp
ResizeFile_InvalidAlignmentOrNegativeSize_ThrowsArgumentOutOfRangeException()
```

---

## Case 5: Truncate Cuts Into Used Extent

### Input

```text
entry   = openFileEntry (CurrentSize = 2MB)
newSize = 1048576 (1MB)
```

### Preconditions

* At least one allocated extent sits in the upper 1MB boundary segment.

### Execution

```csharp
ResizeFile(openFileEntry, 1048576);
```

### Expected Output

```text
Throws FileTruncationException
```

### Expected State

* File length and metadata allocations are unmodified.

### Expected Dependency Calls

```text
dataFile.Size
    -> returns 2097152

AllocationMetadata.CanTruncateTo(16)
    -> returns false
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Resize(fileHandle, ...)
FileWriter.WriteAllocationMetadata(...)
```

### Suggested Test Name

```csharp
ResizeFile_ShrinkCutsIntoAllocatedExtents_ThrowsFileTruncationException()
```

---

## Case 6: Maximum File Size Exceeded

### Input

```text
entry   = openFileEntry
newSize = 20971520 (20MB)
```

### Preconditions

* File's `MaximumSize` is capped at 10MB.

### Execution

```csharp
ResizeFile(openFileEntry, 20971520);
```

### Expected Output

```text
Throws MaximumFileSizeExceededException
```

### Suggested Test Name

```csharp
ResizeFile_ExceedsMaximumCap_ThrowsMaximumFileSizeExceededException()
```

---

## Case 7: Physical OS SetLength Failure Recovery

### Preconditions

* OS `SetLength` throws an `IOException`.

### Execution

```csharp
ResizeFile(openFileEntry, 2097152);
```

### Expected Output

```text
Throws FileResizeException
```

### Expected Exception

* Wraps the root `IOException` thrown by OS.

### Expected State

* In-memory metadata remains unmodified and perfectly consistent.

### Expected Dependency Calls

```text
dataFile.Size
    -> returns 1048576

IPhysicalFileSystem.Resize(fileHandle, 2097152)
    -> throws IOException
```

### Forbidden Dependency Calls

```text
AllocationMetadata.AddExtents(...)
FileWriter.WriteAllocationMetadata(...)
```

### Suggested Test Name

```csharp
ResizeFile_OSSetLengthThrows_ThrowsFileResizeException()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Extend file success             | Sets physical size, adds extents     |
|  2 | Truncate file safe              | Removes extents, shrinks file        |
|  3 | Resize no-op                    | Skips execution                      |
|  4 | Invalid size arguments          | Throws `ArgumentOutOfRangeException` |
|  5 | Shrink cuts into used extents   | Throws `FileTruncationException`     |
|  6 | Exceeds maximum cap             | Throws `MaximumFileSizeExceededException`|
|  7 | Physical resize fails           | Throws `FileResizeException`         |

## Total

```text
7 independent unit test behaviors
```
