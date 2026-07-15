# Integration Test Specification — `PhysicalFileSystem`

## 1. Scope

This document covers integration test specifications for the `PhysicalFileSystem` class.
These tests verify that the system correctly interacts with the underlying operating system file system, including creating files, reading sizes, resizing, deleting, and enforcing OS-level sharing locks.

## 2. Component

* **Namespace:** `DBMS.StorageEngine.FileManagement.PhysicalStorage`
* **Class:** `PhysicalFileSystem`

## 3. Unit Under Test

`PhysicalFileSystem` is responsible for:
* Abstracting OS calls (`File.Exists`, `File.Open`, `File.Create`, `File.Delete`, etc.).
* Enforcing the approved `FileShare` and locking semantics.
* Providing the true physical size of a file.

## 4. Test Sandbox Preconditions

Every test in this specification must:
* Be executed in a temporary test directory.
* Clean up all temporary files and handles it creates, regardless of test success or failure (e.g. using `try/finally` or test teardown hooks).

---

# Test Cases

## Case 1: Create New Physical File

### Purpose
Verifies that a new file can be created on the disk with the specified initial physical size.

### Input
* `fileName` (in a temporary sandbox directory)
* `initialFileSize = 1048576`

### Execution
```csharp
PhysicalFileSystem.Create(fileName, 1048576);
```

### Expected Output
* Returns a valid `FileHandle`.

### Expected State
* A real physical file is created at the target path.
* The physical file size on disk is exactly `1048576` bytes.

### Suggested Test Name
```csharp
PhysicalFileSystem_Create_AllocatesCorrectSizeOnDisk()
```

---

## Case 2: Open File with Exclusive Sharing Lock

### Purpose
Verifies that `Open` uses the approved `FileShare` policy and correctly blocks or allows subsequent access attempts.

### Preconditions
* A physical file exists in the temporary sandbox.

### Execution
1. Thread A calls `PhysicalFileSystem.Open(fileName, FileAccessMode.ReadWrite)`.
2. Thread B attempts to delete or open the same file (dependent on OS and sharing policy).

### Expected State
* **Platform Constraints:** Verify `Open` uses the approved `FileShare` policy. On platforms where the sharing mode prevents deletion (e.g., Windows), a deletion attempt on an open file MUST fail with an `IOException`.
* On UNIX-like environments (if supported), the behavior may differ, and platform-dependent behavior should be asserted accordingly.

### Suggested Test Name
```csharp
PhysicalFileSystem_Open_EnforcesFileSharePolicyLocks()
```

---

## Case 3: Get Physical File Size

### Purpose
Verifies that the `GetSize` method returns the correct physical size from the OS.

### Preconditions
* A physical file of size `4096` exists.
* The file is successfully opened, returning a `FileHandle`.

### Execution
```csharp
PhysicalFileSystem.GetSize(fileHandle);
```

### Expected Output
* Returns `4096`.

### Suggested Test Name
```csharp
PhysicalFileSystem_GetSize_ReturnsExactOSFileSize()
```

---

## Case 4: Resize Physical File

### Purpose
Verifies that `Resize` interacts with the OS to accurately truncate or extend the file.

### Preconditions
* A physical file of size `4096` exists and is open.

### Execution
```csharp
PhysicalFileSystem.Resize(fileHandle, 8192);
```

### Expected State
* The OS reports the physical file size is now `8192`.

### Suggested Test Name
```csharp
PhysicalFileSystem_Resize_UpdatesPhysicalSizeOnDisk()
```

---

## Case 5: Delete Physical File

### Purpose
Verifies that a file that is not locked can be cleanly removed from the OS.

### Preconditions
* A physical file exists and is closed (no active handles).

### Execution
```csharp
PhysicalFileSystem.Delete(fileName);
```

### Expected State
* `Exists(fileName)` returns `false`.
* The physical file is removed from the temporary directory.

### Suggested Test Name
```csharp
PhysicalFileSystem_Delete_RemovesFileFromDisk()
```
