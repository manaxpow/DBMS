# Unit Test Specification — `FileWriter`

## 1. Scope

This document covers unit test specifications for `FileWriter`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileWriter`
* **Methods:**

```csharp
void WriteAtOffset(
    OpenFileEntry entry,
    long offset,
    ReadOnlyMemory<byte> source);

void WriteHeader(
    FileHandle handle,
    FileHeader header);

void WriteAllocationMetadata(
    FileHandle handle,
    FileHeader header,
    AllocationMetadata metadata);

void WriteExtentBitmap(
    FileHandle handle,
    FileHeader header,
    AllocationMetadata metadata,
    ExtentBitmap bitmap);
```

## 3. Unit Under Test

`FileWriter` is responsible for:

* Rejecting write commands if the file is opened under `ReadOnly` access modes.
* Validating offset ranges against physical file bounds before writing.
* Invoking low-level handle writes (`WriteAtOffset`).
* Asserting that physical byte counts written match buffer size (detecting partial/incomplete writes).
* Serializing structure elements (like `FileHeader` and `AllocationMetadata`) into raw byte blocks and persisting them to disk.

## 4. Mocked Dependencies

* `IFileHandle`

---

# Test Cases

## Case 1: Successful WriteAtOffset

### Input

```text
entry  = openFileEntry
offset = 4096
source = ReadOnlyMemory buffer (length 4096)
```

### Preconditions

* Handle is open with `AccessMode = ReadWrite`.
* Range is valid.

### Execution

```csharp
WriteAtOffset(openFileEntry, 4096, source);
```

### Expected Output

* Void return (Success).

### Expected State

* Disk offset contains written data.

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(source, 4096)
    -> succeeds
```

### Suggested Test Name

```csharp
WriteAtOffset_ValidInputs_WritesDataSuccessfully()
```

---

## Case 2: Write to Valid Upper Boundary

### Input

```text
offset = 1044480
```

### Preconditions

* Physical size of file is 1MB.

### Execution

```csharp
WriteAtOffset(openFileEntry, 1044480, source);
```

### Expected Output

* Void return (Success). Fits exactly inside physical file capacity.

### Suggested Test Name

```csharp
WriteAtOffset_LastPageBoundary_Succeeds()
```

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(source, 1044480)
    -> succeeds
```

---

## Case 3: Negative Offset Rejection

### Input

```text
offset = -4096
```

### Execution

```csharp
WriteAtOffset(openFileEntry, -4096, source);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Forbidden Dependency Calls

```text
fileHandle.WriteAtOffset(...)
```

### Suggested Test Name

```csharp
WriteAtOffset_NegativeOffset_ThrowsArgumentOutOfRangeException()
```

---

## Case 4: Range Exceeds Allowed Bounds

### Input

```text
offset = 1048576 (1MB)
```

### Preconditions

* File physical length is 1MB.

### Execution

```csharp
WriteAtOffset(openFileEntry, 1048576, source);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Suggested Test Name

```csharp
WriteAtOffset_OffsetBeyondBounds_ThrowsArgumentOutOfRangeException()
```

---

## Case 5: Read-Only Access Denied

### Preconditions

* File entry was opened with `AccessMode = ReadOnly`.

### Execution

```csharp
WriteAtOffset(openFileEntry, 4096, source);
```

### Expected Output

```text
Throws ReadOnlyFileException
```

### Forbidden Dependency Calls

```text
fileHandle.WriteAtOffset(...)
```

### Suggested Test Name

```csharp
WriteAtOffset_ReadOnlyFile_ThrowsReadOnlyFileException()
```

---

## Case 6: Handle Already Closed Error

### Preconditions

* File handle is closed.

### Execution

```csharp
WriteAtOffset(openFileEntry, 4096, source);
```

### Expected Output

```text
Throws ObjectDisposedException
```

### Suggested Test Name

```csharp
WriteAtOffset_HandleClosed_ThrowsObjectDisposedException()
```

---

## Case 7: Partial Write Error

### Preconditions

* Physical OS write writes fewer bytes than the page size length.

### Execution

```csharp
WriteAtOffset(openFileEntry, 4096, source);
```

### Expected Output

```text
Throws IncompletePageWriteException
```

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(source, 4096)
    -> returns 2048
```

### Suggested Test Name

```csharp
WriteAtOffset_IncompleteWrite_ThrowsIncompletePageWriteException()
```

---

## Case 8: OS Write Failure Recovery

### Preconditions

* Low-level write operations throw `IOException`.

### Execution

```csharp
WriteAtOffset(openFileEntry, 4096, source);
```

### Expected Output

```text
Throws WriteFailureException
```

### Expected Exception

* Wraps the original `IOException`.

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(source, 4096)
    -> throws IOException
```

### Suggested Test Name

```csharp
WriteAtOffset_OSIOException_ThrowsWriteFailureException()
```

---

## Case 9: WriteHeader Serialization

### Input

```text
handle = fileHandle
header = fileHeader
```

### Preconditions

* Handle is open with write access.

### Execution

```csharp
WriteHeader(fileHandle, fileHeader);
```

### Expected Output

* Void return (Success).

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(serializedBytes, 0)
    -> succeeds
```

### Suggested Test Name

```csharp
WriteHeader_ValidHeader_SerializesAndWrites()
```

---

## Case 10: WriteAllocationMetadata Serialization

### Input

```text
handle   = fileHandle
header   = fileHeader
metadata = allocationMetadata
```

### Preconditions

* Handle is open with write access.

### Execution

```csharp
WriteAllocationMetadata(fileHandle, fileHeader, allocationMetadata);
```

### Expected Output

* Void return (Success).

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(serializedBytes, header.AllocationMetadataOffset)
    -> succeeds
```

### Suggested Test Name

```csharp
WriteAllocationMetadata_ValidMetadata_SerializesAndWrites()
```

---

## Case 11: WriteExtentBitmap Serialization

### Input

```text
handle   = fileHandle
header   = fileHeader
metadata = allocationMetadata
bitmap   = extentBitmap
```

### Preconditions

* Handle is open with write access.

### Execution

```csharp
WriteExtentBitmap(fileHandle, fileHeader, allocationMetadata, extentBitmap);
```

### Expected Output

* Void return (Success).

### Expected Dependency Calls

```text
fileHandle.WriteAtOffset(serializedBytes, header.ExtentBitmapOffset)
    -> succeeds
```

### Suggested Test Name

```csharp
WriteExtentBitmap_ValidBitmap_SerializesAndWrites()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Valid page write                | Writes bytes successfully            |
|  2 | Last page boundary write        | Fits bounds and writes successfully  |
|  3 | Negative offset                 | Throws `ArgumentOutOfRangeException` |
|  4 | Offset beyond bounds            | Throws `ArgumentOutOfRangeException` |
|  5 | Read-only file write            | Throws `ReadOnlyFileException`       |
|  6 | Handle already closed           | Throws `ObjectDisposedException`     |
|  7 | Incomplete/Partial write        | Throws `IncompletePageWriteException`|
|  8 | OS write failure                | Throws `WriteFailureException`       |
|  9 | Header serialization            | Serializes and writes successfully   |
| 10 | Metadata serialization          | Serializes and writes successfully   |
| 11 | Bitmap serialization            | Serializes and writes successfully   |

## Total

```text
11 independent unit test behaviors
```
