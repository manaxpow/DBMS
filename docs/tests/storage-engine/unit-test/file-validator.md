# Unit Test Specification — `FileValidator`

## 1. Scope

This document covers unit test specifications for `FileValidator`.
Tests are isolated from all external dependencies.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileValidator`
* **Method:**

```csharp
void Validate(
    FileHeader header,
    AllocationMetadata metadata,
    ExtentBitmap bitmap,
    long physicalFileSize);
```

## 3. Unit Under Test

`FileValidator.Validate` is responsible for:

* Checking magic signatures to confirm it matches the database type.
* Verifying format compatibility versions.
* Validating structural alignments (pages must be power-of-two, extents page-aligned).
* Confirming structural offsets (e.g. metadata offset must fit within the physical file size).
* Verifying that extent counters match bitmap total bit counts.
* Ensuring the free counts do not exceed the total extent capacity bounds.
* Checking that the total physical disk size matches the extent allocations layout.

## 4. Dependencies (None)

None. `FileValidator` is a pure domain validation helper class, tested with in-memory value objects.

---

# Test Cases

## Case 1: Verification Succeeds

### Input

```text
header           = Valid header configuration
metadata         = Valid metadata (TotalExtentCount = 16)
bitmap           = Valid bitmap (16 bits)
physicalFileSize = 1048576 (1MB)
```

### Preconditions

* Struct fields and checksums align.

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

* Void return (Success).

### Suggested Test Name

```csharp
Validate_ValidFileStructures_Succeeds()
```

---

## Case 2: Magic Number Mismatch Failure

### Input

```text
header = Header with invalid magic signature
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws InvalidFileFormatException
```

### Suggested Test Name

```csharp
Validate_MagicNumberMismatch_ThrowsInvalidFileFormatException()
```

---

## Case 3: Unsupported Format Version

### Input

```text
header = Header with unsupported version (e.g. 99)
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws UnsupportedFileVersionException
```

### Suggested Test Name

```csharp
Validate_UnsupportedVersion_ThrowsUnsupportedFileVersionException()
```

---

## Case 4: Invalid Page Size

### Input

```text
header = Header with pageSize = 4097 (not power-of-two)
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws InvalidFileFormatException
```

### Suggested Test Name

```csharp
Validate_InvalidPageSize_ThrowsInvalidFileFormatException()
```

---

## Case 5: Invalid Extent Size Alignment

### Input

```text
header = Header with extentSize = 3000 (not page-aligned)
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws InvalidFileFormatException
```

### Suggested Test Name

```csharp
Validate_InvalidExtentSize_ThrowsInvalidFileFormatException()
```

---

## Case 6: Metadata Offset Out of Bounds

### Input

```text
header = Header where metadata offset exceeds physicalFileSize
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws CorruptedFileMetadataException
```

### Suggested Test Name

```csharp
Validate_MetadataOffsetExceedsBounds_ThrowsCorruptedFileMetadataException()
```

---

## Case 7: Extent Bitmap Size Mismatch

### Input

```text
metadata = TotalExtentCount = 16
bitmap   = Total bits size = 8
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws CorruptedFileMetadataException
```

### Suggested Test Name

```csharp
Validate_BitmapSizeMismatch_ThrowsCorruptedFileMetadataException()
```

---

## Case 8: FreeExtentCount Greater Than TotalExtentCount

### Input

```text
metadata = TotalExtentCount = 16, FreeExtentCount = 17
```

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

```text
Throws CorruptedFileMetadataException
```

### Suggested Test Name

```csharp
Validate_FreeCountExceedsTotal_ThrowsCorruptedFileMetadataException()
```

---

## Case 9: Physical File Size Mismatches Metadata

### Input

```text
physicalFileSize = 500000
metadata         = TotalExtentCount = 16, ExtentSize = 65536
```

### Execution

```csharp
Validate(header, metadata, bitmap, 500000);
```

### Expected Output

```text
Throws CorruptedFileMetadataException
```

### Suggested Test Name

```csharp
Validate_PhysicalSizeTooSmall_ThrowsCorruptedFileMetadataException()
```

---

## Case 10: Header Checksum Skipped

### Purpose

Verifies that the validator does not reject files based on checksum field values. The checksum feature has been deprecated from the file format. This prevents false-positive validation failures on otherwise-valid files that contain non-zero legacy checksum fields.

### Input

```text
header           = fileHeader (with a non-zero legacy checksum field)
metadata         = validAllocationMetadata
bitmap           = validExtentBitmap
physicalFileSize = 1048576
```

### Preconditions

* `header` contains a non-zero value in the checksum field that would fail if checksum validation were active.
* All other fields in `header`, `metadata`, `bitmap`, and `physicalFileSize` are structurally valid.

### Execution

```csharp
Validate(header, metadata, bitmap, 1048576);
```

### Expected Output

* Void return (Success). No exception is thrown despite the invalid checksum value.

### Suggested Test Name

```csharp
Validate_TrailerChecksums_Skipped()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Valid structures                | Succeeds                             |
|  2 | Invalid magic number            | Throws `InvalidFileFormatException`  |
|  3 | Unsupported version             | Throws `UnsupportedFileVersionException`|
|  4 | Non-power-of-two page size      | Throws `InvalidFileFormatException`  |
|  5 | Unaligned extent size           | Throws `InvalidFileFormatException`  |
|  6 | Offset beyond physical size     | Throws `CorruptedFileMetadataException`|
|  7 | Bitmap bitcount mismatch        | Throws `CorruptedFileMetadataException`|
|  8 | Free count exceeds capacity     | Throws `CorruptedFileMetadataException`|
|  9 | Physical size mismatch          | Throws `CorruptedFileMetadataException`|
| 10 | Header checksum deprecated      | Checksums are skipped                |

## Total

```text
10 independent unit test behaviors
```
