# Integration Test Specification — Corruption Detection

## 1. Document Information

- **Specification ID:** IT-FM-VAL-001
- **Component:** File Validation
- **Primary operation:** `FileValidator.Validate` (via `FileLifecycleManager.OpenFile`)
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that the system detects corrupted header structures (such as invalid magic numbers) when a physical file is opened, rejecting the file by throwing the approved exception and refusing to register it in the runtime environment.

## 3. Integration Boundary

- **Concrete components under integration:** `FileValidator`, `FileLifecycleManager`, `PhysicalFileSystem`, `OpenFileManager`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Recovery module (this specification verifies detection only, not repair).
- **Test doubles:** None permitted. Uses real physically corrupted files.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- Valid file name: Unique generated name.
- Corruption payload: `0xFF, 0xFF, 0xFF, 0xFF` written at offset `0`.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- `FileLifecycleManager` has created a valid file and cleanly closed it.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-VAL-001 | Detect Invalid Magic Number on Open | Invalid physical state | Ready |

## 8. Test Cases

### IT-FM-VAL-001 — Detect Invalid Magic Number on Open

#### Objective
Verifies that when the file's primary magic number is corrupted on disk, the system detects it upon opening and cleanly aborts the operation.

#### Preconditions
- A unique temporary directory exists.
- `test_corrupt.db` exists in the temp directory and is a structurally valid database file.

#### Input
- `fileName`: Path to `test_corrupt.db`.

#### Execution
1. Open the file physically via `PhysicalFileSystem.Open(fileName, ReadWrite)`.
2. Overwrite the first 4 bytes with garbage data (`0xFF, 0xFF, 0xFF, 0xFF`) at offset `0` using `FileWriter`.
3. Synchronize and close the physical file.
4. Attempt to open the file via `FileLifecycleManager.OpenFile(fileName, ReadWrite, Exclusive)`.

#### Expected Result
- The `OpenFile` call throws `InvalidFileFormatException`.

#### Expected Persisted State
- The physical file remains corrupted on disk (no automatic recovery or repair).

#### Expected Runtime State
- The file is strictly not registered in `OpenFileManager`.

#### Failure Handling
- If physical corruption writing fails, it throws a standard `IOException`.

#### Cleanup
- Close any auxiliary raw handles via `PhysicalFileSystem`.
- Delete `test_corrupt.db`.
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
| Happy path | Not Applicable |
| Invalid physical state | Covered |
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
