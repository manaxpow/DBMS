# Unit Test Spec - FileLifecycleManager.DeleteFile

## Component
* **Class**: `FileLifecycleManager`
* **Method**: `DeleteFile(fileName: string) : void`

---

## Test Cases

### Case 1: Delete File Not Open (Happy Path)
* **Preconditions**: File exists on disk, not open.
* **Execution**: `DeleteFile("test.db")`
* **Expected Output**: Succeeds.
* **Expected State**: File is deleted on disk. Lock removed.
* **Dependency Calls**:
  * `OpenFileManager.TryBeginDelete("test.db")` -> returns `true`
  * `CheckFileExists("test.db")` -> returns `true`
  * `DeletePhysicalFile("test.db")` -> success
  * `OpenFileManager.CompleteDelete("test.db")` -> success

### Case 2: Blocked when File is Open
* **Preconditions**: File currently registered in `OpenFileManager`.
* **Execution**: `DeleteFile("test.db")`
* **Expected Output**: Throws `FileInUseException`.
* **Dependency Calls**:
  * `OpenFileManager.TryBeginDelete("test.db")` -> returns `false` (acquisition failed because the file is actively open)

### Case 3: File Does Not Exist
* **Preconditions**: File is not open, but does not exist on disk.
* **Execution**: `DeleteFile("test.db")`
* **Expected Output**: Throws `FileNotFoundException`.
* **Dependency Calls**:
  * `OpenFileManager.TryBeginDelete("test.db")` -> returns `true`
  * `CheckFileExists("test.db")` -> returns `false`
  * `OpenFileManager.CancelDelete("test.db")` -> success

### Case 4: TryBeginDelete Conflict Failure
* **Description**: Verifies that if `TryBeginDelete` returns false due to concurrent operations, deletion fails.
* **Execution**: `DeleteFile("test.db")`
* **Expected Output**: Throws `FileInUseException`.
* **Dependency Calls**:
  * `OpenFileManager.TryBeginDelete("test.db")` -> returns `false`

### Case 5: Physical Delete Failure
* **Preconditions**: File not open, exists, but OS locks it (Access Denied).
* **Execution**: `DeleteFile("test.db")`
* **Expected Output**: Throws `FileDeleteException` (wrapping `IOException`).
* **Dependency Calls**:
  * `OpenFileManager.TryBeginDelete("test.db")` -> returns `true`
  * `CheckFileExists("test.db")` -> returns `true`
  * `DeletePhysicalFile("test.db")` -> throws `IOException`
  * `OpenFileManager.CancelDelete("test.db")` -> success

### Case 6: Call CancelDelete when Delete Fails
* **Description**: Verifies that `CancelDelete` is triggered to release the state lock if either `CheckFileExists` or `DeletePhysicalFile` throws an exception.
* **Dependency Calls**: Ensure `CancelDelete` is executed in the `catch` block when filesystem calls throw exceptions.

### Case 7: Call CompleteDelete on Success
* **Description**: Verifies that `CompleteDelete` is executed to finalize registry removal only after physical deletion succeeds.
* **Dependency Calls**: Ensure `CompleteDelete` is invoked after `DeletePhysicalFile` completes.

### Case 8: Do Not Leave File Stuck in Deleting State
* **Description**: Verifies that every code path (success or failure) finishes by calling either `CompleteDelete` or `CancelDelete`, ensuring the entry never remains stuck in the `Deleting` state.
