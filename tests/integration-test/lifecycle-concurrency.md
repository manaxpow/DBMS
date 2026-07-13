# Integration Test Spec - Lifecycle Concurrency

## Scenario Description
Verifies thread-safe operations in `OpenFile`, duplicate registrations, simultaneous closes/deletes, concurrent space allocations, and refCount consistency under heavy thread contention.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Concurrent File Openings & Registrations
1. Create `test_concurrent.db`.
2. Spawn 10 concurrent threads, each executing:
   ```csharp
   lifecycleManager.OpenFile("test_concurrent.db", FileAccessMode.ReadOnly, FileLockMode.Shared);
   ```
3. Assertions:
   * Verify that all threads complete without throws.
   * Verify that `GetOpenFile("test_concurrent.db").ReferenceCount` is exactly `10` (no race conditions on refCount incrementing).

### 2. Concurrent Duplicate Exclusive Opens
1. Spawn 5 concurrent threads, each attempting:
   ```csharp
   lifecycleManager.OpenFile("test_concurrent.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
   ```
2. Assertions:
   * Assert exactly **one** thread succeeds and obtains the handle.
   * Assert other 4 threads throw `LockConflictException` or `FileAlreadyOpenException`.

### 3. Simultaneous Close and Delete
1. Thread A holds an active open handle to `test_concurrent.db` with `ReferenceCount = 1`.
2. Simultaneously:
   * Thread B calls `CloseFile("test_concurrent.db")`.
   * Thread C calls `DeleteFile("test_concurrent.db")`.
3. Assertions:
   * Assert either:
     * Close finishes first, unregisters the entry, and Delete succeeds.
     * Delete finishes first, fails with `FileInUseException` because Close is still processing, and Close finishes successfully.
   * Verify no race conditions leave the file handle open or state corrupted.

### 4. Concurrent Space Allocation Contention
1. Thread A and Thread B concurrently call `AllocateExtent` on `test_concurrent.db` which has only `1` free extent left.
2. Assertions:
   * Assert exactly one thread obtains the free extent.
   * Assert the other thread triggers auto-extension or throws `NoFreeExtentException` (if auto-extend is disabled).
   * Verify that `FreeExtentCount` and bitmap values remain perfectly consistent.

---

## Cleanup
* Close any open entries.
* Delete `test_concurrent.db`.
