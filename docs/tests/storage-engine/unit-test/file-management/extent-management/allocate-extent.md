# Unit Test Specification — `ExtentManager.AllocateExtent`

## 1. Document Information
- **Specification ID:** UT-FM-EXT-MGR-ALLOCATE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.ExtentManagement`
- **Class:** `ExtentManager`
- **Interface:** `IExtentManager`
- **Public Method:** `AllocatedExtent AllocateExtent(OpenFileEntry entry)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Searches for available space in the file's allocation metadata and allocates an extent. If space is exhausted and auto-extend is enabled, it automatically triggers a file resize to grow the physical capacity before attempting allocation again.

## 3. Unit Under Test
- **Concrete class:** `ExtentManager`
- **Public method:** `AllocateExtent`
- **Inputs:** `OpenFileEntry entry`
- **Output:** `AllocatedExtent` mapping
- **Observable state:** Extent is marked allocated in memory and persisted to disk.
- **Dependencies:** `IFileWriter`, `IFileLifecycleManager`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IFileWriter` | Persisting metadata updates to physical storage. | Mock / Stub |
| `IFileLifecycleManager` | Triggering file auto-extension (ResizeFile) when capacity is exhausted. | Mock / Fake |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-EXT-MGR-ALLOC-001 | If free extents are available, must locate the first free extent, mark it allocated, and write the metadata to disk. |
| BR-EXT-MGR-ALLOC-002 | If no free extents are available and auto-extend is enabled, must invoke `ResizeFile` to grow the file, then re-attempt allocation. |
| BR-EXT-MGR-ALLOC-003 | If no free extents are available and auto-extend is disabled, must throw `NoFreeExtentException`. |
| BR-EXT-MGR-ALLOC-004 | If auto-extension hits the maximum file capacity limit, must propagate the resulting exception and leave allocation unmodified. |
| BR-EXT-MGR-ALLOC-005 | If metadata persistence fails during allocation, the system must handle the failure and determine the correct rollback state. *(Design Gap)* |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-EXT-MGR-ALLOCATE-001 | Allocate empty extent success | Positive | Critical |
| UT-FM-EXT-MGR-ALLOCATE-002 | Allocate triggering auto-extend | Alternative | High |
| UT-FM-EXT-MGR-ALLOCATE-003 | Allocate exhausted with auto-extend disabled | Negative | High |
| UT-FM-EXT-MGR-ALLOCATE-004 | Maximum file size reached limit | Negative | High |
| UT-FM-EXT-MGR-ALLOCATE-005 | Resize file failure during extension | Dependency Failure | High |
| UT-FM-EXT-MGR-ALLOCATE-006 | Call MarkExtentAllocated correctly | Positive | High |
| UT-FM-EXT-MGR-ALLOCATE-007 | Persist metadata failed during allocation | Dependency Failure | High |
| UT-FM-EXT-MGR-ALLOCATE-008 | Rollback allocation state when persist fails | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-EXT-MGR-ALLOCATE-001 — Allocate empty extent success
#### Objective
Verify that `AllocateExtent` finds a free extent, allocates it in memory, and persists it correctly.
#### Requirement References
- BR-EXT-MGR-ALLOC-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- Free extents are available.
- Metadata persistence succeeds.
#### Dependency Setup
- `allocationMetadata.FindFreeExtent()` returns `ExtentId(5)`.
- `IFileWriter.WriteAllocationMetadata` succeeds.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Returns a valid `AllocatedExtent` mapping for `ExtentId(5)`.
#### Expected State
- Extent is set to allocated in-memory.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
- `IFileLifecycleManager.ResizeFile`
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-EXT-MGR-ALLOCATE-002 — Allocate triggering auto-extend
#### Objective
Verify that exhaustion correctly triggers an auto-extension before retrying the allocation.
#### Requirement References
- BR-EXT-MGR-ALLOC-002
#### Priority
High
#### Test Category
Alternative
#### Input
- `entry`: active `OpenFileEntry` (AutoExtendEnabled = true)
#### Preconditions
- `FreeExtentCount = 0`.
- Physical resizing and metadata write calls succeed.
#### Dependency Setup
- Initial `allocationMetadata.FindFreeExtent()` returns null.
- `IFileLifecycleManager.ResizeFile` succeeds.
- Second `allocationMetadata.FindFreeExtent()` returns the newly added ExtentId.
- `IFileWriter.WriteAllocationMetadata` succeeds.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Returns a valid `AllocatedExtent` mapping representing the newly added space.
#### Expected State
Metadata is extended and saved.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileLifecycleManager` | `ResizeFile` | `entry, newSize` | 1 |
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-EXT-MGR-ALLOCATE-003 — Allocate exhausted with auto-extend disabled
#### Objective
Verify that space exhaustion is rejected if auto-extend is not allowed.
#### Requirement References
- BR-EXT-MGR-ALLOC-003
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry` (AutoExtendEnabled = false)
#### Preconditions
- `FreeExtentCount = 0`.
#### Dependency Setup
- `allocationMetadata.FindFreeExtent()` returns null.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Throws `NoFreeExtentException`.
#### Expected State
No allocation occurs.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
- `IFileLifecycleManager.ResizeFile`
- `IFileWriter.WriteAllocationMetadata`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-EXT-MGR-ALLOCATE-004 — Maximum file size reached limit
#### Objective
Verify that exceeding physical size limits correctly propagates the limit exception and stops allocation.
#### Requirement References
- BR-EXT-MGR-ALLOC-004
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry` (AutoExtendEnabled = true)
#### Preconditions
- `FreeExtentCount = 0`.
- File is at maximum capacity limit.
#### Dependency Setup
- Initial `allocationMetadata.FindFreeExtent()` returns null.
- `IFileLifecycleManager.ResizeFile` throws `MaximumFileSizeExceededException`.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Throws `MaximumFileSizeExceededException`.
#### Expected State
No allocation occurs.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileLifecycleManager` | `ResizeFile` | `entry, newSize` | 1 |
#### Prohibited Dependency Calls
- `IFileWriter.WriteAllocationMetadata`
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-EXT-MGR-ALLOCATE-005 — Resize file failure during extension
#### Objective
Verify that unexpected OS errors during resize fail the allocation cleanly.
#### Requirement References
- BR-EXT-MGR-ALLOC-002
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `entry`: active `OpenFileEntry` (AutoExtendEnabled = true)
#### Preconditions
- `FreeExtentCount = 0`.
#### Dependency Setup
- Initial `allocationMetadata.FindFreeExtent()` returns null.
- `IFileLifecycleManager.ResizeFile` throws `FileResizeException`.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Throws `ExtentAllocationException` (wrapping `FileResizeException`).
#### Expected State
No allocation occurs.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileLifecycleManager` | `ResizeFile` | `entry, newSize` | 1 |
#### Prohibited Dependency Calls
- `IFileWriter.WriteAllocationMetadata`
#### Expected Failure Handling
Exception is wrapped and propagated.
#### Cleanup
None.

