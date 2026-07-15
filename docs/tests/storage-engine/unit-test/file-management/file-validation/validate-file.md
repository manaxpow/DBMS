# Unit Test Specification — `FileValidator.Validate`

## 1. Document Information
- **Specification ID:** UT-FM-VAL-VALIDATE
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileLifecycle` (Wait, actually FileValidator is under FileValidation or similar. Let's use `DBMS.StorageEngine.FileManagement` based on the architecture diagram).
- **Class:** `FileValidator`
- **Interface:** `IFileValidator`
- **Public Method:** `void Validate(FileHeader header, AllocationMetadata metadata, ExtentBitmap bitmap, long physicalFileSize)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Validate File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/validate-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Validates the structure, identity, version, and metadata consistency of a data file to ensure it is safe to be registered as an active open file.

## 3. Unit Under Test
- **Concrete class:** `FileValidator`
- **Public method:** `Validate`
- **Inputs:** `FileHeader header`, `AllocationMetadata metadata`, `ExtentBitmap bitmap`, `long physicalFileSize`
- **Output:** `void` (Success) or throws specific domain exception on failure.
- **Observable state:** None. This is a pure validation routine.
- **Dependencies:** None. Operates purely on provided in-memory structures.

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| None | N/A | N/A |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-VAL-001 | Must validate that the provided file structures are logically consistent with each other and fit within the `physicalFileSize`. |
| BR-VAL-002 | Must throw `InvalidFileFormatException` if magic number or page alignments are invalid. |
| BR-VAL-003 | Must throw `UnsupportedFileVersionException` if the format version is unsupported. |
| BR-VAL-004 | Must throw `CorruptedFileMetadataException` if offsets are out of bounds or extent counters do not match bitmap bit counts. |
| BR-VAL-005 | Must skip legacy header checksum validation to prevent false-positive validation failures on older valid files. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-VAL-VALIDATE-001 | Valid structures | Positive | Critical |
| UT-FM-VAL-VALIDATE-002 | Invalid magic number | Negative | High |
| UT-FM-VAL-VALIDATE-003 | Unsupported version | Negative | High |
| UT-FM-VAL-VALIDATE-004 | Non-power-of-two page size | Negative | High |
| UT-FM-VAL-VALIDATE-005 | Unaligned extent size | Negative | High |
| UT-FM-VAL-VALIDATE-006 | Offset beyond physical size | Negative | High |
| UT-FM-VAL-VALIDATE-007 | Bitmap bitcount mismatch | Negative | High |
| UT-FM-VAL-VALIDATE-008 | Free count exceeds capacity | Negative | High |
| UT-FM-VAL-VALIDATE-009 | Physical size mismatch | Negative | High |
| UT-FM-VAL-VALIDATE-010 | Header checksum deprecated | Alternative | Medium |

## 7. Test Cases

