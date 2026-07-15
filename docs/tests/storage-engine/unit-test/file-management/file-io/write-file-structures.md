# Unit Test Specification — `FileWriter` Structure Writing

## 1. Document Information
- **Specification ID:** UT-FM-IO-WRITE-STRUCTS
- **Component:** File Management
- **Namespace:** `DBMS.StorageEngine.FileManagement.FileIO`
- **Class:** `FileWriter`
- **Interface:** `IFileWriter`
- **Public Methods:**
  - `void WriteHeader(FileHandle handle, FileHeader header)`
  - `void WriteAllocationMetadata(FileHandle handle, FileHeader header, AllocationMetadata metadata)`
  - `void WriteExtentBitmap(FileHandle handle, FileHeader header, AllocationMetadata metadata, ExtentBitmap bitmap)`
- **Design References:**
  - [Service Architecture](../../../../../diagrams/class-diagrams/storage-engine/file-management/service-architecture.md)
  - [Write File Sequence](../../../../../diagrams/sequence-diagrams/storage-engine/file-management/file-io/write-file.md)
- **Status:** Draft
- **Version:** 1.0

## 2. Purpose
Provides methods to serialize structured value objects (file header, allocation metadata, and extent bitmaps) into byte arrays and write them to their corresponding physical offsets in the file.

## 3. Unit Under Test
- **Concrete class:** `FileWriter`
- **Public methods:** `WriteHeader`, `WriteAllocationMetadata`, `WriteExtentBitmap`
- **Inputs:** `FileHandle handle`, `FileHeader header`, `AllocationMetadata metadata`, `ExtentBitmap bitmap`
- **Output:** `void` (Success)
- **Observable state:** Data is flushed to the physical file handle.
- **Dependencies:** `IFileHandle`

## 4. Dependencies
| Dependency | Role | Test-double type |
|---|---|---|
| `FileHandle` (or `IFileHandle`) | Represents the OS-level file handle for executing low-level writes. | Stub / Fake |

## 5. Behavioral Rules
| Rule ID | Behavioral rule |
|---|---|
| BR-IO-WRITE-STRUCT-001 | Must serialize the value object accurately to a byte array and write it exactly to the structural offset specified in the header/metadata. |
| BR-IO-WRITE-STRUCT-002 | The writer does not own the provided file handle. On any write failure, the handle must remain open. |

## 6. Test Case Summary
| Test Case ID | Scenario | Category | Priority |
|---|---|---|---|
| UT-FM-IO-WRITE-STRUCTS-001 | WriteHeader Serialization | Positive | Critical |
| UT-FM-IO-WRITE-STRUCTS-002 | WriteAllocationMetadata Serialization | Positive | Critical |
| UT-FM-IO-WRITE-STRUCTS-003 | WriteExtentBitmap Serialization | Positive | Critical |

## 7. Test Cases

### Case UT-FM-IO-WRITE-STRUCTS-001 — WriteHeader Serialization
#### Objective
Verify that `WriteHeader` serializes the `FileHeader` object and writes it to offset 0.
#### Requirement References
- BR-IO-WRITE-STRUCT-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `handle`: Valid open file handle with write access.
- `header`: Populated `FileHeader`.
#### Preconditions
- Handle is open.
#### Dependency Setup
- `FileHandle.WriteAtOffset` succeeds.
#### Execution
Invoke `WriteHeader(handle, header)`.
#### Expected Output
Void return (Success).
#### Expected State
Data is written to the physical file.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `serializedBytes, 0` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-STRUCTS-002 — WriteAllocationMetadata Serialization
#### Objective
Verify that `WriteAllocationMetadata` serializes the metadata and writes it to the offset defined in the header.
#### Requirement References
- BR-IO-WRITE-STRUCT-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `handle`: Valid open file handle.
- `header`: Parsed `FileHeader` containing `AllocationMetadataOffset`.
- `metadata`: Populated `AllocationMetadata`.
#### Preconditions
- Handle is open.
#### Dependency Setup
- `FileHandle.WriteAtOffset` succeeds.
#### Execution
Invoke `WriteAllocationMetadata(handle, header, metadata)`.
#### Expected Output
Void return (Success).
#### Expected State
Data is written to the physical file.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `serializedBytes, header.AllocationMetadataOffset` | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

### Case UT-FM-IO-WRITE-STRUCTS-003 — WriteExtentBitmap Serialization
#### Objective
Verify that `WriteExtentBitmap` serializes the bitmap and writes it to the correct structural offset.
#### Requirement References
- BR-IO-WRITE-STRUCT-001
#### Priority
Critical
#### Test Category
Positive
#### Input
- `handle`: Valid file handle.
- `header`: Parsed `FileHeader`.
- `metadata`: Parsed `AllocationMetadata`.
- `bitmap`: Populated `ExtentBitmap`.
#### Preconditions
- Handle is open.
#### Dependency Setup
- `FileHandle.WriteAtOffset` succeeds.
#### Execution
Invoke `WriteExtentBitmap(handle, header, metadata, bitmap)`.
#### Expected Output
Void return (Success).
#### Expected State
Data is written to the physical file.
#### Expected Dependency Calls
| Dependency | Operation | Expected arguments | Call count |
|---|---|---|---:|
| `FileHandle` | `WriteAtOffset` | `serializedBytes, metadata.BitmapOffset` (or derived equivalent) | 1 |
#### Prohibited Dependency Calls
None.
#### Expected Failure Handling
N/A
#### Cleanup
None — resource ownership remains with the caller.

## 8. Coverage Review
- [x] Successful behavior
- [x] Alternative valid behavior (N/A)
- [x] Invalid input (Negative testing delegated to `WriteAtOffset`)
- [x] Boundary conditions (Delegated to `WriteAtOffset`)
- [x] Invalid state (N/A)
- [x] Dependency failures (Delegated to `WriteAtOffset`)
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