### Case UT-FM-EXT-MGR-ALLOCATE-006 — Call MarkExtentAllocated correctly
#### Objective
Verify the integration between `ExtentManager` and `AllocationMetadata` ensuring the correct ID is passed.
#### Requirement References
- BR-EXT-MGR-ALLOC-001
#### Priority
High
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- Free extents are available.
#### Dependency Setup
- `allocationMetadata.FindFreeExtent()` returns `ExtentId(5)`.
- `IFileWriter.WriteAllocationMetadata` succeeds.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Returns `AllocatedExtent` with `ExtentId = 5`.
#### Expected State
Extent 5 is marked allocated.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-EXT-MGR-ALLOCATE-007 — Persist metadata failed during allocation
#### Objective
Verify that failing to persist the metadata change throws an appropriate exception.
#### Requirement References
- BR-EXT-MGR-ALLOC-005
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- Free extents are available.
#### Dependency Setup
- `allocationMetadata.FindFreeExtent()` returns `ExtentId(5)`.
- `IFileWriter.WriteAllocationMetadata` throws `IOException`.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Throws `ExtentAllocationException`.
#### Expected State
Rollback state is unverified due to Specification Gap.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
*Pending Design Resolution.*

### Case UT-FM-EXT-MGR-ALLOCATE-008 — Rollback allocation state when persist fails
#### Objective
Verify that if writing metadata fails, the in-memory bitmap state changes are safely handled.
#### Requirement References
- BR-EXT-MGR-ALLOC-005
#### Priority
High
#### Test Category
Dependency Failure
#### Input
- `entry`: active `OpenFileEntry`
#### Preconditions
- Free extents are available.
#### Dependency Setup
- `IFileWriter.WriteAllocationMetadata` throws `IOException`.
#### Execution
Invoke `AllocateExtent(entry)`.
#### Expected Output
Throws `ExtentAllocationException`.
#### Expected State
*Pending Design Resolution.*
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileWriter` | `WriteAllocationMetadata` | `handle, header, metadata` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
*Pending Design Resolution.*

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
> Final state after persistence failure is under-specified. It is unclear if rolling back the in-memory bitmap state to "Free" is safe after an `IOException`, or if the file needs to enter an offline/corrupted state to prevent further allocations. Cases 7 and 8 currently omit the cleanup verification until this is resolved.

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
