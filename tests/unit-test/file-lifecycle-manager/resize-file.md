# Unit Test Spec - FileLifecycleManager.ResizeFile

## Component
* **Class**: `FileLifecycleManager`
* **Method**: `ResizeFile(entry: OpenFileEntry, newSize: long) : void`

---

## Test Cases

### Case 1: Extend File Successfully (Happy Path)
* **Input**: `entry = openFileEntry`, `newSize = 2097152 (2MB)`
* **Preconditions**: File physical length is 1MB.
* **Execution**: `ResizeFile(openFileEntry, 2097152)`
* **Expected Output**: Succeeds.
* **Expected State**: Length is set to 2MB, extents added to metadata.
* **Dependency Calls**:
  * `FileHandle.GetLength()` -> returns `1048576`
  * `FileHandle.SetLength(2097152)` -> success
  * `AllocationMetadata.AddExtents(newExtentCount)` -> success
  * `FileWriter.WriteAllocationMetadata(fileHandle, header, metadata)` -> success
  * `FileSynchronizer.Sync(openFileEntry)` -> success

### Case 2: Truncate File Safely (Happy Path)
* **Input**: `entry = openFileEntry`, `newSize = 1048576 (1MB)`
* **Preconditions**: File size is 2MB. Shrink area contains only free extents.
* **Execution**: `ResizeFile(openFileEntry, 1048576)`
* **Expected Output**: Succeeds.
* **Expected State**: Length set to 1MB, metadata updated.
* **Dependency Calls**:
  * `FileHandle.GetLength()` -> returns `2097152`
  * `AllocationMetadata.CanTruncateTo(newExtentCount)` -> returns `true`
  * `AllocationMetadata.TruncateTo(newExtentCount)` -> success
  * `FileWriter.WriteAllocationMetadata(fileHandle, header, metadata)` -> success
  * `FileHandle.SetLength(1048576)` -> success
  * `FileSynchronizer.Sync(openFileEntry)` -> success

### Case 3: Resize No-Op (New Size == Current Size)
* **Input**: `entry = openFileEntry`, `newSize = 1048576`
* **Preconditions**: Current file length is exactly 1MB.
* **Execution**: `ResizeFile(openFileEntry, 1048576)`
* **Expected Output**: Succeeds.
* **Dependency Calls**:
  * `FileHandle.GetLength()` -> returns `1048576`
  * No `SetLength`, `AddExtents`, or metadata writes are executed.

### Case 4: Invalid Size Arguments Rejection
* **Input**: `newSize = -500` or `newSize` not aligned with page/extent block sizes.
* **Execution**: `ResizeFile(openFileEntry, -500)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.

### Case 5: Truncate cuts into used extent
* **Input**: `entry = openFileEntry`, `newSize = 1048576`
* **Preconditions**: File is 2MB, but an allocated extent sits in the upper 1MB range.
* **Execution**: `ResizeFile(openFileEntry, 1048576)`
* **Expected Output**: Throws `FileTruncationException`.
* **Dependency Calls**:
  * `FileHandle.GetLength()` -> returns `2097152`
  * `AllocationMetadata.CanTruncateTo(newExtentCount)` -> returns `false`

### Case 6: Maximum File Size Exceeded
* **Input**: `entry = openFileEntry`, `newSize = 99999999999`
* **Preconditions**: File `MaximumSize` is configured to 10MB.
* **Execution**: `ResizeFile(openFileEntry, 99999999999)`
* **Expected Output**: Throws `MaximumFileSizeExceededException`.

### Case 7: Physical OS SetLength Failure Recovery
* **Preconditions**: File extension fails due to locked handles or full disk.
* **Execution**: `ResizeFile(openFileEntry, 2097152)`
* **Expected Output**: Throws `FileResizeException` (wrapping `IOException`).
* **Dependency Calls**:
  * `FileHandle.GetLength()` -> returns `1048576`
  * `FileHandle.SetLength(2097152)` -> throws `IOException`

### Case 8: Write Metadata Failure
* **Description**: Verifies that if writing metadata fails after changing file length, the transaction rolls back or throws.
* **Dependency Calls**:
  * `FileHandle.SetLength(...)` -> success
  * `FileWriter.WriteAllocationMetadata(...)` -> throws `IOException`
  * Throws `FileResizeException`.

### Case 9: Sync Failure
* **Description**: Verifies that if `FileSynchronizer.Sync` fails to commit the resize changes, it propagates a `FileSyncException`.
* **Dependency Calls**:
  * `FileWriter.WriteAllocationMetadata(...)` -> success
  * `FileSynchronizer.Sync(...)` -> throws `FileSyncException`

### Case 10: Calculate Correct Extent Count to Add
* **Description**: Verifies that when size increases from 1MB to 2.5MB, the manager correctly computes the delta in extents (e.g. `(2.5MB - 1MB) / ExtentSize`) and calls `AddExtents` with the exact count.
* **Dependency Calls**:
  * Verifies `AllocationMetadata.AddExtents(expectedCount)` is called with the exact math.

### Case 11: Update CurrentSize Correctly
* **Description**: Verifies that upon completion of the resize flow, `DataFile.CurrentSize` property is set to the new physical size.
* **Expected State**:
  * `openFileEntry.DataFile.CurrentSize` matches `newSize`.
