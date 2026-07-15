# Integration Test Specification — Read and Write at Offset

## 1. Document Information

- **Specification ID:** IT-FM-IO-RW-001
- **Component:** File I/O
- **Primary operation:** `FileWriter.WriteAtOffset` and `FileReader.ReadAtOffset`
- **Test type:** Integration Test
- **Status:** Approved
- **Version:** 1.0
- **Design references:** None

## 2. Purpose

Verifies that concrete `FileReader` and `FileWriter` implementations correctly interact with real physical file handles to persist and retrieve arbitrary byte sequences accurately at exact byte offsets on disk.

## 3. Integration Boundary

- **Concrete components under integration:** `FileWriter`, `FileReader`, `PhysicalFileSystem`.
- **Real infrastructure inside the boundary:** Operating-system file system, .NET file APIs.
- **Systems outside the boundary:** Buffer Manager, Extent Manager, File Validator.
- **Test doubles:** None permitted. Uses real physical files and real handles.

## 4. Environment

- **Required runtime:** .NET runtime with full filesystem access.
- **Real infrastructure:** Local OS standard hard drive.
- **Temporary resource strategy:** Every file-system integration test must use a unique temporary directory created under `Path.GetTempPath()`.
- **Platform requirements:** Cross-platform where applicable.
- **Required permissions:** Read/Write access to the temporary directory.

## 5. Test Data

- Payload: `new byte[] { 0xDE, 0xAD, 0xBE, 0xEF }`
- Write Offset: `4096`
- Buffer size: `4` bytes

## 6. Preconditions

- A unique, isolated temporary directory must exist for the test sandbox.
- A valid physical file has been created and opened, obtaining a valid `FileHandle`.

## 7. Test Case Summary

| Test Case ID | Scenario | Category | Status |
|---|---|---|---|
| IT-FM-IO-RW-001 | Write Payload and Read Back | Persistence round-trip | Ready |

## 8. Test Cases

### IT-FM-IO-RW-001 — Write Payload and Read Back

#### Objective
Verifies that bytes written by `FileWriter` to a specific offset can be perfectly reconstructed by `FileReader` reading from that exact offset.

#### Preconditions
- A unique temporary directory exists.
- The file `test_io.db` exists in the temp directory and is open.

#### Input
- `fileHandle`: Open handle to `test_io.db`.
- `payload`: `{ 0xDE, 0xAD, 0xBE, 0xEF }`
- `offset`: `4096`

#### Execution
1. Invoke `FileWriter.WriteAtOffset(fileHandle, 4096, payload)`.
2. (Optional: Invoke `FileSynchronizer.Sync` to force flush if OS buffering obscures disk bounds).
3. Prepare `buffer` of length `4`.
4. Invoke `FileReader.ReadAtOffset(fileHandle, 4096, buffer)`.

#### Expected Result
- Read and Write complete without throwing.

#### Expected Persisted State
- The physical file contains the exact `payload` bytes starting at offset `4096`.

#### Expected Runtime State
- `buffer` precisely equals `{ 0xDE, 0xAD, 0xBE, 0xEF }`.

#### Failure Handling
- Out-of-bounds writes throw `IncompletePageWriteException` or `WriteFailureException`.
- Out-of-bounds reads throw `IncompletePageReadException` or `ReadFailureException`.

#### Cleanup
- Close `FileHandle` via `PhysicalFileSystem`.
- Delete `test_io.db`.
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
