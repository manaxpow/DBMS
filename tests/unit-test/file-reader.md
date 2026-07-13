# Unit Test Spec - FileReader

## Component
* **Class**: `FileReader`
* **Interface**: `IFileReader`

---

## Test Cases

### Case 1: ReadAtOffset Success (Happy Path)
* **Input**: `entry = openFileEntry`, `offset = 4096`, `destination = Memory buffer (length 4096)`
* **Preconditions**: Handle is valid and page range is within bounds.
* **Execution**: `ReadAtOffset(openFileEntry, 4096, destination)`
* **Expected Output**: Returns `4096` (bytes read).
* **Expected State**: Buffer populated.
* **Dependency Calls**:
  * `FileHandle.ReadAtOffset(destination, 4096)` -> returns `4096`

### Case 2: Offset equals 0
* **Input**: `offset = 0`, `destination = buffer (length 4096)`
* **Description**: Verifies reading the header/first block page offset of the database file.
* **Dependency Calls**:
  * `FileHandle.ReadAtOffset(destination, 0)` -> returns `4096`

### Case 3: Read at Valid Boundary
* **Input**: `offset = 1044480` (Last page offset in 1MB file, where page size = 4096)
* **Preconditions**: File physical size is 1MB (1,048,576 bytes).
* **Execution**: `ReadAtOffset(openFileEntry, 1044480, destination)`
* **Expected Output**: Returns `4096`. Fits exactly within bounds.

### Case 4: Negative Offset Rejection
* **Input**: `offset = -4096`
* **Execution**: `ReadAtOffset(openFileEntry, -4096, destination)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.

### Case 5: Range Exceeds File Size
* **Input**: `offset = 1048576 (1MB)`, `destination = buffer (length 4096)`
* **Preconditions**: File is 1MB.
* **Execution**: `ReadAtOffset(openFileEntry, 1048576, destination)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.

### Case 6: Handle Already Closed Error
* **Input**: `entry = openFileEntry` (where handle is closed/disposed)
* **Execution**: `ReadAtOffset(openFileEntry, 4096, destination)`
* **Expected Output**: Throws `ObjectDisposedException`.

### Case 7: Partial Read Handled
* **Description**: Verifies that partial reads from the OS stream are flagged if they return fewer bytes than the expected length.
* **Input**: `offset = 4096`, `destination = buffer (length 4096)`
* **Dependency Calls**:
  * `FileHandle.ReadAtOffset(destination, 4096)` -> returns `2048`
* **Expected Output**: Throws `IncompletePageReadException`.

### Case 8: Exact Read Not Enough Bytes
* **Description**: Verifies that even if the OS read finishes without error, if the total read byte count is not exactly equal to the page size, it triggers verification errors.
* **Dependency Calls**:
  * `FileHandle.ReadAtOffset(destination, 4096)` -> returns `4095` (1 byte missing)
* **Expected Output**: Throws `IncompletePageReadException`.

### Case 9: OS Read Failure Recovery
* **Preconditions**: OS hardware reads throw `IOException`.
* **Execution**: `ReadAtOffset(...)`
* **Expected Output**: Throws `ReadFailureException` (wrapping `IOException`).
* **Dependency Calls**:
  * `FileHandle.ReadAtOffset(...)` -> throws `IOException`

### Case 10: ReadHeader Deserialize Correctly
* **Input**: `handle = fileHandle`
* **Execution**: `ReadHeader(fileHandle)`
* **Expected Output**: Returns a valid `FileHeader` with exact parsed magic number, format version, and metadata offsets.
* **Dependency Calls**:
  * `FileHandle.ReadAtOffset(headerBytes, 0)` -> returns header bytes

### Case 11: ReadAllocationMetadata Parsed Correctly
* **Input**: `handle = fileHandle`, `header = fileHeader`
* **Execution**: `ReadAllocationMetadata(fileHandle, fileHeader)`
* **Expected Output**: Returns `AllocationMetadata` with correct extent counters matching parsed disk offset.
