# Integration Test Specification — Auto-Extension Boundary

## 1. Document Information

- **Specification ID:** IT-FM-EXT-AUTO-001
- **Component:** Extent Management
- **Primary operation:** `ExtentManager.AllocateExtent`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that when `AllocateExtent` is called and no free extents exist, the `ExtentManager` gracefully coordinates with `FileLifecycleManager` (or underlying components) to physically resize the file on disk, correctly rebuilds the allocation structures in memory, persists them, and successfully returns a newly available extent.

## 3. Integration Boundary

- **Concrete components under integration:** `ExtentManager`, `FileLifecycleManager`, `PhysicalFileSystem`, `FileWriter`, `FileReader`, `FileSynchronizer`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Buffer Manager.
- **Test doubles:** None permitted. Uses real persistence.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- Initial File Size: `1048576` (1MB).
- Expected Extension Size: `2097152` (2MB).

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- `FileLifecycleManager` has created `test_extend.db` of 1MB and keeps it open.
- All available free extents have been drained manually via a loop of `AllocateExtent` calls until `FreeExtentCount == 0`.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-EXT-AUTO-001 | Auto-Extend When Exhausted | Platform boundary | Ready |

## 8. Test Cases

### IT-FM-EXT-AUTO-001 — Auto-Extend When Exhausted

#### Objective
Verifies that exhausting the physical extent capacity dynamically resizes the file via OS calls and successfully serves the allocation request.

#### Preconditions
- A unique temporary directory exists.
- The file `test_extend.db` is open, and `FreeExtentCount` is strictly `0`.

#### Input
- `openFileEntry`: The registered active file.

#### Execution
1. Invoke `ExtentManager.AllocateExtent(openFileEntry)`.

#### Expected Result
- Returns a valid, non-null `AllocatedExtent`.

#### Expected Persisted State
- The physical file size on disk is larger (e.g., exactly `2097152` bytes).
- **Round-Trip Verification**: Close and reopen the file via `FileLifecycleManager` and assert the new structures match the larger disk footprint.

#### Expected Runtime State
- `openFileEntry.DataFile.TotalExtentCapacity` increased to reflect the new physical size.
- `openFileEntry.DataFile.FreeExtentCount` reflects the newly added block minus the 1 extent just returned.

#### Failure Handling
- If physical disk space is exhausted, throws an OS-level `IOException`.

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
> Does `ExtentManager` directly call `PhysicalFileSystem.Resize`, or does it call `FileLifecycleManager.ResizeFile`? The exact architectural coupling for the auto-extension trigger is unverified. Assuming it triggers a defined Resize interface safely.

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
