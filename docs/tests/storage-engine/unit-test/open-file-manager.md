# Unit Test Specification — `OpenFileManager`

## 1. Scope

This document covers unit test specifications for `OpenFileManager`.
Tests are isolated from all external dependencies.
Only public method behavior is verified.

## 2. Component

* **Class:** `OpenFileManager`
* **Methods:**

```csharp
void RegisterOpenFile(
    string fileName,
    OpenFileEntry entry);

OpenFileEntry GetOpenFile(
    string fileName);

void UnregisterOpenFile(
    string fileName);

bool TryBeginDelete(
    string fileName);

void CompleteDelete(
    string fileName);

void CancelDelete(
    string fileName);
```

## 3. Unit Under Test

`OpenFileManager` is responsible for:

* Storing active `OpenFileEntry` maps thread-safely.
* Preventing duplicate registrations of the same file (throwing `FileAlreadyOpenException`).
* Allowing lookups of open files by name.
* Managing thread-safe unregistrations.
* Synchronizing deletion locks using atomic markers (`TryBeginDelete`, `CompleteDelete`, `CancelDelete`).
* Rejecting openings or registrations when a file is locked in a `Deleting` state.

## 4. Dependencies (None)

None. `OpenFileManager` manages collections in memory and is tested directly.

---

# Test Cases

## Case 1: Register New File

### Input

```text
fileName = "test.db"
entry    = openFileEntry
```

### Preconditions

* "test.db" is not registered.

### Execution

```csharp
RegisterOpenFile("test.db", openFileEntry);
```

### Expected State

* The registry maps "test.db" to the entry.

### Suggested Test Name

```csharp
RegisterOpenFile_NewFile_RegistersSuccessfully()
```

---

## Case 2: Get Registered File

### Input

```text
fileName = "test.db"
```

### Preconditions

* "test.db" is registered.

### Execution

```csharp
GetOpenFile("test.db");
```

### Expected Output

* Returns the matching `OpenFileEntry` reference.

### Suggested Test Name

```csharp
GetOpenFile_FileRegistered_ReturnsEntry()
```

---

## Case 3: Unregister File

### Input

```text
fileName = "test.db"
```

### Preconditions

* "test.db" is registered.

### Execution

```csharp
UnregisterOpenFile("test.db");
```

### Expected State

* Entry is removed. Subsequent `GetOpenFile` calls return `null`.

### Suggested Test Name

```csharp
UnregisterOpenFile_FileRegistered_RemovesEntry()
```

---

## Case 4: Register Duplicate File Conflict

### Input

```text
fileName = "test.db"
entry    = newOpenFileEntry
```

### Preconditions

* "test.db" is already registered.

### Execution

```csharp
RegisterOpenFile("test.db", newOpenFileEntry);
```

### Expected Output

```text
Throws FileAlreadyOpenException
```

### Suggested Test Name

```csharp
RegisterOpenFile_DuplicateFile_ThrowsFileAlreadyOpenException()
```

---

## Case 5: TryBeginDelete on Inactive File

### Input

```text
fileName = "test.db"
```

### Preconditions

* "test.db" is not open.

### Execution

```csharp
TryBeginDelete("test.db");
```

### Expected Output

* Returns `true`.

### Expected State

* Registry slot transitions atomically into a `Deleting` lock state.

### Suggested Test Name

```csharp
TryBeginDelete_FileNotOpen_AcquiresLockAndReturnsTrue()
```

---

## Case 6: TryBeginDelete Blocked by Active Entry

### Input

```text
fileName = "test.db"
```

### Preconditions

* "test.db" is open and registered.

### Execution

```csharp
TryBeginDelete("test.db");
```

### Expected Output

* Returns `false`.

### Expected State

* File is not marked for deletion.

### Suggested Test Name

```csharp
TryBeginDelete_FileIsOpen_FailsAndReturnsFalse()
```

---

## Case 7: CompleteDelete

### Input

```text
fileName = "test.db"
```

### Preconditions

* Lock state is `Deleting`.

### Execution

```csharp
CompleteDelete("test.db");
```

### Expected State

* Lock marker is removed, and registry key is completely cleared.

### Suggested Test Name

```csharp
CompleteDelete_ValidLock_ClearsKey()
```

---

## Case 8: CancelDelete

### Input

```text
fileName = "test.db"
```

### Preconditions

* Lock state is `Deleting`.

### Execution

```csharp
CancelDelete("test.db");
```

### Expected State

* Lock marker is removed, returning slot to free/available state.

### Suggested Test Name

```csharp
CancelDelete_ValidLock_ReleasesLock()
```

---

## Case 9: Block Open in Deleting State

### Preconditions

* "test.db" is locked in a `Deleting` state.

### Execution

```csharp
RegisterOpenFile("test.db", entry);
```

### Expected Output

```text
Throws FileInUseException
```

### Suggested Test Name

```csharp
RegisterOpenFile_FileInDeletingState_ThrowsFileInUseException()
```

---

## Case 10: Concurrent Registrations Lock Conflict

### Description

Verifies that if two threads execute duplicate registration concurrently, only one succeeds and the other throws `FileAlreadyOpenException`.

### Preconditions

* `"test.db"` is not registered.
* Two threads are prepared to call `RegisterOpenFile("test.db", ...)` simultaneously.

### Execution

Two concurrent calls to `RegisterOpenFile("test.db", openFileEntry)` from separate threads.

### Expected Output

* One call returns void (success).
* The other call throws `FileAlreadyOpenException`.

### Expected State

* Registry maps `"test.db"` to exactly one entry.

### Suggested Test Name

```csharp
RegisterOpenFile_ConcurrentThreads_OnlyOneSucceeds()
```

---

## Case 11: Reference State Preserved

### Input

```text
fileName = "test.db"
entry    = openFileEntry (ReferenceCount = 1)
```

### Preconditions

* `"test.db"` is not registered.

### Execution

```csharp
RegisterOpenFile("test.db", openFileEntry);
```

### Expected Output

* Void return (success).

### Expected State

* `openFileEntry.ReferenceCount` remains `1` (unchanged by the registration operation).
* Registry maps `"test.db"` to `openFileEntry`.

### Suggested Test Name

```csharp
RegisterOpenFile_EntryReferenceState_RemainsUnchanged()
```

---

# Summary

| ID | Scenario                        | Expected Result                      |
| -: | ------------------------------- | ------------------------------------ |
|  1 | Register new file               | Entry is registered successfully     |
|  2 | Lookup registered file          | Returns active entry reference       |
|  3 | Unregister file                 | Entry removed from collection        |
|  4 | Register duplicate file         | Throws `FileAlreadyOpenException`    |
|  5 | TryBeginDelete inactive file    | Returns `true` and sets lock         |
|  6 | TryBeginDelete open file        | Returns `false`                      |
|  7 | CompleteDelete                  | Clears key from collections          |
|  8 | CancelDelete                    | Removes lock marker                  |
|  9 | Block opens during deletion lock| Rejects registration/throws          |
| 10 | Concurrent registrations conflict| Only one registration succeeds       |
| 11 | Reference state preservation    | Object state remains immutable       |

## Total

```text
11 independent unit test behaviors
```
