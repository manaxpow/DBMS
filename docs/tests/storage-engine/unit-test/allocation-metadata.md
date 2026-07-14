# Unit Test Specification — `AllocationMetadata`

## 1. Scope

This document covers unit test specifications for `AllocationMetadata`.
Tests are isolated from all external dependencies.
Only public method behavior is verified.

## 2. Component

* **Class:** `AllocationMetadata`
* **Methods:**

```csharp
ExtentId? FindFreeExtent();
bool ContainsExtent(ExtentId extentId);
ExtentState GetExtentState(ExtentId extentId);
void MarkExtentAllocated(ExtentId extentId);
void MarkExtentFree(ExtentId extentId);
void AddExtents(int count);
bool CanTruncateTo(int newExtentCount);
void TruncateTo(int newExtentCount);
FileOffset CalculateFileOffset(ExtentId extentId, FileHeader header);
```

## 3. Unit Under Test

`AllocationMetadata` is responsible for:

* Searching the under-buffered `ExtentBitmap` for available zero bits.
* Verifying whether specific extent index boundaries exist.
* Marking specific extents as allocated or free.
* Synchronously updating in-memory free counts alongside bitmap changes.
* Guarding against double-allocation or double-free errors.
* Increasing total capacity bounds (`AddExtents`).
* Evaluating whether shrink truncation bounds collide with active allocations (`CanTruncateTo`).
* Computing absolute physical file offsets using page and extent parameters.

## 4. Dependencies (None)

None. Tested as a pure memory domain object.

---

# Test Cases

## Case 1: FindFreeExtent

### Preconditions

* Extent bitmap contains free bits. First free index is `2`.

### Execution

```csharp
FindFreeExtent();
```

### Expected Output

* Returns `ExtentId(2)`.

### Suggested Test Name

```csharp
FindFreeExtent_FreeExtentExists_ReturnsFirstFreeExtentId()
```

---

## Case 2: ContainsExtent

### Input

```text
extentId = ExtentId(15)
```

### Preconditions

* Metadata has `TotalExtentCount == 16`.

### Execution

```csharp
ContainsExtent(ExtentId(15));
```

### Expected Output

* Returns `true`.

### Suggested Test Name

```csharp
ContainsExtent_WithinRange_ReturnsTrue()
```

---

## Case 3a: GetExtentState — Free

### Input

```text
extentId = ExtentId(2)
```

### Preconditions

* Extent `2` is currently free (not allocated).

### Execution

```csharp
GetExtentState(ExtentId(2));
```

### Expected Output

* Returns `ExtentState.Free`.

### Suggested Test Name

```csharp
GetExtentState_FreeExtent_ReturnsExtentStateFree()
```

---

## Case 3b: GetExtentState — Allocated

### Input

```text
extentId = ExtentId(2)
```

### Preconditions

* Extent `2` has been previously marked as allocated.

### Execution

```csharp
GetExtentState(ExtentId(2));
```

### Expected Output

* Returns `ExtentState.Allocated`.

### Suggested Test Name

```csharp
GetExtentState_AllocatedExtent_ReturnsExtentStateAllocated()
```

---

## Case 4: MarkExtentAllocated State Change

### Input

```text
extentId = ExtentId(2)
```

### Preconditions

* Extent `2` is free.

### Execution

```csharp
MarkExtentAllocated(ExtentId(2));
```

### Expected State

* Bitmap bit at index `2` is set to used (1).
* `FreeExtentCount` decrements by exactly 1.

### Suggested Test Name

```csharp
MarkExtentAllocated_FreeExtent_SetsAllocatedAndDecrementsFreeCount()
```

---

## Case 5: MarkExtentFree State Change

### Input

```text
extentId = ExtentId(2)
```

### Preconditions

* Extent `2` is allocated.

### Execution

```csharp
MarkExtentFree(ExtentId(2));
```

### Expected State

* Bitmap bit at index `2` is set to free (0).
* `FreeExtentCount` increments by exactly 1.

