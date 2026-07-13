# Unit Test Spec - FileWriter

## Component
* **Class**: `FileWriter`
* **Interface**: `IFileWriter`

---

## Test Cases

### Case 1: WriteAtOffset Success (Happy Path)
* **Input**: `entry = openFileEntry`, `offset = 4096`, `source = ReadOnlyMemory buffer (length 4096)`
* **Preconditions**: Handle is write-enabled (`AccessMode = ReadWrite`) and offset is valid.
* **Execution**: `WriteAtOffset(openFileEntry, 4096, source)`
* **Expected Output**: Void return (Success).
* **Expected State**: Data is written.
* **Dependency Calls**:
  * `FileHandle.WriteAtOffset(source, 4096)` -> success

### Case 2: Write to Valid Boundary
* **Input**: `offset = 1044480` (Last page offset in 1MB file)
* **Execution**: `WriteAtOffset(openFileEntry, 1044480, source)`
* **Expected Output**: Void return (Success). Fits exactly inside physical file capacity.

### Case 3: Negative Offset Rejection
* **Input**: `offset = -4096`
* **Execution**: `WriteAtOffset(openFileEntry, -4096, source)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.

### Case 4: Range Exceeds Allowed bounds
* **Input**: `offset = 1048576 (1MB)`, `source = buffer (length 4096)`
* **Preconditions**: File physical length is 1MB.
* **Execution**: `WriteAtOffset(openFileEntry, 1048576, source)`
* **Expected Output**: Throws `ArgumentOutOfRangeException` (preventing physical file sizes mismatch without using `ResizeFile`).

### Case 5: Read-Only Access Denied
* **Input**: `entry = openFileEntry` (where `AccessMode = ReadOnly`)
* **Execution**: `WriteAtOffset(openFileEntry, 4096, source)`
* **Expected Output**: Throws `ReadOnlyFileException`.
* **Dependency Calls**:
  * `ValidateAccessMode(ReadOnly)` -> throws `ReadOnlyFileException`

### Case 6: Handle Already Closed Error
* **Input**: `entry = openFileEntry` (handle closed)
* **Execution**: `WriteAtOffset(openFileEntry, 4096, source)`
* **Expected Output**: Throws `ObjectDisposedException`.

### Case 7: Partial Write Error
* **Description**: Verifies that if the OS writes fewer bytes than expected, it throws an exception.
* **Input**: `offset = 4096`, `source = buffer (length 4096)`
* **Dependency Calls**:
  * `FileHandle.WriteAtOffset(source, 4096)` -> returns `2048`
* **Expected Output**: Throws `IncompletePageWriteException`.

### Case 8: OS Write Failure Recovery
* **Preconditions**: OS hardware write throws `IOException`.
* **Execution**: `WriteAtOffset(...)`
* **Expected Output**: Throws `WriteFailureException` (wrapping `IOException`).
* **Dependency Calls**:
  * `FileHandle.WriteAtOffset(...)` -> throws `IOException`

### Case 9: WriteHeader Serializes Correctly
* **Input**: `handle = fileHandle`, `header = fileHeader`
* **Execution**: `WriteHeader(fileHandle, fileHeader)`
* **Expected Output**: Bytes written matches exact binary layout of format signatures and metadata offsets.
* **Dependency Calls**:
  * `FileHandle.WriteAtOffset(serializedBytes, 0)` -> success

### Case 10: WriteAllocationMetadata Serializes Correctly
* **Input**: `handle = fileHandle`, `header = fileHeader`, `metadata = allocationMetadata`
* **Execution**: `WriteAllocationMetadata(fileHandle, fileHeader, allocationMetadata)`
* **Expected Output**: Binary serialization matches correct layout of extent counts and bitmap segments.
