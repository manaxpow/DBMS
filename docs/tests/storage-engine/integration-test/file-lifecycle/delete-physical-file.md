# Integration Test Spec - Delete Physical File

## Scenario Description
Verifies that deleting a file physically removes it from disk, blocks concurrent openings, and is prevented if the file is open.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Execution & Blocked Check
1. Create and open `test_delete.db`.
2. Attempt to delete `test_delete.db` while open:
   * Assert calling `lifecycleManager.DeleteFile("test_delete.db")` throws `FileInUseException`.
   * Assert file `test_delete.db` still exists on disk.

### 2. Physical Deletion Check
1. Close `test_delete.db` handle:
   ```csharp
   lifecycleManager.CloseFile("test_delete.db");
   ```
2. Execute deletion:
   ```csharp
   lifecycleManager.DeleteFile("test_delete.db");
   ```
3. Assert file is gone:
   * Assert `File.Exists("test_sandbox/test_delete.db")` is `false`.

### 3. Open Rejection Check
1. Attempt to open `test_delete.db` again:
   * Assert calling `lifecycleManager.OpenFile("test_delete.db", ...)` throws `FileNotFoundException`.

---

## Cleanup
* Clean folder.
