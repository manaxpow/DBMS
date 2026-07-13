# Unit Test Spec - FileLifecycleManager.CreateFile

## Component
* **Class**: `FileLifecycleManager`
* **Method**: `CreateFile(fileName: string, fileType: FileType, pageSize: int, initialFileSize: long) : DataFile`

---

## Test Cases

### Case 1: Successful File Creation (Happy Path)
* **Input**: `fileName = "test.db"`, `fileType = FileType.Data`, `pageSize = 4096`, `initialFileSize = 1048576 (1MB)`
* **Preconditions**: File "test.db" does not exist.
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Returns a valid `DataFile` object representing the new file.
* **Expected State**: Handles are initialized and metadata fields are setup.
* **Dependency Calls**:
  * `CheckFileExists("test.db")` -> returns `false`
  * `CreatePhysicalFile("test.db", 1048576)` -> returns `fileHandle`
  * `FileWriter.WriteHeader(fileHandle, FileHeader)` -> success
  * `FileWriter.WriteAllocationMetadata(fileHandle, FileHeader, AllocationMetadata)` -> success
  * `FileWriter.WriteExtentBitmap(fileHandle, FileHeader, AllocationMetadata, ExtentBitmap)` -> success
  * `ClosePhysicalFile(fileHandle)` -> success

### Case 2: File Already Exists Rejection
* **Input**: `fileName = "test.db"`, `fileType = FileType.Data`, `pageSize = 4096`, `initialFileSize = 1048576`
* **Preconditions**: A file "test.db" already exists.
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Throws `FileAlreadyExistsException`.
* **Dependency Calls**:
  * `CheckFileExists("test.db")` -> returns `true`

### Case 3: Physical Create Failure
* **Input**: `fileName = "test.db"`, `fileType = FileType.Data`, `pageSize = 4096`, `initialFileSize = 1048576`
* **Preconditions**: File does not exist, but OS disk errors prevent creation.
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Throws `FileCreationException` (wrapping `IOException`).
* **Dependency Calls**:
  * `CheckFileExists("test.db")` -> returns `false`
  * `CreatePhysicalFile("test.db", 1048576)` -> throws `IOException`

### Case 4: Write Header Failure Rollback
* **Input**: `fileName = "test.db"`, `fileType = FileType.Data`, `pageSize = 4096`, `initialFileSize = 1048576`
* **Preconditions**: File does not exist.
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Throws `FileCreationException` (wrapping `IOException`).
* **Expected State**: File handle is closed and physical file is deleted.
* **Dependency Calls**:
  * `CheckFileExists("test.db")` -> returns `false`
  * `CreatePhysicalFile("test.db", 1048576)` -> returns `fileHandle`
  * `FileWriter.WriteHeader(fileHandle, FileHeader)` -> throws `IOException`
  * `ClosePhysicalFile(fileHandle)` -> success
  * `DeletePhysicalFile("test.db")` -> success

### Case 5: Write Metadata Failure Rollback
* **Input**: `fileName = "test.db"`, `fileType = FileType.Data`, `pageSize = 4096`, `initialFileSize = 1048576`
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Throws `FileCreationException`.
* **Expected State**: Handle closed and file deleted.
* **Dependency Calls**:
  * `CheckFileExists("test.db")` -> returns `false`
  * `CreatePhysicalFile("test.db", 1048576)` -> returns `fileHandle`
  * `FileWriter.WriteHeader(fileHandle, FileHeader)` -> success
  * `FileWriter.WriteAllocationMetadata(fileHandle, FileHeader, AllocationMetadata)` -> throws `IOException`
  * `ClosePhysicalFile(fileHandle)` -> success
  * `DeletePhysicalFile("test.db")` -> success

### Case 6: Write Bitmap Failure Rollback
* **Input**: `fileName = "test.db"`, `fileType = FileType.Data`, `pageSize = 4096`, `initialFileSize = 1048576`
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Throws `FileCreationException`.
* **Expected State**: Handle closed and file deleted.
* **Dependency Calls**:
  * `CheckFileExists("test.db")` -> returns `false`
  * `CreatePhysicalFile("test.db", 1048576)` -> returns `fileHandle`
  * `FileWriter.WriteHeader(fileHandle, FileHeader)` -> success
  * `FileWriter.WriteAllocationMetadata(fileHandle, FileHeader, AllocationMetadata)` -> success
  * `FileWriter.WriteExtentBitmap(fileHandle, FileHeader, AllocationMetadata, ExtentBitmap)` -> throws `IOException`
  * `ClosePhysicalFile(fileHandle)` -> success
  * `DeletePhysicalFile("test.db")` -> success

### Case 7: Close Handle After Success
* **Description**: Verifies that the initial write handle is safely closed after creation succeeds to avoid locks.
* **Input**: `fileName = "test.db"`
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Dependency Calls**:
  * All creation steps succeed, ending in `ClosePhysicalFile(fileHandle)`.

### Case 8: Rollback Close + Delete when Initialization Fails
* **Description**: Verifies that both close and delete are triggered sequentially when writing metadata fails.
* **Dependency Calls**:
  * `FileWriter.WriteAllocationMetadata(...)` -> throws `IOException`
  * Verifies `ClosePhysicalFile(fileHandle)` is called *before* `DeletePhysicalFile("test.db")`.

### Case 9: Rollback Delete also Fails
* **Description**: Verifies that if `DeletePhysicalFile` throws an exception during rollback, the error does not mask the original creation failure.
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, 1048576)`
* **Expected Output**: Throws `FileCreationException` (wrapping the original metadata write error).
* **Dependency Calls**:
  * `FileWriter.WriteHeader(...)` -> throws `IOException`
  * `ClosePhysicalFile(...)` -> success
  * `DeletePhysicalFile(...)` -> throws `IOException` (does not suppress original error)

### Case 10: Invalid Page Size Rejection
* **Input**: `pageSize = -512` or `pageSize = 4097` (non-power-of-2)
* **Execution**: `CreateFile("test.db", FileType.Data, -512, 1048576)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.
* **Dependency Calls**: No physical files or writers are called.

### Case 11: Invalid Initial File Size Rejection
* **Input**: `initialFileSize = -1`
* **Execution**: `CreateFile("test.db", FileType.Data, 4096, -1)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.
* **Dependency Calls**: No physical files are called.
