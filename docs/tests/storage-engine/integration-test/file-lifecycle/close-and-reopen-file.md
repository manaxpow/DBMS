# Integration Test Specification — Close and Reopen File

## 1. Document Information

- **Specification ID:** IT-FM-LC-CLOSE-001
- **Component:** File Lifecycle
- **Primary operation:** `FileLifecycleManager.CloseFile`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that `CloseFile` synchronizes any pending state, closes physical handles, removes the file from the open file registry, and ensures that the physical file is left in a consistent state so that it can be cleanly reopened.

## 3. Integration Boundary

- **Concrete components under integration:** `FileLifecycleManager`, `PhysicalFileSystem`, `FileSynchronizer`, `OpenFileManager`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** None.
- **Test doubles:** None permitted.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- File name: Unique generated name.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- `FileLifecycleManager` has been used to create a file, returning an active `OpenFileEntry` with a `ReferenceCount == 1`.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-LC-CLOSE-001 | Close and Reopen Success | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-LC-CLOSE-001 — Close and Reopen Success

#### Objective
Verifies that closing a file correctly flushes and unregisters it, freeing the OS-level lock so that it can be cleanly reopened.

#### Preconditions
- A unique temporary directory exists.
- `fileName` is an active, open file managed by `FileLifecycleManager`.

#### Input
- `fileName`: Generated unique path inside the temporary directory.

#### Execution
1. Invoke `FileLifecycleManager.CloseFile(fileName)`.
2. Invoke `FileLifecycleManager.OpenFile(fileName, FileAccessMode.ReadWrite, FileLockMode.Exclusive)`.

#### Expected Result
- Close and Open both complete successfully without throwing.
- Returns a new valid `OpenFileEntry` upon reopening.

#### Expected Persisted State
- The physical file remains on disk with valid structures.

#### Expected Runtime State
- After Close: `OpenFileManager.GetOpenFile(fileName)` returns `null`.
- After Reopen: The file is re-registered and `entry.ReferenceCount == 1`.

#### Failure Handling
- If `CloseFile` fails during sync, an exception bubbles up and state recovery is dictated by the caller.

#### Cleanup
- Close the `OpenFileEntry` via `FileLifecycleManager.CloseFile`.
- Delete the created file via `PhysicalFileSystem.Delete`.
- Delete the temporary directory.

## 9. Environment Isolation

Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`. For example: `Path.Combine(Path.GetTempPath(), "DBMS.StorageEngine.IntegrationTests", Guid.NewGuid().ToString("N"))`. Tests must not use fixed shared sandbox folders, desktop paths, or `C:\Users\...`. Each test must own its directory and file names.

## 10. Cleanup Strategy

Every document that creates real resources must define cleanup that executes regardless of test success or failure.
- Lifecycle-managed handles must be closed via `FileLifecycleManager`.
- Test-created files must be deleted.
- Test-created temporary directories must be deleted recursively.

## 11. Specification Gaps

None

## 12. Coverage Review

| Category | Status |
|---|---|
| Happy path | Covered |
| Invalid physical state | Not Applicable |
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
