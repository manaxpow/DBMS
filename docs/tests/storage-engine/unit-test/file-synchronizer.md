# Unit Test Specification — `FileSynchronizer`

## 1. Scope

This document covers unit test specifications for `FileSynchronizer`.
Tests are isolated from all external dependencies using mocks.
Only public method behavior is verified.

## 2. Component

* **Class:** `FileSynchronizer`
* **Method:**

```csharp
void Sync(OpenFileEntry entry);
```

## 3. Unit Under Test

`FileSynchronizer.Sync` is responsible for:

* Fetching the active low-level OS file handle from the open file entry.
* Invoking the low-level handle physical flush (`FlushToDisk`).
* Escalating low-level filesystem sync errors by wrapping them into `FileSyncException`.
* Ensuring that no modifications are made to metadata objects or properties during flush.

## 4. Mocked Dependencies

* `IFileHandle`

---

# Test Cases

## Case 1: Successful File Sync

### Input

```text
entry = openFileEntry
```

### Preconditions

* Associated file handle is active and open.

### Execution

```csharp
Sync(openFileEntry);
```

### Expected Output

* Void return (Success).

### Expected Dependency Calls

```text
openFileEntry.Handle
    -> returns fileHandle

fileHandle.FlushToDisk()
    -> succeeds
```

### Suggested Test Name

```csharp
Sync_ValidHandle_CallsFlushToDisk()
```

---

## Case 2: Handle Already Closed Error

### Preconditions

* File handle associated with the entry has been closed.

### Execution

```csharp
Sync(openFileEntry);
```

### Expected Output

```text
Throws ObjectDisposedException
```

### Suggested Test Name

```csharp
Sync_HandleClosed_ThrowsObjectDisposedException()
```

---

## Case 3: OS Flush Fails

### Preconditions

* OS physical flush throws `IOException`.

### Execution

```csharp
Sync(openFileEntry);
```

### Expected Output

```text
Throws FileSyncException
```

### Expected Exception

* Wraps the original `IOException`.

### Expected Dependency Calls

```text
fileHandle.FlushToDisk()
    -> throws IOException
```

### Suggested Test Name

```csharp
Sync_OSFlushThrows_ThrowsFileSyncException()
```

---

## Case 4: Immutable Metadata

### Input

```text
entry = openFileEntry
```

### Preconditions

* Associated file handle is active and open.
* `openFileEntry.DataFile` has known metadata values.

### Execution

```csharp
Sync(openFileEntry);
```

### Expected State

* `openFileEntry.DataFile` metadata states are unmodified.

### Suggested Test Name

```csharp
Sync_MetadataUnmodified_PropertiesPreserved()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Valid sync call                 | Invokes low-level `FlushToDisk`      |
|  2 | Closed handle                   | Throws `ObjectDisposedException`     |
|  3 | OS flush exception              | Throws `FileSyncException`           |
|  4 | Metadata verification           | State remains completely immutable   |

## Total

```text
4 independent unit test behaviors
```
