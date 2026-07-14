# Unit Test Specification — `FileLifecycleManager.DeleteFile`

## 1. Document Information
- **Specification ID:** UT-FM-LC-DELETE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileLifecycle`
- **Class:** `FileLifecycleManager`
- **Interface:** `IFileLifecycleManager`
- **Public Method:** `void DeleteFile(string fileName)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Delete File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/delete-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Safely deletes a physical database file, ensuring that it is not currently open or locked by the system, and coordinating with the open file registry to prevent concurrent opening during deletion.

## 3. Unit Under Test
- **Concrete class:** `FileLifecycleManager`
- **Public method:** `DeleteFile`
- **Inputs:** `string fileName`
- **Output:** `void` (Success)
- **Observable state:** None locally (registry state is managed by dependencies).
- **Dependencies:** `IOpenFileManager`, `IPhysicalFileSystem`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IOpenFileManager` | Atomic deletion coordination and lock management. | Mock / Fake |
| `IPhysicalFileSystem` | OS-level file existence checks and physical deletion. | Mock / Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-LC-DELETE-001 | Before attempting deletion, existence must be verified. If missing, throws `FileNotFoundException` without interacting with the registry. |
| BR-LC-DELETE-002 | Must request a deletion lock via `TryBeginDelete`. If denied (file in use), throws `FileInUseException` and aborts. |
| BR-LC-DELETE-003 | If the deletion lock is granted, must execute physical deletion. If successful, completes the deletion via `CompleteDelete`. |
| BR-LC-DELETE-004 | If physical deletion throws an exception, must release the deletion lock via `CancelDelete` and propagate the exception. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-LC-DELETE-001 | Successful deletion | Positive | Critical |
| UT-FM-LC-DELETE-002 | Blocked when file is open | Negative | High |
| UT-FM-LC-DELETE-003 | File does not exist | Negative | High |
| UT-FM-LC-DELETE-004 | Physical delete fails | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-LC-DELETE-001 — Successful deletion
#### Objective
Verify that an inactive, existing file is safely locked, deleted physically, and finalized in the registry.
#### Requirement References
- BR-LC-DELETE-003
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: "test.db"
#### Preconditions
- File exists on disk.
- File is not open or in use.
- Physical file deletion will succeed.
#### Dependency Setup
- `IPhysicalFileSystem.Exists` returns true.
- `IOpenFileManager.TryBeginDelete` returns true.
- `IPhysicalFileSystem.Delete` succeeds.
#### Execution
Invoke `DeleteFile("test.db")`.
#### Expected Output
Void return (Success).
#### Expected State
No observable state change in `FileLifecycleManager`.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
| `IOpenFileManager` | `TryBeginDelete` | `"test.db"` | 1 |
| `IPhysicalFileSystem` | `Delete` | `"test.db"` | 1 |
| `IOpenFileManager` | `CompleteDelete` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.CancelDelete`
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-LC-DELETE-002 — Blocked when file is open
#### Objective
Verify that deletion is rejected immediately if the file is currently registered as open.
#### Requirement References
- BR-LC-DELETE-002
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: "test.db"
#### Preconditions
- File is registered with an active handle in `OpenFileManager`.
#### Dependency Setup
- `IPhysicalFileSystem.Exists` returns true.
- `IOpenFileManager.TryBeginDelete` returns false.
#### Execution
Invoke `DeleteFile("test.db")`.
#### Expected Output
Throws `FileInUseException`.
#### Expected State
No state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
| `IOpenFileManager` | `TryBeginDelete` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Delete`
- `IOpenFileManager.CompleteDelete`
- `IOpenFileManager.CancelDelete`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None required (reservation was not acquired).

### Case UT-FM-LC-DELETE-003 — File does not exist
#### Objective
Verify that deletion fails cleanly with `FileNotFoundException` if the physical file does not exist, without altering registry state.
#### Requirement References
- BR-LC-DELETE-001
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: "missing.db"
#### Preconditions
- File does not exist on disk.
#### Dependency Setup
- `IPhysicalFileSystem.Exists` returns false.
#### Execution
Invoke `DeleteFile("missing.db")`.
#### Expected Output
Throws `FileNotFoundException`.
#### Expected State
No state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"missing.db"` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.TryBeginDelete`
- `IPhysicalFileSystem.Delete`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-LC-DELETE-004 — Physical delete fails
#### Objective
Verify that if physical deletion fails due to an OS error, the acquired deletion lock is canceled and the error propagates.
#### Requirement References
- BR-LC-DELETE-004
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `fileName`: "test.db"
#### Preconditions
- File exists and is inactive, but OS locks it or denies access during deletion.
#### Dependency Setup
- `IPhysicalFileSystem.Exists` returns true.
- `IOpenFileManager.TryBeginDelete` returns true.
- `IPhysicalFileSystem.Delete` throws `IOException`.
#### Execution
Invoke `DeleteFile("test.db")`.
#### Expected Output
Throws the originating `IOException` (or a wrapped exception).
#### Expected State
No file deletion occurs. Registry state rolls back.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Exists` | `"test.db"` | 1 |
| `IOpenFileManager` | `TryBeginDelete` | `"test.db"` | 1 |
| `IPhysicalFileSystem` | `Delete` | `"test.db"` | 1 |
| `IOpenFileManager` | `CancelDelete` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.CompleteDelete`
#### Expected Failure Handling
Exception is propagated after cleanup.
#### Cleanup
`IOpenFileManager.CancelDelete` is invoked to release the temporary reservation block.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
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
