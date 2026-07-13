# Unit Test Spec - FileLifecycleManager.OpenFile

## Component
* **Class**: `FileLifecycleManager`
* **Method**: `OpenFile(fileName: string, accessMode: FileAccessMode, lockMode: FileLockMode) : OpenFileEntry`

---

## Test Cases

### Case 1: First-Time Open Success (Happy Path)
* **Input**: `fileName = "test.db"`, `accessMode = FileAccessMode.ReadWrite`, `lockMode = FileLockMode.Exclusive`
* **Preconditions**: File is on disk, not registered in `OpenFileManager`.
* **Expected Output**: Returns valid `OpenFileEntry` with refCount = 1.
* **Dependency Calls**:
  * `OpenFileManager.GetOpenFile("test.db")` -> returns `null`
  * `CheckFileExists("test.db")` -> returns `true`
  * `OpenPhysicalFile("test.db", ReadWrite)` -> returns `fileHandle`
  * `FileReader.ReadHeader(fileHandle)` -> returns `fileHeader`
  * `FileReader.ReadAllocationMetadata(fileHandle, fileHeader)` -> returns `allocationMetadata`
  * `FileReader.ReadExtentBitmap(fileHandle, fileHeader, allocationMetadata)` -> returns `extentBitmap`
  * `FileValidator.Validate(...)` -> succeeds
  * `OpenFileManager.RegisterOpenFile("test.db", entry)` -> succeeds

### Case 2: Already Open Compatible RefCount Increment
* **Input**: `fileName = "test.db"`, `accessMode = FileAccessMode.ReadOnly`, `lockMode = FileLockMode.Shared`
* **Preconditions**: File is already open with compatible Shared/ReadOnly mode.
* **Expected Output**: Returns the existing `OpenFileEntry` with incremented `ReferenceCount == 2`.
* **Dependency Calls**:
  * `OpenFileManager.GetOpenFile("test.db")` -> returns `existingEntry`
  * `existingEntry.IncrementRefCount()` -> returns `2`

### Case 3: Shared Lock Compatibility
* **Description**: Verifies multiple readers can successfully share the file simultaneously.
* **Preconditions**: File already open with `LockMode = Shared`.
* **Execution**: `OpenFile("test.db", ReadOnly, Shared)`
* **Expected Output**: Succeeds and returns existing entry.

### Case 4: Exclusive Lock Conflict
* **Input**: `lockMode = FileLockMode.Exclusive`
* **Preconditions**: File is already open with `LockMode = Shared`.
* **Expected Output**: Throws `LockConflictException`.

### Case 5: Access Mode Conflict
* **Input**: `accessMode = FileAccessMode.ReadWrite`
* **Preconditions**: File already open as `AccessMode = ReadOnly`.
* **Expected Output**: Throws `LockConflictException` (Access Mode Mismatch).

### Case 6: File Does Not Exist
* **Input**: `fileName = "missing.db"`
* **Preconditions**: File not on disk.
* **Expected Output**: Throws `FileNotFoundException`.
* **Dependency Calls**:
  * `OpenFileManager.GetOpenFile("missing.db")` -> returns `null`
  * `CheckFileExists("missing.db")` -> returns `false`

### Case 7: OS Open Physical Handle Failure
* **Input**: `fileName = "test.db"`
* **Preconditions**: File exists, but OS locked it (Access Denied).
* **Expected Output**: Throws `FileOpenException` (wrapping `IOException`).
* **Dependency Calls**:
  * `OpenPhysicalFile("test.db", ...)` -> throws `IOException`

### Case 8: Read Header Failure Recovery
* **Preconditions**: OS open succeeds, but reading headers fails.
* **Expected Output**: Throws `FileOpenException`.
* **Expected State**: Handles are closed and cleanup runs.
* **Dependency Calls**:
  * `OpenPhysicalFile(...)` -> returns `fileHandle`
  * `FileReader.ReadHeader(fileHandle)` -> throws `IOException`
  * `ClosePhysicalFile(fileHandle)` -> success

### Case 9: Read Allocation Metadata Failure Recovery
* **Preconditions**: Headers succeed, metadata read fails.
* **Expected Output**: Throws `FileOpenException`.
* **Expected State**: Handles closed.
* **Dependency Calls**:
  * `FileReader.ReadHeader(...)` -> succeeds
  * `FileReader.ReadAllocationMetadata(...)` -> throws `IOException`
  * `ClosePhysicalFile(fileHandle)` -> success

### Case 10: Validation Failure
* **Preconditions**: Reads succeed, but `FileValidator` fails magic signatures.
* **Expected Output**: Throws `InvalidFileFormatException`.
* **Dependency Calls**:
  * `FileValidator.Validate(...)` -> throws `InvalidFileFormatException`
  * `ClosePhysicalFile(fileHandle)` -> success

### Case 11: Close Handle when Open Not Completed
* **Description**: Verifies that any exception occurring during open will safely close the active handle.
* **Dependency Calls**: Verify `ClosePhysicalFile(fileHandle)` is called in the `catch` block of open flows.

### Case 12: Do Not Register Entry when Validation Fails
* **Description**: Verifies that the entry is never registered in the open file manager if validation fails.
* **Dependency Calls**: Ensure `RegisterOpenFile` is **never** called if validation throws an exception.
