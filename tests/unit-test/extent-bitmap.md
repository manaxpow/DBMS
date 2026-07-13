# Unit Test Spec - ExtentBitmap

## Component
* **Class**: `ExtentBitmap`

---

## Test Cases

### Case 1: Create Bitmap with All Extents Free
* **Input**: `totalExtents = 16`
* **Execution**: `Create(16)`
* **Expected State**: All 16 bits are initialized to free (0).
* **Expected Output**: Returns valid `ExtentBitmap` wrapper.

### Case 2: FindFirstFree Scan
* **Description**: Verifies that the bitmap returns the index of the first zero bit.
* **Preconditions**: Bitmap bits: `1101` (index 2 is free).
* **Execution**: `FindFirstFree()`
* **Expected Output**: Returns `2`.

### Case 3: MarkUsed
* **Input**: `index = 2`
* **Execution**: `MarkUsed(2)`
* **Expected State**: Bit at index 2 is changed from 0 to 1.

### Case 4: MarkFree
* **Input**: `index = 2`
* **Preconditions**: Bit at index 2 is 1 (allocated).
* **Execution**: `MarkFree(2)`
* **Expected State**: Bit at index 2 is changed from 1 to 0.

### Case 5: MarkUsed Twice Safety
* **Input**: `index = 2`
* **Preconditions**: Bit at index 2 is already 1.
* **Execution**: `MarkUsed(2)`
* **Expected Output**: Throws `InvalidOperationException` or overrides cleanly depending on policy (asserts strict exception if double-allocation checks are active).

### Case 6: MarkFree Twice Safety
* **Input**: `index = 2`
* **Preconditions**: Bit at index 2 is already 0.
* **Execution**: `MarkFree(2)`
* **Expected Output**: Throws `InvalidOperationException`.

### Case 7: Negative Index Rejection
* **Input**: `index = -1`
* **Execution**: `MarkUsed(-1)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.

### Case 8: Index Out of Range Rejection
* **Input**: `index = 16` (Bitmap has size 16, valid bounds are 0-15)
* **Execution**: `MarkUsed(16)`
* **Expected Output**: Throws `ArgumentOutOfRangeException`.

### Case 9: AppendFreeExtents Growth
* **Input**: `count = 8`
* **Preconditions**: Bitmap size is 16.
* **Execution**: `AppendFreeExtents(8)`
* **Expected State**: Bitmap size grows to 24 bits. New bits are initialized as free (0).

### Case 10: Truncate Bitmap Size
* **Input**: `newTotalExtents = 8`
* **Preconditions**: Bitmap size is 16.
* **Execution**: `Truncate(8)`
* **Expected State**: Bitmap size is shrunk to 8 bits.

### Case 11: Do Not Truncate Allocated Extents
* **Description**: Verifies that the bitmap blocks truncation if any bit in the removed tail region is set to allocated (1).
* **Preconditions**: Bitmap size is 16. Bit at index 12 is 1 (allocated).
* **Execution**: `Truncate(8)`
* **Expected Output**: Throws `InvalidOperationException`.

### Case 12: Bitmap Spans Multiple Bytes
* **Description**: Verifies bit offsets arithmetic when index spans across byte boundaries (e.g. index 13 sits in the second byte at index 5 of `bitmapBytes[1]`).
* **Execution**:
  * `MarkUsed(13)`
  * Assert `bitmapBytes[1]` matches binary flag pattern (`0x20` or similar).
