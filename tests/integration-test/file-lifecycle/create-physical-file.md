# Integration Test Spec - Create Physical File

## Scenario Description
Verifies that calling `CreateFile` physically creates a database file on disk with initialized header parameters and empty allocation maps.

---

## Environment Setup
* **Storage Provider**: Local standard hard drive directory.
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Execution
1. Invoke the concrete manager:
   ```csharp
   var dataFile = lifecycleManager.CreateFile("test_physical.db", FileType.Data, 4096, 1048576); // 1MB
   ```

### 2. Physical File Verification
1. Verify file exists on disk:
   * Assert `File.Exists("test_sandbox/test_physical.db")` is `true`.
2. Verify size:
   * Assert physical file length on disk is exactly `1,048,576` bytes.

### 3. Structural Round-Trip Verification
1. Reopen the file in raw mode to read the bytes:
   * Open physical handle using `OpenPhysicalFile("test_physical.db", ReadOnly)`.
   * Read the header block and assert:
     * `MagicNumber` matches the configured DBMS signature.
     * `FormatVersion` matches the active layout version.
     * `PageSize` matches `4096`.
   * Read the allocation metadata block and assert:
     * `TotalExtentCount` matches `16` (1MB / 65536 bytes).
     * `FreeExtentCount` matches `16`.
   * Read the bitmap segment and verify:
     * All bits are set to free (0).

---

## Cleanup
* Close any active handles.
* Delete `test_physical.db` from disk.
