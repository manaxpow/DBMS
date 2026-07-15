# Integration Test Spec - Lifecycle Concurrency

## Scenario Description
Verifies thread-safe operations in `OpenFile`, duplicate registrations, simultaneous closes/deletes, concurrent space allocations, and refCount consistency under heavy thread contention.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Concurrent File Openings & Registrations

#### Initial State
* `test_concurrent.db` has been created on disk.
* File is not currently registered in the open file registry.

#### Steps
1. Spawn 10 concurrent threads, each executing:
   ```csharp
   lifecycleManager.OpenFile("test_concurrent.db", FileAccessMode.ReadOnly, FileLockMode.Shared);
   ```
2. Wait for all threads to complete.

#### Assertions
* All 10 threads complete without throwing.
* `GetOpenFile("test_concurrent.db").ReferenceCount` equals exactly `10` (no race conditions on refCount incrementing).

#### Cleanup
* All 10 threads call `CloseFile("test_concurrent.db")` sequentially.
* Assert file is unregistered after the last close.

---

### 2. Concurrent Duplicate Exclusive Opens

#### Initial State
* `test_concurrent.db` is on disk.
* File is not registered in the open file registry.

#### Steps
1. Spawn 5 concurrent threads, each attempting:
   ```csharp
   lifecycleManager.OpenFile("test_concurrent.db", FileAccessMode.ReadWrite, FileLockMode.Exclusive);
   ```
2. Wait for all threads to complete.

#### Assertions
* Exactly **one** thread succeeds and obtains the handle.
* The remaining 4 threads throw `LockConflictException` or `FileAlreadyOpenException`.
* `GetOpenFile("test_concurrent.db").ReferenceCount` equals `1`.

#### Cleanup
* The successful thread calls `CloseFile("test_concurrent.db")`.
* Assert file is unregistered.

---

### 3a. Simultaneous Close and Delete — Close Wins

#### Initial State
* `test_concurrent.db` is registered in the open file registry.
* `ReferenceCount == 1`.
* Thread A holds the active open handle.

#### Steps
1. Thread B calls `CloseFile("test_concurrent.db")`.
2. Thread C calls `DeleteFile("test_concurrent.db")` **after Thread B's close completes** (using a barrier or completion signal).

#### Assertions
* Thread B's `CloseFile` returns void (success).
* Thread C's `DeleteFile` returns void (success).
* File no longer exists on disk: `File.Exists("test_sandbox/test_concurrent.db")` is `false`.
* Registry returns `null` for `GetOpenFile("test_concurrent.db")`.

#### Cleanup
* No additional cleanup needed — file was deleted.

---

### 3b. Simultaneous Close and Delete — Delete Blocked by Open Handle

#### Initial State
* `test_concurrent.db` is registered in the open file registry.
* `ReferenceCount == 1`.
* Thread A holds the active open handle.

#### Steps
1. Thread C calls `DeleteFile("test_concurrent.db")` while Thread A still holds the open handle.
2. Assert deletion is blocked.
3. Thread A calls `CloseFile("test_concurrent.db")`.

#### Assertions
* Thread C's `DeleteFile` throws `FileInUseException`.
* Thread A's `CloseFile` succeeds.
* File still exists on disk after the failed delete.
* Registry returns `null` for `GetOpenFile("test_concurrent.db")` after close.

#### Cleanup
* Delete `test_concurrent.db` manually.

---

### 4. Concurrent Space Allocation Contention

#### Initial State
* `test_concurrent.db` is open and registered.
* `AllocationMetadata.FreeExtentCount == 1`.
* Auto-extend is **disabled**.

#### Steps
1. Thread A and Thread B concurrently call:
   ```csharp
   extentManager.AllocateExtent(entry);
   ```
2. Wait for both threads to complete.

#### Assertions
* Exactly one thread obtains the free extent (returns a valid `AllocatedExtent`).
* The other thread throws `NoFreeExtentException`.
* `FreeExtentCount == 0` and bitmap values remain perfectly consistent (no double allocation).

#### Cleanup
* Close all open entries.
* Delete `test_concurrent.db`.

---

## Final Cleanup
* Close any remaining open entries.
* Delete `test_concurrent.db` if it still exists.
