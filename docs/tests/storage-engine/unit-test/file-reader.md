# Unit Test Specification — `FileReader`

## 1. Scope

This document covers unit test specifications for `FileReader`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileReader`
* **Methods:**

```csharp
int ReadAtOffset(
    OpenFileEntry entry,
    long offset,
    Memory<byte> destination);

FileHeader ReadHeader(
    FileHandle handle);

AllocationMetadata ReadAllocationMetadata(
    FileHandle handle,
    FileHeader header);

ExtentBitmap ReadExtentBitmap(
    FileHandle handle,
    FileHeader header,
    AllocationMetadata metadata);
```

## 3. Unit Under Test

`FileReader` is responsible for:

* Validating offset ranges against physical file bounds before reading.
* Invoking low-level handle reads (`ReadAtOffset`).
* Asserting that the physical byte count read matches requested page size (detecting partial/incomplete reads).
* Deserializing raw byte buffers from disk into structural header and allocation metadata representations.

## 4. Mocked Dependencies

* `IFileHandle`

---

# Test Cases

## Case 1: Successful ReadAtOffset

### Input

```text
entry       = openFileEntry
offset      = 4096
destination = Memory buffer (length 4096)
```

### Preconditions

* Handle is open.
* Requested offset is inside active file boundary.

### Execution

```csharp
ReadAtOffset(openFileEntry, 4096, destination);
```

### Expected Output

* Returns `4096` (number of bytes read).

### Expected State

* Buffer is populated with data.

### Expected Dependency Calls

```text
fileHandle.ReadAtOffset(destination, 4096)
    -> returns 4096
```

### Suggested Test Name

```csharp
ReadAtOffset_ValidRange_ReturnsBytesRead()
```

---

## Case 2: Offset Equals Zero

### Input

```text
offset      = 0
destination = Memory buffer (length 4096)
```

### Execution

```csharp
ReadAtOffset(openFileEntry, 0, destination);
```

### Expected Output

* Returns `4096`.

### Expected Dependency Calls

```text
fileHandle.ReadAtOffset(destination, 0)
    -> returns 4096
```

### Suggested Test Name

```csharp
ReadAtOffset_ZeroOffset_ReadsFirstBlock()
```

---

## Case 3: Read at Valid Upper Boundary

### Input

```text
offset = 1044480 // (1MB - PageSize)
```

### Preconditions

* File physical size is 1MB (1,048,576 bytes).

### Execution

```csharp
ReadAtOffset(openFileEntry, 1044480, destination);
```

### Expected Output

* Returns `4096` (successfully fits bounds).

### Suggested Test Name

```csharp
ReadAtOffset_LastPageBoundary_Succeeds()
```

---

## Case 4: Negative Offset Rejection

### Input

```text
offset = -4096
```

### Execution

```csharp
ReadAtOffset(openFileEntry, -4096, destination);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Forbidden Dependency Calls

```text
fileHandle.ReadAtOffset(...)
```

### Suggested Test Name

```csharp
ReadAtOffset_NegativeOffset_ThrowsArgumentOutOfRangeException()
```

---

## Case 5: Range Exceeds File Size

### Input

```text
offset = 1048576 (1MB)
```

### Preconditions

* Physical file size is 1MB.

### Execution

```csharp
ReadAtOffset(openFileEntry, 1048576, destination);
```

### Expected Output

```text
Throws ArgumentOutOfRangeException
```

### Suggested Test Name

```csharp
ReadAtOffset_OffsetBeyondBounds_ThrowsArgumentOutOfRangeException()
```

---

## Case 6: Handle Already Closed Error

### Preconditions

* Associated handle is disposed or closed.

### Execution

```csharp
ReadAtOffset(openFileEntry, 4096, destination);
```

### Expected Output

```text
Throws ObjectDisposedException
```

### Suggested Test Name

```csharp
ReadAtOffset_HandleClosed_ThrowsObjectDisposedException()
```

---

## Case 7: Partial Read Error

### Input

```text
destination = buffer (length 4096)
```

### Preconditions

* Low-level read return value is lower than requested buffer size.

### Execution

```csharp
ReadAtOffset(openFileEntry, 4096, destination);
```

### Expected Output

```text
Throws IncompletePageReadException
```

### Expected Dependency Calls

