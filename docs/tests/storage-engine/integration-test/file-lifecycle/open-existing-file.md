# Integration Test Specification — Open Existing File

## 1. Document Information

- **Specification ID:** IT-FM-LC-OPEN-001
- **Component:** File Lifecycle
- **Primary operation:** `FileLifecycleManager.OpenFile`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that `OpenFile` correctly opens a previously created physical database file, validates its format, loads its runtime metadata (like extent allocation sizes), and registers it securely in the open file registry.

## 3. Integration Boundary

- **Concrete components under integration:** `FileLifecycleManager`, `PhysicalFileSystem`, `FileReader`, `FileValidator`, `OpenFileManager`.
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
- Access Mode: `FileAccessMode.ReadWrite`.
- Lock Mode: `FileLockMode.Exclusive`.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- `FileLifecycleManager` has previously created a valid 1MB file (`test_existing.db`) and closed it cleanly.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-LC-OPEN-001 | Open Existing Valid File | Happy path | Ready |

## 8. Test Cases

### IT-FM-LC-OPEN-001 — Open Existing Valid File

#### Objective
Verifies that `OpenFile` correctly opens a previously created database file, reads and validates its structural metadata (header, allocation metadata, bitmap), and registers it successfully.

#### Preconditions
- A unique temporary directory exists.
- The 1MB file `fileName` exists and is properly formatted and closed.

#### Input
- `fileName`: Generated unique path inside the temporary directory.
- `accessMode`: `FileAccessMode.ReadWrite`
- `lockMode`: `FileLockMode.Exclusive`

#### Execution
Invoke `FileLifecycleManager.OpenFile(fileName, FileAccessMode.ReadWrite, FileLockMode.Exclusive)`.

#### Expected Result
- Returns a successfully registered `OpenFileEntry` with an active `FileHandle`.

#### Expected Persisted State
- Unchanged (since opening does not modify the physical file structure unless recovering, which is out of scope).

#### Expected Runtime State
- The file is registered in the open file registry via `OpenFileManager` and `GetOpenFile(fileName)` is not null.
- The entry states are accurately loaded:
  - `entry.DataFile.TotalExtentCapacity` equals `16`.
  - `entry.DataFile.FreeExtentCount` equals `16`.
  - `entry.ReferenceCount` equals `1`.

#### Failure Handling
- Corrupt file throws `InvalidFileFormatException`.
- Non-existent file throws `FileNotFoundException`.
- OS locked file throws `FileOpenException` or `LockConflictException`.

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
