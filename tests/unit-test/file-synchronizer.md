# Unit Test Spec - FileSynchronizer

## Component
* **Class**: `FileSynchronizer`
* **Interface**: `IFileSynchronizer`

---

## Test Cases

### Case 1: Sync Calls FileHandle.FlushToDisk() (Happy Path)
* **Input**: `entry = openFileEntry`
* **Preconditions**: Valid open file handle is active.
* **Execution**: `Sync(openFileEntry)`
* **Expected Output**: Void return (Success).
* **Dependency Calls**:
  * `openFileEntry.Handle` -> returns `fileHandle`
  * `fileHandle.FlushToDisk()` -> success

### Case 2: FlushToDisk Succeeds
* **Description**: Verifies that the low-level operating-system flush call is executed cleanly.
* **Dependency Calls**: Verify `fileHandle.FlushToDisk()` is called once.

### Case 3: Handle Already Closed Error
* **Input**: `entry = openFileEntry` (handle closed)
* **Execution**: `Sync(openFileEntry)`
* **Expected Output**: Throws `ObjectDisposedException`.

### Case 4: IOException wrapped into FileSyncException
* **Preconditions**: Low-level disk fsync fails.
* **Execution**: `Sync(openFileEntry)`
* **Expected Output**: Throws `FileSyncException` (wrapping `IOException`).
* **Dependency Calls**:
  * `fileHandle.FlushToDisk()` -> throws `IOException`

### Case 5: Do Not Modify DataFile Metadata
* **Description**: Verifies that a sync operation does not modify the `DataFile` metadata, sizes, or bitmap contents.
* **Expected State**:
  * `openFileEntry.DataFile` attributes and objects remain unmodified.
