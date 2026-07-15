# Unit Test Specification — `FileLifecycleManager.ResizeFile`

## 1. Document Information
- **Specification ID:** UT-FM-LC-RESIZE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileLifecycle`
- **Class:** `FileLifecycleManager`
- **Interface:** `IFileLifecycleManager`
- **Public Method:** `void ResizeFile(OpenFileEntry entry, long newSize)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Changes the physical length of the underlying data file (either growing or shrinking) and perfectly synchronizes the in-memory metadata mappings (like AllocationMetadata) with the new size before committing the changes durably.

## 3. Unit Under Test
- **Concrete class:** `FileLifecycleManager`
- **Public method:** `ResizeFile`
- **Inputs:** `OpenFileEntry entry`, `long newSize`
- **Output:** `void` (Success)
- **Observable state:** Physical file size is adjusted, internal extents added/removed, and changes synced to disk.
- **Dependencies:** `IPhysicalFileSystem`, `IFileWriter`, `IFileSynchronizer`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `IPhysicalFileSystem` | Performs the low-level physical OS file length change. | Mock / Stub |
| `IFileWriter` | Commits updated structural headers (AllocationMetadata) back to the file. | Mock / Stub |
| `IFileSynchronizer` | Flushes structural/data changes synchronously to disk. | Mock / Stub |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-LC-RESIZE-001 | If the new size is not page/extent aligned or is negative, must throw `ArgumentOutOfRangeException`. |
| BR-LC-RESIZE-002 | If the new size exceeds configured limits, must throw `MaximumFileSizeExceededException`. |
| BR-LC-RESIZE-003 | If the new size is equal to the current size, must return silently without invoking dependencies. |
| BR-LC-RESIZE-004 | When shrinking, must verify that the truncated space contains no active allocations. Otherwise, must throw `FileTruncationException`. |
| BR-LC-RESIZE-005 | Must invoke physical `Resize`, recompute/update the in-memory metadata mappings, and write those structural changes back to disk. |
| BR-LC-RESIZE-006 | If the physical `Resize` throws an `IOException`, must wrap and throw it as `FileResizeException` leaving metadata unmodified. |
| BR-LC-RESIZE-007 | If metadata writing or syncing fails AFTER the physical file has been resized, the system must handle compensation or define the corrupt state. *(Design Gap)* |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-LC-RESIZE-001 | Extend file successfully | Positive | Critical |
| UT-FM-LC-RESIZE-002 | Truncate file safely | Positive | Critical |
| UT-FM-LC-RESIZE-003 | Resize no-op | Alternative | Medium |
| UT-FM-LC-RESIZE-004 | Invalid size arguments rejection | Boundary | High |
| UT-FM-LC-RESIZE-005 | Truncate cuts into used extent | Negative | High |
| UT-FM-LC-RESIZE-006 | Maximum file size exceeded | Negative | High |
| UT-FM-LC-RESIZE-007 | Physical OS resize fails | Dependency Failure | Critical |
| UT-FM-LC-RESIZE-008 | Metadata write fails after resize | Dependency Failure | High |

## 7. Test Cases

### Case UT-FM-LC-RESIZE-001 — Extend file successfully
#### Objective
Verify that extending a file physically expands it and updates the metadata structure to track new space.
#### Requirement References
- BR-LC-RESIZE-005
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry` (CurrentSize = 1MB)
- `newSize`: `2097152` (2MB)
#### Preconditions
- Physical disk operations, metadata writes, and sync calls succeed.
#### Dependency Setup
- `entry.DataFile.Size` returns 1MB.
- `IPhysicalFileSystem.Resize(fileHandle, 2097152)` succeeds.
- `AllocationMetadata.AddExtents` succeeds.
- `IFileWriter.WriteAllocationMetadata` succeeds.
- `IFileSynchronizer.Sync(entry)` succeeds.
#### Execution
Invoke `ResizeFile(entry, 2097152)`.
#### Expected Output
Void return (Success).
#### Expected State
- Physical length is set to 2MB.
- New extents are added to allocation maps.
- `entry.DataFile.CurrentSize` is updated to 2MB.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Resize` | `fileHandle, 2097152` | 1 |
| `IFileWriter` | `WriteAllocationMetadata` | `fileHandle, header, metadata` | 1 |
| `IFileSynchronizer` | `Sync` | `entry` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A

