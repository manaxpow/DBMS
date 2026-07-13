# Unit Test Spec - FileLifecycleManager.CloseFile

## Component
* **Class**: `FileLifecycleManager`
* **Method**: `CloseFile(fileName: string) : void`

---

## Test Cases

### Case 1: ReferenceCount > 1 Only Decrements Count (Happy Path)
* **Preconditions**: File open with `ReferenceCount == 3`.
* **Execution**: `CloseFile("test.db")`
* **Expected Output**: Succeeds.
* **Expected State**: ReferenceCount is decremented to `2`. Handle is NOT closed, synchronizers are NOT called.
* **Dependency Calls**:
  * `OpenFileManager.GetOpenFile("test.db")` -> returns `openFileEntry`
  * `openFileEntry.DecrementRefCount()` -> returns `2`

### Case 2: ReferenceCount == 1 Syncs and Closes (Happy Path)
* **Preconditions**: File open with `ReferenceCount == 1`.
* **Execution**: `CloseFile("test.db")`
* **Expected Output**: Succeeds.
* **Expected State**: Handle closed and entry unregistered.
* **Dependency Calls**:
  * `OpenFileManager.GetOpenFile("test.db")` -> returns `openFileEntry`
  * `openFileEntry.DecrementRefCount()` -> returns `0`
  * `FileSynchronizer.Sync(openFileEntry)` -> success
  * `ClosePhysicalFile(fileHandle)` -> success
  * `OpenFileManager.UnregisterOpenFile("test.db")` -> success

### Case 3: Unregister After Successful Close
* **Description**: Verifies that `UnregisterOpenFile` is executed only *after* synchronizing and closing handles succeeds.
* **Dependency Calls**: Ensure the execution order is: `Sync` -> `ClosePhysicalFile` -> `UnregisterOpenFile`.

### Case 4: File Not Opened Error
* **Preconditions**: File not in manager.
* **Execution**: `CloseFile("test.db")`
* **Expected Output**: Throws `FileNotOpenException`.

### Case 5: Prevent Negative Reference Count
* **Description**: Verifies that the entry protects itself from decrementing past 0, throwing a runtime anomaly exception.
* **Preconditions**: File entry has `ReferenceCount == 0`.
* **Execution**: `CloseFile("test.db")`
* **Expected Output**: Throws `InvalidOperationException`.

### Case 6: Sync Failure does not unregister
* **Preconditions**: File open with refCount = 1. Sync throws IOException.
* **Execution**: `CloseFile("test.db")`
* **Expected Output**: Throws `FileCloseException` (wrapping `IOException`).
* **Expected State**: Handle remains registered in `OpenFileManager`.
* **Dependency Calls**:
  * `openFileEntry.DecrementRefCount()` -> returns `0`
  * `FileSynchronizer.Sync(...)` -> throws `IOException`
  * `ClosePhysicalFile` is NOT called, and `UnregisterOpenFile` is NOT called.

### Case 7: Close Handle Failure Recovery
* **Preconditions**: Sync succeeds, but `ClosePhysicalFile` throws.
* **Execution**: `CloseFile("test.db")`
* **Expected Output**: Throws `FileCloseException`.
* **Expected State**: Unregistered to prevent locks, but error is escalated.
* **Dependency Calls**:
  * `FileSynchronizer.Sync(...)` -> succeeds
  * `ClosePhysicalFile(...)` -> throws `IOException`
  * `OpenFileManager.UnregisterOpenFile("test.db")` -> success

### Case 8: Do Not Unregister Too Early
* **Description**: Verifies that the manager does not unregister the file entry when `ReferenceCount > 0` after closing.
* **Dependency Calls**: Verify `UnregisterOpenFile` is **never** called if `DecrementRefCount()` returns a value greater than 0.

### Case 9: Read-Only Files Do Not Trigger Sync
* **Description**: Verifies that if a file is open in `ReadOnly` mode, `CloseFile` bypasses the `FileSynchronizer.Sync` call.
* **Preconditions**: File opened with `FileAccessMode.ReadOnly` and `ReferenceCount == 1`.
* **Dependency Calls**:
  * `FileSynchronizer.Sync` is **never** called.
  * `ClosePhysicalFile` and `UnregisterOpenFile` are executed directly.
