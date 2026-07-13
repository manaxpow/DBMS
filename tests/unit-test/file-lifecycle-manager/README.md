# Unit Test Specs - File Lifecycle Manager

This folder contains the Unit Test Specifications for `FileLifecycleManager`, testing the coordination logic of file creations, openings, closures, deletes, and resizes.

## Mocking Strategy
The unit tests in this section check the orchestration behavior of `FileLifecycleManager` using mock implementations of its direct dependencies:
* `IFileReader`
* `IFileWriter`
* `IFileSynchronizer`
* `IOpenFileManager`
* `IFileValidator`

Unit tests do not touch the real filesystem or check actual file existence on disk.

## Test Files
* **[create-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/create-file.md)**: Logic checks for creating new files and rolling back on dependency errors.
* **[open-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/open-file.md)**: Logic checks for file opens, locks, validations, and reference increments.
* **[close-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/close-file.md)**: Logic checks for closing active handles, refCount decrements, and unregistration triggers.
* **[delete-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/delete-file.md)**: Logic checks for deletion lock status and concurrency blocks.
* **[resize-file.md](file:///c:/Users/ADMIN/Desktop/DBMS/tests/unit-test/file-lifecycle-manager/resize-file.md)**: Logic checks for file resizing (extending and truncating).