```text
fileHandle.ReadAtOffset(destination, 4096)
    -> returns 2048
```

### Suggested Test Name

```csharp
ReadAtOffset_IncompleteRead_ThrowsIncompletePageReadException()
```

---

## Case 8: Exact Read Not Enough Bytes

### Description

Asserts that even if no exception is thrown by OS, missing even 1 byte triggers an exception.

### Input

```text
entry       = openFileEntry
offset      = 4096
destination = Memory buffer (length 4096)
```

### Preconditions

* Handle is open.
* Requested offset is inside file bounds.

### Execution

```csharp
ReadAtOffset(openFileEntry, 4096, destination);
```

### Dependency Calls

```text
fileHandle.ReadAtOffset(destination, 4096)
    -> returns 4095
```

### Expected Output

```text
Throws IncompletePageReadException
```

### Suggested Test Name

```csharp
ReadAtOffset_MissingBytes_ThrowsIncompletePageReadException()
```

---

## Case 9: OS Read Failure Recovery

### Preconditions

* Low-level filesystem throws an `IOException`.

### Execution

```csharp
ReadAtOffset(openFileEntry, 4096, destination);
```

### Expected Output

```text
Throws ReadFailureException
```

### Expected Exception

* Wraps the original `IOException`.

### Expected Dependency Calls

```text
fileHandle.ReadAtOffset(destination, 4096)
    -> throws IOException
```

### Suggested Test Name

```csharp
ReadAtOffset_OSIOException_ThrowsReadFailureException()
```

---

## Case 10: ReadHeader Deserialization

### Input

```text
handle = fileHandle
```

### Preconditions

* Handle is open.
* Raw bytes at offset `0` contain a valid serialized `FileHeader`.

### Execution

```csharp
ReadHeader(fileHandle);
```

### Expected Output

* Returns a valid `FileHeader` object.

### Expected Dependency Calls

```text
fileHandle.ReadAtOffset(headerBytes, 0)
    -> returns bytes
```

### Suggested Test Name

```csharp
ReadHeader_ValidHandle_ParsesBytesToHeader()
```

---

## Case 11: ReadAllocationMetadata Deserialization

### Input

```text
handle = fileHandle
header = fileHeader
```

### Preconditions

* Handle is open.
* Raw bytes at the metadata offset (from `fileHeader`) contain a valid serialized `AllocationMetadata`.

### Execution

```csharp
ReadAllocationMetadata(fileHandle, fileHeader);
```

### Expected Output

* Returns a valid reconstructed `AllocationMetadata` structure.

### Suggested Test Name

```csharp
ReadAllocationMetadata_ValidOffset_ParsesMetadata()
```

---

## Case 12: ReadExtentBitmap Deserialization

### Input

```text
handle   = fileHandle
header   = fileHeader
metadata = allocationMetadata (TotalExtentCount = 16)
```

### Preconditions

* Handle is open.
* `allocationMetadata.TotalExtentCount == 16`.
* Raw bytes at the bitmap offset contain 16 valid bits.

### Execution

```csharp
ReadExtentBitmap(fileHandle, fileHeader, allocationMetadata);
```

### Expected Output

* Returns a valid reconstructed `ExtentBitmap` whose bit count matches the `TotalExtentCount` in `allocationMetadata`.

### Suggested Test Name

```csharp
ReadExtentBitmap_ValidOffset_ParsesBitmap()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Valid page read                 | Returns bytes read                   |
|  2 | Read at offset 0                | Reads header/first page successfully |
|  3 | Read last page boundary         | Fits bounds and reads successfully   |
|  4 | Negative offset                 | Throws `ArgumentOutOfRangeException` |
|  5 | Range exceeds file size         | Throws `ArgumentOutOfRangeException` |
|  6 | Closed handles                  | Throws `ObjectDisposedException`     |
|  7 | Partial read                    | Throws `IncompletePageReadException` |
|  8 | Missing bytes                   | Throws `IncompletePageReadException` |
|  9 | OS read exception               | Throws `ReadFailureException`        |
| 10 | Header deserialization          | Returns parsed header                |
| 11 | Metadata deserialization        | Returns parsed metadata              |
| 12 | Bitmap deserialization          | Returns parsed bitmap                |

## Total

```text
12 independent unit test behaviors
```
