# Storage Engine Unit Tests

This directory contains the unit test specifications for the components of the Storage Engine.

## Components

### 1. File Management
- **[File Management Unit Tests](file-management/README.md)**: Specifications for physical file manipulation, metadata structures, and lifecycle coordination.
*(Note: Some legacy documents are still located in the root of this directory pending migration: `file-lifecycle-manager/`, `file-synchronizer.md`, `open-file-manager.md`, `extent-manager.md`)*

### 2. Buffer Management
*(To be added in future phases)*

### 3. Record Management
*(To be added in future phases)*

## Execution Order
We recommend implementing tests starting from low-level memory logic up to coordinated file system calls. Refer to the individual component indexes for specific execution orders.
