# Unit Test Specification — `OpenFileManager` Delete Coordination

## 1. Document Information
- **Specification ID:** UT-FM-RT-DELETE-COORD
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.RuntimeFileManagement`
- **Class:** `OpenFileManager`
- **Interface:** `IOpenFileManager`
- **Public Methods:**
  - `bool TryBeginDelete(string fileName)`
  - `void CompleteDelete(string fileName)`
  - `void CancelDelete(string fileName)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Delete File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/delete-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Provides atomic locking and coordination for physical file deletion. It prevents new threads from opening a file while it is being deleted and prevents deletion of a file while it is currently in use.

## 3. Unit Under Test
- **Concrete class:** `OpenFileManager`
- **Public methods:** `TryBeginDelete`, `CompleteDelete`, `CancelDelete`
- **Inputs:** `string fileName`
- **Output:** `bool` for `TryBeginDelete`; `void` for others.
- **Observable state:** The internal registry transitions between active, deleting, and free states.
- **Dependencies:** None.

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| None | N/A | N/A |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-RT-DELETE-COORD-001 | `TryBeginDelete` must return `true` and atomically place the file in a `Deleting` state if it is not currently open. |
| BR-RT-DELETE-COORD-002 | `TryBeginDelete` must return `false` if the file is currently open and active. |
| BR-RT-DELETE-COORD-003 | `CompleteDelete` must clear the deletion lock and remove all registry state for the file. |
| BR-RT-DELETE-COORD-004 | `CancelDelete` must remove the deletion lock and revert the file state to inactive/free. |
| BR-RT-DELETE-COORD-005 | Any attempt to register or open a file that is in the `Deleting` state must throw `FileInUseException`. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-RT-DELETE-COORD-001 | TryBeginDelete on inactive file | Positive | Critical |
| UT-FM-RT-DELETE-COORD-002 | TryBeginDelete blocked by active entry | Alternative | High |
| UT-FM-RT-DELETE-COORD-003 | CompleteDelete | Positive | Critical |
| UT-FM-RT-DELETE-COORD-004 | CancelDelete | Positive | Critical |
| UT-FM-RT-DELETE-COORD-005 | Block open in Deleting state | Negative | High |

## 7. Test Cases

### Case UT-FM-RT-DELETE-COORD-001 — TryBeginDelete on inactive file
#### Objective
Verify that `TryBeginDelete` successfully acquires a deletion lock on a file that is not open.
#### Requirement References
- BR-RT-DELETE-COORD-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: "test.db"
#### Preconditions
- "test.db" is not currently open or registered.
#### Dependency Setup
None.
#### Execution
Invoke `TryBeginDelete("test.db")`.
#### Expected Output
Returns `true`.
#### Expected State
Registry slot transitions atomically into a `Deleting` lock state.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-RT-DELETE-COORD-002 — TryBeginDelete blocked by active entry
#### Objective
Verify that deletion locks are denied if a file is currently active.
#### Requirement References
- BR-RT-DELETE-COORD-002
#### Priority
High
#### Test Category
Alternative
#### Input
- `fileName`: "test.db"
#### Preconditions
- "test.db" is currently open and registered with an active entry.
#### Dependency Setup
None.
#### Execution
Invoke `TryBeginDelete("test.db")`.
#### Expected Output
Returns `false`.
#### Expected State
File state remains active and is not marked for deletion.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-RT-DELETE-COORD-003 — CompleteDelete
#### Objective
Verify that finalizing a deletion clears all internal registry state for the file.
#### Requirement References
- BR-RT-DELETE-COORD-003
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: "test.db"
#### Preconditions
- "test.db" is currently in a `Deleting` lock state.
#### Dependency Setup
None.
#### Execution
Invoke `CompleteDelete("test.db")`.
#### Expected Output
Void return.
#### Expected State
Lock marker is removed, and registry key is completely cleared.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-RT-DELETE-COORD-004 — CancelDelete
#### Objective
Verify that canceling a deletion safely reverts the lock state to free.
#### Requirement References
- BR-RT-DELETE-COORD-004
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: "test.db"
#### Preconditions
- "test.db" is currently in a `Deleting` lock state.
#### Dependency Setup
None.
#### Execution
Invoke `CancelDelete("test.db")`.
#### Expected Output
Void return.
#### Expected State
Lock marker is removed, returning slot to free/available state.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-RT-DELETE-COORD-005 — Block open in Deleting state
#### Objective
Verify that incoming open/register requests are rejected while a file is being deleted.
#### Requirement References
- BR-RT-DELETE-COORD-005
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: "test.db"
- `entry`: valid `OpenFileEntry`
#### Preconditions
- "test.db" is locked in a `Deleting` state.
#### Dependency Setup
None.
#### Execution
Invoke `RegisterOpenFile("test.db", entry)`.
#### Expected Output
Throws `FileInUseException`.
#### Expected State
State remains `Deleting`. New entry is not registered.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior
- [x] Invalid input (Negative testing)
- [x] Boundary conditions (N/A)
- [x] Invalid state
- [x] Dependency failures (N/A)
- [x] Cleanup paths (N/A)
- [x] Prohibited dependency calls (N/A)

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
