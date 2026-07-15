# Integration Test Specifications - Storage Engine

This directory contains integration test specifications for the Storage Engine. These tests verify the end-to-end behavior of the storage engine by writing and reading data from a real physical file system and verifying behaviors like recovery and concurrent access.

## Specifications

* **[physical-file-system.md](physical-file-system.md)**: Verifies the `PhysicalFileSystem` behavior against real operating system IO, including sharing policies, locking, and cleanups.
* **[corruption-recovery.md](corruption-recovery.md)**: Tests recovery from storage corruption.
* **[lifecycle-concurrency.md](lifecycle-concurrency.md)**: Tests concurrency in file lifecycle operations.
* **[extent-management/](extent-management/)**: Tests extent allocation and deallocation logic on real disk.
* **[file-lifecycle/](file-lifecycle/)**: Tests file lifecycle workflows.
* **[file-io/](file-io/)**: Tests basic file read and write performance and correctness.
