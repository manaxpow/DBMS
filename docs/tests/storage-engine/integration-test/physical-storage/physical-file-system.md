# Integration Test Specification — Physical File System

## 1. Document Information

- **Specification ID:** IT-FM-PS-001
- **Component:** Physical Storage
- **Primary operation:** `PhysicalFileSystem` adapter methods (Create, Open, GetSize, Resize, Delete)
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that the system correctly interacts with the underlying operating system file system, including creating files, reading sizes, resizing, deleting, and enforcing OS-level sharing locks.

## 3. Integration Boundary

- **Concrete components under integration:** `PhysicalFileSystem`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Higher-level `FileLifecycleManager`, caching layer.
- **Test doubles:** None permitted for physical file interactions.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform (Windows, Linux) semantics where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- Explicit deterministic sizes: `1048576` (1MB), `4096` bytes.
- Randomly generated unique file names.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-PS-001 | Create New Physical File | Persistence round-trip | Ready |
| IT-FM-PS-002 | Open File with Exclusive Sharing Lock | Platform boundary | Ready |
| IT-FM-PS-003 | Get Physical File Size | Happy path | Ready |
| IT-FM-PS-004 | Resize Physical File | Persistence round-trip | Ready |
| IT-FM-PS-005 | Delete Physical File | Cleanup | Ready |

## 8. Test Cases

### IT-FM-PS-001 — Create New Physical File

#### Objective
Verifies that a new file can be created on the disk with the specified initial physical size.

#### Preconditions
- A unique temporary directory exists.

#### Input
- `fileName`: Generated unique path inside the temporary directory.
- `initialFileSize = 1048576`.

#### Execution
Invoke `PhysicalFileSystem.Create(fileName, 1048576)`.

#### Expected Result
- Returns a valid, active `FileHandle`.

#### Expected Persisted State
- A real physical file is created at the target path.
- The physical file size on disk is exactly `1048576` bytes.

#### Expected Runtime State
- The `FileHandle` wraps the active OS file descriptor.

#### Failure Handling
- Creation failure (e.g., lack of space) bubbles up standard `IOException`.

#### Cleanup
- Close the `FileHandle` via `PhysicalFileSystem`.
- Delete the created file via `PhysicalFileSystem`.

### IT-FM-PS-002 — Open File with Exclusive Sharing Lock

#### Objective
Verifies that `Open` uses the approved `FileShare` policy and correctly blocks or allows subsequent access attempts.

#### Preconditions
- A physical file exists in the temporary sandbox.

#### Input
- `fileName`: Path to the existing file.
- `FileAccessMode.ReadWrite`.

#### Execution
1. Thread A calls `PhysicalFileSystem.Open(fileName, FileAccessMode.ReadWrite)`.
2. Thread B attempts to delete or open the same file (dependent on OS and sharing policy).

#### Expected Result
- Thread A receives a valid `FileHandle`.
- Thread B's attempt correctly fails or is blocked based on platform sharing policy (e.g., throwing `IOException` on Windows).

#### Expected Persisted State
- No structural changes to the file.

#### Expected Runtime State
- Exclusive lock is held at the OS level by Thread A.

#### Failure Handling
- Access denial throws OS-level `IOException` or `UnauthorizedAccessException`.

#### Cleanup
- Thread A closes the `FileHandle`.
- Delete the test file.

### IT-FM-PS-003 — Get Physical File Size

#### Objective
Verifies that the `GetSize` method returns the correct physical size from the OS.

#### Preconditions
- A physical file of size `4096` exists.
- The file is successfully opened, returning a `FileHandle`.

#### Input
- `FileHandle` wrapping the open file.

#### Execution
Invoke `PhysicalFileSystem.GetSize(fileHandle)`.

#### Expected Result
- Returns `4096`.

#### Expected Persisted State
- Unchanged.

#### Expected Runtime State
- Unchanged.

#### Failure Handling
- If handle is invalid, throws `ObjectDisposedException` or OS equivalent.

#### Cleanup
- Close the `FileHandle`.
- Delete the file.

### IT-FM-PS-004 — Resize Physical File

#### Objective
Verifies that `Resize` interacts with the OS to accurately truncate or extend the file.

#### Preconditions
- A physical file of size `4096` exists and is open.

#### Input
- `FileHandle` wrapping the file.
- New size: `8192`.

#### Execution
Invoke `PhysicalFileSystem.Resize(fileHandle, 8192)`.

#### Expected Result
- Method completes without throwing.

#### Expected Persisted State
- The OS reports the physical file size is exactly `8192` bytes.

#### Expected Runtime State
- Unchanged (handle remains valid).

#### Failure Handling
- Resize failure bubbling up `IOException`.

#### Cleanup
- Close the `FileHandle`.
- Delete the file.

### IT-FM-PS-005 — Delete Physical File

#### Objective
Verifies that a file that is not locked can be cleanly removed from the OS.

#### Preconditions
- A physical file exists and is closed (no active handles).

#### Input
- `fileName`: Path to the closed file.

#### Execution
Invoke `PhysicalFileSystem.Delete(fileName)`.

#### Expected Result
- Method completes without throwing.

#### Expected Persisted State
- `Exists(fileName)` returns `false`.
- The physical file is removed from the temporary directory.

#### Expected Runtime State
- None.

#### Failure Handling
- If file is locked, throws `IOException`.

#### Cleanup
- File is already deleted. Delete the temporary directory.

## 9. Environment Isolation

Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`. For example: `Path.Combine(Path.GetTempPath(), "DBMS.StorageEngine.IntegrationTests", Guid.NewGuid().ToString("N"))`. Tests must not use fixed shared sandbox folders, desktop paths, or `C:\Users\...`. Each test must own its directory and file names.

## 10. Cleanup Strategy

Every document that creates real resources must define cleanup that executes regardless of test success or failure (e.g., within `finally` blocks or `IDisposable`). 
- Test-created raw handles must be closed via `PhysicalFileSystem`.
- Test-created files must be deleted.
- Test-created temporary directories must be deleted recursively.

## 11. Specification Gaps

None

## 12. Coverage Review

| Category | Status |
|---|---|
| Happy path | Covered |
| Invalid physical state | Covered |
| Persistence round-trip | Covered |
| Infrastructure failure | Covered |
| Cleanup failure | Covered |
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
