# Unit Test Spec - ExtentManager

## Component
* **Class**: `ExtentManager`
* **Interface**: `IExtentManager`

---

## Test Cases

### Case 1: Allocate Empty Extent Success (Happy Path)
* **Input**: `entry = openFileEntry`
* **Preconditions**: Free extents are available.
* **Expected Output**: Returns valid `AllocatedExtent`.
* **Dependency Calls**:
  * `allocationMetadata.FindFreeExtent()` -> returns `ExtentId(5)`
  * `allocationMetadata.MarkExtentAllocated(ExtentId(5))` -> success
  * `FileWriter.WriteAllocationMetadata(...)` -> success

### Case 2: Allocate triggering Auto-Extend
* **Preconditions**: `FreeExtentCount = 0`, `AutoExtendEnabled = true`.
* **Execution**: `AllocateExtent(openFileEntry)`
* **Expected Output**: Returns a new `AllocatedExtent`.
* **Dependency Calls**:
  * `allocationMetadata.FindFreeExtent()` -> returns `null`
  * `FileLifecycleManager.ResizeFile(...)` -> success
  * `allocationMetadata.AddExtents(1)` -> success
  * `FileWriter.WriteAllocationMetadata(...)` -> success

### Case 3: Allocate triggering Auto-Extend but Disabled
* **Preconditions**: `FreeExtentCount = 0`, `AutoExtendEnabled = false`.
* **Execution**: `AllocateExtent(openFileEntry)`
* **Expected Output**: Throws `NoFreeExtentException`.

### Case 4: Maximum File Size Reached Limit
* **Preconditions**: `FreeExtentCount = 0`, `AutoExtendEnabled = true`. Physical size matches `MaximumSize`.
* **Execution**: `AllocateExtent(openFileEntry)`
* **Expected Output**: Throws `MaximumFileSizeExceededException`.

### Case 5: Resize File Failure during Extension
* **Preconditions**: File needs to auto-extend, but `ResizeFile` throws `FileResizeException`.
* **Execution**: `AllocateExtent(openFileEntry)`
* **Expected Output**: Throws `ExtentAllocationException` (wrapping `FileResizeException`).

### Case 6: Call MarkExtentAllocated Correctly
* **Description**: Verifies that allocation logic invokes `AllocationMetadata.MarkExtentAllocated` with the exact found extent ID.
* **Dependency Calls**: Verify `MarkExtentAllocated(foundExtentId)` is called.

### Case 7: Persist Metadata Failed during Allocation
* **Preconditions**: Bitmap updated, but `FileWriter.WriteAllocationMetadata` throws `IOException`.
* **Execution**: `AllocateExtent(openFileEntry)`
* **Expected Output**: Throws `ExtentAllocationException`.

### Case 8: Rollback Allocation State when Persist Fails
* **Description**: Verifies that the allocated extent is rolled back to free in-memory if disk synchronization fails.
* **Dependency Calls**:
  * `FileWriter.WriteAllocationMetadata(...)` -> throws `IOException`
  * Verify `AllocationMetadata.MarkExtentFree(allocatedExtentId)` is called during catch blocks.

### Case 9: Free Extent Successfully (Happy Path)
* **Input**: `entry = openFileEntry`, `extentId = ExtentId(3)`
* **Preconditions**: Extent `3` is currently marked used/allocated.
* **Execution**: `FreeExtent(openFileEntry, ExtentId(3))`
* **Expected Output**: Void return (Success).
* **Dependency Calls**:
  * `allocationMetadata.MarkExtentFree(ExtentId(3))` -> success
  * `FileWriter.WriteAllocationMetadata(...)` -> success

### Case 10: Free Non-Existent Extent
* **Input**: `extentId = ExtentId(99)`
* **Preconditions**: File total extent count is 16.
* **Execution**: `FreeExtent(openFileEntry, ExtentId(99))`
* **Expected Output**: Throws `InvalidExtentException`.

### Case 11: Free Already Free Extent Rejection
* **Preconditions**: Extent `3` state is `Free`.
* **Execution**: `FreeExtent(openFileEntry, ExtentId(3))`
* **Expected Output**: Throws `ExtentAlreadyFreeException`.

### Case 12: Blocked by Active Extent Usage
* **Description**: Verifies that if a higher-level usage tracker flags that the extent is still locked by transactions, release fails.
* **Preconditions**: Extent is flagged as active by `ExtentUsageTracker`.
* **Expected Output**: Throws `ExtentInUseException` (if usage tracker checks are implemented).

### Case 13: Rollback Free State when Persist Fails
* **Description**: Verifies that the extent is marked used again in-memory if writing metadata fails during deallocation.
* **Dependency Calls**:
  * `FileWriter.WriteAllocationMetadata(...)` -> throws `IOException`
  * Verify `AllocationMetadata.MarkExtentAllocated(extentId)` is called in catch blocks.
 stream handle.