### Suggested Test Name

```csharp
MarkExtentFree_AllocatedExtent_SetsFreeAndIncrementsFreeCount()
```

---

## Case 6: Do Not Allocate Used Extent

### Preconditions

* Extent `2` is already allocated.

### Execution

```csharp
MarkExtentAllocated(ExtentId(2));
```

### Expected Output

```text
Throws InvalidOperationException
```

### Suggested Test Name

```csharp
MarkExtentAllocated_AlreadyAllocated_ThrowsInvalidOperationException()
```

---

## Case 7: Do Not Free Already Free Extent

### Preconditions

* Extent `2` is already free.

### Execution

```csharp
MarkExtentFree(ExtentId(2));
```

### Expected Output

```text
Throws InvalidOperationException
```

### Suggested Test Name

```csharp
MarkExtentFree_AlreadyFree_ThrowsInvalidOperationException()
```

---

## Case 8: AddExtents Grow Bounds

### Input

```text
count = 8
```

### Preconditions

* Current `TotalExtentCount == 16`.

### Execution

```csharp
AddExtents(8);
```

### Expected State

* `TotalExtentCount == 24`.
* `FreeExtentCount` is increased by 8.
* Bitmap size grows.

### Suggested Test Name

```csharp
AddExtents_ValidCount_IncreasesCapacityAndFreeCount()
```

---

## Case 9: CanTruncateTo Safe Checks

### Input

```text
newExtentCount = 8
```

### Preconditions

* All extents between index 8 and 15 are free.

### Execution

```csharp
CanTruncateTo(8);
```

### Expected Output

* Returns `true`.

### Suggested Test Name

```csharp
CanTruncateTo_SafeShrinkAreaFree_ReturnsTrue()
```

---

## Case 10: TruncateTo Shrink Bounds

### Input

```text
newExtentCount = 8
```

### Execution

```csharp
TruncateTo(8);
```

### Expected State

* `TotalExtentCount == 8`.
* Bitmap size is truncated.

### Suggested Test Name

```csharp
TruncateTo_ValidShrink_TruncatesBitmapAndCapacity()
```

---

## Case 11: CalculateFileOffset Offset Math

### Input

```text
extentId = ExtentId(2)
header   = PageSize: 4096, ExtentSize: 16 pages
```

### Execution

```csharp
CalculateFileOffset(ExtentId(2), header);
```

### Expected Output

* Returns `FileOffset(131072)` (2 * 16 * 4096).

### Suggested Test Name

```csharp
CalculateFileOffset_ValidExtentId_ReturnsCorrectFileOffset()
```

---

## Case 12: Counters Always Synced with Bitmap

### Description

Asserts that manual bitwise loops counting zero bits in the bitmap yield values exactly equal to `FreeExtentCount`.

### Suggested Test Name

```csharp
FreeExtentCount_AlwaysMatchesBitmapZeroBitsCount()
```

---

# Summary

| ID  | Scenario                        | Expected Result                      |
| --: | ------------------------------- | ------------------------------------ |
|   1 | Scan first free                 | Returns correct extent ID index      |
|   2 | Contains extent check           | Returns `true` inside capacity       |
|  3a | Get extent state — free         | Returns `ExtentState.Free`           |
|  3b | Get extent state — allocated    | Returns `ExtentState.Allocated`      |
|   4 | Allocate extent                 | Decrements `FreeExtentCount`         |
|   5 | Release extent                  | Increments `FreeExtentCount`         |
|   6 | Re-allocate used extent         | Throws `InvalidOperationException`   |
|   7 | Double free checks              | Throws `InvalidOperationException`   |
|   8 | Add extents capacity            | Capacity and free count increase     |
|   9 | Check safe shrink               | Returns `true` if region is free     |
|  10 | Shrink execution                | Resizes capacity and truncates bitmap|
|  11 | Offset calculations             | Returns correct physical address math|
|  12 | Counter synchronization check   | Syncs counters accurately            |

## Total

```text
13 independent unit test behaviors
```
