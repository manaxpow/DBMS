# Unit Test Specification — `FileSynchronizer.Sync`

## 1. Document Information
- **Specification ID:** UT-FM-IO-SYNC
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileIO`
- **Class:** `FileSynchronizer`
- **Interface:** `IFileSynchronizer`
- **Public Method:** `void Sync(OpenFileEntry entry)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Ensures that all in-memory changes applied to the physical file are durably flushed to disk via the operating system. It guarantees that after a successful return, modifications are fully persistent.

## 3. Unit Under Test
- **Concrete class:** `FileSynchronizer`
- **Public method:** `Sync`
- **Inputs:** `OpenFileEntry entry`
- **Output:** `void` (Success)
- **Observable state:** Physical OS file buffers are flushed.
- **Dependencies:** `IFileHandle` (borrowed from `OpenFileEntry`)

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IFileHandle` | Represents the low-level OS file descriptor, providing the `FlushToDisk` operation. | Mock / Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-IO-SYNC-001 | Must fetch the active `IFileHandle` from the entry and invoke its physical `FlushToDisk` method. |
| BR-IO-SYNC-002 | Must not modify any metadata objects or properties of the `OpenFileEntry` during the flush operation. |
| BR-IO-SYNC-003 | If the file handle is closed, must throw `ObjectDisposedException`. |
| BR-IO-SYNC-004 | If the low-level `FlushToDisk` throws an `IOException`, must wrap and propagate it as a `FileSyncException`. |
| BR-IO-SYNC-005 | If a durable sync fails, the system must define the expected final state of the file entry. *(Design Gap)* |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-IO-SYNC-001 | Successful file sync | Positive | Critical |
| UT-FM-IO-SYNC-002 | Handle already closed error | Negative | High |
| UT-FM-IO-SYNC-003 | OS flush fails | Dependency Failure | Critical |
| UT-FM-IO-SYNC-004 | Immutable metadata verification | Invariant | High |

## 7. Test Cases

### Case UT-FM-IO-SYNC-001 — Successful file sync
#### Objective
Verify that a valid sync call correctly invokes the physical OS flush.
#### Requirement References
- BR-IO-SYNC-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- Associated file handle is active and open.
#### Dependency Setup
- `entry.Handle` returns `fileHandle`.
- `fileHandle.FlushToDisk()` succeeds.
#### Execution
Invoke `Sync(entry)`.
#### Expected Output
Void return (Success).
#### Expected State
No internal state changes.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileHandle` | `FlushToDisk` | None | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-SYNC-002 — Handle already closed error
#### Objective
Verify that attempting to sync a closed file handle throws correctly.
#### Requirement References
- BR-IO-SYNC-003
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry` with closed handle
#### Preconditions
- File handle associated with the entry has been closed.
#### Dependency Setup
- `entry.Handle` throws `ObjectDisposedException` or `FlushToDisk` throws it.
#### Execution
Invoke `Sync(entry)`.
#### Expected Output
Throws `ObjectDisposedException`.
#### Expected State
No state changes.
#### Expected Dependency Calls
None successful.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-IO-SYNC-003 — OS flush fails
#### Objective
Verify that underlying OS I/O flush errors are captured and escalated.
#### Requirement References
- BR-IO-SYNC-004
- BR-IO-SYNC-005
#### Priority
Critical
#### Test Category
Dependency Failure
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- File handle is active.
#### Dependency Setup
- `fileHandle.FlushToDisk()` throws `IOException`.
#### Execution
Invoke `Sync(entry)`.
#### Expected Output
Throws `FileSyncException` (wrapping the `IOException`).
#### Expected State
*Pending Design Resolution.*
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileHandle` | `FlushToDisk` | None | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is wrapped and propagated.
#### Cleanup
None — resource ownership remains with the caller. *Final state verification is pending design resolution.*

### Case UT-FM-IO-SYNC-004 — Immutable metadata verification
#### Objective
Verify that the sync operation is strictly read-only regarding internal states.
#### Requirement References
- BR-IO-SYNC-002
#### Priority
High
#### Test Category
Invariant
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- Associated file handle is active and open.
- `entry.DataFile` has known metadata values.
#### Dependency Setup
- `fileHandle.FlushToDisk()` succeeds.
#### Execution
Invoke `Sync(entry)`.
#### Expected State
- `entry.DataFile` metadata states are completely unmodified.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileHandle` | `FlushToDisk` | None | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
- [x] Invalid input (Negative testing)
- [x] Boundary conditions (N/A)
- [x] Invalid state
- [x] Dependency failures
- [ ] Cleanup paths (Blocked by Design Gap)
- [x] Prohibited dependency calls

## 9. Specification Gaps
> [!WARNING]
> **Specification Gap — Design clarification required.**
> Final state after durable-sync failure is under-specified. It is unclear if the file remains functionally open or must be forced into an offline state after a flush error.

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
