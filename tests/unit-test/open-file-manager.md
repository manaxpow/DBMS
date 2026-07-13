# Unit Test Spec - OpenFileManager

## Component
* **Class**: `OpenFileManager`
* **Interface**: `IOpenFileManager`

---

## Test Cases

### Case 1: Register New File (Happy Path)
* **Input**: `fileName = "test.db"`, `entry = openFileEntry`
* **Execution**: `RegisterOpenFile("test.db", openFileEntry)`
* **Expected State**: Internal registry maps "test.db" to the entry.

### Case 2: Get Registered File (Happy Path)
* **Input**: `fileName = "test.db"`
* **Preconditions**: "test.db" is registered.
* **Execution**: `GetOpenFile("test.db")`
* **Expected Output**: Returns the matching `OpenFileEntry` reference.

### Case 3: Unregister File (Happy Path)
* **Input**: `fileName = "test.db"`
* **Preconditions**: "test.db" is registered.
* **Execution**: `UnregisterOpenFile("test.db")`
* **Expected State**: Entry is removed. `GetOpenFile("test.db")` returns `null`.

### Case 4: Register Duplicate File Conflict
* **Input**: `fileName = "test.db"`, `entry = newOpenFileEntry`
* **Preconditions**: "test.db" is already registered.
* **Execution**: `RegisterOpenFile("test.db", newOpenFileEntry)`
* **Expected Output**: Throws `FileAlreadyOpenException`.

### Case 5: TryBeginDelete on Inactive File (Happy Path)
* **Input**: `fileName = "test.db"`
* **Preconditions**: "test.db" is not open.
* **Execution**: `TryBeginDelete("test.db")`
* **Expected Output**: Returns `true`.
* **Expected State**: Atomically transitions "test.db" registry slot to `Deleting` lock state.

### Case 6: TryBeginDelete Blocked by Active Entry
* **Input**: `fileName = "test.db"`
* **Preconditions**: "test.db" is currently open (registered).
* **Execution**: `TryBeginDelete("test.db")`
* **Expected Output**: Returns `false`.

### Case 7: CompleteDelete (Happy Path)
* **Preconditions**: Lock state is `Deleting`.
* **Execution**: `CompleteDelete("test.db")`
* **Expected State**: The deleting lock slot is cleared completely from registry.

### Case 8: CancelDelete (Happy Path)
* **Preconditions**: Lock state is `Deleting`.
* **Execution**: `CancelDelete("test.db")`
* **Expected State**: Deleting lock is removed. Registry slot is free.

### Case 9: Block Open File in Deleting State
* **Description**: Verifies that any `GetOpenFile` or `RegisterOpenFile` requests are rejected or return conflict blocks while a deletion lock is active.
* **Preconditions**: File "test.db" is locked in `Deleting` state.
* **Execution**: `OpenFile("test.db", ...)` or `RegisterOpenFile(...)`
* **Expected Output**: Rejects request (e.g. throws `FileInUseException` or returns null/blocked).

### Case 10: Concurrent Registrations Lock Conflict
* **Description**: Verifies that when two threads call `RegisterOpenFile` concurrently for the same filename, only one succeeds and the other throws `FileAlreadyOpenException`.
* **Execution**: Concurrent thread task execution.

### Case 11: Reference State Preserved
* **Description**: Verifies that registering/unregistering does not modify the properties (such as reference count, locks) within the `OpenFileEntry` itself.
