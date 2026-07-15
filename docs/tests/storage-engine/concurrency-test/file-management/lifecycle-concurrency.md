# Concurrency Test Specification — Lifecycle Operations

## 1. Document Information

- **Specification ID:** CT-FM-LC-001
- **Component:** File Management Concurrency
- **Primary operation:** Thread-safe operations across File Lifecycle and Extent Management.
- **Test type:** Concurrency Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies thread-safe operations in `OpenFile`, duplicate registrations, simultaneous closes/deletes, concurrent space allocations, and refCount consistency under heavy thread contention across the full File Management stack.

## 3. Integration Boundary

- **Concrete components under integration:** `FileLifecycleManager`, `OpenFileManager`, `ExtentManager`, `PhysicalFileSystem`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Buffer Manager.
- **Test doubles:** None permitted. Uses real multi-threading and real file interactions.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system concurrency test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- File name: Unique generated name for each test case to avoid cross-test contention.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- Target data files exist on disk before launching the concurrent thread pool.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| CT-FM-LC-001 | Concurrent File Openings & Registrations | Thread Safety | Ready |
| CT-FM-LC-002 | Concurrent Duplicate Exclusive Opens | Lock Safety | Ready |
| CT-FM-LC-003 | Simultaneous Close and Delete | State Safety | Ready |
| CT-FM-LC-004 | Concurrent Space Allocation Contention | Thread Safety | Ready |

## 8. Test Cases

### CT-FM-LC-001 — Concurrent File Openings & Registrations

#### Objective
Verifies `ReferenceCount` consistency and registration thread safety when multiple threads open the exact same file in Shared mode simultaneously.

#### Preconditions
- A unique temporary directory exists.
- The file `test_concurrent.db` is physically created.
- The file is not currently registered.

#### Execution
1. Spawn 10 concurrent threads, each executing: `FileLifecycleManager.OpenFile(fileName, ReadOnly, Shared)`.
2. Wait for all threads to complete using a `CountdownEvent` or `Task.WhenAll`.

#### Expected Result
- All 10 threads complete without throwing exceptions.

#### Expected Runtime State
- `OpenFileManager.GetOpenFile(fileName).ReferenceCount` exactly equals `10`.

#### Expected Persisted State
- Unchanged.

#### Cleanup
- All 10 threads call `CloseFile(fileName)`.
- Assert file is successfully unregistered.
- Delete the file and the temporary directory.

---

### CT-FM-LC-002 — Concurrent Duplicate Exclusive Opens

#### Objective
Verifies that only one thread can successfully open a file in Exclusive mode when multiple threads race for it.

#### Preconditions
- A unique temporary directory exists.
- The file `test_concurrent.db` is physically created.
- The file is not registered.

#### Execution
1. Spawn 5 concurrent threads, each attempting: `FileLifecycleManager.OpenFile(fileName, ReadWrite, Exclusive)`.
2. Wait for all threads to complete.

#### Expected Result
- Exactly **one** thread succeeds and obtains the handle.
- The remaining 4 threads throw `LockConflictException` or `FileAlreadyOpenException`.

#### Expected Runtime State
- `OpenFileManager.GetOpenFile(fileName).ReferenceCount` exactly equals `1`.

#### Expected Persisted State
- Unchanged.

#### Cleanup
- The successful thread calls `CloseFile(fileName)`.
- Assert file is successfully unregistered.
- Delete the file and the temporary directory.

---

### CT-FM-LC-003 — Simultaneous Close and Delete

#### Objective
Verifies safe state transitions when deletion races with a close operation, ensuring no orphaned handles or inconsistent registry states remain.

#### Preconditions
- A unique temporary directory exists.
- The file `test_concurrent.db` is physically created and opened.
- `ReferenceCount == 1`. Thread A holds the active open handle.

#### Execution
*(Scenario A: Delete waits for Close)*
1. Thread B calls `CloseFile(fileName)`.
2. Thread C calls `DeleteFile(fileName)` after Thread B completes.

*(Scenario B: Delete blocked by Open)*
1. Thread C calls `DeleteFile(fileName)` while Thread A still holds the handle.

#### Expected Result
- **Scenario A:** Both `CloseFile` and `DeleteFile` succeed. The file is unregistered and physically removed.
- **Scenario B:** `DeleteFile` throws `FileInUseException`. `CloseFile` later succeeds.

#### Cleanup
- Delete the file if Scenario B ran. Delete the temporary directory.

---

### CT-FM-LC-004 — Concurrent Space Allocation Contention

#### Objective
Verifies that allocating space concurrently from the same extent bitmap correctly serves one thread and correctly rejects/blocks the other without corrupting the bitmap.

#### Preconditions
- A unique temporary directory exists.
- The file is open and registered.
- `FreeExtentCount == 1`. Auto-extend is disabled for this test.

#### Execution
1. Thread A and Thread B concurrently call: `ExtentManager.AllocateExtent(openFileEntry)`.
2. Wait for both to complete.

#### Expected Result
- Exactly one thread successfully obtains an `AllocatedExtent`.
- The other thread throws `NoFreeExtentException`.

#### Expected Runtime State
- `FreeExtentCount == 0`.
- Bitmap values remain perfectly consistent (no double allocation).

#### Cleanup
- Close the file.
- Delete the file and the temporary directory.

## 9. Environment Isolation

Every concurrency test must use a unique temporary directory created under `Path.GetTempPath()`. Each test case should ideally own a completely distinct file name to avoid test runner collision (e.g., `CT-FM-LC-001.db`).

## 10. Cleanup Strategy

Every document that creates real resources must define cleanup that executes regardless of test success or failure.
- Ensure all pooled threads have completed before cleanup executes.
- Lifecycle-managed handles must be forcefully closed.
- Test-created files and directories must be forcefully deleted.

## 11. Specification Gaps

None

## 12. Coverage Review

| Category | Status |
|---|---|
| Happy path | Covered |
| Concurrency boundaries | Covered |
| Invalid physical state | Not Applicable |
| Thread Safety | Covered |
| Infrastructure failure | Not Applicable |

## 13. Review Checklist

- [x] Tests observable integration behavior.
- [x] Uses real infrastructure inside the boundary.
- [x] Does not inspect private implementation.
- [x] Uses isolated temporary resources.
- [x] Defines cleanup for all owned resources.
- [x] Uses repository-relative links.
- [x] Preserves formal Test Case IDs.
- [x] Contains no invented behavior.
