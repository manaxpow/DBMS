# Unit Test Specification — `FileLifecycleManager.CloseFile`

## 1. Document Information
- **Specification ID:** UT-FM-LC-CLOSE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileLifecycle`
- **Class:** `FileLifecycleManager`
- **Interface:** `IFileLifecycleManager`
- **Public Method:** `void CloseFile(string fileName)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Safely releases an open file's resources by decrementing its reference count. When the reference count reaches 0, it synchronizes dirty data to disk, closes the OS file handle, and unregisters the file from the runtime registry.

## 3. Unit Under Test
- **Concrete class:** `FileLifecycleManager`
- **Public method:** `CloseFile`
- **Inputs:** `string fileName`
- **Output:** `void` (Success)
- **Observable state:** Reference counts are decremented. On final close, file handles are closed and registry cleared.
- **Dependencies:** `IOpenFileManager`, `IFileSynchronizer`, `IPhysicalFileSystem`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IOpenFileManager` | Locates the runtime entry and unregisters it upon final close. | Mock / Stub |
| `IFileSynchronizer` | Flushes structural/data changes to disk before handle closure. | Mock / Stub |
| `IPhysicalFileSystem` | Invoked to physically close the underlying OS file handle. | Mock / Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-LC-CLOSE-001 | If the file is not currently registered in the manager, must throw `FileNotOpenException`. |
| BR-LC-CLOSE-002 | Must decrement the `ReferenceCount`. If the new count `> 0`, no further operations are performed. |
| BR-LC-CLOSE-003 | If the new count reaches `0`, must synchronously flush changes to disk unless the file was opened as `ReadOnly`. |
| BR-LC-CLOSE-004 | Once flushed, must physically close the OS file handle and unregister the file entry. |
| BR-LC-CLOSE-005 | Must prevent a negative reference count by throwing `InvalidOperationException` if count was already 0. |
| BR-LC-CLOSE-006 | If synchronization or OS handle closure fails, the system must define the expected final state of the registry and handle. *(Design Gap)* |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-LC-CLOSE-001 | ReferenceCount > 1 only decrements count | Positive | Critical |
| UT-FM-LC-CLOSE-002 | ReferenceCount == 1 syncs and closes | Positive | Critical |
| UT-FM-LC-CLOSE-003 | File not opened error | Negative | High |
| UT-FM-LC-CLOSE-004 | Prevent negative reference count | Invalid State | High |
| UT-FM-LC-CLOSE-005 | Sync failure recovery | Dependency Failure | Critical |
| UT-FM-LC-CLOSE-006 | Close handle failure recovery | Dependency Failure | Critical |
| UT-FM-LC-CLOSE-007 | Read-only file close | Alternative | High |

## 7. Test Cases

### Case UT-FM-LC-CLOSE-001 — ReferenceCount > 1 only decrements count
#### Objective
Verify that closing a file with multiple active references only decrements the counter.
#### Requirement References
- BR-LC-CLOSE-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File is registered in `OpenFileManager` with `ReferenceCount == 3`.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns `openFileEntry`.
- `openFileEntry.DecrementRefCount` returns `2`.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Void return.
#### Expected State
- `ReferenceCount` is decremented to `2`.
- Operating-system handle remains open.
- Entry remains registered.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IFileSynchronizer.Sync`
- `IPhysicalFileSystem.Close`
- `IOpenFileManager.UnregisterOpenFile`
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-LC-CLOSE-002 — ReferenceCount == 1 syncs and closes
#### Objective
Verify that closing the final active reference triggers a full sync and resource release.
#### Requirement References
- BR-LC-CLOSE-003
- BR-LC-CLOSE-004
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File is registered in `OpenFileManager` with `ReferenceCount == 1`.
#### Dependency Setup
- `openFileEntry.DecrementRefCount` returns `0`.
- All sync, close, and unregister operations succeed.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Void return.
#### Expected State
- OS handle is closed.
- Registry slot is cleared.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
| `IFileSynchronizer` | `Sync` | `openFileEntry` | 1 |
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
| `IOpenFileManager` | `UnregisterOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-LC-CLOSE-003 — File not opened error
#### Objective
Verify that attempting to close a file that is not registered throws an exception.
#### Requirement References
- BR-LC-CLOSE-001
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File is not registered.
#### Dependency Setup
- `IOpenFileManager.GetOpenFile` returns `null`.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Throws `FileNotOpenException`.
#### Expected State
No state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IFileSynchronizer.Sync`
- `IPhysicalFileSystem.Close`
- `IOpenFileManager.UnregisterOpenFile`

