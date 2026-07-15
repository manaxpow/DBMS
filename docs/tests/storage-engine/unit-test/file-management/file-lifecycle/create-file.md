# Unit Test Specification — `FileLifecycleManager.CreateFile`

## 1. Document Information
- **Specification ID:** UT-FM-LC-CREATE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileLifecycle`
- **Class:** `FileLifecycleManager`
- **Interface:** `IFileLifecycleManager`
- **Public Method:** `DataFile CreateFile(string fileName, FileType fileType, int pageSize, long initialFileSize)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Sequence Diagram: Validate File](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/validate-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Creates a new physical file on disk, initializes its required structures (FileHeader, AllocationMetadata, ExtentBitmap), and returns the runtime representation (`DataFile`). It guarantees atomicity by rolling back partial creations on failure.

## 3. Unit Under Test
- **Concrete class:** `FileLifecycleManager`
- **Public method:** `CreateFile`
- **Inputs:** `string fileName, FileType fileType, int pageSize, long initialFileSize`
- **Output:** `DataFile` object representing the newly created file.
- **Observable state:** A physical file is created and fully formatted on disk.
- **Dependencies:** `IPhysicalFileSystem`, `IFileWriter`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IPhysicalFileSystem` | Handles physical file existence checks, creation, closing, and deletion. | Mock / Stub |
| `IFileWriter` | Writes the internal structural segments to the file handle. | Mock / Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-LC-CREATE-001 | If the file already exists, must throw `FileAlreadyExistsException`. |
| BR-LC-CREATE-002 | Must create the physical file and securely write all required structures in order: Header -> AllocationMetadata -> ExtentBitmap. |
| BR-LC-CREATE-003 | Must close the temporary file handle used during creation. |
| BR-LC-CREATE-004 | If any structure fails to write, must attempt to clean up by closing the handle and deleting the partially created physical file. *(Design Gap)* |
| BR-LC-CREATE-005 | Page sizes must be positive powers of two. Initial file size must be positive. Otherwise throws `ArgumentException` or `ArgumentOutOfRangeException`. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-LC-CREATE-001 | Successful file creation | Positive | Critical |
| UT-FM-LC-CREATE-002 | File already exists | Negative | High |
| UT-FM-LC-CREATE-003 | Physical file creation fails | Dependency Failure | Critical |
| UT-FM-LC-CREATE-004 | Header write fails | Dependency Failure | High |
| UT-FM-LC-CREATE-005 | Metadata write fails | Dependency Failure | High |
| UT-FM-LC-CREATE-006 | Bitmap write fails | Dependency Failure | High |
| UT-FM-LC-CREATE-007 | Rollback deletion fails | Dependency Failure | Medium |
| UT-FM-LC-CREATE-008 | Non-positive page size | Boundary | High |
| UT-FM-LC-CREATE-009 | Page size is not a power of two | Negative | High |
| UT-FM-LC-CREATE-010 | Invalid initial file size | Boundary | High |

## 7. Test Cases

### Case UT-FM-LC-CREATE-001 — Successful file creation
#### Objective
Verify that all structures are correctly initialized and written to a new physical file.
#### Requirement References
- BR-LC-CREATE-002
- BR-LC-CREATE-003
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: `"test.db"`
- `fileType`: `FileType.Data`
- `pageSize`: `4096`
- `initialFileSize`: `1048576`
#### Preconditions
- The file `"test.db"` does not exist.
#### Dependency Setup
- `IPhysicalFileSystem.Exists("test.db")` returns `false`.
- `IPhysicalFileSystem.Create` returns a valid `fileHandle`.
- `IFileWriter.WriteHeader`, `WriteAllocationMetadata`, `WriteExtentBitmap` all succeed.
- `IPhysicalFileSystem.Close` succeeds.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Returns a valid `DataFile` representing the created file.
#### Expected State
Physical file is formatted and temporary handle is closed.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
| `IPhysicalFileSystem` | `Create` | `"test.db", 1048576` | 1 |
| `IFileWriter` | `WriteHeader` | `fileHandle, FileHeader` | 1 |
| `IFileWriter` | `WriteAllocationMetadata` | `fileHandle, FileHeader, AllocationMetadata` | 1 |
| `IFileWriter` | `WriteExtentBitmap` | `fileHandle, FileHeader, AllocationMetadata, ExtentBitmap` | 1 |
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Delete`
#### Expected Failure Handling
N/A

### Case UT-FM-LC-CREATE-002 — File already exists
#### Objective
Verify that creating an existing file is safely rejected before touching physical storage.
#### Requirement References
- BR-LC-CREATE-001
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- `"test.db"` already exists.
#### Dependency Setup
- `IPhysicalFileSystem.Exists("test.db")` returns `true`.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Throws `FileAlreadyExistsException`.
#### Expected State
No structural changes.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Create`
- `IFileWriter` (Any method)
- `IPhysicalFileSystem.Close`, `Delete`

### Case UT-FM-LC-CREATE-003 — Physical file creation fails
#### Objective
Verify that OS-level creation errors are wrapped and safely handled.
#### Requirement References
- BR-LC-CREATE-002
#### Priority
Critical
#### Test Category
Dependency Failure
#### Input
- `fileName`: `"test.db"`
#### Dependency Setup
- `IPhysicalFileSystem.Exists("test.db")` returns `false`.
- `IPhysicalFileSystem.Create` throws `IOException`.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Throws `FileCreationException` (wrapping `IOException`).
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Create` | `"test.db", 1048576` | 1 |
#### Prohibited Dependency Calls
- `IFileWriter` (Any method)
- `IPhysicalFileSystem.Delete`

### Case UT-FM-LC-CREATE-004 — Header write fails
#### Objective
Verify that failure to format the header safely aborts and triggers rollback.
#### Requirement References
- BR-LC-CREATE-004
#### Priority
High
#### Test Category
Dependency Failure
#### Dependency Setup
- `IPhysicalFileSystem.Create` returns `fileHandle`.
- `IFileWriter.WriteHeader` throws `IOException`.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Throws `FileCreationException`.
#### Expected State
*Pending Design Resolution.* (Previously specified to close handle and delete file).
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileWriter` | `WriteHeader` | `fileHandle, FileHeader` | 1 |

### Case UT-FM-LC-CREATE-005 — Metadata write fails
#### Objective
Verify that failure to format the metadata safely aborts and triggers rollback.
#### Requirement References
- BR-LC-CREATE-004
#### Priority
High
#### Test Category
Dependency Failure
#### Dependency Setup
- `IFileWriter.WriteHeader` succeeds.
- `IFileWriter.WriteAllocationMetadata` throws `IOException`.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Throws `FileCreationException`.
#### Expected State
*Pending Design Resolution.*

### Case UT-FM-LC-CREATE-006 — Bitmap write fails
#### Objective
Verify that failure to format the bitmap safely aborts and triggers rollback.
#### Requirement References
- BR-LC-CREATE-004
#### Priority
High
#### Test Category
Dependency Failure
#### Dependency Setup
- `IFileWriter.WriteAllocationMetadata` succeeds.
- `IFileWriter.WriteExtentBitmap` throws `IOException`.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Throws `FileCreationException`.
#### Expected State
*Pending Design Resolution.*

### Case UT-FM-LC-CREATE-007 — Rollback deletion fails
#### Objective
Verify that an exception thrown during rollback does not replace the original file creation exception.
#### Requirement References
- BR-LC-CREATE-004
#### Priority
Medium
#### Test Category
Dependency Failure
#### Dependency Setup
- `IFileWriter.WriteAllocationMetadata` throws `metadataWriteException`.
- `IPhysicalFileSystem.Delete` throws `rollbackDeleteException`.
#### Execution
Invoke `CreateFile("test.db", FileType.Data, 4096, 1048576)`.
#### Expected Output
Throws `FileCreationException`.
#### Expected State
`FileCreationException.InnerException` is exactly `metadataWriteException`. `rollbackDeleteException` is not exposed as the primary error.

### Case UT-FM-LC-CREATE-008 — Non-positive page size
#### Objective
Verify that invalid boundaries for page size throw `ArgumentOutOfRangeException`.
#### Requirement References
- BR-LC-CREATE-005
#### Priority
High
#### Test Category
Boundary
#### Input
- `pageSize`: `0` or `-512`
#### Execution
Invoke `CreateFile(..., pageSize: 0, ...)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.

### Case UT-FM-LC-CREATE-009 — Page size is not a power of two
#### Objective
Verify that non-power-of-two page sizes throw `ArgumentException`.
#### Requirement References
- BR-LC-CREATE-005
#### Priority
High
#### Test Category
Negative
#### Input
- `pageSize`: `4097`
#### Execution
Invoke `CreateFile(..., pageSize: 4097, ...)`.
#### Expected Output
Throws `ArgumentException`.

### Case UT-FM-LC-CREATE-010 — Invalid initial file size
#### Objective
Verify that non-positive initial sizes throw `ArgumentOutOfRangeException`.
#### Requirement References
- BR-LC-CREATE-005
#### Priority
High
#### Test Category
Boundary
#### Input
- `initialFileSize`: `-1`
#### Execution
Invoke `CreateFile(..., initialFileSize: -1)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
- [x] Invalid input (Negative testing)
- [x] Boundary conditions
- [x] Invalid state
- [x] Dependency failures
- [ ] Cleanup paths (Blocked by Design Gap)
- [x] Prohibited dependency calls

## 9. Specification Gaps
> [!WARNING]
> **Specification Gap — Design clarification required.**
> Final state after failure (partial creation) is under-specified. While the previous specification mandated closing the file handle and deleting the physical file upon structural formatting failure, the exact sequence and responsibilities (e.g., whether a file that fails initialization should be securely wiped or simply deleted) lack formal architecture sign-off in the class/sequence diagrams. The cleanup verifications for Cases 4, 5, 6, and 7 are marked as pending.
> Boundary condition constraints for maximum file limits are marked as Not Applicable since the design does not stipulate arbitrary max boundaries during creation.

## 10. Review Checklist
- [x] Tests observable behavior.
- [x] Does not test private methods.
- [x] Does not use real infrastructure.
- [x] Expected output is measurable.
- [x] Expected state is explicit.
- [x] Failure paths define cleanup.
- [x] Dependency calls are contract-relevant.
- [x] No undocumented behavior was invented.
- [x] All document links are relative.
