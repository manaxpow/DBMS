# Integration Test Spec - Sync and Persistence

## Scenario Description
Verifies that calling `Sync` on the file entry pushes all in-memory OS and hardware cache segments to disk, ensuring durability before reopenings.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Execution Flow
1. Create and open `test_sync.db`.
2. Allocate extent and write test buffer filled with `0xCC` pattern (offset = 65536, length = 4096).
3. Force flush changes to disk:
   ```csharp
   fileSynchronizer.Sync(entry);
   ```
4. Close file descriptor entry.
5. Reopen the file:
   ```csharp
   var newEntry = lifecycleManager.OpenFile("test_sync.db", ReadOnly, Shared);
   ```
6. Read the page offset:
   ```csharp
   fileReader.ReadAtOffset(newEntry, 65536, readBuffer);
   ```

### 2. Assertions
1. Verify data durability:
   * Assert read buffer contains exactly `0xCC` pattern.
   * Verify that disk metadata is fully readable.

---

## Cleanup
* Delete `test_sync.db`.