### Case UT-FM-VAL-VALIDATE-001 — Valid structures
#### Objective
Verify that validation succeeds without exception when all file structures and sizes are logically consistent.
#### Requirement References
- BR-VAL-001
- [Validate File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-lifecycle/validate-file.md)
#### Priority
Critical
#### Test Category
Positive
#### Input
- `header`: Valid configuration.
- `metadata`: Valid metadata (`TotalExtentCount = 16`).
- `bitmap`: Valid bitmap (16 bits).
- `physicalFileSize`: 1048576 (1MB).
#### Preconditions
Struct fields and alignments are correct.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(header, metadata, bitmap, 1048576)`.
#### Expected Output
Void return (Success).
#### Expected State
No observable state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| None | N/A | N/A | 0 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-002 — Invalid magic number
#### Objective
Verify that validation fails immediately if the header's magic number is incorrect.
#### Requirement References
- BR-VAL-002
#### Priority
High
#### Test Category
Negative
#### Input
- `header`: Header with invalid magic signature.
- `metadata`, `bitmap`, `physicalFileSize`: Valid.
#### Preconditions
Header is initialized with a bad magic number.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(header, metadata, bitmap, 1048576)`.
#### Expected Output
Throws `InvalidFileFormatException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated. Remaining validation steps stop.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-003 — Unsupported version
#### Objective
Verify that validation fails if the format version is unsupported.
#### Requirement References
- BR-VAL-003
#### Priority
High
#### Test Category
Negative
#### Input
- `header`: Header with unsupported version (e.g. 99).
- `metadata`, `bitmap`, `physicalFileSize`: Valid.
#### Preconditions
Header has bad format version.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Throws `UnsupportedFileVersionException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-004 — Non-power-of-two page size
#### Objective
Verify that validation fails if the page size is not a power of two.
#### Requirement References
- BR-VAL-002
#### Priority
High
#### Test Category
Negative
#### Input
- `header`: Header with `pageSize = 4097`.
#### Preconditions
Page size is misaligned.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Throws `InvalidFileFormatException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-005 — Unaligned extent size
#### Objective
Verify that validation fails if the extent size is not page-aligned.
#### Requirement References
- BR-VAL-002
#### Priority
High
#### Test Category
Negative
#### Input
- `header`: Header with `extentSize = 3000`.
#### Preconditions
Extent size is unaligned.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Throws `InvalidFileFormatException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-006 — Offset beyond physical size
#### Objective
Verify that validation fails if the metadata offset points beyond the physical file boundary.
#### Requirement References
- BR-VAL-004
#### Priority
High
#### Test Category
Negative
#### Input
- `header`: Metadata offset exceeds `physicalFileSize`.
#### Preconditions
Offset is too large for the file bounds.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Throws `CorruptedFileMetadataException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-007 — Bitmap bitcount mismatch
#### Objective
Verify that validation fails if the bitmap capacity cannot hold the total extent count.
#### Requirement References
- BR-VAL-004
#### Priority
High
#### Test Category
Negative
#### Input
- `metadata`: `TotalExtentCount = 16`.
- `bitmap`: Total bits size = 8.
#### Preconditions
Mismatch between metadata counters and bitmap size.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Throws `CorruptedFileMetadataException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-008 — Free count exceeds capacity
#### Objective
Verify that validation fails if the free extent count is greater than the total extent count.
#### Requirement References
- BR-VAL-004
#### Priority
High
#### Test Category
Negative
#### Input
- `metadata`: `TotalExtentCount = 16`, `FreeExtentCount = 17`.
#### Preconditions
Invalid free count.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Throws `CorruptedFileMetadataException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-009 — Physical size mismatch
#### Objective
Verify that validation fails if the physical file size is smaller than the space required by the metadata extents.
#### Requirement References
- BR-VAL-004
#### Priority
High
#### Test Category
Negative
#### Input
- `physicalFileSize`: 500000.
- `metadata`: `TotalExtentCount = 16`, `ExtentSize = 65536`. (16 * 65536 = 1048576 > 500000)
#### Preconditions
File on disk has been truncated or is incomplete.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(..., 500000)`.
#### Expected Output
Throws `CorruptedFileMetadataException`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
Exception is propagated.
#### Cleanup
None.

### Case UT-FM-VAL-VALIDATE-010 — Header checksum deprecated
#### Objective
Verify that the validator skips checksum verification on legacy files.
#### Requirement References
- BR-VAL-005
#### Priority
Medium
#### Test Category
Alternative
#### Input
- `header`: Non-zero value in legacy checksum field.
#### Preconditions
All other struct configurations are valid.
#### Dependency Setup
None.
#### Execution
Invoke `Validate(...)`.
#### Expected Output
Void return (Success). No exception thrown.
#### Expected State
No observable state change.
#### Expected Dependency Calls
None.
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior
- [x] Invalid input (Negative testing)
- [x] Boundary conditions
- [x] Invalid state (N/A)
- [x] Dependency failures (N/A)
- [x] Cleanup paths (N/A)
- [x] Prohibited dependency calls (N/A)

## 9. Specification Gaps
> None.

## 10. Review Checklist
- [x] Tests observable behavior.
- [x] Does not test private methods.
- [x] Does not use real infrastructure.
- [x] Expected output is measurable.
- [x] Expected state is explicit.
- [x] Failure paths define cleanup.
- [x] Dependency calls are contract-relevant.
- [x] No undocumented behavior was invented.
- [x] All document links are relative.
