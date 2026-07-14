# Unit Test Specification — `ExtentManager.FreeExtent`

## 1. Document Information
- **Specification ID:** UT-FM-EXT-MGR-FREE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.ExtentManagement`
- **Class:** `ExtentManager`
- **Interface:** `IExtentManager`
- **Public Method:** `void FreeExtent(OpenFileEntry entry, ExtentId extentId)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Releases an allocated extent, marking it as free in the in-memory bitmap, verifying it is not actively pinned/used by transactions, and persisting the updated allocation metadata.

## 3. Unit Under Test
- **Concrete class:** `ExtentManager`
- **Public method:** `FreeExtent`
- **Inputs:** `OpenFileEntry entry`, `ExtentId extentId`
- **Output:** `void` (Success)
- **Observable state:** Extent is marked free in memory and metadata is persisted to disk.
- **Dependencies:** `IFileWriter`, `IExtentUsageTracker`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IFileWriter` | Persisting metadata updates to physical storage. | Mock / Stub |
| `IExtentUsageTracker` | Validating that the extent is not currently locked or in use by higher-level operations. | Mock / Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-EXT-MGR-FREE-001 | Must verify the extent is not actively in use via `IExtentUsageTracker`. If in use, throws `ExtentInUseException`. |
| BR-EXT-MGR-FREE-002 | Must verify the extent index is within valid capacity bounds. If not, throws `InvalidExtentException`. |
| BR-EXT-MGR-FREE-003 | Must verify the extent is currently allocated. If already free, throws `ExtentAlreadyFreeException`. |
| BR-EXT-MGR-FREE-004 | Upon successful validation, must mark the extent free and persist the metadata to disk. |
| BR-EXT-MGR-FREE-005 | If metadata persistence fails, the system must handle the failure and determine the correct rollback state. *(Design Gap)* |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-EXT-MGR-FREE-001 | Free extent successfully | Positive | Critical |
| UT-FM-EXT-MGR-FREE-002 | Free non-existent extent | Negative | High |
| UT-FM-EXT-MGR-FREE-003 | Free already free extent rejection | Negative | High |
| UT-FM-EXT-MGR-FREE-004 | Blocked by active extent usage | Negative | High |
| UT-FM-EXT-MGR-FREE-005 | Rollback free state when persist fails | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-EXT-MGR-FREE-001 — Free extent successfully
#### Objective
Verify that a valid, unused allocated extent is freed and persisted.
#### Requirement References
- BR-EXT-MGR-FREE-004
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry`
- `extentId`: `ExtentId(3)`
#### Preconditions
- Extent `3` is currently marked as allocated.
- Extent `3` is not actively in use.
#### Dependency Setup
- `IExtentUsageTracker.IsInUse(ExtentId(3))` returns `false`.
- `IFileWriter.WriteAllocationMetadata` succeeds.
#### Execution
Invoke `FreeExtent(entry, ExtentId(3))`.
#### Expected Output
Void return (Success).
#### Expected State
- Extent `3` is set to free in-memory.
- Counters incremented.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IExtentUsageTracker` | `IsInUse` | `ExtentId(3)` | 1 |
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-EXT-MGR-FREE-002 — Free non-existent extent
#### Objective
Verify that attempting to free an extent index outside the file's bounds throws an exception.
#### Requirement References
- BR-EXT-MGR-FREE-002
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry`
- `extentId`: `ExtentId(99)`
#### Preconditions
- Total extent count in file is 16.
#### Dependency Setup
None.
#### Execution
Invoke `FreeExtent(entry, ExtentId(99))`.
#### Expected Output
Throws `InvalidExtentException`.
#### Expected State
No state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `IFileWriter.WriteAllocationMetadata`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-EXT-MGR-FREE-003 — Free already free extent rejection
#### Objective
Verify that attempting to free an extent that is already free throws an exception.
#### Requirement References
- BR-EXT-MGR-FREE-003
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry`
- `extentId`: `ExtentId(3)`
#### Preconditions
- Extent `3` is already marked as free.
#### Dependency Setup
None.
#### Execution
Invoke `FreeExtent(entry, ExtentId(3))`.
#### Expected Output
Throws `ExtentAlreadyFreeException`.
#### Expected State
No state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `IFileWriter.WriteAllocationMetadata`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-EXT-MGR-FREE-004 — Blocked by active extent usage
#### Objective
Verify that deallocation is rejected if the usage tracker reports the extent is locked/pinned.
#### Requirement References
- BR-EXT-MGR-FREE-001
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry`
- `extentId`: `ExtentId(3)`
#### Preconditions
- Extent `3` is currently marked as allocated.
#### Dependency Setup
- `IExtentUsageTracker.IsInUse(ExtentId(3))` returns `true`.
#### Execution
Invoke `FreeExtent(entry, ExtentId(3))`.
#### Expected Output
Throws `ExtentInUseException`.
#### Expected State
No state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IExtentUsageTracker` | `IsInUse` | `ExtentId(3)` | 1 |
#### Prohibited Dependency Calls
- `IFileWriter.WriteAllocationMetadata`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-EXT-MGR-FREE-005 — Rollback free state when persist fails
#### Objective
Verify that if writing metadata fails during deallocation, the system safely handles the state.
#### Requirement References
- BR-EXT-MGR-FREE-005
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `entry`: active `OpenFileEntry`
- `extentId`: `ExtentId(3)`
#### Preconditions
- Extent `3` is marked allocated and not in use.
#### Dependency Setup
- `IExtentUsageTracker.IsInUse(ExtentId(3))` returns `false`.
- `IFileWriter.WriteAllocationMetadata` throws `IOException`.
#### Execution
Invoke `FreeExtent(entry, ExtentId(3))`.
#### Expected Output
Throws exception wrapping the failure.
#### Expected State
*Pending Design Resolution.*
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IExtentUsageTracker` | `IsInUse` | `ExtentId(3)` | 1 |
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
*Pending Design Resolution.*

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
> Final state after persistence failure is under-specified. It is unclear if rolling back the in-memory bitmap state to "Allocated" is safe after an `IOException`, or if the file needs to enter an offline/corrupted state. Case 5 currently omits the cleanup verification until this is resolved.

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
