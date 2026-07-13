# Integration Test Spec - Resize Physical File

## Scenario Description
Verifies that extending and truncating files updates both the operating-system physical file length and persists corrected allocation metadata structures.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. File Extension Verification
1. Create and open `test_resize.db` (1MB initial).
2. Execute physical extension:
   ```csharp
   lifecycleManager.ResizeFile(entry, 2097152); // 2MB
   ```
3. Assertions:
   * Assert physical file size on disk is exactly `2,097,152` bytes.
   * Assert `entry.DataFile.AllocationMetadata.TotalExtentCount == 32`.
   * Assert `entry.DataFile.AllocationMetadata.FreeExtentCount == 32`.

### 2. File Truncation Verification
1. Execute safe truncation back to 1MB:
   ```csharp
   lifecycleManager.ResizeFile(entry, 1048576); // 1MB
   ```
2. Assertions:
   * Assert physical file size on disk is exactly `1,048,576` bytes.
   * Assert `entry.DataFile.AllocationMetadata.TotalExtentCount == 16`.
   * Assert `entry.DataFile.AllocationMetadata.FreeExtentCount == 16`.

### 3. Persisted Metadata Verification
1. Close file and reopen using `OpenFile`.
2. Verify:
   * Reconstructed `DataFile.AllocationMetadata` has `TotalExtentCount == 16` and matches physical size.

---

## Cleanup
* Delete `test_resize.db`.
