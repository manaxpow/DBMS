# Unit Test Spec - FileValidator

## Component
* **Class**: `FileValidator`
* **Interface**: `IFileValidator`

---

## Test Cases

### Case 1: Verification Succeeds (Happy Path)
* **Input**: `header`, `metadata`, `bitmap`, `physicalFileSize = 1048576 (1MB)`
* **Preconditions**: Valid metadata (TotalExtentCount = 16, ExtentSize = 65536).
* **Execution**: `Validate(header, metadata, bitmap, 1048576)`
* **Expected Output**: Void return (Success).

### Case 2: Magic Number Mismatch Failure
* **Input**: `header` (with invalid magic signature bytes)
* **Expected Output**: Throws `InvalidFileFormatException`.

### Case 3: Unsupported Format Version
* **Input**: `header` (with invalid version number, e.g. 99)
* **Expected Output**: Throws `UnsupportedFileVersionException`.

### Case 4: Invalid Page Size
* **Input**: `header` (with non-power-of-2 page size, e.g. 4097)
* **Expected Output**: Throws `InvalidFileFormatException` (or validation exception).

### Case 5: Invalid Extent Size
* **Input**: `header` (with non-page-aligned extent size, e.g. 3000)
* **Expected Output**: Throws `InvalidFileFormatException`.

### Case 6: Metadata Offset Out of Bounds
* **Input**: `header` (where `AllocationMetadataOffset` exceeds `physicalFileSize`)
* **Expected Output**: Throws `CorruptedFileMetadataException`.

### Case 7: Extent Bitmap has Insufficient Bits
* **Input**: `metadata` (TotalExtentCount = 16), `bitmap` (TotalExtents = 8)
* **Expected Output**: Throws `CorruptedFileMetadataException`.

### Case 8: FreeExtentCount Greater Than TotalExtentCount
* **Input**: `metadata` (TotalExtentCount = 16, FreeExtentCount = 17)
* **Expected Output**: Throws `CorruptedFileMetadataException`.

### Case 9: Physical File Size Mismatches Metadata
* **Input**: `physicalFileSize = 500000`, `metadata` (TotalExtentCount = 16, ExtentSize = 65536 -> expects 1,048,576 bytes)
* **Expected Output**: Throws `CorruptedFileMetadataException` (Boundary mismatch).

### Case 10: Header Checksum Skipped
* **Description**: Verifies that checksum validations are not performed as file trailers and checksum structures have been deprecated.
