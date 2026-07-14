# Integration Test Spec - Auto-Extension Boundary

## Scenario Description
Verifies that when a file is out of free extents, requesting allocations dynamically resizes the file and adds new extents at correct physical offsets.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`
* **Configuration**: File initialized with exactly `1` extent (65536 bytes) with `AutoExtendEnabled = true`.

---

## Test Scenario Steps

### 1. Allocation Exhaustion
1. Create and open `test_auto.db` (64KB size).
2. Allocate the first extent:
   ```csharp
   var first = extentManager.AllocateExtent(entry); // returns ExtentId(0)
   ```
3. Assertions:
   * Assert `entry.DataFile.AllocationMetadata.FreeExtentCount == 0`.

### 2. Auto-Extension Execution
1. Allocate another extent (exhausted trigger):
   ```csharp
   var second = extentManager.AllocateExtent(entry);
   ```
2. Assertions:
   * Assert file is extended: `entry.DataFile.AllocationMetadata.TotalExtentCount == 2`.
   * Assert free count: `entry.DataFile.AllocationMetadata.FreeExtentCount == 0`.
   * Assert physical file size on disk is exactly `131,072` bytes (2 extents * 65536).
   * Assert second extent details:
     * `second.ExtentId` is `ExtentId(1)`.
     * `second.DiskAddress` is `65536` (exact start of the second block).

### 3. Verification on Reload
1. Close file and reopen.
2. Assertions:
   * Reconstructed `DataFile.AllocationMetadata.TotalExtentCount` is `2`.
   * Reconstructed `DataFile.AllocationMetadata.FreeExtentCount` is `0`.

---

## Cleanup
* Delete `test_auto.db`.
