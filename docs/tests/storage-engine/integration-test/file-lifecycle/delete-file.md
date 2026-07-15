# Integration Test Specification — Delete File

## 1. Document Information

- **Specification ID:** IT-FM-LC-DELETE-001
- **Component:** File Lifecycle
- **Primary operation:** `FileLifecycleManager.DeleteFile`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that `DeleteFile` orchestrates the complete removal of the file from both the physical storage and the open file registry.

## 3. Integration Boundary

- **Concrete components under integration:** `FileLifecycleManager`, `PhysicalFileSystem`, `OpenFileManager`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** None.
- **Test doubles:** None permitted.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write/Delete access to the temporary directory.

## 5. Test Data

- File name: Unique generated name.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- A physical file has been created at the target path and is currently closed (no active handles).

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-LC-DELETE-001 | Delete File Success | Happy path | Ready |

## 8. Test Cases

### IT-FM-LC-DELETE-001 — Delete File Success

#### Objective
Verifies that `DeleteFile` successfully removes an existing closed file from physical storage and ensures it is unregistered from the runtime environment.

#### Preconditions
- A unique temporary directory exists.
- The file `fileName` has been created and closed.

#### Input
- `fileName`: Generated unique path inside the temporary directory.

#### Execution
Invoke `FileLifecycleManager.DeleteFile(fileName)`.

#### Expected Result
- Method completes without throwing.

#### Expected Persisted State
- The file does not exist on disk.

#### Expected Runtime State
- The file is no longer registered: `OpenFileManager.GetOpenFile(fileName)` returns `null`.

#### Failure Handling
- If the file is locked, throws `FileInUseException` or `IOException`.

#### Cleanup
- No physical file cleanup needed as the file was deleted.
- Delete the temporary directory.

## 9. Environment Isolation

Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`. For example: `Path.Combine(Path.GetTempPath(), "DBMS.StorageEngine.IntegrationTests", Guid.NewGuid().ToString("N"))`. Tests must not use fixed shared sandbox folders, desktop paths, or `C:\Users\...`. Each test must own its directory and file names.

## 10. Cleanup Strategy

Every document that creates real resources must define cleanup that executes regardless of test success or failure.
- Test-created temporary directories must be deleted recursively.
- If the deletion operation fails during the test, the fallback cleanup must forcibly delete the test file.

## 11. Specification Gaps

None

## 12. Coverage Review

| Category | Status |
|---|---|
| Happy path | Covered |
| Invalid physical state | Not Applicable |
| Persistence round-trip | Not Applicable |
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
