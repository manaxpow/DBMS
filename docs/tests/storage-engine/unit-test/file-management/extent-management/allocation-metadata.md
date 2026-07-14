# Unit Test Specification — `AllocationMetadata`

## 1. Document Information
- **Specification ID:** UT-FM-EXT-META
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.ExtentManagement`
- **Class:** `AllocationMetadata`
- **Interface:** N/A (Domain Value Object)
- **Public Methods:**
  - `ExtentId? FindFreeExtent()`
  - `bool ContainsExtent(ExtentId extentId)`
  - `ExtentState GetExtentState(ExtentId extentId)`
  - `void MarkExtentAllocated(ExtentId extentId)`
  - `void MarkExtentFree(ExtentId extentId)`
  - `void AddExtents(int count)`
  - `bool CanTruncateTo(int newExtentCount)`
  - `void TruncateTo(int newExtentCount)`
  - `FileOffset CalculateFileOffset(ExtentId extentId, FileHeader header)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Encapsulates in-memory metadata describing the state of extent allocations in a file. It safely manipulates the underlying extent bitmap while maintaining synchronized invariant counters (e.g., `FreeExtentCount`, `TotalExtentCount`) and computing physical offsets based on metadata.

## 3. Unit Under Test
- **Concrete class:** `AllocationMetadata`
- **Public methods:** (Listed above)
- **Output:** Depends on method.
- **Observable state:** Modifies internal counters and updates the internally held `ExtentBitmap`.
- **Dependencies:** `ExtentBitmap` (Internal).

## 4. Dependencies
None. `AllocationMetadata` is tested as a pure memory domain object.

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-EXT-META-001 | Free and allocated counters must remain in perfect synchronization with the underlying bitmap state. |
| BR-EXT-META-002 | Double allocation or double free must throw `InvalidOperationException`. |
| BR-EXT-META-003 | Truncation is only permitted if all extents in the removed region are free; otherwise `CanTruncateTo` returns false. |
| BR-EXT-META-004 | File offsets are calculated deterministically based on header constants and extent indexes. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-EXT-META-001 | Scan first free | Positive | Critical |
| UT-FM-EXT-META-002 | Contains extent check | Positive | High |
| UT-FM-EXT-META-003 | Get extent state — free | Positive | High |
| UT-FM-EXT-META-004 | Get extent state — allocated | Positive | High |
| UT-FM-EXT-META-005 | Allocate free extent | Positive | Critical |
| UT-FM-EXT-META-006 | Release allocated extent | Positive | Critical |
| UT-FM-EXT-META-007 | Re-allocate used extent | Negative | High |
| UT-FM-EXT-META-008 | Double free check | Negative | High |
| UT-FM-EXT-META-009 | Add extents capacity | Positive | High |
| UT-FM-EXT-META-010 | Check safe shrink | Positive | High |
| UT-FM-EXT-META-011 | Shrink execution | Positive | High |
| UT-FM-EXT-META-012 | Offset calculation math | Positive | Critical |
| UT-FM-EXT-META-013 | Counter synchronization | Invariant | Critical |

## 7. Test Cases

### Case UT-FM-EXT-META-001 — Scan first free
#### Objective
Verify that `FindFreeExtent` locates and returns the correct index of the first available extent.
#### Priority
Critical
#### Test Category
Positive
#### Preconditions
- Extent bitmap contains free bits. First free index is `2`.
#### Execution
Invoke `FindFreeExtent()`.
#### Expected Output
Returns `ExtentId(2)`.
#### Expected State
No state change.

### Case UT-FM-EXT-META-002 — Contains extent check
#### Objective
Verify that `ContainsExtent` returns true for valid bounds.
#### Priority
High
#### Test Category
Positive
#### Input
- `extentId = ExtentId(15)`
#### Preconditions
- Metadata has `TotalExtentCount == 16`.
#### Execution
Invoke `ContainsExtent(ExtentId(15))`.
#### Expected Output
Returns `true`.
#### Expected State
No state change.

### Case UT-FM-EXT-META-003 — Get extent state — free
#### Objective
Verify that `GetExtentState` correctly identifies a free extent.
#### Priority
High
#### Test Category
Positive
#### Input
- `extentId = ExtentId(2)`
#### Preconditions
- Extent `2` is free.
#### Execution
Invoke `GetExtentState(ExtentId(2))`.
#### Expected Output
Returns `ExtentState.Free`.
#### Expected State
No state change.

