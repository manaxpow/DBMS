# Unit Test Specification — `ExtentBitmap`

## 1. Document Information
- **Specification ID:** UT-FM-EXT-BITMAP
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.ExtentManagement`
- **Class:** `ExtentBitmap`
- **Interface:** N/A (Domain Value Object)
- **Public Methods:**
  - `static ExtentBitmap Create(int totalExtents)`
  - `int? FindFirstFree()`
  - `bool IsAllocated(int index)`
  - `void MarkUsed(int index)`
  - `void MarkFree(int index)`
  - `void AppendFreeExtents(int count)`
  - `void Truncate(int newTotalExtents)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Encapsulates an in-memory bit array representing the allocation state of extents. Provides safe bitwise operations, capacity resizing, and allocation scans.

## 3. Unit Under Test
- **Concrete class:** `ExtentBitmap`
- **Public methods:** (Listed above)
- **Output:** Depends on method.
- **Observable state:** Internal byte array holding bit flags is updated.
- **Dependencies:** None.

## 4. Dependencies
None. `ExtentBitmap` is tested as a pure in-memory utility representation.

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-EXT-BITMAP-001 | A new bitmap must have all bits initialized to zero (free). |
| BR-EXT-BITMAP-002 | `MarkUsed` must transition a bit from 0 to 1. Double-used throws `InvalidOperationException`. |
| BR-EXT-BITMAP-003 | `MarkFree` must transition a bit from 1 to 0. Double-free throws `InvalidOperationException`. |
| BR-EXT-BITMAP-004 | Indices must be `>= 0` and `< TotalExtents`. Out of bounds throws `ArgumentOutOfRangeException`. |
| BR-EXT-BITMAP-005 | Appending capacity must initialize new bits to 0. |
| BR-EXT-BITMAP-006 | Truncation is only allowed if all bits in the removed region are 0. Otherwise, throws `InvalidOperationException`. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-EXT-BITMAP-001 | Create bitmap | Positive | Critical |
| UT-FM-EXT-BITMAP-002 | Scan first free | Positive | High |
| UT-FM-EXT-BITMAP-003 | Mark used | Positive | Critical |
| UT-FM-EXT-BITMAP-004 | Mark free | Positive | Critical |
| UT-FM-EXT-BITMAP-005 | Duplicate mark used | Negative | High |
| UT-FM-EXT-BITMAP-006 | Duplicate mark free | Negative | High |
| UT-FM-EXT-BITMAP-007 | Negative index | Negative | High |
| UT-FM-EXT-BITMAP-008 | Index exceeds capacity | Negative | High |
| UT-FM-EXT-BITMAP-009 | Append capacity | Positive | High |
| UT-FM-EXT-BITMAP-010 | Truncate size | Positive | High |
| UT-FM-EXT-BITMAP-011 | Truncate allocated segments | Negative | High |
| UT-FM-EXT-BITMAP-012 | Check byte boundaries math | Positive | Critical |

## 7. Test Cases

### Case UT-FM-EXT-BITMAP-001 — Create bitmap
#### Objective
Verify that a newly instantiated bitmap has all bits initialized as free.
#### Requirement References
- BR-EXT-BITMAP-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `totalExtents = 16`
#### Execution
Invoke `ExtentBitmap.Create(16)`.
#### Expected Output
Returns a valid `ExtentBitmap` reference.
#### Expected State
All 16 bits are set to free (0).

### Case UT-FM-EXT-BITMAP-002 — Scan first free
#### Objective
Verify that `FindFirstFree` returns the lowest index with a zero bit.
#### Priority
High
#### Test Category
Positive
#### Preconditions
- Bitmap bits are `1101` (binary indices: 0, 1, 3 are used; index 2 is free).
#### Execution
Invoke `FindFirstFree()`.
#### Expected Output
Returns `2`.

### Case UT-FM-EXT-BITMAP-003 — Mark used
#### Objective
Verify that an available bit is successfully set to 1.
#### Requirement References
- BR-EXT-BITMAP-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `index = 2`
#### Preconditions
- Bit at index 2 is free (0).
#### Execution
Invoke `MarkUsed(2)`.
#### Expected State
Bit at index 2 transitions to used (1).

