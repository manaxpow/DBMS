# Integration Test Spec - Read and Write at Offset

## Scenario Description
Verifies writing bytes to physical disk offsets, reading them back correctly, and validating that multiple page writes do not overwrite adjacent page ranges.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Verification of Basic Read/Write
1. Create and open `test_io.db` (1MB).
2. Prepare write buffer filled with `0xAA` pattern (length 4096).
3. Write to page index 1 (offset = 4096):
   ```csharp
   fileWriter.WriteAtOffset(entry, 4096, writeBuffer);
   ```
4. Read back page index 1:
   ```csharp
   fileReader.ReadAtOffset(entry, 4096, readBuffer);
   ```
5. Assertions:
   * Assert `readBuffer` matches `0xAA` pattern exactly.

### 2. Verification of Multi-Offset Isolation (No Overlap)
1. Prepare write buffer filled with `0xBB` pattern (length 4096).
2. Write to page index 2 (offset = 8192):
   ```csharp
   fileWriter.WriteAtOffset(entry, 8192, writeBuffer);
   ```
3. Read back page index 1 (offset = 4096) and assert it is unmodified:
   * Assert read buffer contains original `0xAA` pattern (no boundary bleeding or overlap).
4. Read back page index 2 (offset = 8192) and assert it contains `0xBB` pattern.

---

## Cleanup
* Delete `test_io.db`.
