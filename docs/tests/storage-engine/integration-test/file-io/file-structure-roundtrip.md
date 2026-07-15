# Integration Test Specification — File Structure Round-Trip

## 1. Document Information

- **Specification ID:** IT-FM-IO-STRUCT-001
- **Component:** File I/O
- **Primary operation:** `FileWriter.WriteFileStructures` and `FileReader.ReadFileStructures`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that the `FileWriter` accurately serializes in-memory data structures (Header, Allocation Metadata, Extent Bitmap) to the physical file, and that `FileReader` perfectly reconstructs these exact structures into new in-memory instances directly from the file bytes.

## 3. Integration Boundary

- **Concrete components under integration:** `FileWriter`, `FileReader`, `PhysicalFileSystem`, Domain Models (`FileHeader`, `AllocationMetadata`, `ExtentBitmap`).
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

- Header: Custom `MagicNumber`, Version.
- AllocationMetadata: `TotalExtentCount = 10`, `FreeExtentCount = 5`.
- ExtentBitmap: Alternating bit pattern or specifically crafted array matching the allocated/free count.

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- A physical file of sufficient size to hold all metadata blocks has been created and opened, obtaining a valid `FileHandle`.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-IO-STRUCT-001 | Round-Trip Metadata Serialization | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-IO-STRUCT-001 — Round-Trip Metadata Serialization

#### Objective
Verifies that structures written using `FileWriter.WriteFileStructures` are reliably persisted to the raw file and correctly rehydrated by `FileReader.ReadFileStructures`.

#### Preconditions
- A unique temporary directory exists.
- An empty 1MB file exists and is open.

#### Input
- `openFileEntry`: An entry populated with specific test structures (Header, AllocationMetadata, ExtentBitmap).

#### Execution
1. Invoke `FileWriter.WriteFileStructures(openFileEntry)`.
2. (Optional: Invoke `FileSynchronizer.Sync` to force flush).
3. Create a fresh `newEntry` with the same `FileHandle`.
4. Invoke `FileReader.ReadFileStructures(newEntry)`.

#### Expected Result
- Read and Write complete without throwing.

#### Expected Persisted State
- The physical file contains the serialized representations matching the approved file format rules at their respective offsets.

#### Expected Runtime State
- `newEntry.DataFile.Header` properties exactly match the original.
- `newEntry.DataFile.AllocationMetadata` properties exactly match the original (`TotalExtentCount`, `FreeExtentCount`).
- `newEntry.DataFile.ExtentBitmap` byte array matches exactly using `SequenceEqual`.

#### Failure Handling
- I/O exceptions during read or write bubble up as `ReadFailureException` or `WriteFailureException`.

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