### Case UT-FM-EXT-META-004 — Get extent state — allocated
#### Objective
Verify that `GetExtentState` correctly identifies an allocated extent.
#### Priority
High
#### Test Category
Positive
#### Input
- `extentId = ExtentId(2)`
#### Preconditions
- Extent `2` is allocated.
#### Execution
Invoke `GetExtentState(ExtentId(2))`.
#### Expected Output
Returns `ExtentState.Allocated`.
#### Expected State
No state change.

### Case UT-FM-EXT-META-005 — Allocate free extent
#### Objective
Verify that allocating an extent safely marks the bitmap and decrements the free counter.
#### Requirement References
- BR-EXT-META-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `extentId = ExtentId(2)`
#### Preconditions
- Extent `2` is free.
#### Execution
Invoke `MarkExtentAllocated(ExtentId(2))`.
#### Expected State
- Bitmap bit at index `2` is set to used.
- `FreeExtentCount` decrements by exactly 1.

### Case UT-FM-EXT-META-006 — Release allocated extent
#### Objective
Verify that freeing an extent safely clears the bitmap and increments the free counter.
#### Requirement References
- BR-EXT-META-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `extentId = ExtentId(2)`
#### Preconditions
- Extent `2` is allocated.
#### Execution
Invoke `MarkExtentFree(ExtentId(2))`.
#### Expected State
- Bitmap bit at index `2` is set to free.
- `FreeExtentCount` increments by exactly 1.

### Case UT-FM-EXT-META-007 — Re-allocate used extent
#### Objective
Verify that double allocation is rejected.
#### Requirement References
- BR-EXT-META-002
#### Priority
High
#### Test Category
Negative
#### Preconditions
- Extent `2` is allocated.
#### Execution
Invoke `MarkExtentAllocated(ExtentId(2))`.
#### Expected Output
Throws `InvalidOperationException`.
#### Expected State
No state change.

### Case UT-FM-EXT-META-008 — Double free check
#### Objective
Verify that double free is rejected.
#### Requirement References
- BR-EXT-META-002
#### Priority
High
#### Test Category
Negative
#### Preconditions
- Extent `2` is free.
#### Execution
Invoke `MarkExtentFree(ExtentId(2))`.
#### Expected Output
Throws `InvalidOperationException`.
#### Expected State
No state change.

### Case UT-FM-EXT-META-009 — Add extents capacity
#### Objective
Verify that adding capacity grows both the total count and free count.
#### Requirement References
- BR-EXT-META-001
#### Priority
High
#### Test Category
Positive
#### Input
- `count = 8`
#### Preconditions
- Current `TotalExtentCount == 16`.
#### Execution
Invoke `AddExtents(8)`.
#### Expected State
- `TotalExtentCount == 24`.
- `FreeExtentCount` is increased by 8.
- Bitmap size grows.

### Case UT-FM-EXT-META-010 — Check safe shrink
#### Objective
Verify that shrink verification succeeds only when the target truncation area is empty.
#### Requirement References
- BR-EXT-META-003
#### Priority
High
#### Test Category
Positive
#### Input
- `newExtentCount = 8`
#### Preconditions
- All extents between index 8 and 15 are free.
#### Execution
Invoke `CanTruncateTo(8)`.
#### Expected Output
Returns `true`.

### Case UT-FM-EXT-META-011 — Shrink execution
#### Objective
Verify that truncating the extent capacity correctly limits the bitmap.
#### Priority
High
#### Test Category
Positive
#### Input
- `newExtentCount = 8`
#### Preconditions
- Area is safe to truncate.
#### Execution
Invoke `TruncateTo(8)`.
#### Expected State
- `TotalExtentCount == 8`.
- Bitmap size is truncated.

### Case UT-FM-EXT-META-012 — Offset calculation math
#### Objective
Verify that physical file offsets are correctly mapped from extent IDs.
#### Requirement References
- BR-EXT-META-004
#### Priority
Critical
#### Test Category
Positive
#### Input
- `extentId = ExtentId(2)`
- `header`: `PageSize = 4096`, `ExtentSize = 16`
#### Execution
Invoke `CalculateFileOffset(ExtentId(2), header)`.
#### Expected Output
Returns `FileOffset(131072)` (2 * 16 * 4096).

### Case UT-FM-EXT-META-013 — Counter synchronization
#### Objective
Assert that manual bitwise loops counting zero bits in the bitmap yield values exactly equal to `FreeExtentCount`.
#### Requirement References
- BR-EXT-META-001
#### Priority
Critical
#### Test Category
Invariant
#### Preconditions
- Multiple allocs/frees have occurred.
#### Execution
Assert invariant checks.
#### Expected State
- Computed zero bits == `FreeExtentCount`.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior
- [x] Invalid input (Negative testing)
- [x] Boundary conditions
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
