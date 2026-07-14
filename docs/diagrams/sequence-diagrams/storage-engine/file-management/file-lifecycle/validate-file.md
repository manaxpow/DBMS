# Sequence Diagram - Validate File

## Use Case Overview

**Actor**: `FileLifecycleManager`  
**Purpose**: Validates the structure, identity, version, and metadata of a data file before it is registered as an active open file.

### Preconditions

- The physical file has already been opened and its header, allocation metadata, and extent bitmap have been read into memory.
- The `physicalFileSize` has been determined.
- The file has not yet been registered in `OpenFileManager`.

---

## 1. Happy Path: Valid File

```mermaid
sequenceDiagram
    autonumber

    actor FLM as FileLifecycleManager
    participant FV as FileValidator

    FLM->>FV: Validate(header, metadata, bitmap, physicalFileSize)
    activate FV

    FV->>FV: ValidateMagicNumber(header)
    FV->>FV: ValidateFormatVersion(header)
    FV->>FV: ValidateFileBoundary(header, physicalFileSize)
    
    note right of FV: Validates physical size can contain the header and metadata.

    FV->>FV: ValidateAllocationMetadata(metadata, bitmap)

    note right of FV: Validates extent count, bitmap size,<br/>offset ranges and capacity bounds.

    FV-->>FLM: void (Success)
    deactivate FV
```

---

## 2. Failure Path: Invalid Magic Number

```mermaid
sequenceDiagram
    autonumber

    actor FLM as FileLifecycleManager
    participant FV as FileValidator

    FLM->>FV: Validate(header, metadata, bitmap, physicalFileSize)
    activate FV

    FV->>FV: ValidateMagicNumber(header)

    FV-->>FLM: throw InvalidFileFormatException
    deactivate FV
```

---

## 3. Failure Path: Unsupported File Version

```mermaid
sequenceDiagram
    autonumber

    actor FLM as FileLifecycleManager
    participant FV as FileValidator

    FLM->>FV: Validate(header, metadata, bitmap, physicalFileSize)
    activate FV

    FV->>FV: ValidateMagicNumber(header)
    FV->>FV: ValidateFormatVersion(header)

    FV-->>FLM: throw UnsupportedFileVersionException
    deactivate FV
```

---

## 4. Failure Path: Corrupted Allocation Metadata

```mermaid
sequenceDiagram
    autonumber

    actor FLM as FileLifecycleManager
    participant FV as FileValidator

    FLM->>FV: Validate(header, metadata, bitmap, physicalFileSize)
    activate FV

    FV->>FV: ValidateMagicNumber(header)
    FV->>FV: ValidateFormatVersion(header)
    FV->>FV: ValidateFileBoundary(header, physicalFileSize)
    
    FV->>FV: ValidateAllocationMetadata(metadata, bitmap)
    
    note right of FV: E.g., free extent count exceeds total extent count

    FV-->>FLM: throw CorruptedFileMetadataException
    deactivate FV
```

---

## Validation Rules

### File Header Validation

```text
Magic number matches the expected DBMS file signature.

Format version is supported by the current DBMS version.

File type is recognized.

Page size is greater than zero.

Page size follows the configured alignment.

Header size does not exceed the physical file size.

Metadata offsets are inside the file boundary.

File identifier is present and valid.
```

### Allocation Metadata Validation

```text
Extent size is greater than zero.

Total extent count matches the physical file size.

Free extent count does not exceed total extent count.

Extent bitmap contains enough bits for all extents.

Metadata offsets do not overlap the file header.

Metadata offsets do not exceed the physical file boundary.
```

---

## Discovered Candidates

### Method Candidates

```text
FileValidator.Validate(
    header: FileHeader,
    metadata: AllocationMetadata,
    bitmap: ExtentBitmap,
    physicalFileSize: long
) : void

FileValidator.ValidateMagicNumber(
    header: FileHeader
) : void

FileValidator.ValidateFormatVersion(
    header: FileHeader
) : void

FileValidator.ValidateFileBoundary(
    header: FileHeader,
    physicalFileSize: long
) : void

FileValidator.ValidateAllocationMetadata(
    metadata: AllocationMetadata,
    bitmap: ExtentBitmap
) : void
```

### Result Candidates

```text
void (Success)
```

### Exception Candidates

```text
InvalidFileFormatException

UnsupportedFileVersionException

CorruptedFileMetadataException
```

### State Candidates

No runtime state change is required.

Validation only reads and verifies existing in-memory data structures.

---

## Responsibility Assignment

### `FileValidator`

- Coordinates all file validation rules.
- Validates relationships between header, metadata and physical file size.
- Validates extent counters and bitmap consistency.
- Converts validation failures into domain-specific exceptions.
- Does not interact with disk or read structures directly.

---

## Integration with Open File

`Validate File` is normally a subflow of `Open File`.

```text
Open physical file
    → Read file structures (FileReader)
    → Validate file (FileValidator)
    → Create DataFile
    → Create OpenFileEntry
    → Register OpenFileEntry
```

The file must not be registered in `OpenFileManager` before validation succeeds.
