# Integration Test Spec - Close and Reopen File

## Scenario Description
Verifies that writes executed before closure are durable, readable upon reopening, and that the original operating-system file handle is cleanly closed.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Execution
1. Create and open `test_durability.db`.
2. Allocate an extent:
   ```csharp
   var extent = extentManager.AllocateExtent(entry); // returns Offset=65536
   ```
3. Write test pattern (`0x55` fill pattern, length 4096) to page offset:
   ```csharp
   fileWriter.WriteAtOffset(entry, 65536, writeBuffer);
   ```
4. Close the file entry:
   ```csharp
   var oldHandle = entry.Handle;
   lifecycleManager.CloseFile("test_durability.db");
   ```
5. Reopen the file:
   ```csharp
   var newEntry = lifecycleManager.OpenFile("test_durability.db", ReadWrite, Exclusive);
   ```
6. Read the page offset:
   ```csharp
   fileReader.ReadAtOffset(newEntry, 65536, readBuffer);
   ```

### 2. Assertions
1. Verify data durability:
   * Assert read buffer contains exactly `0x55` fill pattern.
2. Verify old handle is closed:
   * Attempting to write or read via `oldHandle` throws `ObjectDisposedException` or OS error.

---

## Cleanup
* Close active entries.
* Delete `test_durability.db`.
