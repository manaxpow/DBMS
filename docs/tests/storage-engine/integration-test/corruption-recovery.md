# Integration Test Spec - Corruption Recovery

## Scenario Description
Verifies that database files subjected to manual byte corruptions (magic numbers, format versions, or out-of-bounds metadata sizes) are immediately blocked by the validator.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Magic Number Corruption Recovery
1. Create `test_corrupt_magic.db`.
2. Open raw file stream and overwrite the first 4 bytes with `0xDEADBEEF`.
3. Close raw stream.
4. Attempt to open via manager:
   * Assert calling `lifecycleManager.OpenFile("test_corrupt_magic.db", ...)` throws `InvalidFileFormatException`.

### 2. Format Version Corruption Recovery
1. Create `test_corrupt_version.db`.
2. Open raw stream, seek to format version offset, and write `99`.
3. Close stream.
4. Attempt to open:
   * Assert calling `lifecycleManager.OpenFile("test_corrupt_version.db", ...)` throws `UnsupportedFileVersionException`.

### 3. Allocation Metadata Corruption Recovery
1. Create `test_corrupt_meta.db`.
2. Overwrite the allocation metadata block to assert `TotalExtentCount = 99999` (far exceeding physical capacity).
3. Attempt to open:
   * Assert calling `lifecycleManager.OpenFile("test_corrupt_meta.db", ...)` throws `CorruptedFileMetadataException`.

---

## Cleanup
* Delete corrupt test files.