### Case UT-FM-LC-CLOSE-004 — Prevent negative reference count
#### Objective
Verify that an entry already at 0 references cannot be decremented further.
#### Requirement References
- BR-LC-CLOSE-005
#### Priority
High
#### Test Category
Invalid State
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File entry is already at `ReferenceCount == 0`.
#### Dependency Setup
- `openFileEntry.DecrementRefCount` throws `InvalidOperationException`.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Throws `InvalidOperationException`.
#### Expected State
No state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IOpenFileManager` | `GetOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IFileSynchronizer.Sync`
- `IPhysicalFileSystem.Close`
- `IOpenFileManager.UnregisterOpenFile`

### Case UT-FM-LC-CLOSE-005 — Sync failure recovery
#### Objective
Verify that an exception thrown during file synchronization is safely propagated.
#### Requirement References
- BR-LC-CLOSE-006
#### Priority
Critical
#### Test Category
Dependency Failure
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File is registered with `ReferenceCount == 1`.
#### Dependency Setup
- `openFileEntry.DecrementRefCount` returns `0`.
- `IFileSynchronizer.Sync` throws `IOException`.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Throws `FileCloseException` (wrapping `IOException`).
#### Expected State
*Pending Design Resolution.* (Previously specified to leave handle open and registered).
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileSynchronizer` | `Sync` | `openFileEntry` | 1 |
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Close`
- `IOpenFileManager.UnregisterOpenFile`

### Case UT-FM-LC-CLOSE-006 — Close handle failure recovery
#### Objective
Verify that an exception thrown during physical closure is safely propagated.
#### Requirement References
- BR-LC-CLOSE-006
#### Priority
Critical
#### Test Category
Dependency Failure
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File is registered with `ReferenceCount == 1`.
#### Dependency Setup
- `IFileSynchronizer.Sync` succeeds.
- `IPhysicalFileSystem.Close` throws `IOException`.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Throws `FileCloseException` (wrapping `IOException`).
#### Expected State
*Pending Design Resolution.*
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileSynchronizer` | `Sync` | `openFileEntry` | 1 |
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
#### Prohibited Dependency Calls
- `IOpenFileManager.UnregisterOpenFile`

### Case UT-FM-LC-CLOSE-007 — Read-only file close
#### Objective
Verify that read-only access skips the unnecessary synchronization phase.
#### Requirement References
- BR-LC-CLOSE-003
#### Priority
High
#### Test Category
Alternative
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- File open in `ReadOnly` mode with `ReferenceCount == 1`.
#### Dependency Setup
- All close and unregister operations succeed.
#### Execution
Invoke `CloseFile("test.db")`.
#### Expected Output
Void return.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Close` | `fileHandle` | 1 |
| `IOpenFileManager` | `UnregisterOpenFile` | `"test.db"` | 1 |
#### Prohibited Dependency Calls
- `IFileSynchronizer.Sync`

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior
- [x] Invalid input (Negative testing)
- [x] Boundary conditions
- [x] Invalid state
- [x] Dependency failures
- [ ] Cleanup paths (Blocked by Design Gap)
- [x] Prohibited dependency calls

## 9. Specification Gaps
> [!WARNING]
> **Specification Gap — Design clarification required.**
> Final state after a sync or physical close failure is under-specified. While the previous specification required the registry and OS handles to remain active if an error was thrown, it is unclear if a file that failed to sync safely can be trusted for further I/O, or if it must transition to a corrupted/quarantined status. The cleanup verification for Cases 5 and 6 is pending.

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
