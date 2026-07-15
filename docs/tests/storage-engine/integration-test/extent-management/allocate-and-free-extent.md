# Integration Test Specification — Allocate and Free Extent

## 1. Document Information

- **Specification ID:** IT-FM-EXT-ALLOC-001
- **Component:** Extent Management
- **Primary operation:** `ExtentManager.AllocateExtent` and `ExtentManager.FreeExtent`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that `AllocateExtent` correctly updates in-memory extent bitmaps and allocation metadata and persists these changes via File I/O. It further verifies that `FreeExtent` accurately reverses these operations, marking the space as available and successfully persisting the updated state.

## 3. Integration Boundary

- **Concrete components under integration:** `ExtentManager`, `FileLifecycleManager`, `FileWriter`, `FileReader`, `FileSynchronizer`, `PhysicalFileSystem`.
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

- File name: Unique generated name.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- `FileLifecycleManager` has been used to create an empty, initialized database file and keep it open.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-EXT-ALLOC-001 | Allocate, Free, and Round-Trip | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-EXT-ALLOC-001 — Allocate, Free, and Round-Trip

#### Objective
Verifies that allocation and freeing modify both the in-memory models and the physical disk structures reliably across open/close cycles.

#### Preconditions
- A unique temporary directory exists.
- The file `test_extent.db` exists in the temp directory and is open.
- The initial `FreeExtentCount` is known (e.g., 16).

#### Input
- `openFileEntry`: The registered active file.

#### Execution
1. **Allocate**:
   - Invoke `ExtentManager.AllocateExtent(openFileEntry)`.
   - Close the file via `FileLifecycleManager.CloseFile` and reopen it.
2. **Free**:
   - Invoke `ExtentManager.FreeExtent(openFileEntry, extent.ExtentId)` using the extent acquired above.
   - Close the file via `FileLifecycleManager.CloseFile` and reopen it.

#### Expected Result
- Methods complete without throwing.

#### Expected Persisted State
- **After Allocate**: The physical file's bitmap has the corresponding bit flipped to `1`, and its persisted `FreeExtentCount` is decremented by 1.
- **After Free**: The physical file's bitmap has the corresponding bit reverted to `0`, and its persisted `FreeExtentCount` is restored.

#### Expected Runtime State
- The rebuilt runtime structures after each reopen perfectly match the expected persisted state.

#### Failure Handling
- If physical writing fails during allocation or free, a `WriteFailureException` or `FileSyncException` bubbles up.
- Exhausting extents throws `NoFreeExtentException`.

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
