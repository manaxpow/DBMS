# Integration Test Specification — Resize File

## 1. Document Information

- **Specification ID:** IT-FM-LC-RESIZE-001
- **Component:** File Lifecycle
- **Primary operation:** `FileLifecycleManager.ResizeFile`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that `ResizeFile` accurately adjusts the physical size of the data file on disk and properly updates internal structures, such as total allocated extents and the free extent count, and persists them via synchronization.

## 3. Integration Boundary

- **Concrete components under integration:** `FileLifecycleManager`, `PhysicalFileSystem`, `FileWriter`, `FileReader`, `FileSynchronizer`, `ExtentManager` (or extent calculation algorithm).
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Buffer Manager.
- **Test doubles:** None permitted.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- Initial File Size: `1048576` (1MB).
- New File Size: `2097152` (2MB).

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- `FileLifecycleManager` has been used to create a file of 1MB and it is currently registered and open in `ReadWrite` mode.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-LC-RESIZE-001 | Extend File Size and Round-Trip Metadata | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-LC-RESIZE-001 — Extend File Size and Round-Trip Metadata

#### Objective
Verifies that extending the file physically updates the OS size and synchronizes the newly expanded allocation metadata structures.

#### Preconditions
- A unique temporary directory exists.
- `fileName` exists and is open with size `1048576`.
- Let's assume initial `TotalExtentCount = 16` and `FreeExtentCount = 16`.

#### Input
- `fileName`: The active registered file.
- `newSize`: `2097152` (2MB).

#### Execution
Invoke `FileLifecycleManager.ResizeFile(fileName, 2097152)`.

#### Expected Result
- Method completes without throwing.

#### Expected Persisted State
- The physical file size on disk is exactly `2,097,152` bytes.
- **Round-Trip Verification**: Close the file and reopen it (or read the raw bytes via `PhysicalFileSystem`):
  - `AllocationMetadata`: `TotalExtentCount` increased to `32` (assuming 65536 byte extents).
  - `FreeExtentCount` reflects the newly available space (previous free + 16 new extents, so 32).

#### Expected Runtime State
- The active `OpenFileEntry` metadata matches the new capacities in memory.

#### Failure Handling
- Physical disk failure throws `FileResizeException`.

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

> Specification Gap — Design clarification required.
> The formula `TotalExtentCount = 32` for 2MB needs explicit validation from the approved domain model to ensure no reserved bytes alter this count. Assumes 1 extent = 64KB.

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
