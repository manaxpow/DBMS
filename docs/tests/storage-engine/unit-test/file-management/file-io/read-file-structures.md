# Unit Test Specification — `FileReader` Structure Reading

## 1. Document Information
- **Specification ID:** UT-FM-IO-READ-STRUCTS
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileIO`
- **Class:** `FileReader`
- **Interface:** `IFileReader`
- **Public Methods:**
  - `FileHeader ReadHeader(FileHandle handle)`
  - `AllocationMetadata ReadAllocationMetadata(FileHandle handle, FileHeader header)`
  - `ExtentBitmap ReadExtentBitmap(FileHandle handle, FileHeader header, AllocationMetadata metadata)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Read File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-io/read-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Provides methods to deserialize raw bytes from specific offsets in the file into structured value objects representing the file header, allocation metadata, and extent bitmaps.

## 3. Unit Under Test
- **Concrete class:** `FileReader`
- **Public methods:** `ReadHeader`, `ReadAllocationMetadata`, `ReadExtentBitmap`
- **Inputs:** `FileHandle handle`, `FileHeader header`, `AllocationMetadata metadata`
- **Output:** Parsed struct objects (`FileHeader`, `AllocationMetadata`, `ExtentBitmap`)
- **Observable state:** None locally. Returns new in-memory struct objects.
- **Dependencies:** `IFileHandle`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `FileHandle` (or `IFileHandle`) | Represents the OS-level file handle for executing low-level reads. | Stub / Fake |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-IO-READ-STRUCT-001 | Must read exactly the required number of bytes for the structure from the appropriate physical offset. |
| BR-IO-READ-STRUCT-002 | Must deserialize bytes into the target value object without performing logical validation of the fields. |
| BR-IO-READ-STRUCT-003 | The reader does not own the provided file handle. On any read failure, the handle must remain open. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-IO-READ-STRUCTS-001 | ReadHeader Deserialization | Positive | Critical |
| UT-FM-IO-READ-STRUCTS-002 | ReadAllocationMetadata Deserialization | Positive | Critical |
| UT-FM-IO-READ-STRUCTS-003 | ReadExtentBitmap Deserialization | Positive | Critical |

## 7. Test Cases

### Case UT-FM-IO-READ-STRUCTS-001 — ReadHeader Deserialization
#### Objective
Verify that `ReadHeader` parses the bytes at offset 0 into a `FileHeader` object.
#### Requirement References
- BR-IO-READ-STRUCT-001
- BR-IO-READ-STRUCT-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `handle`: Valid open file handle.
#### Preconditions
- Handle is open.
- Raw bytes at offset 0 contain a valid serialized `FileHeader`.
#### Dependency Setup
- `FileHandle.ReadAtOffset` returns valid bytes for a header.
#### Execution
Invoke `ReadHeader(handle)`.
#### Expected Output
Returns a valid `FileHeader` object matching the raw byte input.
#### Expected State
No observable state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `buffer, 0` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-STRUCTS-002 — ReadAllocationMetadata Deserialization
#### Objective
Verify that `ReadAllocationMetadata` reads from the correct offset specified by the header and parses the metadata.
#### Requirement References
- BR-IO-READ-STRUCT-001
- BR-IO-READ-STRUCT-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `handle`: Valid file handle.
- `header`: Parsed `FileHeader` containing `AllocationMetadataOffset`.
#### Preconditions
- Handle is open.
- Raw bytes at the metadata offset contain a valid serialized `AllocationMetadata`.
#### Dependency Setup
- `FileHandle.ReadAtOffset` returns valid bytes.
#### Execution
Invoke `ReadAllocationMetadata(handle, header)`.
#### Expected Output
Returns a valid reconstructed `AllocationMetadata` structure.
#### Expected State
No observable state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `buffer, header.AllocationMetadataOffset` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-READ-STRUCTS-003 — ReadExtentBitmap Deserialization
#### Objective
Verify that `ReadExtentBitmap` reads from the correct offset and returns a bitmap initialized with the correct number of bits.
#### Requirement References
- BR-IO-READ-STRUCT-001
- BR-IO-READ-STRUCT-002
#### Priority
Critical
#### Test Category
Positive
#### Input
- `handle`: Valid file handle.
- `header`: Parsed `FileHeader`.
- `metadata`: `AllocationMetadata` where `TotalExtentCount = 16`.
#### Preconditions
- Handle is open.
- Raw bytes at the bitmap offset contain 16 valid bits.
#### Dependency Setup
- `FileHandle.ReadAtOffset` returns valid bytes.
#### Execution
Invoke `ReadExtentBitmap(handle, header, metadata)`.
#### Expected Output
Returns a valid reconstructed `ExtentBitmap` whose bit count matches the `TotalExtentCount` in `metadata`.
#### Expected State
No observable state change.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `ReadAtOffset` | `buffer, metadata.BitmapOffset` (or derived equivalent) | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
- [x] Invalid input (Negative testing delegated to `ReadAtOffset`)
- [x] Boundary conditions (Delegated to `ReadAtOffset`)
- [x] Invalid state (N/A)
- [x] Dependency failures (Delegated to `ReadAtOffset`)
- [x] Cleanup paths (Resource ownership clearly documented)
- [x] Prohibited dependency calls

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
