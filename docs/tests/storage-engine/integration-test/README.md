# Integration Tests — Storage Engine

## Purpose

This directory contains the formal integration test specifications for the Storage Engine. Unlike unit tests, integration tests cross architectural boundaries to interact with real infrastructure (primarily the physical operating system file system). These tests verify persistence, round-trip metadata serialization, hardware boundaries, and correct OS-level locking semantics.

## Responsibility Map

| Directory | Scope | Test Type |
|---|---|---|
| `physical-storage/` | `PhysicalFileSystem` wrapper. | Direct Adapter Integration |
| `file-lifecycle/` | `FileLifecycleManager` orchestration. | Orchestrator Integration |
| `file-io/` | `FileReader`, `FileWriter`, `FileSynchronizer`. | Byte Persistence Integration |
| `file-validation/` | `FileValidator` against corrupted headers. | Failure Integration |
| `extent-management/` | `ExtentManager` allocating on real bit streams. | Structural Persistence Integration |

## Environment Policies

To guarantee test stability and reliability, all integration tests must strictly adhere to the following policies:

1. **Isolation Strategy**: Every integration test must utilize a unique temporary directory obtained via `Path.GetTempPath()` combined with a unique identifier (e.g., `Guid.NewGuid()`). Shared static test directories (e.g., `c:\Users\...\test_sandbox`) are strictly forbidden.
2. **Resource Ownership**: A test implicitly owns any resource it creates.
3. **Cleanup Guarantee**: All resources (handles, files, temporary directories) must be forcefully closed and deleted after the test executes. This cleanup must be wrapped in `finally` blocks or `IDisposable` structures so it runs regardless of test pass/fail status.
4. **Platform Agnosticism**: Unless a test specifically targets an OS-bound feature (like Windows-specific `FileShare` locking rules), it must assert platform-agnostic behaviors.

## Concurrency Note

Concurrency tests evaluating race conditions, thread contention, and locking boundaries are explicitly out of scope for this folder. All such tests reside in `docs/tests/storage-engine/concurrency-test/`.