### Case UT-FM-LC-RESIZE-002 — Truncate file safely
#### Objective
Verify that shrinking removes metadata space and safely curtails the physical file.
#### Requirement References
- BR-LC-RESIZE-004
- BR-LC-RESIZE-005
#### Priority
Critical
#### Test Category
Positive
#### Input
- `entry`: active `OpenFileEntry` (CurrentSize = 2MB)
- `newSize`: `1048576` (1MB)
#### Preconditions
- The upper 1MB range contains only unallocated (free) extents.
#### Dependency Setup
- `AllocationMetadata.CanTruncateTo(16)` returns `true`.
- `AllocationMetadata.TruncateTo(16)` succeeds.
- All OS and write operations succeed.
#### Execution
Invoke `ResizeFile(entry, 1048576)`.
#### Expected Output
Void return (Success).
#### Expected State
- Physical length is shrunk to 1MB.
- In-memory metadata removes upper extents.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IFileWriter` | `WriteAllocationMetadata` | `fileHandle, header, metadata` | 1 |
| `IPhysicalFileSystem` | `Resize` | `fileHandle, 1048576` | 1 |
| `IFileSynchronizer` | `Sync` | `entry` | 1 |

### Case UT-FM-LC-RESIZE-003 — Resize no-op
#### Objective
Verify that requesting a resize to the exact same size bypasses physical changes.
#### Requirement References
- BR-LC-RESIZE-003
#### Priority
Medium
#### Test Category
Alternative
#### Input
- `entry`: active `OpenFileEntry` (CurrentSize = 1MB)
- `newSize`: `1048576` (1MB)
#### Execution
Invoke `ResizeFile(entry, 1048576)`.
#### Expected Output
Void return (Success).
#### Expected Dependency Calls
None related to physical changes.
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Resize`
- `IFileWriter.WriteAllocationMetadata`
- `IFileSynchronizer.Sync`

### Case UT-FM-LC-RESIZE-004 — Invalid size arguments rejection
#### Objective
Verify that unaligned or negative size targets are safely rejected.
#### Requirement References
- BR-LC-RESIZE-001
#### Priority
High
#### Test Category
Boundary
#### Input
- `entry`: active `OpenFileEntry`
- `newSize`: `-500` OR `1044481` (Not extent/page aligned)
#### Execution
Invoke `ResizeFile(entry, newSize)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Resize`

### Case UT-FM-LC-RESIZE-005 — Truncate cuts into used extent
#### Objective
Verify that shrinking is rejected if active allocations exist in the space to be removed.
#### Requirement References
- BR-LC-RESIZE-004
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry` (CurrentSize = 2MB)
- `newSize`: `1048576` (1MB)
#### Preconditions
- At least one allocated extent sits in the upper 1MB boundary segment.
#### Dependency Setup
- `AllocationMetadata.CanTruncateTo` returns `false`.
#### Execution
Invoke `ResizeFile(entry, 1048576)`.
#### Expected Output
Throws `FileTruncationException`.
#### Prohibited Dependency Calls
- `IPhysicalFileSystem.Resize`
- `IFileWriter.WriteAllocationMetadata`

### Case UT-FM-LC-RESIZE-006 — Maximum file size exceeded
#### Objective
Verify that size constraints block runaway file growth.
#### Requirement References
- BR-LC-RESIZE-002
#### Priority
High
#### Test Category
Negative
#### Input
- `entry`: active `OpenFileEntry`
- `newSize`: `20971520` (20MB)
#### Preconditions
- File's `MaximumSize` is capped at 10MB.
#### Execution
Invoke `ResizeFile(entry, 20971520)`.
#### Expected Output
Throws `MaximumFileSizeExceededException`.

### Case UT-FM-LC-RESIZE-007 — Physical OS resize fails
#### Objective
Verify that an OS failure to set the length wraps the error without leaving metadata corrupted.
#### Requirement References
- BR-LC-RESIZE-006
#### Priority
Critical
#### Test Category
Dependency Failure
#### Dependency Setup
- `IPhysicalFileSystem.Resize` throws `IOException`.
#### Execution
Invoke `ResizeFile(entry, 2097152)`.
#### Expected Output
Throws `FileResizeException` (wrapping `IOException`).
#### Expected State
- In-memory metadata remains unmodified and consistent, since physical resize failed before metadata updates occurred.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Resize` | `fileHandle, 2097152` | 1 |
#### Prohibited Dependency Calls
- `IFileWriter.WriteAllocationMetadata`

### Case UT-FM-LC-RESIZE-008 — Metadata write fails after resize
#### Objective
Verify handling of partial failures when physical file resizing succeeds but structural updates to disk fail.
#### Requirement References
- BR-LC-RESIZE-007
#### Priority
High
#### Test Category
Dependency Failure
#### Dependency Setup
- `IPhysicalFileSystem.Resize(fileHandle, 2097152)` succeeds.
- `IFileWriter.WriteAllocationMetadata` throws `IOException`.
#### Execution
Invoke `ResizeFile(entry, 2097152)`.
#### Expected Output
Throws exception (e.g., `FileResizeException`).
#### Expected State
*Pending Design Resolution.*
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `IPhysicalFileSystem` | `Resize` | `fileHandle, 2097152` | 1 |
| `IFileWriter` | `WriteAllocationMetadata` | `fileHandle, header, metadata` | 1 |

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
> Compensation after physical resize succeeds, but a subsequent operation fails (like metadata update) is under-specified. If the physical file is expanded, but writing the updated `AllocationMetadata` fails, it is unclear if the manager must issue a rollback truncation, or simply fail and leave the physical file longer than the metadata tracks (which is functionally harmless for append-only, but still a partial state). Case 8 omits exact state verification until this design decision is formalized.

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
