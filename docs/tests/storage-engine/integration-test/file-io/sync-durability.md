# Integration Test Specification — Sync Durability

## 1. Document Information

- **Specification ID:** IT-FM-IO-SYNC-001
- **Component:** File I/O
- **Primary operation:** `FileSynchronizer.Sync`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that `FileSynchronizer.Sync` reliably forces OS file buffers to flush data to the physical storage device, ensuring that recent modifications survive and are durably persisted before the file handle is closed.

## 3. Integration Boundary

- **Concrete components under integration:** `FileSynchronizer`, `FileWriter`, `FileReader`, `PhysicalFileSystem`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Buffer Manager.
- **Test doubles:** None permitted. Uses real physical files and real handles.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- Payload: `new byte[] { 0x11, 0x22 }`
- Write Offset: `8192`
- Buffer size: `2` bytes

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- A physical file has been created and opened, obtaining a valid `FileHandle`.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-IO-SYNC-001 | Sync Flushes Bytes to Disk | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-IO-SYNC-001 — Sync Flushes Bytes to Disk

#### Objective
Verifies that bytes written by `FileWriter` are pushed completely to the underlying OS disk structures when `FileSynchronizer.Sync` is called.

#### Preconditions
- A unique temporary directory exists.
- The file `test_sync.db` exists in the temp directory and is open.

#### Input
- `openFileEntry`: Entry wrapping the handle to `test_sync.db`.
- `payload`: `{ 0x11, 0x22 }`
- `offset`: `8192`

#### Execution
1. Invoke `FileWriter.WriteAtOffset(openFileEntry.Handle, 8192, payload)`.
2. Invoke `FileSynchronizer.Sync(openFileEntry)`.
3. Read the bytes back bypassing active buffers (e.g., close the current handle via `PhysicalFileSystem.Close` and reopen a fresh handle via `PhysicalFileSystem.Open`).
4. Invoke `FileReader.ReadAtOffset` on the new handle at offset `8192` with a `2` byte buffer.

#### Expected Result
- Read, Write, and Sync complete without throwing.

#### Expected Persisted State
- The physical file contains the exact `payload` bytes at offset `8192`.

#### Expected Runtime State
- The reconstructed `buffer` precisely equals `{ 0x11, 0x22 }`.

#### Failure Handling
- Hardware or OS I/O failure during flush throws `FileSyncException` or standard `IOException`.

#### Cleanup
- Close `FileHandle` via `PhysicalFileSystem`.
- Delete the test file.
- Delete the temporary directory.

## 9. Environment Isolation

Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`. For example: `Path.Combine(Path.GetTempPath(), "DBMS.StorageEngine.IntegrationTests", Guid.NewGuid().ToString("N"))`. Tests must not use fixed shared sandbox folders, desktop paths, or `C:\Users\...`. Each test must own its directory and file names.

## 10. Cleanup Strategy

Every document that creates real resources must define cleanup that executes regardless of test success or failure.
- Test-created raw handles must be closed via `PhysicalFileSystem`.
- Test-created files must be deleted.
- Test-created temporary directories must be deleted recursively.

## 11. Specification Gaps

None

## 12. Coverage Review

| Category | Status |
|---|---|
| Happy path | Covered |
| Invalid physical state | Missing |
| Persistence round-trip | Covered |
| Infrastructure failure | Missing |
| Cleanup failure | Not Applicable |
| Platform boundary | Covered |

## 13. Review Checklist

- [x] Tests observable integration behavior.
- [x] Uses real infrastructure inside the boundary.
- [x] Does not inspect private implementation.
- [x] Uses isolated temporary resources.
- [x] Defines cleanup for all owned resources.
- [x] Uses repository-relative links.
- [x] Preserves formal Test Case IDs.
- [x] Contains no invented behavior.
