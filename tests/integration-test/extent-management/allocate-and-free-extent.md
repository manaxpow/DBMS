# Integration Test Spec - Allocate and Free Extent

## Scenario Description
Verifies allocating and freeing extents updates the persistent bitmap on disk, ensuring allocations remain intact and frees return space across file reopening boundaries.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Allocation Verification
1. Create and open `test_extents.db` (1MB).
2. Allocate an extent:
   ```csharp
   var extent = extentManager.AllocateExtent(entry); // Allocates ExtentId(0)
   ```
3. Close the file:
   ```csharp
   lifecycleManager.CloseFile("test_extents.db");
   ```
4. Reopen the file:
   ```csharp
   var newEntry = lifecycleManager.OpenFile("test_extents.db", ReadWrite, Exclusive);
   ```
5. Verification:
   * Assert `newEntry.DataFile.AllocationMetadata.GetExtentState(ExtentId(0))` is `ExtentState.Allocated`.
   * Assert `newEntry.DataFile.AllocationMetadata.FreeExtentCount == 15`.

### 2. Free Verification
1. Free the allocated extent:
   ```csharp
   extentManager.FreeExtent(newEntry, ExtentId(0));
   ```
2. Close the file:
   ```csharp
   lifecycleManager.CloseFile("test_extents.db");
   ```
3. Reopen the file:
   ```csharp
   var entry3 = lifecycleManager.OpenFile("test_extents.db", ReadWrite, Exclusive);
   ```
4. Verification:
   * Assert `entry3.DataFile.AllocationMetadata.GetExtentState(ExtentId(0))` is `ExtentState.Free`.
   * Assert `entry3.DataFile.AllocationMetadata.FreeExtentCount == 16`.

---

## Cleanup
* Delete `test_extents.db`.
