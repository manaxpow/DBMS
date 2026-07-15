# Unit Test Specification — `FileReader.ReadAtOffset`

## 1. Document Information
- **Specification ID:** UT-FM-IO-READ-OFFSET
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileIO`
- **Class:** `FileReader`
- **Interface:** `IFileReader`
- **Public Method:** `int ReadAtOffset(OpenFileEntry entry, long offset, Memory<byte> destination)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Read File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-io/read-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Provides boundary-checked, safe reading of byte sequences from a file at a specific offset. It guarantees that reads do not exceed the physical file boundary and that partial/incomplete OS reads are handled gracefully or rejected.

## 3. Unit Under Test
- **Concrete class:** `FileReader`
- **Public method:** `ReadAtOffset`
- **Inputs:** `OpenFileEntry entry`, `long offset`, `Memory<byte> destination`
- **Output:** `int` (Number of bytes read)
- **Observable state:** The `destination` memory buffer is populated with the read data.
- **Dependencies:** Underlying OS `FileHandle` (provided via `OpenFileEntry`).

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `FileHandle` (or `IFileHandle`) | Represents the OS-level file handle for executing low-level reads. | Stub / Fake |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-IO-READ-OFFSET-001 | Must calculate if the requested `offset` and `destination.Length` exceed the active file bounds and throw `ArgumentOutOfRangeException` if so. |
| BR-IO-READ-OFFSET-002 | Negative offsets are prohibited and must throw `ArgumentOutOfRangeException`. |
| BR-IO-READ-OFFSET-003 | Must throw `IncompletePageReadException` if the OS returns fewer bytes than requested in the buffer. |
| BR-IO-READ-OFFSET-004 | Any underlying `IOException` from the OS must be wrapped and thrown as `ReadFailureException`. |
| BR-IO-READ-OFFSET-005 | The reader does not own the provided file handle. On any read failure, the handle must remain open. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-IO-READ-OFFSET-001 | Valid page read | Positive | Critical |
| UT-FM-IO-READ-OFFSET-002 | Read at offset 0 | Boundary | High |
| UT-FM-IO-READ-OFFSET-003 | Read last page boundary | Boundary | High |
| UT-FM-IO-READ-OFFSET-004 | Negative offset | Negative | High |
| UT-FM-IO-READ-OFFSET-005 | Range exceeds file size | Negative | High |
| UT-FM-IO-READ-OFFSET-006 | Closed handles | Negative | High |
| UT-FM-IO-READ-OFFSET-007 | Partial read | Dependency Failure | High |
| UT-FM-IO-READ-OFFSET-008 | Exact read missing bytes | Dependency Failure | High |
| UT-FM-IO-READ-OFFSET-009 | OS read exception | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-IO-READ-OFFSET-001 — Valid page read
#### Objective
Verify that a standard read within valid boundaries returns the bytes and populates the buffer.
#### Requirement References
- BR-IO-READ-OFFSET-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry` with size >= 8192.
- `offset`: 4096
- `destination`: Memory buffer of length 4096.
#### Preconditions
- Handle is open.
- Offset and length are within bounds.
#### Dependency Setup
- Low-level `ReadAtOffset` returns 4096.
#### Execution
Invoke `ReadAtOffset(entry, 4096, destination)`.
#### Expected Output
Returns `4096`.
#### Expected State
`destination` buffer contains the read bytes.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-IO-READ-OFFSET-002 — Read at offset 0
#### Objective
Verify boundary condition: reading from the very beginning of the file.
#### Requirement References
- BR-IO-READ-OFFSET-001
#### Priority
High
#### Test Category
Boundary
#### Input
- `offset`: 0
- `destination`: buffer of length 4096.
#### Preconditions
- File size >= 4096.
#### Dependency Setup
- Low-level read returns 4096.
#### Execution
Invoke `ReadAtOffset(entry, 0, destination)`.
#### Expected Output
Returns `4096`.
#### Expected State
Buffer populated.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 0` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-IO-READ-OFFSET-003 — Read last page boundary
#### Objective
Verify boundary condition: reading the exact last page fitting exactly within the file bounds.
#### Requirement References
- BR-IO-READ-OFFSET-001
#### Priority
High
#### Test Category
Boundary
#### Input
- `offset`: 1044480 (1MB - 4096)
- `destination`: buffer of length 4096.
#### Preconditions
- File physical size is exactly 1MB.
#### Dependency Setup
- Low-level read returns 4096.
#### Execution
Invoke `ReadAtOffset(entry, 1044480, destination)`.
#### Expected Output
Returns `4096`.
#### Expected State
Buffer populated.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 1044480` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-IO-READ-OFFSET-004 — Negative offset
#### Objective
Verify that negative offsets are rejected before querying the OS.
#### Requirement References
- BR-IO-READ-OFFSET-002
#### Priority
High
#### Test Category
Negative
#### Input
- `offset`: -4096
- `destination`: buffer of length 4096.
#### Preconditions
N/A
#### Dependency Setup
None.
#### Execution
Invoke `ReadAtOffset(...)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.
#### Expected State
No data read.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `FileHandle.ReadAtOffset`
#### Expected Failure Handling
Exception is propagated immediately.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-OFFSET-005 — Range exceeds file size
#### Objective
Verify that requests extending past the physical file boundary are rejected.
#### Requirement References
- BR-IO-READ-OFFSET-001
#### Priority
High
#### Test Category
Negative
#### Input
- `offset`: 1048576 (1MB)
- `destination`: buffer of length 4096.
#### Preconditions
- File physical size is 1MB. (Offset + Length = 1MB + 4KB > 1MB).
#### Dependency Setup
None.
#### Execution
Invoke `ReadAtOffset(...)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.
#### Expected State
No data read.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `FileHandle.ReadAtOffset`
#### Expected Failure Handling
Exception is propagated immediately.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-OFFSET-006 — Closed handles
#### Objective
Verify that reading from a disposed handle throws correctly without corrupting memory.
#### Requirement References
- BR-IO-READ-OFFSET-005
#### Priority
High
#### Test Category
Negative
#### Input
- `offset`: 4096
- `destination`: buffer of length 4096.
#### Preconditions
- Associated handle is disposed or closed prior to the call.
#### Dependency Setup
- Handle throws `ObjectDisposedException` when accessed.
#### Execution
Invoke `ReadAtOffset(...)`.
#### Expected Output
Throws `ObjectDisposedException`.
#### Expected State
No data read.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-OFFSET-007 — Partial read
#### Objective
Verify that an incomplete read from the OS (fewer bytes than requested) throws an exception to prevent silent data corruption.
#### Requirement References
- BR-IO-READ-OFFSET-003
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `offset`: 4096
- `destination`: buffer of length 4096.
#### Preconditions
- Low-level read returns a value strictly lower than requested buffer size.
#### Dependency Setup
- Low-level read returns 2048.
#### Execution
Invoke `ReadAtOffset(...)`.
#### Expected Output
Throws `IncompletePageReadException`.
#### Expected State
Buffer state is indeterminate.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-OFFSET-008 — Exact read missing bytes
#### Objective
Verify extreme case of partial read: missing exactly 1 byte triggers the incomplete read exception.
#### Requirement References
- BR-IO-READ-OFFSET-003
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `offset`: 4096
- `destination`: buffer of length 4096.
#### Preconditions
- OS returns `buffer length - 1`.
#### Dependency Setup
- Low-level read returns 4095.
#### Execution
Invoke `ReadAtOffset(...)`.
#### Expected Output
Throws `IncompletePageReadException`.
#### Expected State
Buffer state is indeterminate.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 4096` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-OFFSET-009 — OS read exception
#### Objective
Verify that standard OS access exceptions are wrapped into a domain-specific exception.
#### Requirement References
- BR-IO-READ-OFFSET-004
- BR-IO-READ-OFFSET-005
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `offset`: 4096
- `destination`: buffer of length 4096.
#### Preconditions
- Low-level filesystem encounters a physical error.
#### Dependency Setup
- Low-level read throws `IOException`.
#### Execution
Invoke `ReadAtOffset(...)`.
#### Expected Output
Throws `ReadFailureException` (wrapping the `IOException`).
#### Expected State
Buffer state is empty/indeterminate.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `destination, 4096` | 1 |
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
