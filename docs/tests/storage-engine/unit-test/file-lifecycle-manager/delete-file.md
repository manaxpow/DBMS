# Unit Test Specification — `FileLifecycleManager.DeleteFile`

## 1. Scope

This document covers unit test specifications for `FileLifecycleManager.DeleteFile`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileLifecycleManager`
* **Method:**

```csharp
void DeleteFile(string fileName);
```

## 3. Unit Under Test

`FileLifecycleManager.DeleteFile` is responsible for:

* Requesting a deletion lock (via `TryBeginDelete`) to block concurrent file openings.
* Rejecting deletion if active handles are open (throwing `FileInUseException`).
* Verifying physical file existence on disk.
* Performing physical file deletion.
* Finalizing registry removal (via `CompleteDelete`) on deletion success.
* Releasing deletion lock markers (via `CancelDelete`) on deletion failure or missing files.

## 4. Mocked Dependencies

* `IOpenFileManager`
* `IPhysicalFileSystem`

---

# Test Cases

## Case 1: Successful File Deletion

### Input

```text
fileName = "test.db"
```

### Preconditions

* File exists on disk.
* File is not open.
* Deletion lock is acquired successfully.
* Physical file deletion succeeds.

### Execution

```csharp
DeleteFile("test.db");
```

### Expected Output

* Void return (Success).

### Expected State

* Physical file is removed from disk.
* Deletion lock is completed and registry slot cleared.

### Expected Dependency Calls

```text
OpenFileManager.TryBeginDelete("test.db")
    -> returns true

IPhysicalFileSystem.Exists("test.db")
    -> returns true

IPhysicalFileSystem.Delete("test.db")
    -> succeeds

OpenFileManager.CompleteDelete("test.db")
    -> succeeds
```

### Expected Call Order

```text
OpenFileManager.TryBeginDelete("test.db")
    before
IPhysicalFileSystem.Exists("test.db")
    before
IPhysicalFileSystem.Delete("test.db")
    before
OpenFileManager.CompleteDelete("test.db")
```

### Suggested Test Name

```csharp
DeleteFile_FileExistsAndNotOpen_Succeeds()
```

---

## Case 2: Blocked when File Is Open

### Input

```text
fileName = "test.db"
```

### Preconditions

* File is registered in the open file registry with active handle.

### Execution

```csharp
DeleteFile("test.db");
```

### Expected Output

```text
Throws FileInUseException
```

### Expected Dependency Calls

```text
OpenFileManager.TryBeginDelete("test.db")
    -> returns false
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Exists(...)
IPhysicalFileSystem.Delete(...)
```

### Suggested Test Name

```csharp
DeleteFile_FileIsOpen_ThrowsFileInUseException()
```

---

## Case 3: File Does Not Exist

### Input

```text
fileName = "test.db"
```

### Preconditions

* File is not open.
* Deletion lock is acquired successfully.
* Physical check reports file does not exist.

### Execution

```csharp
DeleteFile("test.db");
```

### Expected Output

```text
Throws FileNotFoundException
```

### Expected State

* Deletion lock is canceled and registry cleared.

### Expected Dependency Calls

```text
OpenFileManager.TryBeginDelete("test.db")
    -> returns true

IPhysicalFileSystem.Exists("test.db")
    -> returns false

OpenFileManager.CancelDelete("test.db")
    -> succeeds
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Delete(...)
OpenFileManager.CompleteDelete(...)
```

### Suggested Test Name

```csharp
DeleteFile_FileDoesNotExist_ThrowsFileNotFoundException()
```

---

## Case 4: Physical Delete Fails

### Input

```text
fileName = "test.db"
```

### Preconditions

* File is not open, exists, but OS locks it (Access Denied).

### Execution

```csharp
DeleteFile("test.db");
```

### Expected Output

```text
Throws FileDeleteException
```

### Expected Exception

* Wraps the root `IOException` thrown by physical deletion.

### Expected State

* Deletion lock is canceled.
* File is not deleted.

### Expected Dependency Calls

```text
OpenFileManager.TryBeginDelete("test.db")
    -> returns true

IPhysicalFileSystem.Exists("test.db")
    -> returns true

IPhysicalFileSystem.Delete("test.db")
    -> throws IOException

OpenFileManager.CancelDelete("test.db")
    -> succeeds
```

### Forbidden Dependency Calls

```text
OpenFileManager.CompleteDelete(...)
```

### Suggested Test Name

```csharp
DeleteFile_PhysicalDeletionFails_RollsBackLockStateAndThrows()
```


---

# Summary

| ID | Scenario                        | Expected Result                    |
| -: | ------------------------------- | ---------------------------------- |
|  1 | Safe deletion of inactive file  | Physical file deleted and completed|
|  2 | Blocked when open               | Throws `FileInUseException`        |
|  3 | File does not exist             | Throws `FileNotFoundException`     |
|  4 | Physical delete fails           | Throws, cancels deletion lock      |

## Total

```text
4 independent unit test behaviors
```
