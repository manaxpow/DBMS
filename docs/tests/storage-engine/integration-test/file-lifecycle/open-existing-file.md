# Integration Test Spec - Open Existing File

## Scenario Description
Verifies that opening an existing file successfully parses the persistent disk headers and fully reconstructs the in-memory representation.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Execution
1. Create a valid file on disk:
   ```csharp
   lifecycleManager.CreateFile("test_open.db", FileType.Data, 4096, 1048576);
   ```
2. Open and load the entry:
   ```csharp
   var entry = lifecycleManager.OpenFile("test_open.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
   ```

### 2. Reconstruction Verification
1. Verify `OpenFileEntry` properties:
   * Assert `entry.ReferenceCount == 1`.
   * Assert `entry.AccessMode == FileAccessMode.ReadWrite`.
   * Assert `entry.LockMode == FileLockMode.Exclusive`.
2. Verify `DataFile` reconstruction:
   * Assert `entry.DataFile.FileName` is `"test_open.db"`.
   * Assert `entry.DataFile.Header.MagicNumber` matches signature.
   * Assert `entry.DataFile.AllocationMetadata.TotalExtentCount == 16`.
   * Assert `entry.DataFile.AllocationMetadata.FreeExtentCount == 16`.

### 3. Close & Registration Check
1. Call `CloseFile("test_open.db")`.
2. Assert that lookup returns null:
   * `openFileManager.GetOpenFile("test_open.db")` -> returns `null`

---

## Cleanup
* Delete `test_open.db`.
