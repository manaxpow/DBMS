# Integration Test Spec - Metadata Round-Trip

## Scenario Description
Verifies that formatting headers and allocation structure descriptors serialize and deserialize exactly equivalent parameters when round-tripped to disk.

---

## Environment Setup
* **Sandbox Folder**: `c:\Users\ADMIN\Desktop\DBMS\test_sandbox`

---

## Test Scenario Steps

### 1. Header Serialization & Disk Write
1. Create a `FileHeader` manually:
   * `FileId` = `123`
   * `FileType` = `FileType.Data`
   * `PageSize` = `4096`
   * `ExtentSize` = `65536`
   * `FormatVersion` = `1`
2. Open raw file `test_metadata.db`.
3. Call serializer to write bytes:
   ```csharp
   fileWriter.WriteHeader(handle, header);
   ```

### 2. Disk Read & Deserialization
1. Sync and close the raw descriptor.
2. Reopen raw handle in ReadOnly mode.
3. Read header block:
   ```csharp
   var readHeader = fileReader.ReadHeader(handle);
   ```

### 3. Equivalency Assertions
1. Verify equality of round-tripped metadata parameters:
   * Assert `readHeader.FileId == 123`.
   * Assert `readHeader.FileType == FileType.Data`.
   * Assert `readHeader.PageSize == 4096`.
   * Assert `readHeader.ExtentSize == 65536`.
   * Assert `readHeader.FormatVersion == 1`.

---

## Cleanup
* Delete `test_metadata.db`.
