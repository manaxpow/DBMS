# Unit Test Spec - AllocationMetadata

## Component
* **Class**: `AllocationMetadata`

---

## Test Cases

### Case 1: FindFreeExtent
* **Description**: Verifies that the metadata scans the bitmap and returns the first available extent ID.
* **Preconditions**: Extent bitmap has free bits. First free index is `2`.
* **Execution**: `FindFreeExtent()`
* **Expected Output**: Returns `ExtentId(2)`.

### Case 2: ContainsExtent Check
* **Input**: `extentId = ExtentId(15)`
* **Preconditions**: Metadata configured with `TotalExtentCount = 16`.
* **Execution**: `ContainsExtent(ExtentId(15))`
* **Expected Output**: Returns `true`.

### Case 3: GetExtentState
* **Input**: `extentId = ExtentId(2)`
* **Execution**: `GetExtentState(ExtentId(2))`
* **Expected Output**: Returns `ExtentState.Free` or `ExtentState.Allocated` matching the current bitmap value.

### Case 4: MarkExtentAllocated State Change
* **Input**: `extentId = ExtentId(2)`
* **Preconditions**: Extent `2` is free.
* **Execution**: `MarkExtentAllocated(ExtentId(2))`
* **Expected State**: Bit at index `2` is set to used (1). `FreeExtentCount` is decremented by 1.

### Case 5: MarkExtentFree State Change
* **Input**: `extentId = ExtentId(2)`
* **Preconditions**: Extent `2` is allocated.
* **Execution**: `MarkExtentFree(ExtentId(2))`
* **Expected State**: Bit at index `2` is set to free (0). `FreeExtentCount` is incremented.

### Case 6: FreeExtentCount Decrements when Allocating
* **Description**: Assert that allocation decrements the free count by exactly 1.

### Case 7: FreeExtentCount Increments when Freeing
* **Description**: Assert that releasing an extent increases the free count by exactly 1.

### Case 8: Do Not Allocate Used Extent
* **Description**: Verifies that calling `MarkExtentAllocated` on an already allocated extent throws an exception or is blocked.
* **Expected Output**: Throws `InvalidOperationException`.

### Case 9: Do Not Free Already Free Extent
* **Description**: Verifies that calling `MarkExtentFree` on a free extent is blocked.
* **Expected Output**: Throws `InvalidOperationException`.

### Case 10: AddExtents Grow Bounds
* **Input**: `count = 8`
* **Preconditions**: Current `TotalExtentCount = 16`.
* **Execution**: `AddExtents(8)`
* **Expected State**: `TotalExtentCount == 24`, `FreeExtentCount` is increased by 8. Bitmap size grows.

### Case 11: CanTruncateTo Safe Checks
* **Input**: `newExtentCount = 8` (shrinking from 16)
* **Preconditions**: Upper 8 extents are free.
* **Execution**: `CanTruncateTo(8)`
* **Expected Output**: Returns `true`.

### Case 12: TruncateTo Shrink Bounds
* **Input**: `newExtentCount = 8`
* **Execution**: `TruncateTo(8)`
* **Expected State**: `TotalExtentCount == 8`. Bitmap is truncated.

### Case 13: CalculateFileOffset Offset Math
* **Input**: `extentId = ExtentId(2)`, `header` (pageSize = 4096, extentSize = 16 pages -> 65536 bytes)
* **Execution**: `CalculateFileOffset(ExtentId(2), header)`
* **Expected Output**: Returns `FileOffset(131072)` (2 * 65536).

### Case 14: Counters Always Synced with Bitmap
* **Description**: Verifies that any manual loop counting all free bits in the `ExtentBitmap` yields a result exactly equal to `FreeExtentCount`.
