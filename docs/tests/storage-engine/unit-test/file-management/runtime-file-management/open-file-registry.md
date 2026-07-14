# Unit Test Specification — `OpenFileManager` (Registry)

## 1. Document Information
- **Specification ID:** UT-FM-RT-REGISTRY
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.RuntimeFileManagement`
- **Class:** `OpenFileManager`
- **Interface:** `IOpenFileManager`
- **Public Methods:**
  - `void RegisterOpenFile(string fileName, OpenFileEntry entry)`
  - `OpenFileEntry GetOpenFile(string fileName)`
  - `void UnregisterOpenFile(string fileName)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Provides thread-safe, centralized mapping between active file names and their `OpenFileEntry` runtime objects. It ensures that consumers can lookup open files, and strictly controls duplicate registrations.

## 3. Unit Under Test
- **Concrete class:** `OpenFileManager`
- **Public methods:** `RegisterOpenFile`, `GetOpenFile`, `UnregisterOpenFile`
- **Inputs:** `string fileName`, `OpenFileEntry entry`
- **Output:** Depends on method.
- **Observable state:** Internal registry collections are updated.
- **Dependencies:** None. Tested as an in-memory collection manager.

## 4. Dependencies
None.

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-RT-REGISTRY-001 | Must correctly store and retrieve an `OpenFileEntry` by its file name string. |
| BR-RT-REGISTRY-002 | Must completely remove an entry from internal collections upon unregistration, so subsequent lookups return null. |
| BR-RT-REGISTRY-003 | Must leave the internal state of the provided `OpenFileEntry` (e.g., ReferenceCount) unmodified. |
| BR-RT-REGISTRY-004 | If a file name is already registered, duplicate registrations must be safely handled according to the design specification. *(Design Gap)* |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-RT-REGISTRY-001 | Register new file | Positive | Critical |
| UT-FM-RT-REGISTRY-002 | Get registered file | Positive | Critical |
| UT-FM-RT-REGISTRY-003 | Unregister file | Positive | Critical |
| UT-FM-RT-REGISTRY-004 | Register duplicate file conflict | Negative | High |
| UT-FM-RT-REGISTRY-005 | Reference state preservation | Invariant | High |

## 7. Test Cases

### Case UT-FM-RT-REGISTRY-001 — Register new file
#### Objective
Verify that a new file is successfully stored in the registry.
#### Requirement References
- BR-RT-REGISTRY-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: `"test.db"`
- `entry`: active `OpenFileEntry`
#### Preconditions
- `"test.db"` is not currently registered.
#### Execution
Invoke `RegisterOpenFile("test.db", entry)`.
#### Expected Output
Void return (Success).
#### Expected State
- The registry correctly maps `"test.db"` to the provided entry.
#### Expected Failure Handling
N/A

### Case UT-FM-RT-REGISTRY-002 — Get registered file
#### Objective
Verify that a registered file can be retrieved by its name.
#### Requirement References
- BR-RT-REGISTRY-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- `"test.db"` has been previously registered with `entry`.
#### Execution
Invoke `GetOpenFile("test.db")`.
#### Expected Output
Returns the matching `OpenFileEntry` reference.
#### Expected State
No state change.

### Case UT-FM-RT-REGISTRY-003 — Unregister file
#### Objective
Verify that a file is completely removed from the registry.
#### Requirement References
- BR-RT-REGISTRY-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `fileName`: `"test.db"`
#### Preconditions
- `"test.db"` is registered.
#### Execution
Invoke `UnregisterOpenFile("test.db")`.
#### Expected Output
Void return.
#### Expected State
- Entry is removed from all internal collections.
- Subsequent `GetOpenFile("test.db")` returns `null`.

### Case UT-FM-RT-REGISTRY-004 — Register duplicate file conflict
#### Objective
Verify the behavior of the registry when a duplicate registration is attempted.
#### Requirement References
- BR-RT-REGISTRY-004
#### Priority
High
#### Test Category
Negative
#### Input
- `fileName`: `"test.db"`
- `entry`: `newOpenFileEntry`
#### Preconditions
- `"test.db"` is already registered with a different entry.
#### Execution
Invoke `RegisterOpenFile("test.db", newOpenFileEntry)`.
#### Expected Output
*Pending Design Resolution.* (Previously specified to throw `FileAlreadyOpenException`, but requires clarification).
#### Expected State
*Pending Design Resolution.*
#### Expected Failure Handling
*Pending Design Resolution.*

### Case UT-FM-RT-REGISTRY-005 — Reference state preservation
#### Objective
Verify that the registry strictly acts as a container and does not alter the referenced objects.
#### Requirement References
- BR-RT-REGISTRY-003
#### Priority
High
#### Test Category
Invariant
#### Input
- `fileName`: `"test.db"`
- `entry`: `openFileEntry` (ReferenceCount = 1)
#### Preconditions
- `"test.db"` is not registered.
#### Execution
Invoke `RegisterOpenFile("test.db", entry)`.
#### Expected State
- `entry.ReferenceCount` remains exactly `1` (unchanged by the registration operation).

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
- [x] Invalid input (Negative testing)
- [x] Boundary conditions (N/A)
- [x] Invalid state
- [x] Dependency failures (N/A)
- [x] Cleanup paths (N/A)
- [x] Prohibited dependency calls (N/A)

## 9. Specification Gaps
> [!WARNING]
> **Specification Gap — Design clarification required.**
> The result of duplicate registration is under-specified. It must be clarified whether a duplicate registration throws an exception, returns a boolean flag, or acts idempotently. The expected final state of the registry (whether the original entry is preserved or replaced) must also be explicitly approved in the architecture.

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
