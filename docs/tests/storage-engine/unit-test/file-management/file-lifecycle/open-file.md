# Unit Test Specification — `FileLifecycleManager.OpenFile`

## 1. Document Information
- **Specification ID:** UT-FM-LC-OPEN
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileLifecycle`
- **Class:** `FileLifecycleManager`
- **Interface:** `IFileLifecycleManager`
- **Public Method:** `OpenFileEntry OpenFile(string fileName, FileAccessMode accessMode, FileLockMode lockMode)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Open File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/open-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Opens a physical database file, reconstructs in-memory structures from disk, validates format compatibility, and registers an active entry to coordinate concurrent handle usage.

## 3. Unit Under Test
- **Concrete class:** `FileLifecycleManager`
- **Public method:** `OpenFile`
- **Inputs:** `string fileName`, `FileAccessMode accessMode`, `FileLockMode lockMode`
- **Output:** `OpenFileEntry`
- **Observable state:** Registration of the open file entry with incremented reference count.
- **Dependencies:** `IOpenFileManager`, `IPhysicalFileSystem`, `IFileReader`, `IFileValidator`.

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IOpenFileManager` | File registration and concurrent lock conflict checks. | Mock / Fake |
| `IPhysicalFileSystem` | OS-level physical file checks and file handles. | Mock / Stub |
| `IFileReader` | Read structural metadata (header, allocation metadata, bitmap). | Stub |
| `IFileValidator` | Validating the parsed file structures. | Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-LC-OPEN-001 | If the file is already open with compatible locks, increment reference count and return the existing entry without re-opening. |
| BR-LC-OPEN-002 | If the file exists and is not open, open it, read structures, validate them, and register a new entry with reference count 1. |
| BR-LC-OPEN-003 | If physical operations or dependencies fail, close the physical handle immediately. |
| BR-LC-OPEN-004 | Any failure must prevent registration in `OpenFileManager` and propagate the exception. |
| BR-LC-OPEN-005 | Lock or access mode conflicts must throw `LockConflictException` and leave state unchanged. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-LC-OPEN-001 | First-time open success | Positive | Critical |
| UT-FM-LC-OPEN-002 | Reopen compatible | Alternative | High |
| UT-FM-LC-OPEN-003 | Exclusive lock conflict | Negative | High |
| UT-FM-LC-OPEN-004 | Access mode conflict | Negative | High |
| UT-FM-LC-OPEN-005 | File does not exist | Negative | High |
| UT-FM-LC-OPEN-006 | OS open handle fails | Dependency Failure | High |
| UT-FM-LC-OPEN-007 | Read header fails | Dependency Failure | High |
| UT-FM-LC-OPEN-008 | Read metadata fails | Dependency Failure | High |
| UT-FM-LC-OPEN-009 | Validation fails | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-LC-OPEN-001 — First-time open success
#### Objective
Verify that a non-registered valid file is opened, validated, and registered correctly.
#### Requirement References
- BR-LC-OPEN-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: "test.db"
- `accessMode`: `FileAccessMode.ReadWrite`
- `lockMode`: `FileLockMode.Exclusive`
#### Preconditions
- File exists on disk.
- File is not registered in `OpenFileManager`.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns null.
- `IPhysicalFileSystem.Exists` returns true.
- `IPhysicalFileSystem.Open` returns a test double `FileHandle`.
- `IFileReader` methods return valid structs.
- `IPhysicalFileSystem.GetSize` returns a valid physical size.
#### Execution
Invoke `OpenFile("test.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive)`.
#### Expected Output
Returns a valid `OpenFileEntry` wrapping the reconstructed structures and handles.
#### Expected State
- Entry is registered.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
| `IPhysicalFileSystem` | `Open` | `"test.db", FileAccessMode.ReadWrite` | 1 |
| `IFileReader` | `ReadHeader` | `fileHandle` | 1 |
| `IFileReader` | `ReadAllocationMetadata` | `fileHandle, fileHeader` | 1 |
| `IFileReader` | `ReadExtentBitmap` | `fileHandle, fileHeader, allocationMetadata` | 1 |
| `IPhysicalFileSystem` | `GetSize` | `fileHandle` | 1 |
| `IFileValidator` | `Validate` | `fileHeader, allocationMetadata, extentBitmap, physicalSize` | 1 |
| `IOpenFileManager` | `RegisterOpenFile` | `"test.db", openFileEntry` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-LC-OPEN-002 — Reopen compatible
#### Objective
Verify that opening an already-open file with compatible locks increments reference counts and bypasses physical reads.
#### Requirement References
- BR-LC-OPEN-001
#### Priority
High
#### Test Category
Alternative
#### Input
- `fileName`: "test.db"
- `accessMode`: `FileAccessMode.ReadOnly`
- `lockMode`: `FileLockMode.Shared`
#### Preconditions
- File is registered with `AccessMode = ReadOnly` and `LockMode = Shared`.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns `existingOpenFileEntry`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Returns the `existingOpenFileEntry`.
#### Expected State
Reference count on `existingOpenFileEntry` increments. (No new entry is registered).
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Open`
- `IFileReader.ReadHeader`
- `IFileValidator.Validate`
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-LC-OPEN-003 — Exclusive lock conflict
#### Objective
Verify that opening fails if requested exclusive lock conflicts with existing shared lock.
#### Requirement References
- BR-LC-OPEN-005
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: "test.db"
- `accessMode`: `FileAccessMode.ReadWrite`
- `lockMode`: `FileLockMode.Exclusive`
#### Preconditions
- File is registered with `LockMode = Shared`.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns `existingOpenFileEntry`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Throws `LockConflictException`.
#### Expected State
No observable state change (reference count is unchanged).
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Open`
#### Expected Failure Handling
Exception is propagated. Remaining operations stop.
#### Cleanup
None.

### Case UT-FM-LC-OPEN-004 — Access mode conflict
#### Objective
Verify that opening fails if requested write access conflicts with existing read-only access.
#### Requirement References
- BR-LC-OPEN-005
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: "test.db"
- `accessMode`: `FileAccessMode.ReadWrite`
- `lockMode`: `FileLockMode.Shared`
#### Preconditions
- File is registered with `AccessMode = ReadOnly`.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns `existingOpenFileEntry`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Throws `LockConflictException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Open`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-LC-OPEN-005 — File does not exist
#### Objective
Verify that opening a missing file throws an appropriate exception without attempting physical open.
#### Requirement References
- BR-LC-OPEN-004
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: "missing.db"
#### Preconditions
- File does not exist on disk.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns null.
- `IPhysicalFileSystem.Exists` returns false.
#### Execution
Invoke `OpenFile("missing.db", ...)`
#### Expected Output
Throws `FileNotFoundException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"missing.db"` | 1 |
| `IPhysicalFileSystem` | `Exists` | `"missing.db"` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Open`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-LC-OPEN-006 — OS open handle fails
#### Objective
Verify that an OS failure to open the handle propagates correctly and stops initialization.
#### Requirement References
- BR-LC-OPEN-004
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `fileName`: "test.db"
#### Preconditions
- File exists. OS denies access or throws `IOException`.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns null.
- `IPhysicalFileSystem.Exists` returns true.
- `IPhysicalFileSystem.Open` throws `IOException`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Throws `FileOpenException` wrapping the root `IOException`.
#### Expected State
No observable state change. Entry is not registered.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
| `IPhysicalFileSystem` | `Open` | `"test.db", accessMode` | 1 |
#### Prohibited Dependency Calls
- `IFileReader.ReadHeader`
- `IOpenFileManager.RegisterOpenFile`
#### Expected Failure Handling
Exception is propagated. Registration stops.
#### Cleanup
None required (handle was never acquired).

### Case UT-FM-LC-OPEN-007 — Read header fails
#### Objective
Verify that failure to read the header closes the acquired handle and prevents registration.
#### Requirement References
- BR-LC-OPEN-003
- BR-LC-OPEN-004
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `fileName`: "test.db"
#### Preconditions
- OS open succeeds, but `ReadHeader` throws `IOException`.
#### Dependency Setup
- `IPhysicalFileSystem.Open` returns `fileHandle`.
- `IFileReader.ReadHeader` throws `IOException`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Throws `FileOpenException` (or propagates the `IOException`).
#### Expected State
No entry is registered.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Open` | `"test.db", accessMode` | 1 |
| `IFileReader` | `ReadHeader` | `fileHandle` | 1 |
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.RegisterOpenFile`
#### Expected Failure Handling
Exception is propagated. State remains unregistered.
#### Cleanup
`FileLifecycleManager` explicitly calls `IPhysicalFileSystem.Close(fileHandle)` to release the acquired resource.

### Case UT-FM-LC-OPEN-008 — Read metadata fails
#### Objective
Verify that failure to read allocation metadata closes the handle and prevents registration.
#### Requirement References
- BR-LC-OPEN-003
- BR-LC-OPEN-004
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `fileName`: "test.db"
#### Preconditions
- Header read succeeds, `ReadAllocationMetadata` throws `IOException`.
#### Dependency Setup
- `IFileReader.ReadAllocationMetadata` throws `IOException`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Throws `FileOpenException`.
#### Expected State
No entry is registered.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileReader` | `ReadAllocationMetadata` | `fileHandle, fileHeader` | 1 |
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.RegisterOpenFile`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
`IPhysicalFileSystem.Close(fileHandle)` is called.

### Case UT-FM-LC-OPEN-009 — Validation fails
#### Objective
Verify that if structural validation fails, the handle is closed and registration is prevented.
#### Requirement References
- BR-LC-OPEN-003
- BR-LC-OPEN-004
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `fileName`: "test.db"
#### Preconditions
- File is read successfully into memory, but `IFileValidator.Validate` throws `InvalidFileFormatException`.
#### Dependency Setup
- `IFileValidator.Validate` throws `InvalidFileFormatException`.
#### Execution
Invoke `OpenFile(...)`.
#### Expected Output
Throws `InvalidFileFormatException`.
#### Expected State
No entry is registered.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileValidator` | `Validate` | `fileHeader, allocationMetadata, extentBitmap, physicalSize` | 1 |
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.RegisterOpenFile`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
`IPhysicalFileSystem.Close(fileHandle)` is called.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior
- [x] Invalid input (Negative testing)
- [x] Boundary conditions (N/A)
- [x] Invalid state
- [x] Dependency failures
- [x] Cleanup paths
- [x] Prohibited dependency calls

## 9. Specification Gaps
> None.

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
