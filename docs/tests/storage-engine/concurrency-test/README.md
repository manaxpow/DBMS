# Concurrency Tests — Storage Engine

## Purpose

This directory contains the formal concurrency test specifications for the Storage Engine. These tests verify thread safety, race condition handling, lock policies, and parallel execution boundary conditions across all Storage Engine components. 

## Structure

| Directory | Scope | Test Type |
|---|---|---|
| `file-management/` | File creation, registration, and extent allocation contention. | Thread Safety |

## Concurrency Test Policies

Concurrency tests are often non-deterministic by nature, but these specifications are designed to enforce as much determinism as possible through structural patterns:

1. **Isolation Strategy**: Every concurrent test interacting with physical files must use a strictly unique temporary directory (e.g., `Path.GetTempPath()` combined with a test-unique GUID). Test files must never be shared across test boundaries.
2. **Deterministic Locking**: Tests evaluating exclusive locks must assert exact success and failure counts (e.g., exactly 1 thread succeeds, exactly N-1 threads fail with specific `LockConflictException`).
3. **Barrier Synchronization**: Tests simulating race conditions (e.g., simultaneous Delete and Close) must describe the mechanism used to align the threads (e.g., `CountdownEvent` or thread synchronization primitives).
4. **Guaranteed Cleanup**: Concurrency tests must block the main thread until all pooled worker threads finish, after which a guaranteed cleanup mechanism (such as `try/finally`) must shut down all handles and wipe the physical temporary directory.
