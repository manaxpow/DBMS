# Integration Test Specification — Create File

## 1. Document Information

- **Specification ID:** IT-FM-LC-CREATE-001
- **Component:** File Lifecycle
- **Primary operation:** `FileLifecycleManager.CreateFile`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that calling `CreateFile` physically creates a database file on disk with initialized header parameters, empty allocation maps, synchronizes to disk, and registers the runtime entry.

## 3. Integration Boundary

- **Concrete components under integration:** `FileLifecycleManager`, `PhysicalFileSystem`, `FileWriter`, `FileReader`, `FileSynchronizer`, `OpenFileManager`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Extent Management algorithms (beyond initial empty maps).
- **Test doubles:** None permitted.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- File name: Unique generated name.
- Parameters: `FileType.Data`, `PageSize = 4096`, `InitialPhysicalSize = 1048576` (1MB).

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- The target file name does not exist.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-LC-CREATE-001 | Create and Round-Trip Initialization | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-LC-CREATE-001 — Create and Round-Trip Initialization

#### Objective
Verifies that `CreateFile` physically creates a database file on disk with initialized header parameters, empty allocation maps, and properly synchronized structural bytes.

#### Preconditions
- A unique temporary directory exists.

#### Input
- `fileName`: Generated unique path inside the temporary directory.
- `fileType`: `FileType.Data`
- `pageSize`: `4096`
- `initialPhysicalSize`: `1048576`

#### Execution
Invoke `FileLifecycleManager.CreateFile(fileName, FileType.Data, 4096, 1048576)`.

#### Expected Result
- Returns a successfully registered `OpenFileEntry` with an active `FileHandle`.

#### Expected Persisted State
- A real physical file is created at the target path.
- The physical file size on disk is exactly `1,048,576` bytes.
- **Round-Trip Verification**: Read the raw bytes using `PhysicalFileSystem.Open(fileName, ReadOnly)` (or via another independent FileReader):
  - `Header`: `MagicNumber` matches the configured DBMS signature, `FormatVersion` matches the active layout version, `PageSize` matches `4096`.
  - `AllocationMetadata`: `TotalExtentCount` matches `16` (derived from 1MB / 65536 bytes block size if approved), `FreeExtentCount` matches `16`.
  - `Bitmap`: All bits are set to free (0).

#### Expected Runtime State
- The new file is registered in the open file registry via `OpenFileManager`.
- The reference count for the entry is `1`.

#### Failure Handling
- If physical creation fails, it throws a `FileCreationException` and no registration occurs.

#### Cleanup
- Close the `OpenFileEntry` via `FileLifecycleManager.CloseFile`.
- Close any auxiliary read-only raw handles via `PhysicalFileSystem`.
- Delete the file and the temporary directory.

## 9. Environment Isolation

Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`. For example: `Path.Combine(Path.GetTempPath(), "DBMS.StorageEngine.IntegrationTests", Guid.NewGuid().ToString("N"))`. Tests must not use fixed shared sandbox folders, desktop paths, or `C:\Users\...`. Each test must own its directory and file names.

## 10. Cleanup Strategy

Every document that creates real resources must define cleanup that executes regardless of test success or failure.
- Lifecycle-managed handles must be closed via `FileLifecycleManager`.
- Test-created raw handles must be closed via `PhysicalFileSystem`.
- Test-created files must be deleted.
- Test-created temporary directories must be deleted recursively.

## 11. Specification Gaps

> Specification Gap — Design clarification required.
> The formula `TotalExtentCount = 16` (1MB / 65536) needs explicit validation from the approved domain model to ensure no reserved bytes alter this count. Currently assuming no reserved extents based on existing document.

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
