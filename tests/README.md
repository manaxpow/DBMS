# Test Documentation - File Management

This directory contains the complete test specifications for the File Management component of the Storage Engine, split into Unit and Integration tests.

## Directory Structure
* **[tests/README.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/README.md)**: Main test directory index.
* **[unit-test/](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test)**: Component unit tests asserting public behaviors and dependency interactions using mocks.
* **[integration-test/](file:///c:/Users/ADMIN/Desktop/DBMS/tests/integration-test)**: End-to-end integration tests validating real filesystem I/O, resizing, recovery, and concurrency on C#.

## Unit Tests
1. **[file-lifecycle-manager/](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager)**: Coordinates file lifecycle actions.
   - [create-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/create-file.md): CreateFile() validation, constraints, and failures.
   - [open-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/open-file.md): OpenFile() compatibility, locks, and validation errors.
   - [close-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/close-file.md): CloseFile() reference counts and resource releases.
   - [delete-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/delete-file.md): DeleteFile() status markers and conflict blocks.
   - [resize-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/resize-file.md): ResizeFile() extensions, truncations, and errors.
2. **[file-reader.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-reader.md)**: Physical offset reads, block bounds, and format deserializers.
3. **[file-writer.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-writer.md)**: Physical writes, serialization, and write-mode access blocks.
4. **[file-synchronizer.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-synchronizer.md)**: Flushing hardware commits and I/O exceptions wrapper.
5. **[file-validator.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-validator.md)**: File format structure and counter validators.
6. **[open-file-manager.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/open-file-manager.md)**: Open entry collections, duplicates, and deletion marks.
7. **[extent-manager.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/extent-manager.md)**: Extent management space checks and rollback logic.
8. **[allocation-metadata.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/allocation-metadata.md)**: Space allocation metadata counters, bitmap tracking, and safe truncations.
9. **[extent-bitmap.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/extent-bitmap.md)**: Bitmap operations, find frees, and bitmap index boundary checks.

## Integration Tests
1. **[file-lifecycle/](file:///c:/Users/ADMIN/Desktop/DBMS/tests/integration-test/file-lifecycle)**: Real creation, opening, closing, deletion, and resizing on disk.
2. **[file-io/](file:///c:/Users/ADMIN/Desktop/DBMS/tests/integration-test/file-io)**: Real reads/writes, persistence flushes, and round-tripping headers.
3. **[extent-management/](file:///c:/Users/ADMIN/Desktop/DBMS/tests/integration-test/extent-management)**: Disk allocations/frees and dynamic physical file auto-resizing.
4. **[corruption-recovery.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/integration-test/corruption-recovery.md)**: Garbage header injections and validation error recoveries.
5. **[lifecycle-concurrency.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/integration-test/lifecycle-concurrency.md)**: Multi-threaded opens, deletes, allocations, and lock stress runs.

## Execution Order
We recommend implementing tests in the following order starting from low-level memory logic up to coordinated file system calls:
1. `extent-bitmap.md`
2. `allocation-metadata.md`
3. `file-validator.md`
4. `open-file-manager.md`
5. `file-reader.md`
6. `file-writer.md`
7. `file-synchronizer.md`
8. `extent-manager.md`
9. `file-lifecycle-manager/`
10. `integration-test/`
