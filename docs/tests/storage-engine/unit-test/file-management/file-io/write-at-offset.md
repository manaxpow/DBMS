# Unit Test Specification — `FileWriter.WriteAtOffset`

## 1. Document Information
- **Specification ID:** UT-FM-IO-WRITE-OFFSET
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileIO`
- **Class:** `FileWriter`
- **Interface:** `IFileWriter`
- **Public Method:** `void WriteAtOffset(OpenFileEntry entry, long offset, ReadOnlyMemory<byte> source)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Write File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-io/write-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Provides boundary-checked, safe writing of byte sequences to a file at a specific offset. Guarantees that writes do not exceed the physical file bounds, enforces read-only access modes, and handles partial/incomplete OS writes by escalating them as failures.

## 3. Unit Under Test
- **Concrete class:** `FileWriter`
- **Public method:** `WriteAtOffset`
- **Inputs:** `OpenFileEntry entry`, `long offset`, `ReadOnlyMemory<byte> source`
- **Output:** `void` (Success)
- **Observable state:** Data is passed to the underlying OS file handle to mutate the physical file.
- **Dependencies:** Underlying OS `FileHandle` (provided via `OpenFileEntry`).

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `FileHandle` (or `IFileHandle`) | Represents the OS-level file handle for executing low-level writes. | Stub / Fake |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-IO-WRITE-OFFSET-001 | Must verify that `entry.AccessMode` allows writing (e.g., `ReadWrite`). If `ReadOnly`, throws `ReadOnlyFileException`. |
| BR-IO-WRITE-OFFSET-002 | Must calculate if the requested `offset` and `source.Length` exceed the active file bounds and throw `ArgumentOutOfRangeException` if so. |
| BR-IO-WRITE-OFFSET-003 | Negative offsets are prohibited and must throw `ArgumentOutOfRangeException`. |
| BR-IO-WRITE-OFFSET-004 | Must throw `IncompletePageWriteException` if the OS writes fewer bytes than provided in the source buffer. |
| BR-IO-WRITE-OFFSET-005 | Any underlying `IOException` from the OS must be wrapped and thrown as `WriteFailureException`. |
| BR-IO-WRITE-OFFSET-006 | The writer does not own the provided file handle. On any write failure, the handle must remain open. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-IO-WRITE-OFFSET-001 | Valid page write | Positive | Critical |
| UT-FM-IO-WRITE-OFFSET-002 | Last page boundary write | Boundary | High |
| UT-FM-IO-WRITE-OFFSET-003 | Negative offset | Negative | High |
| UT-FM-IO-WRITE-OFFSET-004 | Offset beyond bounds | Negative | High |
| UT-FM-IO-WRITE-OFFSET-005 | Read-only file write | Negative | High |
| UT-FM-IO-WRITE-OFFSET-006 | Handle already closed | Negative | High |
| UT-FM-IO-WRITE-OFFSET-007 | Incomplete/Partial write | Dependency Failure | High |
| UT-FM-IO-WRITE-OFFSET-008 | OS write failure | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-IO-WRITE-OFFSET-001 — Valid page write
#### Objective
Verify that a standard write within valid boundaries successfully writes the data.
#### Requirement References
- BR-IO-WRITE-OFFSET-001
- BR-IO-WRITE-OFFSET-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry` with `AccessMode = ReadWrite` and `physicalFileSize >= 8192`.
- `offset`: 4096
- `source`: Memory buffer of length 4096.
#### Preconditions
- Handle is open.
- Range is valid.
#### Dependency Setup
- Low-level `WriteAtOffset` succeeds entirely.
#### Execution
Invoke `WriteAtOffset(entry, 4096, source)`.
#### Expected Output
Void return (Success).
#### Expected State
Data is passed to the OS to be written to disk.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `source, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-IO-WRITE-OFFSET-002 — Last page boundary write
#### Objective
Verify boundary condition: writing exactly to the end of the file capacity bounds.
#### Requirement References
- BR-IO-WRITE-OFFSET-002
#### Priority
High
#### Test Category
Boundary
#### Input
- `entry`: `OpenFileEntry` with physical size exactly 1MB.
- `offset`: 1044480 (1MB - 4096)
- `source`: buffer of length 4096.
#### Preconditions
- Range fits exactly inside physical file capacity.
#### Dependency Setup
- Low-level write succeeds.
#### Execution
Invoke `WriteAtOffset(entry, 1044480, source)`.
#### Expected Output
Void return (Success).
#### Expected State
Data is written.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `source, 1044480` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-IO-WRITE-OFFSET-003 — Negative offset
#### Objective
Verify that negative offsets are rejected before querying the OS.
#### Requirement References
- BR-IO-WRITE-OFFSET-003
#### Priority
High
#### Test Category
Negative
#### Input
- `offset`: -4096
- `source`: buffer of length 4096.
#### Preconditions
N/A
#### Dependency Setup
None.
#### Execution
Invoke `WriteAtOffset(...)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.
#### Expected State
No data written.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `FileHandle.WriteAtOffset`
#### Expected Failure Handling
Exception is propagated immediately.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-OFFSET-004 — Offset beyond bounds
#### Objective
Verify that requests extending past the physical file boundary are rejected.
#### Requirement References
- BR-IO-WRITE-OFFSET-002
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: `OpenFileEntry` with physical size 1MB.
- `offset`: 1048576 (1MB)
- `source`: buffer of length 4096.
#### Preconditions
- Offset + Length exceeds physical boundaries.
#### Dependency Setup
None.
#### Execution
Invoke `WriteAtOffset(...)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.
#### Expected State
No data written.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `FileHandle.WriteAtOffset`
#### Expected Failure Handling
Exception is propagated immediately.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-OFFSET-005 — Read-only file write
#### Objective
Verify that attempting to write to a read-only file handle is explicitly denied at the domain level.
#### Requirement References
- BR-IO-WRITE-OFFSET-001
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: `OpenFileEntry` opened with `AccessMode = ReadOnly`.
#### Preconditions
N/A
#### Dependency Setup
None.
#### Execution
Invoke `WriteAtOffset(...)`.
#### Expected Output
Throws `ReadOnlyFileException`.
#### Expected State
No data written.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `FileHandle.WriteAtOffset`
#### Expected Failure Handling
Exception is propagated immediately.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-OFFSET-006 — Handle already closed
#### Objective
Verify that writing to a disposed handle throws correctly.
#### Requirement References
- BR-IO-WRITE-OFFSET-006
#### Priority
High
#### Test Category
Negative
#### Input
- `offset`: 4096
- `source`: buffer of length 4096.
#### Preconditions
- Associated handle is disposed or closed.
#### Dependency Setup
- Handle throws `ObjectDisposedException` when accessed.
#### Execution
Invoke `WriteAtOffset(...)`.
#### Expected Output
Throws `ObjectDisposedException`.
#### Expected State
No data written.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `source, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-OFFSET-007 — Incomplete/Partial write
#### Objective
Verify that an incomplete write to the OS throws an exception to prevent silent data loss.
#### Requirement References
- BR-IO-WRITE-OFFSET-004
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `offset`: 4096
- `source`: buffer of length 4096.
#### Preconditions
- Physical OS write commits fewer bytes than the requested buffer length.
#### Dependency Setup
- Low-level write returns 2048 instead of 4096.
#### Execution
Invoke `WriteAtOffset(...)`.
#### Expected Output
Throws `IncompletePageWriteException`.
#### Expected State
File state on disk is indeterminate/corrupted.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `source, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-OFFSET-008 — OS write failure
#### Objective
Verify that standard OS access exceptions are wrapped into a domain-specific write failure exception.
#### Requirement References
- BR-IO-WRITE-OFFSET-005
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `offset`: 4096
- `source`: buffer of length 4096.
#### Preconditions
- Low-level write encounters physical disk error (e.g. disk full, access denied).
#### Dependency Setup
- Low-level write throws `IOException`.
#### Execution
Invoke `WriteAtOffset(...)`.
#### Expected Output
Throws `WriteFailureException` (wrapping the `IOException`).
#### Expected State
No data written.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `source, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is wrapped and propagated.
#### Cleanup
None — resource ownership remains with the caller.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
- [x] Invalid input (Negative testing)
- [x] Boundary conditions
- [x] Invalid state
- [x] Dependency failures
- [x] Cleanup paths (Resource ownership clearly documented)
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