### Case UT-FM-EXT-BITMAP-004 — Mark free
#### Objective
Verify that a used bit is successfully set to 0.
#### Requirement References
- BR-EXT-BITMAP-003
#### Priority
Critical
#### Test Category
Positive
#### Input
- `index = 2`
#### Preconditions
- Bit at index 2 is used (1).
#### Execution
Invoke `MarkFree(2)`.
#### Expected State
Bit at index 2 transitions to free (0).

### Case UT-FM-EXT-BITMAP-005 — Duplicate mark used
#### Objective
Verify that double allocation throws an exception.
#### Requirement References
- BR-EXT-BITMAP-002
#### Priority
High
#### Test Category
Negative
#### Input
- `index = 2`
#### Preconditions
- Bit at index 2 is already used (1).
#### Execution
Invoke `MarkUsed(2)`.
#### Expected Output
Throws `InvalidOperationException`.

### Case UT-FM-EXT-BITMAP-006 — Duplicate mark free
#### Objective
Verify that double free throws an exception.
#### Requirement References
- BR-EXT-BITMAP-003
#### Priority
High
#### Test Category
Negative
#### Input
- `index = 2`
#### Preconditions
- Bit at index 2 is already free (0).
#### Execution
Invoke `MarkFree(2)`.
#### Expected Output
Throws `InvalidOperationException`.

### Case UT-FM-EXT-BITMAP-007 — Negative index
#### Objective
Verify that negative indices are rejected.
#### Requirement References
- BR-EXT-BITMAP-004
#### Priority
High
#### Test Category
Negative
#### Input
- `index = -1`
#### Execution
Invoke `MarkUsed(-1)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.

### Case UT-FM-EXT-BITMAP-008 — Index exceeds capacity
#### Objective
Verify that out of bounds indices are rejected.
#### Requirement References
- BR-EXT-BITMAP-004
#### Priority
High
#### Test Category
Negative
#### Input
- `index = 16`
#### Preconditions
- Bitmap capacity is 16.
#### Execution
Invoke `MarkUsed(16)`.
#### Expected Output
Throws `ArgumentOutOfRangeException`.

### Case UT-FM-EXT-BITMAP-009 — Append capacity
#### Objective
Verify that extending the bitmap grows the capacity and initializes new bits to 0.
#### Requirement References
- BR-EXT-BITMAP-005
#### Priority
High
#### Test Category
Positive
#### Input
- `count = 8`
#### Preconditions
- Bitmap size is 16.
#### Execution
Invoke `AppendFreeExtents(8)`.
#### Expected State
- Capacity grows to 24 bits.
- New bits are initialized as free (0).

### Case UT-FM-EXT-BITMAP-010 — Truncate size
#### Objective
Verify that the bitmap can be shrunk if the tail area is free.
#### Requirement References
- BR-EXT-BITMAP-006
#### Priority
High
#### Test Category
Positive
#### Input
- `newTotalExtents = 8`
#### Preconditions
- Bitmap size is 16.
- Lower 8 indices are unmodified.
#### Execution
Invoke `Truncate(8)`.
#### Expected State
Capacity is truncated to 8 bits.

### Case UT-FM-EXT-BITMAP-011 — Truncate allocated segments
#### Objective
Verify that shrinking is rejected if the tail area contains active allocations.
#### Requirement References
- BR-EXT-BITMAP-006
#### Priority
High
#### Test Category
Negative
#### Input
- `newTotalExtents = 8`
#### Preconditions
- Bitmap size is 16.
- Bit at index 12 is used (1).
#### Execution
Invoke `Truncate(8)`.
#### Expected Output
Throws `InvalidOperationException`.

### Case UT-FM-EXT-BITMAP-012 — Check byte boundaries math
#### Objective
Assert that bit shifting logic calculates correctly when indexes span across byte boundaries.
#### Priority
Critical
#### Test Category
Positive
#### Input
- `index = 13`
#### Execution
Invoke `MarkUsed(13)`.
#### Expected State
Second byte of internal bitmap array has matching bit flag set.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
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
