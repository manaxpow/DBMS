# Unit Test Specification — `FileLifecycleManager.CloseFile`

## 1. Scope

This document covers unit test specifications for `FileLifecycleManager.CloseFile`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileLifecycleManager`
* **Method:**

```csharp
void CloseFile(string fileName);
```

## 3. Unit Under Test

`FileLifecycleManager.CloseFile` is responsible for:

* Finding the active `OpenFileEntry` mapping by name.
* Decrementing the `ReferenceCount`.
* Bypassing synchronization and handle closure if `ReferenceCount > 0`.
* Persisting all buffered pages to disk (via `Sync`) when `ReferenceCount` reaches 0 (unless opened in `ReadOnly` mode).
* Closing physical operating-system handles.
* Unregistering the entry from the active open registry.
* Safely escalating synchronization or file handle closure errors while maintaining directory states.

## 4. Mocked Dependencies

* `IOpenFileManager`
* `IFileSynchronizer`
* `IPhysicalFileSystem`

---

# Test Cases

## Case 1: ReferenceCount > 1 Only Decrements Count

### Input

```text
fileName = "test.db"
```

### Preconditions

* File is registered in `OpenFileManager` with `ReferenceCount == 3`.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Output

* Void return (Success).

### Expected State

* `ReferenceCount` is decremented to `2`.
* Operating-system handle remains open.
* Entry remains registered in the collection.

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("test.db")
    -> returns openFileEntry

openFileEntry.DecrementRefCount()
    -> returns 2
```

### Forbidden Dependency Calls

```text
FileSynchronizer.Sync(...)
IPhysicalFileSystem.Close(...)
OpenFileManager.UnregisterOpenFile(...)
```

### Suggested Test Name

```csharp
CloseFile_RefCountGreaterThanOne_DecrementsCountOnly()
```

---

## Case 2: ReferenceCount == 1 Syncs and Closes

### Input

```text
fileName = "test.db"
```

### Preconditions

* File is registered in `OpenFileManager` with `ReferenceCount == 1`.
* All sync, close, and unregister operations succeed.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Output

* Void return (Success).

### Expected State

* OS handle is closed.
* Registry slot is cleared.

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("test.db")
    -> returns openFileEntry

openFileEntry.DecrementRefCount()
    -> returns 0

FileSynchronizer.Sync(openFileEntry)
    -> succeeds

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds

OpenFileManager.UnregisterOpenFile("test.db")
    -> succeeds
```

### Suggested Test Name

```csharp
CloseFile_LastReferenceClosed_SyncsAndReleasesResources()
```

### Expected Call Order

```text
FileSynchronizer.Sync(openFileEntry)
    before
IPhysicalFileSystem.Close(fileHandle)
    before
OpenFileManager.UnregisterOpenFile("test.db")
```


---

## Case 3: File Not Opened Error

### Input

```text
fileName = "test.db"
```

### Preconditions

* File is not currently registered in the manager.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Output

```text
Throws FileNotOpenException
```

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("test.db")
    -> returns null
```

### Suggested Test Name

```csharp
CloseFile_FileNotOpen_ThrowsFileNotOpenException()
```

---

## Case 4: Prevent Negative Reference Count

### Preconditions

* File entry is already at `ReferenceCount == 0`.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Output

```text
Throws InvalidOperationException
```

### Expected Dependency Calls

```text
OpenFileManager.GetOpenFile("test.db")
    -> returns openFileEntry (with ReferenceCount == 0)

openFileEntry.DecrementRefCount()
    -> throws InvalidOperationException
```

### Forbidden Dependency Calls

```text
FileSynchronizer.Sync(...)
IPhysicalFileSystem.Close(...)
OpenFileManager.UnregisterOpenFile(...)
```

### Suggested Test Name

```csharp
CloseFile_CountAlreadyZero_ThrowsInvalidOperationException()
```

---

## Case 5: Sync Failure Recovery

### Preconditions

* File is registered with `ReferenceCount == 1`.
* Synchronization call throws `IOException`.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Output

```text
Throws FileCloseException
```

### Expected Exception

* Wraps the root `IOException`.

### Expected State

* File handle remains registered.
* OS handle is not closed.

### Expected Dependency Calls

```text
openFileEntry.DecrementRefCount()
    -> returns 0

FileSynchronizer.Sync(openFileEntry)
    -> throws IOException
```

### Forbidden Dependency Calls

```text
IPhysicalFileSystem.Close(...)
OpenFileManager.UnregisterOpenFile(...)
```

### Suggested Test Name

```csharp
CloseFile_SyncFails_RetainsRegistrationAndThrows()
```

---

## Case 6: Close Handle Failure Recovery

### Preconditions

* Sync succeeds, but `IPhysicalFileSystem.Close` throws `IOException`.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Output

```text
Throws FileCloseException
```

### Expected State

* File remains registered in the active collections.
* OS handle close failure exception is propagated.

### Expected Dependency Calls

```text
FileSynchronizer.Sync(openFileEntry)
    -> succeeds

IPhysicalFileSystem.Close(fileHandle)
    -> throws IOException
```

### Forbidden Dependency Calls

```text
OpenFileManager.UnregisterOpenFile(...)
```

### Suggested Test Name

```csharp
CloseFile_HandleCloseFails_RetainsRegistrationAndThrows()
```

---

## Case 7: Read-Only File Close

### Description

Verifies that if a file is open in `ReadOnly` access mode, the synchronizer sync call is skipped entirely.

### Preconditions

* File open in `ReadOnly` with `ReferenceCount == 1`.

### Execution

```csharp
CloseFile("test.db");
```

### Expected Dependency Calls

```text
openFileEntry.DecrementRefCount()
    -> returns 0

IPhysicalFileSystem.Close(fileHandle)
    -> succeeds

OpenFileManager.UnregisterOpenFile("test.db")
    -> succeeds
```

### Forbidden Dependency Calls

```text
FileSynchronizer.Sync(...)
```

### Suggested Test Name

```csharp
CloseFile_ReadOnlyFile_BypassesSync()
```

---

# Summary

| ID | Scenario                           | Expected Result                       |
| -: | ---------------------------------- | ------------------------------------- |
|  1 | Decrement refCount > 1             | RefCount decremented without close    |
|  2 | Close final reference (refCount=1) | Syncs, closes handles, unregisters    |
|  3 | File not open                      | Throws `FileNotOpenException`         |
|  4 | Prevent negative count             | Throws `InvalidOperationException`    |
|  5 | Sync fails                         | Throws, handle remains registered     |
|  6 | Close handle fails                 | Throws, handle remains registered     |
|  7 | Read-only file close               | Bypasses `Sync` call entirely         |

## Total

```text
7 independent unit test behaviors
```
